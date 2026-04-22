using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using PropertyMapper.Benchmarks.Models;

namespace PropertyMapper.Benchmarks.Benchmarks;

/// <summary>
/// Cold-path benchmark: measures the cost of pre-compiling mapping delegates
/// via <see cref="PropMap.Warmup{TIn,TOut}"/> and
/// <see cref="PropMap.WarmupBatch(Type[])"/>.
/// </summary>
/// <remarks>
/// <para>
/// Each benchmark method creates a brand-new <see cref="PropMap"/> instance so
/// that no delegates are cached, then calls the warmup API under test.
/// This intentionally includes the full IL-emit cost incurred before the first
/// mapping call (the cold path).
/// </para>
/// <list type="bullet">
/// <item><description><b>Warmup_SinglePair</b> — compiles one <see cref="WideSource"/>→<see cref="WideTarget"/> delegate.</description></item>
/// <item><description><b>WarmupBatch_TwoPairs</b> — compiles two delegates in a single sequential pass; amortises lock acquisition over two pairs.</description></item>
/// <item><description><b>WarmupBatch_FourPairs</b> — compiles four delegates in a single sequential pass; demonstrates linear per-pair scaling.</description></item>
/// </list>
/// </remarks>
[BenchmarkCategory("Warmup")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class WarmupBenchmark
{
    // -----------------------------------------------------------------------
    // Benchmarks — each creates a fresh PropMap so the cold path fires
    // -----------------------------------------------------------------------

    /// <summary>
    /// Compiles a single <see cref="WideSource"/>→<see cref="WideTarget"/> delegate.
    /// This is the baseline; represents the minimum overhead of one IL-emit compilation.
    /// </summary>
    [Benchmark(Baseline = true)]
    public PropMap Warmup_SinglePair()
    {
        var mapper = new PropMap();
        mapper.Warmup<WideSource, WideTarget>();
        return mapper;
    }

    /// <summary>
    /// Compiles two type-pair delegates in one <see cref="PropMap.WarmupBatch(Type[])"/> call.
    /// The sequential for-loop in <c>WarmupBatch</c> avoids thread-pool overhead
    /// while still amortising lock acquisition over multiple pairs.
    /// </summary>
    [Benchmark]
    public PropMap WarmupBatch_TwoPairs()
    {
        var mapper = new PropMap();
        mapper.WarmupBatch(
            typeof(WideSource), typeof(WideTarget),
            typeof(FlatSource), typeof(FlatTarget));
        return mapper;
    }

    /// <summary>
    /// Compiles four type-pair delegates in one <see cref="PropMap.WarmupBatch(Type[])"/> call.
    /// Demonstrates that per-pair cost remains constant relative to single-pair warmup.
    /// </summary>
    [Benchmark]
    public PropMap WarmupBatch_FourPairs()
    {
        var mapper = new PropMap();
        mapper.WarmupBatch(
            typeof(WideSource), typeof(WideTarget),
            typeof(FlatSource), typeof(FlatTarget),
            typeof(PersonSource), typeof(PersonTarget),
            typeof(OrderSource), typeof(OrderTarget));
        return mapper;
    }
}
