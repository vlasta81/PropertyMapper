using PropertyMapper.Configuration;
using PropertyMapper.Core;

namespace PropertyMapper
{
    /// <summary>
    /// High-performance object mapper using runtime IL generation.
    /// Thread-safe with lock-free hot path after initial compilation.
    /// Supports classes, structs, records, nested mappings, collections, and async operations.
    /// </summary>
    public sealed partial class PropMap : IPropMap
    {
        /// <summary>Thread-safe cache of compiled mapping delegates and their corresponding plans.</summary>
        private readonly MappingCache _cache = new();
        /// <summary>Strategy used to build mapping plans via reflection.</summary>
        private readonly IPlanBuilder _planBuilder;
        /// <summary>Write-side lock that serialises cold-path delegate compilation. Reads are always lock-free.</summary>
        private readonly Lock _compileLock = new();
        /// <summary>Active configuration that governs mapping behaviour (depth, unmapped-property handling, etc.).</summary>
        private readonly PropMapConfiguration _configuration;
        /// <summary>Shared builder for in-place mapping delegates; allocation-free once constructed.</summary>
        private static readonly InPlaceDelegateBuilder s_inPlaceBuilder = new();
        /// <summary>Per-type-pair configuration store; keyed by <see cref="TypePairKey"/>, protected by <see cref="_compileLock"/>.</summary>
        private readonly Dictionary<TypePairKey, object> _configStore = new();
        /// <summary>Cache of compiled projection expression trees; keyed by <see cref="TypePairKey"/>, protected by <see cref="_compileLock"/>.</summary>
        private readonly Dictionary<TypePairKey, object> _projectionCache = new();
        /// <summary>
        /// When <see langword="true"/> this instance was built by <see cref="PropMapBuilder"/> and
        /// runtime calls to <c>Configure</c> are rejected. Prevents cross-request configuration interference.
        /// </summary>
        private volatile bool _frozen;

        #region Constructors

        /// <summary>
        /// Creates a new <see cref="PropMap"/> instance with default <see cref="PropMapConfiguration"/>.
        /// </summary>
        public PropMap()
        {
            _configuration = new PropMapConfiguration();
            _planBuilder = new DefaultPlanBuilder(_configuration);
        }

        /// <summary>
        /// Creates a new <see cref="PropMap"/> instance with a custom <see cref="PropMapConfiguration"/>.
        /// </summary>
        /// <param name="configuration">Configuration controlling depth limits, unmapped-property handling, etc.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="configuration"/> is <see langword="null"/>.</exception>
        public PropMap(PropMapConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration);
            _configuration = configuration;
            _planBuilder = new DefaultPlanBuilder(_configuration);
        }

        /// <summary>
        /// Creates a new <see cref="PropMap"/> instance with a custom <see cref="IPlanBuilder"/> and default configuration.
        /// For advanced scenarios that require a fully custom mapping strategy.
        /// </summary>
        /// <param name="planBuilder">The custom plan-building strategy to use.</param>
        internal PropMap(IPlanBuilder planBuilder) : this(planBuilder, new PropMapConfiguration())
        {
        }

        /// <summary>
        /// Creates a new <see cref="PropMap"/> instance with a custom <see cref="IPlanBuilder"/> and a custom
        /// <see cref="PropMapConfiguration"/>.
        /// </summary>
        /// <param name="planBuilder">The custom plan-building strategy to use.</param>
        /// <param name="configuration">Configuration controlling depth limits, unmapped-property handling, etc.</param>
        /// <exception cref="ArgumentNullException">When either argument is <see langword="null"/>.</exception>
        private PropMap(IPlanBuilder planBuilder, PropMapConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(planBuilder);
            ArgumentNullException.ThrowIfNull(configuration);
            _planBuilder = planBuilder;
            _configuration = configuration;
        }

        /// <summary>
        /// Creates a frozen <see cref="PropMap"/> from a pre-built configuration store.
        /// Called exclusively by <see cref="PropMapBuilder.Build"/>.
        /// </summary>
        /// <param name="configuration">Global mapping configuration.</param>
        /// <param name="configStore">Snapshot of per-type-pair configurations assembled by the builder.</param>
        internal PropMap(PropMapConfiguration configuration, Dictionary<TypePairKey, object> configStore)
        {
            ArgumentNullException.ThrowIfNull(configuration);
            _configuration = configuration;
            _planBuilder = new DefaultPlanBuilder(configuration);
            _configStore = configStore;
            _frozen = true;
        }

        #endregion
    }
}
