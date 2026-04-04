## IPropMap\.MapBatch\<TIn,TOut\>\(ReadOnlySpan\<TIn\>\) Method

Maps all items in [sources](IPropMap.MapBatch.BQFZ297YMDXZOORD45EH6C2W2.md#PropertyMapper.IPropMap.MapBatch_TIn,TOut_(System.ReadOnlySpan_TIn_).sources 'PropertyMapper\.IPropMap\.MapBatch\<TIn,TOut\>\(System\.ReadOnlySpan\<TIn\>\)\.sources') and returns a new array\.

```csharp
TOut[] MapBatch<TIn,TOut>(System.ReadOnlySpan<TIn> sources)
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.IPropMap.MapBatch_TIn,TOut_(System.ReadOnlySpan_TIn_).TIn'></a>

`TIn`

<a name='PropertyMapper.IPropMap.MapBatch_TIn,TOut_(System.ReadOnlySpan_TIn_).TOut'></a>

`TOut`
#### Parameters

<a name='PropertyMapper.IPropMap.MapBatch_TIn,TOut_(System.ReadOnlySpan_TIn_).sources'></a>

`sources` [System\.ReadOnlySpan&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1')[TIn](IPropMap.MapBatch.BQFZ297YMDXZOORD45EH6C2W2.md#PropertyMapper.IPropMap.MapBatch_TIn,TOut_(System.ReadOnlySpan_TIn_).TIn 'PropertyMapper\.IPropMap\.MapBatch\<TIn,TOut\>\(System\.ReadOnlySpan\<TIn\>\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1')

#### Returns
[TOut](IPropMap.MapBatch.BQFZ297YMDXZOORD45EH6C2W2.md#PropertyMapper.IPropMap.MapBatch_TIn,TOut_(System.ReadOnlySpan_TIn_).TOut 'PropertyMapper\.IPropMap\.MapBatch\<TIn,TOut\>\(System\.ReadOnlySpan\<TIn\>\)\.TOut')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')