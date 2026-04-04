using System.Runtime.CompilerServices;
using PropertyMapper.Configuration;
using PropertyMapper.Core;

namespace PropertyMapper
{
    public sealed partial class PropMap
    {
        #region Per-Property Configuration

        /// <summary>
        /// Throws <see cref="InvalidOperationException"/> when this instance is frozen.
        /// Called at the start of every <c>Configure</c> overload to prevent runtime
        /// reconfiguration of a singleton built by <see cref="PropMapBuilder"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when <see cref="_frozen"/> is <see langword="true"/>.
        /// </exception>
        private void ThrowIfFrozen([CallerMemberName] string? caller = null)
        {
            if (_frozen)
                throw new InvalidOperationException(
                    $"'{caller}' cannot be called on a frozen PropMap instance. " +
                    "Use PropMapBuilder.Configure<TIn,TOut>() before calling Build(), " +
                    "or register via services.AddPropertyMapper(builder => ...).");
        }

        /// <summary>
        /// Returns a fluent <see cref="TypePairConfiguration{TIn,TOut}"/> builder that governs how individual
        /// properties are handled during <typeparamref name="TIn"/>→<typeparamref name="TOut"/> mapping.
        /// Calling this method invalidates any previously compiled delegate for the type pair so that the
        /// new configuration takes effect on the very next <c>Map</c> call.
        /// </summary>
        /// <typeparam name="TIn">Source type.</typeparam>
        /// <typeparam name="TOut">Target type (must have a parameterless constructor).</typeparam>
        /// <returns>A new <see cref="TypePairConfiguration{TIn,TOut}"/> for further fluent configuration.</returns>
        /// <exception cref="InvalidOperationException">When this instance was built by <see cref="PropMapBuilder"/> and is frozen.</exception>
        internal TypePairConfiguration<TIn, TOut> Configure<TIn, TOut>() where TOut : new()
        {
            ThrowIfFrozen();
            lock (_compileLock)
            {
                TypePairKey key = new TypePairKey(typeof(TIn), typeof(TOut));
                TypePairConfiguration<TIn, TOut> config = new();
                _configStore[key] = config;
                _cache.Remove(key);
                _projectionCache.Remove(key);
                return config;
            }
        }

        /// <summary>
        /// Applies per-property configuration for the <typeparamref name="TIn"/>→<typeparamref name="TOut"/>
        /// mapping pair using the supplied <paramref name="configure"/> action, then invalidates any cached
        /// delegate so the new configuration takes effect on the very next <c>Map</c> call.
        /// </summary>
        /// <typeparam name="TIn">Source type.</typeparam>
        /// <typeparam name="TOut">Target type (must have a parameterless constructor).</typeparam>
        /// <param name="configure">Action that receives the configuration builder and applies the desired rules.</param>
        /// <exception cref="ArgumentNullException">When <paramref name="configure"/> is null.</exception>
        /// <exception cref="InvalidOperationException">When this instance was built by <see cref="PropMapBuilder"/> and is frozen.</exception>
        internal void Configure<TIn, TOut>(Action<TypePairConfiguration<TIn, TOut>> configure) where TOut : new()
        {
            ArgumentNullException.ThrowIfNull(configure);
            ThrowIfFrozen();
            lock (_compileLock)
            {
                TypePairKey key = new TypePairKey(typeof(TIn), typeof(TOut));
                TypePairConfiguration<TIn, TOut> config = new();
                configure(config);
                _configStore[key] = config;
                _cache.Remove(key);
                _projectionCache.Remove(key);
            }
        }

        #endregion
    }
}
