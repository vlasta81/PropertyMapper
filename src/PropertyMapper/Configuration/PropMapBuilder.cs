using PropertyMapper.Core;

namespace PropertyMapper.Configuration
{
    /// <summary>
    /// Fluent builder that assembles a frozen, thread-safe <see cref="PropMap"/> singleton.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="PropMap"/> produced by <see cref="Build"/> is <em>frozen</em>: all
    /// type-pair configurations are applied at build time and runtime calls to
    /// <c>Configure</c> throw <see cref="InvalidOperationException"/>.
    /// This eliminates the cross-request configuration interference that would occur if
    /// <c>Configure</c> were called on a shared singleton during request processing.
    /// </para>
    /// <para>
    /// For dependency injection, prefer the
    /// <see cref="Extensions.ServiceCollectionExtensions.AddPropertyMapper(Microsoft.Extensions.DependencyInjection.IServiceCollection, System.Action{PropMapBuilder})"/>
    /// overload which uses this builder internally.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// PropMap mapper = new PropMapBuilder()
    ///     .WithConfiguration(cfg => cfg.WithMaxDepth(64))
    ///     .Configure&lt;User, UserDto&gt;(c => c.Ignore(x => x.Password))
    ///     .Configure&lt;Order, OrderDto&gt;(c => c.MapFrom(x => x.Total, src => src.Subtotal + src.Tax))
    ///     .Build();
    /// </code>
    /// </example>
    public sealed class PropMapBuilder
    {
        /// <summary>Active global configuration applied to all type pairs; defaults to a new <see cref="PropMapConfiguration"/>.</summary>
        private PropMapConfiguration _globalConfig = new();
        /// <summary>Per-type-pair configuration entries keyed by <see cref="TypePairKey"/>; last registration per pair wins.</summary>
        private readonly Dictionary<TypePairKey, object> _configStore = new();
        /// <summary>Type pairs accumulated via <see cref="WarmupOnStartup{TIn,TOut}"/> for startup pre-compilation.</summary>
        private readonly List<(Type Source, Type Target)> _warmupPairs = [];

        /// <summary>
        /// Sets the global <see cref="PropMapConfiguration"/> applied to all type pairs.
        /// Replaces any previously set value.
        /// </summary>
        /// <param name="configuration">The configuration to use.</param>
        /// <returns>This builder for method chaining.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="configuration"/> is <see langword="null"/>.</exception>
        public PropMapBuilder WithConfiguration(PropMapConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);
            _globalConfig = configuration;
            return this;
        }

        /// <summary>
        /// Sets the global <see cref="PropMapConfiguration"/> via a factory applied to the default instance.
        /// </summary>
        /// <param name="configure">
        /// A factory that receives the default <see cref="PropMapConfiguration"/> and returns
        /// the customised version. Use the fluent <c>WithX()</c> API or object-initialiser syntax.
        /// </param>
        /// <returns>This builder for method chaining.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="configure"/> is <see langword="null"/>.</exception>
        public PropMapBuilder WithConfiguration(Func<PropMapConfiguration, PropMapConfiguration> configure)
        {
            ArgumentNullException.ThrowIfNull(configure);
            _globalConfig = configure(new PropMapConfiguration());
            return this;
        }

        /// <summary>
        /// Registers per-property configuration for the <typeparamref name="TIn"/>→<typeparamref name="TOut"/>
        /// type pair. If called multiple times for the same pair, the last registration wins.
        /// </summary>
        /// <typeparam name="TIn">Source type.</typeparam>
        /// <typeparam name="TOut">Target type (must have a parameterless constructor).</typeparam>
        /// <param name="configure">Action that receives the configuration builder and applies the desired rules.</param>
        /// <returns>This builder for method chaining.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="configure"/> is <see langword="null"/>.</exception>
        public PropMapBuilder Configure<TIn, TOut>(Action<TypePairConfiguration<TIn, TOut>> configure) where TOut : new()
        {
            ArgumentNullException.ThrowIfNull(configure);
            TypePairConfiguration<TIn, TOut> config = new();
            configure(config);
            TypePairKey key = new TypePairKey(typeof(TIn), typeof(TOut));
            _configStore[key] = config;
            return this;
        }

