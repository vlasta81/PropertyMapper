using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using PropertyMapper.Masking;
using PropertyMapper.Benchmarks.Models;

namespace PropertyMapper.Benchmarks.Benchmarks;

/// <summary>
/// Hot-path benchmark: compares <see cref="PropMap.Map{TIn,TOut}"/> (plain mapping)
/// with <see cref="PropMap.MapThenApplyMask{TIn,TOut}"/> (mapping followed by field-masking).
/// </summary>
/// <remarks>
/// <para>
/// Both methods share the same pre-compiled IL delegate (from the cache) so the
/// only variable cost is the per-call work done by the mask's compiled
/// <see cref="Action{T}"/> clearers.
/// </para>
/// <list type="bullet">
/// <item><description><b>Map_NoMask</b> — steady-state mapping with zero masking overhead (baseline).</description></item>
/// <item><description><b>MapThenApplyMask_OneField</b> — masks a single property (<c>Name</c>); one compiled clearer invoked.</description></item>
/// <item><description><b>MapThenApplyMask_ThreeFields</b> — masks three properties; three compiled clearers invoked.</description></item>
/// </list>
/// </remarks>
[BenchmarkCategory("FieldMask")]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
public class FieldMaskBenchmark
{
    // -----------------------------------------------------------------------
    // Mapper and mask instances
    // -----------------------------------------------------------------------

    private PropMap _mapper = null!;
    private FlatSource _source = null!;
    private FieldMask<FlatTarget> _maskOneField = null!;
    private FieldMask<FlatTarget> _maskThreeFields = null!;

    // -----------------------------------------------------------------------
    // Setup
    // -----------------------------------------------------------------------

    /// <summary>
    /// Pre-warms the mapping delegate and constructs reusable mask instances.
    /// Mask clearers are compiled once here; subsequent calls are allocation-free.
    /// </summary>
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

        _mapper = new PropMap();
        _mapper.Warmup<FlatSource, FlatTarget>();

        // Single-field mask: only Name is cleared.
        _maskOneField = new FieldMask<FlatTarget>("Name");

        // Three-field mask: Name, IsActive, CreatedAt are cleared.
        _maskThreeFields = new FieldMask<FlatTarget>("Name", "IsActive", "CreatedAt");
    }

    // -----------------------------------------------------------------------
    // Benchmarks
    // -----------------------------------------------------------------------

    /// <summary>
    /// Plain mapping with no masking — IL-emit delegate only.
    /// Serves as the baseline for measuring masking overhead.
    /// </summary>
    [Benchmark(Baseline = true)]
    public FlatTarget Map_NoMask()
        => _mapper.Map<FlatSource, FlatTarget>(_source);

    /// <summary>
    /// Maps and then clears one property (<c>Name</c>).
    /// One compiled <see cref="Action{T}"/> clearer is invoked after mapping.
    /// </summary>
    [Benchmark]
    public FlatTarget MapThenApplyMask_OneField()
        => _mapper.MapThenApplyMask<FlatSource, FlatTarget>(_source, _maskOneField);

    /// <summary>
    /// Maps and then clears three properties (<c>Name</c>, <c>IsActive</c>, <c>CreatedAt</c>).
    /// Three compiled <see cref="Action{T}"/> clearers are invoked after mapping.
    /// </summary>
    [Benchmark]
    public FlatTarget MapThenApplyMask_ThreeFields()
        => _mapper.MapThenApplyMask<FlatSource, FlatTarget>(_source, _maskThreeFields);
}
