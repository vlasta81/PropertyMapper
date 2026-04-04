# Changelog

All notable changes to PropertyMapper are documented here.
Format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

---

## [0.1.0] — 2026-04-04

Initial release.

### Added

#### Core mapping
- `Map<TIn,TOut>(TIn)` — IL-compiled delegate, `FrozenDictionary` cache, lock-free hot path
- `MapInto<TIn,TOut>(TIn, TOut)` — map onto an existing target instance
- `MapMerge<TIn1,TIn2,TOut>(TIn1, TIn2)` — merge two sources into one target
- `Clone<T>(T)` — shallow property-copy clone
- `MapOrDefault<TIn,TOut>(TIn)` — null-safe map, returns `null` when source is `null`
- `MapOrElse<TIn,TOut>(TIn, TOut)` / `MapOrElse<TIn,TOut>(TIn, Func<TOut>)` — map with fallback
- `TryMap<TIn,TOut>(TIn, out TOut)` — try-map pattern

#### Type support
- Mutable classes and structs
- Immutable records with primary constructors
- `init`-only properties
- Nullable wrapping, unwrapping, and nullable-to-nullable conversions
- `implicit` / `explicit` operator conversions
- Nested objects with configurable recursion depth (`MaxMappingDepth`)

#### Span / batch
- `MapBatch<TIn,TOut>(ReadOnlySpan<TIn>)` — map span, return `TOut[]`
- `MapBatchInPlace<TIn,TOut>(ReadOnlySpan<TIn>, Span<TOut>)` — map into pre-allocated span (zero extra allocation)

#### Collections
- `MapToList`, `MapToArray`, `MapArray`
- `MapToImmutableArray`, `MapToImmutableList`
- `MapToHashSet`
- `MapDictionary`, `MapToFrozenDictionary`
- `MapNestedList` — `IEnumerable<IEnumerable<TIn>>` → `List<List<TOut>>`
- `MapCollection<TIn,TOut,TOutCollection>` — any `ICollection<TOut>`
- `MapCollection<TIn,TOut>(IEnumerable<TIn>, ICollection<TOut>)` — append into existing collection

#### Async & streaming
- `MapAsync<TIn,TOut>(TIn, CancellationToken)` — single item via `Task.Run`
- `MapAsync<TIn,TOut>(IEnumerable<TIn>, CancellationToken)` — collection via `Task.Run`
- `MapParallelAsync<TIn,TOut>(IEnumerable<TIn>, int, CancellationToken)` — parallel via `Parallel.ForAsync`
- `MapStreamAsync<TIn,TOut>(IAsyncEnumerable<TIn>, CancellationToken)` — streaming one-at-a-time
- `MapStreamBatchedAsync<TIn,TOut>(IAsyncEnumerable<TIn>, int, CancellationToken)` — streaming in batches

#### Configuration
- `PropMapConfiguration` — `MaxMappingDepth`, `ThrowOnUnmappedProperties`, `WithMaxDepth`, `WithGlobalIgnore`
- `PropMapBuilder` — fluent frozen-singleton builder: `Configure`, `WithConfiguration`, `WithGlobalIgnore`, `WarmupOnStartup`, `Build`
- `TypePairConfiguration<TIn,TOut>` — per-pair rules: `Ignore`, `MapFrom`, `MapFromExpression`, `MapFromWithContext`, `AfterMap`, `ReverseMap`

#### Advanced
- `MapWithContext<TIn,TOut,TCtx>` — per-call context (exchange rates, tenant settings, etc.)
- `MapThenApplyMask<TIn,TOut>(TIn, IFieldMask<TOut>)` — map then zero out excluded fields
- `Project<TIn,TOut>(IQueryable<TIn>)` / `Project<TIn,TOut>(IQueryable<TIn>, IFieldMask<TOut>)` — EF Core / `IQueryable` projection
- `GetProjectionExpression<TIn,TOut>()` — cached `Expression<Func<TIn,TOut>>`
- `Validate<TIn,TOut>()` — reports unmapped target properties
- `Warmup<TIn,TOut>()` — pre-compile a single delegate
- `WarmupBatch(Type[])` — parallel pre-compilation of multiple pairs
- `GetStatistics()` — cache utilisation snapshot
- `Clear()` on `PropMap` — reset cache (intentionally absent from `IPropMap`)

#### Dependency Injection
- `AddPropertyMapper()` — registers `IPropMap` + `PropMap` as singletons
- `AddPropertyMapper(Action<PropMapBuilder>)` — with builder configuration
- Hosted warmup service — `WarmupOnStartup` pairs compiled before first request

#### Performance optimisations (async path)
- Delegate compiled before `Task.Run` entry to avoid IL-emit inside the thread pool
- Static `ParallelOptions` cache — eliminates per-call allocation in `MapParallelAsync`
- `List<T>` fast-path in `MapParallelAsync` — avoids `ToList()` copy for `IList<T>` inputs
- `MapParallelAsync` allocation reduced from 8,328 B → 1,760 B (−79%) vs initial implementation

#### Test suite
- 282 xUnit tests (281 passing, 1 timing-skipped)
- 11 test classes: Basic, Nullable, Nested, Struct, Record, Operator, Caching, EdgeCase, Performance, Concurrency, Integration

#### Benchmarks
- 13 BenchmarkDotNet classes covering all API surface
- `Run-Benchmarks.ps1` wrapper script (interactive menu, Quick mode, custom output dir)
- Results vs AutoMapper and Mapster across hot path, collections, async, warmup and cold-path scenarios
