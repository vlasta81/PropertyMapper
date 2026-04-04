namespace PropertyMapper.Tests;

public class PerPropertyConfigurationTests
{
    #region Test models

    private sealed class PersonSource
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public decimal Price { get; set; }
        public decimal Tax { get; set; }
        public string PasswordHash { get; set; } = "";
    }

    private sealed class PersonTarget
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public decimal TotalAmount { get; set; }
        public string PasswordHash { get; set; } = "";
    }

    #endregion

    #region Ignore

    [Fact]
    public void Ignore_SkipsProperty_TargetRetainsDefault()
    {
        var mapper = new PropMap();
        mapper.Configure<PersonSource, PersonTarget>()
              .Ignore(x => x.PasswordHash);

        var source = new PersonSource { PasswordHash = "secret", FirstName = "Alice" };
        PersonTarget result = mapper.Map<PersonSource, PersonTarget>(source);

        Assert.Equal("", result.PasswordHash);
        Assert.Equal("Alice", result.FirstName);
    }

    [Fact]
    public void Ignore_MultipleProperties_AllSkipped()
    {
        var mapper = new PropMap();
        mapper.Configure<PersonSource, PersonTarget>()
              .Ignore(x => x.PasswordHash)
              .Ignore(x => x.Email);

        var source = new PersonSource { PasswordHash = "secret", Email = "a@b.com", FirstName = "Bob" };
        PersonTarget result = mapper.Map<PersonSource, PersonTarget>(source);

        Assert.Equal("", result.PasswordHash);
        Assert.Equal("", result.Email);
        Assert.Equal("Bob", result.FirstName);
    }

    [Fact]
    public void Ignore_DoesNotAffectUnrelatedProperties()
    {
        var mapper = new PropMap();
        mapper.Configure<PersonSource, PersonTarget>()
              .Ignore(x => x.PasswordHash);

        var source = new PersonSource
        {
            FirstName = "Alice",
            LastName = "Smith",
            Email = "alice@example.com",
            Price = 10m,
            Tax = 2m,
            PasswordHash = "secret"
        };
        PersonTarget result = mapper.Map<PersonSource, PersonTarget>(source);

        Assert.Equal("Alice", result.FirstName);
        Assert.Equal("Smith", result.LastName);
        Assert.Equal("alice@example.com", result.Email);
        Assert.Equal("", result.PasswordHash);
    }

    #endregion

    #region MapFrom

    [Fact]
    public void MapFrom_CustomExpression_CombinesFields()
    {
        var mapper = new PropMap();
        mapper.Configure<PersonSource, PersonTarget>()
              .MapFrom(x => x.FullName, src => $"{src.FirstName} {src.LastName}");

        var source = new PersonSource { FirstName = "Alice", LastName = "Smith" };
        PersonTarget result = mapper.Map<PersonSource, PersonTarget>(source);

        Assert.Equal("Alice Smith", result.FullName);
    }

    [Fact]
    public void MapFrom_ComputedValue_SumsFields()
    {
        var mapper = new PropMap();
        mapper.Configure<PersonSource, PersonTarget>()
              .MapFrom(x => x.TotalAmount, src => src.Price + src.Tax);

        var source = new PersonSource { Price = 100m, Tax = 21m };
        PersonTarget result = mapper.Map<PersonSource, PersonTarget>(source);

        Assert.Equal(121m, result.TotalAmount);
    }

    [Fact]
    public void MapFrom_OverridesDefaultMapping_EmailTransformed()
    {
        var mapper = new PropMap();
        mapper.Configure<PersonSource, PersonTarget>()
              .MapFrom(x => x.Email, src => src.Email.ToUpperInvariant());

        var source = new PersonSource { Email = "alice@example.com" };
        PersonTarget result = mapper.Map<PersonSource, PersonTarget>(source);

        Assert.Equal("ALICE@EXAMPLE.COM", result.Email);
    }

    [Fact]
    public void MapFrom_PropertyNotOnSource_SetToConstant()
    {
        var mapper = new PropMap();
        mapper.Configure<PersonSource, PersonTarget>()
              .MapFrom(x => x.FullName, _ => "REDACTED");

        var source = new PersonSource { FirstName = "Alice", LastName = "Smith" };
        PersonTarget result = mapper.Map<PersonSource, PersonTarget>(source);

        Assert.Equal("REDACTED", result.FullName);
    }

    #endregion

    #region Chaining

    [Fact]
    public void Configure_ChainedCalls_AllRulesApplied()
    {
        var mapper = new PropMap();
        mapper.Configure<PersonSource, PersonTarget>()
              .Ignore(x => x.PasswordHash)
              .MapFrom(x => x.FullName, src => $"{src.FirstName} {src.LastName}")
              .MapFrom(x => x.TotalAmount, src => src.Price + src.Tax);

        var source = new PersonSource
        {
            FirstName = "Alice",
            LastName = "Smith",
            Price = 50m,
            Tax = 10m,
            PasswordHash = "secret"
        };
        PersonTarget result = mapper.Map<PersonSource, PersonTarget>(source);

        Assert.Equal("Alice Smith", result.FullName);
        Assert.Equal(60m, result.TotalAmount);
        Assert.Equal("", result.PasswordHash);
    }

    [Fact]
    public void Configure_ActionOverload_AppliesRules()
    {
        var mapper = new PropMap();
        mapper.Configure<PersonSource, PersonTarget>(cfg =>
        {
            cfg.Ignore(x => x.PasswordHash);
            cfg.MapFrom(x => x.FullName, src => $"{src.FirstName} {src.LastName}");
        });

        var source = new PersonSource { FirstName = "Bob", LastName = "Jones", PasswordHash = "pwd" };
        PersonTarget result = mapper.Map<PersonSource, PersonTarget>(source);

        Assert.Equal("Bob Jones", result.FullName);
        Assert.Equal("", result.PasswordHash);
    }

    #endregion

    #region Cache invalidation

    [Fact]
    public void Configure_AfterFirstMap_InvalidatesCachedDelegate()
    {
        var mapper = new PropMap();

        // First Map — no config, PasswordHash is copied.
        var source = new PersonSource { PasswordHash = "secret", FirstName = "Alice" };
        PersonTarget first = mapper.Map<PersonSource, PersonTarget>(source);
        Assert.Equal("secret", first.PasswordHash);

        // Reconfigure — ignore PasswordHash.
        mapper.Configure<PersonSource, PersonTarget>()
              .Ignore(x => x.PasswordHash);

        // Second Map — new config should be respected.
        PersonTarget second = mapper.Map<PersonSource, PersonTarget>(source);
        Assert.Equal("", second.PasswordHash);
        Assert.Equal("Alice", second.FirstName);
    }

    [Fact]
    public void Configure_CalledTwice_LastConfigWins()
    {
        var mapper = new PropMap();

        mapper.Configure<PersonSource, PersonTarget>()
              .MapFrom(x => x.FullName, _ => "FIRST");

        mapper.Configure<PersonSource, PersonTarget>()
              .MapFrom(x => x.FullName, _ => "SECOND");

        var source = new PersonSource { FirstName = "Alice", LastName = "Smith" };
        PersonTarget result = mapper.Map<PersonSource, PersonTarget>(source);

        Assert.Equal("SECOND", result.FullName);
    }

    #endregion

    #region MapInto integration

    [Fact]
    public void MapInto_RespectsIgnore()
    {
        var mapper = new PropMap();
        mapper.Configure<PersonSource, PersonTarget>()
              .Ignore(x => x.PasswordHash);

        var source = new PersonSource { FirstName = "Alice", PasswordHash = "secret" };
        var target = new PersonTarget { PasswordHash = "existing" };
        mapper.MapInto(source, target);

        // Ignore means the in-place mapper skips the property entirely → target retains its original value.
        Assert.Equal("existing", target.PasswordHash);
        Assert.Equal("Alice", target.FirstName);
    }

    [Fact]
    public void MapInto_RespectsMapFrom()
    {
        var mapper = new PropMap();
        mapper.Configure<PersonSource, PersonTarget>()
              .MapFrom(x => x.FullName, src => $"{src.FirstName} {src.LastName}");

        var source = new PersonSource { FirstName = "Alice", LastName = "Smith" };
        var target = new PersonTarget();
        mapper.MapInto(source, target);

        Assert.Equal("Alice Smith", target.FullName);
    }

    #endregion
}
