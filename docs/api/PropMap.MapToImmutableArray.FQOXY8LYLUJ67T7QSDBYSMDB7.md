## PropMap\.MapToImmutableArray\<TIn,TOut\>\(IEnumerable\<TIn\>\) Method

Maps [System\.Collections\.Generic\.IEnumerable&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1') to an [System\.Collections\.Immutable\.ImmutableArray&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.immutable.immutablearray-1 'System\.Collections\.Immutable\.ImmutableArray\`1')\.

```csharp
public System.Collections.Immutable.ImmutableArray<TOut> MapToImmutableArray<TIn,TOut>(System.Collections.Generic.IEnumerable<TIn> source)
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.MapToImmutableArray_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).TIn'></a>

`TIn`

Source element type\.

<a name='PropertyMapper.PropMap.MapToImmutableArray_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).TOut'></a>

`TOut`

Target element type \(must have a parameterless constructor\)\.
#### Parameters

<a name='PropertyMapper.PropMap.MapToImmutableArray_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).source'></a>

`source` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[TIn](PropMap.MapToImmutableArray.FQOXY8LYLUJ67T7QSDBYSMDB7.md#PropertyMapper.PropMap.MapToImmutableArray_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).TIn 'PropertyMapper\.PropMap\.MapToImmutableArray\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

Source sequence to map\.

#### Returns
[System\.Collections\.Immutable\.ImmutableArray&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.immutable.immutablearray-1 'System\.Collections\.Immutable\.ImmutableArray\`1')[TOut](PropMap.MapToImmutableArray.FQOXY8LYLUJ67T7QSDBYSMDB7.md#PropertyMapper.PropMap.MapToImmutableArray_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).TOut 'PropertyMapper\.PropMap\.MapToImmutableArray\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.immutable.immutablearray-1 'System\.Collections\.Immutable\.ImmutableArray\`1')  
A new [System\.Collections\.Immutable\.ImmutableArray&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.immutable.immutablearray-1 'System\.Collections\.Immutable\.ImmutableArray\`1') containing the mapped elements\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [source](PropMap.MapToImmutableArray.FQOXY8LYLUJ67T7QSDBYSMDB7.md#PropertyMapper.PropMap.MapToImmutableArray_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).source 'PropertyMapper\.PropMap\.MapToImmutableArray\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.source') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

### Remarks
Fast paths \(in priority order\):
1. `List<TIn>` — `CollectionsMarshal.AsSpan`; builder pre-allocated with exact capacity (`MoveToImmutable`, zero-copy).
2. `TIn[]` — indexed loop; builder pre-allocated.
3. `ICollection<TIn>` — builder pre-allocated from `Count` (`MoveToImmutable`).
4. General `IEnumerable<TIn>` — falls back to `ToImmutable`.