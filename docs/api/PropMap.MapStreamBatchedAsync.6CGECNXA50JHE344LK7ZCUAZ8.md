## PropMap\.MapStreamBatchedAsync\<TIn,TOut\>\(IAsyncEnumerable\<TIn\>, int, CancellationToken\) Method

Streams mapped results in batches, yielding a [System\.Collections\.Generic\.List&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1') every time
[batchSize](PropMap.MapStreamBatchedAsync.6CGECNXA50JHE344LK7ZCUAZ8.md#PropertyMapper.PropMap.MapStreamBatchedAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,int,System.Threading.CancellationToken).batchSize 'PropertyMapper\.PropMap\.MapStreamBatchedAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IAsyncEnumerable\<TIn\>, int, System\.Threading\.CancellationToken\)\.batchSize') source items have been accumulated\.
More throughput\-efficient than [MapStreamAsync&lt;TIn,TOut&gt;\(IAsyncEnumerable&lt;TIn&gt;, CancellationToken\)](PropMap.MapStreamAsync.G8NVOBKEVAFQYVRA0UCY309RD.md 'PropertyMapper\.PropMap\.MapStreamAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IAsyncEnumerable\<TIn\>, System\.Threading\.CancellationToken\)') because the compiled
mapper delegate is retrieved once per batch rather than once per item\.

```csharp
public System.Collections.Generic.IAsyncEnumerable<System.Collections.Generic.List<TOut>> MapStreamBatchedAsync<TIn,TOut>(System.Collections.Generic.IAsyncEnumerable<TIn> source, int batchSize=100, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken))
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.MapStreamBatchedAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,int,System.Threading.CancellationToken).TIn'></a>

`TIn`

Source element type\.

<a name='PropertyMapper.PropMap.MapStreamBatchedAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,int,System.Threading.CancellationToken).TOut'></a>

`TOut`

Target element type \(must have a parameterless constructor\)\.
#### Parameters

<a name='PropertyMapper.PropMap.MapStreamBatchedAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,int,System.Threading.CancellationToken).source'></a>

`source` [System\.Collections\.Generic\.IAsyncEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1 'System\.Collections\.Generic\.IAsyncEnumerable\`1')[TIn](PropMap.MapStreamBatchedAsync.6CGECNXA50JHE344LK7ZCUAZ8.md#PropertyMapper.PropMap.MapStreamBatchedAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,int,System.Threading.CancellationToken).TIn 'PropertyMapper\.PropMap\.MapStreamBatchedAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IAsyncEnumerable\<TIn\>, int, System\.Threading\.CancellationToken\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1 'System\.Collections\.Generic\.IAsyncEnumerable\`1')

Async sequence of source items\.

<a name='PropertyMapper.PropMap.MapStreamBatchedAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,int,System.Threading.CancellationToken).batchSize'></a>

`batchSize` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Number of items per emitted batch\. Default: 100\.

<a name='PropertyMapper.PropMap.MapStreamBatchedAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token passed to the source enumerator to cancel iteration\.

#### Returns
[System\.Collections\.Generic\.IAsyncEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1 'System\.Collections\.Generic\.IAsyncEnumerable\`1')[System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[TOut](PropMap.MapStreamBatchedAsync.6CGECNXA50JHE344LK7ZCUAZ8.md#PropertyMapper.PropMap.MapStreamBatchedAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,int,System.Threading.CancellationToken).TOut 'PropertyMapper\.PropMap\.MapStreamBatchedAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IAsyncEnumerable\<TIn\>, int, System\.Threading\.CancellationToken\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1 'System\.Collections\.Generic\.IAsyncEnumerable\`1')  
An async sequence of [System\.Collections\.Generic\.List&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1') batches\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [source](PropMap.MapStreamBatchedAsync.6CGECNXA50JHE344LK7ZCUAZ8.md#PropertyMapper.PropMap.MapStreamBatchedAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,int,System.Threading.CancellationToken).source 'PropertyMapper\.PropMap\.MapStreamBatchedAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IAsyncEnumerable\<TIn\>, int, System\.Threading\.CancellationToken\)\.source') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.