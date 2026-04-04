using System.Collections.Immutable;

namespace PropertyMapper.Tests;

public class ImmutableCollectionMappingTests
{
    private readonly PropMap _mapper = new();

    #region ImmutableArray

    [Fact]
    public void MapToImmutableArray_FromArray_MapsAllItems()
    {
        var sources = new[] { new SimpleSource { Id = 1 }, new SimpleSource { Id = 2 } };

        ImmutableArray<SimpleTarget> result = _mapper.MapToImmutableArray<SimpleSource, SimpleTarget>(sources);

        Assert.Equal(2, result.Length);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(2, result[1].Id);
    }

    [Fact]
    public void MapToImmutableArray_FromList_MapsAllItems()
    {
        var sources = new List<SimpleSource> { new() { Id = 10 }, new() { Id = 20 } };

        ImmutableArray<SimpleTarget> result = _mapper.MapToImmutableArray<SimpleSource, SimpleTarget>(sources);

        Assert.Equal(2, result.Length);
        Assert.Equal(10, result[0].Id);
        Assert.Equal(20, result[1].Id);
    }

    [Fact]
    public void MapToImmutableArray_EmptySource_ReturnsEmptyArray()
    {
        ImmutableArray<SimpleTarget> result =
            _mapper.MapToImmutableArray<SimpleSource, SimpleTarget>(Array.Empty<SimpleSource>());

        Assert.True(result.IsEmpty);
    }

    [Fact]
    public void MapToImmutableArray_NullSource_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => _mapper.MapToImmutableArray<SimpleSource, SimpleTarget>(null!));
    }

    [Fact]
    public void MapToImmutableArray_IsImmutable()
    {
        var sources = new[] { new SimpleSource { Id = 1 } };
        ImmutableArray<SimpleTarget> result = _mapper.MapToImmutableArray<SimpleSource, SimpleTarget>(sources);

        // ImmutableArray has no mutation methods — verifying it is ImmutableArray<T> (value type)
        Assert.IsType<ImmutableArray<SimpleTarget>>(result);
    }

    [Fact]
    public void MapToImmutableArray_FromICollection_MapsAllItems()
    {
        // Queue<T> implements ICollection<T> but is neither List<T> nor T[] — hits the ICollection path
        IEnumerable<SimpleSource> sources = new Queue<SimpleSource>(new[]
        {
            new SimpleSource { Id = 1, Name = "A" },
            new SimpleSource { Id = 2, Name = "B" },
            new SimpleSource { Id = 3, Name = "C" }
        });

        ImmutableArray<SimpleTarget> result =
            _mapper.MapToImmutableArray<SimpleSource, SimpleTarget>(sources);

        Assert.Equal(3, result.Length);
        Assert.Equal(1, result[0].Id);
        Assert.Equal(2, result[1].Id);
        Assert.Equal(3, result[2].Id);
    }

    [Fact]
    public void MapToImmutableArray_FromIEnumerable_GeneralPath()
    {
        // Lazy generator — not a List, array, or ICollection; hits the ToImmutable() fallback
        static IEnumerable<SimpleSource> Generate()
        {
            yield return new SimpleSource { Id = 10, Name = "G1" };
            yield return new SimpleSource { Id = 20, Name = "G2" };
            yield return new SimpleSource { Id = 30, Name = "G3" };
        }

        ImmutableArray<SimpleTarget> result =
            _mapper.MapToImmutableArray<SimpleSource, SimpleTarget>(Generate());

        Assert.Equal(3, result.Length);
        Assert.Equal(10, result[0].Id);
        Assert.Equal(20, result[1].Id);
        Assert.Equal(30, result[2].Id);
    }

    [Fact]
    public void MapToImmutableArray_AllPathsProduceSameResult()
    {
        // List, array, ICollection, IEnumerable paths must all produce identical output
        SimpleSource[] sourceArray =
        [
            new() { Id = 1, Name = "One" },
            new() { Id = 2, Name = "Two" },
            new() { Id = 3, Name = "Three" }
        ];

        ImmutableArray<SimpleTarget> fromArray = _mapper.MapToImmutableArray<SimpleSource, SimpleTarget>(sourceArray);
        ImmutableArray<SimpleTarget> fromList  = _mapper.MapToImmutableArray<SimpleSource, SimpleTarget>(sourceArray.ToList());
        ImmutableArray<SimpleTarget> fromQueue = _mapper.MapToImmutableArray<SimpleSource, SimpleTarget>(new Queue<SimpleSource>(sourceArray));
        ImmutableArray<SimpleTarget> fromEnum  = _mapper.MapToImmutableArray<SimpleSource, SimpleTarget>(sourceArray.Where(_ => true));

        Assert.Equal(3, fromArray.Length);
        Assert.Equal(3, fromList.Length);
        Assert.Equal(3, fromQueue.Length);
        Assert.Equal(3, fromEnum.Length);

        for (int i = 0; i < 3; i++)
        {
            Assert.Equal(fromArray[i].Id, fromList[i].Id);
            Assert.Equal(fromArray[i].Id, fromQueue[i].Id);
            Assert.Equal(fromArray[i].Id, fromEnum[i].Id);
        }
    }

    #endregion

    #region ImmutableList

    [Fact]
    public void MapToImmutableList_MapsAllItems()
    {
        var sources = new List<SimpleSource> { new() { Id = 10, Name = "A" }, new() { Id = 20, Name = "B" } };

        ImmutableList<SimpleTarget> result = _mapper.MapToImmutableList<SimpleSource, SimpleTarget>(sources);

        Assert.Equal(2, result.Count);
        Assert.Equal(10, result[0].Id);
        Assert.Equal(20, result[1].Id);
    }

    [Fact]
    public void MapToImmutableList_NullSource_Throws()
    {
        Assert.Throws<ArgumentNullException>(
            () => _mapper.MapToImmutableList<SimpleSource, SimpleTarget>(null!));
    }

    #endregion
}

