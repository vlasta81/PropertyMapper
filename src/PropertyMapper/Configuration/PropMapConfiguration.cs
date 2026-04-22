namespace PropertyMapper.Configuration
{
    /// <summary>
    /// Controls how <see cref="PropMap"/> resolves and executes property mappings.
    /// Use object-initialiser syntax or the fluent <c>WithX()</c> API to customise behaviour.
    /// </summary>
    public sealed record PropMapConfiguration
    {
        /// <summary>Throws exception when source has properties not present on target. Default: false.</summary>
        public bool ThrowOnUnmappedProperties { get; init; } = false;

        /// <summary>Maximum depth for nested object mapping (prevents stack overflow). Default: 32.</summary>
        public int MaxMappingDepth { get; init; } = 32;

        /// <summary>
        /// Property names that are globally excluded from every <see cref="PropertyMapper.Core.MappingPlan"/>,
        /// regardless of type pair. Set via <see cref="WithGlobalIgnore(string[])"/>.
        /// <see langword="null"/> means no global ignores are active (zero-overhead hot path).
        /// </summary>
        public IReadOnlySet<string>? GlobalIgnoredProperties { get; init; }

        // Fluent API

        /// <summary>Returns a new configuration with <see cref="ThrowOnUnmappedProperties"/> set to <paramref name="enabled"/>.</summary>
        /// <param name="enabled">When <see langword="true"/>, mapping throws if any source property has no matching target property.</param>
        /// <returns>A new <see cref="PropMapConfiguration"/> with the updated value.</returns>
        public PropMapConfiguration ThrowOnUnmapped(bool enabled = true) => this with { ThrowOnUnmappedProperties = enabled };

        /// <summary>Returns a new configuration with <see cref="MaxMappingDepth"/> set to <paramref name="depth"/>.</summary>
        /// <param name="depth">Maximum nesting depth (1–256) for recursive object mapping.</param>
        /// <returns>A new <see cref="PropMapConfiguration"/> with the updated value.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="depth"/> is outside the range 1–256.</exception>
        public PropMapConfiguration WithMaxDepth(int depth)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(depth, 1);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(depth, 256);
            return this with { MaxMappingDepth = depth };
        }

        /// <summary>
        /// Returns a new configuration where the supplied property names are excluded from every
        /// <see cref="PropertyMapper.Core.MappingPlan"/>, across all type pairs.
        /// Useful for audit fields such as <c>CreatedAt</c>, <c>UpdatedAt</c>, or <c>RowVersion</c>
        /// that should never flow through any mapping.
        /// </summary>
        /// <param name="propertyNames">One or more property names to ignore globally (ordinal, case-sensitive).</param>
        /// <returns>A new <see cref="PropMapConfiguration"/> with the updated value.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="propertyNames"/> is <see langword="null"/>.</exception>
        public PropMapConfiguration WithGlobalIgnore(params string[] propertyNames)
        {
            ArgumentNullException.ThrowIfNull(propertyNames);
            HashSet<string> merged = GlobalIgnoredProperties is not null ? new HashSet<string>(GlobalIgnoredProperties, StringComparer.Ordinal) : new HashSet<string>(StringComparer.Ordinal);
            foreach (string name in propertyNames)
                merged.Add(name);
            return this with { GlobalIgnoredProperties = merged };
        }

    }

}
