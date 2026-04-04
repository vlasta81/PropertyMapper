using PropertyMapper.Configuration;
using System.Collections.Frozen;

namespace PropertyMapper.Tests;

// ─── Test-local models ────────────────────────────────────────────────────────

file class PriceSource
{
    public string Name { get; set; } = string.Empty;
    public decimal PriceUsd { get; set; }
}

file class PriceTarget
{
    public string Name { get; set; } = string.Empty;
    public decimal PriceUsd { get; set; }
    public decimal PriceEur { get; set; }   // populated via MapFromWithContext
}

file class ExchangeContext
{
    public decimal UsdToEur { get; set; }
}

file class FullyMappedTarget
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

file class PartiallyMappedTarget
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ExtraProperty { get; set; } = string.Empty;   // no source match → unmapped
}

// ─────────────────────────────────────────────────────────────────────────────

public class NewFeaturesTests
{
    // ── 1. ReverseMap ──────────────────────────────────────────────────────────

    [Fact]
    public void ReverseMap_ForwardMapping_Works()
    {
        PropMap mapper = new PropMapBuilder()
            .Configure<SimpleSource, SimpleTarget>(c => c.ReverseMap())
            .Build();

        SimpleTarget result = mapper.Map<SimpleSource, SimpleTarget>(
            new SimpleSource { Id = 7, Name = "Forward" });

        Assert.Equal(7, result.Id);
        Assert.Equal("Forward", result.Name);
    }

    [Fact]
    public void ReverseMap_ReverseDirection_Works()
    {
        PropMap mapper = new PropMapBuilder()
            .Configure<SimpleSource, SimpleTarget>(c => c.ReverseMap())
            .Build();

        SimpleSource back = mapper.Map<SimpleTarget, SimpleSource>(
            new SimpleTarget { Id = 42, Name = "Reverse" });

        Assert.Equal(42, back.Id);
        Assert.Equal("Reverse", back.Name);
    }

    [Fact]
    public void ReverseMap_IgnoredPropertiesMirroredToReverse()
    {
        PropMap mapper = new PropMapBuilder()
            .Configure<SimpleSource, SimpleTarget>(c => c
                .Ignore(x => x.CreatedAt)
                .ReverseMap())
            .Build();

        var sourceDate = new DateTime(2024, 1, 1);
        SimpleSource back = mapper.Map<SimpleTarget, SimpleSource>(
            new SimpleTarget { Id = 1, Name = "X", CreatedAt = sourceDate });

        // CreatedAt is ignored in both directions — stays at default.
        Assert.Equal(default, back.CreatedAt);
    }

    [Fact]
    public void ReverseMap_ExplicitReverseConfigTakesPrecedence()
    {
        // If the user explicitly registers the reverse direction, that config wins.
        PropMap mapper = new PropMapBuilder()
            .Configure<SimpleSource, SimpleTarget>(c => c.ReverseMap())
            .Configure<SimpleTarget, SimpleSource>(c => c.Ignore(x => x.Name))
            .Build();

        SimpleSource back = mapper.Map<SimpleTarget, SimpleSource>(
            new SimpleTarget { Id = 5, Name = "ShouldBeIgnored" });

        Assert.Equal(5, back.Id);
        Assert.Equal(string.Empty, back.Name);   // ignored by explicit config
    }

    // ── 2. AfterMap ────────────────────────────────────────────────────────────

    [Fact]
    public void AfterMap_InvokedAfterPropertyCopy()
    {
        bool afterMapCalled = false;
        PropMap mapper = new PropMapBuilder()
            .Configure<SimpleSource, SimpleTarget>(c => c
                .AfterMap((src, tgt) =>
                {
                    afterMapCalled = true;
                    tgt.Name = tgt.Name.ToUpperInvariant();
                }))
            .Build();

        SimpleTarget result = mapper.Map<SimpleSource, SimpleTarget>(
            new SimpleSource { Id = 1, Name = "hello" });

        Assert.True(afterMapCalled);
        Assert.Equal("HELLO", result.Name);
    }

    [Fact]
    public void AfterMap_MultipleHooks_InvokedInRegistrationOrder()
    {
        List<int> order = [];
        PropMap mapper = new PropMapBuilder()
            .Configure<SimpleSource, SimpleTarget>(c => c
                .AfterMap((_, _) => order.Add(1))
                .AfterMap((_, _) => order.Add(2))
                .AfterMap((_, _) => order.Add(3)))
            .Build();

        mapper.Map<SimpleSource, SimpleTarget>(new SimpleSource { Id = 1 });

        Assert.Equal([1, 2, 3], order);
    }

