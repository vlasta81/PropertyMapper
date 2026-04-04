using PropertyMapper.Masking;

namespace PropertyMapper.Tests;

/// <summary>
/// Tests for <see cref="FieldMask{T}"/> and the
/// <see cref="PropMap.MapThenApplyMask{TIn,TOut}(TIn, IFieldMask{TOut})"/> integration.
/// </summary>
public class FieldMaskTests
{
    private readonly PropMap _mapper = new();

    // -----------------------------------------------------------------------
    // FieldMask construction
    // -----------------------------------------------------------------------

    /// <summary>
    /// Verifies the defensive copy introduced in fix #10:
    /// mutating the original <c>string[]</c> after construction must not affect
    /// <see cref="FieldMask{T}.ExcludedFields"/>.
    /// </summary>
    [Fact]
    public void FieldMask_DefensiveCopy_ExternalMutationDoesNotAffectExcludedFields()
    {
        string[] names = ["Name", "IsActive"];
        var mask = new FieldMask<SimpleTarget>(names);

        // Mutate the original array after the mask is constructed.
        names[0] = "Id";
        names[1] = "CreatedAt";

        Assert.Contains("Name", mask.ExcludedFields);
        Assert.Contains("IsActive", mask.ExcludedFields);
        Assert.DoesNotContain("Id", mask.ExcludedFields);
        Assert.DoesNotContain("CreatedAt", mask.ExcludedFields);
    }

    /// <summary>
    /// Verifies that <see cref="FieldMask{T}.ExcludedFields"/> reflects the exact names
    /// passed to the constructor when no mutation has occurred.
    /// </summary>
    [Fact]
    public void FieldMask_ExcludedFields_ReflectsConstructorArguments()
    {
        var mask = new FieldMask<SimpleTarget>("Name", "IsActive");

        Assert.Equal(2, mask.ExcludedFields.Count);
        Assert.Contains("Name", mask.ExcludedFields);
        Assert.Contains("IsActive", mask.ExcludedFields);
    }

