using System.Collections.Frozen;
using System.Runtime.CompilerServices;

namespace PropertyMapper.Core
{
    /// <summary>
    /// Thread-safe lock-free cache for mapping plans and compiled delegates.
    /// Uses copy-on-write pattern with volatile fields for safe concurrent access.
    /// </summary>
    /// <remarks>
    /// .NET 10 optimizations applied:
    /// <list type="bullet">
    /// <item><see cref="System.Collections.Frozen.FrozenDictionary{TKey,TValue}"/> with <see cref="TypePairKey"/> (<c>nint</c> handles) for zero-allocation hot-path lookups.</item>
    /// <item><see cref="System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining"/> on all read methods to minimise call overhead.</item>
    /// </list>
    /// </remarks>
    internal sealed class MappingCache
    {
        /// <summary>Immutable snapshot of all cached mapping plans, replaced atomically on write.</summary>
        private volatile FrozenDictionary<TypePairKey, MappingPlan> _plans = FrozenDictionary<TypePairKey, MappingPlan>.Empty;
        /// <summary>Immutable snapshot of all compiled mapping delegates (<see cref="Func{TIn,TOut}"/>), replaced atomically on write.</summary>
        private volatile FrozenDictionary<TypePairKey, object> _delegates = FrozenDictionary<TypePairKey, object>.Empty;
        /// <summary>Immutable snapshot of all compiled in-place delegates (<see cref="Action{TIn,TOut}"/>), replaced atomically on write.</summary>
        private volatile FrozenDictionary<TypePairKey, object> _intoDelegates = FrozenDictionary<TypePairKey, object>.Empty;

        /// <summary>
        /// Tries to retrieve the compiled mapping delegate for the given type pair.
        /// Lock-free hot path backed by a <see cref="FrozenDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <typeparam name="TIn">Source type.</typeparam>
        /// <typeparam name="TOut">Target type.</typeparam>
        /// <param name="mapper">The compiled delegate when found; <see langword="null"/> otherwise.</param>
        /// <returns><see langword="true"/> if a delegate was found in the cache.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetDelegate<TIn, TOut>(out Func<TIn, TOut>? mapper)
        {
            TypePairKey key = new TypePairKey(typeof(TIn), typeof(TOut));

            // .NET 10 optimization: FrozenDictionary already optimized for lookups
            if (_delegates.TryGetValue(key, out object? cached))
            {
                mapper = Unsafe.As<Func<TIn, TOut>>(cached);
                return true;
            }

            mapper = null;
            return false;
        }

        /// <summary>
        /// Tries to retrieve the compiled in-place mapping delegate for the given type pair.
        /// Lock-free hot path backed by a <see cref="FrozenDictionary{TKey,TValue}"/>.
        /// </summary>
        /// <typeparam name="TIn">Source type.</typeparam>
        /// <typeparam name="TOut">Target type.</typeparam>
        /// <param name="merger">The compiled in-place delegate when found; <see langword="null"/> otherwise.</param>
        /// <returns><see langword="true"/> if a delegate was found in the cache.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetIntoDelegate<TIn, TOut>(out Action<TIn, TOut>? merger)
        {
            TypePairKey key = new TypePairKey(typeof(TIn), typeof(TOut));

            if (_intoDelegates.TryGetValue(key, out object? cached))
            {
                merger = Unsafe.As<Action<TIn, TOut>>(cached);
                return true;
            }

            merger = null;
            return false;
        }

        /// <summary>
        /// Tries to retrieve the mapping plan for the given type-pair key.
        /// Lock-free hot path.
        /// </summary>
        /// <param name="key">The type-pair key to look up.</param>
        /// <param name="plan">The cached plan when found; <see langword="null"/> otherwise.</param>
        /// <returns><see langword="true"/> if a plan was found in the cache.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetPlan(TypePairKey key, out MappingPlan? plan)
        {
            return _plans.TryGetValue(key, out plan);
        }

        /// <summary>
        /// Atomically adds a mapping plan and its compiled delegate to the cache using copy-on-write.
        /// Must be called under the caller's write lock.
        /// </summary>
        /// <typeparam name="TIn">Source type.</typeparam>
        /// <typeparam name="TOut">Target type.</typeparam>
        /// <param name="key">The type-pair key for the new entry.</param>
        /// <param name="plan">The mapping plan to cache.</param>
        /// <param name="mapper">The compiled delegate to cache.</param>
        public void AddPlanAndDelegate<TIn, TOut>(TypePairKey key, MappingPlan plan, Func<TIn, TOut> mapper)
        {
            // Copy-on-write: create new immutable snapshots
            Dictionary<TypePairKey, MappingPlan> newPlans = _plans.ToDictionary();
            newPlans[key] = plan;
            _plans = newPlans.ToFrozenDictionary();

            Dictionary<TypePairKey, object> newDelegates = _delegates.ToDictionary();
            newDelegates[key] = mapper;
            _delegates = newDelegates.ToFrozenDictionary(); // Atomic publish
        }

