## IPropMap\.MapStreamBatchedAsync\<TIn,TOut\>\(IAsyncEnumerable\<TIn\>, int, CancellationToken\) Method

Streams mapped results in batches from an async source sequence\.

```csharp
System.Collections.Generic.IAsyncEnumerable<System.Collections.Generic.List<TOut>> MapStreamBatchedAsync<TIn,TOut>(System.Collections.Generic.IAsyncEnumerable<TIn> source, int batchSize=100, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken))
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.IPropMap.MapStreamBatchedAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,int,System.Threading.CancellationToken).TIn'></a>

`TIn`

<a name='PropertyMapper.IPropMap.MapStreamBatchedAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,int,System.Threading.CancellationToken).TOut'></a>

`TOut`
#### Parameters

<a name='PropertyMapper.IPropMap.MapStreamBatchedAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,int,System.Threading.CancellationToken).source'></a>

`source` [System\.Collections\.Generic\.IAsyncEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1 'System\.Collections\.Generic\.IAsyncEnumerable\`1')[TIn](IPropMap.MapStreamBatchedAsync.7EIMDTPPGA30NU3Y8APH8BQO2.md#PropertyMapper.IPropMap.MapStreamBatchedAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,int,System.Threading.CancellationToken).TIn 'PropertyMapper\.IPropMap\.MapStreamBatchedAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IAsyncEnumerable\<TIn\>, int, System\.Threading\.CancellationToken\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1 'System\.Collections\.Generic\.IAsyncEnumerable\`1')

<a name='PropertyMapper.IPropMap.MapStreamBatchedAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,int,System.Threading.CancellationToken).batchSize'></a>

`batchSize` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='PropertyMapper.IPropMap.MapStreamBatchedAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,int,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

#### Returns
[System\.Collections\.Generic\.IAsyncEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1 'System\.Collections\.Generic\.IAsyncEnumerable\`1')[System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[TOut](IPropMap.MapStreamBatchedAsync.7EIMDTPPGA30NU3Y8APH8BQO2.md#PropertyMapper.IPropMap.MapStreamBatchedAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,int,System.Threading.CancellationToken).TOut 'PropertyMapper\.IPropMap\.MapStreamBatchedAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IAsyncEnumerable\<TIn\>, int, System\.Threading\.CancellationToken\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1 'System\.Collections\.Generic\.IAsyncEnumerable\`1')