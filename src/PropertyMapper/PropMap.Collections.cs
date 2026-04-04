using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PropertyMapper
{
    public sealed partial class PropMap
    {
        #region Collection Mapping

        /// <summary>
        /// Maps all items in <paramref name="source"/> using a compiled delegate and adds them to a new
        /// <typeparamref name="TOutCollection"/> instance.
        /// </summary>
        /// <remarks>
        /// When the output collection is a <see cref="List{T}"/> and the source is an array, the
        /// backing buffer is written directly via <c>CollectionsMarshal.AsSpan</c> to avoid
        /// per-element <c>Add</c> overhead. For all other collection types the standard <c>Add</c>
        /// loop is used.
        /// </remarks>
        /// <typeparam name="TIn">Source element type.</typeparam>
        /// <typeparam name="TOut">Target element type (must have a parameterless constructor).</typeparam>
        /// <typeparam name="TOutCollection">
        /// Target collection type; must implement <see cref="ICollection{T}"/> and provide a
        /// parameterless constructor.
        /// </typeparam>
        /// <param name="source">Source sequence to map.</param>
        /// <returns>A new <typeparamref name="TOutCollection"/> containing all mapped elements.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="source"/> is <see langword="null"/>.</exception>
        public TOutCollection MapCollection<TIn, TOut, TOutCollection>(IEnumerable<TIn> source) where TOut : new() where TOutCollection : ICollection<TOut>, new()
        {
            ArgumentNullException.ThrowIfNull(source);

            Func<TIn, TOut> mapper = GetOrCompileMapper<TIn, TOut>();
            TOutCollection result = new TOutCollection();

            if (source is TIn[] array && result is List<TOut> list)
            {
                CollectionsMarshal.SetCount(list, array.Length);
                Span<TOut> outSpan = CollectionsMarshal.AsSpan(list);
                for (int i = 0; i < array.Length; i++)
                    outSpan[i] = mapper(array[i]);
                return (TOutCollection)(object)list;
            }

            if (source is ICollection<TIn> collection && result is List<TOut> listResult)
            {
                listResult.Capacity = collection.Count;
            }

            foreach (TIn item in source)
            {
                result.Add(mapper(item));
            }

            return result;
        }

        /// <summary>
        /// Maps items from <paramref name="source"/> and appends them into an existing
        /// <paramref name="destination"/> collection. Unlike the three-type-parameter
        /// <see cref="MapCollection{TIn,TOut,TOutCollection}"/> overload, the destination is
        /// caller-supplied rather than created by the method, making it suitable for
        /// populating pre-allocated or pre-populated collections.
        /// </summary>
        /// <typeparam name="TIn">Source element type.</typeparam>
        /// <typeparam name="TOut">Target element type (must have a parameterless constructor).</typeparam>
        /// <param name="source">Source sequence to map.</param>
        /// <param name="destination">Existing collection to populate with mapped elements.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="source"/> or <paramref name="destination"/> is <see langword="null"/>.</exception>
        public void MapCollection<TIn, TOut>(IEnumerable<TIn> source, ICollection<TOut> destination) where TOut : new()
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(destination);
            Func<TIn, TOut> mapper = GetOrCompileMapper<TIn, TOut>();
            foreach (TIn item in source)
                destination.Add(mapper(item));
        }

        /// <summary>
        /// Maps a <c>TIn[]</c> source array to a new <c>TOut[]</c>.
        /// Delegates to <see cref="MapBatch{TIn,TOut}(ReadOnlySpan{TIn})"/> via a span:
        /// single allocation, no enumerator overhead.
        /// </summary>
        /// <typeparam name="TIn">Source element type.</typeparam>
        /// <typeparam name="TOut">Target element type (must have a parameterless constructor).</typeparam>
        /// <param name="source">Source array to map.</param>
        /// <returns>
        /// A new <c>TOut[]</c> containing the mapped elements,
        /// or an empty array when <paramref name="source"/> is empty.
        /// </returns>
        /// <exception cref="ArgumentNullException">When <paramref name="source"/> is <see langword="null"/>.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TOut[] MapArray<TIn, TOut>(TIn[] source) where TOut : new()
        {
            ArgumentNullException.ThrowIfNull(source);
            return source.Length == 0 ? [] : MapBatch<TIn, TOut>(source.AsSpan());
        }

        /// <summary>Per-type-pair cache for key-casting delegates used by <see cref="MapDictionary{TInKey,TInValue,TOutKey,TOutValue}"/>.
        /// One static <see cref="Func{TIn,TOut}"/> instance is allocated per concrete <c>(TIn, TOut)</c> pair — never per call.</summary>
        private static class KeyCast<TIn, TOut>
        {
            internal static readonly Func<TIn, TOut> Instance = CreateCastDelegate();

            private static Func<TIn, TOut> CreateCastDelegate()
            {
                if (!typeof(TOut).IsAssignableFrom(typeof(TIn)))
                {
                    // Return a throwing delegate rather than throwing here.
                    // Throwing during a static field initialiser causes the CLR to cache a
                    // TypeInitializationException for the lifetime of the process, permanently
                    // poisoning this type pair. Surfacing the error at the call site instead
                    // produces a clean InvalidOperationException on every invocation.
                    string msg = $"Cannot cast dictionary key from '{typeof(TIn).Name}' to '{typeof(TOut).Name}': types are not assignable.";
                    return _ => throw new InvalidOperationException(msg);
                }
                return static k => (TOut)(object)k!;
            }
        }

        /// <summary>
        /// Maps a source dictionary, applying the compiled value mapper to each entry.
        /// When <typeparamref name="TInKey"/> and <typeparamref name="TOutKey"/> differ, the key is cast
        /// via the validated <see cref="KeyCast{TIn,TOut}"/> delegate; otherwise a direct cast is used.
        /// </summary>
        /// <typeparam name="TInKey">Source key type.</typeparam>
        /// <typeparam name="TInValue">Source value type.</typeparam>
        /// <typeparam name="TOutKey">Target key type.</typeparam>
        /// <typeparam name="TOutValue">Target value type (must have a parameterless constructor).</typeparam>
        /// <param name="source">The source dictionary to map.</param>
        /// <returns>
        /// A new <see cref="Dictionary{TKey,TValue}"/> with mapped keys and values,
        /// pre-sized to <c><paramref name="source"/>.Count</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">When <paramref name="source"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">
        /// When <typeparamref name="TInKey"/> and <typeparamref name="TOutKey"/> are unrelated types
        /// that cannot be cast to each other. Detected on the first use of the key type pair via
        /// <see cref="KeyCast{TIn,TOut}"/>.
        /// </exception>
        public Dictionary<TOutKey, TOutValue> MapDictionary<TInKey, TInValue, TOutKey, TOutValue>(Dictionary<TInKey, TInValue> source) where TInKey : notnull where TOutKey : notnull where TOutValue : new()
        {
            ArgumentNullException.ThrowIfNull(source);

            Dictionary<TOutKey, TOutValue> result = new Dictionary<TOutKey, TOutValue>(source.Count);
            Func<TInValue, TOutValue> valueMapper = GetOrCompileMapper<TInValue, TOutValue>();

            bool mapKeys = typeof(TInKey) != typeof(TOutKey);
            Func<TInKey, TOutKey>? keyMapper = mapKeys ? KeyCast<TInKey, TOutKey>.Instance : null;

            foreach (KeyValuePair<TInKey, TInValue> kvp in source)
            {
                TOutKey outKey = mapKeys ? keyMapper!(kvp.Key) : (TOutKey)(object)kvp.Key;
                TOutValue outValue = valueMapper(kvp.Value);
                result[outKey] = outValue;
            }

            return result;
        }

        /// <summary>
        /// Maps a dictionary and returns the result as a <see cref="FrozenDictionary{TKey,TValue}"/>.
        /// Equivalent to <see cref="MapDictionary{TInKey,TInValue,TOutKey,TOutValue}"/> followed by
        /// <c>ToFrozenDictionary()</c>, but expressed as a single call.
        /// </summary>
        /// <typeparam name="TInKey">Source key type.</typeparam>
        /// <typeparam name="TInValue">Source value type.</typeparam>
        /// <typeparam name="TOutKey">Target key type.</typeparam>
        /// <typeparam name="TOutValue">Target value type (must have a parameterless constructor).</typeparam>
        /// <param name="source">The dictionary to map.</param>
        /// <returns>A new <see cref="FrozenDictionary{TKey,TValue}"/> with mapped keys and values.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="source"/> is <see langword="null"/>.</exception>
        public FrozenDictionary<TOutKey, TOutValue> MapToFrozenDictionary<TInKey, TInValue, TOutKey, TOutValue>(Dictionary<TInKey, TInValue> source) where TInKey : notnull where TOutKey : notnull where TOutValue : new() => MapDictionary<TInKey, TInValue, TOutKey, TOutValue>(source).ToFrozenDictionary();

        /// <summary>
        /// Maps <see cref="IEnumerable{T}"/> to <see cref="List{T}"/>.
        /// </summary>
        /// <remarks>
        /// Optimised fast paths (in priority order):
        /// <list type="number">
        ///   <item><description>
        ///     <c>List&lt;TIn&gt;</c> — <c>CollectionsMarshal.AsSpan</c> exposes the internal backing
        ///     array directly; <c>CollectionsMarshal.SetCount</c> pre-sizes the output list so the loop
        ///     writes via a span index — no <c>Add</c> overhead.
        ///   </description></item>
        ///   <item><description>
        ///     <c>TIn[]</c> — same <c>SetCount + AsSpan</c> strategy as the List path; single allocation.
        ///   </description></item>
        ///   <item><description>
        ///     <c>ICollection&lt;TIn&gt;</c> — output list pre-sized from <c>Count</c>; span-indexed write.
        ///   </description></item>
        ///   <item><description>General <c>IEnumerable&lt;TIn&gt;</c> — standard foreach / <c>Add</c> fallback (length unknown).</description></item>
        /// </list>
        /// </remarks>
        /// <typeparam name="TIn">Source element type.</typeparam>
        /// <typeparam name="TOut">Target element type.</typeparam>
        /// <param name="source">Source sequence to map.</param>
        /// <returns>A new <see cref="List{TOut}"/> containing the mapped elements.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="source"/> is null.</exception>
        public List<TOut> MapToList<TIn, TOut>(IEnumerable<TIn> source) where TOut : new()
        {
            ArgumentNullException.ThrowIfNull(source);

            Func<TIn, TOut> mapper = GetOrCompileMapper<TIn, TOut>();

            // List<TIn> fast path: SetCount pre-sizes _size to avoid List<TOut>.Add overhead;
            // AsSpan exposes the backing buffer — the loop writes directly via span index (stelem).
            if (source is List<TIn> srcList)
            {
                ReadOnlySpan<TIn> span = CollectionsMarshal.AsSpan(srcList);
                List<TOut> result = new(span.Length);
                CollectionsMarshal.SetCount(result, span.Length);
                Span<TOut> outSpan = CollectionsMarshal.AsSpan(result);
                for (int i = 0; i < span.Length; i++)
                    outSpan[i] = mapper(span[i]);
                return result;
            }

            // Array fast path: same SetCount + AsSpan strategy; single allocation.
            if (source is TIn[] array)
            {
                List<TOut> result = new(array.Length);
                CollectionsMarshal.SetCount(result, array.Length);
                Span<TOut> outSpan = CollectionsMarshal.AsSpan(result);
                for (int i = 0; i < array.Length; i++)
                    outSpan[i] = mapper(array[i]);
                return result;
            }

            // ICollection<TIn>: pre-size output list from Count; span-indexed write via foreach counter.
            if (source is ICollection<TIn> collection)
            {
                int count = collection.Count;
                List<TOut> result = new(count);
                CollectionsMarshal.SetCount(result, count);
                Span<TOut> outSpan = CollectionsMarshal.AsSpan(result);
                int idx = 0;
                foreach (TIn item in collection)
                    outSpan[idx++] = mapper(item);
                return result;
            }

            // General IEnumerable fallback — length unknown; Add is the only viable strategy.
            {
                List<TOut> result = [];
                foreach (TIn item in source)
                    result.Add(mapper(item));
                return result;
            }
        }

        /// <summary>
        /// Maps a <see cref="ReadOnlySpan{T}"/> directly to <see cref="List{T}"/>.
        /// </summary>
        /// <remarks>
        /// Equivalent throughput to <see cref="MapBatch{TIn,TOut}(ReadOnlySpan{TIn})"/> but returns a
        /// <see cref="List{TOut}"/> instead of an array.
        /// Uses <c>CollectionsMarshal.SetCount</c> to pre-size the output list and
        /// <c>CollectionsMarshal.AsSpan</c> to write elements directly into the backing buffer —
        /// no <c>Add</c> overhead, single allocation.
        /// </remarks>
        /// <typeparam name="TIn">Source element type.</typeparam>
        /// <typeparam name="TOut">Target element type.</typeparam>
        /// <param name="source">Source span to map.</param>
        /// <returns>A new <see cref="List{TOut}"/> containing the mapped elements.</returns>
        public List<TOut> MapToList<TIn, TOut>(ReadOnlySpan<TIn> source) where TOut : new()
        {
            if (source.IsEmpty)
                return [];

            Func<TIn, TOut> mapper = GetOrCompileMapper<TIn, TOut>();

            List<TOut> result = new(source.Length);
            CollectionsMarshal.SetCount(result, source.Length);
            Span<TOut> outSpan = CollectionsMarshal.AsSpan(result);
            for (int i = 0; i < source.Length; i++)
                outSpan[i] = mapper(source[i]);
            return result;
        }

        /// <summary>
        /// Maps <see cref="IEnumerable{T}"/> to <c>TOut[]</c>.
        /// </summary>
        /// <remarks>
        /// Fast paths (in priority order):
        /// <list type="number">
        ///   <item><description>
        ///     <c>TIn[]</c> — passed directly to <see cref="MapBatch{TIn,TOut}"/> as a span; single allocation.
        ///   </description></item>
        ///   <item><description>
        ///     <c>List&lt;TIn&gt;</c> — internal backing array exposed via <c>CollectionsMarshal.AsSpan</c>;
        ///     single allocation, no enumerator.
        ///   </description></item>
        ///   <item><description>
        ///     <c>ICollection&lt;TIn&gt;</c> — output array pre-allocated from <c>Count</c> and filled
        ///     in a single pass; avoids the intermediate <c>List&lt;TOut&gt;</c> + <c>ToArray()</c>
        ///     double-allocation that the general fallback would cause.
        ///   </description></item>
        ///   <item><description>
        ///     General <c>IEnumerable&lt;TIn&gt;</c> — falls back through <see cref="MapToList{TIn,TOut}(System.Collections.Generic.IEnumerable{TIn})"/>.
        ///   </description></item>
        /// </list>
        /// </remarks>
        /// <typeparam name="TIn">Source element type.</typeparam>
        /// <typeparam name="TOut">Target element type.</typeparam>
        /// <param name="source">Source sequence to map.</param>
        /// <returns>A new <c>TOut[]</c> containing the mapped elements.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="source"/> is null.</exception>
        public TOut[] MapToArray<TIn, TOut>(IEnumerable<TIn> source) where TOut : new()
        {
            ArgumentNullException.ThrowIfNull(source);

            // TIn[] — direct span, single allocation.
            if (source is TIn[] array)
                return MapBatch<TIn, TOut>(array.AsSpan());

            // List<TIn> — internal backing array via CollectionsMarshal, single allocation.
            if (source is List<TIn> srcList)
                return MapBatch<TIn, TOut>(CollectionsMarshal.AsSpan(srcList));

            // ICollection<TIn> — known count: allocate result array once, fill in a single pass.
            // Avoids MapToList() + ToArray() which would allocate two arrays (List internal buffer + copy).
            if (source is ICollection<TIn> collection)
            {
                TOut[] result = new TOut[collection.Count];
                Func<TIn, TOut> mapper = GetOrCompileMapper<TIn, TOut>();
                int i = 0;
                foreach (TIn item in collection)
                    result[i++] = mapper(item);
                return result;
            }

            // General IEnumerable — unknown count, must build list first.
            return MapToList<TIn, TOut>(source).ToArray();
        }

        /// <summary>
        /// Maps all items in <paramref name="source"/> to a new <see cref="HashSet{T}"/>.
        /// Delegates to <see cref="MapCollection{TIn,TOut,TOutCollection}"/>;
        /// insertion order and uniqueness semantics follow the default <see cref="HashSet{T}"/> equality comparer.
        /// </summary>
        /// <typeparam name="TIn">Source element type.</typeparam>
        /// <typeparam name="TOut">Target element type (must have a parameterless constructor).</typeparam>
        /// <param name="source">Source sequence to map.</param>
        /// <returns>A new <see cref="HashSet{TOut}"/> containing all mapped elements.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="source"/> is <see langword="null"/>.</exception>
        public HashSet<TOut> MapToHashSet<TIn, TOut>(IEnumerable<TIn> source) where TOut : new() => MapCollection<TIn, TOut, HashSet<TOut>>(source);

        /// <summary>
        /// Maps a sequence of sequences (e.g. a jagged array or list-of-lists) to a
        /// <see cref="List{T}"/> of <see cref="List{T}"/>.
        /// Each inner sequence is mapped independently via
        /// <see cref="MapToList{TIn,TOut}(IEnumerable{TIn})"/>.
        /// </summary>
        /// <typeparam name="TIn">Source element type.</typeparam>
        /// <typeparam name="TOut">Target element type (must have a parameterless constructor).</typeparam>
        /// <param name="source">Outer sequence of inner source sequences to map.</param>
        /// <returns>
        /// A new <see cref="List{T}"/> of <see cref="List{TOut}"/>;
        /// outer count matches the number of inner sequences in <paramref name="source"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">When <paramref name="source"/> is <see langword="null"/>.</exception>
        public List<List<TOut>> MapNestedList<TIn, TOut>(IEnumerable<IEnumerable<TIn>> source) where TOut : new()
        {
            ArgumentNullException.ThrowIfNull(source);

            List<List<TOut>> result = source is ICollection<IEnumerable<TIn>> col ? new(col.Count) : [];
            foreach (IEnumerable<TIn> innerSource in source)
            {
                result.Add(MapToList<TIn, TOut>(innerSource));
            }
            return result;
        }

        /// <summary>
        /// Maps <see cref="IEnumerable{T}"/> to an <see cref="ImmutableArray{T}"/>.
        /// </summary>
        /// <remarks>
        /// Fast paths (in priority order):
        /// <list type="number">
        ///   <item><description><c>List&lt;TIn&gt;</c> — <c>CollectionsMarshal.AsSpan</c>; builder pre-allocated with exact capacity (<c>MoveToImmutable</c>, zero-copy).</description></item>
        ///   <item><description><c>TIn[]</c> — indexed loop; builder pre-allocated.</description></item>
        ///   <item><description><c>ICollection&lt;TIn&gt;</c> — builder pre-allocated from <c>Count</c> (<c>MoveToImmutable</c>).</description></item>
        ///   <item><description>General <c>IEnumerable&lt;TIn&gt;</c> — falls back to <c>ToImmutable</c>.</description></item>
        /// </list>
        /// </remarks>
        /// <typeparam name="TIn">Source element type.</typeparam>
        /// <typeparam name="TOut">Target element type (must have a parameterless constructor).</typeparam>
        /// <param name="source">Source sequence to map.</param>
        /// <returns>A new <see cref="ImmutableArray{T}"/> containing the mapped elements.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="source"/> is <see langword="null"/>.</exception>
        public ImmutableArray<TOut> MapToImmutableArray<TIn, TOut>(IEnumerable<TIn> source) where TOut : new()
        {
            ArgumentNullException.ThrowIfNull(source);
            Func<TIn, TOut> mapper = GetOrCompileMapper<TIn, TOut>();

            // List<TIn> fast path: span avoids enumerator allocation; exact capacity enables MoveToImmutable.
            if (source is List<TIn> srcList)
            {
                ReadOnlySpan<TIn> span = CollectionsMarshal.AsSpan(srcList);
                ImmutableArray<TOut>.Builder builder = ImmutableArray.CreateBuilder<TOut>(span.Length);
                builder.Count = span.Length;
                for (int i = 0; i < span.Length; i++)
                    builder[i] = mapper(span[i]);
                return builder.MoveToImmutable();
            }

            // TIn[] fast path: indexed loop; exact capacity.
            if (source is TIn[] array)
            {
                ImmutableArray<TOut>.Builder builder = ImmutableArray.CreateBuilder<TOut>(array.Length);
                builder.Count = array.Length;
                for (int i = 0; i < array.Length; i++)
                    builder[i] = mapper(array[i]);
                return builder.MoveToImmutable();
            }

            // ICollection<TIn>: known count — pre-allocate and use zero-copy MoveToImmutable.
            if (source is ICollection<TIn> col)
            {
                ImmutableArray<TOut>.Builder builder = ImmutableArray.CreateBuilder<TOut>(col.Count);
                builder.Count = col.Count;
                int idx = 0;
                foreach (TIn item in col)
                    builder[idx++] = mapper(item);
                return builder.MoveToImmutable();
            }

            // General IEnumerable — unknown count; use ToImmutable (copies internal buffer once).
            {
                ImmutableArray<TOut>.Builder builder = ImmutableArray.CreateBuilder<TOut>();
                foreach (TIn item in source)
                    builder.Add(mapper(item));
                return builder.ToImmutable();
            }
        }

        /// <summary>
        /// Maps all items in <paramref name="source"/> to an <see cref="ImmutableList{T}"/>.
        /// Uses <see cref="ImmutableList{T}.Builder"/> to accumulate mapped elements before
        /// calling <c>ToImmutable()</c> once, minimising intermediate allocations.
        /// </summary>
        /// <typeparam name="TIn">Source element type.</typeparam>
        /// <typeparam name="TOut">Target element type (must have a parameterless constructor).</typeparam>
        /// <param name="source">Source sequence to map.</param>
        /// <returns>An <see cref="ImmutableList{TOut}"/> containing all mapped elements in source order.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="source"/> is <see langword="null"/>.</exception>
        public ImmutableList<TOut> MapToImmutableList<TIn, TOut>(IEnumerable<TIn> source) where TOut : new()
        {
            ArgumentNullException.ThrowIfNull(source);
            Func<TIn, TOut> mapper = GetOrCompileMapper<TIn, TOut>();
            ImmutableList<TOut>.Builder builder = ImmutableList.CreateBuilder<TOut>();
            foreach (TIn item in source)
                builder.Add(mapper(item));
            return builder.ToImmutable();
        }

        #endregion
    }
}
