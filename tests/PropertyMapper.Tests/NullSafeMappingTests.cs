namespace PropertyMapper.Tests;

public class NullSafeMappingTests
{
    private readonly PropMap _mapper = new();

    #region MapOrDefault

    [Fact]
    public void MapOrDefault_NullSource_ReturnsNull()
    {
        SimpleTarget? result = _mapper.MapOrDefault<SimpleSource, SimpleTarget>(null);
        Assert.Null(result);
    }

    [Fact]
    public void MapOrDefault_ValidSource_ReturnsMappedObject()
    {
        var source = new SimpleSource { Id = 1, Name = "Test" };
        SimpleTarget? result = _mapper.MapOrDefault<SimpleSource, SimpleTarget>(source);
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test", result.Name);
    }

    #endregion

    #region TryMap

    [Fact]
    public void TryMap_NullSource_ReturnsFalseAndNullResult()
    {
        bool success = _mapper.TryMap<SimpleSource, SimpleTarget>(null, out SimpleTarget? result);
        Assert.False(success);
        Assert.Null(result);
    }

    [Fact]
    public void TryMap_ValidSource_ReturnsTrueAndMappedObject()
    {
        var source = new SimpleSource { Id = 42, Name = "Alice" };

        bool success = _mapper.TryMap<SimpleSource, SimpleTarget>(source, out SimpleTarget? result);

        Assert.True(success);
        Assert.NotNull(result);
        Assert.Equal(42, result.Id);
        Assert.Equal("Alice", result.Name);
    }

    #endregion

    #region MapOrElse (fallback value)

    [Fact]
    public void MapOrElse_NullSource_ReturnsFallback()
    {
        var fallback = new SimpleTarget { Id = 99, Name = "Fallback" };
        SimpleTarget result = _mapper.MapOrElse<SimpleSource, SimpleTarget>(null, fallback);
        Assert.Same(fallback, result);
    }

    [Fact]
    public void MapOrElse_ValidSource_ReturnsMappedObject()
    {
        var source = new SimpleSource { Id = 1, Name = "Mapped" };
        var fallback = new SimpleTarget { Id = 99 };

        SimpleTarget result = _mapper.MapOrElse<SimpleSource, SimpleTarget>(source, fallback);

        Assert.Equal(1, result.Id);
        Assert.Equal("Mapped", result.Name);
    }

    [Fact]
    public void MapOrElse_NullFallback_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => _mapper.MapOrElse<SimpleSource, SimpleTarget>(null, (SimpleTarget)null!));
    }

    #endregion

    #region MapOrElse (fallback factory)

    [Fact]
    public void MapOrElse_WithFactory_NullSource_InvokesFactory()
    {
        bool factoryInvoked = false;

        SimpleTarget result = _mapper.MapOrElse<SimpleSource, SimpleTarget>(null, () =>
        {
            factoryInvoked = true;
            return new SimpleTarget { Id = -1 };
        });

        Assert.True(factoryInvoked);
        Assert.Equal(-1, result.Id);
    }

    [Fact]
    public void MapOrElse_WithFactory_ValidSource_DoesNotInvokeFactory()
    {
        bool factoryInvoked = false;
        var source = new SimpleSource { Id = 5 };

        SimpleTarget result = _mapper.MapOrElse<SimpleSource, SimpleTarget>(source, () =>
        {
            factoryInvoked = true;
            return new SimpleTarget { Id = -1 };
        });

        Assert.False(factoryInvoked);
        Assert.Equal(5, result.Id);
    }

    [Fact]
    public void MapOrElse_NullFactory_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => _mapper.MapOrElse<SimpleSource, SimpleTarget>(null, (Func<SimpleTarget>)null!));
    }

    #endregion
}
