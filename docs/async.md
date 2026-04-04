# Async & Streaming

← [Back to README](../README.md)

PropertyMapper provides four async methods covering single items, collections, parallel execution and streaming `IAsyncEnumerable<T>` sources.

## MapAsync — Single Item

Offloads the mapping of a single object to a thread pool thread via `Task.Run`. Useful when the caller is on the UI or I/O thread and wants to yield:

```csharp
OrderDto dto = await mapper.MapAsync<Order, OrderDto>(order, cancellationToken)
    .ConfigureAwait(false);
```

> **Note:** For bulk work this is equivalent to calling `Map` inside `Task.Run`. Prefer `MapParallelAsync` for actual parallel throughput.

## MapAsync — Collection

Maps an entire `IEnumerable<TIn>` on a single thread-pool thread:

```csharp
IEnumerable<Order> orders = await repo.GetOrdersAsync(ct).ConfigureAwait(false);
OrderDto[] results = await mapper.MapAsync<Order, OrderDto>(orders, ct)
    .ConfigureAwait(false);
```

The mapper compiles the delegate before entering `Task.Run` to avoid IL-emit inside the thread pool.

## MapParallelAsync

Maps a collection in parallel using `Parallel.ForAsync`. Use when the source is large and per-item work justifies thread overhead:

```csharp
OrderDto[] results = await mapper.MapParallelAsync<Order, OrderDto>(
    orders,
    degreeOfParallelism: Environment.ProcessorCount,
    cancellationToken: ct)
    .ConfigureAwait(false);
```

The degree-of-parallelism parameter is forwarded to `ParallelOptions.MaxDegreeOfParallelism`. Pass `-1` for unbounded.

> For small collections (N ≤ ~100) `MapAsync` is typically faster due to lower scheduling overhead. See benchmark data below.

## MapStreamAsync

Maps items one-at-a-time from an `IAsyncEnumerable<TIn>` source. Preserves the streaming nature of the source — no buffering:

```csharp
IAsyncEnumerable<Order> stream = repo.StreamOrdersAsync(ct);

await foreach (OrderDto dto in mapper.MapStreamAsync<Order, OrderDto>(stream, ct)
    .ConfigureAwait(false))
{
    await writer.WriteAsync(dto, ct).ConfigureAwait(false);
}
```

## MapStreamBatchedAsync

Like `MapStreamAsync` but collects items into fixed-size batches before yielding:

```csharp
await foreach (OrderDto[] batch in mapper.MapStreamBatchedAsync<Order, OrderDto>(stream, batchSize: 100, ct)
    .ConfigureAwait(false))
{
    await bulkInsert.InsertAsync(batch, ct).ConfigureAwait(false);
}
```

Useful for batch-writing to a database or bulk-sending to an API while keeping memory usage bounded.

## Choosing the Right Method

| Scenario | Recommended method |
|----------|-------------------|
| Single object, async caller | `MapAsync<TIn,TOut>(TIn, CT)` |
| In-memory list, sequential | `MapAsync<TIn,TOut>(IEnumerable<TIn>, CT)` or synchronous `MapToList` |
| Large list, CPU-bound parallelism | `MapParallelAsync` |
| `IAsyncEnumerable` source, low memory | `MapStreamAsync` |
| `IAsyncEnumerable` source, batch processing | `MapStreamBatchedAsync` |

## Benchmark Results (Run 9 — Intel i5-7600K, .NET 10)

| Method | N=10 | N=100 | N=1 000 |
|--------|-----:|------:|--------:|
| ManualSequential (baseline) | 316 ns | 2,822 ns | 30,926 ns |
| `MapAsync_Sequential` | 1,145 ns | 5,701 ns | 39,572 ns |
| `ManualAsync_TaskRun` | 1,182 ns | 5,806 ns | 37,037 ns |
| `MapParallelAsync` | 3,378 ns | 11,629 ns | 81,350 ns |

Key observations:
- `MapAsync` for N=10 is **3% faster** than an equivalent `Task.Run` wrapper.
- `MapParallelAsync` has higher overhead for small N; it pays off for CPU-bound workloads at large N with multiple physical cores.
- All async variants include `Task.Run` scheduling cost vs the synchronous baseline.

→ Full results: [benchmarks/PropertyMapper.Benchmarks/README.md](../benchmarks/PropertyMapper.Benchmarks/README.md)

## Related

- [Collections](collections.md) — synchronous collection mapping
- [API Reference — IPropMap](api/IPropMap.md)
