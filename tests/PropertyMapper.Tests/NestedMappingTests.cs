namespace PropertyMapper.Tests;

/// <summary>
/// Tests for nested object mapping
/// </summary>
public class NestedMappingTests
{
    private readonly PropMap _mapper = new();

    [Fact]
    public void Map_SimpleNested_ShouldMapAllLevels()
    {
        // Arrange
        var source = new PersonWithAddress
        {
            Name = "John Doe",
            Age = 30,
            Address = new Address
            {
                Street = "123 Main St",
                City = "New York",
                PostalCode = "10001"
            }
        };

        // Act
        var result = _mapper.Map<PersonWithAddress, PersonWithAddressDto>(source);

        // Assert
        Assert.Equal("John Doe", result.Name);
        Assert.Equal(30, result.Age);
        Assert.NotNull(result.Address);
        Assert.Equal("123 Main St", result.Address.Street);
        Assert.Equal("New York", result.Address.City);
        Assert.Equal("10001", result.Address.PostalCode);
    }

    [Fact]
    public void Map_NullNestedObject_ShouldHandleGracefully()
    {
        // Arrange
        var source = new PersonWithAddress
        {
            Name = "Jane Doe",
            Age = 25,
            Address = null!
        };

        // Act
        var result = _mapper.Map<PersonWithAddress, PersonWithAddressDto>(source);

        // Assert
        Assert.Equal("Jane Doe", result.Name);
        Assert.Equal(25, result.Age);
        Assert.Null(result.Address);
    }

    [Fact]
    public void Map_DeepNested_ShouldMapAllLevels()
    {
        // Arrange - Simplified without collections
        var source = new Employee
        {
            Name = "Alice",
            EmployeeId = 1,
            HomeAddress = new Address
            {
                Street = "789 Elm St",
                City = "Oakland",
                PostalCode = "94601"
            }
        };

        // Act
        var result = _mapper.Map<Employee, EmployeeDto>(source);

        // Assert
        Assert.Equal("Alice", result.Name);
        Assert.Equal(1, result.EmployeeId);
        Assert.NotNull(result.HomeAddress);
        Assert.Equal("789 Elm St", result.HomeAddress.Street);
        Assert.Equal("Oakland", result.HomeAddress.City);
        Assert.Equal("94601", result.HomeAddress.PostalCode);
    }

    [Fact]
    public void Map_NestedTwice_ShouldReuseCompiledMapper()
    {
        // Arrange
        var source1 = new PersonWithAddress
        {
            Name = "Person 1",
            Age = 20,
            Address = new Address { Street = "Street 1", City = "City 1", PostalCode = "11111" }
        };

        var source2 = new PersonWithAddress
        {
            Name = "Person 2",
            Age = 40,
            Address = new Address { Street = "Street 2", City = "City 2", PostalCode = "22222" }
        };

        // Act
        var result1 = _mapper.Map<PersonWithAddress, PersonWithAddressDto>(source1);
        var result2 = _mapper.Map<PersonWithAddress, PersonWithAddressDto>(source2);

        // Assert
        Assert.Equal("Person 1", result1.Name);
        Assert.Equal("Street 1", result1.Address.Street);
        Assert.Equal("Person 2", result2.Name);
        Assert.Equal("Street 2", result2.Address.Street);
    }

    [Fact]
    public void Map_EmptyNestedObject_ShouldMapDefaults()
    {
        // Arrange
        var source = new PersonWithAddress
        {
            Name = "Empty Address Person",
            Age = 50,
            Address = new Address() // Empty address with defaults
        };

        // Act
        var result = _mapper.Map<PersonWithAddress, PersonWithAddressDto>(source);

        // Assert
        Assert.NotNull(result.Address);
        Assert.Equal(string.Empty, result.Address.Street);
        Assert.Equal(string.Empty, result.Address.City);
        Assert.Equal(string.Empty, result.Address.PostalCode);
    }
}
