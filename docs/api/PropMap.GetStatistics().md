## PropMap\.GetStatistics\(\) Method

Returns a consistent point\-in\-time snapshot of this mapper's cache utilisation\.
Acquires the compile lock to ensure the three cache dictionaries are read atomically\.

```csharp
public PropertyMapper.MappingStatistics GetStatistics();
```

#### Returns
[MappingStatistics](MappingStatistics.md 'PropertyMapper\.MappingStatistics')  
A [MappingStatistics](MappingStatistics.md 'PropertyMapper\.MappingStatistics') value with counts of cached delegates, plans, and a memory estimate\.