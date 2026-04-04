using System.Runtime.CompilerServices;
using PropertyMapper.Configuration;
using PropertyMapper.Core;

namespace PropertyMapper
{
    public sealed partial class PropMap
    {
        #region Core Mapping

        /// <summary>
        /// Maps <paramref name="source"/> to a new <typeparamref name="TOut"/> instance by copying all
        /// matched properties. Uses a lock-free cache on the hot path; compiles the delegate on first use.
        /// </summary>
        /// <typeparam name="TIn">Source type.</typeparam>
        /// <typeparam name="TOut">Target type (must have a parameterless constructor).</typeparam>
        /// <param name="source">Source object to map from.</param>
        /// <returns>A new <typeparamref name="TOut"/> with all matched properties copied from <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="source"/> is <see langword="null"/>.</exception>
        public TOut Map<TIn, TOut>(TIn source) where TOut : new()
        {
            ArgumentNullException.ThrowIfNull(source);

            // Fast-path for identical blittable types (zero-cost cast)
            if (typeof(TIn) == typeof(TOut) && typeof(TIn).IsValueType && !RuntimeHelpers.IsReferenceOrContainsReferences<TIn>())
            {
                return Unsafe.As<TIn, TOut>(ref source);
            }

            // Hot path: lock-free delegate lookup
            if (_cache.TryGetDelegate<TIn, TOut>(out Func<TIn, TOut>? mapper))
                return mapper!(source);

            // Cold path: compile new mapper (only happens once per type pair)
            return CompileAndExecute<TIn, TOut>(source);
        }

        /// <summary>
        /// Removes all cached mapping plans, compiled mapping delegates, and projection expression trees.
        /// The next call to any mapping method will recompile from scratch.
        /// </summary>
        /// <remarks>
        /// This method acquires the compile lock, so it is safe to call concurrently with mapping operations.
        /// Any <see cref="Configure{TIn,TOut}()"/> registrations are preserved; only the compiled artefacts
        /// are discarded.
        /// </remarks>
        public void Clear()
        {
            lock (_compileLock)
            {
                _cache.Clear();
                _projectionCache.Clear();
            }
        }

        /// <summary>
        /// Cold path: compiles and caches the delegate for the <typeparamref name="TIn"/>→<typeparamref name="TOut"/>
        /// type pair under the write lock (double-checked locking), then maps <paramref name="source"/>.
        /// </summary>
        /// <typeparam name="TIn">Source type.</typeparam>
        /// <typeparam name="TOut">Target type.</typeparam>
        /// <param name="source">The source object to map.</param>
        /// <returns>A newly created <typeparamref name="TOut"/> with all matching properties copied.</returns>
        private TOut CompileAndExecute<TIn, TOut>(TIn source) where TOut : new()
        {
            lock (_compileLock)
            {
                TypePairKey key = new TypePairKey(typeof(TIn), typeof(TOut));

                // Double-checked locking
                if (_cache.TryGetDelegate<TIn, TOut>(out Func<TIn, TOut>? mapper))
                    return mapper!(source);

                // Build plan and apply configuration
                (MappingPlan plan, TypePairConfiguration<TIn, TOut>? config) = GetOrBuildPlan<TIn, TOut>(key);

                IDelegateBuilder delegateBuilder = DelegateBuilderFactory.Create();
                Func<TIn, TOut> compiledMapper = delegateBuilder.BuildDelegate<TIn, TOut>(plan);

                if (config?.HasCustomMappings == true)
                    compiledMapper = config.WrapWithCustomMappings(compiledMapper);

                // Publish to cache (atomic copy-on-write)
                _cache.AddPlanAndDelegate(key, plan, compiledMapper);

                return compiledMapper(source);
            }
        }

        /// <summary>
        /// Batch mapping with buffer pooling for collection scenarios.
        /// Significantly faster than repeated Map() calls.
        /// </summary>
        public TOut[] MapBatch<TIn, TOut>(ReadOnlySpan<TIn> sources) where TOut : new()
        {
            if (sources.Length == 0)
                return [];

            Func<TIn, TOut> mapper = GetOrCompileMapper<TIn, TOut>();

            // Bulk map with minimal overhead
            TOut[] results = new TOut[sources.Length];
            for (int i = 0; i < sources.Length; i++)
            {
                results[i] = mapper(sources[i]);
            }
            return results;
        }

