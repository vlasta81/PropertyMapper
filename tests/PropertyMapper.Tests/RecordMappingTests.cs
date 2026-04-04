namespace PropertyMapper.Tests;

/// <summary>
/// Tests for record type mapping
/// </summary>
public class RecordMappingTests
{
    private readonly PropMap _mapper = new();

    [Fact]
    public void Map_RecordToRecord_ShouldMapAllProperties()
    {
        // Arrange
        var source = new PersonRecord { FirstName = "John", LastName = "Doe", Age = 35 };

        // Act
        var result = _mapper.Map<PersonRecord, PersonDto>(source);

        // Assert
        Assert.Equal("John", result.FirstName);
        Assert.Equal("Doe", result.LastName);
        Assert.Equal(35, result.Age);
    }

    [Fact]
    public void Map_RecordWithEmptyStrings_ShouldMapCorrectly()
    {
        // Arrange
        var source = new PersonRecord { FirstName = "", LastName = "", Age = 0 };

        // Act
        var result = _mapper.Map<PersonRecord, PersonDto>(source);

        // Assert
        Assert.Equal(string.Empty, result.FirstName);
        Assert.Equal(string.Empty, result.LastName);
        Assert.Equal(0, result.Age);
    }

    [Fact]
    public void Map_OrderRecord_ShouldMapAllProperties()
    {
        // Arrange
        var source = new OrderRecord { OrderId = 12345, CustomerName = "Customer XYZ", Amount = 999.99m };

        // Act
        var result = _mapper.Map<OrderRecord, OrderDto>(source);

        // Assert
        Assert.Equal(12345, result.OrderId);
        Assert.Equal("Customer XYZ", result.CustomerName);
        Assert.Equal(999.99m, result.Amount);
    }

    [Fact]
    public void Map_RecordMultipleTimes_ShouldUseCachedDelegate()
    {
        // Arrange
        var source1 = new PersonRecord { FirstName = "Alice", LastName = "Smith", Age = 25 };
        var source2 = new PersonRecord { FirstName = "Bob", LastName = "Jones", Age = 30 };
        var source3 = new PersonRecord { FirstName = "Charlie", LastName = "Brown", Age = 35 };

        // Act
        var result1 = _mapper.Map<PersonRecord, PersonDto>(source1);
        var result2 = _mapper.Map<PersonRecord, PersonDto>(source2);
        var result3 = _mapper.Map<PersonRecord, PersonDto>(source3);

        // Assert
        Assert.Equal("Alice", result1.FirstName);
        Assert.Equal(25, result1.Age);
        Assert.Equal("Bob", result2.FirstName);
        Assert.Equal(30, result2.Age);
        Assert.Equal("Charlie", result3.FirstName);
        Assert.Equal(35, result3.Age);
    }

    [Fact]
    public void Map_RecordWithNegativeNumbers_ShouldMapCorrectly()
    {
        // Arrange
        var source = new OrderRecord { OrderId = -1, CustomerName = "Refund Customer", Amount = -50.00m };

        // Act
        var result = _mapper.Map<OrderRecord, OrderDto>(source);

        // Assert
        Assert.Equal(-1, result.OrderId);
        Assert.Equal("Refund Customer", result.CustomerName);
        Assert.Equal(-50.00m, result.Amount);
    }

    [Fact]
    public void Map_InitOnlyRecord_ShouldMapCorrectly()
    {
        // Arrange
        var source = new InitOnlyRecord { Id = 100, Name = "Init Only Test" };

        // Act
        var result = _mapper.Map<InitOnlyRecord, InitOnlyRecord>(source);

        // Assert
        Assert.Equal(100, result.Id);
        Assert.Equal("Init Only Test", result.Name);
    }
}
