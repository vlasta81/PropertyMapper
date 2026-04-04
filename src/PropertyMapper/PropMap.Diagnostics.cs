using System.Reflection;
using PropertyMapper.Configuration;
using PropertyMapper.Core;

namespace PropertyMapper
{
    public sealed partial class PropMap
    {
        /// <summary>Open generic <see cref="MethodInfo"/> for <see cref="BuildAndCachePair{TIn,TOut}"/>; used to create per-pair delegates via <c>MakeGenericMethod</c> + <c>CreateDelegate</c>.</summary>
        private static readonly MethodInfo s_buildAndCachePairDef = typeof(PropMap).GetMethod(nameof(BuildAndCachePair), BindingFlags.NonPublic | BindingFlags.Instance)!;

        /// <summary>Cache of pre-built <see cref="BuildAndCachePair{TIn,TOut}"/> delegates, keyed by (TIn, TOut); avoids repeated <c>MakeGenericMethod</c> + <c>CreateDelegate</c> allocations on every <see cref="WarmupBatch(Type[])"/> call.</summary>
        private readonly Dictionary<(Type, Type), Action<TypePairKey>> _warmupDelegateCache = [];

        #region Warmup & Statistics

        /// <summary>
        /// Pre-compiles the mapping delegate for the <typeparamref name="TIn"/>→<typeparamref name="TOut"/>
        /// type pair and caches it, eliminating first-call JIT and IL-emit overhead.
        /// </summary>
        /// <remarks>
        /// Call during application startup for each type pair used on hot request paths.
        /// For multiple pairs, prefer <see cref="WarmupBatch"/> which processes them all
        /// in a single sequential pass and is more efficient than repeated <see cref="Warmup{TIn,TOut}"/> calls.
        /// </remarks>
        /// <typeparam name="TIn">Source type.</typeparam>
        /// <typeparam name="TOut">Target type (must have a parameterless constructor).</typeparam>
        public void Warmup<TIn, TOut>() where TOut : new() => GetOrCompileMapper<TIn, TOut>();

        /// <summary>
        /// Pre-compiles mapping delegates for multiple type pairs concurrently.
        /// Useful at application startup to eliminate first-call overhead for all known type pairs at once.
        /// </summary>
        /// <param name="typePairs">
        /// An even-length array of types in alternating source/target order.
        /// Example: <c>typeof(Foo), typeof(FooDto), typeof(Bar), typeof(BarDto)</c>.
        /// </param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="typePairs"/> has an odd number of elements.</exception>
        public void WarmupBatch(params Type[] typePairs)
        {
            if (typePairs.Length % 2 != 0)
                throw new ArgumentException("Type pairs must have even count", nameof(typePairs));

            for (int i = 0; i < typePairs.Length / 2; i++)
            {
                Type tIn = typePairs[i * 2];
                Type tOut = typePairs[i * 2 + 1];

                TypePairKey key = new TypePairKey(tIn, tOut);
                if (_cache.TryGetPlan(key, out _))
                    continue;

                lock (_compileLock)
                {
                    if (!_cache.TryGetPlan(key, out _))
                    {
                        if (!_warmupDelegateCache.TryGetValue((tIn, tOut), out Action<TypePairKey>? warmupAction))
                        {
                            warmupAction = (Action<TypePairKey>)
                                s_buildAndCachePairDef.MakeGenericMethod(tIn, tOut)
                                    .CreateDelegate(typeof(Action<TypePairKey>), this);
                            _warmupDelegateCache[(tIn, tOut)] = warmupAction;
                        }

                        warmupAction(key);
                    }
                }
            }

            DefaultPlanBuilder.FreezeNameCache();
            TypeMetadataCache.Freeze();
        }

        /// <summary>
        /// Returns a point-in-time snapshot of this mapper's cache utilisation.
        /// </summary>
        /// <returns>A <see cref="MappingStatistics"/> value with counts of cached delegates, plans, and a memory estimate.</returns>
        public MappingStatistics GetStatistics() => _cache.GetStatistics();

        /// <summary>
        /// Compiles the <typeparamref name="TIn"/>→<typeparamref name="TOut"/> mapping delegate,
        /// optionally wraps it with per-pair custom mappings, and stores both the plan and delegate in the cache.
        /// </summary>
        /// <typeparam name="TIn">Source type.</typeparam>
        /// <typeparam name="TOut">Target type (must have a parameterless constructor).</typeparam>
        /// <param name="key">The type-pair cache key for this mapping.</param>
        private void BuildAndCachePair<TIn, TOut>(TypePairKey key) where TOut : new()
        {
            (MappingPlan plan, TypePairConfiguration<TIn, TOut>? config) = GetOrBuildPlan<TIn, TOut>(key);
            Func<TIn, TOut> compiled = DelegateBuilderFactory.Create().BuildDelegate<TIn, TOut>(plan);
            if (config?.HasCustomMappings == true)
                compiled = config.WrapWithCustomMappings(compiled);
            _cache.AddPlanAndDelegate(key, plan, compiled);
        }

        #endregion

        #region Validation

        /// <summary>
        /// Validates the <typeparamref name="TIn"/>→<typeparamref name="TOut"/> mapping without executing it.
        /// Builds the mapping plan and reports which target properties have no matching source property.
        /// </summary>
        /// <remarks>
        /// No IL is emitted and no delegate is cached. Use at application startup or in unit tests
        /// to catch misconfigured mappings early.
        /// </remarks>
        /// <typeparam name="TIn">Source type.</typeparam>
        /// <typeparam name="TOut">Target type (must have a parameterless constructor).</typeparam>
        /// <returns>A <see cref="MappingValidationResult"/> describing any unmapped target properties.</returns>
        public MappingValidationResult Validate<TIn, TOut>() where TOut : new()
        {
            lock (_compileLock)
            {
                TypePairKey key = new TypePairKey(typeof(TIn), typeof(TOut));

                (MappingPlan plan, TypePairConfiguration<TIn, TOut>? _) = GetOrBuildPlan<TIn, TOut>(key);

                // Collect all writable target properties.
                PropertyInfo[] targetProps = TypeMetadata<TOut>.Properties;

                HashSet<string> mapped = new(plan.Bindings.Length, StringComparer.Ordinal);
                foreach (PropertyBinding b in plan.Bindings)
                    mapped.Add(b.Target.Name);

                List<string> unmapped = [];
                foreach (PropertyInfo p in targetProps)
                {
                    if (p.CanWrite && !mapped.Contains(p.Name))
                        unmapped.Add(p.Name);
                }

                return new MappingValidationResult(unmapped.Count == 0, [.. unmapped]);
            }
        }

        #endregion
    }
}
