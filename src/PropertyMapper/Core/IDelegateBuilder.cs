
namespace PropertyMapper.Core
{
    /// <summary>
    /// Strategy interface for generating IL delegates that perform mapping between two types.
    /// Allows specialized implementations for mutable classes, structs, and immutable records.
    /// </summary>
    internal interface IDelegateBuilder
    {
        /// <summary>
        /// Builds a compiled mapping delegate from <typeparamref name="TIn"/> to <typeparamref name="TOut"/>
        /// using the pre-computed <paramref name="plan"/>.
        /// </summary>
        /// <typeparam name="TIn">Source type.</typeparam>
        /// <typeparam name="TOut">Target type; must have a parameterless constructor.</typeparam>
        /// <param name="plan">The resolved mapping plan that describes which properties to copy.</param>
        /// <returns>A compiled <see cref="Func{TIn,TOut}"/> delegate ready for repeated invocation.</returns>
        Func<TIn, TOut> BuildDelegate<TIn, TOut>(MappingPlan plan) where TOut : new();
    }

    /// <summary>
    /// Factory that returns the shared <see cref="UnifiedDelegateBuilder"/> instance.
    /// </summary>
    internal static class DelegateBuilderFactory
    {
        /// <summary>Shared singleton <see cref="UnifiedDelegateBuilder"/> instance returned by <see cref="Create"/>.</summary>
        private static readonly UnifiedDelegateBuilder s_unifiedBuilder = new();

        /// <summary>Exposes the shared <see cref="UnifiedDelegateBuilder"/> for fallback use by other builders.</summary>
        internal static UnifiedDelegateBuilder UnifiedBuilder => s_unifiedBuilder;

        /// <summary>
        /// Returns the shared <see cref="UnifiedDelegateBuilder"/>.
        /// </summary>
        /// <returns>The shared <see cref="UnifiedDelegateBuilder"/>.</returns>
        public static IDelegateBuilder Create() => s_unifiedBuilder;
    }

}

