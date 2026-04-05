# PropertyMapper

High-performance, IL-based property mapper for .NET 10+. Delegates are compiled once via `Reflection.Emit` and cached in a lock-free `FrozenDictionary` — the hot path is as fast as hand-written code.

## Features

- **IL code generation** — mapping delegates compiled once; zero overhead on repeated calls.
- **Zero-allocation hot path** — lock-free `FrozenDictionary` cache after first compilation.
- **Structs, records, classes** — mutable classes/structs and immutable records with primary constructors.
- **Nested objects** — recursive mapping with configurable depth limit.
- **Nullable support** — wrapping, unwrapping, and nullable-to-nullable conversions.
- **Operator conversions** — respects `implicit`/`explicit` operators.
- **Rich collections** — `List<T>`, `T[]`, `ImmutableArray<T>`, `ImmutableList<T>`, `HashSet<T>`, `FrozenDictionary<K,V>`, nested `List<List<T>>`.
- **Async & parallel** — `MapAsync`, `MapParallelAsync`, `MapStreamAsync`, `MapStreamBatchedAsync`.
- **Fluent configuration** — `PropMapBuilder` with per-type-pair `Ignore`, `MapFrom`, `AfterMap`, `ReverseMap`.
- **FieldMask / Projection** — `MapThenApplyMask`, `Project<TIn,TOut>` for EF Core / `IQueryable`.
- **Dependency Injection** — `AddPropertyMapper()` for `IServiceCollection` with hosted warmup.
- **Diagnostics** — `Validate<TIn,TOut>()`, `GetStatistics()`, `Warmup`, `WarmupBatch`.

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

// Span batch (zero extra allocation)
OrderDto[] batch = mapper.MapBatch<Order, OrderDto>(orders.AsSpan());

// Async
OrderDto dto = await mapper.MapAsync<Order, OrderDto>(order, cancellationToken);

// Parallel async
List<OrderDto> results = await mapper.MapParallelAsync<Order, OrderDto>(orders, maxDegreeOfParallelism: 4, cancellationToken);

// Streaming (IAsyncEnumerable)
await foreach (OrderDto item in mapper.MapStreamAsync<Order, OrderDto>(asyncOrders, cancellationToken))
{
    // process
}
```

## Configuration

```csharp
using PropertyMapper.Configuration;

// Simple — PropMapConfiguration
PropMap mapper = new PropMap(new PropMapConfiguration
{
    MaxMappingDepth = 8,
    ThrowOnUnmappedProperties = true
});

// Advanced — PropMapBuilder (fluent, builds a frozen singleton)
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
    {
        c.MapFrom(x => x.Total, src => src.Subtotal + src.Tax);
        c.AfterMap((src, dst) => dst.Label = src.Total > 1000 ? "VIP" : "Standard");
    })
    .WarmupOnStartup<User, UserDto>()
    .Build();
```

## Dependency Injection

```csharp
// Minimal
services.AddPropertyMapper();

