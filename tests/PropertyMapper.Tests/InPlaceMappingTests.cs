namespace PropertyMapper.Tests;

public class InPlaceMappingTests
{
    private readonly PropMap _mapper = new();

    #region MapInto

    [Fact]
    public void MapInto_UpdatesAllMatchingProperties()
    {
        var source = new SimpleSource { Id = 42, Name = "Alice", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) };
        var target = new SimpleTarget();

        _mapper.MapInto(source, target);

        Assert.Equal(42, target.Id);
        Assert.Equal("Alice", target.Name);
        Assert.True(target.IsActive);
        Assert.Equal(new DateTime(2024, 1, 1), target.CreatedAt);
    }

    [Fact]
    public void MapInto_PreservesUnmatchedTargetProperties()
    {
        // PartialSource only has Id and Name — IsActive and CreatedAt must survive on the target.
        var source = new PartialSource { Id = 99, Name = "Bob" };
        var target = new SimpleTarget
        {
            Id = 1,
            Name = "Original",
            IsActive = true,
            CreatedAt = new DateTime(2020, 6, 15)
        };

        _mapper.MapInto(source, target);

        Assert.Equal(99, target.Id);
        Assert.Equal("Bob", target.Name);
        Assert.True(target.IsActive);                          // not in PartialSource — must be preserved
        Assert.Equal(new DateTime(2020, 6, 15), target.CreatedAt); // not in PartialSource — must be preserved
    }

    [Fact]
    public void MapInto_NullSource_Throws()
    {
        var target = new SimpleTarget();
        Assert.Throws<ArgumentNullException>(() => _mapper.MapInto<SimpleSource, SimpleTarget>(null!, target));
    }

    [Fact]
    public void MapInto_NullTarget_Throws()
    {
        var source = new SimpleSource { Id = 1 };
        Assert.Throws<ArgumentNullException>(() => _mapper.MapInto<SimpleSource, SimpleTarget>(source, null!));
    }

    [Fact]
    public void MapInto_CalledTwice_SecondCallOverwrites()
    {
        var target = new SimpleTarget { Id = 1, Name = "First" };

        _mapper.MapInto(new SimpleSource { Id = 10, Name = "Second" }, target);
        Assert.Equal(10, target.Id);
        Assert.Equal("Second", target.Name);

        _mapper.MapInto(new SimpleSource { Id = 20, Name = "Third" }, target);
        Assert.Equal(20, target.Id);
        Assert.Equal("Third", target.Name);
    }

    [Fact]
    public void MapInto_ReusesPlanFromPriorMap()
    {
        // Prime the Func<> cache first — GetOrCompileIntoMapper must reuse the plan.
        _ = _mapper.Map<SimpleSource, SimpleTarget>(new SimpleSource { Id = 1, Name = "Seed" });

        var source = new SimpleSource { Id = 99, Name = "Reused" };
        var target = new SimpleTarget();

        _mapper.MapInto(source, target);

        Assert.Equal(99, target.Id);
        Assert.Equal("Reused", target.Name);
    }

    [Fact]
    public void MapInto_PriorMapIntoEnablesReuseByMap()
    {
        // Prime the Action<> cache first — subsequent Map<> call must reuse the plan.
        var primeTarget = new SimpleTarget();
        _mapper.MapInto(new SimpleSource { Id = 5 }, primeTarget);

        // Map<> must still work correctly and share the cached plan.
        var result = _mapper.Map<SimpleSource, SimpleTarget>(new SimpleSource { Id = 77, Name = "AfterInto" });

        Assert.Equal(77, result.Id);
        Assert.Equal("AfterInto", result.Name);
    }

    #endregion

    #region MapMerge

    [Fact]
    public void MapMerge_CombinesPropertiesFromBothSources()
    {
        // MergeSourceA provides Id (not in B), MergeSourceB provides IsActive (not in A).
        var sourceA = new MergeSourceA { Id = 1, Name = "FromA" };
        var sourceB = new MergeSourceB { Name = "FromB", IsActive = true };

        MergeTarget result = _mapper.MapMerge<MergeSourceA, MergeSourceB, MergeTarget>(sourceA, sourceB);

        Assert.Equal(1, result.Id);      // only in sourceA
        Assert.True(result.IsActive);    // only in sourceB
    }

    [Fact]
    public void MapMerge_SecondSourceWinsOnConflict()
    {
        var sourceA = new MergeSourceA { Id = 1, Name = "FromA" };
        var sourceB = new MergeSourceB { Name = "FromB", IsActive = false };

        MergeTarget result = _mapper.MapMerge<MergeSourceA, MergeSourceB, MergeTarget>(sourceA, sourceB);

        Assert.Equal("FromB", result.Name); // sourceB overrides sourceA
    }

    [Fact]
    public void MapMerge_ReturnsNewInstance()
    {
        var sourceA = new MergeSourceA { Id = 7 };
        var sourceB = new MergeSourceB { IsActive = true };

        MergeTarget r1 = _mapper.MapMerge<MergeSourceA, MergeSourceB, MergeTarget>(sourceA, sourceB);
        MergeTarget r2 = _mapper.MapMerge<MergeSourceA, MergeSourceB, MergeTarget>(sourceA, sourceB);

        Assert.NotSame(r1, r2);
    }

    [Fact]
    public void MapMerge_NullFirstSource_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => _mapper.MapMerge<MergeSourceA, MergeSourceB, MergeTarget>(null!, new MergeSourceB()));
    }

    [Fact]
    public void MapMerge_NullSecondSource_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => _mapper.MapMerge<MergeSourceA, MergeSourceB, MergeTarget>(new MergeSourceA(), null!));
    }

    #endregion
}
