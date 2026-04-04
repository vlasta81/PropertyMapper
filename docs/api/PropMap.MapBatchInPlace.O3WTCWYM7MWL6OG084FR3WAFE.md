## PropMap\.MapBatchInPlace\<TIn,TOut\>\(ReadOnlySpan\<TIn\>, Span\<TOut\>\) Method

Maps all items from [source](PropMap.MapBatchInPlace.O3WTCWYM7MWL6OG084FR3WAFE.md#PropertyMapper.PropMap.MapBatchInPlace_TIn,TOut_(System.ReadOnlySpan_TIn_,System.Span_TOut_).source 'PropertyMapper\.PropMap\.MapBatchInPlace\<TIn,TOut\>\(System\.ReadOnlySpan\<TIn\>, System\.Span\<TOut\>\)\.source') directly into a pre\-allocated
[destination](PropMap.MapBatchInPlace.O3WTCWYM7MWL6OG084FR3WAFE.md#PropertyMapper.PropMap.MapBatchInPlace_TIn,TOut_(System.ReadOnlySpan_TIn_,System.Span_TOut_).destination 'PropertyMapper\.PropMap\.MapBatchInPlace\<TIn,TOut\>\(System\.ReadOnlySpan\<TIn\>, System\.Span\<TOut\>\)\.destination') span\. Zero additional allocation on the hot path\.

```csharp
public void MapBatchInPlace<TIn,TOut>(System.ReadOnlySpan<TIn> source, System.Span<TOut> destination)
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.MapBatchInPlace_TIn,TOut_(System.ReadOnlySpan_TIn_,System.Span_TOut_).TIn'></a>

`TIn`

Source element type\.

<a name='PropertyMapper.PropMap.MapBatchInPlace_TIn,TOut_(System.ReadOnlySpan_TIn_,System.Span_TOut_).TOut'></a>

`TOut`

Target element type \(must have a parameterless constructor\)\.
#### Parameters

<a name='PropertyMapper.PropMap.MapBatchInPlace_TIn,TOut_(System.ReadOnlySpan_TIn_,System.Span_TOut_).source'></a>

`source` [System\.ReadOnlySpan&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1')[TIn](PropMap.MapBatchInPlace.O3WTCWYM7MWL6OG084FR3WAFE.md#PropertyMapper.PropMap.MapBatchInPlace_TIn,TOut_(System.ReadOnlySpan_TIn_,System.Span_TOut_).TIn 'PropertyMapper\.PropMap\.MapBatchInPlace\<TIn,TOut\>\(System\.ReadOnlySpan\<TIn\>, System\.Span\<TOut\>\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1')

Source span to map\.

<a name='PropertyMapper.PropMap.MapBatchInPlace_TIn,TOut_(System.ReadOnlySpan_TIn_,System.Span_TOut_).destination'></a>

`destination` [System\.Span&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1 'System\.Span\`1')[TOut](PropMap.MapBatchInPlace.O3WTCWYM7MWL6OG084FR3WAFE.md#PropertyMapper.PropMap.MapBatchInPlace_TIn,TOut_(System.ReadOnlySpan_TIn_,System.Span_TOut_).TOut 'PropertyMapper\.PropMap\.MapBatchInPlace\<TIn,TOut\>\(System\.ReadOnlySpan\<TIn\>, System\.Span\<TOut\>\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1 'System\.Span\`1')

Pre\-allocated output span; must be at least as long as [source](PropMap.MapBatchInPlace.O3WTCWYM7MWL6OG084FR3WAFE.md#PropertyMapper.PropMap.MapBatchInPlace_TIn,TOut_(System.ReadOnlySpan_TIn_,System.Span_TOut_).source 'PropertyMapper\.PropMap\.MapBatchInPlace\<TIn,TOut\>\(System\.ReadOnlySpan\<TIn\>, System\.Span\<TOut\>\)\.source')\.

#### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
When [destination](PropMap.MapBatchInPlace.O3WTCWYM7MWL6OG084FR3WAFE.md#PropertyMapper.PropMap.MapBatchInPlace_TIn,TOut_(System.ReadOnlySpan_TIn_,System.Span_TOut_).destination 'PropertyMapper\.PropMap\.MapBatchInPlace\<TIn,TOut\>\(System\.ReadOnlySpan\<TIn\>, System\.Span\<TOut\>\)\.destination') is shorter than [source](PropMap.MapBatchInPlace.O3WTCWYM7MWL6OG084FR3WAFE.md#PropertyMapper.PropMap.MapBatchInPlace_TIn,TOut_(System.ReadOnlySpan_TIn_,System.Span_TOut_).source 'PropertyMapper\.PropMap\.MapBatchInPlace\<TIn,TOut\>\(System\.ReadOnlySpan\<TIn\>, System\.Span\<TOut\>\)\.source')\.

### Remarks
Ideal for `stackalloc` and buffer\-pool scenarios where the caller owns the
output buffer\. Use [MapBatch&lt;TIn,TOut&gt;\(ReadOnlySpan&lt;TIn&gt;\)](PropMap.MapBatch.AAPNY1A78JL9AD0VK3C7TBLPA.md 'PropertyMapper\.PropMap\.MapBatch\<TIn,TOut\>\(System\.ReadOnlySpan\<TIn\>\)') when
the mapper should allocate the result array\.