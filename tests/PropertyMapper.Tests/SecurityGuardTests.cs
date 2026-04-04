using Microsoft.Extensions.DependencyInjection;
using PropertyMapper.Configuration;
using PropertyMapper.Extensions;
using PropertyMapper.Masking;

namespace PropertyMapper.Tests;

// ─── Models local to this file ───────────────────────────────────────────────

file class PriceSource  { public decimal PriceUsd { get; set; } }
file class PriceTarget  { public decimal PriceUsd { get; set; } public decimal PriceEur { get; set; } }
file class RateContext  { public decimal Rate { get; set; } }

// Fake scoped service that implements IServiceProvider — hits the definitive IsAssignableFrom guard.
// A file-local class keeps its declared name in Type.Name on standard C# toolchains, but
// implementing IServiceProvider is more robust: it bypasses the string-heuristic path entirely.
file sealed class FakeScopedService : IServiceProvider
{
    public object? GetService(Type serviceType) => null;
}

// A plain value-object context — safe to use
file class TenantContext { public string TenantId { get; set; } = string.Empty; }

/// <summary>
/// Tests for the runtime security guards introduced to prevent:
/// <list type="bullet">
///   <item><description>Request-scoped state captured inside singleton <c>MapFrom</c> / <c>AfterMap</c> lambdas.</description></item>
///   <item><description><see cref="IServiceProvider"/> passed as a <c>MapWithContext</c> context.</description></item>
///   <item><description>Type-safe warmup via <see cref="PropMapBuilder.WarmupOnStartup{TIn,TOut}"/>.</description></item>
///   <item><description>Scoped <see cref="FieldMask{T}"/> registration via <see cref="PropertyMapper.Extensions.ServiceCollectionExtensions"/>.</description></item>
/// </list>
/// </summary>
public class SecurityGuardTests
{
    // ── MapFrom closure-capture guard ─────────────────────────────────────────

    /// <summary>
    /// Verifies that a <c>MapFrom</c> lambda that captures a field whose type name matches
    /// a known scoped-service heuristic throws <see cref="InvalidOperationException"/> at
    /// configuration time rather than silently embedding stale state in the singleton delegate.
    /// </summary>
    [Fact]
    public void MapFrom_ClosureCapturesKnownScopedServiceType_ThrowsAtConfigTime()
    {
        // Captures a type that implements IServiceProvider — hits the definitive guard path.
        var fakeService = new FakeScopedService();

        var ex = Assert.Throws<InvalidOperationException>((Action)(() =>
        {
            new TypePairConfiguration<SimpleSource, SimpleTarget>()
                .MapFrom(x => x.Name, src => fakeService.GetService(typeof(string))!.ToString());
        }));

        Assert.Contains("FakeScopedService", ex.Message);
        Assert.Contains("MapFrom", ex.Message);
    }

    /// <summary>
    /// Verifies that a static lambda (no closure, no captured state) does not trigger the guard.
    /// </summary>
    [Fact]
    public void MapFrom_StaticLambda_DoesNotThrow()
    {
        // Static lambda — no closure object, Target == null.
        var cfg = new TypePairConfiguration<SimpleSource, SimpleTarget>();
        cfg.MapFrom(static x => x.Name, static src => src.Name.ToUpperInvariant());
        // Reaching here means no exception was thrown.
        Assert.True(true);
    }

    /// <summary>
    /// Verifies that an instance-method delegate (non-compiler-generated closure)
    /// does not trigger the guard — only compiler-generated closures are inspected.
    /// </summary>
    [Fact]
    public void MapFrom_ExplicitInstanceMethod_DoesNotThrow()
    {
        var helper = new MappingHelper();
        var cfg = new TypePairConfiguration<SimpleSource, SimpleTarget>();
        // Delegate whose Target is a non-compiler-generated class — guard must not fire.
        cfg.MapFrom(x => x.Name, helper.GetName);
        Assert.True(true);
    }

    /// <summary>
    /// Verifies that an <c>AfterMap</c> hook that captures a known scoped-service type
    /// throws at configuration time.
    /// </summary>
    [Fact]
    public void AfterMap_ClosureCapturesKnownScopedServiceType_ThrowsAtConfigTime()
    {
        var fakeService = new FakeScopedService();

        var ex = Assert.Throws<InvalidOperationException>((Action)(() =>
        {
            new TypePairConfiguration<SimpleSource, SimpleTarget>()
                .AfterMap((src, tgt) => { _ = fakeService.GetService(typeof(string)); });
        }));

        Assert.Contains("FakeScopedService", ex.Message);
        Assert.Contains("AfterMap", ex.Message);
    }

    // ── MapFromWithContext IServiceProvider guard ─────────────────────────────

    /// <summary>
    /// Verifies that registering a context-aware setter with <see cref="IServiceProvider"/>
    /// as the context type throws <see cref="InvalidOperationException"/> at configuration time.
    /// </summary>
    [Fact]
    public void MapFromWithContext_IServiceProviderContext_ThrowsAtConfigTime()
    {
        var ex = Assert.Throws<InvalidOperationException>(() =>
            new TypePairConfiguration<PriceSource, PriceTarget>()
                .MapFromWithContext<IServiceProvider, decimal>(
                    x => x.PriceEur,
                    (src, sp) => 0m));

        Assert.Contains("IServiceProvider", ex.Message);
    }

