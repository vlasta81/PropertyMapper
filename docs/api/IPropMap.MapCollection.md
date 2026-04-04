#### [PropertyMapper](index.md 'index')
### [PropertyMapper](PropertyMapper.md 'PropertyMapper').[IPropMap](IPropMap.md 'PropertyMapper\.IPropMap')

## IPropMap\.MapCollection Method

| Overloads | |
| :--- | :--- |
| [MapCollection&lt;TIn,TOut,TOutCollection&gt;\(IEnumerable&lt;TIn&gt;\)](IPropMap.MapCollection.md#PropertyMapper.IPropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_) 'PropertyMapper\.IPropMap\.MapCollection\<TIn,TOut,TOutCollection\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)') | Maps [source](IPropMap.md#PropertyMapper.IPropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_).source 'PropertyMapper\.IPropMap\.MapCollection\<TIn,TOut,TOutCollection\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.source') into a new [TOutCollection](IPropMap.md#PropertyMapper.IPropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_).TOutCollection 'PropertyMapper\.IPropMap\.MapCollection\<TIn,TOut,TOutCollection\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.TOutCollection')\. |
| [MapCollection&lt;TIn,TOut&gt;\(IEnumerable&lt;TIn&gt;, ICollection&lt;TOut&gt;\)](IPropMap.MapCollection.md#PropertyMapper.IPropMap.MapCollection_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Collections.Generic.ICollection_TOut_) 'PropertyMapper\.IPropMap\.MapCollection\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, System\.Collections\.Generic\.ICollection\<TOut\>\)') | Maps [source](IPropMap.md#PropertyMapper.IPropMap.MapCollection_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Collections.Generic.ICollection_TOut_).source 'PropertyMapper\.IPropMap\.MapCollection\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, System\.Collections\.Generic\.ICollection\<TOut\>\)\.source') and appends results into an existing [destination](IPropMap.md#PropertyMapper.IPropMap.MapCollection_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Collections.Generic.ICollection_TOut_).destination 'PropertyMapper\.IPropMap\.MapCollection\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, System\.Collections\.Generic\.ICollection\<TOut\>\)\.destination') collection\. |

<a name='PropertyMapper.IPropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_)'></a>

## IPropMap\.MapCollection\<TIn,TOut,TOutCollection\>\(IEnumerable\<TIn\>\) Method

Maps [source](IPropMap.md#PropertyMapper.IPropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_).source 'PropertyMapper\.IPropMap\.MapCollection\<TIn,TOut,TOutCollection\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.source') into a new [TOutCollection](IPropMap.md#PropertyMapper.IPropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_).TOutCollection 'PropertyMapper\.IPropMap\.MapCollection\<TIn,TOut,TOutCollection\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.TOutCollection')\.

```csharp
TOutCollection MapCollection<TIn,TOut,TOutCollection>(System.Collections.Generic.IEnumerable<TIn> source)
    where TOut : new()
    where TOutCollection : System.Collections.Generic.ICollection<TOut>, new();
```
#### Type parameters

<a name='PropertyMapper.IPropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_).TIn'></a>

`TIn`

<a name='PropertyMapper.IPropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_).TOut'></a>

`TOut`

<a name='PropertyMapper.IPropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_).TOutCollection'></a>

`TOutCollection`
#### Parameters

<a name='PropertyMapper.IPropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_).source'></a>

`source` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[TIn](IPropMap.md#PropertyMapper.IPropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_).TIn 'PropertyMapper\.IPropMap\.MapCollection\<TIn,TOut,TOutCollection\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

#### Returns
[TOutCollection](IPropMap.md#PropertyMapper.IPropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_).TOutCollection 'PropertyMapper\.IPropMap\.MapCollection\<TIn,TOut,TOutCollection\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.TOutCollection')

<a name='PropertyMapper.IPropMap.MapCollection_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Collections.Generic.ICollection_TOut_)'></a>

## IPropMap\.MapCollection\<TIn,TOut\>\(IEnumerable\<TIn\>, ICollection\<TOut\>\) Method

Maps [source](IPropMap.md#PropertyMapper.IPropMap.MapCollection_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Collections.Generic.ICollection_TOut_).source 'PropertyMapper\.IPropMap\.MapCollection\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, System\.Collections\.Generic\.ICollection\<TOut\>\)\.source') and appends results into an existing [destination](IPropMap.md#PropertyMapper.IPropMap.MapCollection_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Collections.Generic.ICollection_TOut_).destination 'PropertyMapper\.IPropMap\.MapCollection\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, System\.Collections\.Generic\.ICollection\<TOut\>\)\.destination') collection\.

```csharp
void MapCollection<TIn,TOut>(System.Collections.Generic.IEnumerable<TIn> source, System.Collections.Generic.ICollection<TOut> destination)
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.IPropMap.MapCollection_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Collections.Generic.ICollection_TOut_).TIn'></a>

`TIn`

<a name='PropertyMapper.IPropMap.MapCollection_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Collections.Generic.ICollection_TOut_).TOut'></a>

`TOut`
#### Parameters

<a name='PropertyMapper.IPropMap.MapCollection_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Collections.Generic.ICollection_TOut_).source'></a>

`source` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[TIn](IPropMap.md#PropertyMapper.IPropMap.MapCollection_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Collections.Generic.ICollection_TOut_).TIn 'PropertyMapper\.IPropMap\.MapCollection\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, System\.Collections\.Generic\.ICollection\<TOut\>\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='PropertyMapper.IPropMap.MapCollection_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Collections.Generic.ICollection_TOut_).destination'></a>

`destination` [System\.Collections\.Generic\.ICollection&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1 'System\.Collections\.Generic\.ICollection\`1')[TOut](IPropMap.md#PropertyMapper.IPropMap.MapCollection_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Collections.Generic.ICollection_TOut_).TOut 'PropertyMapper\.IPropMap\.MapCollection\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, System\.Collections\.Generic\.ICollection\<TOut\>\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1 'System\.Collections\.Generic\.ICollection\`1')

---
Generated by [DefaultDocumentation](https://github.com/Doraku/DefaultDocumentation 'https://github\.com/Doraku/DefaultDocumentation')