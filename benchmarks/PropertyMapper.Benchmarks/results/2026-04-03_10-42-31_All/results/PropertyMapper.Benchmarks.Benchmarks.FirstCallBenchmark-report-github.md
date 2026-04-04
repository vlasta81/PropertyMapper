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
| PropertyMapper | 1.386 ms |  3.362 ms |  5.032 ms | 0.4423 ms |  2.91 |   10.66 |  33.73 KB |        1.00 |
| AutoMapper     | 5.198 ms | 12.104 ms | 18.116 ms | 1.7827 ms | 10.92 |   38.39 | 248.49 KB |        7.37 |
| Mapster        | 6.042 ms | 10.344 ms | 15.483 ms | 3.1545 ms | 12.70 |   32.86 | 979.78 KB |       29.04 |
