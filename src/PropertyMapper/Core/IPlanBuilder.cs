using PropertyMapper.Configuration;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Reflection;
using System.Runtime.InteropServices;

namespace PropertyMapper.Core
{
    /// <summary>
    /// Strategy interface for building mapping plans that describe how to copy properties
    /// between a source and target type.
    /// </summary>
    internal interface IPlanBuilder
    {
        /// <summary>
        /// Builds a <see cref="MappingPlan"/> for the given source/target type pair.
        /// </summary>
        /// <param name="source">The CLR type to map from.</param>
        /// <param name="target">The CLR type to map to.</param>
        /// <param name="visited">Set of already-visited type pairs used to detect circular references.</param>
        /// <returns>A fully resolved <see cref="MappingPlan"/> with all matching property bindings.</returns>
        MappingPlan BuildPlan(Type source, Type target, HashSet<TypePairKey> visited);

        /// <summary>
        /// Resolves the <see cref="ConversionKind"/> required to copy a value of <paramref name="source"/> type
        /// to a property of <paramref name="target"/> type.
        /// </summary>
        /// <param name="source">The source property type.</param>
        /// <param name="target">The target property type.</param>
        /// <returns>The <see cref="ConversionKind"/> that the IL emitter should use, or <see cref="ConversionKind.None"/> when no compatible conversion exists.</returns>
        ConversionKind ResolveConversion(Type source, Type target);
    }

    /// <summary>
    /// Default implementation of <see cref="IPlanBuilder"/> that uses reflection to match properties
    /// by name (ordinal, case-sensitive) and resolves the appropriate <see cref="ConversionKind"/>
    /// for each matched pair. Supports nested mappings, nullable conversions, and operator overloads.
    /// </summary>
    /// <remarks>
    /// Property names are deduplicated using an <see cref="ImmutableDictionary{TKey,TValue}"/> with
    /// copy-on-write semantics (swapped via <see cref="Interlocked.CompareExchange{T}"/>) to reduce
    /// string allocations and enable reference-equal comparisons without <see cref="string.Intern(string)"/>.
    /// After warmup the snapshot is promoted to a <see cref="FrozenDictionary{TKey,TValue}"/> for
    /// fastest read throughput on the hot path.
    /// </remarks>
    internal sealed class DefaultPlanBuilder : IPlanBuilder
    {
        /// <summary>
        /// Well-known "leaf" types that are treated as simple scalar values and never mapped as nested objects,
        /// even though they are reference or value types with their own properties.
        /// </summary>
        private static readonly FrozenSet<Type> LeafTypes = new[]
        {
        typeof(string), typeof(decimal), typeof(DateTime), typeof(DateTimeOffset),
        typeof(TimeSpan), typeof(TimeOnly), typeof(DateOnly), typeof(Guid), typeof(Uri)
    }.ToFrozenSet();

        /// <summary>
        /// Maximum number of entries in <see cref="s_nameMutable"/>.
        /// Prevents unbounded growth when dynamically generated types (e.g. EF Core proxies)
        /// produce many unique property names. After the limit is reached new names are returned
        /// as-is without caching — a minor allocation cost, not a functional failure.
        /// </summary>
        private const int NameCacheMaxSize = 4096;

        /// <summary>
        /// Mutable snapshot of the global name-deduplication cache.
        /// Replaced atomically via <see cref="Interlocked.CompareExchange{T}"/> on first encounter of each name.
        /// Bounded by <see cref="NameCacheMaxSize"/>; promoted to <see cref="s_nameFrozen"/> after warmup.
        /// </summary>
        private static ImmutableDictionary<string, string> s_nameMutable = ImmutableDictionary.Create<string, string>(StringComparer.Ordinal);

        /// <summary>
        /// Frozen snapshot of the name-deduplication cache, set once by <see cref="FreezeNameCache"/>.
        /// When non-<see langword="null"/>, all reads bypass the mutable dictionary for ~3–4× faster lookups.
        /// </summary>
        private static FrozenDictionary<string, string>? s_nameFrozen;

