## IPropMap\.MapStreamAsync\<TIn,TOut\>\(IAsyncEnumerable\<TIn\>, CancellationToken\) Method

Streams mapped results one at a time from an async source sequence\.

```csharp
System.Collections.Generic.IAsyncEnumerable<TOut> MapStreamAsync<TIn,TOut>(System.Collections.Generic.IAsyncEnumerable<TIn> source, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken))
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.IPropMap.MapStreamAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,System.Threading.CancellationToken).TIn'></a>

`TIn`

<a name='PropertyMapper.IPropMap.MapStreamAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,System.Threading.CancellationToken).TOut'></a>

`TOut`
#### Parameters

<a name='PropertyMapper.IPropMap.MapStreamAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,System.Threading.CancellationToken).source'></a>

`source` [System\.Collections\.Generic\.IAsyncEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1 'System\.Collections\.Generic\.IAsyncEnumerable\`1')[TIn](IPropMap.MapStreamAsync.BGRGFBMTUJZ0RLX4Z1SIDAHV9.md#PropertyMapper.IPropMap.MapStreamAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,System.Threading.CancellationToken).TIn 'PropertyMapper\.IPropMap\.MapStreamAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IAsyncEnumerable\<TIn\>, System\.Threading\.CancellationToken\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1 'System\.Collections\.Generic\.IAsyncEnumerable\`1')

<a name='PropertyMapper.IPropMap.MapStreamAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

#### Returns
[System\.Collections\.Generic\.IAsyncEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1 'System\.Collections\.Generic\.IAsyncEnumerable\`1')[TOut](IPropMap.MapStreamAsync.BGRGFBMTUJZ0RLX4Z1SIDAHV9.md#PropertyMapper.IPropMap.MapStreamAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,System.Threading.CancellationToken).TOut 'PropertyMapper\.IPropMap\.MapStreamAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IAsyncEnumerable\<TIn\>, System\.Threading\.CancellationToken\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1 'System\.Collections\.Generic\.IAsyncEnumerable\`1')