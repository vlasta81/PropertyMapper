    using AutoMapper;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Mapster;
using Microsoft.Extensions.Logging.Abstractions;
using PropertyMapper.Benchmarks.Models;

namespace PropertyMapper.Benchmarks.Benchmarks;

/// <summary>
/// Hot-path benchmark: maps value types (<c>PointSource</c> → <c>PointTarget</c>).
/// AutoMapper and Mapster are included for a fair cross-library comparison.
/// </summary>
[BenchmarkCategory("Struct")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class StructBenchmark
{
    private PropMap _propertyMapper = null!;
    private IMapper _autoMapper = null!;
    private TypeAdapterConfig _mapsterConfig = null!;

    private PointSource _source;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _source = new PointSource { X = 1.5f, Y = 2.7f, Z = -0.3f };

        _propertyMapper = new PropMap();
        _propertyMapper.Warmup<PointSource, PointTarget>();

        _autoMapper = new MapperConfiguration(cfg =>
            cfg.CreateMap<PointSource, PointTarget>(), NullLoggerFactory.Instance).CreateMapper();

        _mapsterConfig = new TypeAdapterConfig();
        _mapsterConfig.NewConfig<PointSource, PointTarget>();
        _mapsterConfig.Compile();
    }

    /// <summary>Hand-written struct copy — zero-overhead reference.</summary>
    [Benchmark(Baseline = true)]
    public PointTarget Manual()
        => new() { X = _source.X, Y = _source.Y, Z = _source.Z };

    /// <summary>PropertyMapper struct hot-path via <c>Map&lt;PointSource, PointTarget&gt;</c>.</summary>
    [Benchmark]
    public PointTarget PropertyMapper()
        => _propertyMapper.Map<PointSource, PointTarget>(_source);

    /// <summary>Mapster expression-tree struct adapter.</summary>
    [Benchmark]
    public PointTarget Mapster()
        => _source.Adapt<PointSource, PointTarget>(_mapsterConfig);

    /// <summary>AutoMapper expression-tree struct mapper.</summary>
    [Benchmark]
    public PointTarget AutoMapper()
        => _autoMapper.Map<PointTarget>(_source);
}
