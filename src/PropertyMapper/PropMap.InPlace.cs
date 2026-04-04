using System.Runtime.CompilerServices;
using PropertyMapper.Configuration;
using PropertyMapper.Core;

namespace PropertyMapper
{
    public sealed partial class PropMap
    {
        #region In-Place Mapping

        /// <summary>
        /// Maps matching properties from <paramref name="source"/> onto an existing <paramref name="target"/> instance.
        /// Only properties whose names match are updated; unmatched properties on the target are left unchanged.
        /// </summary>
        /// <typeparam name="TIn">Source type.</typeparam>
        /// <typeparam name="TOut">Target type (must be a reference type).</typeparam>
        /// <param name="source">Source object to map from.</param>
        /// <param name="target">Existing target object to update in place.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="source"/> or <paramref name="target"/> is null.</exception>
        public void MapInto<TIn, TOut>(TIn source, TOut target) where TOut : class
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(target);

            GetOrCompileIntoMapper<TIn, TOut>()(source, target);
        }

        /// <summary>
        /// Creates a new <typeparamref name="TOut"/> from <paramref name="source1"/>, then merges matching
        /// properties from <paramref name="source2"/> onto it.
        /// When both sources expose a property with the same name, <paramref name="source2"/> wins.
        /// </summary>
        /// <typeparam name="TIn1">First (base) source type.</typeparam>
        /// <typeparam name="TIn2">Second (override) source type.</typeparam>
        /// <typeparam name="TOut">Target type (must be a reference type with a parameterless constructor).</typeparam>
        /// <param name="source1">Base source; provides the initial mapping.</param>
        /// <param name="source2">Override source; its matching properties overwrite those already set from <paramref name="source1"/>.</param>
        /// <returns>A new <typeparamref name="TOut"/> with properties merged from both sources.</returns>
        /// <exception cref="ArgumentNullException">When either source is null.</exception>
        public TOut MapMerge<TIn1, TIn2, TOut>(TIn1 source1, TIn2 source2) where TOut : class, new()
        {
            ArgumentNullException.ThrowIfNull(source1);
            ArgumentNullException.ThrowIfNull(source2);

            TOut result = Map<TIn1, TOut>(source1);
            GetOrCompileIntoMapper<TIn2, TOut>()(source2, result);
            return result;
        }

        /// <summary>
        /// Returns the cached in-place mapping delegate for <typeparamref name="TIn"/>→<typeparamref name="TOut"/>,
        /// compiling and caching it on first access (double-checked locking).
        /// Reuses the existing <see cref="MappingPlan"/> when <c>Map&lt;TIn,TOut&gt;</c> has already been called.
        /// </summary>
        private Action<TIn, TOut> GetOrCompileIntoMapper<TIn, TOut>() where TOut : class
        {
            if (_cache.TryGetIntoDelegate<TIn, TOut>(out Action<TIn, TOut>? merger))
                return merger!;

            lock (_compileLock)
            {
                if (_cache.TryGetIntoDelegate<TIn, TOut>(out merger))
                    return merger!;

                TypePairKey key = new TypePairKey(typeof(TIn), typeof(TOut));

                (MappingPlan plan, TypePairConfiguration<TIn, TOut>? config) = GetOrBuildPlan<TIn, TOut>(key);

                merger = s_inPlaceBuilder.BuildDelegate<TIn, TOut>(plan);

                if (config?.HasCustomMappings == true)
                    merger = config.WrapIntoWithCustomMappings(merger);

                _cache.AddIntoDelegate(key, plan, merger);
                return merger;
            }
        }

        #endregion

        #region Cloning

        /// <summary>
        /// Creates a shallow property-copy clone of <paramref name="source"/>.
        /// Returns a new <typeparamref name="T"/> instance with all matching properties copied.
        /// Nested reference-type properties are shared (shallow clone); nested value-type properties are copied by value.
        /// </summary>
        /// <typeparam name="T">Object type to clone (must have a parameterless constructor).</typeparam>
        /// <param name="source">The object to clone.</param>
        /// <returns>A new <typeparamref name="T"/> with properties copied from <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="source"/> is null.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Clone<T>(T source) where T : class, new()
        {
            ArgumentNullException.ThrowIfNull(source);
            return Map<T, T>(source);
        }

        #endregion
    }
}
