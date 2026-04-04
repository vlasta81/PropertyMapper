using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PropertyMapper.Configuration;
using PropertyMapper.Masking;

namespace PropertyMapper.Extensions
{
    /// <summary>
    /// Extension methods for registering PropertyMapper with dependency injection.
    /// </summary>
    /// <remarks>
    /// All overloads register <see cref="IPropMap"/> as a singleton built by <see cref="PropMapBuilder"/>,
    /// so the resulting instance is <em>frozen</em>: runtime calls to <c>Configure</c> are rejected
    /// and cross-request configuration interference is structurally impossible.
    /// The concrete <see cref="PropMap"/> type is intentionally not registered — inject
    /// <see cref="IPropMap"/> to keep consumers decoupled from the implementation.
    /// </remarks>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers <see cref="IPropMap"/> as a singleton service with default configuration.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
        public static IServiceCollection AddPropertyMapper(this IServiceCollection services)
        {
            services.TryAddSingleton<IPropMap>(_ => new PropMapBuilder().Build());
            return services;
        }

        /// <summary>
        /// Registers <see cref="IPropMap"/> as a frozen singleton with custom global configuration.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <param name="configure">
        /// A factory function that receives the default <see cref="PropMapConfiguration"/> and returns
        /// the configured instance. Use the fluent API or object-initialiser syntax.
        /// </param>
        /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
        public static IServiceCollection AddPropertyMapper(this IServiceCollection services, Func<PropMapConfiguration, PropMapConfiguration> configure)
        {
            ArgumentNullException.ThrowIfNull(configure);
            services.TryAddSingleton<IPropMap>(_ => new PropMapBuilder().WithConfiguration(configure).Build());
            return services;
        }

        /// <summary>
        /// Registers <see cref="IPropMap"/> as a frozen singleton with full builder access:
        /// global configuration and per-type-pair rules (<see cref="TypePairConfiguration{TIn,TOut}"/>).
        /// This is the recommended overload for production applications.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <param name="configure">
        /// Action that receives a <see cref="PropMapBuilder"/> and applies global configuration
        /// and/or per-type-pair rules before <see cref="PropMapBuilder.Build"/> is called.
        /// </param>
        /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
        /// <example>
        /// <code>
        /// services.AddPropertyMapper(builder => builder
        ///     .WithConfiguration(cfg => cfg.WithMaxDepth(64))
        ///     .Configure&lt;User, UserDto&gt;(c => c.Ignore(x => x.Password))
        ///     .Configure&lt;Order, OrderDto&gt;(c => c.MapFrom(x => x.Total, s => s.Subtotal + s.Tax)));
        /// </code>
        /// </example>
        public static IServiceCollection AddPropertyMapper(this IServiceCollection services, Action<PropMapBuilder> configure)
        {
            ArgumentNullException.ThrowIfNull(configure);
            PropMapBuilder builder = new();
            configure(builder);
            services.TryAddSingleton<IPropMap>(builder.Build());

            // Auto-register warmup service when WarmupOnStartup<TIn,TOut>() calls were made on the builder.
            if (builder.WarmupPairs.Count > 0)
            {
                Type[] typePairs = builder.WarmupPairs
                    .SelectMany(static p => new[] { p.Source, p.Target })
                    .ToArray();
                services.AddHostedService(sp => new PropMapWarmupService(sp.GetRequiredService<IPropMap>(), typePairs));
            }

            return services;
        }

        /// <summary>
        /// Registers <see cref="IPropMap"/> with custom global configuration and schedules warmup
        /// compilation of the specified type pairs during application startup via
        /// <see cref="PropMapWarmupService"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <param name="configure">A factory function that produces the active <see cref="PropMapConfiguration"/>.</param>
        /// <param name="warmupTypePairs">
        /// An even-length list of types in alternating source/target order
        /// (e.g. <c>typeof(Foo), typeof(FooDto), typeof(Bar), typeof(BarDto)</c>).
        /// </param>
        /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
        public static IServiceCollection AddPropertyMapper(this IServiceCollection services, Func<PropMapConfiguration, PropMapConfiguration> configure, params Type[] warmupTypePairs)
        {
            services.AddPropertyMapper(configure);
            services.AddHostedService(sp => new PropMapWarmupService(sp.GetRequiredService<IPropMap>(), warmupTypePairs));
            return services;
        }

