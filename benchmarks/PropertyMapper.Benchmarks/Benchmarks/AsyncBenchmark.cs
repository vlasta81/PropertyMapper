using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using PropertyMapper.Benchmarks.Models;

namespace PropertyMapper.Benchmarks.Benchmarks;

/// <summary>
/// Async mapping throughput benchmark: compares sequential vs parallel collection mapping
/// for varying collection sizes.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item><b>ManualSequential</b> — hand-written synchronous foreach loop — absolute lower
/// bound; used only to show the raw cost of allocation + field-copy work.</item>
/// <item><b>ManualAsync_TaskRun</b> — same hand-written loop wrapped in <c>Task.Run</c> —
/// fair baseline for all async methods because it includes the thread-pool dispatch overhead
/// that every <c>Task.Run</c>-based implementation must pay.</item>
/// <item><b>MapAsync_Sequential</b> — <c>MapAsync&lt;TIn,TOut&gt;(IEnumerable)</c>:
/// single <c>Task.Run</c> wrapping a tight loop on the thread-pool.</item>
/// <item><b>MapParallelAsync</b> — <c>MapParallelAsync&lt;TIn,TOut&gt;</c>: indexed parallel
/// writes via <c>Parallel.ForAsync</c> + <c>AsyncMappingWorker</c> struct
/// (zero-allocation hot path).</item>
/// </list>
/// <para>
/// Cross-over point: for small N, <c>MapAsync_Sequential</c> wins because thread-dispatch
/// overhead dominates; for large N, <c>MapParallelAsync</c> should scale with CPU count.
/// </para>
/// </remarks>
[BenchmarkCategory("Async")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class AsyncBenchmark
{
    // -----------------------------------------------------------------------
    // Parameter
    // -----------------------------------------------------------------------

    /// <summary>Collection size swept across 10, 100, and 1 000 elements.</summary>
    [Params(10, 100, 1_000)]
    public int N { get; set; }

    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    private PropMap _mapper = null!;
    private List<WideSource> _data = null!;

    // -----------------------------------------------------------------------
    // Setup
    // -----------------------------------------------------------------------

    [GlobalSetup]
    public void GlobalSetup()
    {
        _data = Enumerable.Range(1, N)
            .Select(i => new WideSource
            {
                Id = i,
                FirstName = $"First{i}",
                LastName = $"Last{i}",
                Email = $"user{i}@example.com",
                Age = 20 + (i % 40),
                Salary = 50_000m + i * 100m,
                IsActive = i % 2 == 0,
                CreatedAt = DateTime.UtcNow.AddDays(-i),
                UpdatedAt = DateTime.UtcNow.AddHours(-i),
                Department = $"Dept{i % 5}",
                ManagerId = i % 10,
                PhoneNumber = $"+1-555-{i:D4}"
            })
            .ToList();

        _mapper = new PropMap();
        _mapper.Warmup<WideSource, WideTarget>();
    }

    // -----------------------------------------------------------------------
    // Benchmarks
    // -----------------------------------------------------------------------

    /// <summary>
    /// Hand-written synchronous loop — absolute lower bound showing pure allocation
    /// and field-copy cost without any Task overhead.
    /// </summary>
    [Benchmark(Baseline = true)]
    public List<WideTarget> ManualSequential()
    {
        List<WideTarget> result = new List<WideTarget>(_data.Count);
        foreach (WideSource src in _data)
        {
            result.Add(new WideTarget
            {
                Id = src.Id,
                FirstName = src.FirstName,
                LastName = src.LastName,
                Email = src.Email,
                Age = src.Age,
                Salary = src.Salary,
                IsActive = src.IsActive,
                CreatedAt = src.CreatedAt,
                UpdatedAt = src.UpdatedAt,
                Department = src.Department,
                ManagerId = src.ManagerId,
                PhoneNumber = src.PhoneNumber
            });
        }
        return result;
    }

    /// <summary>
    /// Same hand-written loop offloaded via <c>Task.Run</c> — fair baseline for all
    /// async benchmarks because it captures the thread-pool dispatch overhead that every
    /// <c>Task.Run</c>-based implementation must pay, without any PropertyMapper code.
    /// </summary>
    [Benchmark]
    public async Task<List<WideTarget>> ManualAsync_TaskRun()
        => await Task.Run(() =>
        {
            List<WideTarget> result = new List<WideTarget>(_data.Count);
            foreach (WideSource src in _data)
            {
                result.Add(new WideTarget
                {
                    Id = src.Id,
                    FirstName = src.FirstName,
                    LastName = src.LastName,
                    Email = src.Email,
                    Age = src.Age,
                    Salary = src.Salary,
                    IsActive = src.IsActive,
                    CreatedAt = src.CreatedAt,
                    UpdatedAt = src.UpdatedAt,
                    Department = src.Department,
                    ManagerId = src.ManagerId,
                    PhoneNumber = src.PhoneNumber
                });
            }
            return result;
        }).ConfigureAwait(false);

    /// <summary>
    /// Sequential collection mapping on a thread-pool thread.
    /// Single <c>Task.Run</c> amortises scheduling cost across all N items.
    /// Compare against <see cref="ManualAsync_TaskRun"/> for a fair overhead reading.
    /// </summary>
    [Benchmark]
    public async Task<List<WideTarget>> MapAsync_Sequential()
        => await _mapper.MapAsync<WideSource, WideTarget>(_data).ConfigureAwait(false);

    /// <summary>
    /// Parallel collection mapping via <c>Parallel.ForAsync</c>.
    /// Zero-allocation struct worker writes directly into the pre-sized output
    /// list via <c>CollectionsMarshal.AsSpan</c>.
    /// </summary>
    [Benchmark]
    public async Task<List<WideTarget>> MapParallelAsync()
        => await _mapper.MapParallelAsync<WideSource, WideTarget>(_data).ConfigureAwait(false);
}
