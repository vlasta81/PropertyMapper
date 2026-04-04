namespace PropertyMapper.Tests;

/// <summary>
/// Tests for operator conversion mapping
/// </summary>
public class OperatorConversionTests
{
    private readonly PropMap _mapper = new();

    [Fact]
    public void Map_MetersToFeet_UsingOperator_ShouldConvert()
    {
        // Arrange
        var source = new Measurement
        {
            Name = "Distance Test",
            Distance = new Meters(10.0)
        };

        // Act
        var result = _mapper.Map<Measurement, MeasurementDto>(source);

        // Assert
        Assert.Equal("Distance Test", result.Name);
        Assert.Equal(32.8084, result.Distance.Value, precision: 3);
    }

    [Fact]
    public void Map_FeetToMeters_UsingOperator_ShouldConvert()
    {
        // Arrange
        var source = new MeasurementDto
        {
            Name = "Reverse Test",
            Distance = new Feet(32.8084)
        };

        // Act
        var result = _mapper.Map<MeasurementDto, Measurement>(source);

        // Assert
        Assert.Equal("Reverse Test", result.Name);
        Assert.Equal(10.0, result.Distance.Value, precision: 3);
    }

    [Fact]
    public void Map_ZeroMeters_ShouldConvertToZeroFeet()
    {
        // Arrange
        var source = new Measurement
        {
            Name = "Zero Test",
            Distance = new Meters(0.0)
        };

        // Act
        var result = _mapper.Map<Measurement, MeasurementDto>(source);

        // Assert
        Assert.Equal(0.0, result.Distance.Value);
    }

    [Fact]
    public void Map_LargeMetersValue_ShouldConvertCorrectly()
    {
        // Arrange
        var source = new Measurement
        {
            Name = "Large Value",
            Distance = new Meters(1000.0)
        };

        // Act
        var result = _mapper.Map<Measurement, MeasurementDto>(source);

        // Assert
        Assert.Equal(3280.84, result.Distance.Value, precision: 2);
    }
}
