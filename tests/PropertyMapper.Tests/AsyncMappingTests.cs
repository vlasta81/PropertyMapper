namespace PropertyMapper.Tests;

/// <summary>
/// Tests for async mapping functionality
/// </summary>
public class AsyncMappingTests
{
    private readonly PropMap _mapper = new();
    private static CancellationToken Ct => TestContext.Current.CancellationToken;

    public class AsyncSource
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class AsyncTarget
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    [Fact]
    public async Task MapAsync_ShouldMapObject()
    {
        // Arrange
        var source = new AsyncSource { Id = 1, Name = "Test" };

        // Act
        AsyncTarget result = await _mapper.MapAsync<AsyncSource, AsyncTarget>(source, Ct);

        // Assert
        Assert.Equal(1, result.Id);
        Assert.Equal("Test", result.Name);
    }

    [Fact]
    public async Task MapAsync_Collection_ShouldMapAllItems()
    {
        // Arrange
        var sources = new[]
        {
            new AsyncSource { Id = 1, Name = "Item 1" },
            new AsyncSource { Id = 2, Name = "Item 2" },
            new AsyncSource { Id = 3, Name = "Item 3" }
        };

        // Act
        List<AsyncTarget> result = await _mapper.MapAsync<AsyncSource, AsyncTarget>(sources, Ct);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(3, result[2].Id);
    }

    [Fact]
    public async Task MapParallelAsync_ShouldMapInParallel()
    {
        // Arrange
        var sources = Enumerable.Range(1, 100)
            .Select(i => new AsyncSource { Id = i, Name = $"Item {i}" })
            .ToList();

        // Act
        List<AsyncTarget> result = await _mapper.MapParallelAsync<AsyncSource, AsyncTarget>(sources, cancellationToken: Ct);

        // Assert
        Assert.Equal(100, result.Count);
        Assert.All(result, item => Assert.True(item.Id > 0));
    }

    [Fact]
    public async Task MapStreamAsync_ShouldStreamResults()
    {
        // Arrange
        var sources = GenerateAsyncItems();
        var results = new List<AsyncTarget>();

        // Act
        await foreach (AsyncTarget item in _mapper.MapStreamAsync<AsyncSource, AsyncTarget>(sources, Ct))
        {
            results.Add(item);
        }

        // Assert
        Assert.Equal(5, results.Count);
    }

