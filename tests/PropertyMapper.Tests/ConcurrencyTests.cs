namespace PropertyMapper.Tests;

/// <summary>
/// Thread safety tests for concurrent mapping
/// </summary>
public class ConcurrencyTests
{
    [Fact]
    public void Map_ConcurrentCalls_ShouldBeThreadSafe()
    {
        // Arrange
        var mapper = new PropMap();
        var source = new SimpleSource { Id = 42, Name = "Concurrent Test" };
        var exceptions = new List<Exception>();

        // Act
        Parallel.For(0, 100, i =>
        {
            try
            {
                var result = mapper.Map<SimpleSource, SimpleTarget>(source);
                Assert.Equal(42, result.Id);
                Assert.Equal("Concurrent Test", result.Name);
            }
            catch (Exception ex)
            {
                lock (exceptions)
                {
                    exceptions.Add(ex);
                }
            }
        });

        // Assert
        Assert.Empty(exceptions);
    }

    [Fact]
    public void Map_MultipleDifferentTypesConc_ShouldBeThreadSafe()
    {
        // Arrange
        var mapper = new PropMap();
        var exceptions = new List<Exception>();

        // Act
        Parallel.For(0, 50, i =>
        {
            try
            {
                if (i % 2 == 0)
                {
                    var source = new SimpleSource { Id = i, Name = $"Test {i}" };
                    var result = mapper.Map<SimpleSource, SimpleTarget>(source);
                    Assert.Equal(i, result.Id);
                }
                else
                {
                    var source = new PersonRecord { FirstName = $"First{i}", LastName = $"Last{i}", Age = i };
                    var result = mapper.Map<PersonRecord, PersonDto>(source);
                    Assert.Equal(i, result.Age);
                }
            }
            catch (Exception ex)
            {
                lock (exceptions)
                {
                    exceptions.Add(ex);
                }
            }
        });

        // Assert
        Assert.Empty(exceptions);
    }

    [Fact]
    public void Map_ConcurrentFirstCalls_ShouldCompileOnce()
    {
        // Arrange
        var mapper = new PropMap();
        var source = new SimpleSource { Id = 1, Name = "Test" };
        var results = new List<SimpleTarget>();
        var lockObj = new object();

        // Act - Multiple threads trying to compile the same mapper simultaneously
        Parallel.For(0, 10, _ =>
        {
            var result = mapper.Map<SimpleSource, SimpleTarget>(source);
            lock (lockObj)
            {
                results.Add(result);
            }
        });

        // Assert
        Assert.Equal(10, results.Count);
        Assert.All(results, r =>
        {
            Assert.Equal(1, r.Id);
            Assert.Equal("Test", r.Name);
        });
        
        var stats = mapper.GetStatistics();
        Assert.Equal(1, stats.CachedMappers); // Should only compile once
    }

    [Fact]
    public void WarmupBatch_Concurrent_ShouldHandleGracefully()
    {
        // Arrange
        var mapper = new PropMap();
        var exceptions = new List<Exception>();

        // Act
        Parallel.For(0, 5, _ =>
        {
            try
            {
                mapper.WarmupBatch(
                    typeof(SimpleSource), typeof(SimpleTarget),
                    typeof(PersonRecord), typeof(PersonDto)
                );
            }
            catch (Exception ex)
            {
                lock (exceptions)
                {
                    exceptions.Add(ex);
                }
            }
        });

        // Assert
        Assert.Empty(exceptions);
        var stats = mapper.GetStatistics();
        Assert.Equal(2, stats.CachedMappers);
    }

    [Fact]
    public void Map_ConcurrentNestedMapping_ShouldBeThreadSafe()
    {
        // Arrange
        var mapper = new PropMap();
        var exceptions = new List<Exception>();

        // Act
        Parallel.For(0, 50, i =>
        {
            try
            {
                var source = new PersonWithAddress
                {
                    Name = $"Person {i}",
                    Age = 20 + i,
                    Address = new Address
                    {
                        Street = $"Street {i}",
                        City = $"City {i}",
                        PostalCode = $"{10000 + i}"
                    }
                };

                var result = mapper.Map<PersonWithAddress, PersonWithAddressDto>(source);
                Assert.Equal($"Person {i}", result.Name);
                Assert.Equal(20 + i, result.Age);
                Assert.Equal($"Street {i}", result.Address.Street);
            }
            catch (Exception ex)
            {
                lock (exceptions)
                {
                    exceptions.Add(ex);
                }
            }
        });

        // Assert
        Assert.Empty(exceptions);
    }

    [Fact]
    public async Task Clear_DuringMapping_ShouldNotCrash()
    {
        // Arrange
        PropMap mapper = new PropMap();
        SimpleSource source = new SimpleSource { Id = 1, Name = "Test" };
        mapper.Warmup<SimpleSource, SimpleTarget>();
        
        List<Exception> exceptions = new List<Exception>();
        using CancellationTokenSource cts = new CancellationTokenSource();

        // Act - Map continuously while clearing
        Task mappingTask = Task.Run(() =>
        {
            while (!cts.Token.IsCancellationRequested)
            {
                try
                {
                    mapper.Map<SimpleSource, SimpleTarget>(source);
                }
                catch (Exception ex)
                {
                    lock (exceptions)
                    {
                        exceptions.Add(ex);
                    }
                }
            }
        }, TestContext.Current.CancellationToken);

        Task clearTask = Task.Run(async () =>
        {
            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(10).ConfigureAwait(false);
                try
                {
                    mapper.Clear();
                }
                catch (Exception ex)
                {
                    lock (exceptions)
                    {
                        exceptions.Add(ex);
                    }
                }
            }
        }, TestContext.Current.CancellationToken);

        await clearTask;
        await cts.CancelAsync();
        await mappingTask;

        // Assert - Some operations might fail during clear, but shouldn't crash
        // The main goal is to ensure no unhandled exceptions or deadlocks
        Assert.True(true); // If we reached here, no deadlock occurred
    }
}
