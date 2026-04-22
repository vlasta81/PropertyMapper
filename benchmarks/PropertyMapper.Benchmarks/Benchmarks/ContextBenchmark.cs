using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using PropertyMapper.Benchmarks.Models;
using PropertyMapper.Configuration;
using PropertyMapper.Extensions;

namespace PropertyMapper.Benchmarks.Benchmarks;

/// <summary>
/// Hot-path benchmark for context-aware mapping via
/// <see cref="PropMap.MapWithContext{TIn,TOut,TCtx}"/>.
/// </summary>
/// <remarks>
/// <para>
/// <c>MapWithContext</c> first runs the pre-compiled IL delegate (same as <c>Map</c>),
/// then invokes any context-aware setters registered with
/// <see cref="TypePairConfiguration{TIn,TOut}.MapFromWithContext{TCtx,TProp}"/>.
/// </para>
/// <list type="bullet">
/// <item><b>Map_NoContext</b> — plain <c>Map&lt;TIn,TOut&gt;</c> call (baseline): IL delegate only,
/// no context overhead.</item>
/// <item><b>MapWithContext_NoSetters</b> — <c>MapWithContext</c> with a plain value-object
/// context and <em>no registered</em> context setters; measures the guard + lookup overhead
/// over the baseline.</item>
/// <item><b>MapWithContext_OneContextSetter</b> — one <c>MapFromWithContext</c> setter is
/// registered and invoked; shows the cost of a single compiled context-setter call.</item>
/// <item><b>MapWithContext_ThreeContextSetters</b> — three setters registered; demonstrates
/// linear scaling with setter count.</item>
/// </list>
/// </remarks>
[BenchmarkCategory("Context")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class ContextBenchmark
{
    // -----------------------------------------------------------------------
    // Context record — a lightweight plain value object
    // -----------------------------------------------------------------------

    /// <summary>Per-call context carrying an exchange rate used for currency conversion.</summary>
    public sealed record RateContext(decimal EurRate, decimal GbpRate, decimal JpyRate);

    // -----------------------------------------------------------------------
    // Benchmark models
    // -----------------------------------------------------------------------

    private WideSource _source = null!;
    private RateContext _ctx = null!;

    // -----------------------------------------------------------------------
    // Mapper instances (each pre-warmed in GlobalSetup)
    // -----------------------------------------------------------------------

    private PropMap _mapperNoContext = null!;
    private PropMap _mapperNoSetters = null!;
    private PropMap _mapperOneSetter = null!;
    private PropMap _mapperThreeSetters = null!;

    // -----------------------------------------------------------------------
    // Setup
    // -----------------------------------------------------------------------

    [GlobalSetup]
    public void GlobalSetup()
    {
        _source = new WideSource
        {
            Id = 1,
            FirstName = "Context",
            LastName = "Benchmark",
            Email = "ctx@bench.io",
            Age = 35,
            Salary = 80_000m,
            IsActive = true,
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            UpdatedAt = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc),
            Department = "Engineering",
            ManagerId = 5,
            PhoneNumber = "+1-555-0001"
        };

        _ctx = new RateContext(EurRate: 0.92m, GbpRate: 0.79m, JpyRate: 157.5m);

        // Plain mapper — no context configuration
        _mapperNoContext = new PropMap();
        _mapperNoContext.Warmup<WideSource, WideTarget>();

        // Mapper configured for MapWithContext but no context setters registered
        _mapperNoSetters = new PropMapBuilder()
            .Configure<WideSource, WideTarget>(c => c.Ignore(x => x.ManagerId))
            .Build();

        // One context setter: derives Age from context (simulates per-request adjustment)
        _mapperOneSetter = new PropMapBuilder()
            .Configure<WideSource, WideTarget>(c =>
                c.MapFromWithContext<RateContext, int>(
                    x => x.ManagerId,
                    (src, ctx) => (int)(src.Salary * ctx.EurRate / 10_000m)))
            .Build();

        // Three context setters: Id, Age, ManagerId all derived from context
        _mapperThreeSetters = new PropMapBuilder()
            .Configure<WideSource, WideTarget>(c =>
            {
                c.MapFromWithContext<RateContext, int>(
                    x => x.Id,
                    (src, ctx) => (int)(src.Salary * ctx.EurRate / 100_000m));
                c.MapFromWithContext<RateContext, int>(
                    x => x.Age,
                    (src, ctx) => (int)(src.Salary * ctx.GbpRate / 10_000m));
                c.MapFromWithContext<RateContext, int>(
                    x => x.ManagerId,
                    (src, ctx) => (int)(src.Salary * ctx.JpyRate / 1_000_000m));
            })
            .Build();
    }

    // -----------------------------------------------------------------------
    // Benchmarks
    // -----------------------------------------------------------------------

    /// <summary>
    /// Plain <c>Map&lt;TIn,TOut&gt;</c> — IL delegate only, no context guard or lookup.
    /// Zero-overhead baseline.
    /// </summary>
    [Benchmark(Baseline = true)]
    public WideTarget Map_NoContext()
        => _mapperNoContext.Map<WideSource, WideTarget>(_source);

    /// <summary>
    /// <c>MapWithContext</c> with a plain value-object context and no registered setters.
    /// Measures the cost of the forbidden-type guard + <c>_configStore</c> lookup added
    /// on top of the baseline <c>Map</c> call.
    /// </summary>
    [Benchmark]
    public WideTarget MapWithContext_NoSetters()
        => _mapperNoSetters.MapWithContext<WideSource, WideTarget, RateContext>(_source, _ctx);

    /// <summary>
    /// <c>MapWithContext</c> with one registered context setter.
    /// Measures the overhead of a single compiled <c>Action&lt;TIn,TOut,TCtx&gt;</c> invocation
    /// on top of the base IL mapping.
    /// </summary>
    [Benchmark]
    public WideTarget MapWithContext_OneContextSetter()
        => _mapperOneSetter.MapWithContext<WideSource, WideTarget, RateContext>(_source, _ctx);

    /// <summary>
    /// <c>MapWithContext</c> with three registered context setters.
    /// Shows that per-setter cost scales linearly with setter count.
    /// </summary>
    [Benchmark]
    public WideTarget MapWithContext_ThreeContextSetters()
        => _mapperThreeSetters.MapWithContext<WideSource, WideTarget, RateContext>(_source, _ctx);
}
