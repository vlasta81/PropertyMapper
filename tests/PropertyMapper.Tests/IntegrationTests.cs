namespace PropertyMapper.Tests.Integration;

/// <summary>
/// Real-world integration scenarios
/// </summary>
public class RealWorldScenarioTests
{
    private readonly PropMap _mapper = new();

    #region E-commerce Scenario

    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public Category Category { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }

    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public CategoryDto Category { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }

    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    [Fact]
    public void Scenario_ECommerce_ProductWithCategory_ShouldMapCompletely()
    {
        // Arrange
        var product = new Product
        {
            ProductId = 1001,
            Name = "Laptop",
            Price = 1299.99m,
            StockQuantity = 50,
            Category = new Category
            {
                CategoryId = 10,
                Name = "Electronics",
                Description = "Electronic devices and accessories"
            },
            CreatedAt = new DateTime(2024, 1, 15, 10, 30, 0)
        };

        // Act
        var dto = _mapper.Map<Product, ProductDto>(product);

        // Assert
        Assert.Equal(1001, dto.ProductId);
        Assert.Equal("Laptop", dto.Name);
        Assert.Equal(1299.99m, dto.Price);
        Assert.Equal(50, dto.StockQuantity);
        Assert.Equal(10, dto.Category.CategoryId);
        Assert.Equal("Electronics", dto.Category.Name);
        Assert.Equal("Electronic devices and accessories", dto.Category.Description);
        Assert.Equal(new DateTime(2024, 1, 15, 10, 30, 0), dto.CreatedAt);
    }

    #endregion

    #region User Profile Scenario

