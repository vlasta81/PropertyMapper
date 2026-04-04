namespace PropertyMapper.Tests;

public class CloningTests
{
    private readonly PropMap _mapper = new();

    [Fact]
    public void Clone_ReturnsNewInstance()
    {
        var source = new SimpleSource { Id = 1, Name = "Test" };
        SimpleSource clone = _mapper.Clone(source);
        Assert.NotSame(source, clone);
    }

    [Fact]
    public void Clone_CopiesAllProperties()
    {
        var source = new SimpleSource
        {
            Id = 42,
            Name = "Alice",
            IsActive = true,
            CreatedAt = new DateTime(2024, 3, 15)
        };

        SimpleSource clone = _mapper.Clone(source);

        Assert.Equal(42, clone.Id);
        Assert.Equal("Alice", clone.Name);
        Assert.True(clone.IsActive);
        Assert.Equal(new DateTime(2024, 3, 15), clone.CreatedAt);
    }

    [Fact]
    public void Clone_NullSource_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => _mapper.Clone<SimpleSource>(null!));
    }

    [Fact]
    public void Clone_ModifyingCloneDoesNotAffectOriginal()
    {
        var source = new SimpleSource { Id = 1, Name = "Original" };
        SimpleSource clone = _mapper.Clone(source);

        clone.Id = 999;
        clone.Name = "Modified";

        Assert.Equal(1, source.Id);
        Assert.Equal("Original", source.Name);
    }

    [Fact]
    public void Clone_RecordType_CopiesAllProperties()
    {
        var source = new PersonRecord { FirstName = "John", LastName = "Doe", Age = 30 };
        PersonRecord clone = _mapper.Clone(source);

        Assert.NotSame(source, clone);
        Assert.Equal("John", clone.FirstName);
        Assert.Equal("Doe", clone.LastName);
        Assert.Equal(30, clone.Age);
    }

    [Fact]
    public void Clone_CalledMultipleTimes_AlwaysReturnsDistinctInstances()
    {
        var source = new SimpleSource { Id = 7 };

        SimpleSource c1 = _mapper.Clone(source);
        SimpleSource c2 = _mapper.Clone(source);
        SimpleSource c3 = _mapper.Clone(source);

        Assert.NotSame(c1, c2);
        Assert.NotSame(c2, c3);
        Assert.Equal(7, c1.Id);
        Assert.Equal(7, c2.Id);
        Assert.Equal(7, c3.Id);
    }
}
