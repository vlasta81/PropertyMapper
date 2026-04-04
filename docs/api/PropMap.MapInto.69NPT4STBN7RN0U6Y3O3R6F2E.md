## PropMap\.MapInto\<TIn,TOut\>\(TIn, TOut\) Method

Maps matching properties from [source](PropMap.MapInto.69NPT4STBN7RN0U6Y3O3R6F2E.md#PropertyMapper.PropMap.MapInto_TIn,TOut_(TIn,TOut).source 'PropertyMapper\.PropMap\.MapInto\<TIn,TOut\>\(TIn, TOut\)\.source') onto an existing [target](PropMap.MapInto.69NPT4STBN7RN0U6Y3O3R6F2E.md#PropertyMapper.PropMap.MapInto_TIn,TOut_(TIn,TOut).target 'PropertyMapper\.PropMap\.MapInto\<TIn,TOut\>\(TIn, TOut\)\.target') instance\.
Only properties whose names match are updated; unmatched properties on the target are left unchanged\.

```csharp
public void MapInto<TIn,TOut>(TIn source, TOut target)
    where TOut : class;
```
#### Type parameters

<a name='PropertyMapper.PropMap.MapInto_TIn,TOut_(TIn,TOut).TIn'></a>

`TIn`

Source type\.

<a name='PropertyMapper.PropMap.MapInto_TIn,TOut_(TIn,TOut).TOut'></a>

`TOut`

Target type \(must be a reference type\)\.
#### Parameters

<a name='PropertyMapper.PropMap.MapInto_TIn,TOut_(TIn,TOut).source'></a>

`source` [TIn](PropMap.MapInto.69NPT4STBN7RN0U6Y3O3R6F2E.md#PropertyMapper.PropMap.MapInto_TIn,TOut_(TIn,TOut).TIn 'PropertyMapper\.PropMap\.MapInto\<TIn,TOut\>\(TIn, TOut\)\.TIn')

Source object to map from\.

<a name='PropertyMapper.PropMap.MapInto_TIn,TOut_(TIn,TOut).target'></a>

`target` [TOut](PropMap.MapInto.69NPT4STBN7RN0U6Y3O3R6F2E.md#PropertyMapper.PropMap.MapInto_TIn,TOut_(TIn,TOut).TOut 'PropertyMapper\.PropMap\.MapInto\<TIn,TOut\>\(TIn, TOut\)\.TOut')

Existing target object to update in place\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [source](PropMap.MapInto.69NPT4STBN7RN0U6Y3O3R6F2E.md#PropertyMapper.PropMap.MapInto_TIn,TOut_(TIn,TOut).source 'PropertyMapper\.PropMap\.MapInto\<TIn,TOut\>\(TIn, TOut\)\.source') or [target](PropMap.MapInto.69NPT4STBN7RN0U6Y3O3R6F2E.md#PropertyMapper.PropMap.MapInto_TIn,TOut_(TIn,TOut).target 'PropertyMapper\.PropMap\.MapInto\<TIn,TOut\>\(TIn, TOut\)\.target') is null\.