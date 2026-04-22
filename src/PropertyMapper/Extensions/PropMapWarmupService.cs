using Microsoft.Extensions.Hosting;

namespace PropertyMapper.Extensions
{
    /// <summary>
    /// <see cref="IHostedService"/> that pre-compiles mapping delegates for the specified type pairs
    /// during application startup, eliminating first-request JIT and IL-emit overhead.
    /// </summary>
    internal sealed class PropMapWarmupService : IHostedService
    {
        /// <summary>The mapper whose delegates will be pre-compiled during startup.</summary>
        private readonly IPropMap _mapper;
        /// <summary>Flat, interleaved array of source/target type pairs to warm up (always even-length).</summary>
        private readonly Type[] _typePairs;

        /// <summary>
        /// Initializes a new <see cref="PropMapWarmupService"/>.
        /// </summary>
        /// <param name="mapper">The <see cref="IPropMap"/> to warm up.</param>
        /// <param name="typePairs">Even-length array of alternating source and target types.</param>
        public PropMapWarmupService(IPropMap mapper, Type[] typePairs)
        {
            _mapper = mapper;
            _typePairs = typePairs;
        }

        /// <summary>
        /// Invoked by the host during application startup. Calls
        /// <see cref="PropMap.WarmupBatch"/> to pre-compile all registered type pairs synchronously.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token provided by the host (not observed; warmup is fast).</param>
        /// <returns>A completed <see cref="Task"/>.</returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _mapper.WarmupBatch(_typePairs);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Invoked by the host during graceful shutdown. No cleanup is required.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token provided by the host.</param>
        /// <returns>A completed <see cref="Task"/>.</returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }

}