    [Fact]
    public async Task MapAsync_WithCancellation_ShouldRespectToken()
    {
        // Arrange — cancel the token before issuing the call; Task.Run will throw
        // OperationCanceledException without executing the work item.
        using CancellationTokenSource cts = new CancellationTokenSource();
        await cts.CancelAsync();

        var source = new AsyncSource { Id = 1, Name = "Cancelled" };

        // Act & Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
            await _mapper.MapAsync<AsyncSource, AsyncTarget>(source, cts.Token));
    }

    /// <summary>
    /// Verifies that <c>cancellationToken.ThrowIfCancellationRequested()</c> inside the foreach loop
    /// causes <see cref="OperationCanceledException"/> when the token is cancelled after
    /// the work item has already started executing (mid-loop cancellation).
    /// </summary>
    [Fact]
    public async Task MapAsync_Collection_CancellationMidLoop_ThrowsOperationCanceledException()
    {
        // Arrange — large list so the loop is still running when we cancel
        using CancellationTokenSource cts = new CancellationTokenSource();
        IEnumerable<AsyncSource> sources = Enumerable.Range(1, 100_000)
            .Select(i => new AsyncSource { Id = i, Name = $"Item {i}" });

        Task<List<AsyncTarget>> task = _mapper.MapAsync<AsyncSource, AsyncTarget>(sources, cts.Token);

        // Cancel while Task.Run body is still iterating
        await cts.CancelAsync();

        // Act & Assert
        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () => await task);
    }

    [Fact]
    public async Task MapAsync_Collection_NullSource_Throws()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _mapper.MapAsync<AsyncSource, AsyncTarget>((IEnumerable<AsyncSource>)null!, Ct));
    }

    [Fact]
    public async Task MapParallelAsync_NullSource_Throws()
    {
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await _mapper.MapParallelAsync<AsyncSource, AsyncTarget>(null!, cancellationToken: Ct));
    }

    [Fact]
    public async Task MapStreamBatchedAsync_ShouldYieldBatchedResults()
    {
        // Arrange — 5 items with batchSize=2 should produce batches: [2, 2, 1]
        var sources = GenerateAsyncItems();
        var batches = new List<List<AsyncTarget>>();

        // Act
        await foreach (List<AsyncTarget> batch in _mapper.MapStreamBatchedAsync<AsyncSource, AsyncTarget>(sources, batchSize: 2, cancellationToken: Ct))
            batches.Add(batch);

        // Assert
        Assert.Equal(3, batches.Count);
        Assert.Equal(2, batches[0].Count);
        Assert.Equal(2, batches[1].Count);
        Assert.Single(batches[2]);

        // Content is preserved
        Assert.Equal(1, batches[0][0].Id);
        Assert.Equal(5, batches[2][0].Id);
    }

    [Fact]
    public async Task MapStreamBatchedAsync_BatchLargerThanSource_YieldsSingleBatch()
    {
        // batchSize exceeds total item count → trailing flush emits one batch
        var sources = GenerateAsyncItems(); // 5 items
        var batches = new List<List<AsyncTarget>>();

        await foreach (List<AsyncTarget> batch in _mapper.MapStreamBatchedAsync<AsyncSource, AsyncTarget>(sources, batchSize: 100, cancellationToken: Ct))
            batches.Add(batch);

        Assert.Single(batches);
        Assert.Equal(5, batches[0].Count);
        Assert.Equal(1, batches[0][0].Id);
    }

    [Fact]
    public async Task MapStreamAsync_WithCancellation_ShouldHonourToken()
    {
        // Arrange — generator that respects cancellation via [EnumeratorCancellation]
        using CancellationTokenSource cts = new CancellationTokenSource();
        List<AsyncTarget> results = new List<AsyncTarget>();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(async () =>
        {
#pragma warning disable xUnit1051 // intentional: test verifies cancellation via custom CTS
            await foreach (AsyncTarget item in _mapper.MapStreamAsync<AsyncSource, AsyncTarget>(
                GenerateCancellableItems(), cts.Token))
#pragma warning restore xUnit1051
            {
                results.Add(item);
                if (results.Count >= 3)
                    await cts.CancelAsync();
            }
        });

        // Exactly 3 items yielded before cancellation took effect
        Assert.Equal(3, results.Count);
    }

    private async IAsyncEnumerable<AsyncSource> GenerateAsyncItems()
    {
        for (int i = 1; i <= 5; i++)
        {
            await Task.Delay(1); // Simulate async operation
            yield return new AsyncSource { Id = i, Name = $"Async Item {i}" };
        }
    }

    private async IAsyncEnumerable<AsyncSource> GenerateCancellableItems(
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        for (int i = 1; i <= 20; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Yield();
            yield return new AsyncSource { Id = i, Name = $"Item {i}" };
        }
    }

    #region Order and correctness

    [Fact]
    public async Task MapParallelAsync_PreservesSourceOrder()
    {
        // 500 items; parallel indexed writes must land in the correct slot
        var sources = Enumerable.Range(1, 500)
            .Select(i => new AsyncSource { Id = i, Name = $"Item{i}" })
            .ToList();

        List<AsyncTarget> result = await _mapper.MapParallelAsync<AsyncSource, AsyncTarget>(sources, cancellationToken: Ct);

        Assert.Equal(500, result.Count);
        for (int i = 0; i < 500; i++)
            Assert.Equal(i + 1, result[i].Id);
    }

    [Fact]
    public async Task MapParallelAsync_WithArraySource_UsesCastPath()
    {
        // T[] implements IReadOnlyList<T> — the "source as IReadOnlyList<T>" cast succeeds
        // without a ToList() copy
        AsyncSource[] sources = Enumerable.Range(1, 20)
            .Select(i => new AsyncSource { Id = i, Name = $"A{i}" })
            .ToArray();

        List<AsyncTarget> result = await _mapper.MapParallelAsync<AsyncSource, AsyncTarget>(sources, cancellationToken: Ct);

        Assert.Equal(20, result.Count);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(20, result[19].Id);
    }

    [Fact]
    public async Task MapAsync_Collection_WithHashSet_UsesICollectionPath()
    {
        // HashSet<T> implements ICollection<T>; MapAsync(IEnumerable) pre-allocates the result
        // list with Count via the ICollection branch
        var sources = new HashSet<AsyncSource>(
            Enumerable.Range(1, 5).Select(i => new AsyncSource { Id = i, Name = $"H{i}" }));

        List<AsyncTarget> result = await _mapper.MapAsync<AsyncSource, AsyncTarget>(sources, Ct);

        Assert.Equal(5, result.Count);
        Assert.All(result, item => Assert.True(item.Id >= 1 && item.Id <= 5));
    }

    [Fact]
    public async Task MapStreamBatchedAsync_ContentPreservedAcrossBatches()
    {
        // 6 items, batchSize=2 → 3 batches; verify exact content in each slot
        var sources = GenerateSixItems();
        var batches = new List<List<AsyncTarget>>();

        await foreach (List<AsyncTarget> batch in _mapper.MapStreamBatchedAsync<AsyncSource, AsyncTarget>(sources, batchSize: 2, cancellationToken: Ct))
            batches.Add(batch);

        Assert.Equal(3, batches.Count);

        Assert.Equal(1, batches[0][0].Id);
        Assert.Equal("Async Item 1", batches[0][0].Name);
        Assert.Equal(2, batches[0][1].Id);

        Assert.Equal(3, batches[1][0].Id);
        Assert.Equal(4, batches[1][1].Id);

        Assert.Equal(5, batches[2][0].Id);
        Assert.Equal(6, batches[2][1].Id);
    }

    [Fact]
    public async Task MapStreamBatchedAsync_SingleItem_YieldsSingleBatch()
    {
        var sources = GenerateSingleItem();
        var batches = new List<List<AsyncTarget>>();

        await foreach (List<AsyncTarget> batch in _mapper.MapStreamBatchedAsync<AsyncSource, AsyncTarget>(sources, batchSize: 10, cancellationToken: Ct))
            batches.Add(batch);

        Assert.Single(batches);
        Assert.Single(batches[0]);
        Assert.Equal(99, batches[0][0].Id);
    }

    [Fact]
    public async Task MapStreamBatchedAsync_ExactMultipleOfBatchSize_NoEmptyTrailingBatch()
    {
        // 4 items, batchSize=2 → exactly 2 full batches; no empty trailing flush
        var sources = GenerateFourItems();
        var batches = new List<List<AsyncTarget>>();

        await foreach (List<AsyncTarget> batch in _mapper.MapStreamBatchedAsync<AsyncSource, AsyncTarget>(sources, batchSize: 2, cancellationToken: Ct))
            batches.Add(batch);

        Assert.Equal(2, batches.Count);
        Assert.Equal(2, batches[0].Count);
        Assert.Equal(2, batches[1].Count);
        Assert.All(batches, b => Assert.NotEmpty(b));
    }

    private async IAsyncEnumerable<AsyncSource> GenerateSixItems()
    {
        for (int i = 1; i <= 6; i++)
        {
            await Task.Yield();
            yield return new AsyncSource { Id = i, Name = $"Async Item {i}" };
        }
    }

    private async IAsyncEnumerable<AsyncSource> GenerateSingleItem()
    {
        await Task.Yield();
        yield return new AsyncSource { Id = 99, Name = "Solo" };
    }

    private async IAsyncEnumerable<AsyncSource> GenerateFourItems()
    {
        for (int i = 1; i <= 4; i++)
        {
            await Task.Yield();
            yield return new AsyncSource { Id = i, Name = $"Item{i}" };
        }
    }

    #endregion

    #region batchSize guard

    /// <summary>
    /// Verifies that a negative <c>batchSize</c> argument raises
    /// <see cref="ArgumentOutOfRangeException"/> as soon as iteration begins.
    /// The guard lives inside the async iterator body, so it only fires on the
    /// first <c>MoveNextAsync</c> call.
    /// </summary>
    [Fact]
    public async Task MapStreamBatchedAsync_NegativeBatchSize_Throws()
    {
        static async IAsyncEnumerable<AsyncSource> Empty()
        { yield break; }

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
        {
            await foreach (List<AsyncTarget> _ in _mapper.MapStreamBatchedAsync<AsyncSource, AsyncTarget>(
                Empty(), batchSize: -1, cancellationToken: Ct))
            {
                Assert.Fail("Iterator should not yield before throwing.");
            }
        });
    }

    /// <summary>
    /// Verifies that a zero <c>batchSize</c> argument raises
    /// <see cref="ArgumentOutOfRangeException"/> as soon as iteration begins.
    /// </summary>
    [Fact]
    public async Task MapStreamBatchedAsync_ZeroBatchSize_Throws()
    {
        static async IAsyncEnumerable<AsyncSource> Empty()
        { yield break; }

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
        {
            await foreach (List<AsyncTarget> _ in _mapper.MapStreamBatchedAsync<AsyncSource, AsyncTarget>(
                Empty(), batchSize: 0, cancellationToken: Ct))
            {
                Assert.Fail("Iterator should not yield before throwing.");
            }
        });
    }

    #endregion
}
