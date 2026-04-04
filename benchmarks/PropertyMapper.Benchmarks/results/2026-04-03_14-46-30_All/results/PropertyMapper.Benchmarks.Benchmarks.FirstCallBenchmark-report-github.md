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
| PropertyMapper | 1.358 ms |  3.134 ms |  4.690 ms | 0.4395 ms |  2.76 |    9.78 |  33.91 KB |        1.00 |
| AutoMapper     | 5.192 ms | 12.146 ms | 18.179 ms | 1.7369 ms | 10.57 |   37.89 | 248.57 KB |        7.33 |
| Mapster        | 5.951 ms | 10.103 ms | 15.122 ms | 3.1313 ms | 12.11 |   31.61 |  979.7 KB |       28.89 |
