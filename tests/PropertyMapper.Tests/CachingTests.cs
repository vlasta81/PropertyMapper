namespace PropertyMapper.Tests;

/// <summary>
/// Tests for caching and performance
/// </summary>
public class CachingTests
{
    [Fact]
    public void Map_SameTypePairMultipleTimes_ShouldUseCache()
    {
        // Arrange
        var mapper = new PropMap();
        var source1 = new SimpleSource { Id = 1, Name = "First" };
        var source2 = new SimpleSource { Id = 2, Name = "Second" };
        var source3 = new SimpleSource { Id = 3, Name = "Third" };

        // Act
        var result1 = mapper.Map<SimpleSource, SimpleTarget>(source1);
        var stats1 = mapper.GetStatistics();
        
        var result2 = mapper.Map<SimpleSource, SimpleTarget>(source2);
        var stats2 = mapper.GetStatistics();
        
        var result3 = mapper.Map<SimpleSource, SimpleTarget>(source3);
        var stats3 = mapper.GetStatistics();

        // Assert
        Assert.Equal(1, result1.Id);
        Assert.Equal(2, result2.Id);
        Assert.Equal(3, result3.Id);
        
        // Cache should be populated after first call
        Assert.Equal(1, stats1.CachedMappers);
        Assert.Equal(1, stats2.CachedMappers);
        Assert.Equal(1, stats3.CachedMappers);
    }

    [Fact]
    public void Map_DifferentTypePairs_ShouldCreateSeparateEntries()
    {
        // Arrange
        var mapper = new PropMap();
        var simpleSource = new SimpleSource { Id = 1, Name = "Test" };
        var nestedSource = new PersonWithAddress 
        { 
            Name = "John", 
            Age = 30,
            Address = new Address { Street = "Main St", City = "NYC", PostalCode = "10001" }
        };

        // Act
        var simpleResult = mapper.Map<SimpleSource, SimpleTarget>(simpleSource);
        var nestedResult = mapper.Map<PersonWithAddress, PersonWithAddressDto>(nestedSource);
        var stats = mapper.GetStatistics();

        // Assert
        Assert.Equal(1, simpleResult.Id);
        Assert.Equal("John", nestedResult.Name);
        Assert.True(stats.CachedMappers >= 2); // At least 2 type pairs cached
    }

    [Fact]
    public void Clear_ShouldRemoveAllCachedMappers()
    {
        // Arrange
        var mapper = new PropMap();
        var source = new SimpleSource { Id = 1, Name = "Test" };
        
        // Act
        var result1 = mapper.Map<SimpleSource, SimpleTarget>(source);
        var statsBefore = mapper.GetStatistics();
        
        mapper.Clear();
        var statsAfter = mapper.GetStatistics();
        
        var result2 = mapper.Map<SimpleSource, SimpleTarget>(source);
        var statsFinal = mapper.GetStatistics();

        // Assert
        Assert.Equal(1, statsBefore.CachedMappers);
        Assert.Equal(0, statsAfter.CachedMappers);
        Assert.Equal(1, statsFinal.CachedMappers); // Recompiled after clear
    }

    [Fact]
    public void Warmup_ShouldPrecompileMapper()
    {
        // Arrange
        var mapper = new PropMap();
        
        // Act
        mapper.Warmup<SimpleSource, SimpleTarget>();
        var statsAfterWarmup = mapper.GetStatistics();
        
        var source = new SimpleSource { Id = 42, Name = "Test" };
        var result = mapper.Map<SimpleSource, SimpleTarget>(source);

        // Assert
        Assert.Equal(1, statsAfterWarmup.CachedMappers);
        Assert.Equal(42, result.Id);
        Assert.Equal("Test", result.Name);
    }

    [Fact]
    public void Warmup_CalledTwice_ShouldNotDuplicate()
    {
        // Arrange
        var mapper = new PropMap();
        
        // Act
        mapper.Warmup<SimpleSource, SimpleTarget>();
        var stats1 = mapper.GetStatistics();
        
        mapper.Warmup<SimpleSource, SimpleTarget>();
        var stats2 = mapper.GetStatistics();

        // Assert
        Assert.Equal(1, stats1.CachedMappers);
        Assert.Equal(1, stats2.CachedMappers);
    }

    [Fact]
    public void WarmupBatch_ShouldPrecompileMultipleMappers()
    {
        // Arrange
        var mapper = new PropMap();
        
        // Act
        mapper.WarmupBatch(
            typeof(SimpleSource), typeof(SimpleTarget),
            typeof(PersonRecord), typeof(PersonDto)
        );
        var stats = mapper.GetStatistics();

        // Assert
        Assert.Equal(2, stats.CachedMappers);
    }

    [Fact]
    public void WarmupBatch_WithOddCount_ShouldThrowException()
    {
        // Arrange
        var mapper = new PropMap();
        
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            mapper.WarmupBatch(typeof(SimpleSource), typeof(SimpleTarget), typeof(PersonRecord))
        );
    }

    [Fact]
    public void GetStatistics_ShouldReturnCorrectCounts()
    {
        // Arrange
        var mapper = new PropMap();
        
        // Act
        var statsEmpty = mapper.GetStatistics();
        
        mapper.Map<SimpleSource, SimpleTarget>(new SimpleSource());
        var stats1 = mapper.GetStatistics();
        
        mapper.Map<PersonRecord, PersonDto>(new PersonRecord { FirstName = "A", LastName = "B", Age = 1 });
        var stats2 = mapper.GetStatistics();

        // Assert
        Assert.Equal(0, statsEmpty.CachedMappers);
        Assert.Equal(1, stats1.CachedMappers);
        Assert.True(stats2.CachedMappers >= 2);
    }
}
