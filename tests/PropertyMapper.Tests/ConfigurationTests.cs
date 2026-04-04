using PropertyMapper.Configuration;

namespace PropertyMapper.Tests;

/// <summary>
/// Tests for <see cref="PropMapConfiguration"/> options and their effect on mapping behaviour.
/// </summary>
public class ConfigurationTests
{
    #region Mapper construction and basic mapping with config

    [Fact]
    public void Configuration_WithMaxDepth_ShouldApply()
    {
        var config = new PropMapConfiguration().WithMaxDepth(5);
        var mapper = new PropMap(config);

        var source = new SimpleSource { Id = 1, Name = "Depth Test", IsActive = true };
        var result = mapper.Map<SimpleSource, SimpleTarget>(source);

        Assert.Equal(1, result.Id);
        Assert.Equal("Depth Test", result.Name);
        Assert.True(result.IsActive);
    }

    [Fact]
    public void Configuration_Fluent_ShouldChain()
    {
        var config = new PropMapConfiguration()
            .WithMaxDepth(10);

        var mapper = new PropMap(config);

        var source = new SimpleSource { Id = 99, Name = "Chained", IsActive = true };
        var result = mapper.Map<SimpleSource, SimpleTarget>(source);

        Assert.Equal(99, result.Id);
        Assert.Equal("Chained", result.Name);
        Assert.True(result.IsActive);
    }

    #endregion

    #region ThrowOnUnmappedProperties

    [Fact]
    public void Configuration_ThrowOnUnmappedProperties_ExtraSourceProps_ShouldThrow()
    {
        // SourceWithExtra has 'ExtraProperty' that has no match on TargetWithLess.
        var config = new PropMapConfiguration().ThrowOnUnmapped(true);
        var mapper = new PropMap(config);

        var source = new SourceWithExtra { Id = 1, Name = "Test", ExtraProperty = "extra" };
        Assert.Throws<InvalidOperationException>(
            () => mapper.Map<SourceWithExtra, TargetWithLess>(source));
    }

    [Fact]
    public void Configuration_ThrowOnUnmappedProperties_AllPropsMapped_ShouldNotThrow()
    {
        // SimpleSource and SimpleTarget have identical property sets — no unmapped props.
        var config = new PropMapConfiguration().ThrowOnUnmapped(true);
        var mapper = new PropMap(config);

        var source = new SimpleSource { Id = 1, Name = "OK" };
        var result = mapper.Map<SimpleSource, SimpleTarget>(source); // must not throw

        Assert.Equal(1, result.Id);
        Assert.Equal("OK", result.Name);
    }

    [Fact]
    public void Configuration_ThrowOnUnmappedProperties_DefaultIsFalse_DoesNotThrow()
    {
        // Default config has ThrowOnUnmappedProperties = false.
        var mapper = new PropMap();

        var source = new SourceWithExtra { Id = 2, Name = "No throw", ExtraProperty = "silently ignored" };
        var result = mapper.Map<SourceWithExtra, TargetWithLess>(source);

        Assert.Equal(2, result.Id);
        Assert.Equal("No throw", result.Name);
    }

    #endregion

    #region WithMaxDepth edge cases

    [Fact]
    public void Configuration_WithMaxDepth_OutOfRange_ShouldThrow()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new PropMapConfiguration().WithMaxDepth(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new PropMapConfiguration().WithMaxDepth(257));
    }

    [Fact]
    public void Configuration_WithMaxDepth_ValidBoundaries_ShouldNotThrow()
    {
        var min = new PropMapConfiguration().WithMaxDepth(1);
        var max = new PropMapConfiguration().WithMaxDepth(256);

        Assert.NotNull(new PropMap(min));
        Assert.NotNull(new PropMap(max));
    }

    #endregion
}