        /// <summary>
        /// Excludes the supplied property names from every mapping plan built by this instance,
        /// across all type pairs. Useful for audit fields such as <c>CreatedAt</c>, <c>UpdatedAt</c>,
        /// or <c>RowVersion</c> that should never flow through any mapping.
        /// Equivalent to calling <see cref="PropMapConfiguration.WithGlobalIgnore(string[])"/> on
        /// the global configuration.
        /// </summary>
        /// <param name="propertyNames">One or more property names to ignore (ordinal, case-sensitive).</param>
        /// <returns>This builder for method chaining.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="propertyNames"/> is <see langword="null"/>.</exception>
        public PropMapBuilder WithGlobalIgnore(params string[] propertyNames)
        {
            ArgumentNullException.ThrowIfNull(propertyNames);
            _globalConfig = _globalConfig.WithGlobalIgnore(propertyNames);
            return this;
        }

        /// <summary>
        /// Type pairs accumulated via <see cref="WarmupOnStartup{TIn,TOut}"/>.
        /// Exposed so <see cref="Extensions.ServiceCollectionExtensions"/> can register
        /// <see cref="Extensions.PropMapWarmupService"/> automatically when the list is non-empty.
        /// </summary>
        internal IReadOnlyList<(Type Source, Type Target)> WarmupPairs => _warmupPairs;

        /// <summary>
        /// Schedules the <typeparamref name="TIn"/>→<typeparamref name="TOut"/> delegate for pre-compilation
        /// during application startup when the mapper is registered via
        /// <see cref="Extensions.ServiceCollectionExtensions.AddPropertyMapper(Microsoft.Extensions.DependencyInjection.IServiceCollection,System.Action{PropMapBuilder})"/>.
        /// Multiple calls accumulate; duplicates are silently ignored at warmup time.
        /// </summary>
        /// <typeparam name="TIn">Source type.</typeparam>
        /// <typeparam name="TOut">Target type (must have a parameterless constructor).</typeparam>
        /// <returns>This builder for method chaining.</returns>
        /// <example>
        /// <code>
        /// services.AddPropertyMapper(builder => builder
        ///     .Configure&lt;User, UserDto&gt;(c => c.Ignore(x => x.Password))
        ///     .WarmupOnStartup&lt;User, UserDto&gt;()
        ///     .WarmupOnStartup&lt;Order, OrderDto&gt;());
        /// </code>
        /// </example>
        public PropMapBuilder WarmupOnStartup<TIn, TOut>() where TOut : new()
        {
            _warmupPairs.Add((typeof(TIn), typeof(TOut)));
            return this;
        }

        /// <summary>
        /// Creates a frozen, immutable <see cref="PropMap"/> from the current builder state.
        /// The builder's internal state is snapshotted at this point; subsequent calls to
        /// <see cref="Configure{TIn,TOut}"/> or <see cref="WithConfiguration(PropMapConfiguration)"/>
        /// on this builder do not affect the returned instance.
        /// </summary>
        /// <returns>A new frozen <see cref="PropMap"/> ready for use as a singleton.</returns>
        public PropMap Build()
        {
            // Snapshot the config store so post-Build mutations to the builder don't affect the mapper.
            Dictionary<TypePairKey, object> snapshot = new(_configStore);

            // Auto-register reverse mappings for any type pair that called ReverseMap().
            foreach (KeyValuePair<TypePairKey, object> entry in _configStore)
            {
                if (entry.Value is not ITypePairConfiguration itpc || !itpc.IsReverseMapEnabled)
                    continue;

                (TypePairKey reverseKey, object reverseCfg) = itpc.CreateReverseEntry();
                if (!snapshot.ContainsKey(reverseKey))
                    snapshot[reverseKey] = reverseCfg;  // Explicit reverse config takes precedence.
            }

            return new PropMap(_globalConfig, snapshot);
        }
    }
}
