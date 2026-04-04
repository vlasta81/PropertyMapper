```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8037/25H2/2025Update/HudsonValley2)
Intel Core i5-7600K CPU 3.80GHz (Kaby Lake), 1 CPU, 4 logical and 4 physical cores
.NET SDK 10.0.201
  [Host]            : .NET 10.0.5 (10.0.5, 10.0.526.15411), X64 RyuJIT x86-64-v3
  ColdStart .NET 10 : .NET 10.0.5 (10.0.5, 10.0.526.15411), X64 RyuJIT x86-64-v3

Job=ColdStart .NET 10  IterationCount=30  RunStrategy=ColdStart  
WarmupCount=0  

```
| Method         | Mean     | Error     | StdDev    | Median    | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------- |---------:|----------:|----------:|----------:|------:|--------:|----------:|------------:|
| PropertyMapper | 1.543 ms |  3.430 ms |  5.134 ms | 0.5466 ms |  2.58 |    8.76 |  33.73 KB |        1.00 |
| AutoMapper     | 5.427 ms | 12.387 ms | 18.540 ms | 1.9402 ms |  9.08 |   31.64 | 248.41 KB |        7.36 |
| Mapster        | 6.400 ms | 10.652 ms | 15.943 ms | 3.3220 ms | 10.70 |   27.28 | 979.94 KB |       29.05 |
