
namespace PropertyMapper.Core
{
    /// <summary>
    /// Specifies the kind of type conversion required when mapping a source property to a target property.
    /// </summary>
    internal enum ConversionKind
    {
        /// <summary>No compatible conversion exists; the property pair will be skipped.</summary>
        None,
        /// <summary>Types are identical; the value is copied directly without any conversion.</summary>
        Direct,
        /// <summary>Source is <c>Nullable&lt;T&gt;</c> and target is <c>T</c>; uses <c>GetValueOrDefault()</c> semantics.</summary>
        NullableToValue,
        /// <summary>Source is <c>T</c> and target is <c>Nullable&lt;T&gt;</c>; wraps the value via <c>new Nullable&lt;T&gt;(value)</c>.</summary>
        ValueToNullable,
        /// <summary>Both source and target are <c>Nullable&lt;T&gt;</c> of the same underlying type.</summary>
        NullableToNullable,
        /// <summary>Types differ but a user-defined implicit or explicit conversion operator exists on one of the types.</summary>
        Operator,
        /// <summary>Target property is a complex object type; a nested <see cref="MappingPlan"/> is used to map its properties recursively.</summary>
        Nested
    }

    // TypePair removed - now using optimized TypePairKey with nint handles (see TypePairKey.cs)

    /// <summary>
    /// Describes how to map all matching properties between a source and target type.
    /// </summary>
    /// <remarks>
    /// .NET 10 Optimization: Uses <see cref="InlineBindings"/> (an <see cref="System.Runtime.CompilerServices.InlineArrayAttribute"/> buffer)
    /// for ≤16 bindings to avoid heap allocation. For types with more than 16 properties a regular array is allocated instead.
    /// </remarks>
    internal sealed class MappingPlan
    {
        /// <summary>Inline (stack-allocated) storage for ≤16 property bindings.</summary>
        private InlineBindings _inlineBindings;
        /// <summary>Heap-allocated fallback storage used when <see cref="BindingCount"/> exceeds 16.</summary>
        private PropertyBinding[]? _heapBindings;

        /// <summary>Gets the CLR type being mapped from.</summary>
        public Type SourceType { get; }
        /// <summary>Gets the CLR type being mapped to.</summary>
        public Type TargetType { get; }
        /// <summary>Gets the total number of resolved property bindings in this plan.</summary>
        public int BindingCount { get; }

        /// <summary>
        /// Initializes a new <see cref="MappingPlan"/> with the given source/target types and property bindings.
        /// </summary>
        /// <param name="sourceType">The CLR type being mapped from.</param>
        /// <param name="targetType">The CLR type being mapped to.</param>
        /// <param name="bindings">Array of resolved property bindings to store.</param>
        public MappingPlan(Type sourceType, Type targetType, PropertyBinding[] bindings) : this(sourceType, targetType, bindings.AsSpan())
        {
        }

        /// <summary>
        /// Initializes a new <see cref="MappingPlan"/> from a span of bindings.
        /// Avoids an intermediate <c>PropertyBinding[]</c> allocation for the common ≤16-property case:
        /// the span is copied directly into the inline buffer without first materialising a heap array.
        /// For types with more than 16 properties the span is copied to a heap array exactly once.
        /// </summary>
        /// <param name="sourceType">The CLR type being mapped from.</param>
        /// <param name="targetType">The CLR type being mapped to.</param>
        /// <param name="bindings">Span of resolved property bindings to store.</param>
        public MappingPlan(Type sourceType, Type targetType, ReadOnlySpan<PropertyBinding> bindings)
        {
            SourceType = sourceType;
            TargetType = targetType;
            BindingCount = bindings.Length;

            // .NET 10 optimization: Use inline array for small objects
            if (bindings.Length <= 16)
            {
                // Copy directly from span — no intermediate heap array for the common case.
                for (int i = 0; i < bindings.Length; i++)
                {
                    _inlineBindings[i] = bindings[i];
                }
                _heapBindings = null;
            }
            else
            {
                // Heap allocation for large objects — ToArray() materialises the span once.
                _heapBindings = bindings.ToArray();
            }
        }

        /// <summary>
        /// Gets the property bindings as a <see cref="ReadOnlySpan{T}"/> for zero-allocation iteration.
        /// Returns a span over the inline buffer or the heap array depending on <see cref="BindingCount"/>.
        /// </summary>
        public ReadOnlySpan<PropertyBinding> Bindings => BindingCount <= 16 ? _inlineBindings.AsSpan(0, BindingCount) : _heapBindings.AsSpan();
    }

    /// <summary>
    /// Describes the mapping between a single source property and its corresponding target property,
    /// including the conversion kind and any nested mapping plan.
    /// </summary>
    internal sealed record PropertyBinding
    {
        /// <summary>
        /// Initializes a new <see cref="PropertyBinding"/>.
        /// </summary>
        /// <param name="Source">Reflection metadata for the source property.</param>
        /// <param name="Target">Reflection metadata for the target property.</param>
        /// <param name="Conversion">The kind of type conversion required for this binding.</param>
        /// <param name="NestedPlan">
        /// The nested mapping plan; populated only when <paramref name="Conversion"/> is
        /// <see cref="ConversionKind.Nested"/>.
        /// </param>
        public PropertyBinding(System.Reflection.PropertyInfo Source, System.Reflection.PropertyInfo Target, ConversionKind Conversion, MappingPlan? NestedPlan = null)
        {
            this.Source = Source;
            this.Target = Target;
            this.Conversion = Conversion;
            this.NestedPlan = NestedPlan;
        }

        /// <summary>Gets the reflection metadata for the source property.</summary>
        public System.Reflection.PropertyInfo Source { get; init; }
        /// <summary>Gets the reflection metadata for the target property.</summary>
        public System.Reflection.PropertyInfo Target { get; init; }
        /// <summary>Gets the kind of type conversion to apply when emitting IL for this binding.</summary>
        public ConversionKind Conversion { get; init; }
        /// <summary>
        /// Gets the nested mapping plan used when <see cref="Conversion"/> is <see cref="ConversionKind.Nested"/>;
        /// <see langword="null"/> for all other conversion kinds.
        /// </summary>
        public MappingPlan? NestedPlan { get; init; }

    }

}

