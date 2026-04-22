# Collections

← [Back to README](../README.md)

PropertyMapper provides a full set of typed collection mapping methods. All delegates are compiled on the first call and cached — subsequent calls have near-zero overhead.

## List

```csharp
// IEnumerable<TIn> → List<TOut>
List<OrderDto> list = mapper.MapToList<Order, OrderDto>(orders);

// ReadOnlySpan<TIn> → List<TOut>
List<OrderDto> list = mapper.MapToList<Order, OrderDto>(orders.AsSpan());
```

## Array

```csharp
// IEnumerable<TIn> → TOut[]
OrderDto[] array = mapper.MapToArray<Order, OrderDto>(orders);

// TIn[] → TOut[] (source is already an array)
OrderDto[] array = mapper.MapArray<Order, OrderDto>(sourceArray);
```

## Span Batch

```csharp
// ReadOnlySpan<TIn> → new TOut[] (allocates the output array)
OrderDto[] batch = mapper.MapBatch<Order, OrderDto>(orders.AsSpan());

// ReadOnlySpan<TIn> → pre-allocated Span<TOut> (zero extra allocation)
OrderDto[] destination = new OrderDto[orders.Length];
mapper.MapBatchInPlace<Order, OrderDto>(orders.AsSpan(), destination.AsSpan());
```

`MapBatchInPlace` is the lowest-overhead option for high-throughput scenarios. The destination span must be at least as long as the source span.

## Immutable Collections

```csharp
ImmutableArray<OrderDto> ia = mapper.MapToImmutableArray<Order, OrderDto>(orders);
ImmutableList<OrderDto> il = mapper.MapToImmutableList<Order, OrderDto>(orders);
```

## HashSet

```csharp
HashSet<OrderDto> set = mapper.MapToHashSet<Order, OrderDto>(orders);
```

`TOut` must implement `IEquatable<TOut>` or provide a comparer for meaningful set semantics.

## Dictionary

```csharp
// Dictionary<TInKey,TInValue> → Dictionary<TOutKey,TOutValue>
Dictionary<int, OrderDto> dict =
    mapper.MapDictionary<int, Order, int, OrderDto>(sourceDict);

// Dictionary<TInKey,TInValue> → FrozenDictionary<TOutKey,TOutValue>
FrozenDictionary<int, OrderDto> frozen =
    mapper.MapToFrozenDictionary<int, Order, int, OrderDto>(sourceDict);
```

## Nested Lists

```csharp
// IEnumerable<IEnumerable<TIn>> → List<List<TOut>>
List<List<OrderDto>> nested =
    mapper.MapNestedList<Order, OrderDto>(groupedOrders);
```

## Generic Collection (into any ICollection)

```csharp
// IEnumerable<TIn> → new TOutCollection (must implement ICollection<TOut>)
SortedSet<OrderDto> sorted =
    mapper.MapCollection<Order, OrderDto, SortedSet<OrderDto>>(orders);

// Append into an existing ICollection<TOut>
ObservableCollection<OrderDto> target = new ObservableCollection<OrderDto>();
mapper.MapCollection<Order, OrderDto>(orders, target);
```

## Benchmark Results (N=1 000, Run 10)

| Benchmark | Mean | vs Manual |
|-----------|-----:|----------:|
| Manual | 27,828 ns | baseline |
| `MapToList` (Span) | 28,524 ns | +2.5% |
| `PropertyMapper` (MapToList) | 28,737 ns | +3.3% |
| `MapToArray` | 28,737 ns | +3.3% |
| `MapToImmutableArray` | 29,230 ns | +5.0% |
| Mapster | 28,074 ns | +0.9% |
| AutoMapper | 31,293 ns | +12.5% |

Batch (Span):

| Benchmark | Mean | vs Manual |
|-----------|-----:|----------:|
| Manual | 25,466 ns | baseline |
| `MapBatchInPlace` | 27,600 ns | +8.4% |
| `MapBatch` | 27,109 ns | +6.5% |

→ Full results: [benchmarks/PropertyMapper.Benchmarks/README.md](../benchmarks/PropertyMapper.Benchmarks/README.md)

## Related

- [Async & Streaming](async.md) — async collection mapping
- [API Reference — IPropMap](api/IPropMap.md)
