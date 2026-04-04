## IPropMap\.MapParallelAsync\<TIn,TOut\>\(IEnumerable\<TIn\>, int, CancellationToken\) Method

Maps a collection in parallel using `Parallel.ForAsync`\.

```csharp
System.Threading.Tasks.Task<System.Collections.Generic.List<TOut>> MapParallelAsync<TIn,TOut>(System.Collections.Generic.IEnumerable<TIn> source, int maxDegreeOfParallelism=-1, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken))
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.IPropMap.MapParallelAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,int,System.Threading.CancellationToken).TIn'></a>

`TIn`

<a name='PropertyMapper.IPropMap.MapParallelAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,int,System.Threading.CancellationToken).TOut'></a>

`TOut`
#### Parameters

<a name='PropertyMapper.IPropMap.MapParallelAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,int,System.Threading.CancellationToken).source'></a>

`source` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[TIn](IPropMap.MapParallelAsync.DH6CGTKDT4RBJG69O8FHPIFSE.md#PropertyMapper.IPropMap.MapParallelAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,int,System.Threading.CancellationToken).TIn 'PropertyMapper\.IPropMap\.MapParallelAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, int, System\.Threading\.CancellationToken\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='PropertyMapper.IPropMap.MapParallelAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,int,System.Threading.CancellationToken).maxDegreeOfParallelism'></a>

`maxDegreeOfParallelism` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='PropertyMapper.IPropMap.MapParallelAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[TOut](IPropMap.MapParallelAsync.DH6CGTKDT4RBJG69O8FHPIFSE.md#PropertyMapper.IPropMap.MapParallelAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,int,System.Threading.CancellationToken).TOut 'PropertyMapper\.IPropMap\.MapParallelAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, int, System\.Threading\.CancellationToken\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')