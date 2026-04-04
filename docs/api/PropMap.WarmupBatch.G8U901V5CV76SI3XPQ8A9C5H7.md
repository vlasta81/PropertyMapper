## PropMap\.WarmupBatch\(Type\[\]\) Method

Pre\-compiles mapping delegates for multiple type pairs concurrently\.
Useful at application startup to eliminate first\-call overhead for all known type pairs at once\.

```csharp
public void WarmupBatch(params System.Type[] typePairs);
```
#### Parameters

<a name='PropertyMapper.PropMap.WarmupBatch(System.Type[]).typePairs'></a>

`typePairs` [System\.Type](https://learn.microsoft.com/en-us/dotnet/api/system.type 'System\.Type')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

An even\-length array of types in alternating source/target order\.
Example: `typeof(Foo), typeof(FooDto), typeof(Bar), typeof(BarDto)`\.

#### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
Thrown when [typePairs](PropMap.WarmupBatch.G8U901V5CV76SI3XPQ8A9C5H7.md#PropertyMapper.PropMap.WarmupBatch(System.Type[]).typePairs 'PropertyMapper\.PropMap\.WarmupBatch\(System\.Type\[\]\)\.typePairs') has an odd number of elements\.