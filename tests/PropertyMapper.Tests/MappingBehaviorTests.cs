namespace PropertyMapper.Tests;

/// <summary>
/// Behavior tests verifying correct property copying, unmapped-property handling,
/// and cache statistics — the scenarios formerly covered by the removed MappingDiagnostics class.
/// </summary>
public class MappingBehaviorTests
{
    private readonly PropMap _mapper = new();

    #region Property copying

    [Fact]
    public void Map_SimpleSourceToTarget_CopiesAllProperties()
    {
        var source = new SimpleSource { Id = 42, Name = "Test", IsActive = true, CreatedAt = new DateTime(2024, 1, 1) };

        var result = _mapper.Map<SimpleSource, SimpleTarget>(source);

        Assert.Equal(source.Id, result.Id);
        Assert.Equal(source.Name, result.Name);
        Assert.Equal(source.IsActive, result.IsActive);
        Assert.Equal(source.CreatedAt, result.CreatedAt);
    }

    [Fact]
    public void Map_NestedType_CopiesNestedPropertiesDeep()
    {
        var source = new PersonWithAddress
        {
            Name = "Alice",
            Age = 30,
            Address = new Address { Street = "Main St", City = "Prague", PostalCode = "11000" }
        };

        var result = _mapper.Map<PersonWithAddress, PersonWithAddressDto>(source);

        Assert.Equal(source.Name, result.Name);
        Assert.Equal(source.Age, result.Age);
        Assert.NotNull(result.Address);
        Assert.Equal(source.Address.Street, result.Address.Street);
        Assert.Equal(source.Address.City, result.Address.City);
        Assert.Equal(source.Address.PostalCode, result.Address.PostalCode);
    }

    #endregion

    #region Unmapped properties

    [Fact]
    public void Map_SourceHasExtraProperty_MappedPropertiesAreCorrectAndExtraIsIgnored()
    {
        // SourceWithExtra.ExtraProperty has no match on TargetWithLess — silently ignored
        var source = new SourceWithExtra { Id = 5, Name = "Bob", ExtraProperty = "ignored" };

        var result = _mapper.Map<SourceWithExtra, TargetWithLess>(source);

        Assert.Equal(source.Id, result.Id);
        Assert.Equal(source.Name, result.Name);
    }

    [Fact]
    public void Map_TargetHasExtraProperty_OnlyMatchingPropertyIsCopied()
    {
        // SourceWithLess has only Id; TargetWithExtra also has Name which stays at its default
        var source = new SourceWithLess { Id = 7 };

        var result = _mapper.Map<SourceWithLess, TargetWithExtra>(source);

        Assert.Equal(source.Id, result.Id);
        Assert.Equal(string.Empty, result.Name);
    }

    #endregion

    #region Cache statistics

    [Fact]
    public void GetStatistics_AfterMapping_ReportsNonZeroCachedMappers()
    {
        _mapper.Map<SimpleSource, SimpleTarget>(new SimpleSource { Id = 1 });

        var stats = _mapper.GetStatistics();

        Assert.True(stats.CachedMappers > 0);
    }

    [Fact]
    public void GetStatistics_AfterMapping_ReportsNonZeroCachedPlans()
    {
        _mapper.Map<PersonWithAddress, PersonWithAddressDto>(
            new PersonWithAddress { Name = "Eve", Age = 25, Address = new Address { City = "Brno" } });

        var stats = _mapper.GetStatistics();

        Assert.True(stats.CachedPlans > 0);
    }

    #endregion
}
