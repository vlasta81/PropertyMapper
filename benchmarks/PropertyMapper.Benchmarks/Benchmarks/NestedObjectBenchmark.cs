using AutoMapper;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Mapster;
using Microsoft.Extensions.Logging.Abstractions;
using PropertyMapper.Benchmarks.Models;

namespace PropertyMapper.Benchmarks.Benchmarks;

/// <summary>
/// Hot-path benchmark: maps an object that contains a nested child object.
/// Highlights the overhead of recursive type compilation and object allocation
/// for each mapper when handling object graphs.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item>AutoMapper requires explicit <c>CreateMap</c> for both the parent and the nested type.</item>
/// <item>PropertyMapper resolves nested pairs automatically during plan building.</item>
/// <item>Mapster maps nested objects by convention; explicit config is registered for both pairs to ensure parity.</item>
/// </list>
/// </remarks>
[BenchmarkCategory("NestedObject")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class NestedObjectBenchmark
{
    private PropMap _propertyMapper = null!;
    private IMapper _autoMapper = null!;
    private TypeAdapterConfig _mapsterConfig = null!;

    private PersonSource _source = null!;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _source = new PersonSource
        {
            Name = "Bob Builder",
            Age = 45,
            Address = new AddressSource
            {
                Street = "123 Main Street",
                City = "Springfield",
                PostalCode = "62701"
            }
        };

        _propertyMapper = new PropMap();
        _propertyMapper.Warmup<PersonSource, PersonTarget>();

        _autoMapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<AddressSource, AddressTarget>();
            cfg.CreateMap<PersonSource, PersonTarget>();
        }, NullLoggerFactory.Instance).CreateMapper();

        _mapsterConfig = new TypeAdapterConfig();
        _mapsterConfig.NewConfig<AddressSource, AddressTarget>();
        _mapsterConfig.NewConfig<PersonSource, PersonTarget>();
        _mapsterConfig.Compile();
    }

    [Benchmark(Baseline = true)]
    public PersonTarget Manual()
        => new()
        {
            Name = _source.Name,
            Age = _source.Age,
            Address = new AddressTarget
            {
                Street = _source.Address.Street,
                City = _source.Address.City,
                PostalCode = _source.Address.PostalCode
            }
        };

    [Benchmark]
    public PersonTarget PropertyMapper()
        => _propertyMapper.Map<PersonSource, PersonTarget>(_source);

    [Benchmark]
    public PersonTarget Mapster()
        => _source.Adapt<PersonSource, PersonTarget>(_mapsterConfig);

    [Benchmark]
    public PersonTarget AutoMapper()
        => _autoMapper.Map<PersonTarget>(_source);
}
