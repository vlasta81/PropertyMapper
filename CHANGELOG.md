# Changelog

All notable changes to PropertyMapper are documented here.
Format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

---

## [0.2.0] — 2026-04-22

### Added

#### Core
- `IsForbiddenContextType(Type)` — shared predicate in `ClosureInspector` that blocks `IServiceProvider`, `IServiceScopeFactory`, `HttpContext`, and `DbContext`-derived types from being used as mapping context; enforced both at configuration time (`GuardAgainstSingletonContext`, throws `InvalidOperationException`) and at call time (`MapWithContext`, throws `ArgumentException`)

#### Benchmarks
- `ContextBenchmark` — four methods measuring `MapWithContext` overhead: `Map_NoContext` baseline, `MapWithContext_NoSetters`, `MapWithContext_OneContextSetter`, `MapWithContext_ThreeContextSetters`
- `StatisticsBenchmark` — two methods measuring `GetStatistics` lock-acquire cost vs lock-free `Map_Hot` baseline
- `AsyncBenchmark` — added `MapStreamAsync_Collect` and `MapStreamBatchedAsync_Collect` (batchSize=10) methods; added private `GenerateAsync()` helper using `Task.Yield()`
- `Run-Benchmarks.ps1` — `Context` and `Statistics` entries added to `[ValidateSet]`, `$Registry`, `.PARAMETER` description, and two new `.EXAMPLE` blocks

#### Test suite
- `SecurityGuardTests` — 3 new tests: `IServiceScopeFactory` rejected at configuration time, `IServiceScopeFactory` rejected at call time, valid context type accepted
- `AsyncMappingTests` — 1 new test: mid-loop cancellation of `MapStreamAsync` propagates `OperationCanceledException`
- `ConcurrencyTests` — 1 new test: concurrent `GetStatistics` calls return consistent non-negative snapshot values

### Changed

#### Core
- `GetStatistics()` now acquires `_compileLock` before reading `_mappers`, `_plans`, and `_delegateSizes` — guarantees a consistent three-dictionary snapshot under concurrent compilation; overhead is ~4 ns uncontended (previously lock-free but potentially inconsistent)
- `WarmupBatch` — processes type pairs in a single sequential pass (no `Parallel.ForEach`); eliminates thread-scheduling noise on the cold path and simplifies allocation tracking
- `WarmupBenchmark` — class-level XML summary corrected: "single sequential pass; amortises lock acquisition" (was "sequential for-loop, no thread overhead")

#### Documentation
- `benchmarks/README.md` — Run 10 (2026-04-22) results section added; Run 9 preserved as archive; Context and Statistics result sections updated with actual measured values (Context: 80 ns / 171 ns; Statistics: 17.5 ns); Async table extended with streaming methods
- `src/PropertyMapper/README.md` — **Context-aware mapping** row added to Key Features table; `MapWithContext (1 setter) | 17.4 ns` row added to benchmark summary
- `tests/PropertyMapper.Tests/README.md` — test count updated to 286 / 285; performance reference table updated to Run 10 values; `MapWithContext` row added
- `README.md` — benchmark table updated to Run 10 values; `MapWithContext (1 setter) | 80 ns` row added; Tests documentation link corrected to 286 tests
- `docs/getting-started.md` — warmup latency updated: ~278 μs → ~253 μs
- `docs/collections.md` — Collection and Batch Span benchmark tables updated to Run 10 values
- `docs/async.md` — benchmark table updated to Run 10 with `MapStreamAsync_Collect` and `MapStreamBatchedAsync_Collect` rows; guidance on streaming overhead added
- `docs/advanced.md` — FieldMask benchmark table updated to Run 10 values (Run 9 → Run 10)

### Fixed

#### Code quality
- `ClosureInspector` — `EndsWith` calls replaced with `EndsWith(string, StringComparison.Ordinal)` to eliminate implicit culture-sensitive comparison
- `PropMap.Context.cs` — `Unsafe.AsRef<T>(in T)` updated to `Unsafe.AsRef<T>(ref Unsafe.AsRef(in value))` / correct overload for .NET 10 `ref readonly` semantics
- `PropMap.Async.cs` — `CancellationToken` now threaded through the mid-loop `await` in `MapStreamAsync` so that cancellation is observed between items, not only at stream entry
- `WarmupService` — added null-guard for `_pairsToWarm`; service no longer throws `NullReferenceException` when registered without explicit warmup pairs
- `PropMap.Async.cs` — `SelectMany` LINQ chain replaced with a pre-allocated `List<T>` to eliminate hidden intermediate allocations in `MapParallelAsync`
- `PropMapBuilder` — `ArgumentOutOfRangeException.ThrowIfLessThan` / `ThrowIfGreaterThan` used instead of manual `if + throw` for numeric guard clauses

#### Documentation
- `benchmarks/README.md` — `MapWithContext` estimated values (17–23 ns) corrected to actual measurements (80 ns / 171 ns); `GetStatistics` estimated value (118 ns / +104 ns) corrected to actual measurement (17.5 ns / +4.4 ns)
- `docs/async.md` — "concurrently" removed from `WarmupBatch` description (batch is sequential, not parallel)

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
- `Clear()` on `PropMap` — reset cache; `internal` (intentionally absent from `IPropMap`)

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

### Changed
- `Clear()` on `PropMap` changed from `public` to `internal` — accessible only within the library and the test project (via `InternalsVisibleTo`); never part of the `IPropMap` DI contract
- NuGet package metadata: added `<PackageIcon>icon.png</PackageIcon>` (128 × 128 px) and `<RepositoryType>git</RepositoryType>`; removed duplicate `<PackageLicenseExpression>` entry from `.csproj`

### Fixed
- `docs/advanced.md`: `FieldMask<T>` examples corrected — constructor accepts *excluded* property names (`params string[]`); non-existent static `Include()` calls removed from all code samples
- `docs/advanced.md`: `Validate<TIn,TOut>()` return type corrected to `MappingValidationResult` (record struct with `IsValid` and `UnmappedTargetProperties`); was incorrectly shown as `IReadOnlyList<string>`
- `docs/advanced.md`: `GetStatistics()` member names corrected to `CachedMappers`, `CachedPlans`, and `TotalMemoryBytes`
- `docs/advanced.md`, `docs/async.md`, `README.md`: `MapParallelAsync` return type corrected to `Task<List<TOut>>`; parameter name corrected to `maxDegreeOfParallelism`
- `docs/advanced.md`: Typo `concretMapper` → `concreteMapper`
