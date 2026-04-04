using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PropertyMapper.Core
{
    /// <summary>
    /// Fixed-size inline array buffer for up to 16 <see cref="PropertyBinding"/> elements.
    /// Enables stack allocation for type pairs with ≤ 16 properties — the most common scenario.
    /// </summary>
    /// <remarks>
    /// .NET 10 <c>[InlineArray(16)]</c> optimization:
    /// <list type="bullet">
    /// <item>Zero heap allocations for small objects — all bindings reside in a single contiguous stack region.</item>
    /// <item>Better cache locality compared to a separately heap-allocated array.</item>
    /// <item>Reduced GC pressure for the common case of shallow type mappings.</item>
    /// </list>
    /// </remarks>
    [InlineArray(16)]
    internal struct InlineBindings
    {
        /// <summary>First element of the inline array; the runtime uses this field to anchor the fixed-size buffer.</summary>
        private PropertyBinding _element0;

        /// <summary>
        /// Returns a read-only view of <paramref name="length"/> elements starting at <paramref name="start"/>
        /// within the inline buffer.
        /// </summary>
        /// <param name="start">Zero-based index of the first element to include in the span.</param>
        /// <param name="length">Number of elements to include; must not exceed 16.</param>
        /// <returns>
        /// A <see cref="ReadOnlySpan{T}"/> over the requested slice of the inline buffer.
        /// The span is stack-bound and must not be stored on the heap.
        /// </returns>
        public readonly ReadOnlySpan<PropertyBinding> AsSpan(int start, int length)
        {
            ReadOnlySpan<PropertyBinding> fullSpan = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in _element0), 16);
            return fullSpan.Slice(start, length);
        }
    }

}

