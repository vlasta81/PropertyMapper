namespace PropertyMapper.Tests;

/// <summary>
/// Basic mapping tests for simple types
/// </summary>
public class BasicMappingTests
{
    private readonly PropMap _mapper = new();

    [Fact]
    public void Map_SimpleProperties_ShouldCopyAllValues()
    {
        // Arrange
        var source = new SimpleSource
        {
            Id = 123,
            Name = "Test User",
            IsActive = true,
            CreatedAt = new DateTime(2024, 1, 15)
        };

        // Act
        var result = _mapper.Map<SimpleSource, SimpleTarget>(source);

        // Assert
        Assert.Equal(source.Id, result.Id);
        Assert.Equal(source.Name, result.Name);
        Assert.Equal(source.IsActive, result.IsActive);
        Assert.Equal(source.CreatedAt, result.CreatedAt);
    }

    [Fact]
    public void Map_IntToInt_ShouldMapCorrectly()
    {
        // Arrange
        var source = new SimpleSource { Id = 42 };

        // Act
        var result = _mapper.Map<SimpleSource, SimpleTarget>(source);

        // Assert
        Assert.Equal(42, result.Id);
    }

    [Fact]
    public void Map_StringToString_ShouldMapCorrectly()
    {
        // Arrange
        var source = new SimpleSource { Name = "Hello World" };

        // Act
        var result = _mapper.Map<SimpleSource, SimpleTarget>(source);

        // Assert
        Assert.Equal("Hello World", result.Name);
    }

    [Fact]
    public void Map_BooleanToBoolean_ShouldMapCorrectly()
    {
        // Arrange
        var source = new SimpleSource { IsActive = true };

        // Act
        var result = _mapper.Map<SimpleSource, SimpleTarget>(source);

        // Assert
        Assert.True(result.IsActive);
    }

    [Fact]
    public void Map_DateTimeToDateTime_ShouldMapCorrectly()
    {
        // Arrange
        var date = new DateTime(2024, 12, 25, 10, 30, 0);
        var source = new SimpleSource { CreatedAt = date };

        // Act
        var result = _mapper.Map<SimpleSource, SimpleTarget>(source);

        // Assert
        Assert.Equal(date, result.CreatedAt);
    }

    [Fact]
    public void Map_NullSource_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            _mapper.Map<SimpleSource, SimpleTarget>(null!));
    }

    [Fact]
    public void Map_MultipleCallsSameTypes_ShouldUseCachedDelegate()
    {
        // Arrange
        var source1 = new SimpleSource { Id = 1, Name = "First" };
        var source2 = new SimpleSource { Id = 2, Name = "Second" };

        // Act
        var result1 = _mapper.Map<SimpleSource, SimpleTarget>(source1);
        var result2 = _mapper.Map<SimpleSource, SimpleTarget>(source2);

        // Assert
        Assert.Equal(1, result1.Id);
        Assert.Equal("First", result1.Name);
        Assert.Equal(2, result2.Id);
        Assert.Equal("Second", result2.Name);
    }

    [Fact]
    public void Map_EmptyString_ShouldMapCorrectly()
    {
        // Arrange
        var source = new SimpleSource { Name = string.Empty };

        // Act
        var result = _mapper.Map<SimpleSource, SimpleTarget>(source);

        // Assert
        Assert.Equal(string.Empty, result.Name);
    }

    [Fact]
    public void Map_DefaultValues_ShouldMapCorrectly()
    {
        // Arrange
        var source = new SimpleSource(); // All default values

        // Act
        var result = _mapper.Map<SimpleSource, SimpleTarget>(source);

        // Assert
        Assert.Equal(0, result.Id);
        Assert.NotNull(result.Name);
        Assert.False(result.IsActive);
        Assert.Equal(default(DateTime), result.CreatedAt);
    }
}
