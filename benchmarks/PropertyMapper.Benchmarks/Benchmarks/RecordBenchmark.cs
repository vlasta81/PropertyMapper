using AutoMapper;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Mapster;
using Microsoft.Extensions.Logging.Abstractions;
using PropertyMapper.Benchmarks.Models;

namespace PropertyMapper.Benchmarks.Benchmarks;

/// <summary>
/// Hot-path benchmark: maps C# records (<c>OrderSource</c> → <c>OrderTarget</c>).
/// Records with mutable <c>{ get; set; }</c> properties are mapped exactly like
/// regular classes; this benchmark confirms there is no record-specific overhead.
/// </summary>
[BenchmarkCategory("Record")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class RecordBenchmark
{
    private PropMap _propertyMapper = null!;
    private IMapper _autoMapper = null!;
    private TypeAdapterConfig _mapsterConfig = null!;

    private OrderSource _source = null!;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _source = new OrderSource
        {
            OrderId = 9001,
            CustomerName = "ACME Corporation",
            TotalAmount = 1_234.56m,
            Status = "Shipped"
        };

        _propertyMapper = new PropMap();
        _propertyMapper.Warmup<OrderSource, OrderTarget>();

        _autoMapper = new MapperConfiguration(cfg =>
            cfg.CreateMap<OrderSource, OrderTarget>(), NullLoggerFactory.Instance).CreateMapper();

        _mapsterConfig = new TypeAdapterConfig();
        _mapsterConfig.NewConfig<OrderSource, OrderTarget>();
        _mapsterConfig.Compile();
    }

    /// <summary>Hand-written record copy — zero-overhead baseline.</summary>
    [Benchmark(Baseline = true)]
    public OrderTarget Manual()
        => new()
        {
            OrderId = _source.OrderId,
            CustomerName = _source.CustomerName,
            TotalAmount = _source.TotalAmount,
            Status = _source.Status
        };

    /// <summary>PropertyMapper IL-emit hot path for record types.</summary>
    [Benchmark]
    public OrderTarget PropertyMapper()
        => _propertyMapper.Map<OrderSource, OrderTarget>(_source);

    /// <summary>Mapster expression-tree record adapter.</summary>
    [Benchmark]
    public OrderTarget Mapster()
        => _source.Adapt<OrderSource, OrderTarget>(_mapsterConfig);

    /// <summary>AutoMapper expression-tree record mapper.</summary>
    [Benchmark]
    public OrderTarget AutoMapper()
        => _autoMapper.Map<OrderTarget>(_source);
}
