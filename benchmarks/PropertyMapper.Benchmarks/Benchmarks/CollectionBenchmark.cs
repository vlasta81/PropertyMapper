using System.Collections.Immutable;
using System.Runtime.InteropServices;
using AutoMapper;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Mapster;
using Microsoft.Extensions.Logging.Abstractions;
using PropertyMapper.Benchmarks.Models;

namespace PropertyMapper.Benchmarks.Benchmarks;

/// <summary>
/// Throughput benchmark: maps a <see cref="List{T}"/> of varying sizes.
/// Uses <c>[Params]</c> to sweep over three collection sizes (10 / 100 / 1 000 items)
/// so the report shows how each library scales with input length.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>PropertyMapper — <c>MapToList&lt;TIn, TOut&gt;</c>, single compiled delegate reused per item.</item>
/// <item>Mapster — <c>list.Adapt&lt;List&lt;WideTarget&gt;&gt;(config)</c>, IEnumerable adapter.</item>
/// <item>AutoMapper — <c>mapper.Map&lt;List&lt;WideTarget&gt;&gt;(list)</c>, built-in collection support.</item>
/// <item>Manual — LINQ <c>Select</c> + <c>ToList</c> as theoretical minimum.</item>
/// </list>
/// </remarks>
[BenchmarkCategory("Collection")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class CollectionBenchmark
{
    // -----------------------------------------------------------------------
    // Parameter
    // -----------------------------------------------------------------------

    /// <summary>Collection size swept across 10, 100, and 1 000 elements.</summary>
    [Params(10, 100, 1_000)]
    public int N { get; set; }

    // -----------------------------------------------------------------------
    // Mapper instances (pre-warmed per [Params] value)
    // -----------------------------------------------------------------------

    private PropMap _propertyMapper = null!;
    private IMapper _autoMapper = null!;
    private TypeAdapterConfig _mapsterConfig = null!;

    private List<WideSource> _data = null!;

    // -----------------------------------------------------------------------
    // Setup — called once per unique N value
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

        _propertyMapper = new PropMap();
        _propertyMapper.Warmup<WideSource, WideTarget>();

        _autoMapper = new MapperConfiguration(cfg =>
            cfg.CreateMap<WideSource, WideTarget>(), NullLoggerFactory.Instance).CreateMapper();

        _mapsterConfig = new TypeAdapterConfig();
        _mapsterConfig.NewConfig<WideSource, WideTarget>();
        _mapsterConfig.Compile();
    }

    // -----------------------------------------------------------------------
    // Benchmarks
    // -----------------------------------------------------------------------

    /// <summary>LINQ Select with hand-written initialiser — zero-overhead baseline.</summary>
    [Benchmark(Baseline = true)]
    public List<WideTarget> Manual()
    {
        var result = new List<WideTarget>(_data.Count);
        foreach (var src in _data)
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

    /// <summary>PropertyMapper <c>MapToList</c> — single compiled delegate, pre-allocated list.</summary>
    [Benchmark]
    public List<WideTarget> PropertyMapper()
        => _propertyMapper.MapToList<WideSource, WideTarget>(_data);

    /// <summary>Mapster IEnumerable adapter.</summary>
    [Benchmark]
    public List<WideTarget> Mapster()
        => _data.Adapt<List<WideTarget>>(_mapsterConfig);

    /// <summary>AutoMapper built-in collection mapping.</summary>
    [Benchmark]
    public List<WideTarget> AutoMapper()
        => _autoMapper.Map<List<WideTarget>>(_data);

    /// <summary>
    /// PropertyMapper <c>MapToList(ReadOnlySpan&lt;TIn&gt;)</c> — bypasses IEnumerable dispatch;
    /// single SetCount+AsSpan write loop, no enumerator allocation.
    /// </summary>
    [Benchmark]
    public List<WideTarget> PropertyMapper_MapToList_Span()
    {
        ReadOnlySpan<WideSource> span = CollectionsMarshal.AsSpan(_data);
        return _propertyMapper.MapToList<WideSource, WideTarget>(span);
    }

    /// <summary>
    /// PropertyMapper <c>MapToArray</c> from <see cref="List{T}"/> — internally uses
    /// <c>CollectionsMarshal.AsSpan</c> and delegates to <c>MapBatch</c>.
    /// </summary>
    [Benchmark]
    public WideTarget[] PropertyMapper_MapToArray()
        => _propertyMapper.MapToArray<WideSource, WideTarget>(_data);

    /// <summary>
    /// PropertyMapper <c>MapToImmutableArray</c> from <see cref="List{T}"/> —
    /// <c>builder.Count = n</c> + indexed write + <c>MoveToImmutable()</c> (zero-copy).
    /// </summary>
    [Benchmark]
    public ImmutableArray<WideTarget> PropertyMapper_MapToImmutableArray()
        => _propertyMapper.MapToImmutableArray<WideSource, WideTarget>(_data);
}