    public class User
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserProfile Profile { get; set; } = new();
        public DateTime RegisteredAt { get; set; }
        public bool IsActive { get; set; }
    }

    public class UserProfile
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public Address Address { get; set; } = new();
    }

    public class UserDto
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserProfileDto Profile { get; set; } = new();
        public DateTime RegisteredAt { get; set; }
        public bool IsActive { get; set; }
    }

    public class UserProfileDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public AddressDto Address { get; set; } = new();
    }

    public class Address
    {
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }

    public class AddressDto
    {
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }

    [Fact]
    public void Scenario_UserProfile_MultiLevelNesting_ShouldMapCorrectly()
    {
        // Arrange
        var user = new User
        {
            UserId = Guid.NewGuid(),
            Username = "johndoe",
            Email = "john.doe@example.com",
            Profile = new UserProfile
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1990, 5, 15),
                PhoneNumber = "+1-555-0123",
                Address = new Address
                {
                    Street = "123 Main St",
                    City = "New York",
                    State = "NY",
                    PostalCode = "10001",
                    Country = "USA"
                }
            },
            RegisteredAt = new DateTime(2024, 1, 1),
            IsActive = true
        };

        // Act
        var dto = _mapper.Map<User, UserDto>(user);

        // Assert
        Assert.Equal(user.UserId, dto.UserId);
        Assert.Equal("johndoe", dto.Username);
        Assert.Equal("john.doe@example.com", dto.Email);
        Assert.Equal("John", dto.Profile.FirstName);
        Assert.Equal("Doe", dto.Profile.LastName);
        Assert.Equal(new DateTime(1990, 5, 15), dto.Profile.DateOfBirth);
        Assert.Equal("+1-555-0123", dto.Profile.PhoneNumber);
        Assert.Equal("123 Main St", dto.Profile.Address.Street);
        Assert.Equal("New York", dto.Profile.Address.City);
        Assert.Equal("NY", dto.Profile.Address.State);
        Assert.Equal("10001", dto.Profile.Address.PostalCode);
        Assert.Equal("USA", dto.Profile.Address.Country);
        Assert.Equal(new DateTime(2024, 1, 1), dto.RegisteredAt);
        Assert.True(dto.IsActive);
    }

    #endregion

    #region API Response Scenario

    public record ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public record ApiResponseDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public DateTime Timestamp { get; set; }
    }

    [Fact]
    public void Scenario_ApiResponse_GenericRecords_ShouldMap()
    {
        // Arrange
        var response = new ApiResponse<Product>
        {
            Success = true,
            Message = "Product retrieved successfully",
            Data = new Product
            {
                ProductId = 500,
                Name = "Smartphone",
                Price = 799.99m,
                StockQuantity = 100,
                Category = new Category { CategoryId = 5, Name = "Mobile" },
                CreatedAt = DateTime.Now
            },
            Timestamp = DateTime.Now
        };

        // Act
        var dto = _mapper.Map<ApiResponse<Product>, ApiResponseDto<ProductDto>>(response);

        // Assert
        Assert.True(dto.Success);
        Assert.Equal("Product retrieved successfully", dto.Message);
        Assert.NotNull(dto.Data);
        Assert.Equal(500, dto.Data.ProductId);
        Assert.Equal("Smartphone", dto.Data.Name);
    }

    #endregion

    #region Bulk Processing Scenario

    [Fact]
    public void Scenario_BulkProcessing_ThousandRecords_ShouldBeEfficient()
    {
        // Arrange
        _mapper.Warmup<Product, ProductDto>();

        var products = Enumerable.Range(1, 1000).Select(i => new Product
        {
            ProductId = i,
            Name = $"Product {i}",
            Price = 10.0m * i,
            StockQuantity = i * 5,
            Category = new Category
            {
                CategoryId = i % 10,
                Name = $"Category {i % 10}",
                Description = $"Description for category {i % 10}"
            },
            CreatedAt = DateTime.Now.AddDays(-i)
        }).ToList();

        // Act
        var dtos = products.Select(p => _mapper.Map<Product, ProductDto>(p)).ToList();

        // Assert
        Assert.Equal(1000, dtos.Count);
        Assert.Equal(1, dtos[0].ProductId);
        Assert.Equal("Product 1", dtos[0].Name);
        Assert.Equal(1000, dtos[999].ProductId);
        Assert.Equal("Product 1000", dtos[999].Name);
        Assert.All(dtos, dto => Assert.NotNull(dto.Category));
    }

    #endregion

    #region Nullable Fields Scenario

    public class OptionalData
    {
        public int Id { get; set; }
        public string? OptionalText { get; set; }
        public int? OptionalNumber { get; set; }
        public DateTime? OptionalDate { get; set; }
        public decimal RequiredDecimal { get; set; }
    }

    public class OptionalDataDto
    {
        public int Id { get; set; }
        public string? OptionalText { get; set; }
        public int? OptionalNumber { get; set; }
        public DateTime? OptionalDate { get; set; }
        public decimal RequiredDecimal { get; set; }
    }

    [Fact]
    public void Scenario_NullableFields_MixedValues_ShouldMapCorrectly()
    {
        // Arrange
        var data = new OptionalData
        {
            Id = 100,
            OptionalText = null,
            OptionalNumber = 42,
            OptionalDate = null,
            RequiredDecimal = 123.45m
        };

        // Act
        var dto = _mapper.Map<OptionalData, OptionalDataDto>(data);

        // Assert
        Assert.Equal(100, dto.Id);
        Assert.Null(dto.OptionalText);
        Assert.Equal(42, dto.OptionalNumber);
        Assert.Null(dto.OptionalDate);
        Assert.Equal(123.45m, dto.RequiredDecimal);
    }

    #endregion

    #region Collection Pipeline Scenario

    [Fact]
    public void Scenario_CollectionPipeline_AllFormatsProduceSameResult()
    {
        // MapToList, MapToArray, MapBatch, and MapToList(Span) must all map identically
        var products = Enumerable.Range(1, 20).Select(i => new Product
        {
            ProductId = i,
            Name = $"Product {i}",
            Price = i * 9.99m,
            StockQuantity = i * 2,
            Category = new Category { CategoryId = i % 3, Name = $"Cat{i % 3}" },
            CreatedAt = new DateTime(2024, 1, i)
        }).ToList();

        _mapper.Warmup<Product, ProductDto>();

        List<ProductDto>  fromList  = _mapper.MapToList<Product, ProductDto>(products);
        ProductDto[]      fromArray = _mapper.MapToArray<Product, ProductDto>(products);
        ProductDto[]      fromBatch = _mapper.MapBatch<Product, ProductDto>(products.ToArray().AsSpan());
        List<ProductDto>  fromSpan  = _mapper.MapToList<Product, ProductDto>(products.ToArray().AsSpan());

        Assert.Equal(20, fromList.Count);
        Assert.Equal(20, fromArray.Length);
        Assert.Equal(20, fromBatch.Length);
        Assert.Equal(20, fromSpan.Count);

        for (int i = 0; i < 20; i++)
        {
            Assert.Equal(fromList[i].ProductId, fromArray[i].ProductId);
            Assert.Equal(fromList[i].ProductId, fromBatch[i].ProductId);
            Assert.Equal(fromList[i].ProductId, fromSpan[i].ProductId);
            Assert.Equal(fromList[i].Name, fromArray[i].Name);
        }
    }

    [Fact]
    public void Scenario_DictionaryMapping_WithStringKeys()
    {
        var catalog = new Dictionary<string, Product>
        {
            ["laptop"]     = new() { ProductId = 1, Name = "Laptop",     Price = 999m,  StockQuantity = 10, Category = new() { CategoryId = 1, Name = "Electronics" }, CreatedAt = DateTime.Now },
            ["smartphone"] = new() { ProductId = 2, Name = "Smartphone", Price = 599m,  StockQuantity = 30, Category = new() { CategoryId = 1, Name = "Electronics" }, CreatedAt = DateTime.Now },
            ["tablet"]     = new() { ProductId = 3, Name = "Tablet",     Price = 799m,  StockQuantity = 15, Category = new() { CategoryId = 1, Name = "Electronics" }, CreatedAt = DateTime.Now }
        };

        var result = _mapper.MapDictionary<string, Product, string, ProductDto>(catalog);

        Assert.Equal(3, result.Count);
        Assert.Equal(1, result["laptop"].ProductId);
        Assert.Equal(599m, result["smartphone"].Price);
        Assert.Equal("Tablet", result["tablet"].Name);
    }

    #endregion
}
