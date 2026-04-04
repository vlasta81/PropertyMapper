## PropMap\.MapStreamAsync\<TIn,TOut\>\(IAsyncEnumerable\<TIn\>, CancellationToken\) Method

Streams mapped results asynchronously, yielding one [TOut](PropMap.MapStreamAsync.G8NVOBKEVAFQYVRA0UCY309RD.md#PropertyMapper.PropMap.MapStreamAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,System.Threading.CancellationToken).TOut 'PropertyMapper\.PropMap\.MapStreamAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IAsyncEnumerable\<TIn\>, System\.Threading\.CancellationToken\)\.TOut') at a time
as items arrive on the source [System\.Collections\.Generic\.IAsyncEnumerable&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1 'System\.Collections\.Generic\.IAsyncEnumerable\`1')\.
Memory\-efficient for large or infinite datasets because only one batch is in\-flight at a time\.
Mapping is synchronous CPU work and is called inline \(no extra thread\-pool dispatch per item\)\.

```csharp
public System.Collections.Generic.IAsyncEnumerable<TOut> MapStreamAsync<TIn,TOut>(System.Collections.Generic.IAsyncEnumerable<TIn> source, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken))
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.MapStreamAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,System.Threading.CancellationToken).TIn'></a>

`TIn`

Source element type\.

<a name='PropertyMapper.PropMap.MapStreamAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,System.Threading.CancellationToken).TOut'></a>

`TOut`

Target element type \(must have a parameterless constructor\)\.
#### Parameters

<a name='PropertyMapper.PropMap.MapStreamAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,System.Threading.CancellationToken).source'></a>

`source` [System\.Collections\.Generic\.IAsyncEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1 'System\.Collections\.Generic\.IAsyncEnumerable\`1')[TIn](PropMap.MapStreamAsync.G8NVOBKEVAFQYVRA0UCY309RD.md#PropertyMapper.PropMap.MapStreamAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,System.Threading.CancellationToken).TIn 'PropertyMapper\.PropMap\.MapStreamAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IAsyncEnumerable\<TIn\>, System\.Threading\.CancellationToken\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1 'System\.Collections\.Generic\.IAsyncEnumerable\`1')

Async sequence of source items\.

<a name='PropertyMapper.PropMap.MapStreamAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token passed to the source enumerator to cancel iteration\.

#### Returns
[System\.Collections\.Generic\.IAsyncEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1 'System\.Collections\.Generic\.IAsyncEnumerable\`1')[TOut](PropMap.MapStreamAsync.G8NVOBKEVAFQYVRA0UCY309RD.md#PropertyMapper.PropMap.MapStreamAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,System.Threading.CancellationToken).TOut 'PropertyMapper\.PropMap\.MapStreamAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IAsyncEnumerable\<TIn\>, System\.Threading\.CancellationToken\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1 'System\.Collections\.Generic\.IAsyncEnumerable\`1')  
An async sequence of mapped [TOut](PropMap.MapStreamAsync.G8NVOBKEVAFQYVRA0UCY309RD.md#PropertyMapper.PropMap.MapStreamAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,System.Threading.CancellationToken).TOut 'PropertyMapper\.PropMap\.MapStreamAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IAsyncEnumerable\<TIn\>, System\.Threading\.CancellationToken\)\.TOut') instances\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [source](PropMap.MapStreamAsync.G8NVOBKEVAFQYVRA0UCY309RD.md#PropertyMapper.PropMap.MapStreamAsync_TIn,TOut_(System.Collections.Generic.IAsyncEnumerable_TIn_,System.Threading.CancellationToken).source 'PropertyMapper\.PropMap\.MapStreamAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IAsyncEnumerable\<TIn\>, System\.Threading\.CancellationToken\)\.source') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.