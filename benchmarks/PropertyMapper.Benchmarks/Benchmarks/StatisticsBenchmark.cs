using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using PropertyMapper.Benchmarks.Models;

namespace PropertyMapper.Benchmarks.Benchmarks;

/// <summary>
/// Benchmark for <see cref="PropMap.GetStatistics"/>.
/// </summary>
/// <remarks>
/// <para>
/// <c>GetStatistics</c> acquires the compile lock to produce a consistent
/// three-dictionary snapshot. This benchmark measures the overhead of that
/// lock acquisition on a warmed mapper where no compilation contention exists
/// (i.e. best-case scenario: lock is always immediately available).
/// </para>
/// <list type="bullet">
/// <item><b>Map_Hot</b> — steady-state <c>Map</c> call (baseline): lock-free
/// <see cref="System.Collections.Frozen.FrozenDictionary{TKey,TValue}"/> read,
/// no lock acquired.</item>
/// <item><b>GetStatistics</b> — acquires <c>_compileLock</c>, reads three
/// <c>FrozenDictionary</c> snapshots, computes memory estimate, releases lock.
/// Compare against <b>Map_Hot</b> to quantify the locking cost.</item>
/// </list>
/// </remarks>
[BenchmarkCategory("Statistics")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class StatisticsBenchmark
{
    private PropMap _mapper = null!;
    private FlatSource _source = null!;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _source = new FlatSource
        {
            Id = 1,
            Name = "Statistics",
            IsActive = true,
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        };

        _mapper = new PropMap();
        // Warm up two pairs so GetStatistics returns non-trivial counts.
        _mapper.Warmup<FlatSource, FlatTarget>();
        _mapper.Warmup<WideSource, WideTarget>();
    }

    /// <summary>
    /// Lock-free hot-path <c>Map</c> call — baseline with zero lock overhead.
    /// </summary>
    [Benchmark(Baseline = true)]
    public FlatTarget Map_Hot()
        => _mapper.Map<FlatSource, FlatTarget>(_source);

    /// <summary>
    /// Acquires the compile lock, reads three <see cref="System.Collections.Frozen.FrozenDictionary{TKey,TValue}"/>
    /// snapshots atomically, and returns a <see cref="MappingStatistics"/> value.
    /// </summary>
    [Benchmark]
    public MappingStatistics GetStatistics()
        => _mapper.GetStatistics();
}
