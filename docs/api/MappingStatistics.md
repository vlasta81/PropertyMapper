## MappingStatistics Struct

Snapshot of runtime statistics for a [PropMap](PropMap.md 'PropertyMapper\.PropMap') instance\.

```csharp
public readonly record struct MappingStatistics : System.IEquatable<PropertyMapper.MappingStatistics>
```

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[MappingStatistics](MappingStatistics.md 'PropertyMapper\.MappingStatistics')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Constructors | |
| :--- | :--- |
| [MappingStatistics\(int, int, long\)](MappingStatistics..ctor.UA3BVD3EV7TW086XRNOMWOKJ4.md 'PropertyMapper\.MappingStatistics\.MappingStatistics\(int, int, long\)') | Snapshot of runtime statistics for a [PropMap](PropMap.md 'PropertyMapper\.PropMap') instance\. |

| Properties | |
| :--- | :--- |
| [CachedMappers](MappingStatistics.CachedMappers.md 'PropertyMapper\.MappingStatistics\.CachedMappers') | Number of compiled mapping delegates currently in the cache\. |
| [CachedPlans](MappingStatistics.CachedPlans.md 'PropertyMapper\.MappingStatistics\.CachedPlans') | Number of mapping plans currently in the cache\. |
| [TotalMemoryBytes](MappingStatistics.TotalMemoryBytes.md 'PropertyMapper\.MappingStatistics\.TotalMemoryBytes') | Rough estimate of memory used by cached plans and delegates, in bytes\. |
