# PropertyMapper Benchmarks

← [Back to README](../../README.md)

BenchmarkDotNet v0.15.8 benchmark suite comparing PropertyMapper against AutoMapper and Mapster across all major mapping scenarios.

## Environment

```
BenchmarkDotNet v0.15.8, Windows 11 (25H2)
Intel Core i5-7600K CPU 3.80GHz (Kaby Lake), 4 cores
.NET SDK 10.0.201 / .NET 10.0.5, X64 RyuJIT x86-64-v3
Job: Server=True, IterationCount=15, LaunchCount=2, WarmupCount=5
```

## Benchmark Classes

| Script parameter | Class | Description |
|-----------------|-------|-------------|
| `Simple` | `SimpleObjectBenchmark` | Simple 4-property POCO — hot path |
| `Wide` | `WideObjectBenchmark` | Wide 10-property POCO — hot path |
| `Nested` | `NestedObjectBenchmark` | Object with one nested child — hot path |
| `Record` | `RecordBenchmark` | Record types with `init`-only properties |
| `Struct` | `StructBenchmark` | Value types — zero allocation path |
| `Clone` | `CloneBenchmark` | `Clone<T>` vs manual copy |
| `FieldMask` | `FieldMaskBenchmark` | `Map` vs `MapThenApplyMask` (1 and 3 fields) |
| `Collection` | `CollectionBenchmark` | `MapToList`, `MapToArray`, `ImmutableArray` — N=10/100/1 000 |
| `Batch` | `BatchBenchmark` | `MapBatch`, `MapBatchInPlace` — N=10/100/1 000 |
| `Async` | `AsyncBenchmark` | `MapAsync`, `MapParallelAsync` — N=10/100/1 000 |
| `Dictionary` | `DictionaryBenchmark` | `MapDictionary` — N=10/100/1 000 |
| `Warmup` | `WarmupBenchmark` | Cold-path IL compilation cost |
| `FirstCall` | `FirstCallBenchmark` | First-ever call (cold path) vs AutoMapper/Mapster |

## Running Benchmarks

Use the `Run-Benchmarks.ps1` wrapper script (requires PowerShell 7+):

```powershell
# Interactive menu
.\scripts\Run-Benchmarks.ps1

# All benchmarks (~60+ minutes)
.\scripts\Run-Benchmarks.ps1 -Benchmark All

# Single class
.\scripts\Run-Benchmarks.ps1 -Benchmark Simple

# Quick functional verification (non-representative)
.\scripts\Run-Benchmarks.ps1 -Benchmark Collection -Quick

# Custom output directory, no auto-open
.\scripts\Run-Benchmarks.ps1 -Benchmark Async -OutputDir C:\bench\results -NoOpen

# List available methods without running
.\scripts\Run-Benchmarks.ps1 -List
```

Results are saved to `docs/results/<timestamp>_<Benchmark>/`.

---

## Results — Run 9 (2026-04-04)

Full report: [docs/results/BenchmarkRun-joined-2026-04-04-15-34-12-report-github.md](docs/results/BenchmarkRun-joined-2026-04-04-15-34-12-report-github.md)

### Single-Object Hot Path

| Benchmark | PropertyMapper | AutoMapper | Mapster | Manual |
|-----------|:-------------:|:---------:|:------:|:------:|
| Struct | **7.2 ns** | 54.7 ns | 7.4 ns | ~0 ns† |
| Simple object | **13.5 ns** | 58.3 ns | 16.3 ns | 10.1 ns |
| Clone | **13.8 ns** | 55.8 ns | 18.4 ns | 10.3 ns |
| Record | **17.0 ns** | 60.4 ns | 21.9 ns | 13.6 ns |
| Wide object (10 props) | **30.4 ns** | 80.9 ns | 32.7 ns | 27.0 ns |
| Nested object | **40.4 ns** | 89.2 ns | 43.7 ns | 36.1 ns |

† Struct manual is JIT-eliminated in the benchmark.

Allocations — all single-object scenarios: **40 B** (same as manual). Struct: **0 B**.

### FieldMask Hot Path

| Method | Mean | Overhead vs plain Map |
|--------|-----:|-----------------------:|
| `Map` (no mask) | 13.6 ns | baseline |
| `MapThenApplyMask` (1 field) | 15.2 ns | +1.6 ns |
| `MapThenApplyMask` (3 fields) | 18.8 ns | +5.2 ns |

### Collection (N=1 000)

| Benchmark | Mean | vs Manual |
|-----------|-----:|----------:|
| Manual | 30,734 ns | baseline |
| `MapToList` (Span) | 30,862 ns | +0.4% |
| `MapToList` | 31,686 ns | +3.1% |
| `MapToArray` | 31,949 ns | +3.9% |
| `MapToImmutableArray` | 31,740 ns | +3.3% |
| Mapster | 30,583 ns | −0.5% |
| AutoMapper | 35,257 ns | +14.7% |

### Batch Span (N=1 000)

| Benchmark | Mean | vs Manual |
|-----------|-----:|----------:|
| Manual | 28,564 ns | baseline |
| `MapBatchInPlace` | 30,916 ns | +8.2% |
| `MapBatch` | 31,656 ns | +10.8% |

### Async (N=10 / N=1 000)

| Method | N=10 | N=1 000 |
|--------|-----:|--------:|
| ManualSequential | 316 ns | 30,926 ns |
| `MapAsync_Sequential` | **1,145 ns** | 39,572 ns |
| ManualAsync_TaskRun | 1,182 ns | 37,037 ns |
| `MapParallelAsync` | 3,378 ns | 81,350 ns |

`MapAsync` at N=10 is **~3% faster** than an equivalent manual `Task.Run` wrapper.

### Cold Path — First Call

| Mapper | Mean | Ratio |
|--------|-----:|------:|
| **PropertyMapper** | **278 μs** | 1× |
| AutoMapper | 927 μs | 3.3× slower |
| Mapster | 2,028 μs | 7.3× slower |

Allocations at first call: PropertyMapper **11 KB**, AutoMapper **116 KB**, Mapster **924 KB**.

### Warmup (cold-path IL compilation)

| Method | Mean |
|--------|-----:|
| `Warmup` (1 pair) | 16.5 μs |
| `WarmupBatch` (2 pairs) | 27.2 μs |
| `WarmupBatch` (4 pairs) | 49.9 μs |

---

## Related

- [Getting Started — Warmup](../../docs/getting-started.md#warmup)
- [Async & Streaming](../../docs/async.md)
- [Collections](../../docs/collections.md)

