# PropertyMapper Tests

← [Back to README](../../README.md)

Comprehensive xUnit test suite for the PropertyMapper library.

## Test Statistics

| Metric | Value |
|--------|-------|
| Total tests | **282** |
| Passing | **281** |
| Skipped | 1 (timing-sensitive, environment-dependent) |
| Framework | xUnit (.NET 10) |

## Running Tests

### Visual Studio

```
Test Explorer → Run All Tests
```

### CLI

```powershell
dotnet test tests\PropertyMapper.Tests\PropertyMapper.Tests.csproj
```

### With code coverage

```powershell
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover
```

## Test Classes

### 1. BasicMappingTests
Core mapping behaviour:
- ✅ Primitive types (`int`, `string`, `bool`, `DateTime`)
- ✅ `null` input validation
- ✅ Cache reuse across calls
- ✅ Default value handling

### 2. NullableConversionTests
Nullable type conversions:
- ✅ `Nullable<T>` → `Nullable<T>` (with value and `null`)
- ✅ `T` → `T?` (wrapping)
- ✅ `T?` → `T` (unwrapping via `GetValueOrDefault`)
- ✅ Mixed nullable/value properties

### 3. NestedMappingTests
Recursive object mapping:
- ✅ Simple nested objects
- ✅ `null` nested objects
- ✅ Multi-level nesting
- ✅ Cache reuse for nested type pairs

### 4. StructMappingTests
Value type mapping:
- ✅ `struct` → `struct`
- ✅ Record structs
- ✅ Default value preservation
- ✅ Value-type copy semantics

### 5. RecordMappingTests
Record type mapping:
- ✅ `record` → `record`
- ✅ `init`-only properties
- ✅ Empty strings and default values
- ✅ Negative numbers

### 6. OperatorConversionTests
Custom operator conversions:
- ✅ `explicit` operators (`Meters` ↔ `Feet`)
- ✅ Zero values
- ✅ Large values

### 7. CachingTests
Cache mechanism:
- ✅ Delegate caching across calls
- ✅ Multiple type pairs
- ✅ `Clear()` resets cache
- ✅ `Warmup<TIn,TOut>()` API
- ✅ `WarmupBatch()` with parallelism
- ✅ `GetStatistics()` snapshot

### 8. EdgeCaseTests
Boundary conditions:
- ✅ Extra source properties (ignored)
- ✅ Missing source properties (default on target)
- ✅ Read-only properties
- ✅ `int.MinValue` / `int.MaxValue`
- ✅ `DateTime.MinValue` / `DateTime.MaxValue`
- ✅ Unicode strings
- ✅ Long strings (10 000+ characters)
- ✅ Theory tests with varied inputs

### 9. PerformanceTests
Performance assertions:
- ✅ First call (IL compilation) completes within budget
- ✅ Subsequent calls average < 10 μs
- ✅ Nested mapping performance
- ✅ Warmup reduces first-call latency
- ✅ Bulk processing (1 000 objects)
- ✅ Struct mapping average < 1 000 ns
- ✅ Parallel `WarmupBatch`

### 10. ConcurrencyTests
Thread-safety:
- ✅ Concurrent mapping of the same type pair
- ✅ Concurrent mapping of different type pairs
- ✅ Concurrent first-call (double-checked locking)
- ✅ Concurrent `WarmupBatch`
- ✅ `Clear()` while mapping
- ✅ Nested mapping concurrency

### 11. IntegrationTests
Real-world scenarios:
- ✅ E-commerce: `Product` → `ProductDto` (with `Category`)
- ✅ User profile: multi-level nesting (`User` → `Profile` → `Address`)
- ✅ API response: generic records
- ✅ Bulk processing: 1 000 records
- ✅ Nullable fields: mixed `null`/value inputs

## Test Models

All test models are defined in `TestModels.cs`:

| Model group | Types |
|-------------|-------|
| Simple | `SimpleSource`, `SimpleTarget` |
| Nullable | `NullableSource`, `NullableTarget` |
| Nested | `PersonWithAddress`, `Address` |
| Structs | `PointStruct`, `VectorStruct` |
| Records | `PersonRecord`, `OrderRecord` |
| Operators | `Meters`, `Feet` |
| Complex nested | `Company`, `Employee` |
| Edge cases | `ReadOnlySource`, `InitOnlyRecord` |

## Test Strategy

| Type | Purpose |
|------|---------|
| **Unit tests** | Isolate individual features |
| **Integration tests** | Verify real-world scenarios end-to-end |
| **Performance tests** | Assert timing budgets (single skipped when OS scheduler is noisy) |
| **Concurrency tests** | Prove thread-safety under race conditions |
| **Edge case tests** | Cover boundary inputs and unusual type combinations |

## Coverage Goals

| Metric | Target |
|--------|--------|
| Line coverage | > 90% |
| Branch coverage | > 85% |
| Public API surface | 100% |

## Performance Reference (from benchmarks)

| Operation | Measured time |
|-----------|--------------|
| First call — IL compilation | ~278 μs |
| Hot path — simple object | ~13.5 ns |
| Hot path — struct | ~7.2 ns |
| Hot path — nested object | ~40.4 ns |
| Batch — 1 000 objects | ~31 μs |

→ Full benchmark results: [benchmarks/PropertyMapper.Benchmarks/README.md](../../benchmarks/PropertyMapper.Benchmarks/README.md)
