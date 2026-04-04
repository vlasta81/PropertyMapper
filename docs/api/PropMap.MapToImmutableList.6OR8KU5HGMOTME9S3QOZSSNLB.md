## PropMap\.MapToImmutableList\<TIn,TOut\>\(IEnumerable\<TIn\>\) Method

Maps all items in [source](PropMap.MapToImmutableList.6OR8KU5HGMOTME9S3QOZSSNLB.md#PropertyMapper.PropMap.MapToImmutableList_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).source 'PropertyMapper\.PropMap\.MapToImmutableList\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.source') to an [System\.Collections\.Immutable\.ImmutableList&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.immutable.immutablelist-1 'System\.Collections\.Immutable\.ImmutableList\`1')\.
Uses [System\.Collections\.Immutable\.ImmutableList&lt;&gt;\.Builder](https://learn.microsoft.com/en-us/dotnet/api/system.collections.immutable.immutablelist-1.builder 'System\.Collections\.Immutable\.ImmutableList\`1\.Builder') to accumulate mapped elements before
calling `ToImmutable()` once, minimising intermediate allocations\.

```csharp
public System.Collections.Immutable.ImmutableList<TOut> MapToImmutableList<TIn,TOut>(System.Collections.Generic.IEnumerable<TIn> source)
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.MapToImmutableList_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).TIn'></a>

`TIn`

Source element type\.

<a name='PropertyMapper.PropMap.MapToImmutableList_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).TOut'></a>

`TOut`

Target element type \(must have a parameterless constructor\)\.
#### Parameters

<a name='PropertyMapper.PropMap.MapToImmutableList_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).source'></a>

`source` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[TIn](PropMap.MapToImmutableList.6OR8KU5HGMOTME9S3QOZSSNLB.md#PropertyMapper.PropMap.MapToImmutableList_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).TIn 'PropertyMapper\.PropMap\.MapToImmutableList\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

Source sequence to map\.

#### Returns
[System\.Collections\.Immutable\.ImmutableList&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.immutable.immutablelist-1 'System\.Collections\.Immutable\.ImmutableList\`1')[TOut](PropMap.MapToImmutableList.6OR8KU5HGMOTME9S3QOZSSNLB.md#PropertyMapper.PropMap.MapToImmutableList_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).TOut 'PropertyMapper\.PropMap\.MapToImmutableList\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.immutable.immutablelist-1 'System\.Collections\.Immutable\.ImmutableList\`1')  
An [System\.Collections\.Immutable\.ImmutableList&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.immutable.immutablelist-1 'System\.Collections\.Immutable\.ImmutableList\`1') containing all mapped elements in source order\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [source](PropMap.MapToImmutableList.6OR8KU5HGMOTME9S3QOZSSNLB.md#PropertyMapper.PropMap.MapToImmutableList_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).source 'PropertyMapper\.PropMap\.MapToImmutableList\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.source') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.