    [Fact]
    public void AfterMap_NullAction_Throws()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new TypePairConfiguration<SimpleSource, SimpleTarget>().AfterMap(null!));
    }

    // ── 3. WithGlobalIgnore on builder ────────────────────────────────────────

    [Fact]
    public void WithGlobalIgnore_PropertyNotMappedAcrossAllPairs()
    {
        PropMap mapper = new PropMapBuilder()
            .WithGlobalIgnore(nameof(SimpleSource.CreatedAt))
            .Build();

        var source = new SimpleSource { Id = 1, Name = "X", CreatedAt = new DateTime(2024, 6, 1) };
        SimpleTarget result = mapper.Map<SimpleSource, SimpleTarget>(source);

        Assert.Equal(1, result.Id);
        Assert.Equal(default, result.CreatedAt);   // globally ignored
    }

    [Fact]
    public void WithGlobalIgnore_ViaConfiguration_Works()
    {
        PropMap mapper = new PropMapBuilder()
            .WithConfiguration(cfg => cfg.WithGlobalIgnore(nameof(SimpleSource.IsActive)))
            .Build();

        var source = new SimpleSource { Id = 2, IsActive = true };
        SimpleTarget result = mapper.Map<SimpleSource, SimpleTarget>(source);

        Assert.False(result.IsActive);   // not copied — globally ignored
    }

    [Fact]
    public void WithGlobalIgnore_NullArgument_Throws()
    {
        Assert.Throws<ArgumentNullException>(() =>
            new PropMapBuilder().WithGlobalIgnore(null!));
    }

    // ── 4. MapWithContext ──────────────────────────────────────────────────────

    [Fact]
    public void MapWithContext_ContextSetterAppliedAfterBaseMapping()
    {
        PropMap mapper = new PropMapBuilder()
            .Configure<PriceSource, PriceTarget>(c => c
                .MapFromWithContext<ExchangeContext, decimal>(
                    x => x.PriceEur,
                    (src, ctx) => src.PriceUsd * ctx.UsdToEur))
            .Build();

        var ctx = new ExchangeContext { UsdToEur = 0.92m };
        PriceTarget result = mapper.MapWithContext<PriceSource, PriceTarget, ExchangeContext>(
            new PriceSource { Name = "Widget", PriceUsd = 100m }, ctx);

        Assert.Equal("Widget", result.Name);      // base IL mapping
        Assert.Equal(92m, result.PriceEur);        // context setter
    }

    [Fact]
    public void MapWithContext_DifferentContextValuesProduceDifferentResults()
    {
        PropMap mapper = new PropMapBuilder()
            .Configure<PriceSource, PriceTarget>(c => c
                .MapFromWithContext<ExchangeContext, decimal>(
                    x => x.PriceEur,
                    (src, ctx) => src.PriceUsd * ctx.UsdToEur))
            .Build();

        var source = new PriceSource { PriceUsd = 100m };

        PriceTarget r1 = mapper.MapWithContext<PriceSource, PriceTarget, ExchangeContext>(
            source, new ExchangeContext { UsdToEur = 0.90m });
        PriceTarget r2 = mapper.MapWithContext<PriceSource, PriceTarget, ExchangeContext>(
            source, new ExchangeContext { UsdToEur = 0.85m });

        Assert.Equal(90m, r1.PriceEur);
        Assert.Equal(85m, r2.PriceEur);
    }

    [Fact]
    public void MapWithContext_NullSource_Throws()
    {
        PropMap mapper = new PropMapBuilder()
            .Configure<PriceSource, PriceTarget>(_ => { })
            .Build();

        Assert.Throws<ArgumentNullException>(() =>
            mapper.MapWithContext<PriceSource, PriceTarget, ExchangeContext>(
                null!, new ExchangeContext()));
    }

    [Fact]
    public void MapWithContext_NoContextSettersRegistered_BaseMappingStillWorks()
    {
        PropMap mapper = new PropMap();

        // No MapFromWithContext registered — behaves like a normal Map call.
        PriceTarget result = mapper.MapWithContext<PriceSource, PriceTarget, ExchangeContext>(
            new PriceSource { Name = "Test", PriceUsd = 50m },
            new ExchangeContext { UsdToEur = 1m });

        Assert.Equal("Test", result.Name);
        Assert.Equal(50m, result.PriceUsd);
        Assert.Equal(0m, result.PriceEur);   // no context setter, stays default
    }

    // ── 5. MapToFrozenDictionary ───────────────────────────────────────────────

    [Fact]
    public void MapToFrozenDictionary_ContainsMappedValues()
    {
        PropMap mapper = new PropMap();

        var source = new Dictionary<int, SimpleSource>
        {
            [1] = new SimpleSource { Id = 1, Name = "Alpha" },
            [2] = new SimpleSource { Id = 2, Name = "Beta" },
        };

        FrozenDictionary<int, SimpleTarget> result =
            mapper.MapToFrozenDictionary<int, SimpleSource, int, SimpleTarget>(source);

        Assert.Equal(2, result.Count);
        Assert.Equal("Alpha", result[1].Name);
        Assert.Equal("Beta", result[2].Name);
    }

    [Fact]
    public void MapToFrozenDictionary_NullSource_Throws()
    {
        PropMap mapper = new PropMap();
        Assert.Throws<ArgumentNullException>(() =>
            mapper.MapToFrozenDictionary<int, SimpleSource, int, SimpleTarget>(null!));
    }

    [Fact]
    public void MapToFrozenDictionary_EmptySource_ReturnsEmptyFrozenDictionary()
    {
        PropMap mapper = new PropMap();
        FrozenDictionary<int, SimpleTarget> result =
            mapper.MapToFrozenDictionary<int, SimpleSource, int, SimpleTarget>(new Dictionary<int, SimpleSource>());
        Assert.Empty(result);
    }

    // ── 6. Validate ────────────────────────────────────────────────────────────

    [Fact]
    public void Validate_FullyMatchedTypes_IsValid()
    {
        PropMap mapper = new PropMap();
        MappingValidationResult result = mapper.Validate<SimpleSource, FullyMappedTarget>();

        Assert.True(result.IsValid);
        Assert.Empty(result.UnmappedTargetProperties);
    }

    [Fact]
    public void Validate_TargetHasExtraProperty_ReportsUnmapped()
    {
        PropMap mapper = new PropMap();
        MappingValidationResult result = mapper.Validate<SimpleSource, PartiallyMappedTarget>();

        Assert.False(result.IsValid);
        Assert.Contains("ExtraProperty", result.UnmappedTargetProperties);
    }

    [Fact]
    public void Validate_IgnoredProperty_ReportedAsUnmapped()
    {
        PropMap mapper = new PropMapBuilder()
            .Configure<SimpleSource, SimpleTarget>(c => c.Ignore(x => x.Name))
            .Build();

        MappingValidationResult result = mapper.Validate<SimpleSource, SimpleTarget>();

        Assert.False(result.IsValid);
        Assert.Contains("Name", result.UnmappedTargetProperties);
    }

    [Fact]
    public void Validate_DoesNotSideEffect_MapStillWorksAfter()
    {
        PropMap mapper = new PropMap();
        mapper.Validate<SimpleSource, SimpleTarget>();   // triggers plan build without IL emit

        // Map should compile and execute correctly afterwards.
        SimpleTarget result = mapper.Map<SimpleSource, SimpleTarget>(
            new SimpleSource { Id = 99, Name = "Post-Validate" });

        Assert.Equal(99, result.Id);
        Assert.Equal("Post-Validate", result.Name);
    }

    // ── 7. MapCollection<TIn,TOut>(IEnumerable, ICollection) ──────────────────

    [Fact]
    public void MapCollection_AppendsToExistingList()
    {
        PropMap mapper = new PropMap();
        var sources = new[] { new SimpleSource { Id = 1 }, new SimpleSource { Id = 2 } };
        var destination = new List<SimpleTarget> { new SimpleTarget { Id = 0 } };   // pre-existing item

        mapper.MapCollection<SimpleSource, SimpleTarget>(sources, destination);

        Assert.Equal(3, destination.Count);
        Assert.Equal(0, destination[0].Id);   // original item preserved
        Assert.Equal(1, destination[1].Id);
        Assert.Equal(2, destination[2].Id);
    }

    [Fact]
    public void MapCollection_EmptySource_DestinationUnchanged()
    {
        PropMap mapper = new PropMap();
        var destination = new List<SimpleTarget> { new SimpleTarget { Id = 7 } };

        mapper.MapCollection<SimpleSource, SimpleTarget>([], destination);

        Assert.Single(destination);
        Assert.Equal(7, destination[0].Id);
    }

    [Fact]
    public void MapCollection_NullSource_Throws()
    {
        PropMap mapper = new PropMap();
        Assert.Throws<ArgumentNullException>(() =>
            mapper.MapCollection<SimpleSource, SimpleTarget>(null!, new List<SimpleTarget>()));
    }

    [Fact]
    public void MapCollection_NullDestination_Throws()
    {
        PropMap mapper = new PropMap();
        Assert.Throws<ArgumentNullException>(() =>
            mapper.MapCollection<SimpleSource, SimpleTarget>([], null!));
    }

    [Fact]
    public void MapCollection_WorksWithHashSetDestination()
    {
        PropMap mapper = new PropMap();
        var sources = new[] { new SimpleSource { Id = 3 }, new SimpleSource { Id = 4 } };
        var destination = new HashSet<SimpleTarget>();

        mapper.MapCollection<SimpleSource, SimpleTarget>(sources, destination);

        Assert.Equal(2, destination.Count);
    }
}
