using System.Diagnostics;

namespace PropertyMapper.Tests;

/// <summary>
/// Performance and benchmark tests
/// </summary>
public class PerformanceTests
{
    [Fact]
    public void Map_FirstCall_ShouldCompileAndExecute()
    {
        // Arrange
        var mapper = new PropMap();
        var source = new SimpleSource { Id = 1, Name = "Performance Test" };
        var sw = Stopwatch.StartNew();

        // Act
        var result = mapper.Map<SimpleSource, SimpleTarget>(source);
        sw.Stop();

        // Assert
        Assert.Equal(1, result.Id);
        Assert.Equal("Performance Test", result.Name);
        // First call includes compilation overhead
        Assert.True(sw.ElapsedMilliseconds < 1000, $"First call took {sw.ElapsedMilliseconds}ms");
    }

    [Fact]
    public void Map_SubsequentCalls_ShouldBeFast()
    {
        // Arrange
        var mapper = new PropMap();
        var source = new SimpleSource { Id = 1, Name = "Test" };
        
        // Warm up
        mapper.Map<SimpleSource, SimpleTarget>(source);
        
        var sw = Stopwatch.StartNew();

        // Act
        for (int i = 0; i < 10000; i++)
        {
            mapper.Map<SimpleSource, SimpleTarget>(source);
        }
        sw.Stop();

        // Assert
        var avgMicroseconds = (sw.Elapsed.TotalMicroseconds / 10000);
        Assert.True(avgMicroseconds < 10, $"Average call took {avgMicroseconds:F2}μs");
    }

    [Fact]
    public void Map_NestedObjects_Performance()
    {
        // Arrange
        var mapper = new PropMap();
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

        // Warm up
        mapper.Map<PersonWithAddress, PersonWithAddressDto>(source);
        
        var sw = Stopwatch.StartNew();

        // Act
        for (int i = 0; i < 1000; i++)
        {
            mapper.Map<PersonWithAddress, PersonWithAddressDto>(source);
        }
        sw.Stop();

        // Assert
        var avgMicroseconds = (sw.Elapsed.TotalMicroseconds / 1000);
        Assert.True(avgMicroseconds < 500, $"Average nested mapping took {avgMicroseconds:F2}μs");
    }

    [Fact]
    public void Warmup_ShouldReduceFirstCallTime()
    {
        // Arrange
        var mapper = new PropMap();
        var source = new SimpleSource { Id = 1, Name = "Test" };

        // Act
        mapper.Warmup<SimpleSource, SimpleTarget>();
        
        var sw = Stopwatch.StartNew();
        var result = mapper.Map<SimpleSource, SimpleTarget>(source);
        sw.Stop();

        // Assert
        Assert.Equal(1, result.Id);
        // Should be very fast since it's pre-compiled (5 ms threshold accounts for Debug builds and debugger overhead)
        var elapsedMicroseconds = sw.Elapsed.TotalMicroseconds;
        Assert.True(elapsedMicroseconds < 5_000, $"Warmed up call took {elapsedMicroseconds:F2}μs");
    }

    [Fact]
    public void Map_ThousandDifferentObjects_ShouldBeFast()
    {
        // Arrange
        var mapper = new PropMap();
        mapper.Warmup<SimpleSource, SimpleTarget>();
        
        var sources = Enumerable.Range(1, 1000)
            .Select(i => new SimpleSource { Id = i, Name = $"Test {i}" })
            .ToList();

        var sw = Stopwatch.StartNew();

        // Act
        var results = sources.Select(s => mapper.Map<SimpleSource, SimpleTarget>(s)).ToList();
        sw.Stop();

        // Assert
        Assert.Equal(1000, results.Count);
        Assert.Equal(1, results[0].Id);
        Assert.Equal(1000, results[999].Id);
        Assert.True(sw.ElapsedMilliseconds < 100, $"Mapping 1000 objects took {sw.ElapsedMilliseconds}ms");
    }

    [Fact]
    public void Map_StructMapping_Performance()
    {
        // Arrange
        var mapper = new PropMap();
        var source = new PointStruct { X = 10, Y = 20, Label = "Test" };
        
        // Warm up
        mapper.Map<PointStruct, PointStruct>(source);
        
        var sw = Stopwatch.StartNew();

        // Act
        for (int i = 0; i < 100000; i++)
        {
            mapper.Map<PointStruct, PointStruct>(source);
        }
        sw.Stop();

        // Assert - Struct mapping should be extremely fast
        var avgNanoseconds = (sw.Elapsed.TotalMicroseconds * 1000 / 100000);
        Assert.True(avgNanoseconds < 1000, $"Average struct mapping took {avgNanoseconds:F2}ns");
    }

    [Fact]
    public void WarmupBatch_ShouldBeParallel()
    {
        // Arrange
        var mapper = new PropMap();
        var sw = Stopwatch.StartNew();

        // Act - Warmup multiple type pairs
        mapper.WarmupBatch(
            typeof(SimpleSource), typeof(SimpleTarget),
            typeof(PersonRecord), typeof(PersonDto),
            typeof(PointStruct), typeof(PointStruct),
            typeof(PersonWithAddress), typeof(PersonWithAddressDto)
        );
        sw.Stop();

        // Assert
        var stats = mapper.GetStatistics();
        Assert.Equal(4, stats.CachedMappers);
        // Parallel warmup should be faster than sequential
        Assert.True(sw.ElapsedMilliseconds < 500, $"Batch warmup took {sw.ElapsedMilliseconds}ms");
    }
}
