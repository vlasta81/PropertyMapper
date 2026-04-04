namespace PropertyMapper.Tests;

/// <summary>
/// Tests for advanced collections mapping
/// </summary>
public class CollectionMappingTests
{
    private readonly PropMap _mapper = new();

    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    [Fact]
    public void MapToList_ShouldMapAllItems()
    {
        // Arrange
        var items = new[]
        {
            new Item { Id = 1, Name = "Item 1" },
            new Item { Id = 2, Name = "Item 2" },
            new Item { Id = 3, Name = "Item 3" }
        };

        // Act
        var result = _mapper.MapToList<Item, ItemDto>(items);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal(1, result[0].Id);
        Assert.Equal("Item 1", result[0].Name);
        Assert.Equal(3, result[2].Id);
    }

    [Fact]
    public void MapToArray_ShouldMapAllItems()
    {
        // Arrange
        var items = new List<Item>
        {
            new Item { Id = 1, Name = "Item 1" },
            new Item { Id = 2, Name = "Item 2" }
        };

        // Act
        var result = _mapper.MapToArray<Item, ItemDto>(items);

        // Assert
        Assert.Equal(2, result.Length);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(2, result[1].Id);
    }

    [Fact]
    public void MapToHashSet_ShouldMapUniqueItems()
    {
        // Arrange
        var items = new[]
        {
            new Item { Id = 1, Name = "Item 1" },
            new Item { Id = 2, Name = "Item 2" }
        };

        // Act
        var result = _mapper.MapToHashSet<Item, ItemDto>(items);

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void MapDictionary_ShouldMapValues()
    {
        // Arrange
        var dict = new Dictionary<int, Item>
        {
            [1] = new Item { Id = 1, Name = "Item 1" },
            [2] = new Item { Id = 2, Name = "Item 2" }
        };

        // Act
        var result = _mapper.MapDictionary<int, Item, int, ItemDto>(dict);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Item 1", result[1].Name);
        Assert.Equal("Item 2", result[2].Name);
    }

    [Fact]
    public void MapNestedList_ShouldMapNestedStructure()
    {
        // Arrange
        var nested = new List<List<Item>>
        {
            new List<Item>
            {
                new Item { Id = 1, Name = "Item 1-1" },
                new Item { Id = 2, Name = "Item 1-2" }
            },
            new List<Item>
            {
                new Item { Id = 3, Name = "Item 2-1" }
            }
        };

        // Act
        var result = _mapper.MapNestedList<Item, ItemDto>(nested);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(2, result[0].Count);
        Assert.Single(result[1]);
        Assert.Equal("Item 1-1", result[0][0].Name);
    }

    [Fact]
    public void MapCollection_EmptyList_ShouldReturnEmpty()
    {
        // Arrange
        var items = new List<Item>();

        // Act
        var result = _mapper.MapToList<Item, ItemDto>(items);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void MapToList_NullSource_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => _mapper.MapToList<Item, ItemDto>((IEnumerable<Item>)null!));
    }

