## PropMap\.MapToHashSet\<TIn,TOut\>\(IEnumerable\<TIn\>\) Method

Maps all items in [source](PropMap.MapToHashSet.ED9S80O5C0IXQ7YINOANZSG57.md#PropertyMapper.PropMap.MapToHashSet_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).source 'PropertyMapper\.PropMap\.MapToHashSet\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.source') to a new [System\.Collections\.Generic\.HashSet&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1 'System\.Collections\.Generic\.HashSet\`1')\.
Delegates to [MapCollection&lt;TIn,TOut,TOutCollection&gt;\(IEnumerable&lt;TIn&gt;\)](PropMap.MapCollection.md#PropertyMapper.PropMap.MapCollection_TIn,TOut,TOutCollection_(System.Collections.Generic.IEnumerable_TIn_) 'PropertyMapper\.PropMap\.MapCollection\<TIn,TOut,TOutCollection\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)');
insertion order and uniqueness semantics follow the default [System\.Collections\.Generic\.HashSet&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1 'System\.Collections\.Generic\.HashSet\`1') equality comparer\.

```csharp
public System.Collections.Generic.HashSet<TOut> MapToHashSet<TIn,TOut>(System.Collections.Generic.IEnumerable<TIn> source)
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.MapToHashSet_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).TIn'></a>

`TIn`

Source element type\.

<a name='PropertyMapper.PropMap.MapToHashSet_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).TOut'></a>

`TOut`

Target element type \(must have a parameterless constructor\)\.
#### Parameters

<a name='PropertyMapper.PropMap.MapToHashSet_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).source'></a>

`source` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[TIn](PropMap.MapToHashSet.ED9S80O5C0IXQ7YINOANZSG57.md#PropertyMapper.PropMap.MapToHashSet_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).TIn 'PropertyMapper\.PropMap\.MapToHashSet\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

Source sequence to map\.

#### Returns
[System\.Collections\.Generic\.HashSet&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1 'System\.Collections\.Generic\.HashSet\`1')[TOut](PropMap.MapToHashSet.ED9S80O5C0IXQ7YINOANZSG57.md#PropertyMapper.PropMap.MapToHashSet_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).TOut 'PropertyMapper\.PropMap\.MapToHashSet\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1 'System\.Collections\.Generic\.HashSet\`1')  
A new [System\.Collections\.Generic\.HashSet&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1 'System\.Collections\.Generic\.HashSet\`1') containing all mapped elements\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [source](PropMap.MapToHashSet.ED9S80O5C0IXQ7YINOANZSG57.md#PropertyMapper.PropMap.MapToHashSet_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_).source 'PropertyMapper\.PropMap\.MapToHashSet\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)\.source') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.