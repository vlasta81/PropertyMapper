namespace PropertyMapper.Tests;

/// <summary>
/// Tests for nullable type conversions
/// </summary>
public class NullableConversionTests
{
    private readonly PropMap _mapper = new();

    #region Nullable to Nullable

    [Fact]
    public void Map_NullableIntToNullableInt_WithValue_ShouldMap()
    {
        // Arrange
        var source = new NullableSource { NullableInt = 42 };

        // Act
        var result = _mapper.Map<NullableSource, NullableTarget>(source);

        // Assert
        Assert.Equal(42, result.NullableInt);
    }

    [Fact]
    public void Map_NullableIntToNullableInt_WithNull_ShouldMapNull()
    {
        // Arrange
        var source = new NullableSource { NullableInt = null };

        // Act
        var result = _mapper.Map<NullableSource, NullableTarget>(source);

        // Assert
        Assert.Null(result.NullableInt);
    }

    [Fact]
    public void Map_NullableDateToNullableDate_WithValue_ShouldMap()
    {
        // Arrange
        var date = new DateTime(2024, 1, 1);
        var source = new NullableSource { NullableDate = date };

        // Act
        var result = _mapper.Map<NullableSource, NullableTarget>(source);

        // Assert
        Assert.Equal(date, result.NullableDate);
    }

    #endregion

    #region Value to Nullable

    [Fact]
    public void Map_ValueToNullable_ShouldWrapInNullable()
    {
        // Arrange
        var source = new NullableSource { RequiredInt = 100 };

        // Act
        var result = _mapper.Map<NullableSource, NullableTarget>(source);

        // Assert
        Assert.NotNull(result.RequiredInt);
        Assert.Equal(100, result.RequiredInt);
    }

    [Fact]
    public void Map_ValueToNullable_ZeroValue_ShouldWrap()
    {
        // Arrange
        var source = new NullableSource { RequiredInt = 0 };

        // Act
        var result = _mapper.Map<NullableSource, NullableTarget>(source);

        // Assert
        Assert.NotNull(result.RequiredInt);
        Assert.Equal(0, result.RequiredInt);
    }

    #endregion

    #region Nullable to Value

    [Fact]
    public void Map_NullableToValue_WithValue_ShouldUnwrap()
    {
        // Arrange
        var source = new NullableSource 
        { 
            NullableInt = 42,
            NullableDouble = 3.14,
            NullableDate = new DateTime(2024, 1, 1)
        };

        // Act
        var result = _mapper.Map<NullableSource, ValueTarget>(source);

        // Assert
        Assert.Equal(42, result.NullableInt);
        Assert.Equal(3.14, result.NullableDouble);
        Assert.Equal(new DateTime(2024, 1, 1), result.NullableDate);
    }

    [Fact]
    public void Map_NullableToValue_WithNull_ShouldUseDefault()
    {
        // Arrange
        var source = new NullableSource 
        { 
            NullableInt = null,
            NullableDouble = null,
            NullableDate = null
        };

        // Act
        var result = _mapper.Map<NullableSource, ValueTarget>(source);

        // Assert
        Assert.Equal(0, result.NullableInt);
        Assert.Equal(0.0, result.NullableDouble);
        Assert.Equal(default(DateTime), result.NullableDate);
    }

    #endregion

    [Fact]
    public void Map_MixedNullableProperties_ShouldMapCorrectly()
    {
        // Arrange
        var source = new NullableSource
        {
            NullableInt = 10,
            NullableDouble = null,
            NullableDate = new DateTime(2024, 6, 15),
            RequiredInt = 999
        };

        // Act
        var result = _mapper.Map<NullableSource, NullableTarget>(source);

        // Assert
        Assert.Equal(10, result.NullableInt);
        Assert.Null(result.NullableDouble);
        Assert.Equal(new DateTime(2024, 6, 15), result.NullableDate);
        Assert.Equal(999, result.RequiredInt);
    }
}
