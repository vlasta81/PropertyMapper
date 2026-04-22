using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PropertyMapper
{
    public sealed partial class PropMap
    {
        #region Async Mapping

        /// <summary>
        /// .NET 10 Optimization: Struct worker for zero-allocation async mapping.
        /// Eliminates closure allocations in Parallel.ForAsync.
        /// </summary>
        private readonly struct AsyncMappingWorker<TIn, TOut> where TOut : new()
        {
            /// <summary>The mapper instance used to perform each individual item mapping.</summary>
            private readonly PropMap _mapper;
            /// <summary>The read-only list of source items to map.</summary>
            private readonly IReadOnlyList<TIn> _source;
            /// <summary>Pre-sized destination list whose backing buffer receives mapped results directly via <c>CollectionsMarshal.AsSpan</c>.</summary>
            private readonly List<TOut> _destination;

            /// <summary>
            /// Initializes a new <see cref="AsyncMappingWorker{TIn,TOut}"/> that maps items from
            /// <paramref name="source"/> into <paramref name="destination"/> using <paramref name="mapper"/>.
            /// </summary>
            /// <param name="mapper">The mapper to use for each item.</param>
            /// <param name="source">The ordered list of source items.</param>
            /// <param name="destination">Pre-sized list (via <c>CollectionsMarshal.SetCount</c>); must hold at least as many elements as <paramref name="source"/>.</param>
            public AsyncMappingWorker(PropMap mapper, IReadOnlyList<TIn> source, List<TOut> destination)
            {
                _mapper = mapper;
                _source = source;
                _destination = destination;
            }

            /// <summary>
            /// Maps the item at <paramref name="index"/> and stores the result directly into the destination
            /// list's backing buffer via <c>CollectionsMarshal.AsSpan</c>.
            /// Called by <c>Parallel.ForAsync</c> — the struct layout eliminates closure allocations.
            /// </summary>
            /// <param name="index">Index of the item to process.</param>
            /// <param name="ct">Cancellation token (unused; mapping is synchronous).</param>
            /// <returns>A completed <see cref="ValueTask"/>.</returns>
            public ValueTask MapAtIndexAsync(int index, CancellationToken ct)
            {
                CollectionsMarshal.AsSpan(_destination)[index] = _mapper.Map<TIn, TOut>(_source[index]);
                return ValueTask.CompletedTask;
            }
        }

        /// <summary>
        /// Offloads the synchronous <see cref="Map{TIn,TOut}(TIn)"/> call to a thread-pool thread
        /// so the caller's <c>await</c> does not block.
        /// </summary>
        /// <typeparam name="TIn">Source type.</typeparam>
        /// <typeparam name="TOut">Target type (must have a parameterless constructor).</typeparam>
        /// <param name="source">Source object to map.</param>
        /// <param name="cancellationToken">Token that can cancel the queued work item before it starts.</param>
        /// <returns>A task that resolves to a newly mapped <typeparamref name="TOut"/> instance.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="source"/> is <see langword="null"/>.</exception>
        public Task<TOut> MapAsync<TIn, TOut>(TIn source, CancellationToken cancellationToken = default) where TOut : new()
        {
            ArgumentNullException.ThrowIfNull(source);
            return Task.Run(() => Map<TIn, TOut>(source), cancellationToken);
        }

        /// <summary>
        /// Asynchronously maps a collection of items.
        /// Offloads the entire synchronous mapping loop to a thread-pool thread in a single
        /// <see cref="Task.Run{TResult}(Func{TResult},CancellationToken)"/> call, avoiding
        /// per-item scheduling overhead.
        /// </summary>
        /// <typeparam name="TIn">Source element type.</typeparam>
        /// <typeparam name="TOut">Target element type (must have a parameterless constructor).</typeparam>
        /// <param name="source">The sequence of items to map.</param>
        /// <param name="cancellationToken">Token observed inside the mapping loop to support cooperative cancellation.</param>
        /// <returns>A task that resolves to a <see cref="List{T}"/> of mapped <typeparamref name="TOut"/> instances.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="source"/> is <see langword="null"/>.</exception>
        public Task<List<TOut>> MapAsync<TIn, TOut>(IEnumerable<TIn> source, CancellationToken cancellationToken = default) where TOut : new()
        {
            ArgumentNullException.ThrowIfNull(source);

            // Resolve the compiled mapper on the calling thread to avoid a ConcurrentDictionary
            // lookup inside the Task.Run lambda, and to keep the hot-path allocation-free.
            Func<TIn, TOut> mapper = GetOrCompileMapper<TIn, TOut>();

            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                List<TOut> result = source is ICollection<TIn> col ? new(col.Count) : [];
                foreach (TIn item in source)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    result.Add(mapper(item));
                }
                return result;
            }, cancellationToken);
        }

        // Cached ParallelOptions for the common default case (unconstrained parallelism,
        // no cancellation) — avoids a heap allocation on every MapParallelAsync call.
        private static readonly ParallelOptions s_defaultParallelOptions = new() { MaxDegreeOfParallelism = -1 };

        /// <summary>
        /// Maps a collection in parallel using <c>Parallel.ForAsync</c>.
        /// Uses a zero-allocation struct worker (<see cref="AsyncMappingWorker{TIn,TOut}"/>) to eliminate
        /// closure allocations on the hot path.
        /// </summary>
        /// <typeparam name="TIn">Source element type.</typeparam>
        /// <typeparam name="TOut">Target element type (must have a parameterless constructor).</typeparam>
        /// <param name="source">The sequence of items to map.</param>
        /// <param name="maxDegreeOfParallelism">
        /// Maximum number of concurrent mapping operations.
        /// Pass <c>-1</c> (default) to let the runtime choose based on available processors.
        /// </param>
        /// <param name="cancellationToken">Token that cancels the parallel loop.</param>
        /// <returns>A task that resolves to a <see cref="List{T}"/> of mapped <typeparamref name="TOut"/> instances in source order.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="source"/> is <see langword="null"/>.</exception>
        public async Task<List<TOut>> MapParallelAsync<TIn, TOut>(IEnumerable<TIn> source, int maxDegreeOfParallelism = -1, CancellationToken cancellationToken = default) where TOut : new()
        {
            ArgumentNullException.ThrowIfNull(source);

            // Fast-path: if source is already a List<TIn>, use it directly to avoid an
            // unnecessary copy. Otherwise snapshot to prevent mid-flight modification.
            List<TIn> sourceList = source is List<TIn> list ? list : source.ToList();
            int count = sourceList.Count;

            List<TOut> result = new(count);
            CollectionsMarshal.SetCount(result, count);

            // Reuse the cached options for the common case; allocate only when the caller
            // provides a custom degree of parallelism or a cancellation token.
            ParallelOptions options = (maxDegreeOfParallelism == -1 && cancellationToken == default) ? s_defaultParallelOptions : new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism, CancellationToken = cancellationToken };

            AsyncMappingWorker<TIn, TOut> worker = new(this, sourceList, result);
            await Parallel.ForAsync(0, count, options, (i, ct) => worker.MapAtIndexAsync(i, ct)).ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Streams mapped results asynchronously, yielding one <typeparamref name="TOut"/> at a time
        /// as items arrive on the source <see cref="IAsyncEnumerable{T}"/>.
        /// Memory-efficient for large or infinite datasets because only one batch is in-flight at a time.
        /// Mapping is synchronous CPU work and is called inline (no extra thread-pool dispatch per item).
        /// </summary>
        /// <typeparam name="TIn">Source element type.</typeparam>
        /// <typeparam name="TOut">Target element type (must have a parameterless constructor).</typeparam>
        /// <param name="source">Async sequence of source items.</param>
        /// <param name="cancellationToken">Token passed to the source enumerator to cancel iteration.</param>
        /// <returns>An async sequence of mapped <typeparamref name="TOut"/> instances.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="source"/> is <see langword="null"/>.</exception>
        public async IAsyncEnumerable<TOut> MapStreamAsync<TIn, TOut>(IAsyncEnumerable<TIn> source, [EnumeratorCancellation] CancellationToken cancellationToken = default) where TOut : new()
        {
            ArgumentNullException.ThrowIfNull(source);

            Func<TIn, TOut> mapper = GetOrCompileMapper<TIn, TOut>();
            await foreach (TIn item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                yield return mapper(item);
            }
        }

        /// <summary>
        /// Streams mapped results in batches, yielding a <see cref="List{TOut}"/> every time
        /// <paramref name="batchSize"/> source items have been accumulated.
        /// More throughput-efficient than <see cref="MapStreamAsync{TIn,TOut}"/> because the compiled
        /// mapper delegate is retrieved once per batch rather than once per item.
        /// </summary>
        /// <typeparam name="TIn">Source element type.</typeparam>
        /// <typeparam name="TOut">Target element type (must have a parameterless constructor).</typeparam>
        /// <param name="source">Async sequence of source items.</param>
        /// <param name="batchSize">Number of items per emitted batch. Default: 100.</param>
        /// <param name="cancellationToken">Token passed to the source enumerator to cancel iteration.</param>
        /// <returns>An async sequence of <see cref="List{TOut}"/> batches.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="source"/> is <see langword="null"/>.</exception>
        public async IAsyncEnumerable<List<TOut>> MapStreamBatchedAsync<TIn, TOut>(IAsyncEnumerable<TIn> source, int batchSize = 100, [EnumeratorCancellation] CancellationToken cancellationToken = default) where TOut : new()
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(batchSize);

            Func<TIn, TOut> mapper = GetOrCompileMapper<TIn, TOut>();
            List<TIn> batch = new List<TIn>(batchSize);

            await foreach (TIn item in source.WithCancellation(cancellationToken).ConfigureAwait(false))
            {
                batch.Add(item);

                if (batch.Count >= batchSize)
                {
                    yield return FlushBatch(batch, mapper);
                    batch.Clear();
                }
            }

            if (batch.Count > 0)
                yield return FlushBatch(batch, mapper);

            static List<TOut> FlushBatch(List<TIn> src, Func<TIn, TOut> map)
            {
                ReadOnlySpan<TIn> srcSpan = CollectionsMarshal.AsSpan(src);
                List<TOut> mapped = new(srcSpan.Length);
                CollectionsMarshal.SetCount(mapped, srcSpan.Length);
                Span<TOut> outSpan = CollectionsMarshal.AsSpan(mapped);
                for (int i = 0; i < srcSpan.Length; i++)
                    outSpan[i] = map(srcSpan[i]);
                return mapped;
            }
        }

        #endregion
    }
}
