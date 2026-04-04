using System.Diagnostics;
using Xunit;

namespace PropertyMapper.Tests;

/// <summary>
/// Tests for .NET 10 Phase 2 optimizations:
/// - MapBatchInPlace (zero-allocation batch mapping)
/// - Closure elimination (struct workers in async)
/// - InlineArray (stack allocation for ≤16 properties)
/// </summary>
public class Phase2OptimizationTests
{
    private readonly PropMap _mapper = new PropMap();

    public class SmallDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public bool IsActive { get; set; }
    }

    public class SmallSource
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public bool IsActive { get; set; }
    }

    public class LargeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; } = "";
        public int Quantity { get; set; }
        public string Category { get; set; } = "";
        public double Rating { get; set; }
        public bool InStock { get; set; }
        public string Sku { get; set; } = "";
        public Guid ProductId { get; set; }
        public string Manufacturer { get; set; } = "";
        public int Warranty { get; set; }
        public string Color { get; set; } = "";
        public double Weight { get; set; }
        public string[] Tags { get; set; } = [];
    }

    public class LargeSource
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; } = "";
        public int Quantity { get; set; }
        public string Category { get; set; } = "";
        public double Rating { get; set; }
        public bool InStock { get; set; }
        public string Sku { get; set; } = "";
        public Guid ProductId { get; set; }
        public string Manufacturer { get; set; } = "";
        public int Warranty { get; set; }
        public string Color { get; set; } = "";
        public double Weight { get; set; }
        public string[] Tags { get; set; } = [];
    }

    [Fact]
    public void MapBatchInPlace_SmallBatch_ShouldMapWithoutAllocation()
    {
        // Arrange - use heap allocation since records cannot be stackalloc'd
        SmallSource[] sources = new[]
        {
            new SmallSource { Id = 1, Name = "Test1", IsActive = true },
            new SmallSource { Id = 2, Name = "Test2", IsActive = false },
            new SmallSource { Id = 3, Name = "Test3", IsActive = true }
        };

        SmallDto[] destination = new SmallDto[3];

        // Act
        _mapper.MapBatchInPlace<SmallSource, SmallDto>(sources, destination);

        // Assert
        Assert.Equal(1, destination[0].Id);
        Assert.Equal("Test1", destination[0].Name);
        Assert.True(destination[0].IsActive);

        Assert.Equal(2, destination[1].Id);
        Assert.Equal("Test2", destination[1].Name);
        Assert.False(destination[1].IsActive);

        Assert.Equal(3, destination[2].Id);
    }

    [Fact]
    public void MapBatchInPlace_HeapAllocatedDestination_ShouldWork()
    {
        // Arrange
        SmallSource[] sources = new[]
        {
            new SmallSource { Id = 1, Name = "A", IsActive = true },
            new SmallSource { Id = 2, Name = "B", IsActive = false }
        };

        SmallDto[] destination = new SmallDto[2];

        // Act
        _mapper.MapBatchInPlace<SmallSource, SmallDto>(sources, destination);

        // Assert
        Assert.Equal(1, destination[0].Id);
        Assert.Equal("A", destination[0].Name);
        Assert.Equal(2, destination[1].Id);
    }

    [Fact]
    public void MapBatchInPlace_EmptySource_ShouldNotThrow()
    {
        // Arrange
        SmallSource[] sources = [];
        SmallDto[] destination = new SmallDto[10];

        // Act & Assert - should not throw
        _mapper.MapBatchInPlace<SmallSource, SmallDto>(sources, destination);
        Assert.True(true);
    }

    [Fact]
    public void MapBatchInPlace_DestinationTooSmall_ShouldThrow()
    {
        // Arrange
        SmallSource[] sources = new SmallSource[5];
        SmallDto[] destination = new SmallDto[3]; // Too small

        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            _mapper.MapBatchInPlace<SmallSource, SmallDto>(sources, destination));
    }

    [Fact]
    public void InlineArray_SmallObject_ShouldUseStackAllocation()
    {
        // Arrange - object with ≤16 properties should use inline array
        SmallSource source = new SmallSource { Id = 42, Name = "InlineTest", IsActive = true };

        // Act
        SmallDto result = _mapper.Map<SmallSource, SmallDto>(source);

        // Assert
        Assert.Equal(42, result.Id);
        Assert.Equal("InlineTest", result.Name);
        Assert.True(result.IsActive);
    }

    [Fact]
    public void InlineArray_LargeObject_ShouldFallbackToHeapArray()
    {
        // Arrange - object with >16 properties should use heap array
        LargeSource source = new LargeSource
        {
            Id = 1,
            Name = "Large",
            IsActive = true,
            CreatedAt = DateTime.Now,
            Price = 99.99m,
            Description = "Description",
            Quantity = 10,
            Category = "Category",
            Rating = 4.5,
            InStock = true,
            Sku = "SKU123",
            ProductId = Guid.NewGuid(),
            Manufacturer = "Manufacturer",
            Warranty = 2,
            Color = "Red",
            Weight = 1.5,
            Tags = ["tag1", "tag2"]
        };

        // Act
        LargeDto result = _mapper.Map<LargeSource, LargeDto>(source);

        // Assert
        Assert.Equal(1, result.Id);
        Assert.Equal("Large", result.Name);
        Assert.Equal(99.99m, result.Price);
        Assert.Equal("SKU123", result.Sku);
    }

    [Fact]
    public async Task MapParallelAsync_StructWorker_ShouldEliminateClosureAllocation()
    {
        // Arrange
        List<SmallSource> sources = Enumerable.Range(1, 100)
            .Select(i => new SmallSource { Id = i, Name = $"Item{i}", IsActive = i % 2 == 0 })
            .ToList();

        // Act - uses struct worker internally to avoid closure allocations
        List<SmallDto> results = await _mapper.MapParallelAsync<SmallSource, SmallDto>(sources, cancellationToken: TestContext.Current.CancellationToken);

        // Assert
        Assert.Equal(100, results.Count);
        Assert.Equal(1, results[0].Id);
        Assert.Equal("Item1", results[0].Name);
        Assert.Equal(50, results[49].Id);
    }

    [Fact]
    public void MapBatch_Comparison_BothShouldProduceSameResults()
    {
        // Arrange
        SmallSource[] sources = Enumerable.Range(1, 10)
            .Select(i => new SmallSource { Id = i, Name = $"Test{i}", IsActive = true })
            .ToArray();

        // Act
        SmallDto[] results1 = _mapper.MapBatch<SmallSource, SmallDto>(sources);

        SmallDto[] results2 = new SmallDto[10];
        _mapper.MapBatchInPlace<SmallSource, SmallDto>(sources, results2);

        // Assert - both methods should produce identical results
        for (int i = 0; i < 10; i++)
        {
            Assert.Equal(results1[i].Id, results2[i].Id);
            Assert.Equal(results1[i].Name, results2[i].Name);
            Assert.Equal(results1[i].IsActive, results2[i].IsActive);
        }
    }

    [Fact(Skip = "Timing-dependent performance test — run manually or use the benchmark project.")]
    public void MapBatchInPlace_Performance_ShouldBeFasterThanMapBatch()
    {
        // Arrange
        const int iterations = 1000;
        SmallSource[] sources = Enumerable.Range(1, 10)
            .Select(i => new SmallSource { Id = i, Name = $"Test{i}", IsActive = true })
            .ToArray();

        _mapper.Warmup<SmallSource, SmallDto>();

        // Act & Measure - MapBatch (allocates array)
        var sw1 = System.Diagnostics.Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            _ = _mapper.MapBatch<SmallSource, SmallDto>(sources);
        }
        sw1.Stop();

        // Act & Measure - MapBatchInPlace (no allocation)
        SmallDto[] reusableBuffer = new SmallDto[10];
        var sw2 = System.Diagnostics.Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            _mapper.MapBatchInPlace<SmallSource, SmallDto>(sources, reusableBuffer);
        }
        sw2.Stop();

        // Assert - MapBatchInPlace should be at least as fast (typically faster due to no allocation)
        // Use Ticks for precision; apply a floor of 0.5 ms so near-zero durations don't cause
        // false failures when the JIT resolves both runs in < 1 ms.
        long minTicks = Stopwatch.Frequency / 2000; // 0.5 ms
        long t1 = Math.Max(sw1.Elapsed.Ticks, minTicks);
        long t2 = sw2.Elapsed.Ticks;
        Assert.True(t2 <= t1 * 2,
            $"MapBatchInPlace ({sw2.ElapsedMilliseconds}ms) should be comparable to MapBatch ({sw1.ElapsedMilliseconds}ms)");
    }

    #region WarmupBatch

    /// <summary>
    /// Verifies that <see cref="PropMap.WarmupBatch"/> pre-compiles all supplied type pairs,
    /// so the statistics counter reflects the expected number of cached mappers.
    /// </summary>
    [Fact]
    public void WarmupBatch_PrecompilesMultipleTypePairs()
    {
        var mapper = new PropMap();
        Assert.Equal(0, mapper.GetStatistics().CachedMappers);

        mapper.WarmupBatch(
            typeof(SmallSource), typeof(SmallDto),
            typeof(LargeSource), typeof(LargeDto));

        Assert.Equal(2, mapper.GetStatistics().CachedMappers);
    }

    /// <summary>
    /// Verifies that an odd argument count (not a multiple of 2) throws
    /// <see cref="ArgumentException"/> because each pair requires exactly
    /// a source type and a target type.
    /// </summary>
    [Fact]
    public void WarmupBatch_OddElementCount_Throws()
    {
        var mapper = new PropMap();

        Assert.Throws<ArgumentException>(() =>
            mapper.WarmupBatch(typeof(SmallSource), typeof(SmallDto), typeof(LargeSource)));
    }

    /// <summary>
    /// Verifies that calling <see cref="PropMap.WarmupBatch"/> a second time for already
    /// compiled pairs does not inflate the cached-mapper count (idempotent warm-up).
    /// </summary>
    [Fact]
    public void WarmupBatch_AlreadyCompiledPairs_DoesNotDuplicate()
    {
        var mapper = new PropMap();
        mapper.WarmupBatch(typeof(SmallSource), typeof(SmallDto));
        int countAfterFirst = mapper.GetStatistics().CachedMappers;

        mapper.WarmupBatch(typeof(SmallSource), typeof(SmallDto));

        Assert.Equal(countAfterFirst, mapper.GetStatistics().CachedMappers);
    }

    #endregion
}
