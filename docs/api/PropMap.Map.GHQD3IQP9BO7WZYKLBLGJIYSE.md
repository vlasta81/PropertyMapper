## PropMap\.Map\<TIn,TOut\>\(TIn\) Method

Maps [source](PropMap.Map.GHQD3IQP9BO7WZYKLBLGJIYSE.md#PropertyMapper.PropMap.Map_TIn,TOut_(TIn).source 'PropertyMapper\.PropMap\.Map\<TIn,TOut\>\(TIn\)\.source') to a new [TOut](PropMap.Map.GHQD3IQP9BO7WZYKLBLGJIYSE.md#PropertyMapper.PropMap.Map_TIn,TOut_(TIn).TOut 'PropertyMapper\.PropMap\.Map\<TIn,TOut\>\(TIn\)\.TOut') instance by copying all
matched properties\. Uses a lock\-free cache on the hot path; compiles the delegate on first use\.

```csharp
public TOut Map<TIn,TOut>(TIn source)
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.Map_TIn,TOut_(TIn).TIn'></a>

`TIn`

Source type\.

<a name='PropertyMapper.PropMap.Map_TIn,TOut_(TIn).TOut'></a>

`TOut`

Target type \(must have a parameterless constructor\)\.
#### Parameters

<a name='PropertyMapper.PropMap.Map_TIn,TOut_(TIn).source'></a>

`source` [TIn](PropMap.Map.GHQD3IQP9BO7WZYKLBLGJIYSE.md#PropertyMapper.PropMap.Map_TIn,TOut_(TIn).TIn 'PropertyMapper\.PropMap\.Map\<TIn,TOut\>\(TIn\)\.TIn')

Source object to map from\.

#### Returns
[TOut](PropMap.Map.GHQD3IQP9BO7WZYKLBLGJIYSE.md#PropertyMapper.PropMap.Map_TIn,TOut_(TIn).TOut 'PropertyMapper\.PropMap\.Map\<TIn,TOut\>\(TIn\)\.TOut')  
A new [TOut](PropMap.Map.GHQD3IQP9BO7WZYKLBLGJIYSE.md#PropertyMapper.PropMap.Map_TIn,TOut_(TIn).TOut 'PropertyMapper\.PropMap\.Map\<TIn,TOut\>\(TIn\)\.TOut') with all matched properties copied from [source](PropMap.Map.GHQD3IQP9BO7WZYKLBLGJIYSE.md#PropertyMapper.PropMap.Map_TIn,TOut_(TIn).source 'PropertyMapper\.PropMap\.Map\<TIn,TOut\>\(TIn\)\.source')\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [source](PropMap.Map.GHQD3IQP9BO7WZYKLBLGJIYSE.md#PropertyMapper.PropMap.Map_TIn,TOut_(TIn).source 'PropertyMapper\.PropMap\.Map\<TIn,TOut\>\(TIn\)\.source') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.