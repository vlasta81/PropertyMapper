## PropMap\.MapParallelAsync\<TIn,TOut\>\(IEnumerable\<TIn\>, int, CancellationToken\) Method

Maps a collection in parallel using `Parallel.ForAsync`\.
Uses a zero\-allocation struct worker \([PropertyMapper\.PropMap\.AsyncMappingWorker&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/propertymapper.propmap.asyncmappingworker-2 'PropertyMapper\.PropMap\.AsyncMappingWorker\`2')\) to eliminate
closure allocations on the hot path\.

```csharp
public System.Threading.Tasks.Task<System.Collections.Generic.List<TOut>> MapParallelAsync<TIn,TOut>(System.Collections.Generic.IEnumerable<TIn> source, int maxDegreeOfParallelism=-1, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken))
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.MapParallelAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,int,System.Threading.CancellationToken).TIn'></a>

`TIn`

Source element type\.

<a name='PropertyMapper.PropMap.MapParallelAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,int,System.Threading.CancellationToken).TOut'></a>

`TOut`

Target element type \(must have a parameterless constructor\)\.
#### Parameters

<a name='PropertyMapper.PropMap.MapParallelAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,int,System.Threading.CancellationToken).source'></a>

`source` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[TIn](PropMap.MapParallelAsync.YKYTUOOTNCKCMTRPDGAVEH3B1.md#PropertyMapper.PropMap.MapParallelAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,int,System.Threading.CancellationToken).TIn 'PropertyMapper\.PropMap\.MapParallelAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, int, System\.Threading\.CancellationToken\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The sequence of items to map\.

<a name='PropertyMapper.PropMap.MapParallelAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,int,System.Threading.CancellationToken).maxDegreeOfParallelism'></a>

`maxDegreeOfParallelism` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Maximum number of concurrent mapping operations\.
Pass `-1` \(default\) to let the runtime choose based on available processors\.

<a name='PropertyMapper.PropMap.MapParallelAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token that cancels the parallel loop\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[TOut](PropMap.MapParallelAsync.YKYTUOOTNCKCMTRPDGAVEH3B1.md#PropertyMapper.PropMap.MapParallelAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,int,System.Threading.CancellationToken).TOut 'PropertyMapper\.PropMap\.MapParallelAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, int, System\.Threading\.CancellationToken\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that resolves to a [System\.Collections\.Generic\.List&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1') of mapped [TOut](PropMap.MapParallelAsync.YKYTUOOTNCKCMTRPDGAVEH3B1.md#PropertyMapper.PropMap.MapParallelAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,int,System.Threading.CancellationToken).TOut 'PropertyMapper\.PropMap\.MapParallelAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, int, System\.Threading\.CancellationToken\)\.TOut') instances in source order\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [source](PropMap.MapParallelAsync.YKYTUOOTNCKCMTRPDGAVEH3B1.md#PropertyMapper.PropMap.MapParallelAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,int,System.Threading.CancellationToken).source 'PropertyMapper\.PropMap\.MapParallelAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, int, System\.Threading\.CancellationToken\)\.source') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.