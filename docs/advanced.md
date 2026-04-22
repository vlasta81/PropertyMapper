# Advanced Features

← [Back to README](../README.md)

## Clone

Creates a shallow property-copy clone of an object (same type, new instance):

```csharp
Order clone = mapper.Clone(order);
```

Benchmarked at **13.8 ns** vs manual copy 10.3 ns — AutoMapper 55.8 ns.

---

## MapInto

Maps matching properties from source onto an **existing** target instance. Useful for updating domain entities from DTOs:

```csharp
UserDto dto = GetUpdatedDto();
User existingUser = await repo.GetAsync(dto.Id).ConfigureAwait(false);

mapper.MapInto<UserDto, User>(dto, existingUser);
await repo.SaveAsync(existingUser).ConfigureAwait(false);
```

---

## MapMerge

Creates a new `TOut` from `source1`, then merges matching properties from `source2` onto it. `source2` properties overwrite `source1` where names match:

```csharp
// Combine a base profile with request overrides
UserDto merged = mapper.MapMerge<UserBase, UserPatch, UserDto>(baseUser, patch);
```

---

## Null-Safe Variants

```csharp
// Returns null when source is null (no throw)
OrderDto? dto = mapper.MapOrDefault<Order, OrderDto>(maybeNull);

// Returns a fallback value
OrderDto dto = mapper.MapOrElse<Order, OrderDto>(maybeNull, OrderDto.Empty);

// Returns result of a factory (lazy)
OrderDto dto = mapper.MapOrElse<Order, OrderDto>(maybeNull, () => new OrderDto { Id = -1 });

// Try-map pattern
if (mapper.TryMap<Order, OrderDto>(maybeNull, out OrderDto? result))
{
    Process(result);
}
```

---

## MapWithContext

Passes a per-call context value to properties configured with `MapFromWithContext`. The context is not compiled into the IL delegate, making it safe for per-request state:

```csharp
// Configuration (at build time)
PropMap mapper = new PropMapBuilder()
    .Configure<Order, OrderDto>(c =>
        c.MapFromWithContext<ExchangeRates, decimal>(
            x => x.TotalEur,
            (src, rates) => src.TotalUsd * rates.UsdToEur))
    .Build();

// Runtime
ExchangeRates rates = GetCurrentRates();
OrderDto dto = mapper.MapWithContext<Order, OrderDto, ExchangeRates>(order, rates);
```

---

## FieldMask

`MapThenApplyMask` maps the object and then zeroes out (sets to `default`) any properties excluded by the `IFieldMask<TOut>`. Useful for partial-response APIs:

```csharp
using PropertyMapper.Masking;

// Mask that zeroes out sensitive fields after mapping
FieldMask<UserDto> mask = new FieldMask<UserDto>("Email", "Phone");

UserDto dto = mapper.MapThenApplyMask<User, UserDto>(user, mask);
// dto.Email, dto.Phone are null/default
```

### Benchmark (hot path, Run 10)

| Method | Mean |
|--------|-----:|
| `Map` (no mask) | 15.3 ns |
| `MapThenApplyMask` (1 field) | 15.3 ns |
| `MapThenApplyMask` (3 fields) | 18.6 ns |

---

## IQueryable Projection (EF Core)

`Project` applies the cached `Expression<Func<TIn,TOut>>` to an `IQueryable<TIn>`, allowing the database provider to generate a SQL `SELECT` with only the needed columns:

```csharp
// Without mask — uses cached expression, fully translated by EF Core
IQueryable<UserDto> query = mapper.Project<User, UserDto>(dbContext.Users);
List<UserDto> dtos = await query.ToListAsync(ct).ConfigureAwait(false);

// With field mask — excludes sensitive fields from the SQL SELECT
FieldMask<UserDto> mask = new FieldMask<UserDto>("Email", "Phone");
IQueryable<UserDto> query = mapper.Project<User, UserDto>(dbContext.Users, mask);

// Obtain the expression directly
Expression<Func<User, UserDto>> expr = mapper.GetProjectionExpression<User, UserDto>();
```

> Use `MapFromExpression` (not `MapFrom`) in `Configure` to keep custom mappings translatable by LINQ providers.

---

## Validate

Reports target properties that have no matching source property. Use this at startup or in tests to catch mapping gaps early:

```csharp
MappingValidationResult result = mapper.Validate<Order, OrderDto>();
if (!result.IsValid)
{
    throw new InvalidOperationException($"Unmapped: {string.Join(", ", result.UnmappedTargetProperties)}");
}
```

---

## Diagnostics & Statistics

`GetStatistics()` returns a point-in-time snapshot of the delegate cache:

```csharp
MappingStatistics stats = mapper.GetStatistics();
Console.WriteLine($"Cached mappers: {stats.CachedMappers}");
Console.WriteLine($"Cached plans:   {stats.CachedPlans}");
Console.WriteLine($"Memory estimate: {stats.TotalMemoryBytes} bytes");
```

> `Clear()` is `internal` and not part of the public API. It exists solely for test isolation
> (accessible to `PropertyMapper.Tests` via `InternalsVisibleTo`). Production code — including
> code that injects `IPropMap` — never needs to reset the cache.

---

## Related

- [Configuration](configuration.md) — `MapFromWithContext`, `MapFromExpression` setup
- [API Reference — IPropMap](api/IPropMap.md)
