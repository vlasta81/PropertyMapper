using System.Reflection;

namespace PropertyMapper.Core
{
    /// <summary>
    /// Provides compile-time-safe accessors for <see cref="Nullable{T}"/> property getters,
    /// replacing fragile string-based <c>GetMethod("get_HasValue")</c> / <c>GetMethod("get_Value")</c> calls.
    /// </summary>
    internal static class NullableInfo
    {
        /// <summary>Returns the <c>get_HasValue</c> accessor for the given <c>Nullable&lt;T&gt;</c> type.</summary>
        internal static MethodInfo GetHasValueGetter(Type nullableType) => nullableType.GetProperty(nameof(Nullable<int>.HasValue))!.GetGetMethod()!;

        /// <summary>Returns the <c>get_Value</c> accessor for the given <c>Nullable&lt;T&gt;</c> type.</summary>
        internal static MethodInfo GetValueGetter(Type nullableType) => nullableType.GetProperty(nameof(Nullable<int>.Value))!.GetGetMethod()!;
    }
}
