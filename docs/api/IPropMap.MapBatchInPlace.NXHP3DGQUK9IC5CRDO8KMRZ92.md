## IPropMap\.MapBatchInPlace\<TIn,TOut\>\(ReadOnlySpan\<TIn\>, Span\<TOut\>\) Method

Maps all items from [source](IPropMap.MapBatchInPlace.NXHP3DGQUK9IC5CRDO8KMRZ92.md#PropertyMapper.IPropMap.MapBatchInPlace_TIn,TOut_(System.ReadOnlySpan_TIn_,System.Span_TOut_).source 'PropertyMapper\.IPropMap\.MapBatchInPlace\<TIn,TOut\>\(System\.ReadOnlySpan\<TIn\>, System\.Span\<TOut\>\)\.source') directly into the pre\-allocated [destination](IPropMap.MapBatchInPlace.NXHP3DGQUK9IC5CRDO8KMRZ92.md#PropertyMapper.IPropMap.MapBatchInPlace_TIn,TOut_(System.ReadOnlySpan_TIn_,System.Span_TOut_).destination 'PropertyMapper\.IPropMap\.MapBatchInPlace\<TIn,TOut\>\(System\.ReadOnlySpan\<TIn\>, System\.Span\<TOut\>\)\.destination') span\.

```csharp
void MapBatchInPlace<TIn,TOut>(System.ReadOnlySpan<TIn> source, System.Span<TOut> destination)
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.IPropMap.MapBatchInPlace_TIn,TOut_(System.ReadOnlySpan_TIn_,System.Span_TOut_).TIn'></a>

`TIn`

<a name='PropertyMapper.IPropMap.MapBatchInPlace_TIn,TOut_(System.ReadOnlySpan_TIn_,System.Span_TOut_).TOut'></a>

`TOut`
#### Parameters

<a name='PropertyMapper.IPropMap.MapBatchInPlace_TIn,TOut_(System.ReadOnlySpan_TIn_,System.Span_TOut_).source'></a>

`source` [System\.ReadOnlySpan&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1')[TIn](IPropMap.MapBatchInPlace.NXHP3DGQUK9IC5CRDO8KMRZ92.md#PropertyMapper.IPropMap.MapBatchInPlace_TIn,TOut_(System.ReadOnlySpan_TIn_,System.Span_TOut_).TIn 'PropertyMapper\.IPropMap\.MapBatchInPlace\<TIn,TOut\>\(System\.ReadOnlySpan\<TIn\>, System\.Span\<TOut\>\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1')

<a name='PropertyMapper.IPropMap.MapBatchInPlace_TIn,TOut_(System.ReadOnlySpan_TIn_,System.Span_TOut_).destination'></a>

`destination` [System\.Span&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1 'System\.Span\`1')[TOut](IPropMap.MapBatchInPlace.NXHP3DGQUK9IC5CRDO8KMRZ92.md#PropertyMapper.IPropMap.MapBatchInPlace_TIn,TOut_(System.ReadOnlySpan_TIn_,System.Span_TOut_).TOut 'PropertyMapper\.IPropMap\.MapBatchInPlace\<TIn,TOut\>\(System\.ReadOnlySpan\<TIn\>, System\.Span\<TOut\>\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.span-1 'System\.Span\`1')