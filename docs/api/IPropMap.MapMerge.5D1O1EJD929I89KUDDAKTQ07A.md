## IPropMap\.MapMerge\<TIn1,TIn2,TOut\>\(TIn1, TIn2\) Method

Creates a new [TOut](IPropMap.MapMerge.5D1O1EJD929I89KUDDAKTQ07A.md#PropertyMapper.IPropMap.MapMerge_TIn1,TIn2,TOut_(TIn1,TIn2).TOut 'PropertyMapper\.IPropMap\.MapMerge\<TIn1,TIn2,TOut\>\(TIn1, TIn2\)\.TOut') from [source1](IPropMap.MapMerge.5D1O1EJD929I89KUDDAKTQ07A.md#PropertyMapper.IPropMap.MapMerge_TIn1,TIn2,TOut_(TIn1,TIn2).source1 'PropertyMapper\.IPropMap\.MapMerge\<TIn1,TIn2,TOut\>\(TIn1, TIn2\)\.source1'), then merges properties from [source2](IPropMap.MapMerge.5D1O1EJD929I89KUDDAKTQ07A.md#PropertyMapper.IPropMap.MapMerge_TIn1,TIn2,TOut_(TIn1,TIn2).source2 'PropertyMapper\.IPropMap\.MapMerge\<TIn1,TIn2,TOut\>\(TIn1, TIn2\)\.source2') onto it\.

```csharp
TOut MapMerge<TIn1,TIn2,TOut>(TIn1 source1, TIn2 source2)
    where TOut : class, new();
```
#### Type parameters

<a name='PropertyMapper.IPropMap.MapMerge_TIn1,TIn2,TOut_(TIn1,TIn2).TIn1'></a>

`TIn1`

<a name='PropertyMapper.IPropMap.MapMerge_TIn1,TIn2,TOut_(TIn1,TIn2).TIn2'></a>

`TIn2`

<a name='PropertyMapper.IPropMap.MapMerge_TIn1,TIn2,TOut_(TIn1,TIn2).TOut'></a>

`TOut`
#### Parameters

<a name='PropertyMapper.IPropMap.MapMerge_TIn1,TIn2,TOut_(TIn1,TIn2).source1'></a>

`source1` [TIn1](IPropMap.MapMerge.5D1O1EJD929I89KUDDAKTQ07A.md#PropertyMapper.IPropMap.MapMerge_TIn1,TIn2,TOut_(TIn1,TIn2).TIn1 'PropertyMapper\.IPropMap\.MapMerge\<TIn1,TIn2,TOut\>\(TIn1, TIn2\)\.TIn1')

<a name='PropertyMapper.IPropMap.MapMerge_TIn1,TIn2,TOut_(TIn1,TIn2).source2'></a>

`source2` [TIn2](IPropMap.MapMerge.5D1O1EJD929I89KUDDAKTQ07A.md#PropertyMapper.IPropMap.MapMerge_TIn1,TIn2,TOut_(TIn1,TIn2).TIn2 'PropertyMapper\.IPropMap\.MapMerge\<TIn1,TIn2,TOut\>\(TIn1, TIn2\)\.TIn2')

#### Returns
[TOut](IPropMap.MapMerge.5D1O1EJD929I89KUDDAKTQ07A.md#PropertyMapper.IPropMap.MapMerge_TIn1,TIn2,TOut_(TIn1,TIn2).TOut 'PropertyMapper\.IPropMap\.MapMerge\<TIn1,TIn2,TOut\>\(TIn1, TIn2\)\.TOut')