    /// <summary>
    /// Verifies that a plain value-object context type does not trigger the guard.
    /// </summary>
    [Fact]
    public void MapFromWithContext_PlainValueObjectContext_DoesNotThrow()
    {
        new TypePairConfiguration<PriceSource, PriceTarget>()
            .MapFromWithContext<TenantContext, decimal>(
                x => x.PriceEur,
                (src, ctx) => src.PriceUsd);
        // No exception expected.
        Assert.True(true);
    }

    // ── MapWithContext call-time IServiceProvider guard ───────────────────────

    /// <summary>
    /// Verifies that passing an <see cref="IServiceProvider"/> instance at call-time to
    /// <see cref="PropMap.MapWithContext{TIn,TOut,TCtx}"/> throws <see cref="ArgumentException"/>.
    /// </summary>
    [Fact]
    public void MapWithContext_IServiceProviderPassedAtCallTime_Throws()
    {
        var mapper = new PropMap();
        IServiceProvider sp = new ServiceCollection().BuildServiceProvider();

        var ex = Assert.Throws<ArgumentException>(() =>
            mapper.MapWithContext<SimpleSource, SimpleTarget, IServiceProvider>(
                new SimpleSource { Id = 1 }, sp));

        Assert.Contains("IServiceProvider", ex.Message);
    }

    /// <summary>
    /// Verifies that passing a plain context value does not trigger the call-time guard.
    /// </summary>
    [Fact]
    public void MapWithContext_PlainContext_DoesNotThrow()
    {
        var mapper = new PropMap();
        var ctx = new TenantContext { TenantId = "acme" };
        // No context-setters registered, but the call itself must not throw.
        SimpleTarget result = mapper.MapWithContext<SimpleSource, SimpleTarget, TenantContext>(
            new SimpleSource { Id = 42, Name = "Test" }, ctx);
        Assert.Equal(42, result.Id);
    }

    // ── WarmupOnStartup type-safe fluent API ──────────────────────────────────

    /// <summary>
    /// Verifies that <see cref="PropMapBuilder.WarmupOnStartup{TIn,TOut}"/> accumulates the
    /// correct type pairs on the builder.
    /// </summary>
    [Fact]
    public void WarmupOnStartup_AccumulatesTypePairs()
    {
        var builder = new PropMapBuilder()
            .WarmupOnStartup<SimpleSource, SimpleTarget>()
            .WarmupOnStartup<PriceSource, PriceTarget>();

        Assert.Equal(2, builder.WarmupPairs.Count);
        Assert.Contains((typeof(SimpleSource), typeof(SimpleTarget)), builder.WarmupPairs);
        Assert.Contains((typeof(PriceSource), typeof(PriceTarget)), builder.WarmupPairs);
    }

    /// <summary>
    /// Verifies that <see cref="PropertyMapper.Extensions.ServiceCollectionExtensions"/>
    /// automatically registers <see cref="PropMapWarmupService"/> when
    /// <see cref="PropMapBuilder.WarmupOnStartup{TIn,TOut}"/> has been called on the builder.
    /// </summary>
    [Fact]
    public void AddPropertyMapper_WithWarmupOnStartup_RegistersWarmupHostedService()
    {
        var services = new ServiceCollection();
        services.AddPropertyMapper(builder => builder
            .WarmupOnStartup<SimpleSource, SimpleTarget>());

        // The hosted service must be present in the container.
        ServiceProvider sp = services.BuildServiceProvider();
        var hostedServices = sp.GetServices<Microsoft.Extensions.Hosting.IHostedService>();
        Assert.Single(hostedServices);
    }

    /// <summary>
    /// Verifies that <see cref="PropertyMapper.Extensions.ServiceCollectionExtensions"/>
    /// does NOT register a warmup hosted service when <see cref="PropMapBuilder.WarmupOnStartup{TIn,TOut}"/>
    /// was never called.
    /// </summary>
    [Fact]
    public void AddPropertyMapper_WithoutWarmupOnStartup_DoesNotRegisterWarmupHostedService()
    {
        var services = new ServiceCollection();
        services.AddPropertyMapper(builder => builder
            .Configure<SimpleSource, SimpleTarget>(c => c.Ignore(x => x.Name)));

        ServiceProvider sp = services.BuildServiceProvider();
        var hostedServices = sp.GetServices<Microsoft.Extensions.Hosting.IHostedService>();
        Assert.Empty(hostedServices);
    }

    // ── AddScopedFieldMask factory overload ───────────────────────────────────

