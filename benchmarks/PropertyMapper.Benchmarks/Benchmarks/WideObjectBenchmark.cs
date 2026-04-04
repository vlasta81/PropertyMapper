using AutoMapper;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Mapster;
using Microsoft.Extensions.Logging.Abstractions;
using PropertyMapper.Benchmarks.Models;

namespace PropertyMapper.Benchmarks.Benchmarks;

/// <summary>
/// Hot-path benchmark: maps a wide 12-property class.
/// Exercises the compiled delegate for a larger property set and shows whether
/// per-property overhead scales linearly across all three mappers.
/// </summary>
[BenchmarkCategory("WideObject")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class WideObjectBenchmark
{
    private PropMap _propertyMapper = null!;
    private IMapper _autoMapper = null!;
    private TypeAdapterConfig _mapsterConfig = null!;

    private WideSource _source = null!;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _source = new WideSource
        {
            Id = 1,
            FirstName = "Alice",
            LastName = "Wonderland",
            Email = "alice@example.com",
            Age = 30,
            Salary = 85_000.50m,
            IsActive = true,
            CreatedAt = new DateTime(2023, 1, 15, 9, 0, 0, DateTimeKind.Utc),
            UpdatedAt = new DateTime(2024, 6, 1, 12, 0, 0, DateTimeKind.Utc),
            Department = "Engineering",
            ManagerId = 7,
            PhoneNumber = "+1-555-0100"
        };

        _propertyMapper = new PropMap();
        _propertyMapper.Warmup<WideSource, WideTarget>();

        _autoMapper = new MapperConfiguration(cfg =>
            cfg.CreateMap<WideSource, WideTarget>(), NullLoggerFactory.Instance).CreateMapper();

        _mapsterConfig = new TypeAdapterConfig();
        _mapsterConfig.NewConfig<WideSource, WideTarget>();
        _mapsterConfig.Compile();
    }

    /// <summary>Hand-written 12-property copy — zero-overhead baseline.</summary>
    [Benchmark(Baseline = true)]
    public WideTarget Manual()
        => new()
        {
            Id = _source.Id,
            FirstName = _source.FirstName,
            LastName = _source.LastName,
            Email = _source.Email,
            Age = _source.Age,
            Salary = _source.Salary,
            IsActive = _source.IsActive,
            CreatedAt = _source.CreatedAt,
            UpdatedAt = _source.UpdatedAt,
            Department = _source.Department,
            ManagerId = _source.ManagerId,
            PhoneNumber = _source.PhoneNumber
        };

    /// <summary>PropertyMapper IL-emit hot path for wide (12-property) object.</summary>
    [Benchmark]
    public WideTarget PropertyMapper()
        => _propertyMapper.Map<WideSource, WideTarget>(_source);

    /// <summary>Mapster expression-tree adapter for wide object.</summary>
    [Benchmark]
    public WideTarget Mapster()
        => _source.Adapt<WideSource, WideTarget>(_mapsterConfig);

    /// <summary>AutoMapper expression-tree mapper for wide object.</summary>
    [Benchmark]
    public WideTarget AutoMapper()
        => _autoMapper.Map<WideTarget>(_source);
}
