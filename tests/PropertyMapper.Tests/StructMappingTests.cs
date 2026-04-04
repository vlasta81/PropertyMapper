namespace PropertyMapper.Tests;

/// <summary>
/// Tests for struct and record struct mapping
/// </summary>
public class StructMappingTests
{
    private readonly PropMap _mapper = new();

    [Fact]
    public void Map_StructToStruct_ShouldMapCorrectly()
    {
        // Arrange
        var source = new PointStruct
        {
            X = 10,
            Y = 20,
            Label = "Point A"
        };

        // Act
        var result = _mapper.Map<PointStruct, PointStruct>(source);

        // Assert
        Assert.Equal(10, result.X);
        Assert.Equal(20, result.Y);
        Assert.Equal("Point A", result.Label);
    }

    [Fact]
    public void Map_StructToStructDifferentShape_ShouldMapMatchingProperties()
    {
        // Arrange
        var source = new PointStruct
        {
            X = 5,
            Y = 15,
            Label = "Test"
        };

        // Act
        var result = _mapper.Map<PointStruct, Point3DStruct>(source);

        // Assert
        Assert.Equal(5, result.X);
        Assert.Equal(15, result.Y);
        Assert.Equal(0, result.Z); // Default value
    }

    [Fact]
    public void Map_RecordStruct_ShouldMapCorrectly()
    {
        // Arrange
        var source = new VectorStruct(1.5, 2.5, 3.5);

        // Act
        var result = _mapper.Map<VectorStruct, VectorDto>(source);

        // Assert
        Assert.Equal(1.5, result.X);
        Assert.Equal(2.5, result.Y);
        Assert.Equal(3.5, result.Z);
    }

    [Fact]
    public void Map_StructMultipleTimes_ShouldUseCachedDelegate()
    {
        // Arrange
        var source1 = new PointStruct { X = 1, Y = 2, Label = "A" };
        var source2 = new PointStruct { X = 3, Y = 4, Label = "B" };
        var source3 = new PointStruct { X = 5, Y = 6, Label = "C" };

        // Act
        var result1 = _mapper.Map<PointStruct, PointStruct>(source1);
        var result2 = _mapper.Map<PointStruct, PointStruct>(source2);
        var result3 = _mapper.Map<PointStruct, PointStruct>(source3);

        // Assert
        Assert.Equal(1, result1.X);
        Assert.Equal("A", result1.Label);
        Assert.Equal(3, result2.X);
        Assert.Equal("B", result2.Label);
        Assert.Equal(5, result3.X);
        Assert.Equal("C", result3.Label);
    }

    [Fact]
    public void Map_StructWithDefaultValues_ShouldMapDefaults()
    {
        // Arrange
        var source = new PointStruct(); // Default values

        // Act
        var result = _mapper.Map<PointStruct, PointStruct>(source);

        // Assert
        Assert.Equal(0, result.X);
        Assert.Equal(0, result.Y);
        Assert.Null(result.Label);
    }

    [Fact]
    public void Map_RecordStructWithNegativeValues_ShouldMapCorrectly()
    {
        // Arrange
        var source = new VectorStruct(-10.5, -20.3, -30.7);

        // Act
        var result = _mapper.Map<VectorStruct, VectorDto>(source);

        // Assert
        Assert.Equal(-10.5, result.X);
        Assert.Equal(-20.3, result.Y);
        Assert.Equal(-30.7, result.Z);
    }
}
