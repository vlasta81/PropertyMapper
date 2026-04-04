## PropMap\.MapToArray\<TIn,TOut\>\(IEnumerable\<TIn\>\) Method

Maps [System\.Collections\.Generic\.IEnumerable&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1') to `TOut[]`\.

```csharp
public TOut[] MapToArray<TIn,TOut>(System.Collections.Generic.IEnumerable<TIn> source)
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.MapToArray_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).TIn'></a>

`TIn`

Source element type\.

<a name='PropertyMapper.PropMap.MapToArray_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).TOut'></a>

`TOut`

Target element type\.
#### Parameters

<a name='PropertyMapper.PropMap.MapToArray_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).source'></a>

`source` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[TIn](PropMap.MapToArray.7HWNWPH5PR9SJS1O2ZGLJIQ38.md#PropertyMapper.PropMap.MapToArray_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).TIn 'PropertyMapper\.PropMap\.MapToArray\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

Source sequence to map\.

#### Returns
[TOut](PropMap.MapToArray.7HWNWPH5PR9SJS1O2ZGLJIQ38.md#PropertyMapper.PropMap.MapToArray_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).TOut 'PropertyMapper\.PropMap\.MapToArray\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.TOut')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')  
A new `TOut[]` containing the mapped elements\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [source](PropMap.MapToArray.7HWNWPH5PR9SJS1O2ZGLJIQ38.md#PropertyMapper.PropMap.MapToArray_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).source 'PropertyMapper\.PropMap\.MapToArray\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.source') is null\.

### Remarks
Fast paths \(in priority order\):
1. `TIn[]` — passed directly to [MapBatch&lt;TIn,TOut&gt;\(ReadOnlySpan&lt;TIn&gt;\)](PropMap.MapBatch.AAPNY1A78JL9AD0VK3C7TBLPA.md 'PropertyMapper\.PropMap\.MapBatch\<TIn,TOut\>\(System\.ReadOnlySpan\<TIn\>\)') as a span; single allocation.
2. `List<TIn>` — internal backing array exposed via `CollectionsMarshal.AsSpan`;
                  single allocation, no enumerator.
3. `ICollection<TIn>` — output array pre-allocated from `Count` and filled
                  in a single pass; avoids the intermediate `List<TOut>` + `ToArray()`
                  double-allocation that the general fallback would cause.
4. General `IEnumerable<TIn>` — falls back through [MapToList&lt;TIn,TOut&gt;\(IEnumerable&lt;TIn&gt;\)](PropMap.MapToList.md#PropertyMapper.PropMap.MapToList_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_) 'PropertyMapper\.PropMap\.MapToList\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)').