        /// <summary>The active configuration that governs depth limits and unmapped-property behavior.</summary>
        private readonly PropMapConfiguration _configuration;

        /// <summary>Initializes a <see cref="DefaultPlanBuilder"/> with default configuration.</summary>
        public DefaultPlanBuilder() : this(new PropMapConfiguration()) { }

        /// <summary>
        /// Initializes a <see cref="DefaultPlanBuilder"/> with the supplied configuration.
        /// </summary>
        /// <param name="configuration">Configuration controlling depth limits, unmapped-property handling, etc.</param>
        public DefaultPlanBuilder(PropMapConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Returns the deduplicated (canonical) instance of <paramref name="name"/> from the name cache,
        /// inserting it on first use. Once the cache reaches <see cref="NameCacheMaxSize"/> entries,
        /// new names are returned without caching to prevent unbounded memory growth.
        /// </summary>
        /// <param name="name">The property name to look up or add to the deduplication cache.</param>
        /// <returns>The canonical instance of <paramref name="name"/> stored in the cache, or <paramref name="name"/> itself when the cache is full.</returns>
        private static string InternName(string name)
        {
            // Fast path: frozen snapshot (post-warmup, proposal D)
            if (Volatile.Read(ref s_nameFrozen) is { } frozen)
                return frozen.TryGetValue(name, out string? f) ? f : name;

            // Mutable path: ImmutableDictionary + CAS loop (proposal E)
            while (true)
            {
                ImmutableDictionary<string, string> current = Volatile.Read(ref s_nameMutable);
                if (current.TryGetValue(name, out string? cached))
                    return cached;

                if (current.Count >= NameCacheMaxSize)
                    return name;

                ImmutableDictionary<string, string> updated = current.SetItem(name, name);
                if (Interlocked.CompareExchange(ref s_nameMutable, updated, current) == current)
                    return name;
                // CAS lost the race; retry — next iteration finds the key or still sees Count < max.
            }
        }

        /// <summary>
        /// Promotes the name-deduplication cache to a <see cref="FrozenDictionary{TKey,TValue}"/> for
        /// fastest read throughput. Call once after all known type pairs are warmed up.
        /// Idempotent — subsequent calls have no effect.
        /// </summary>
        internal static void FreezeNameCache()
        {
            if (Volatile.Read(ref s_nameFrozen) is not null)
                return;
            FrozenDictionary<string, string> candidate = Volatile.Read(ref s_nameMutable).ToFrozenDictionary(StringComparer.Ordinal);
            Interlocked.CompareExchange(ref s_nameFrozen, candidate, null);
        }

        /// <inheritdoc/>
        public MappingPlan BuildPlan(Type source, Type target, HashSet<TypePairKey> visited) => BuildPlanCore(source, target, visited, 0);

        /// <summary>
        /// Recursive plan-building implementation that tracks the current nesting depth and
        /// guards against circular references and excessive recursion.
        /// </summary>
        /// <param name="source">The CLR type to map from.</param>
        /// <param name="target">The CLR type to map to.</param>
        /// <param name="visited">Set of type pairs already in the current recursion stack.</param>
        /// <param name="depth">Current nesting depth; compared against <see cref="PropMapConfiguration.MaxMappingDepth"/>.</param>
        /// <returns>A resolved <see cref="MappingPlan"/> for the given type pair at this depth.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when <paramref name="depth"/> exceeds <see cref="PropMapConfiguration.MaxMappingDepth"/>
        /// or when unmapped source properties are detected and <see cref="PropMapConfiguration.ThrowOnUnmappedProperties"/> is enabled.
        /// </exception>
        private MappingPlan BuildPlanCore(Type source, Type target, HashSet<TypePairKey> visited, int depth)
        {
            if (depth > _configuration.MaxMappingDepth)
                throw new InvalidOperationException(
                    $"Maximum mapping depth of {_configuration.MaxMappingDepth} exceeded. " +
                    $"Possible circular reference between '{source.Name}' and '{target.Name}'.");

            TypePairKey currentPair = new TypePairKey(source, target);

            if (visited.Contains(currentPair))
                return new MappingPlan(source, target, []);

            visited.Add(currentPair);

            PropertyInfo[] sourceProps = TypeMetadataCache.GetProperties(source);
            PropertyInfo[] targetProps = TypeMetadataCache.GetProperties(target);

            // .NET 10 optimization: Use interned names for faster equality comparison
            Dictionary<string, PropertyInfo> targetMap = new Dictionary<string, PropertyInfo>(targetProps.Length, StringComparer.Ordinal);
            foreach (PropertyInfo tp in targetProps)
            {
                if (tp.CanWrite)
                {
                    string internedName = InternName(tp.Name);
                    targetMap[internedName] = tp;
                }
            }

            IReadOnlySet<string>? globalIgnore = _configuration.GlobalIgnoredProperties;

            List<PropertyBinding> bindings = new List<PropertyBinding>();
            foreach (PropertyInfo sp in sourceProps)
            {
                if (!sp.CanRead)
                    continue;

                string internedSourceName = InternName(sp.Name);

                if (globalIgnore is not null && globalIgnore.Contains(internedSourceName))
                    continue;

                if (!targetMap.TryGetValue(internedSourceName, out PropertyInfo? tp))
                    continue;

                ConversionKind conv = ResolveConversion(sp.PropertyType, tp.PropertyType);
                if (conv == ConversionKind.None)
                    continue;

                MappingPlan? nested = null;
                if (conv == ConversionKind.Nested)
                    nested = BuildPlanCore(sp.PropertyType, tp.PropertyType, visited, depth + 1);

                bindings.Add(new PropertyBinding(sp, tp, conv, nested));
            }

            if (_configuration.ThrowOnUnmappedProperties)
            {
                HashSet<PropertyInfo> mappedSources = new(bindings.Count);
                foreach (PropertyBinding b in bindings)
                    mappedSources.Add(b.Source);

                List<string> unmapped = new();
                foreach (PropertyInfo sp in sourceProps)
                {
                    if (sp.CanRead && !mappedSources.Contains(sp))
                        unmapped.Add(sp.Name);
                }

                if (unmapped.Count > 0)
                    throw new InvalidOperationException($"Unmapped source properties on '{source.Name}': {string.Join(", ", unmapped)}.");
            }

            visited.Remove(currentPair);

            // CollectionsMarshal.AsSpan exposes the List's internal buffer directly —
            // MappingPlan copies it into the inline array without a ToArray() intermediate allocation.
            return new MappingPlan(source, target, CollectionsMarshal.AsSpan(bindings));
        }

        /// <inheritdoc/>
        public ConversionKind ResolveConversion(Type source, Type target)
        {
            if (source == target) return ConversionKind.Direct;

            Type? sNull = Nullable.GetUnderlyingType(source);
            Type? tNull = Nullable.GetUnderlyingType(target);

            if (sNull is not null && tNull is not null && sNull == tNull) return ConversionKind.NullableToNullable;
            if (sNull is not null && sNull == target) return ConversionKind.NullableToValue;
            if (tNull is not null && tNull == source) return ConversionKind.ValueToNullable;

            if (OperatorDiscovery.Find(source, target) is not null) return ConversionKind.Operator;

            if (sNull is not null || tNull is not null)
                return ConversionKind.None;

            if (CanMapNested(source, target)) return ConversionKind.Nested;
            return ConversionKind.None;
        }

        /// <summary>
        /// Returns <see langword="true"/> when both <paramref name="s"/> and <paramref name="t"/> are
        /// complex object types that can be recursively mapped (i.e. neither is a primitive, an enum, nor a known leaf type).
        /// </summary>
        /// <param name="s">The source property type.</param>
        /// <param name="t">The target property type.</param>
        private static bool CanMapNested(Type s, Type t) => !s.IsPrimitive && !t.IsPrimitive && !s.IsEnum && !t.IsEnum && !LeafTypes.Contains(s) && !LeafTypes.Contains(t);

    }

}