        /// <summary>
        /// Maps all items from <paramref name="source"/> directly into a pre-allocated
        /// <paramref name="destination"/> span. Zero additional allocation on the hot path.
        /// </summary>
        /// <remarks>
        /// Ideal for <c>stackalloc</c> and buffer-pool scenarios where the caller owns the
        /// output buffer. Use <see cref="MapBatch{TIn,TOut}(ReadOnlySpan{TIn})"/> when
        /// the mapper should allocate the result array.
        /// </remarks>
        /// <typeparam name="TIn">Source element type.</typeparam>
        /// <typeparam name="TOut">Target element type (must have a parameterless constructor).</typeparam>
        /// <param name="source">Source span to map.</param>
        /// <param name="destination">Pre-allocated output span; must be at least as long as <paramref name="source"/>.</param>
        /// <exception cref="ArgumentException">When <paramref name="destination"/> is shorter than <paramref name="source"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void MapBatchInPlace<TIn, TOut>(ReadOnlySpan<TIn> source, Span<TOut> destination) where TOut : new()
        {
            if (source.Length > destination.Length)
                throw new ArgumentException("Destination span is too small", nameof(destination));

            if (source.Length == 0)
                return;

            Func<TIn, TOut> mapper = GetOrCompileMapper<TIn, TOut>();

            // Zero-allocation tight loop
            for (int i = 0; i < source.Length; i++)
            {
                destination[i] = mapper(source[i]);
            }
        }

        /// <summary>
        /// Returns the cached mapping delegate for <typeparamref name="TIn"/>→<typeparamref name="TOut"/>,
        /// compiling and caching it on first access (double-checked locking).
        /// </summary>
        /// <typeparam name="TIn">Source type.</typeparam>
        /// <typeparam name="TOut">Target type.</typeparam>
        /// <returns>The compiled <see cref="Func{TIn, TOut}"/> delegate.</returns>
        private Func<TIn, TOut> GetOrCompileMapper<TIn, TOut>() where TOut : new()
        {
            if (_cache.TryGetDelegate<TIn, TOut>(out Func<TIn, TOut>? mapper))
                return mapper!;

            lock (_compileLock)
            {
                if (_cache.TryGetDelegate<TIn, TOut>(out mapper))
                    return mapper!;

                TypePairKey key = new TypePairKey(typeof(TIn), typeof(TOut));
                (MappingPlan plan, TypePairConfiguration<TIn, TOut>? config) = GetOrBuildPlan<TIn, TOut>(key);

                IDelegateBuilder delegateBuilder = DelegateBuilderFactory.Create();
                mapper = delegateBuilder.BuildDelegate<TIn, TOut>(plan);

                if (config?.HasCustomMappings == true)
                    mapper = config.WrapWithCustomMappings(mapper);

                _cache.AddPlanAndDelegate(key, plan, mapper);
                return mapper;
            }
        }

        /// <summary>
        /// Returns the <see cref="MappingPlan"/> and optional <see cref="Configuration.TypePairConfiguration{TIn,TOut}"/>
        /// for <typeparamref name="TIn"/>→<typeparamref name="TOut"/>.
        /// Reuses a cached plan when available; builds a fresh one otherwise, then applies any registered configuration.
        /// Must be called under <c>_compileLock</c>.
        /// </summary>
        private (MappingPlan Plan, TypePairConfiguration<TIn, TOut>? Config) GetOrBuildPlan<TIn, TOut>(TypePairKey key)
        {
            bool fromCache = _cache.TryGetPlan(key, out MappingPlan? plan);
            if (!fromCache)
                plan = _planBuilder.BuildPlan(typeof(TIn), typeof(TOut), new HashSet<TypePairKey>());
            TypePairConfiguration<TIn, TOut>? config = _configStore.TryGetValue(key, out object? cfg) ? (TypePairConfiguration<TIn, TOut>)cfg : null;
            if (config is not null && !fromCache)
                plan = config.ApplyToPlan(plan!);
            return (plan!, config);
        }

        #endregion
    }
}
