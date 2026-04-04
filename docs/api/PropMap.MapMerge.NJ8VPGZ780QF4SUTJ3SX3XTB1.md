## PropMap\.MapMerge\<TIn1,TIn2,TOut\>\(TIn1, TIn2\) Method

Creates a new [TOut](PropMap.MapMerge.NJ8VPGZ780QF4SUTJ3SX3XTB1.md#PropertyMapper.PropMap.MapMerge_TIn1,TIn2,TOut_(TIn1,TIn2).TOut 'PropertyMapper\.PropMap\.MapMerge\<TIn1,TIn2,TOut\>\(TIn1, TIn2\)\.TOut') from [source1](PropMap.MapMerge.NJ8VPGZ780QF4SUTJ3SX3XTB1.md#PropertyMapper.PropMap.MapMerge_TIn1,TIn2,TOut_(TIn1,TIn2).source1 'PropertyMapper\.PropMap\.MapMerge\<TIn1,TIn2,TOut\>\(TIn1, TIn2\)\.source1'), then merges matching
properties from [source2](PropMap.MapMerge.NJ8VPGZ780QF4SUTJ3SX3XTB1.md#PropertyMapper.PropMap.MapMerge_TIn1,TIn2,TOut_(TIn1,TIn2).source2 'PropertyMapper\.PropMap\.MapMerge\<TIn1,TIn2,TOut\>\(TIn1, TIn2\)\.source2') onto it\.
When both sources expose a property with the same name, [source2](PropMap.MapMerge.NJ8VPGZ780QF4SUTJ3SX3XTB1.md#PropertyMapper.PropMap.MapMerge_TIn1,TIn2,TOut_(TIn1,TIn2).source2 'PropertyMapper\.PropMap\.MapMerge\<TIn1,TIn2,TOut\>\(TIn1, TIn2\)\.source2') wins\.

```csharp
public TOut MapMerge<TIn1,TIn2,TOut>(TIn1 source1, TIn2 source2)
    where TOut : class, new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.MapMerge_TIn1,TIn2,TOut_(TIn1,TIn2).TIn1'></a>

`TIn1`

First \(base\) source type\.

<a name='PropertyMapper.PropMap.MapMerge_TIn1,TIn2,TOut_(TIn1,TIn2).TIn2'></a>

`TIn2`

Second \(override\) source type\.

<a name='PropertyMapper.PropMap.MapMerge_TIn1,TIn2,TOut_(TIn1,TIn2).TOut'></a>

`TOut`

Target type \(must be a reference type with a parameterless constructor\)\.
#### Parameters

<a name='PropertyMapper.PropMap.MapMerge_TIn1,TIn2,TOut_(TIn1,TIn2).source1'></a>

`source1` [TIn1](PropMap.MapMerge.NJ8VPGZ780QF4SUTJ3SX3XTB1.md#PropertyMapper.PropMap.MapMerge_TIn1,TIn2,TOut_(TIn1,TIn2).TIn1 'PropertyMapper\.PropMap\.MapMerge\<TIn1,TIn2,TOut\>\(TIn1, TIn2\)\.TIn1')

Base source; provides the initial mapping\.

<a name='PropertyMapper.PropMap.MapMerge_TIn1,TIn2,TOut_(TIn1,TIn2).source2'></a>

`source2` [TIn2](PropMap.MapMerge.NJ8VPGZ780QF4SUTJ3SX3XTB1.md#PropertyMapper.PropMap.MapMerge_TIn1,TIn2,TOut_(TIn1,TIn2).TIn2 'PropertyMapper\.PropMap\.MapMerge\<TIn1,TIn2,TOut\>\(TIn1, TIn2\)\.TIn2')

Override source; its matching properties overwrite those already set from [source1](PropMap.MapMerge.NJ8VPGZ780QF4SUTJ3SX3XTB1.md#PropertyMapper.PropMap.MapMerge_TIn1,TIn2,TOut_(TIn1,TIn2).source1 'PropertyMapper\.PropMap\.MapMerge\<TIn1,TIn2,TOut\>\(TIn1, TIn2\)\.source1')\.

#### Returns
[TOut](PropMap.MapMerge.NJ8VPGZ780QF4SUTJ3SX3XTB1.md#PropertyMapper.PropMap.MapMerge_TIn1,TIn2,TOut_(TIn1,TIn2).TOut 'PropertyMapper\.PropMap\.MapMerge\<TIn1,TIn2,TOut\>\(TIn1, TIn2\)\.TOut')  
A new [TOut](PropMap.MapMerge.NJ8VPGZ780QF4SUTJ3SX3XTB1.md#PropertyMapper.PropMap.MapMerge_TIn1,TIn2,TOut_(TIn1,TIn2).TOut 'PropertyMapper\.PropMap\.MapMerge\<TIn1,TIn2,TOut\>\(TIn1, TIn2\)\.TOut') with properties merged from both sources\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When either source is null\.