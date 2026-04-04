## PropMap\.MapNestedList\<TIn,TOut\>\(IEnumerable\<IEnumerable\<TIn\>\>\) Method

Maps a sequence of sequences \(e\.g\. a jagged array or list\-of\-lists\) to a
[System\.Collections\.Generic\.List&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1') of [System\.Collections\.Generic\.List&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')\.
Each inner sequence is mapped independently via
[MapToList&lt;TIn,TOut&gt;\(IEnumerable&lt;TIn&gt;\)](PropMap.MapToList.md#PropertyMapper.PropMap.MapToList_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_) 'PropertyMapper\.PropMap\.MapToList\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>\)')\.

```csharp
public System.Collections.Generic.List<System.Collections.Generic.List<TOut>> MapNestedList<TIn,TOut>(System.Collections.Generic.IEnumerable<System.Collections.Generic.IEnumerable<TIn>> source)
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.MapNestedList_TIn,TOut_(System.Collections.Generic.IEnumerable_System.Collections.Generic.IEnumerable_TIn__).TIn'></a>

`TIn`

Source element type\.

<a name='PropertyMapper.PropMap.MapNestedList_TIn,TOut_(System.Collections.Generic.IEnumerable_System.Collections.Generic.IEnumerable_TIn__).TOut'></a>

`TOut`

Target element type \(must have a parameterless constructor\)\.
#### Parameters

<a name='PropertyMapper.PropMap.MapNestedList_TIn,TOut_(System.Collections.Generic.IEnumerable_System.Collections.Generic.IEnumerable_TIn__).source'></a>

`source` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[TIn](PropMap.MapNestedList.E6BFIQ8CJN60XBVWYGFA82DSB.md#PropertyMapper.PropMap.MapNestedList_TIn,TOut_(System.Collections.Generic.IEnumerable_System.Collections.Generic.IEnumerable_TIn__).TIn 'PropertyMapper\.PropMap\.MapNestedList\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<System\.Collections\.Generic\.IEnumerable\<TIn\>\>\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

Outer sequence of inner source sequences to map\.

#### Returns
[System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[TOut](PropMap.MapNestedList.E6BFIQ8CJN60XBVWYGFA82DSB.md#PropertyMapper.PropMap.MapNestedList_TIn,TOut_(System.Collections.Generic.IEnumerable_System.Collections.Generic.IEnumerable_TIn__).TOut 'PropertyMapper\.PropMap\.MapNestedList\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<System\.Collections\.Generic\.IEnumerable\<TIn\>\>\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')  
A new [System\.Collections\.Generic\.List&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1') of [System\.Collections\.Generic\.List&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1');
outer count matches the number of inner sequences in [source](PropMap.MapNestedList.E6BFIQ8CJN60XBVWYGFA82DSB.md#PropertyMapper.PropMap.MapNestedList_TIn,TOut_(System.Collections.Generic.IEnumerable_System.Collections.Generic.IEnumerable_TIn__).source 'PropertyMapper\.PropMap\.MapNestedList\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<System\.Collections\.Generic\.IEnumerable\<TIn\>\>\)\.source')\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [source](PropMap.MapNestedList.E6BFIQ8CJN60XBVWYGFA82DSB.md#PropertyMapper.PropMap.MapNestedList_TIn,TOut_(System.Collections.Generic.IEnumerable_System.Collections.Generic.IEnumerable_TIn__).source 'PropertyMapper\.PropMap\.MapNestedList\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<System\.Collections\.Generic\.IEnumerable\<TIn\>\>\)\.source') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.