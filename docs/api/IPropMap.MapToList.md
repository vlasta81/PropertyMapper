#### [PropertyMapper](index.md 'index')
### [PropertyMapper](PropertyMapper.md 'PropertyMapper').[IPropMap](IPropMap.md 'PropertyMapper\.IPropMap')

## IPropMap\.MapToList Method

| Overloads | |
| :--- | :--- |
| [MapToList&lt;TIn,TOut&gt;\(IEnumerable&lt;TIn&gt;\)](IPropMap.MapToList.md#PropertyMapper.IPropMap.MapToList_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_) 'PropertyMapper\.IPropMap\.MapToList\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)') | Maps [source](IPropMap.md#PropertyMapper.IPropMap.MapToList_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).source 'PropertyMapper\.IPropMap\.MapToList\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.source') to a [System\.Collections\.Generic\.List&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')\. |
| [MapToList&lt;TIn,TOut&gt;\(ReadOnlySpan&lt;TIn&gt;\)](IPropMap.MapToList.md#PropertyMapper.IPropMap.MapToList_TIn,TOut_(System.ReadOnlySpan_TIn_) 'PropertyMapper\.IPropMap\.MapToList\<TIn,TOut\>\(System\.ReadOnlySpan\<TIn\>\)') | Maps a [System\.ReadOnlySpan&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1') to a [System\.Collections\.Generic\.List&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')\. |

<a name='PropertyMapper.IPropMap.MapToList_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_)'></a>

## IPropMap\.MapToList\<TIn,TOut\>\(IEnumerable\<TIn\>\) Method

Maps [source](IPropMap.md#PropertyMapper.IPropMap.MapToList_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).source 'PropertyMapper\.IPropMap\.MapToList\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.source') to a [System\.Collections\.Generic\.List&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')\.

```csharp
System.Collections.Generic.List<TOut> MapToList<TIn,TOut>(System.Collections.Generic.IEnumerable<TIn> source)
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.IPropMap.MapToList_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).TIn'></a>

`TIn`

<a name='PropertyMapper.IPropMap.MapToList_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).TOut'></a>

`TOut`
#### Parameters

<a name='PropertyMapper.IPropMap.MapToList_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).source'></a>

`source` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[TIn](IPropMap.md#PropertyMapper.IPropMap.MapToList_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).TIn 'PropertyMapper\.IPropMap\.MapToList\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

#### Returns
[System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[TOut](IPropMap.md#PropertyMapper.IPropMap.MapToList_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).TOut 'PropertyMapper\.IPropMap\.MapToList\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')

<a name='PropertyMapper.IPropMap.MapToList_TIn,TOut_(System.ReadOnlySpan_TIn_)'></a>

## IPropMap\.MapToList\<TIn,TOut\>\(ReadOnlySpan\<TIn\>\) Method

Maps a [System\.ReadOnlySpan&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1') to a [System\.Collections\.Generic\.List&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')\.

```csharp
System.Collections.Generic.List<TOut> MapToList<TIn,TOut>(System.ReadOnlySpan<TIn> source)
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.IPropMap.MapToList_TIn,TOut_(System.ReadOnlySpan_TIn_).TIn'></a>

`TIn`

<a name='PropertyMapper.IPropMap.MapToList_TIn,TOut_(System.ReadOnlySpan_TIn_).TOut'></a>

`TOut`
#### Parameters

<a name='PropertyMapper.IPropMap.MapToList_TIn,TOut_(System.ReadOnlySpan_TIn_).source'></a>

`source` [System\.ReadOnlySpan&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1')[TIn](IPropMap.md#PropertyMapper.IPropMap.MapToList_TIn,TOut_(System.ReadOnlySpan_TIn_).TIn 'PropertyMapper\.IPropMap\.MapToList\<TIn,TOut\>\(System\.ReadOnlySpan\<TIn\>\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1')

#### Returns
[System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[TOut](IPropMap.md#PropertyMapper.IPropMap.MapToList_TIn,TOut_(System.ReadOnlySpan_TIn_).TOut 'PropertyMapper\.IPropMap\.MapToList\<TIn,TOut\>\(System\.ReadOnlySpan\<TIn\>\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')

---
Generated by [DefaultDocumentation](https://github.com/Doraku/DefaultDocumentation 'https://github\.com/Doraku/DefaultDocumentation')