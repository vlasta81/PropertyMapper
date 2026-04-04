namespace PropertyMapper
{
    /// <summary>
    /// Snapshot of runtime statistics for a <see cref="PropMap"/> instance.
    /// </summary>
    /// <param name="CachedMappers">Number of compiled mapping delegates currently in the cache.</param>
    /// <param name="CachedPlans">Number of mapping plans currently in the cache.</param>
    /// <param name="TotalMemoryBytes">Rough estimate of memory used by cached plans and delegates, in bytes.</param>
    public readonly record struct MappingStatistics(int CachedMappers, int CachedPlans, long TotalMemoryBytes);

    /// <summary>
    /// Result of a <see cref="PropMap.Validate{TIn,TOut}"/> call.
    /// </summary>
    /// <param name="IsValid">
    /// <see langword="true"/> when every writable property on <c>TOut</c> has a matching source binding.
    /// </param>
    /// <param name="UnmappedTargetProperties">
    /// Names of writable target properties that have no matching source property.
    /// Empty when <see cref="IsValid"/> is <see langword="true"/>.
    /// </param>
    public readonly record struct MappingValidationResult(bool IsValid, string[] UnmappedTargetProperties);
}
