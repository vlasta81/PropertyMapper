using AutoMapper;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Mapster;
using Microsoft.Extensions.Logging.Abstractions;
using PropertyMapper.Benchmarks.Models;

namespace PropertyMapper.Benchmarks.Benchmarks;

/// <summary>
/// Self-copy (clone) benchmark: copies a flat 4-property object to a new instance
/// of the <em>same</em> type.
/// </summary>
/// <remarks>
/// PropertyMapper exposes <c>Clone&lt;T&gt;</c> as a first-class API backed by the same
/// IL-emitted delegate used by <c>Map&lt;TIn, TOut&gt;</c>.
/// This benchmark answers: how much overhead does the mapper add over a manual
/// field-by-field copy for the most common shallow-clone scenario?
/// <para>
/// Compared libraries:
/// </para>
/// <list type="bullet">
/// <item><b>Manual</b> — object initialiser with explicit property assignments (baseline).</item>
/// <item><b>PropertyMapper.Clone</b> — <c>Clone&lt;FlatSource&gt;()</c>, same-type IL delegate.</item>
/// <item><b>Mapster</b> — <c>source.Adapt&lt;FlatSource&gt;(config)</c>, expression-tree adapter.</item>
/// <item><b>AutoMapper</b> — <c>mapper.Map&lt;FlatSource&gt;(source)</c>, expression-tree mapper.</item>
/// </list>
/// </remarks>
[BenchmarkCategory("Clone")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class CloneBenchmark
{
    // -----------------------------------------------------------------------
    // Mapper instances (pre-warmed in GlobalSetup)
    // -----------------------------------------------------------------------

    private PropMap _propertyMapper = null!;
    private IMapper _autoMapper = null!;
    private TypeAdapterConfig _mapsterConfig = null!;

    private FlatSource _source = null!;

    // -----------------------------------------------------------------------
    // Setup
    // -----------------------------------------------------------------------

    [GlobalSetup]
    public void GlobalSetup()
    {
        _source = new FlatSource
        {
            Id = 1,
            Name = "Clone Benchmark",
            IsActive = true,
            CreatedAt = new DateTime(2024, 6, 1, 12, 0, 0, DateTimeKind.Utc)
        };

        // PropertyMapper — Clone<T> internally calls Map<T,T>; warm up compiles the IL delegate.
        _propertyMapper = new PropMap();
        _propertyMapper.Warmup<FlatSource, FlatSource>();

        // AutoMapper — CreateMapper() compiles expression trees for the self-map.
        _autoMapper = new MapperConfiguration(cfg =>
            cfg.CreateMap<FlatSource, FlatSource>(), NullLoggerFactory.Instance).CreateMapper();

        // Mapster — Compile() pre-builds the self-adapter.
        _mapsterConfig = new TypeAdapterConfig();
        _mapsterConfig.NewConfig<FlatSource, FlatSource>();
        _mapsterConfig.Compile();
    }

    // -----------------------------------------------------------------------
    // Benchmarks
    // -----------------------------------------------------------------------

    /// <summary>Hand-written property copy — zero-overhead baseline.</summary>
    [Benchmark(Baseline = true)]
    public FlatSource Manual()
        => new()
        {
            Id = _source.Id,
            Name = _source.Name,
            IsActive = _source.IsActive,
            CreatedAt = _source.CreatedAt
        };

    /// <summary>
    /// <c>PropMap.Clone&lt;T&gt;</c> — IL-emitted same-type delegate, lock-free hot path.
    /// </summary>
    [Benchmark]
    public FlatSource PropertyMapperClone()
        => _propertyMapper.Clone(_source);

    /// <summary>Mapster expression-tree self-adapter.</summary>
    [Benchmark]
    public FlatSource Mapster()
        => _source.Adapt<FlatSource>(_mapsterConfig);

    /// <summary>AutoMapper expression-tree self-mapper.</summary>
    [Benchmark]
    public FlatSource AutoMapper()
        => _autoMapper.Map<FlatSource>(_source);
}