    /// <summary>
    /// Verifies that a <see langword="null"/> argument throws <see cref="ArgumentNullException"/>.
    /// </summary>
    [Fact]
    public void FieldMask_NullArgument_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new FieldMask<SimpleTarget>(null!));
    }

    // -----------------------------------------------------------------------
    // FieldMask.Apply
    // -----------------------------------------------------------------------

    /// <summary>
    /// Verifies that <see cref="FieldMask{T}.Apply"/> zeroes out exactly the excluded
    /// properties while leaving all other properties unchanged.
    /// </summary>
    [Fact]
    public void FieldMask_Apply_ZerosExcludedFields()
    {
        var mask = new FieldMask<SimpleTarget>("Name", "IsActive");
        var target = new SimpleTarget
        {
            Id = 42,
            Name = "Alice",
            IsActive = true,
            CreatedAt = DateTime.Today
        };

        mask.Apply(target);

        // Excluded properties are reset to their default values.
        Assert.Null(target.Name);        // default(string) == null
        Assert.False(target.IsActive);

        // Non-excluded properties are untouched.
        Assert.Equal(42, target.Id);
        Assert.Equal(DateTime.Today, target.CreatedAt);
    }

    /// <summary>
    /// Verifies that a property name that does not exist on <c>T</c>
    /// is silently ignored — no exception and no unintended side-effects.
    /// </summary>
    [Fact]
    public void FieldMask_NonExistentProperty_IsIgnoredSilently()
    {
        var mask = new FieldMask<SimpleTarget>("NonExistent", "AlsoMissing");
        var target = new SimpleTarget { Id = 1, Name = "Test", IsActive = true };

        // Should not throw; all properties remain at their original values.
        mask.Apply(target);

        Assert.Equal(1, target.Id);
        Assert.Equal("Test", target.Name);
        Assert.True(target.IsActive);
    }

    /// <summary>
    /// Verifies that passing an empty name array produces a mask that leaves every
    /// property unchanged — the compiled clearer list is empty.
    /// </summary>
    [Fact]
    public void FieldMask_EmptyNames_NoFieldsCleared()
    {
        var mask = new FieldMask<SimpleTarget>();
        var target = new SimpleTarget { Id = 7, Name = "Bob", IsActive = true };

        mask.Apply(target);

        Assert.Equal(7, target.Id);
        Assert.Equal("Bob", target.Name);
        Assert.True(target.IsActive);
    }

    /// <summary>
    /// Verifies that <see cref="FieldMask{T}.Apply"/> throws
    /// <see cref="ArgumentNullException"/> when called with a <see langword="null"/> instance.
    /// </summary>
    [Fact]
    public void FieldMask_Apply_NullInstance_Throws()
    {
        var mask = new FieldMask<SimpleTarget>("Name");
        Assert.Throws<ArgumentNullException>(() => mask.Apply(null!));
    }

    // -----------------------------------------------------------------------
    // PropMap.MapThenApplyMask integration
    // -----------------------------------------------------------------------

    /// <summary>
    /// Verifies that <see cref="PropMap.MapThenApplyMask{TIn,TOut}"/> first maps all matching
    /// properties and then applies the mask, zeroing the excluded fields.
    /// </summary>
    [Fact]
    public void MapThenApplyMask_ZerosExcludedFieldsAfterMapping()
    {
        var source = new SimpleSource
        {
            Id = 5,
            Name = "Bob",
            IsActive = true,
            CreatedAt = DateTime.Today
        };
        var mask = new FieldMask<SimpleTarget>("Name", "IsActive");

        var result = _mapper.MapThenApplyMask<SimpleSource, SimpleTarget>(source, mask);

        // Excluded fields are zeroed.
        Assert.Null(result.Name);        // default(string) == null
        Assert.False(result.IsActive);

        // Non-excluded fields carry the source values.
        Assert.Equal(5, result.Id);
        Assert.Equal(DateTime.Today, result.CreatedAt);
    }

    /// <summary>
    /// Verifies that using a mask with no excluded names produces a result identical
    /// to a plain <see cref="PropMap.Map{TIn,TOut}"/> call.
    /// </summary>
    [Fact]
    public void MapThenApplyMask_EmptyMask_EquivalentToPlainMap()
    {
        var source = new SimpleSource { Id = 9, Name = "Eve", IsActive = false, CreatedAt = DateTime.Today };
        var mask = new FieldMask<SimpleTarget>();

        var masked = _mapper.MapThenApplyMask<SimpleSource, SimpleTarget>(source, mask);
        var plain = _mapper.Map<SimpleSource, SimpleTarget>(source);

        Assert.Equal(plain.Id, masked.Id);
        Assert.Equal(plain.Name, masked.Name);
        Assert.Equal(plain.IsActive, masked.IsActive);
        Assert.Equal(plain.CreatedAt, masked.CreatedAt);
    }

    /// <summary>
    /// Verifies that <see cref="PropMap.MapThenApplyMask{TIn,TOut}"/> is idempotent when the
    /// same mask instance is reused across multiple calls.
    /// </summary>
    [Fact]
    public void MapThenApplyMask_ReusedMask_IsIdempotent()
    {
        var mask = new FieldMask<SimpleTarget>("Name");
        var source1 = new SimpleSource { Id = 1, Name = "First" };
        var source2 = new SimpleSource { Id = 2, Name = "Second" };

        var r1 = _mapper.MapThenApplyMask<SimpleSource, SimpleTarget>(source1, mask);
        var r2 = _mapper.MapThenApplyMask<SimpleSource, SimpleTarget>(source2, mask);

        Assert.Null(r1.Name);           // default(string) == null
        Assert.Null(r2.Name);
        Assert.Equal(1, r1.Id);
        Assert.Equal(2, r2.Id);
    }
}
