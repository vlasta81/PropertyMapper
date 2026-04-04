using System.Linq.Expressions;

namespace PropertyMapper.Tests;

public class ExpressionProjectionTests
{
    #region Test models

    private sealed class ProductSource
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public decimal TaxRate { get; set; }
        public string InternalCode { get; set; } = "";
    }

    private sealed class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TotalPrice { get; set; }
        public string InternalCode { get; set; } = "";
    }

    private sealed class NullableSource
    {
        public int? Score { get; set; }
        public string Label { get; set; } = "";
    }

    private sealed class ValueDto
    {
        public int Score { get; set; }
        public string Label { get; set; } = "";
    }

    private sealed class NestedSource
    {
        public int Id { get; set; }
        public AddressSource? Address { get; set; }
    }

    private sealed class AddressSource
    {
        public string Street { get; set; } = "";
        public string City { get; set; } = "";
    }

    private sealed class NestedDto
    {
        public int Id { get; set; }
        public AddressDto? Address { get; set; }
    }

    private sealed class AddressDto
    {
        public string Street { get; set; } = "";
        public string City { get; set; } = "";
    }

    #endregion

    #region GetProjectionExpression — basic

    [Fact]
    public void GetProjectionExpression_ReturnsNonNull()
    {
        var mapper = new PropMap();
        Expression<Func<ProductSource, ProductDto>> expr =
            mapper.GetProjectionExpression<ProductSource, ProductDto>();
        Assert.NotNull(expr);
    }

    [Fact]
    public void GetProjectionExpression_BodyIsMemberInitExpression()
    {
        var mapper = new PropMap();
        Expression<Func<ProductSource, ProductDto>> expr =
            mapper.GetProjectionExpression<ProductSource, ProductDto>();
        Assert.IsType<MemberInitExpression>(expr.Body);
    }

    [Fact]
    public void GetProjectionExpression_IsCached_ReturnsSameReference()
    {
        var mapper = new PropMap();
        var expr1 = mapper.GetProjectionExpression<ProductSource, ProductDto>();
        var expr2 = mapper.GetProjectionExpression<ProductSource, ProductDto>();
        Assert.Same(expr1, expr2);
    }

    [Fact]
    public void GetProjectionExpression_CompiledDelegate_ProducesCorrectResult()
    {
        var mapper = new PropMap();
        Func<ProductSource, ProductDto> compiled =
            mapper.GetProjectionExpression<ProductSource, ProductDto>().Compile();

        var source = new ProductSource { Id = 1, Name = "Widget", Price = 9.99m, TaxRate = 0.2m };
        ProductDto result = compiled(source);

        Assert.Equal(1, result.Id);
        Assert.Equal("Widget", result.Name);
        Assert.Equal(9.99m, result.Price);
        Assert.Equal(0.2m, result.TaxRate);
    }

    #endregion

    #region Project — IQueryable

    [Fact]
    public void Project_InMemoryQueryable_MapsAllItems()
    {
        var mapper = new PropMap();
        List<ProductSource> sources =
        [
            new() { Id = 1, Name = "A", Price = 10m },
            new() { Id = 2, Name = "B", Price = 20m },
        ];

        List<ProductDto> results = mapper.Project<ProductSource, ProductDto>(
            sources.AsQueryable()).ToList();

        Assert.Equal(2, results.Count);
        Assert.Equal(1, results[0].Id);
        Assert.Equal("A", results[0].Name);
        Assert.Equal(2, results[1].Id);
        Assert.Equal("B", results[1].Name);
    }

    [Fact]
    public void Project_EmptyQueryable_ReturnsEmptyList()
    {
        var mapper = new PropMap();
        List<ProductDto> results = mapper
            .Project<ProductSource, ProductDto>(Enumerable.Empty<ProductSource>().AsQueryable())
            .ToList();
        Assert.Empty(results);
    }

    [Fact]
    public void Project_NullSource_Throws()
    {
        var mapper = new PropMap();
        Assert.Throws<ArgumentNullException>(() =>
            mapper.Project<ProductSource, ProductDto>(null!));
    }

    #endregion

    #region NullableToValue conversion

    [Fact]
    public void GetProjectionExpression_NullableToValue_WithValue_ReturnsValue()
    {
        var mapper = new PropMap();
        Func<NullableSource, ValueDto> compiled =
            mapper.GetProjectionExpression<NullableSource, ValueDto>().Compile();

        ValueDto result = compiled(new NullableSource { Score = 42, Label = "x" });
        Assert.Equal(42, result.Score);
    }

    [Fact]
    public void GetProjectionExpression_NullableToValue_WithNull_ReturnsDefault()
    {
        var mapper = new PropMap();
        Func<NullableSource, ValueDto> compiled =
            mapper.GetProjectionExpression<NullableSource, ValueDto>().Compile();

        ValueDto result = compiled(new NullableSource { Score = null, Label = "x" });
        Assert.Equal(0, result.Score);
    }

    #endregion

    #region Nested type projection

    [Fact]
    public void GetProjectionExpression_Nested_MapsNestedProperties()
    {
        var mapper = new PropMap();
        Func<NestedSource, NestedDto> compiled =
            mapper.GetProjectionExpression<NestedSource, NestedDto>().Compile();

        var source = new NestedSource { Id = 5, Address = new AddressSource { Street = "Main St", City = "Springfield" } };
        NestedDto result = compiled(source);

        Assert.Equal(5, result.Id);
        Assert.NotNull(result.Address);
        Assert.Equal("Main St", result.Address.Street);
        Assert.Equal("Springfield", result.Address.City);
    }

    [Fact]
    public void GetProjectionExpression_Nested_NullNavProp_ReturnsNullNested()
    {
        var mapper = new PropMap();
        Func<NestedSource, NestedDto> compiled =
            mapper.GetProjectionExpression<NestedSource, NestedDto>().Compile();

        var source = new NestedSource { Id = 7, Address = null };
        NestedDto result = compiled(source);

        Assert.Equal(7, result.Id);
        Assert.Null(result.Address);
    }

    #endregion

    #region Config integration — Ignore

    [Fact]
    public void GetProjectionExpression_WithIgnore_PropertyExcludedFromExpression()
    {
        var mapper = new PropMap();
        mapper.Configure<ProductSource, ProductDto>()
              .Ignore(x => x.InternalCode);

        Func<ProductSource, ProductDto> compiled =
            mapper.GetProjectionExpression<ProductSource, ProductDto>().Compile();

        ProductDto result = compiled(new ProductSource { Name = "Widget", InternalCode = "SECRET" });

        Assert.Equal("Widget", result.Name);
        Assert.Equal("", result.InternalCode);
    }

    #endregion

    #region Config integration — MapFromExpression

    [Fact]
    public void MapFromExpression_InlinedIntoProjectionExpression()
    {
        var mapper = new PropMap();
        mapper.Configure<ProductSource, ProductDto>()
              .MapFromExpression(x => x.TotalPrice, src => src.Price * (1m + src.TaxRate));

        Func<ProductSource, ProductDto> compiled =
            mapper.GetProjectionExpression<ProductSource, ProductDto>().Compile();

        ProductDto result = compiled(new ProductSource { Price = 100m, TaxRate = 0.21m });
        Assert.Equal(121m, result.TotalPrice);
    }

    [Fact]
    public void MapFromExpression_ProjectionExpression_BodyStillMemberInit()
    {
        var mapper = new PropMap();
        mapper.Configure<ProductSource, ProductDto>()
              .MapFromExpression(x => x.TotalPrice, src => src.Price + src.TaxRate);

        Expression<Func<ProductSource, ProductDto>> expr =
            mapper.GetProjectionExpression<ProductSource, ProductDto>();

        Assert.IsType<MemberInitExpression>(expr.Body);
    }

    [Fact]
    public void MapFromExpression_AlsoAppliesWhenUsingRegularMap()
    {
        var mapper = new PropMap();
        mapper.Configure<ProductSource, ProductDto>()
              .MapFromExpression(x => x.TotalPrice, src => src.Price * (1m + src.TaxRate));

        var source = new ProductSource { Price = 200m, TaxRate = 0.10m };
        ProductDto result = mapper.Map<ProductSource, ProductDto>(source);

        Assert.Equal(220m, result.TotalPrice);
    }

    [Fact]
    public void MapFromExpression_ChainedWithIgnore_BothApplied()
    {
        var mapper = new PropMap();
        mapper.Configure<ProductSource, ProductDto>()
              .Ignore(x => x.InternalCode)
              .MapFromExpression(x => x.TotalPrice, src => src.Price + src.TaxRate);

        Func<ProductSource, ProductDto> compiled =
            mapper.GetProjectionExpression<ProductSource, ProductDto>().Compile();

        ProductDto result = compiled(new ProductSource { Price = 50m, TaxRate = 5m, InternalCode = "X" });

        Assert.Equal(55m, result.TotalPrice);
        Assert.Equal("", result.InternalCode);
    }

    [Fact]
    public void MapFromExpression_Project_OnQueryable()
    {
        var mapper = new PropMap();
        mapper.Configure<ProductSource, ProductDto>()
              .MapFromExpression(x => x.TotalPrice, src => src.Price * (1m + src.TaxRate));

        List<ProductSource> sources =
        [
            new() { Id = 1, Price = 100m, TaxRate = 0.10m },
            new() { Id = 2, Price = 200m, TaxRate = 0.20m },
        ];

        List<ProductDto> results = mapper
            .Project<ProductSource, ProductDto>(sources.AsQueryable())
            .ToList();

        Assert.Equal(110m, results[0].TotalPrice);
        Assert.Equal(240m, results[1].TotalPrice);
    }

    #endregion

    #region Cache invalidation

    [Fact]
    public void Configure_InvalidatesCachedProjectionExpression()
    {
        var mapper = new PropMap();
        Expression<Func<ProductSource, ProductDto>> expr1 =
            mapper.GetProjectionExpression<ProductSource, ProductDto>();

        mapper.Configure<ProductSource, ProductDto>()
              .MapFromExpression(x => x.TotalPrice, src => src.Price + src.TaxRate);

        Expression<Func<ProductSource, ProductDto>> expr2 =
            mapper.GetProjectionExpression<ProductSource, ProductDto>();

        // Reconfiguring must produce a new expression, not return the stale cached one.
        Assert.NotSame(expr1, expr2);

        Func<ProductSource, ProductDto> compiled = expr2.Compile();
        ProductDto result = compiled(new ProductSource { Price = 100m, TaxRate = 15m });
        Assert.Equal(115m, result.TotalPrice);
    }

    [Fact]
    public void Clear_AlsoInvalidatesProjectionCache()
    {
        var mapper = new PropMap();
        Expression<Func<ProductSource, ProductDto>> expr1 =
            mapper.GetProjectionExpression<ProductSource, ProductDto>();

        mapper.Clear();

        Expression<Func<ProductSource, ProductDto>> expr2 =
            mapper.GetProjectionExpression<ProductSource, ProductDto>();

        Assert.NotSame(expr1, expr2);
    }

    #endregion
}