        /// <summary>
        /// Registers <see cref="IPropMap"/> with full builder access and schedules warmup compilation
        /// of the specified type pairs during application startup.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <param name="configure">Action that configures the <see cref="PropMapBuilder"/>.</param>
        /// <param name="warmupTypePairs">
        /// An even-length list of types in alternating source/target order.
        /// </param>
        /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
        public static IServiceCollection AddPropertyMapper(this IServiceCollection services, Action<PropMapBuilder> configure, params Type[] warmupTypePairs)
        {
            services.AddPropertyMapper(configure);
            services.AddHostedService(sp => new PropMapWarmupService(sp.GetRequiredService<IPropMap>(), warmupTypePairs));
            return services;
        }

        // ── Field-mask scoped registration ───────────────────────────────────────

        /// <summary>
        /// Registers <see cref="FieldMask{T}"/> (and <see cref="IFieldMask{T}"/>) as a
        /// <c>Scoped</c> service using a per-request factory delegate.
        /// </summary>
        /// <remarks>
        /// Use this overload when the excluded fields depend on the current request context
        /// (e.g. the authenticated user's permissions). The factory receives the request-scoped
        /// <see cref="IServiceProvider"/> so it can resolve scoped services such as
        /// <c>ICurrentUser</c> or <c>IAuthorizationService</c>.
        /// <para>
        /// Both <c>FieldMask&lt;T&gt;</c> and <c>IFieldMask&lt;T&gt;</c> are registered so
        /// either type can be injected. <see cref="PropMap.MapThenApplyMask{TIn,TOut}"/> uses
        /// the concrete type for its zero-reflection fast path.
        /// </para>
        /// </remarks>
        /// <typeparam name="T">The target DTO type whose fields this mask controls.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <param name="factory">
        /// Per-request factory that resolves scoped services and returns a
        /// <see cref="FieldMask{T}"/> configured for the current user.
        /// </param>
        /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
        /// <example>
        /// <code>
        /// services.AddScopedFieldMask&lt;UserDto&gt;(sp =>
        /// {
        ///     var user = sp.GetRequiredService&lt;ICurrentUser&gt;();
        ///     return user.IsAdmin
        ///         ? new FieldMask&lt;UserDto&gt;()                           // no fields hidden
        ///         : new FieldMask&lt;UserDto&gt;("Salary", "NationalId");    // sensitive fields zeroed
        /// });
        /// </code>
        /// </example>
        public static IServiceCollection AddScopedFieldMask<T>(this IServiceCollection services, Func<IServiceProvider, FieldMask<T>> factory)
        {
            ArgumentNullException.ThrowIfNull(factory);
            services.TryAddScoped<FieldMask<T>>(factory);
            services.TryAddScoped<IFieldMask<T>>(sp => sp.GetRequiredService<FieldMask<T>>());
            return services;
        }

        /// <summary>
        /// Registers <see cref="FieldMask{T}"/> (and <see cref="IFieldMask{T}"/>) as a
        /// <c>Scoped</c> service with a fixed set of excluded property names.
        /// </summary>
        /// <remarks>
        /// Use this overload when the same fields are always excluded regardless of the current
        /// user (e.g. a field that should never be exposed in a particular endpoint).
        /// The <see cref="FieldMask{T}"/> instance is immutable after construction
        /// (clearing delegates are compiled once); it is nonetheless registered as <c>Scoped</c>
        /// to maintain a consistent lifetime model and to allow easy migration to the
        /// <see cref="AddScopedFieldMask{T}(IServiceCollection,Func{IServiceProvider,FieldMask{T}})"/>
        /// factory overload when dynamic field selection is needed in the future.
        /// </remarks>
        /// <typeparam name="T">The target DTO type whose fields this mask controls.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the service to.</param>
        /// <param name="excludedPropertyNames">Names of properties to zero out after mapping.</param>
        /// <returns>The same <see cref="IServiceCollection"/> for chaining.</returns>
        /// <example>
        /// <code>
        /// services.AddScopedFieldMask&lt;UserDto&gt;("Password", "SecurityStamp");
        /// </code>
        /// </example>
        public static IServiceCollection AddScopedFieldMask<T>(this IServiceCollection services, params string[] excludedPropertyNames)
        {
            ArgumentNullException.ThrowIfNull(excludedPropertyNames);
            // Build the mask (and compile its clearers) once at registration time.
            FieldMask<T> mask = new(excludedPropertyNames);
            services.TryAddScoped<FieldMask<T>>(_ => mask);
            services.TryAddScoped<IFieldMask<T>>(sp => sp.GetRequiredService<FieldMask<T>>());
            return services;
        }
    }
}

