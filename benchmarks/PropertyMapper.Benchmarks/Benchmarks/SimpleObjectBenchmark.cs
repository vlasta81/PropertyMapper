using AutoMapper;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Mapster;
using Microsoft.Extensions.Logging.Abstractions;
using PropertyMapper.Benchmarks.Models;

namespace PropertyMapper.Benchmarks.Benchmarks;

/// <summary>
/// Hot-path benchmark: maps a flat 4-property class.
/// All mappers are pre-warmed in <see cref="GlobalSetup"/> so every measurement
/// reflects only the steady-state cost of a single mapping call.
/// </summary>
/// <remarks>
/// Compared libraries:
/// <list type="bullet">
/// <item><b>Manual</b> — hand-written property assignment (zero-overhead baseline).</item>
/// <item><b>PropertyMapper</b> — IL-emit delegate, lock-free cache hot path.</item>
/// <item><b>Mapster 10</b> — expression-tree based, convention-driven.</item>
/// <item><b>AutoMapper 16</b> — expression-tree based, explicit configuration required.</item>
/// </list>
/// </remarks>
[BenchmarkCategory("SimpleObject")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class SimpleObjectBenchmark
{
    // -----------------------------------------------------------------------
    // Mapper instances (pre-warmed once in GlobalSetup)
    // -----------------------------------------------------------------------

    private PropMap _propertyMapper = null!;
    private IMapper _autoMapper = null!;
    private TypeAdapterConfig _mapsterConfig = null!;

    // -----------------------------------------------------------------------
    // Input data
    // -----------------------------------------------------------------------

    private FlatSource _source = null!;

    // -----------------------------------------------------------------------
    // Setup
    // -----------------------------------------------------------------------

    [GlobalSetup]
    public void GlobalSetup()
    {
        _source = new FlatSource
        {
            Id = 42,
            Name = "Alice Wonderland",
            IsActive = true,
            CreatedAt = new DateTime(2024, 6, 1, 12, 0, 0, DateTimeKind.Utc)
        };

        // PropertyMapper — warm up compiles the IL delegate
        _propertyMapper = new PropMap();
        _propertyMapper.Warmup<FlatSource, FlatTarget>();

        // AutoMapper — CreateMapper() compiles expression trees
        _autoMapper = new MapperConfiguration(cfg =>
            cfg.CreateMap<FlatSource, FlatTarget>(), NullLoggerFactory.Instance).CreateMapper();

        // Mapster — Compile() pre-builds all registered adapters
        _mapsterConfig = new TypeAdapterConfig();
        _mapsterConfig.NewConfig<FlatSource, FlatTarget>();
        _mapsterConfig.Compile();
    }

    // -----------------------------------------------------------------------
    // Benchmarks
    // -----------------------------------------------------------------------

    /// <summary>Hand-written property copy — zero-overhead baseline.</summary>
    [Benchmark(Baseline = true)]
    public FlatTarget Manual()
        => new()
        {
            Id = _source.Id,
            Name = _source.Name,
            IsActive = _source.IsActive,
            CreatedAt = _source.CreatedAt
        };

    /// <summary>PropertyMapper IL-emit hot path.</summary>
    [Benchmark]
    public FlatTarget PropertyMapper()
        => _propertyMapper.Map<FlatSource, FlatTarget>(_source);

    /// <summary>Mapster expression-tree adapter.</summary>
    [Benchmark]
    public FlatTarget Mapster()
        => _source.Adapt<FlatSource, FlatTarget>(_mapsterConfig);

    /// <summary>AutoMapper expression-tree mapper.</summary>
    [Benchmark]
    public FlatTarget AutoMapper()
        => _autoMapper.Map<FlatTarget>(_source);
}
