using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using PropertyMapper.Benchmarks.Models;

namespace PropertyMapper.Benchmarks.Benchmarks;

/// <summary>
/// Dictionary mapping throughput benchmark: compares manual copy against
/// <c>PropMap.MapDictionary</c> for varying dictionary sizes.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item><b>Manual</b> — hand-written foreach over the source dictionary,
/// constructing each <see cref="WideTarget"/> explicitly (baseline).</item>
/// <item><b>PropertyMapper</b> — <c>MapDictionary&lt;int,WideSource,int,WideTarget&gt;</c>:
/// IL-emitted value mapper; key type is identical so the <c>KeyCast</c> branch
/// is bypassed and keys are forwarded with a direct cast.</item>
/// </list>
/// </remarks>
[BenchmarkCategory("Dictionary")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class DictionaryBenchmark
{
    // -----------------------------------------------------------------------
    // Parameter
    // -----------------------------------------------------------------------

    /// <summary>Dictionary size swept across 10, 100, and 1 000 entries.</summary>
    [Params(10, 100, 1_000)]
    public int N { get; set; }

    // -----------------------------------------------------------------------
    // Fields
    // -----------------------------------------------------------------------

    private PropMap _mapper = null!;
    private Dictionary<int, WideSource> _data = null!;

    // -----------------------------------------------------------------------
    // Setup
    // -----------------------------------------------------------------------

    [GlobalSetup]
    public void GlobalSetup()
    {
        _data = Enumerable.Range(1, N).ToDictionary(
            i => i,
            i => new WideSource
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
            });

        _mapper = new PropMap();
        _mapper.Warmup<WideSource, WideTarget>();
    }

    // -----------------------------------------------------------------------
    // Benchmarks
    // -----------------------------------------------------------------------

    /// <summary>Hand-written dictionary copy — zero-overhead baseline.</summary>
    [Benchmark(Baseline = true)]
    public Dictionary<int, WideTarget> Manual()
    {
        var result = new Dictionary<int, WideTarget>(_data.Count);
        foreach (var (key, src) in _data)
        {
            result[key] = new WideTarget
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
            };
        }
        return result;
    }

    /// <summary>
    /// <c>MapDictionary</c> — IL-emitted value mapper; same key type bypasses the
    /// <c>KeyCast</c> branch so key overhead is a single cast expression.
    /// </summary>
    [Benchmark]
    public Dictionary<int, WideTarget> PropertyMapper()
        => _mapper.MapDictionary<int, WideSource, int, WideTarget>(_data);
}