    [Fact]
    public void MapToArray_NullSource_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => _mapper.MapToArray<Item, ItemDto>(null!));
    }

    [Fact]
    public void MapToHashSet_NullSource_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => _mapper.MapToHashSet<Item, ItemDto>(null!));
    }

    [Fact]
    public void MapDictionary_NullSource_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => _mapper.MapDictionary<int, Item, int, ItemDto>(null!));
    }

    [Fact]
    public void MapNestedList_NullSource_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => _mapper.MapNestedList<Item, ItemDto>(null!));
    }

    [Fact]
    public void MapBatch_ShouldReturnArray()
    {
        // Arrange
        var items = new[]
        {
            new Item { Id = 10, Name = "Alpha" },
            new Item { Id = 20, Name = "Beta" },
            new Item { Id = 30, Name = "Gamma" }
        };

        // Act
        ItemDto[] result = _mapper.MapBatch<Item, ItemDto>(items);

        // Assert
        Assert.Equal(3, result.Length);
        Assert.Equal(10, result[0].Id);
        Assert.Equal("Alpha", result[0].Name);
        Assert.Equal(20, result[1].Id);
        Assert.Equal(30, result[2].Id);
        Assert.Equal("Gamma", result[2].Name);
    }

    [Fact]
    public void MapBatch_EmptySource_ShouldReturnEmptyArray()
    {
        Item[] items = [];
        ItemDto[] result = _mapper.MapBatch<Item, ItemDto>(items);
        Assert.Empty(result);
    }

    #region MapToList fast-path coverage

    [Fact]
    public void MapToList_FromList_MapsAllItemsViaSpanPath()
    {
        var items = new List<Item>
        {
            new() { Id = 1, Name = "A" },
            new() { Id = 2, Name = "B" },
            new() { Id = 3, Name = "C" }
        };

        var result = _mapper.MapToList<Item, ItemDto>(items);

        Assert.Equal(3, result.Count);
        Assert.Equal(1, result[0].Id);
        Assert.Equal("B", result[1].Name);
        Assert.Equal(3, result[2].Id);
    }

    [Fact]
    public void MapToList_FromArray_MapsAllItemsViaSpanPath()
    {
        Item[] items = [new() { Id = 10, Name = "X" }, new() { Id = 20, Name = "Y" }];

        var result = _mapper.MapToList<Item, ItemDto>(items);

        Assert.Equal(2, result.Count);
        Assert.Equal(10, result[0].Id);
        Assert.Equal("Y", result[1].Name);
    }

    [Fact]
    public void MapToList_FromICollection_UsesCountPath()
    {
        // Queue<T> implements ICollection<T> but is neither List<T> nor T[]
        IEnumerable<Item> items = new Queue<Item>(new[]
        {
            new Item { Id = 7, Name = "Q1" },
            new Item { Id = 8, Name = "Q2" }
        });

        var result = _mapper.MapToList<Item, ItemDto>(items);

        Assert.Equal(2, result.Count);
        Assert.Equal(7, result[0].Id);
        Assert.Equal(8, result[1].Id);
    }

    [Fact]
    public void MapToList_FromIEnumerable_GeneralPath_MapsAllItems()
    {
        // Lazy generator — not a List, array, or ICollection
        static IEnumerable<Item> Generate()
        {
            yield return new Item { Id = 100, Name = "G1" };
            yield return new Item { Id = 200, Name = "G2" };
        }

        var result = _mapper.MapToList<Item, ItemDto>(Generate());

        Assert.Equal(2, result.Count);
        Assert.Equal(100, result[0].Id);
        Assert.Equal(200, result[1].Id);
    }

    #endregion

    #region MapToList(ReadOnlySpan) overload

    [Fact]
    public void MapToList_FromSpan_MapsAllItems()
    {
        Item[] array = [new() { Id = 1, Name = "S1" }, new() { Id = 2, Name = "S2" }, new() { Id = 3, Name = "S3" }];
        ReadOnlySpan<Item> span = array.AsSpan();

        var result = _mapper.MapToList<Item, ItemDto>(span);

        Assert.Equal(3, result.Count);
        Assert.Equal(1, result[0].Id);
        Assert.Equal("S2", result[1].Name);
        Assert.Equal(3, result[2].Id);
    }

    [Fact]
    public void MapToList_FromSpan_EmptySpan_ReturnsEmpty()
    {
        var result = _mapper.MapToList<Item, ItemDto>(ReadOnlySpan<Item>.Empty);
        Assert.Empty(result);
    }

    [Fact]
    public void MapToList_FromSpan_PreservesOrder()
    {
        Item[] items = Enumerable.Range(1, 50)
            .Select(i => new Item { Id = i, Name = $"Item{i}" })
            .ToArray();

        var result = _mapper.MapToList<Item, ItemDto>(items.AsSpan());

        Assert.Equal(50, result.Count);
        for (int i = 0; i < 50; i++)
            Assert.Equal(i + 1, result[i].Id);
    }

    #endregion

    #region MapToArray fast-path coverage

    [Fact]
    public void MapToArray_FromList_UsesSpanFastPath()
    {
        var items = new List<Item>
        {
            new() { Id = 5, Name = "L1" },
            new() { Id = 6, Name = "L2" }
        };

        ItemDto[] result = _mapper.MapToArray<Item, ItemDto>(items);

        Assert.Equal(2, result.Length);
        Assert.Equal(5, result[0].Id);
        Assert.Equal("L2", result[1].Name);
    }

    [Fact]
    public void MapToArray_FromICollection_AllocatesResultOnce()
    {
        // Queue<T> implements ICollection<T> and provides Count
        IEnumerable<Item> items = new Queue<Item>(new[]
        {
            new Item { Id = 11, Name = "Q1" },
            new Item { Id = 12, Name = "Q2" },
            new Item { Id = 13, Name = "Q3" }
        });

        ItemDto[] result = _mapper.MapToArray<Item, ItemDto>(items);

        Assert.Equal(3, result.Length);
        Assert.Equal(11, result[0].Id);
        Assert.Equal(12, result[1].Id);
        Assert.Equal(13, result[2].Id);
    }

    [Fact]
    public void MapToArray_FromIEnumerable_GeneralFallback_MapsAllItems()
    {
        static IEnumerable<Item> Generate()
        {
            yield return new Item { Id = 1, Name = "G1" };
            yield return new Item { Id = 2, Name = "G2" };
        }

        ItemDto[] result = _mapper.MapToArray<Item, ItemDto>(Generate());

        Assert.Equal(2, result.Length);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(2, result[1].Id);
    }

    #endregion

    #region MapDictionary key conversion

    [Fact]
    public void MapDictionary_SameKeyType_MapsValues()
    {
        // TInKey == TOutKey → keyMapper is null, direct cast branch is used
        var source = new Dictionary<string, Item>
        {
            ["alpha"] = new() { Id = 1, Name = "Alpha" },
            ["beta"]  = new() { Id = 2, Name = "Beta" }
        };

        var result = _mapper.MapDictionary<string, Item, string, ItemDto>(source);

        Assert.Equal(2, result.Count);
        Assert.Equal(1, result["alpha"].Id);
        Assert.Equal("Beta", result["beta"].Name);
    }

    [Fact]
    public void MapDictionary_DifferentKeyTypes_UsesKeyCast()
    {
        // TInKey (string) != TOutKey (object) → KeyCast<string, object>.Instance is used
        var source = new Dictionary<string, Item>
        {
            ["x"] = new() { Id = 42, Name = "X" }
        };

        var result = _mapper.MapDictionary<string, Item, object, ItemDto>(source);

        Assert.Single(result);
        Assert.True(result.ContainsKey("x"));
        Assert.Equal(42, result["x"].Id);
    }

    #endregion

    #region MapCollection generic result type

    [Fact]
    public void MapCollection_WithHashSetResult_MapsAllItems()
    {
        var items = new List<Item>
        {
            new() { Id = 1, Name = "A" },
            new() { Id = 2, Name = "B" },
            new() { Id = 3, Name = "C" }
        };

        HashSet<ItemDto> result = _mapper.MapCollection<Item, ItemDto, HashSet<ItemDto>>(items);

        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void MapCollection_WithListResult_UsesSetCountFastPath()
    {
        Item[] items = Enumerable.Range(1, 10)
            .Select(i => new Item { Id = i, Name = $"Item{i}" })
            .ToArray();

        // List<T> result triggers the SetCount+AsSpan fast path in MapCollection
        List<ItemDto> result = _mapper.MapCollection<Item, ItemDto, List<ItemDto>>(items);

        Assert.Equal(10, result.Count);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(10, result[9].Id);
    }

    #endregion

    #region KeyCast validation

    /// <summary>
    /// Verifies that <see cref="PropMap.MapDictionary{TInKey,TInValue,TOutKey,TOutValue}"/>
    /// throws <see cref="InvalidOperationException"/> when the key types are incompatible.
    /// The guard lives in <c>KeyCast&lt;TIn,TOut&gt;.CreateCastDelegate()</c>, which returns a
    /// throwing delegate rather than throwing during static initialisation — this ensures the
    /// error surfaces as a clean <see cref="InvalidOperationException"/> at the call site on
    /// every invocation, avoiding the CLR's one-shot <see cref="TypeInitializationException"/>
    /// caching trap.
    /// </summary>
    [Fact]
    public void MapDictionary_IncompatibleKeyTypes_ThrowsInvalidOperationException()
    {
        var dict = new Dictionary<int, Item> { [1] = new Item { Id = 1, Name = "x" } };

        var ex = Assert.Throws<InvalidOperationException>(() =>
            _mapper.MapDictionary<int, Item, string, ItemDto>(dict));

        Assert.Contains("Int32", ex.Message);
        Assert.Contains("String", ex.Message);
    }

    #endregion

    #region Immutable collection mappings

    /// <summary>
    /// Verifies that <see cref="PropMap.MapToImmutableArray{TIn,TOut}"/> maps all elements
    /// and uses the array fast path when the source is a plain array.
    /// </summary>
    [Fact]
    public void MapToImmutableArray_ShouldMapAllItems()
    {
        var items = new[] { new Item { Id = 1, Name = "A" }, new Item { Id = 2, Name = "B" } };

        var result = _mapper.MapToImmutableArray<Item, ItemDto>(items);

        Assert.Equal(2, result.Length);
        Assert.Equal(1, result[0].Id);
        Assert.Equal("A", result[0].Name);
        Assert.Equal(2, result[1].Id);
        Assert.Equal("B", result[1].Name);
    }

    /// <summary>
    /// Verifies that <see cref="PropMap.MapToImmutableList{TIn,TOut}"/> maps all elements
    /// and returns an <see cref="System.Collections.Immutable.ImmutableList{T}"/>.
    /// </summary>
    [Fact]
    public void MapToImmutableList_ShouldMapAllItems()
    {
        var items = new[] { new Item { Id = 1, Name = "A" }, new Item { Id = 2, Name = "B" } };

        var result = _mapper.MapToImmutableList<Item, ItemDto>(items);

        Assert.Equal(2, result.Count);
        Assert.Equal(1, result[0].Id);
        Assert.Equal("A", result[0].Name);
        Assert.Equal(2, result[1].Id);
    }

    #endregion
}
