using PropertyMapper.Masking;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace PropertyMapper
{
    /// <summary>
    /// Abstraction over <see cref="PropMap"/> that exposes the full public mapping API.
    /// Implement or mock this interface in tests; register via
    /// <see cref="Extensions.ServiceCollectionExtensions.AddPropertyMapper(Microsoft.Extensions.DependencyInjection.IServiceCollection)"/>
    /// and inject <see cref="IPropMap"/> rather than the concrete <see cref="PropMap"/>.
    /// </summary>
    /// <remarks>
    /// <see cref="PropMap.Clear"/> is intentionally omitted from this interface to prevent
    /// consumers from wiping the singleton mapping cache.
    /// </remarks>
    public interface IPropMap
    {
        // ── Core ────────────────────────────────────────────────────────────────

        /// <summary>Maps <paramref name="source"/> to a new <typeparamref name="TOut"/> instance.</summary>
        TOut Map<TIn, TOut>(TIn source) where TOut : new();

        // ── Null-Safe ────────────────────────────────────────────────────────────

        /// <summary>Maps <paramref name="source"/>, or returns <see langword="null"/> when source is <see langword="null"/>.</summary>
        TOut? MapOrDefault<TIn, TOut>(TIn? source) where TIn : class where TOut : class, new();

        /// <summary>Try-map pattern; returns <see langword="false"/> and sets <paramref name="result"/> to <see langword="null"/> when source is <see langword="null"/>.</summary>
        bool TryMap<TIn, TOut>(TIn? source, [NotNullWhen(true)] out TOut? result) where TIn : class where TOut : class, new();

        /// <summary>Maps <paramref name="source"/>, or returns <paramref name="fallback"/> when source is <see langword="null"/>.</summary>
        TOut MapOrElse<TIn, TOut>(TIn? source, TOut fallback) where TIn : class where TOut : class, new();

        /// <summary>Maps <paramref name="source"/>, or invokes <paramref name="fallbackFactory"/> when source is <see langword="null"/>.</summary>
        TOut MapOrElse<TIn, TOut>(TIn? source, Func<TOut> fallbackFactory) where TIn : class where TOut : class, new();

        // ── Field-Masked ─────────────────────────────────────────────────────────

        /// <summary>Maps <paramref name="source"/>, then applies <paramref name="mask"/> to zero out excluded fields.</summary>
        TOut MapThenApplyMask<TIn, TOut>(TIn source, IFieldMask<TOut> mask) where TOut : new();

        // ── Batch ────────────────────────────────────────────────────────────────

        /// <summary>Maps all items in <paramref name="sources"/> and returns a new array.</summary>
        TOut[] MapBatch<TIn, TOut>(ReadOnlySpan<TIn> sources) where TOut : new();

        /// <summary>Maps all items from <paramref name="source"/> directly into the pre-allocated <paramref name="destination"/> span.</summary>
        void MapBatchInPlace<TIn, TOut>(ReadOnlySpan<TIn> source, Span<TOut> destination) where TOut : new();

        // ── In-Place / Merge / Clone ─────────────────────────────────────────────

        /// <summary>Maps matching properties from <paramref name="source"/> onto an existing <paramref name="target"/> instance.</summary>
        void MapInto<TIn, TOut>(TIn source, TOut target) where TOut : class;

        /// <summary>Creates a new <typeparamref name="TOut"/> from <paramref name="source1"/>, then merges properties from <paramref name="source2"/> onto it.</summary>
        TOut MapMerge<TIn1, TIn2, TOut>(TIn1 source1, TIn2 source2) where TOut : class, new();

        /// <summary>Creates a shallow property-copy clone of <paramref name="source"/>.</summary>
        T Clone<T>(T source) where T : class, new();

        // ── Async ────────────────────────────────────────────────────────────────

        /// <summary>Asynchronously maps a single <paramref name="source"/> item.</summary>
        Task<TOut> MapAsync<TIn, TOut>(TIn source, CancellationToken cancellationToken = default) where TOut : new();

        /// <summary>Asynchronously maps all items in <paramref name="source"/>.</summary>
        Task<List<TOut>> MapAsync<TIn, TOut>(IEnumerable<TIn> source, CancellationToken cancellationToken = default) where TOut : new();

        /// <summary>Maps a collection in parallel using <c>Parallel.ForAsync</c>.</summary>
        Task<List<TOut>> MapParallelAsync<TIn, TOut>(IEnumerable<TIn> source, int maxDegreeOfParallelism = -1, CancellationToken cancellationToken = default) where TOut : new();

        /// <summary>Streams mapped results one at a time from an async source sequence.</summary>
        IAsyncEnumerable<TOut> MapStreamAsync<TIn, TOut>(IAsyncEnumerable<TIn> source, CancellationToken cancellationToken = default) where TOut : new();

        /// <summary>Streams mapped results in batches from an async source sequence.</summary>
        IAsyncEnumerable<List<TOut>> MapStreamBatchedAsync<TIn, TOut>(IAsyncEnumerable<TIn> source, int batchSize = 100, CancellationToken cancellationToken = default) where TOut : new();

        // ── Collections ──────────────────────────────────────────────────────────

        /// <summary>Maps <paramref name="source"/> into a new <typeparamref name="TOutCollection"/>.</summary>
        TOutCollection MapCollection<TIn, TOut, TOutCollection>(IEnumerable<TIn> source) where TOut : new() where TOutCollection : ICollection<TOut>, new();

        /// <summary>Maps <paramref name="source"/> and appends results into an existing <paramref name="destination"/> collection.</summary>
        void MapCollection<TIn, TOut>(IEnumerable<TIn> source, ICollection<TOut> destination) where TOut : new();

        /// <summary>Maps a source array to a new <c>TOut[]</c>.</summary>
        TOut[] MapArray<TIn, TOut>(TIn[] source) where TOut : new();

        /// <summary>Maps dictionary keys and/or values.</summary>
        Dictionary<TOutKey, TOutValue> MapDictionary<TInKey, TInValue, TOutKey, TOutValue>(Dictionary<TInKey, TInValue> source) where TInKey : notnull where TOutKey : notnull where TOutValue : new();

        /// <summary>Maps a dictionary and returns the result as a <see cref="FrozenDictionary{TKey,TValue}"/>.</summary>
        FrozenDictionary<TOutKey, TOutValue> MapToFrozenDictionary<TInKey, TInValue, TOutKey, TOutValue>(Dictionary<TInKey, TInValue> source) where TInKey : notnull where TOutKey : notnull where TOutValue : new();

        /// <summary>Maps <paramref name="source"/> to a <see cref="List{T}"/>.</summary>
        List<TOut> MapToList<TIn, TOut>(IEnumerable<TIn> source) where TOut : new();

        /// <summary>Maps a <see cref="ReadOnlySpan{T}"/> to a <see cref="List{T}"/>.</summary>
        List<TOut> MapToList<TIn, TOut>(ReadOnlySpan<TIn> source) where TOut : new();

        /// <summary>Maps <paramref name="source"/> to a <c>TOut[]</c>.</summary>
        TOut[] MapToArray<TIn, TOut>(IEnumerable<TIn> source) where TOut : new();

        /// <summary>Maps <paramref name="source"/> to a <see cref="HashSet{T}"/>.</summary>
        HashSet<TOut> MapToHashSet<TIn, TOut>(IEnumerable<TIn> source) where TOut : new();

        /// <summary>Maps nested collections (sequence of sequences) to a <see cref="List{T}"/> of <see cref="List{T}"/>.</summary>
        List<List<TOut>> MapNestedList<TIn, TOut>(IEnumerable<IEnumerable<TIn>> source) where TOut : new();

        /// <summary>Maps <paramref name="source"/> to an <see cref="ImmutableArray{T}"/>.</summary>
        ImmutableArray<TOut> MapToImmutableArray<TIn, TOut>(IEnumerable<TIn> source) where TOut : new();

        /// <summary>Maps <paramref name="source"/> to an <see cref="ImmutableList{T}"/>.</summary>
        ImmutableList<TOut> MapToImmutableList<TIn, TOut>(IEnumerable<TIn> source) where TOut : new();

        // ── Context ──────────────────────────────────────────────────────────────

        /// <summary>
        /// Maps <paramref name="source"/> and applies context-aware setters registered via
        /// <see cref="PropertyMapper.Configuration.TypePairConfiguration{TIn,TOut}.MapFromWithContext{TCtx,TProp}"/>.
        /// </summary>
        TOut MapWithContext<TIn, TOut, TCtx>(TIn source, TCtx context) where TOut : new();

        // ── Warmup & Diagnostics ─────────────────────────────────────────────────

        /// <summary>Pre-compiles the mapping delegate for the specified type pair.</summary>
        void Warmup<TIn, TOut>() where TOut : new();

        /// <summary>Pre-compiles mapping delegates for multiple type pairs concurrently.</summary>
        void WarmupBatch(params Type[] typePairs);

        /// <summary>Returns a point-in-time snapshot of cache utilisation.</summary>
        MappingStatistics GetStatistics();

        /// <summary>Validates the mapping plan and reports unmapped target properties.</summary>
        MappingValidationResult Validate<TIn, TOut>() where TOut : new();

        // ── Projection ───────────────────────────────────────────────────────────

        /// <summary>Returns a cached expression tree for <c>IQueryable</c> projection.</summary>
        Expression<Func<TIn, TOut>> GetProjectionExpression<TIn, TOut>() where TOut : new();

        /// <summary>Projects an <see cref="IQueryable{T}"/> using the cached expression tree.</summary>
        IQueryable<TOut> Project<TIn, TOut>(IQueryable<TIn> source) where TOut : new();

        /// <summary>Returns a per-request (uncached) expression tree with masked fields excluded.</summary>
        Expression<Func<TIn, TOut>> GetProjectionExpression<TIn, TOut>(IFieldMask<TOut> mask) where TOut : new();

        /// <summary>Projects an <see cref="IQueryable{T}"/> with masked fields excluded from the SQL SELECT.</summary>
        IQueryable<TOut> Project<TIn, TOut>(IQueryable<TIn> source, IFieldMask<TOut> mask) where TOut : new();
    }
}
