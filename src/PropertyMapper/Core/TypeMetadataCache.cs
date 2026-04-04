using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Reflection;

namespace PropertyMapper.Core
{
    /// <summary>
    /// JIT-initialized, zero-overhead property metadata for a compile-time known type <typeparamref name="T"/>.
    /// </summary>
    /// <remarks>
    /// The CLR type-initializer guarantee provides exactly-once, thread-safe initialization with no lock
    /// overhead on subsequent reads — equivalent to <c>Lazy&lt;T&gt;</c> with
    /// <c>LazyThreadSafetyMode.ExecutionAndPublication</c> but without the wrapper allocation.
    /// </remarks>
    /// <typeparam name="T">The CLR type whose public instance properties are cached.</typeparam>
    internal static class TypeMetadata<T>
    {
        /// <summary>All public instance properties of <typeparamref name="T"/>, ordered as returned by reflection.</summary>
        internal static readonly PropertyInfo[] Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        /// <summary>
        /// Name-indexed map of all public instance properties of <typeparamref name="T"/>.
        /// Backed by a <see cref="FrozenDictionary{TKey,TValue}"/> for optimal lookup throughput.
        /// </summary>
        internal static readonly FrozenDictionary<string, PropertyInfo> PropertyMap = Properties.ToFrozenDictionary(p => p.Name, StringComparer.Ordinal);
    }

    /// <summary>
    /// Thread-safe, lock-free cache mapping a runtime <see cref="Type"/> to its public instance
    /// <see cref="PropertyInfo"/> array.
    /// </summary>
    /// <remarks>
    /// <para>
    /// <b>Mutable phase</b> (before <see cref="Freeze"/> is called):
    /// reads and writes operate on an <see cref="ImmutableDictionary{TKey,TValue}"/> snapshot
    /// swapped via <see cref="Interlocked.CompareExchange{T}"/> — lock-free copy-on-write semantics
    /// without the per-entry overhead of <see cref="System.Collections.Concurrent.ConcurrentDictionary{TKey,TValue}"/>.
    /// </para>
    /// <para>
    /// <b>Frozen phase</b> (after <see cref="Freeze"/> is called):
    /// a single atomic publish promotes the snapshot to a <see cref="FrozenDictionary{TKey,TValue}"/>
    /// whose reads are ~3–4× faster. Types absent from the frozen snapshot fall through to live reflection.
    /// </para>
    /// </remarks>
    internal static class TypeMetadataCache
    {
        // Mutable phase: ImmutableDictionary + Interlocked.CompareExchange (proposals C / E)
        private static ImmutableDictionary<Type, PropertyInfo[]> _mutable = ImmutableDictionary<Type, PropertyInfo[]>.Empty;

        // Frozen phase: set once after Warmup (proposal D)
        private static FrozenDictionary<Type, PropertyInfo[]>? _frozen;

        /// <summary>
        /// Returns the cached public instance properties for <paramref name="type"/>,
        /// invoking reflection only on the first encounter per type.
        /// </summary>
        /// <param name="type">The CLR type to inspect.</param>
        /// <returns>All public instance <see cref="PropertyInfo"/> objects for <paramref name="type"/>.</returns>
        internal static PropertyInfo[] GetProperties(Type type)
        {
            // Fast path: frozen snapshot (post-warmup, proposal D)
            if (Volatile.Read(ref _frozen) is { } frozen)
            {
                return frozen.TryGetValue(type, out PropertyInfo[]? f) ? f : type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            }

            // Mutable path: ImmutableDictionary + CAS loop (proposal E)
            while (true)
            {
                ImmutableDictionary<Type, PropertyInfo[]> current = Volatile.Read(ref _mutable);
                if (current.TryGetValue(type, out PropertyInfo[]? cached))
                    return cached;

                PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                ImmutableDictionary<Type, PropertyInfo[]> updated = current.SetItem(type, props);
                if (Interlocked.CompareExchange(ref _mutable, updated, current) == current)
                    return props;
                // CAS lost the race; another thread updated the dictionary. Retry — next iteration finds the key.
            }
        }

        /// <summary>
        /// Promotes the current cache snapshot to a <see cref="FrozenDictionary{TKey,TValue}"/>
        /// for maximum read throughput. Call once after all known type pairs are warmed up.
        /// Idempotent — subsequent calls have no effect.
        /// </summary>
        internal static void Freeze()
        {
            if (Volatile.Read(ref _frozen) is not null)
                return;
            FrozenDictionary<Type, PropertyInfo[]> candidate = Volatile.Read(ref _mutable).ToFrozenDictionary();
            Interlocked.CompareExchange(ref _frozen, candidate, null);
        }
    }
}
