## IPropMap\.MapNestedList\<TIn,TOut\>\(IEnumerable\<IEnumerable\<TIn\>\>\) Method

Maps nested collections \(sequence of sequences\) to a [System\.Collections\.Generic\.List&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1') of [System\.Collections\.Generic\.List&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')\.

```csharp
System.Collections.Generic.List<System.Collections.Generic.List<TOut>> MapNestedList<TIn,TOut>(System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<TIn>> source)
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.IPropMap.MapNestedList_TIn,TOut_(System.Collections.Generic.IEnumerable_System.Collections.Generic.IEnumerable_TIn__).TIn'></a>

`TIn`

<a name='PropertyMapper.IPropMap.MapNestedList_TIn,TOut_(System.Collections.Generic.IEnumerable_System.Collections.Generic.IEnumerable_TIn__).TOut'></a>

`TOut`
#### Parameters

<a name='PropertyMapper.IPropMap.MapNestedList_TIn,TOut_(System.Collections.Generic.IEnumerable_System.Collections.Generic.IEnumerable_TIn__).source'></a>

`source` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[TIn](IPropMap.MapNestedList.WOODYGK46R8V3HBZDEQZIAVV2.md#PropertyMapper.IPropMap.MapNestedList_TIn,TOut_(System.Collections.Generic.IEnumerable_System.Collections.Generic.IEnumerable_TIn__).TIn 'PropertyMapper\.IPropMap\.MapNestedList\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<System\.Collections\.Generic\.IEnumerable\<TIn\>\>\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

#### Returns
[System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[TOut](IPropMap.MapNestedList.WOODYGK46R8V3HBZDEQZIAVV2.md#PropertyMapper.IPropMap.MapNestedList_TIn,TOut_(System.Collections.Generic.IEnumerable_System.Collections.Generic.IEnumerable_TIn__).TOut 'PropertyMapper\.IPropMap\.MapNestedList\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<System\.Collections\.Generic\.IEnumerable\<TIn\>\>\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')