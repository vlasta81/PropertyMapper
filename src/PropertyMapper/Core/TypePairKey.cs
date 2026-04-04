using System.Runtime.CompilerServices;

namespace PropertyMapper.Core
{
    /// <summary>
    /// An efficient, allocation-free key that uniquely identifies an ordered pair of CLR types.
    /// Uses raw <see cref="nint"/> type-handle values so that equality and hashing avoid
    /// managed <see cref="Type.Equals(object?)"/> overhead.
    /// </summary>
    /// <remarks>
    /// .NET 10 optimization: <see cref="nint"/> comparison is faster than <see cref="Type.Equals(object?)"/>
    /// because it is a single CPU instruction rather than a virtual dispatch.
    /// </remarks>
    internal readonly struct TypePairKey : IEquatable<TypePairKey>
    {
        /// <summary>Raw handle of the source (<c>TIn</c>) type.</summary>
        private readonly nint _inTypeHandle;
        /// <summary>Raw handle of the target (<c>TOut</c>) type.</summary>
        private readonly nint _outTypeHandle;

        /// <summary>
        /// Initializes a new <see cref="TypePairKey"/> from a source and target type.
        /// </summary>
        /// <param name="inType">The source CLR type.</param>
        /// <param name="outType">The target CLR type.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TypePairKey(Type inType, Type outType)
        {
            _inTypeHandle = inType.TypeHandle.Value;
            _outTypeHandle = outType.TypeHandle.Value;
        }

        /// <summary>
        /// Returns <see langword="true"/> when both the source and target type handles are identical to those of <paramref name="other"/>.
        /// </summary>
        /// <param name="other">The other key to compare with.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(TypePairKey other) => _inTypeHandle == other._inTypeHandle && _outTypeHandle == other._outTypeHandle;

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is TypePairKey other && Equals(other);

        /// <summary>
        /// Returns a hash code derived from both type handles.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() => HashCode.Combine(_inTypeHandle, _outTypeHandle);

        /// <summary>Returns <see langword="true"/> when <paramref name="left"/> and <paramref name="right"/> represent the same type pair.</summary>
        public static bool operator ==(TypePairKey left, TypePairKey right) => left.Equals(right);

        /// <summary>Returns <see langword="true"/> when <paramref name="left"/> and <paramref name="right"/> represent different type pairs.</summary>
        public static bool operator !=(TypePairKey left, TypePairKey right) => !left.Equals(right);
    }

}