        /// <summary>
        /// Atomically adds an in-place mapping delegate to the cache using copy-on-write.
        /// Also caches the <paramref name="plan"/> if it is not already present, so that a subsequent
        /// <c>Map&lt;TIn,TOut&gt;</c> call can reuse it without rebuilding.
        /// Must be called under the caller's write lock.
        /// </summary>
        /// <typeparam name="TIn">Source type.</typeparam>
        /// <typeparam name="TOut">Target type.</typeparam>
        /// <param name="key">The type-pair key for the new entry.</param>
        /// <param name="plan">The mapping plan to cache when not already present.</param>
        /// <param name="merger">The compiled in-place delegate to cache.</param>
        public void AddIntoDelegate<TIn, TOut>(TypePairKey key, MappingPlan plan, Action<TIn, TOut> merger)
        {
            // Cache plan only if not already present (avoids redundant rebuild when Map<> is called later).
            if (!_plans.ContainsKey(key))
            {
                Dictionary<TypePairKey, MappingPlan> newPlans = _plans.ToDictionary();
                newPlans[key] = plan;
                _plans = newPlans.ToFrozenDictionary();
            }

            Dictionary<TypePairKey, object> newInto = _intoDelegates.ToDictionary();
            newInto[key] = merger;
            _intoDelegates = newInto.ToFrozenDictionary();
        }

        /// <summary>
        /// Removes all cached entries (plan, mapping delegate, and in-place delegate) for the given type-pair key.
        /// Uses the same copy-on-write pattern as the Add methods.
        /// Called by <see cref="PropMap.Configure{TIn,TOut}()"/> to invalidate stale compiled delegates.
        /// </summary>
        /// <param name="key">The type-pair key whose entries should be evicted.</param>
        public void Remove(TypePairKey key)
        {
            if (_plans.ContainsKey(key))
            {
                Dictionary<TypePairKey, MappingPlan> newPlans = _plans.ToDictionary();
                newPlans.Remove(key);
                _plans = newPlans.ToFrozenDictionary();
            }

            if (_delegates.ContainsKey(key))
            {
                Dictionary<TypePairKey, object> newDelegates = _delegates.ToDictionary();
                newDelegates.Remove(key);
                _delegates = newDelegates.ToFrozenDictionary();
            }

            if (_intoDelegates.ContainsKey(key))
            {
                Dictionary<TypePairKey, object> newInto = _intoDelegates.ToDictionary();
                newInto.Remove(key);
                _intoDelegates = newInto.ToFrozenDictionary();
            }
        }

        /// <summary>
        /// Removes all cached mapping plans and compiled delegates.
        /// </summary>
        public void Clear()
        {
            _plans = FrozenDictionary<TypePairKey, MappingPlan>.Empty;
            _delegates = FrozenDictionary<TypePairKey, object>.Empty;
            _intoDelegates = FrozenDictionary<TypePairKey, object>.Empty;
        }

        /// <summary>
        /// Returns a snapshot of cache utilisation.
        /// Must be called under the caller's write lock for a consistent read across the three dictionaries.
        /// </summary>
        /// <returns>
        /// A <see cref="MappingStatistics"/> value containing the number of cached mapping delegates,
        /// in-place delegates, mapping plans, and a rough memory estimate in bytes.
        /// </returns>
        public MappingStatistics GetStatistics()
        {
            FrozenDictionary<TypePairKey, MappingPlan> plans = _plans;
            FrozenDictionary<TypePairKey, object> delegates = _delegates;
            FrozenDictionary<TypePairKey, object> intoDelegates = _intoDelegates;

            // Rough per-entry estimates: MappingPlan ~128 B (bindings array + metadata),
            // compiled delegate ~64 B (Func/Action wrapper + IL method stub reference).
            long memory = (plans.Count * 128L) + ((delegates.Count + intoDelegates.Count) * 64L);

            return new MappingStatistics(
                CachedMappers: delegates.Count + intoDelegates.Count,
                CachedPlans: plans.Count,
                TotalMemoryBytes: memory
            );
        }
    }

}

