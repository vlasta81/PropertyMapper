using PropertyMapper.Configuration;
using PropertyMapper.Core;

namespace PropertyMapper
{
    public sealed partial class PropMap
    {
        #region Context Mapping

        /// <summary>
        /// Maps <paramref name="source"/> to a new <typeparamref name="TOut"/> instance, then applies
        /// all context-aware setters registered via
        /// <see cref="TypePairConfiguration{TIn,TOut}.MapFromWithContext{TCtx,TProp}"/> using the
        /// supplied <paramref name="context"/> value.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The base IL mapping (name-matched properties) executes first; context-aware setters run after.
        /// <paramref name="context"/> is passed on each call and is <b>never</b> compiled into the cached
        /// IL delegate — the mapper remains stateless and thread-safe.
        /// </para>
        /// <para>
        /// Typical uses: currency conversion, tenant-specific transforms, per-request user preferences.
        /// </para>
        /// </remarks>
        /// <typeparam name="TIn">Source type.</typeparam>
        /// <typeparam name="TOut">Target type (must have a parameterless constructor).</typeparam>
        /// <typeparam name="TCtx">Context type — passed per call, not cached.</typeparam>
        /// <param name="source">Source object to map.</param>
        /// <param name="context">Per-call context passed to all registered context-aware setters.</param>
        /// <returns>A new <typeparamref name="TOut"/> with all matched properties and context-derived values applied.</returns>
        /// <exception cref="ArgumentNullException">When <paramref name="source"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">When <paramref name="context"/> is an <see cref="IServiceProvider"/> or <see cref="Microsoft.Extensions.DependencyInjection.IServiceScopeFactory"/>.</exception>
        public TOut MapWithContext<TIn, TOut, TCtx>(TIn source, TCtx context) where TOut : new()
        {
            ArgumentNullException.ThrowIfNull(source);
            if (ClosureInspector.IsForbiddenContextType(typeof(TCtx)))
                throw new ArgumentException(
                    "IServiceProvider and IServiceScopeFactory must not be passed as a MapWithContext context. " +
                    "Resolve the required values before calling MapWithContext and pass a " +
                    "plain value object (exchange rates, tenant ID, user preferences, etc.) instead.",
                    nameof(context));

            TOut result = Map<TIn, TOut>(source);

            TypePairKey key = new TypePairKey(typeof(TIn), typeof(TOut));
            Action<TIn, TOut, TCtx>? contextDelegate = null;

            // _frozen is a volatile bool set once by PropMapBuilder.Build().
            // After freezing, _configStore is immutable — no lock needed on the hot path.
            if (_frozen)
            {
                if (_configStore.TryGetValue(key, out object? cfg) && cfg is TypePairConfiguration<TIn, TOut> typedCfg)
                    contextDelegate = typedCfg.BuildContextDelegate<TCtx>();
            }
            else
            {
                lock (_compileLock)
                {
                    if (_configStore.TryGetValue(key, out object? cfg) && cfg is TypePairConfiguration<TIn, TOut> typedCfg)
                        contextDelegate = typedCfg.BuildContextDelegate<TCtx>();
                }
            }

            contextDelegate?.Invoke(source, result, context);
            return result;
        }

        #endregion
    }
}
