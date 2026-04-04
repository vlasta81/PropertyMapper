## PropMap\.MapBatch\<TIn,TOut\>\(ReadOnlySpan\<TIn\>\) Method

Batch mapping with buffer pooling for collection scenarios\.
Significantly faster than repeated Map\(\) calls\.

```csharp
public TOut[] MapBatch<TIn,TOut>(System.ReadOnlySpan<TIn> sources)
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.MapBatch_TIn,TOut_(System.ReadOnlySpan_TIn_).TIn'></a>

`TIn`

<a name='PropertyMapper.PropMap.MapBatch_TIn,TOut_(System.ReadOnlySpan_TIn_).TOut'></a>

`TOut`
#### Parameters

<a name='PropertyMapper.PropMap.MapBatch_TIn,TOut_(System.ReadOnlySpan_TIn_).sources'></a>

`sources` [System\.ReadOnlySpan&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1')[TIn](PropMap.MapBatch.AAPNY1A78JL9AD0VK3C7TBLPA.md#PropertyMapper.PropMap.MapBatch_TIn,TOut_(System.ReadOnlySpan_TIn_).TIn 'PropertyMapper\.PropMap\.MapBatch\<TIn,TOut\>\(System\.ReadOnlySpan\<TIn\>\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1')

#### Returns
[TOut](PropMap.MapBatch.AAPNY1A78JL9AD0VK3C7TBLPA.md#PropertyMapper.PropMap.MapBatch_TIn,TOut_(System.ReadOnlySpan_TIn_).TOut 'PropertyMapper\.PropMap\.MapBatch\<TIn,TOut\>\(System\.ReadOnlySpan\<TIn\>\)\.TOut')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')