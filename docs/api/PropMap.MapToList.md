#### [PropertyMapper](index.md 'index')
### [PropertyMapper](PropertyMapper.md 'PropertyMapper').[PropMap](PropMap.md 'PropertyMapper\.PropMap')

## PropMap\.MapToList Method

| Overloads | |
| :--- | :--- |
| [MapToList&lt;TIn,TOut&gt;\(IEnumerable&lt;TIn&gt;\)](PropMap.MapToList.md#PropertyMapper.PropMap.MapToList_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_) 'PropertyMapper\.PropMap\.MapToList\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)') | Maps [System\.Collections\.Generic\.IEnumerable&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1') to [System\.Collections\.Generic\.List&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')\. |
| [MapToList&lt;TIn,TOut&gt;\(ReadOnlySpan&lt;TIn&gt;\)](PropMap.MapToList.md#PropertyMapper.PropMap.MapToList_TIn,TOut_(System.ReadOnlySpan_TIn_) 'PropertyMapper\.PropMap\.MapToList\<TIn,TOut\>\(System\.ReadOnlySpan\<TIn\>\)') | Maps a [System\.ReadOnlySpan&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1') directly to [System\.Collections\.Generic\.List&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')\. |

<a name='PropertyMapper.PropMap.MapToList_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_)'></a>

## PropMap\.MapToList\<TIn,TOut\>\(IEnumerable\<TIn\>\) Method

Maps [System\.Collections\.Generic\.IEnumerable&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1') to [System\.Collections\.Generic\.List&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')\.

```csharp
public System.Collections.Generic.List<TOut> MapToList<TIn,TOut>(System.Collections.Generic.IEnumerable<TIn> source)
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.MapToList_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).TIn'></a>

`TIn`

Source element type\.

<a name='PropertyMapper.PropMap.MapToList_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).TOut'></a>

`TOut`

Target element type\.
#### Parameters

<a name='PropertyMapper.PropMap.MapToList_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).source'></a>

`source` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[TIn](PropMap.md#PropertyMapper.PropMap.MapToList_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).TIn 'PropertyMapper\.PropMap\.MapToList\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

Source sequence to map\.

#### Returns
[System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[TOut](PropMap.md#PropertyMapper.PropMap.MapToList_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).TOut 'PropertyMapper\.PropMap\.MapToList\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')  
A new [System\.Collections\.Generic\.List&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1') containing the mapped elements\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [source](PropMap.md#PropertyMapper.PropMap.MapToList_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).source 'PropertyMapper\.PropMap\.MapToList\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.source') is null\.

### Remarks
Optimised fast paths \(in priority order\):
1. `List<TIn>` — `CollectionsMarshal.AsSpan` exposes the internal backing
                  array directly; `CollectionsMarshal.SetCount` pre-sizes the output list so the loop
                  writes via a span index — no `Add` overhead.
2. `TIn[]` — same `SetCount + AsSpan` strategy as the List path; single allocation.
3. `ICollection<TIn>` — output list pre-sized from `Count`; span-indexed write.
4. General `IEnumerable<TIn>` — standard foreach / `Add` fallback (length unknown).

<a name='PropertyMapper.PropMap.MapToList_TIn,TOut_(System.ReadOnlySpan_TIn_)'></a>

## PropMap\.MapToList\<TIn,TOut\>\(ReadOnlySpan\<TIn\>\) Method

Maps a [System\.ReadOnlySpan&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1') directly to [System\.Collections\.Generic\.List&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')\.

```csharp
public System.Collections.Generic.List<TOut> MapToList<TIn,TOut>(System.ReadOnlySpan<TIn> source)
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.MapToList_TIn,TOut_(System.ReadOnlySpan_TIn_).TIn'></a>

`TIn`

Source element type\.

<a name='PropertyMapper.PropMap.MapToList_TIn,TOut_(System.ReadOnlySpan_TIn_).TOut'></a>

`TOut`

Target element type\.
#### Parameters

<a name='PropertyMapper.PropMap.MapToList_TIn,TOut_(System.ReadOnlySpan_TIn_).source'></a>

`source` [System\.ReadOnlySpan&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1')[TIn](PropMap.md#PropertyMapper.PropMap.MapToList_TIn,TOut_(System.ReadOnlySpan_TIn_).TIn 'PropertyMapper\.PropMap\.MapToList\<TIn,TOut\>\(System\.ReadOnlySpan\<TIn\>\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1')

Source span to map\.

#### Returns
[System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[TOut](PropMap.md#PropertyMapper.PropMap.MapToList_TIn,TOut_(System.ReadOnlySpan_TIn_).TOut 'PropertyMapper\.PropMap\.MapToList\<TIn,TOut\>\(System\.ReadOnlySpan\<TIn\>\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')  
A new [System\.Collections\.Generic\.List&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1') containing the mapped elements\.

### Remarks
Equivalent throughput to [MapBatch&lt;TIn,TOut&gt;\(ReadOnlySpan&lt;TIn&gt;\)](PropMap.MapBatch.AAPNY1A78JL9AD0VK3C7TBLPA.md 'PropertyMapper\.PropMap\.MapBatch\<TIn,TOut\>\(System\.ReadOnlySpan\<TIn\>\)') but returns a
[System\.Collections\.Generic\.List&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1') instead of an array\.
Uses `CollectionsMarshal.SetCount` to pre\-size the output list and
`CollectionsMarshal.AsSpan` to write elements directly into the backing buffer —
no `Add` overhead, single allocation\.

---
Generated by [DefaultDocumentation](https://github.com/Doraku/DefaultDocumentation 'https://github\.com/Doraku/DefaultDocumentation')