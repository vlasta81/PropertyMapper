# Configuration

← [Back to README](../README.md)

## PropMapConfiguration

`PropMapConfiguration` controls global mapper behaviour. Pass it to the `PropMap` constructor or apply it via `PropMapBuilder.WithConfiguration`.

```csharp
using PropertyMapper.Configuration;

PropMap mapper = new PropMap(new PropMapConfiguration
{
    MaxMappingDepth = 16,
    ThrowOnUnmappedProperties = true
});
```

| Property | Default | Description |
|----------|---------|-------------|
| `MaxMappingDepth` | `32` | Maximum nesting depth for recursive object mapping |
| `ThrowOnUnmappedProperties` | `false` | Throw when a target property has no matching source property |

### Fluent factory methods

```csharp
PropMapConfiguration config = new PropMapConfiguration()
    .WithMaxDepth(8)
    .WithGlobalIgnore("CreatedAt", "UpdatedAt", "RowVersion");
```

---

## PropMapBuilder

`PropMapBuilder` assembles a **frozen**, thread-safe `PropMap` singleton. All type-pair configurations are applied at build time; calling `Configure` on the built `PropMap` throws `InvalidOperationException`.

```csharp
PropMap mapper = new PropMapBuilder()
    .WithConfiguration(cfg => cfg.WithMaxDepth(16))
    .WithGlobalIgnore("CreatedAt", "UpdatedAt")
    .Configure<User, UserDto>(c =>
    {
        c.Ignore(x => x.PasswordHash);
        c.MapFrom(x => x.FullName, src => $"{src.FirstName} {src.LastName}");
        c.ReverseMap();
    })
    .Configure<Order, OrderDto>(c =>
        c.MapFrom(x => x.Total, src => src.Subtotal + src.Tax))
    .WarmupOnStartup<User, UserDto>()
    .WarmupOnStartup<Order, OrderDto>()
    .Build();
```

### Builder methods

| Method | Description |
|--------|-------------|
| `WithConfiguration(PropMapConfiguration)` | Set the global configuration |
| `WithConfiguration(Func<PropMapConfiguration, PropMapConfiguration>)` | Mutate the default configuration via factory |
| `WithGlobalIgnore(string[])` | Exclude property names globally across all type pairs (e.g. audit fields) |
| `Configure<TIn,TOut>(Action<TypePairConfiguration<TIn,TOut>>)` | Register per-property rules for a type pair; last registration wins for duplicates |
| `WarmupOnStartup<TIn,TOut>()` | Schedule the delegate for pre-compilation at application startup when registered via `AddPropertyMapper` |
| `Build()` | Produce the frozen `PropMap`; the builder's state is snapshotted |

---

## TypePairConfiguration\<TIn,TOut\>

Obtained via `PropMapBuilder.Configure<TIn,TOut>`. Provides fluent per-property rules for a single type pair.

### Ignore

Skip a target property — it retains its default value after mapping:

```csharp
.Configure<User, UserDto>(c =>
{
    c.Ignore(x => x.PasswordHash);
    c.Ignore(x => x.SecurityStamp);
})
```

### MapFrom

Override the value for a target property with a custom factory:

```csharp
.Configure<Order, OrderDto>(c =>
{
    // Computed from multiple source fields
    c.MapFrom(x => x.Total, src => src.Subtotal + src.Tax);

    // Rename
    c.MapFrom(x => x.CustomerName, src => src.Customer.FullName);
})
```

### MapFromExpression

Like `MapFrom` but expressed as an `Expression<Func<TIn,TProp>>`, which allows LINQ providers (e.g. EF Core) to translate it server-side when using `Project<TIn,TOut>`:

```csharp
.Configure<Product, ProductDto>(c =>
    c.MapFromExpression(x => x.DisplayName, src => src.Brand + " " + src.Name))
```

### MapFromWithContext

Maps a property using a per-call context value — safe for per-request state like tenant settings or exchange rates. The context is **not** compiled into the IL delegate:

```csharp
.Configure<Order, OrderDto>(c =>
    c.MapFromWithContext<ExchangeRates, decimal>(
        x => x.TotalEur,
        (src, rates) => src.TotalUsd * rates.UsdToEur))
```

Call at runtime with:

```csharp
ExchangeRates rates = GetCurrentRates();
OrderDto dto = mapper.MapWithContext<Order, OrderDto, ExchangeRates>(order, rates);
```

### AfterMap

Register a post-map hook invoked after all property copies and custom setters have completed:

```csharp
.Configure<Order, OrderDto>(c =>
    c.AfterMap((src, dst) =>
    {
        dst.Label = src.Total > 1000 ? "VIP" : "Standard";
        dst.MappedAt = DateTime.UtcNow;
    }))
```

### ReverseMap

Automatically register a `TOut → TIn` mapping alongside the `TIn → TOut` registration. Name-matched properties and `Ignore` exclusions are mirrored; `MapFrom` factories are not inverted:

```csharp
.Configure<User, UserDto>(c =>
{
    c.Ignore(x => x.PasswordHash);
    c.ReverseMap(); // registers UserDto → User (without PasswordHash)
})
```

---

## Dependency Injection

Use `AddPropertyMapper` (from `PropertyMapper.Extensions`) to register `IPropMap` and `PropMap` as singletons. When `WarmupOnStartup` pairs are configured, warmup is executed as a hosted background service before the first request is served.

```csharp
using PropertyMapper.Extensions;

builder.Services.AddPropertyMapper(b =>
{
    b.WithConfiguration(cfg => cfg.WithMaxDepth(16));
    b.WithGlobalIgnore("CreatedAt", "UpdatedAt");
    b.Configure<User, UserDto>(c =>
    {
        c.Ignore(x => x.PasswordHash);
        c.MapFrom(x => x.FullName, src => $"{src.FirstName} {src.LastName}");
        c.ReverseMap();
    });
    b.WarmupOnStartup<User, UserDto>();
});
```

---

## Related

- [Getting Started](getting-started.md)
- [API Reference — PropMapBuilder](api/PropMapBuilder.md)
- [API Reference — TypePairConfiguration](api/TypePairConfiguration_TIn,TOut_.md)
