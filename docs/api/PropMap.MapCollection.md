#### [PropertyMapper](index.md 'index')
### [PropertyMapper](PropertyMapper.md 'PropertyMapper').[PropMap](PropMap.md 'PropertyMapper\.PropMap')

## PropMap\.MapCollection Method

| Overloads | |
| :--- | :--- |
| [MapCollection&lt;TIn,TOut,TOutCollection&gt;\(IEnumerable&lt;TIn&gt;\)](PropMap.MapCollection.md#PropertyMapper.PropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_) 'PropertyMapper\.PropMap\.MapCollection\<TIn,TOut,TOutCollection\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)') | Maps all items in [source](PropMap.md#PropertyMapper.PropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_).source 'PropertyMapper\.PropMap\.MapCollection\<TIn,TOut,TOutCollection\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.source') using a compiled delegate and adds them to a new [TOutCollection](PropMap.md#PropertyMapper.PropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_).TOutCollection 'PropertyMapper\.PropMap\.MapCollection\<TIn,TOut,TOutCollection\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.TOutCollection') instance\. |
| [MapCollection&lt;TIn,TOut&gt;\(IEnumerable&lt;TIn&gt;, ICollection&lt;TOut&gt;\)](PropMap.MapCollection.md#PropertyMapper.PropMap.MapCollection_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Collections.Generic.ICollection_TOut_) 'PropertyMapper\.PropMap\.MapCollection\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, System\.Collections\.Generic\.ICollection\<TOut\>\)') | Maps items from [source](PropMap.md#PropertyMapper.PropMap.MapCollection_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Collections.Generic.ICollection_TOut_).source 'PropertyMapper\.PropMap\.MapCollection\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, System\.Collections\.Generic\.ICollection\<TOut\>\)\.source') and appends them into an existing [destination](PropMap.md#PropertyMapper.PropMap.MapCollection_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Collections.Generic.ICollection_TOut_).destination 'PropertyMapper\.PropMap\.MapCollection\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, System\.Collections\.Generic\.ICollection\<TOut\>\)\.destination') collection\. Unlike the three\-type\-parameter [MapCollection&lt;TIn,TOut,TOutCollection&gt;\(IEnumerable&lt;TIn&gt;\)](PropMap.MapCollection.md#PropertyMapper.PropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_) 'PropertyMapper\.PropMap\.MapCollection\<TIn,TOut,TOutCollection\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)') overload, the destination is caller\-supplied rather than created by the method, making it suitable for populating pre\-allocated or pre\-populated collections\. |

<a name='PropertyMapper.PropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_)'></a>

## PropMap\.MapCollection\<TIn,TOut,TOutCollection\>\(IEnumerable\<TIn\>\) Method

Maps all items in [source](PropMap.md#PropertyMapper.PropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_).source 'PropertyMapper\.PropMap\.MapCollection\<TIn,TOut,TOutCollection\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.source') using a compiled delegate and adds them to a new
[TOutCollection](PropMap.md#PropertyMapper.PropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_).TOutCollection 'PropertyMapper\.PropMap\.MapCollection\<TIn,TOut,TOutCollection\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.TOutCollection') instance\.

```csharp
public TOutCollection MapCollection<TIn,TOut,TOutCollection>(System.Collections.Generic.IEnumerable<TIn> source)
    where TOut : new()
    where TOutCollection : System.Collections.Generic.ICollection<TOut>, new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_).TIn'></a>

`TIn`

Source element type\.

<a name='PropertyMapper.PropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_).TOut'></a>

`TOut`

Target element type \(must have a parameterless constructor\)\.

<a name='PropertyMapper.PropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_).TOutCollection'></a>

`TOutCollection`

Target collection type; must implement [System\.Collections\.Generic\.ICollection&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1 'System\.Collections\.Generic\.ICollection\`1') and provide a
parameterless constructor\.
#### Parameters

<a name='PropertyMapper.PropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_).source'></a>

`source` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[TIn](PropMap.md#PropertyMapper.PropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_).TIn 'PropertyMapper\.PropMap\.MapCollection\<TIn,TOut,TOutCollection\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

Source sequence to map\.

#### Returns
[TOutCollection](PropMap.md#PropertyMapper.PropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_).TOutCollection 'PropertyMapper\.PropMap\.MapCollection\<TIn,TOut,TOutCollection\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.TOutCollection')  
A new [TOutCollection](PropMap.md#PropertyMapper.PropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_).TOutCollection 'PropertyMapper\.PropMap\.MapCollection\<TIn,TOut,TOutCollection\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.TOutCollection') containing all mapped elements\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [source](PropMap.md#PropertyMapper.PropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_).source 'PropertyMapper\.PropMap\.MapCollection\<TIn,TOut,TOutCollection\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.source') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

### Remarks
When the output collection is a [System\.Collections\.Generic\.List&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1') and the source is an array, the
backing buffer is written directly via `CollectionsMarshal.AsSpan` to avoid
per\-element `Add` overhead\. For all other collection types the standard `Add`
loop is used\.

<a name='PropertyMapper.PropMap.MapCollection_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Collections.Generic.ICollection_TOut_)'></a>

## PropMap\.MapCollection\<TIn,TOut\>\(IEnumerable\<TIn\>, ICollection\<TOut\>\) Method

Maps items from [source](PropMap.md#PropertyMapper.PropMap.MapCollection_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Collections.Generic.ICollection_TOut_).source 'PropertyMapper\.PropMap\.MapCollection\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, System\.Collections\.Generic\.ICollection\<TOut\>\)\.source') and appends them into an existing
[destination](PropMap.md#PropertyMapper.PropMap.MapCollection_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Collections.Generic.ICollection_TOut_).destination 'PropertyMapper\.PropMap\.MapCollection\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, System\.Collections\.Generic\.ICollection\<TOut\>\)\.destination') collection\. Unlike the three\-type\-parameter
[MapCollection&lt;TIn,TOut,TOutCollection&gt;\(IEnumerable&lt;TIn&gt;\)](PropMap.MapCollection.md#PropertyMapper.PropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_) 'PropertyMapper\.PropMap\.MapCollection\<TIn,TOut,TOutCollection\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)') overload, the destination is
caller\-supplied rather than created by the method, making it suitable for
populating pre\-allocated or pre\-populated collections\.

```csharp
public void MapCollection<TIn,TOut>(System.Collections.Generic.IEnumerable<TIn> source, System.Collections.Generic.ICollection<TOut> destination)
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.MapCollection_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Collections.Generic.ICollection_TOut_).TIn'></a>

`TIn`

Source element type\.

<a name='PropertyMapper.PropMap.MapCollection_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Collections.Generic.ICollection_TOut_).TOut'></a>

`TOut`

Target element type \(must have a parameterless constructor\)\.
#### Parameters

<a name='PropertyMapper.PropMap.MapCollection_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Collections.Generic.ICollection_TOut_).source'></a>

`source` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[TIn](PropMap.md#PropertyMapper.PropMap.MapCollection_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Collections.Generic.ICollection_TOut_).TIn 'PropertyMapper\.PropMap\.MapCollection\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, System\.Collections\.Generic\.ICollection\<TOut\>\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

Source sequence to map\.

<a name='PropertyMapper.PropMap.MapCollection_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Collections.Generic.ICollection_TOut_).destination'></a>

`destination` [System\.Collections\.Generic\.ICollection&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1 'System\.Collections\.Generic\.ICollection\`1')[TOut](PropMap.md#PropertyMapper.PropMap.MapCollection_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Collections.Generic.ICollection_TOut_).TOut 'PropertyMapper\.PropMap\.MapCollection\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, System\.Collections\.Generic\.ICollection\<TOut\>\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.icollection-1 'System\.Collections\.Generic\.ICollection\`1')

Existing collection to populate with mapped elements\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [source](PropMap.md#PropertyMapper.PropMap.MapCollection_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Collections.Generic.ICollection_TOut_).source 'PropertyMapper\.PropMap\.MapCollection\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, System\.Collections\.Generic\.ICollection\<TOut\>\)\.source') or [destination](PropMap.md#PropertyMapper.PropMap.MapCollection_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Collections.Generic.ICollection_TOut_).destination 'PropertyMapper\.PropMap\.MapCollection\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, System\.Collections\.Generic\.ICollection\<TOut\>\)\.destination') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

---
Generated by [DefaultDocumentation](https://github.com/Doraku/DefaultDocumentation 'https://github\.com/Doraku/DefaultDocumentation')