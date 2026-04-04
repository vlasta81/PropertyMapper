namespace PropertyMapper.Tests;

/// <summary>
/// Tests for edge cases and error handling
/// </summary>
public class EdgeCaseTests
{
    private readonly PropMap _mapper = new();

    [Fact]
    public void Map_SourceWithExtraProperties_ShouldMapOnlyMatching()
    {
        // Arrange
        var source = new SourceWithExtra
        {
            Id = 123,
            Name = "Test",
            ExtraProperty = "This should be ignored"
        };

        // Act
        var result = _mapper.Map<SourceWithExtra, TargetWithLess>(source);

        // Assert
        Assert.Equal(123, result.Id);
        Assert.Equal("Test", result.Name);
    }

    [Fact]
    public void Map_TargetWithExtraProperties_ShouldLeaveDefaults()
    {
        // Arrange
        var source = new SourceWithLess { Id = 456 };

        // Act
        var result = _mapper.Map<SourceWithLess, TargetWithExtra>(source);

        // Assert
        Assert.Equal(456, result.Id);
        Assert.Equal(string.Empty, result.Name); // Default value
    }

    [Fact]
    public void Map_ReadOnlyProperties_ShouldSkip()
    {
        // Arrange
        var source = new ReadOnlySource { Name = "Test" };

        // Act
        var result = _mapper.Map<ReadOnlySource, SimpleTarget>(source);

        // Assert - Id is read-only in source, should map if target has setter
        Assert.Equal("Test", result.Name);
    }

    [Fact]
    public void Map_VeryLargeInt_ShouldMapCorrectly()
    {
        // Arrange
        var source = new SimpleSource { Id = int.MaxValue };

        // Act
        var result = _mapper.Map<SimpleSource, SimpleTarget>(source);

        // Assert
        Assert.Equal(int.MaxValue, result.Id);
    }

    [Fact]
    public void Map_VerySmallInt_ShouldMapCorrectly()
    {
        // Arrange
        var source = new SimpleSource { Id = int.MinValue };

        // Act
        var result = _mapper.Map<SimpleSource, SimpleTarget>(source);

        // Assert
        Assert.Equal(int.MinValue, result.Id);
    }

    [Fact]
    public void Map_UnicodeString_ShouldMapCorrectly()
    {
        // Arrange
        var source = new SimpleSource { Name = "Hello 世界 🌍 Привет" };

        // Act
        var result = _mapper.Map<SimpleSource, SimpleTarget>(source);

        // Assert
        Assert.Equal("Hello 世界 🌍 Привет", result.Name);
    }

    [Fact]
    public void Map_LongString_ShouldMapCorrectly()
    {
        // Arrange
        var longString = new string('A', 10000);
        var source = new SimpleSource { Name = longString };

        // Act
        var result = _mapper.Map<SimpleSource, SimpleTarget>(source);

        // Assert
        Assert.Equal(longString, result.Name);
        Assert.Equal(10000, result.Name.Length);
    }

    [Fact]
    public void Map_DateTimeMinValue_ShouldMapCorrectly()
    {
        // Arrange
        var source = new SimpleSource { CreatedAt = DateTime.MinValue };

        // Act
        var result = _mapper.Map<SimpleSource, SimpleTarget>(source);

        // Assert
        Assert.Equal(DateTime.MinValue, result.CreatedAt);
    }

    [Fact]
    public void Map_DateTimeMaxValue_ShouldMapCorrectly()
    {
        // Arrange
        var source = new SimpleSource { CreatedAt = DateTime.MaxValue };

        // Act
        var result = _mapper.Map<SimpleSource, SimpleTarget>(source);

        // Assert
        Assert.Equal(DateTime.MaxValue, result.CreatedAt);
    }

    [Fact]
    public void Map_NullableWithHasValue_ShouldPreserveNull()
    {
        // Arrange
        var source = new NullableSource
        {
            NullableInt = null,
            NullableDouble = null,
            NullableDate = null
        };

        // Act
        var result = _mapper.Map<NullableSource, NullableTarget>(source);

        // Assert
        Assert.Null(result.NullableInt);
        Assert.Null(result.NullableDouble);
        Assert.Null(result.NullableDate);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(42)]
    [InlineData(1000)]
    public void Map_VariousIntValues_ShouldMapCorrectly(int value)
    {
        // Arrange
        var source = new SimpleSource { Id = value };

        // Act
        var result = _mapper.Map<SimpleSource, SimpleTarget>(source);

        // Assert
        Assert.Equal(value, result.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData("A")]
    [InlineData("Test")]
    [InlineData("Very Long String With Spaces And Special !@#$%")]
    public void Map_VariousStringValues_ShouldMapCorrectly(string value)
    {
        // Arrange
        var source = new SimpleSource { Name = value };

        // Act
        var result = _mapper.Map<SimpleSource, SimpleTarget>(source);

        // Assert
        Assert.Equal(value, result.Name);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Map_BooleanValues_ShouldMapCorrectly(bool value)
    {
        // Arrange
        var source = new SimpleSource { IsActive = value };

        // Act
        var result = _mapper.Map<SimpleSource, SimpleTarget>(source);

        // Assert
        Assert.Equal(value, result.IsActive);
    }
}