    /// <summary>
    /// Verifies that <see cref="PropertyMapper.Extensions.ServiceCollectionExtensions"/>
    /// registers both <c>FieldMask&lt;T&gt;</c> and <c>IFieldMask&lt;T&gt;</c> as Scoped,
    /// and that the factory is invoked per scope.
    /// </summary>
    [Fact]
    public void AddScopedFieldMask_FactoryOverload_RegistersScopedLifetime()
    {
        var services = new ServiceCollection();
        services.AddScopedFieldMask<SimpleTarget>(
            _ => new FieldMask<SimpleTarget>("Name"));

        ServiceProvider root = services.BuildServiceProvider();

        // Each scope gets its own instance.
        using IServiceScope scope1 = root.CreateScope();
        using IServiceScope scope2 = root.CreateScope();

        var mask1 = scope1.ServiceProvider.GetRequiredService<FieldMask<SimpleTarget>>();
        var mask2 = scope2.ServiceProvider.GetRequiredService<FieldMask<SimpleTarget>>();

        Assert.NotSame(mask1, mask2);
        Assert.Contains("Name", mask1.ExcludedFields);
    }

    /// <summary>
    /// Verifies that <c>IFieldMask&lt;T&gt;</c> resolves to the same instance as
    /// <c>FieldMask&lt;T&gt;</c> within the same scope.
    /// </summary>
    [Fact]
    public void AddScopedFieldMask_FactoryOverload_IFieldMaskResolvesToSameScopedInstance()
    {
        var services = new ServiceCollection();
        services.AddScopedFieldMask<SimpleTarget>(
            _ => new FieldMask<SimpleTarget>("IsActive"));

        using ServiceProvider root = services.BuildServiceProvider();
        using IServiceScope scope = root.CreateScope();

        var concrete = scope.ServiceProvider.GetRequiredService<FieldMask<SimpleTarget>>();
        var iface    = scope.ServiceProvider.GetRequiredService<IFieldMask<SimpleTarget>>();

        Assert.Same(concrete, iface);
    }

    // ── AddScopedFieldMask static params overload ─────────────────────────────

    /// <summary>
    /// Verifies that <see cref="PropertyMapper.Extensions.ServiceCollectionExtensions"/>
    /// registers a mask with the specified excluded fields and that both
    /// <c>FieldMask&lt;T&gt;</c> and <c>IFieldMask&lt;T&gt;</c> are resolvable.
    /// </summary>
    [Fact]
    public void AddScopedFieldMask_StaticOverload_RegistersWithCorrectExcludedFields()
    {
        var services = new ServiceCollection();
        services.AddScopedFieldMask<SimpleTarget>("Name", "IsActive");

        using ServiceProvider root = services.BuildServiceProvider();
        using IServiceScope scope = root.CreateScope();

        var mask = scope.ServiceProvider.GetRequiredService<FieldMask<SimpleTarget>>();

        Assert.Contains("Name", mask.ExcludedFields);
        Assert.Contains("IsActive", mask.ExcludedFields);
    }

    /// <summary>
    /// Verifies that the static-params overload returns the same pre-built instance per scope
    /// (the mask is immutable and compiled once at registration time).
    /// </summary>
    [Fact]
    public void AddScopedFieldMask_StaticOverload_ReturnsSamePreBuiltInstance()
    {
        var services = new ServiceCollection();
        services.AddScopedFieldMask<SimpleTarget>("Name");

        using ServiceProvider root = services.BuildServiceProvider();
        using IServiceScope scope1 = root.CreateScope();
        using IServiceScope scope2 = root.CreateScope();

        var mask1 = scope1.ServiceProvider.GetRequiredService<FieldMask<SimpleTarget>>();
        var mask2 = scope2.ServiceProvider.GetRequiredService<FieldMask<SimpleTarget>>();

        // Static mask is built once — the lambda always returns the same pre-built instance.
        Assert.Same(mask1, mask2);
    }

    // ── MapThenApplyMask integration with AddScopedFieldMask ─────────────────

    /// <summary>
    /// End-to-end: resolves a scoped <see cref="FieldMask{T}"/> from DI and uses it with
    /// <see cref="PropMap.MapThenApplyMask{TIn,TOut}"/> to verify the post-map filter works.
    /// </summary>
    [Fact]
    public void MapThenApplyMask_WithScopedFieldMaskFromDI_ZerosExcludedFields()
    {
        var services = new ServiceCollection();
        services.AddPropertyMapper();
        services.AddScopedFieldMask<SimpleTarget>("Name", "IsActive");

        using ServiceProvider root = services.BuildServiceProvider();
        using IServiceScope scope = root.CreateScope();

        var mapper = scope.ServiceProvider.GetRequiredService<IPropMap>();
        var mask   = scope.ServiceProvider.GetRequiredService<IFieldMask<SimpleTarget>>();

        var source = new SimpleSource { Id = 7, Name = "Alice", IsActive = true, CreatedAt = DateTime.Today };
        SimpleTarget result = mapper.MapThenApplyMask<SimpleSource, SimpleTarget>(source, mask);

        Assert.Equal(7, result.Id);
        Assert.Equal(DateTime.Today, result.CreatedAt);
        Assert.Null(result.Name);        // zeroed by mask
        Assert.False(result.IsActive);   // zeroed by mask
    }

    // ─── Helper ──────────────────────────────────────────────────────────────

    private sealed class MappingHelper
    {
        public string GetName(SimpleSource src) => src.Name;
    }
}
