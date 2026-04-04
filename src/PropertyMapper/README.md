# PropertyMapper

High-performance, IL-based property mapper for .NET 10+.

> Full documentation, API reference and benchmarks: **[github.com/vlasta81/PropertyMapper](https://github.com/vlasta81/PropertyMapper)**

## Installation

```bash
dotnet add package PropertyMapper
```

## Quick Start

```csharp
using PropertyMapper;

var mapper = new PropMap();

// Single object
OrderDto dto = mapper.Map<Order, OrderDto>(order);

// Collection → List
List<OrderDto> dtos = mapper.MapToList<Order, OrderDto>(orders);

// Span batch
OrderDto[] batch = mapper.MapBatch<Order, OrderDto>(orders.AsSpan());

// Async
OrderDto dto = await mapper.MapAsync<Order, OrderDto>(order, cancellationToken);
```

## Key Features

| Feature | Details |
|---------|---------|
| **IL code generation** | Delegates compiled once via `Reflection.Emit` — subsequent calls as fast as hand-written code |
| **Zero-allocation hot path** | Lock-free `FrozenDictionary` cache after first compilation |
| **Structs, records, classes** | Mutable classes/structs and immutable records with primary constructors |
| **Nested objects** | Recursive mapping with configurable depth limit |
| **Nullable support** | Wrapping, unwrapping, and nullable-to-nullable conversions |
| **Operator conversions** | Respects `implicit`/`explicit` operators |
| **Rich collections** | `List<T>`, `T[]`, `ImmutableArray<T>`, `ImmutableList<T>`, `HashSet<T>`, `FrozenDictionary<K,V>`, nested lists |
| **Async & parallel** | `MapAsync`, `MapParallelAsync`, `MapStreamAsync`, `MapStreamBatchedAsync` |
| **Fluent configuration** | `PropMapBuilder` with `Ignore`, `MapFrom`, `AfterMap`, `ReverseMap` per type-pair |
| **FieldMask / Projection** | `MapThenApplyMask`, `Project<TIn,TOut>` for EF Core / `IQueryable` |
| **Dependency Injection** | `AddPropertyMapper()` extension for `IServiceCollection` |
| **Diagnostics** | `Validate<TIn,TOut>()`, `GetStatistics()`, `Warmup`, `WarmupBatch` |

## Configuration

```csharp
using PropertyMapper.Configuration;

// Simple — PropMapConfiguration
PropMap mapper = new PropMap(new PropMapConfiguration
{
    MaxMappingDepth = 8,
    ThrowOnUnmappedProperties = true
});

// Advanced — PropMapBuilder (fluent, frozen singleton)
PropMap mapper = new PropMapBuilder()
    .WithConfiguration(cfg => cfg.WithMaxDepth(8))
    .Configure<User, UserDto>(c =>
    {
        c.Ignore(x => x.PasswordHash);
        c.MapFrom(x => x.FullName, src => $"{src.FirstName} {src.LastName}");
        c.ReverseMap();
    })
    .Configure<Order, OrderDto>(c =>
        c.MapFrom(x => x.Total, src => src.Subtotal + src.Tax))
    .WarmupOnStartup<User, UserDto>()
    .Build();
```

## Dependency Injection

```csharp
// Minimal
services.AddPropertyMapper();

// With configuration
services.AddPropertyMapper(builder =>
{
    builder.WithConfiguration(cfg => cfg.WithMaxDepth(16));
    builder.Configure<User, UserDto>(c => c.Ignore(x => x.PasswordHash));
    builder.WarmupOnStartup<User, UserDto>();
});
```

Inject `IPropMap` (highly recommended) instead of `PropMap` directly:

```csharp
public class OrderService(IPropMap mapper) { }
```

## Benchmarks (Run 9 — .NET 10, Intel i5-7600K)

| Scenario | PropertyMapper | AutoMapper | Mapster |
|----------|---------------|------------|---------|
| Simple object | **13.5 ns** | 58.3 ns | 16.3 ns |
| Nested object | **40.4 ns** | 89.2 ns | 43.7 ns |
| Struct (zero-alloc) | **7.2 ns** | 54.7 ns | 7.4 ns |
| First call (cold path) | **278 μs** | 927 μs | 2,028 μs |
| Collection N=1 000 | **~31 μs** | ~35 μs | ~31 μs |

## License

MIT © 2025 vlasta81