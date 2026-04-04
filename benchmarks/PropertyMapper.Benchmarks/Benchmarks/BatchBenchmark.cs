using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using PropertyMapper.Benchmarks.Models;

namespace PropertyMapper.Benchmarks.Benchmarks;

/// <summary>
/// Batch-mapping benchmark: compares three strategies for mapping a <c>WideSource[]</c>
/// to a <c>WideTarget[]</c> for varying collection sizes.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item><b>Manual</b> — hand-written foreach loop as the zero-overhead baseline.</item>
/// <item><b>MapBatch</b> — <c>PropMap.MapBatch&lt;TIn,TOut&gt;(ReadOnlySpan&lt;TIn&gt;)</c>, allocates and returns a new array.</item>
/// <item><b>MapBatchInPlace</b> — <c>PropMap.MapBatchInPlace&lt;TIn,TOut&gt;(ReadOnlySpan&lt;TIn&gt;, Span&lt;TOut&gt;)</c>,
/// writes into a caller-owned buffer — zero array allocation.</item>
/// </list>
/// The <c>_destination</c> buffer for <c>MapBatchInPlace</c> is allocated once in <see cref="GlobalSetup"/>
/// and reused every iteration, so per-call allocation is zero (ideal for pooled scenarios).
/// </remarks>
[BenchmarkCategory("Batch")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class BatchBenchmark
{
    // -----------------------------------------------------------------------
    // Parameter
    // -----------------------------------------------------------------------

    /// <summary>Batch size swept across 10, 100 and 1 000 elements.</summary>
    [Params(10, 100, 1_000)]
    public int N { get; set; }

    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    private PropMap _mapper = null!;
    private WideSource[] _sources = null!;

    /// <summary>Pre-allocated destination buffer reused by every <c>MapBatchInPlace</c> call.</summary>
    private WideTarget[] _destination = null!;

    // -----------------------------------------------------------------------
    // Setup
    // -----------------------------------------------------------------------

    [GlobalSetup]
    public void GlobalSetup()
    {
        _sources = Enumerable.Range(1, N)
            .Select(i => new WideSource
            {
                Id = i,
                FirstName = $"First{i}",
                LastName = $"Last{i}",
                Email = $"user{i}@example.com",
                Age = 20 + (i % 50),
                Salary = 40_000m + i * 250m,
                IsActive = i % 2 == 0,
                CreatedAt = DateTime.UtcNow.AddDays(-i),
                UpdatedAt = DateTime.UtcNow.AddHours(-i),
                Department = $"Dept{i % 8}",
                ManagerId = i % 15,
                PhoneNumber = $"+1-555-{i:D4}"
            })
            .ToArray();

        _destination = new WideTarget[N];

        _mapper = new PropMap();
        _mapper.Warmup<WideSource, WideTarget>();
    }

    // -----------------------------------------------------------------------
    // Benchmarks
    // -----------------------------------------------------------------------

    /// <summary>Hand-written loop — zero-overhead baseline.</summary>
    [Benchmark(Baseline = true)]
    public WideTarget[] Manual()
    {
        var result = new WideTarget[_sources.Length];
        for (int i = 0; i < _sources.Length; i++)
        {
            var s = _sources[i];
            result[i] = new WideTarget
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Email = s.Email,
                Age = s.Age,
                Salary = s.Salary,
                IsActive = s.IsActive,
                CreatedAt = s.CreatedAt,
                UpdatedAt = s.UpdatedAt,
                Department = s.Department,
                ManagerId = s.ManagerId,
                PhoneNumber = s.PhoneNumber
            };
        }
        return result;
    }

    /// <summary>
    /// <c>MapBatch</c> — allocates and returns a new <c>WideTarget[]</c> on each call.
    /// Measures the full cost including output array allocation.
    /// </summary>
    [Benchmark]
    public WideTarget[] MapBatch()
        => _mapper.MapBatch<WideSource, WideTarget>(_sources);

    /// <summary>
    /// <c>MapBatchInPlace</c> — writes into the pre-allocated <see cref="_destination"/> buffer.
    /// Per-call heap allocation is zero; measures only delegate dispatch and property-copy overhead.
    /// </summary>
    [Benchmark]
    public void MapBatchInPlace()
        => _mapper.MapBatchInPlace<WideSource, WideTarget>(_sources, _destination);
}
