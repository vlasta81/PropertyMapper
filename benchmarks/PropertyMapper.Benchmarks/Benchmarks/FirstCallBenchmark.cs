using AutoMapper;
using BenchmarkDotNet.Attributes;
using Mapster;
using Microsoft.Extensions.Logging.Abstractions;
using PropertyMapper.Benchmarks.Models;

namespace PropertyMapper.Benchmarks.Benchmarks;

/// <summary>
/// Cold-start benchmark: measures the end-to-end cost from creating a fresh
/// mapper instance through receiving the first mapped result.
/// </summary>
/// <remarks>
/// <para>
/// Each benchmark method creates a brand-new mapper pipeline and immediately
/// performs one mapping call.  This intentionally includes the compilation or
/// configuration overhead that each library incurs before the first mapping.
/// </para>
/// <para>
/// Uses <c>RunStrategy.ColdStart</c> so BenchmarkDotNet treats every invocation
/// as a single cold measurement (no iteration batching, no warm-up).
/// </para>
/// <list type="bullet">
/// <item><b>PropertyMapper</b> — IL delegate is compiled on the first <c>Map()</c> call.</item>
/// <item><b>Mapster</b> — expression tree is compiled by <c>Compile()</c>.</item>
/// <item><b>AutoMapper</b> — expression trees are compiled by <c>CreateMapper()</c>.</item>
/// </list>
/// </remarks>
[BenchmarkCategory("FirstCall")]
public class FirstCallBenchmark
{
    // Source data is the only state that persists across iterations —
    // mapper instances are created fresh inside each benchmark method.
    private WideSource _source = null!;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _source = new WideSource
        {
            Id = 1,
            FirstName = "Cold",
            LastName = "Start",
            Email = "cold@start.io",
            Age = 25,
            Salary = 60_000m,
            IsActive = true,
            CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            UpdatedAt = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc),
            Department = "Benchmarks",
            ManagerId = 0,
            PhoneNumber = "+0-000-0000"
        };
    }

    // -----------------------------------------------------------------------
    // Benchmarks — each creates a fresh pipeline and maps once
    // -----------------------------------------------------------------------

    /// <summary>
    /// PropertyMapper: create <see cref="PropMap"/> instance and call <c>Map()</c>.
    /// The delegate is compiled (IL emit) during this call.
    /// </summary>
    [Benchmark(Baseline = true)]
    public WideTarget PropertyMapper()
    {
        var mapper = new PropMap();
        return mapper.Map<WideSource, WideTarget>(_source);
    }

    /// <summary>
    /// Mapster: create <see cref="TypeAdapterConfig"/>, register mapping,
    /// call <c>Compile()</c>, then <c>Adapt()</c>.
    /// </summary>
    [Benchmark]
    public WideTarget Mapster()
    {
        var config = new TypeAdapterConfig();
        config.NewConfig<WideSource, WideTarget>();
        config.Compile();
        return _source.Adapt<WideSource, WideTarget>(config);
    }

    /// <summary>
    /// AutoMapper: create <see cref="MapperConfiguration"/>, call <c>CreateMapper()</c>
    /// (expression-tree compilation), then <c>Map()</c>.
    /// </summary>
    [Benchmark]
    public WideTarget AutoMapper()
    {
        var mapper = new MapperConfiguration(cfg =>
            cfg.CreateMap<WideSource, WideTarget>(), NullLoggerFactory.Instance).CreateMapper();
        return mapper.Map<WideTarget>(_source);
    }
}
