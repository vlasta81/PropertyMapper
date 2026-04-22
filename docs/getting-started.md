# Getting Started

← [Back to README](../README.md)

## Prerequisites

- .NET 10 or later
- C# 13 or later

## Installation

```bash
dotnet add package PropertyMapper
```

Or via NuGet Package Manager:

```
Install-Package PropertyMapper
```

## First Mapping

Add `using PropertyMapper;` and create a `PropMap` instance. No profiles, no global state — just a plain object:

```csharp
using PropertyMapper;

PropMap mapper = new PropMap();

Order order = new Order { Id = 1, CustomerId = 42, Total = 199.99m };
OrderDto dto = mapper.Map<Order, OrderDto>(order);
```

PropertyMapper matches properties by name (case-sensitive) and handles compatible types automatically. The IL delegate is compiled on the first call and cached for every subsequent call.

## Supported Type Mappings

PropertyMapper handles these conversions without any configuration:

| Source | Target | Notes |
|--------|--------|-------|
| `T` | `T` | Direct copy |
| `T` | `T?` | Wrapping |
| `T?` | `T` | Unwrapping (`GetValueOrDefault`) |
| `T?` | `T?` | Nullable-to-nullable |
| `TSrc` | `TDst` (explicit op) | `explicit operator TDst(TSrc)` |
| `TSrc` | `TDst` (implicit op) | `implicit operator TDst(TSrc)` |
| Nested class | Nested class | Recursive, respects depth limit |

## Null Safety

When the source is `null`, `Map` throws `ArgumentNullException`. Use null-safe variants instead:

```csharp
// Returns null when source is null
OrderDto? dto = mapper.MapOrDefault<Order, OrderDto>(maybeNull);

// Returns a fallback value when source is null
OrderDto dto = mapper.MapOrElse<Order, OrderDto>(maybeNull, new OrderDto());

// Returns a fallback from a factory when source is null
OrderDto dto = mapper.MapOrElse<Order, OrderDto>(maybeNull, () => OrderDto.CreateEmpty());

// Try-map pattern
if (mapper.TryMap<Order, OrderDto>(maybeNull, out OrderDto? result))
{
    // result is non-null
}
```

## Dependency Injection

Register `IPropMap` as a singleton via `AddPropertyMapper()`:

```csharp
// Program.cs
using PropertyMapper.Extensions;

// Minimal
builder.Services.AddPropertyMapper();

// With configuration
builder.Services.AddPropertyMapper(b =>
{
    b.WithConfiguration(cfg => cfg.WithMaxDepth(8));
    b.Configure<User, UserDto>(c => c.Ignore(x => x.PasswordHash));
    b.WarmupOnStartup<User, UserDto>();
});
```

Inject `IPropMap` in your services:

```csharp
public class UserService(IPropMap mapper, IUserRepository repo)
{
    public async Task<UserDto> GetAsync(int id, CancellationToken ct)
    {
        User user = await repo.GetByIdAsync(id, ct).ConfigureAwait(false);
        return mapper.Map<User, UserDto>(user);
    }
}
```

> **Tip:** Inject `IPropMap` rather than the concrete `PropMap` — it is mockable in tests and does not expose `Clear()` to consumers.

## Warmup

The first call to any type pair compiles the IL delegate (~253 μs). For latency-sensitive services, pre-compile at startup:

```csharp
// Individual pair
mapper.Warmup<Order, OrderDto>();

// Multiple pairs in parallel
mapper.WarmupBatch(
    typeof(Order), typeof(OrderDto),
    typeof(User), typeof(UserDto),
    typeof(Product), typeof(ProductDto)
);

// Via builder (recommended with DI — runs as hosted service before first request)
builder.Services.AddPropertyMapper(b =>
{
    b.WarmupOnStartup<Order, OrderDto>();
    b.WarmupOnStartup<User, UserDto>();
});
```

## Next Steps

- [Configuration](configuration.md) — `PropMapBuilder`, per-type-pair rules, global options
- [Collections](collections.md) — collection and batch mapping
- [Async & Streaming](async.md) — async and parallel mapping
- [Advanced Features](advanced.md) — FieldMask, Projection, Clone, MapMerge, MapWithContext
- [API Reference](api/index.md) — full auto-generated API docs