// With configuration + startup warmup
services.AddPropertyMapper(builder =>
{
    builder.WithConfiguration(cfg => cfg.WithMaxDepth(16));
    builder.Configure<User, UserDto>(c => c.Ignore(x => x.PasswordHash));
    builder.WarmupOnStartup<User, UserDto>();
    builder.WarmupOnStartup<Order, OrderDto>();
});
```

Inject `IPropMap` (recommended — mockable in tests):

```csharp
public class OrderService(IPropMap mapper)
{
    public OrderDto GetDto(Order order) => mapper.Map<Order, OrderDto>(order);
}
```

## API Overview

### Core mapping

| Method | Description |
|--------|-------------|
| `Map<TIn,TOut>(TIn)` | Map a single object |
| `MapInto<TIn,TOut>(TIn, TOut)` | Map onto an existing target instance |
| `MapMerge<TIn1,TIn2,TOut>(TIn1, TIn2)` | Merge two sources into one target |
| `Clone<T>(T)` | Shallow property-copy clone |
| `MapOrDefault<TIn,TOut>(TIn)` | Map or return `null` when source is `null` |
| `MapOrElse<TIn,TOut>(TIn, TOut)` | Map or return a fallback value |
| `MapOrElse<TIn,TOut>(TIn, Func<TOut>)` | Map or invoke a fallback factory |
| `TryMap<TIn,TOut>(TIn, out TOut)` | Try-map pattern |

### Span / batch

| Method | Description |
|--------|-------------|
| `MapBatch<TIn,TOut>(ReadOnlySpan<TIn>)` | Map a span, return `TOut[]` |
| `MapBatchInPlace<TIn,TOut>(ReadOnlySpan<TIn>, Span<TOut>)` | Map into a pre-allocated span |

### Collections

| Method | Description |
|--------|-------------|
| `MapToList<TIn,TOut>(IEnumerable<TIn>)` | → `List<TOut>` |
| `MapToList<TIn,TOut>(ReadOnlySpan<TIn>)` | → `List<TOut>` from span |
| `MapToArray<TIn,TOut>(IEnumerable<TIn>)` | → `TOut[]` |
| `MapArray<TIn,TOut>(TIn[])` | Source array → `TOut[]` |
| `MapToImmutableArray<TIn,TOut>(IEnumerable<TIn>)` | → `ImmutableArray<TOut>` |
| `MapToImmutableList<TIn,TOut>(IEnumerable<TIn>)` | → `ImmutableList<TOut>` |
| `MapToHashSet<TIn,TOut>(IEnumerable<TIn>)` | → `HashSet<TOut>` |
| `MapDictionary<...>(Dictionary<TInKey,TInValue>)` | → `Dictionary<TOutKey,TOutValue>` |
| `MapToFrozenDictionary<...>(Dictionary<TInKey,TInValue>)` | → `FrozenDictionary<TOutKey,TOutValue>` |
| `MapNestedList<TIn,TOut>(IEnumerable<IEnumerable<TIn>>)` | → `List<List<TOut>>` |
| `MapCollection<TIn,TOut,TOutCollection>(IEnumerable<TIn>)` | → any `ICollection<TOut>` |
| `MapCollection<TIn,TOut>(IEnumerable<TIn>, ICollection<TOut>)` | Append into existing collection |

### Async & streaming

| Method | Description |
|--------|-------------|
| `MapAsync<TIn,TOut>(TIn, CT)` | Single item on a `Task.Run` thread |
| `MapAsync<TIn,TOut>(IEnumerable<TIn>, CT)` | Collection on a `Task.Run` thread |
| `MapParallelAsync<TIn,TOut>(IEnumerable<TIn>, int, CT)` | Parallel with `Parallel.ForAsync` |
| `MapStreamAsync<TIn,TOut>(IAsyncEnumerable<TIn>, CT)` | Streaming one-at-a-time |
| `MapStreamBatchedAsync<TIn,TOut>(IAsyncEnumerable<TIn>, int, CT)` | Streaming in batches |

### Advanced

| Method | Description |
|--------|-------------|
| `MapWithContext<TIn,TOut,TCtx>(TIn, TCtx)` | Per-call context (e.g. tenant, exchange rate) |
| `MapThenApplyMask<TIn,TOut>(TIn, IFieldMask<TOut>)` | Map then zero out excluded fields |
| `Project<TIn,TOut>(IQueryable<TIn>)` | `IQueryable` projection (EF Core) |
| `Project<TIn,TOut>(IQueryable<TIn>, IFieldMask<TOut>)` | Projection with field mask |
| `GetProjectionExpression<TIn,TOut>()` | Cached `Expression<Func<TIn,TOut>>` |
| `Validate<TIn,TOut>()` | Reports unmapped target properties |
| `Warmup<TIn,TOut>()` | Pre-compile a single type-pair delegate |
| `WarmupBatch(Type[])` | Pre-compile multiple pairs concurrently |
| `GetStatistics()` | Cache utilisation snapshot |

## Benchmarks

Measured with BenchmarkDotNet v0.15.8, .NET 10, Intel i5-7600K, Server GC.

| Scenario | PropertyMapper | AutoMapper | Mapster |
|----------|:-------------:|:---------:|:------:|
| Simple object (hot) | **13.5 ns** | 58.3 ns | 16.3 ns |
| Nested object (hot) | **40.4 ns** | 89.2 ns | 43.7 ns |
| Struct (zero-alloc) | **7.2 ns** | 54.7 ns | 7.4 ns |
| Record | **17.0 ns** | 60.4 ns | 21.9 ns |
| Wide object (10 props) | **30.4 ns** | 80.9 ns | 32.7 ns |
| First call (cold path) | **278 μs** | 927 μs | 2,028 μs |
| Collection N=1 000 | **~31 μs** | ~35 μs | ~31 μs |

→ Full results: [benchmarks/PropertyMapper.Benchmarks/README.md](benchmarks/PropertyMapper.Benchmarks/README.md)

## Documentation

| Page | Description |
|------|-------------|
| [Getting Started](docs/getting-started.md) | Installation, first mapping, DI setup |
| [Configuration](docs/configuration.md) | `PropMapBuilder`, type-pair rules, global options |
| [Collections](docs/collections.md) | All collection mapping methods |
| [Async & Streaming](docs/async.md) | `MapAsync`, `MapParallelAsync`, `MapStreamAsync` |
| [Advanced Features](docs/advanced.md) | FieldMask, Projection, Clone, MapMerge, MapWithContext |
| [API Reference](docs/api/index.md) | Auto-generated XML doc reference |
| [Benchmarks](benchmarks/PropertyMapper.Benchmarks/README.md) | BenchmarkDotNet results vs AutoMapper & Mapster |
| [Tests](tests/PropertyMapper.Tests/README.md) | Test suite overview (282 tests) |

## License

MIT © 2025 vlasta81
