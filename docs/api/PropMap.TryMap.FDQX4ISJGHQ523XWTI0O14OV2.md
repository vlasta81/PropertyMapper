## PropMap\.TryMap\<TIn,TOut\>\(TIn, TOut\) Method

Try\-map pattern: maps [source](PropMap.TryMap.FDQX4ISJGHQ523XWTI0O14OV2.md#PropertyMapper.PropMap.TryMap_TIn,TOut_(TIn,TOut).source 'PropertyMapper\.PropMap\.TryMap\<TIn,TOut\>\(TIn, TOut\)\.source') into [result](PropMap.TryMap.FDQX4ISJGHQ523XWTI0O14OV2.md#PropertyMapper.PropMap.TryMap_TIn,TOut_(TIn,TOut).result 'PropertyMapper\.PropMap\.TryMap\<TIn,TOut\>\(TIn, TOut\)\.result') and returns
[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool'), or sets [result](PropMap.TryMap.FDQX4ISJGHQ523XWTI0O14OV2.md#PropertyMapper.PropMap.TryMap_TIn,TOut_(TIn,TOut).result 'PropertyMapper\.PropMap\.TryMap\<TIn,TOut\>\(TIn, TOut\)\.result') to [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null') and returns
[false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') when [source](PropMap.TryMap.FDQX4ISJGHQ523XWTI0O14OV2.md#PropertyMapper.PropMap.TryMap_TIn,TOut_(TIn,TOut).source 'PropertyMapper\.PropMap\.TryMap\<TIn,TOut\>\(TIn, TOut\)\.source') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

```csharp
public bool TryMap<TIn,TOut>(TIn? source, out TOut? result)
    where TIn : class
    where TOut : class, new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.TryMap_TIn,TOut_(TIn,TOut).TIn'></a>

`TIn`

<a name='PropertyMapper.PropMap.TryMap_TIn,TOut_(TIn,TOut).TOut'></a>

`TOut`
#### Parameters

<a name='PropertyMapper.PropMap.TryMap_TIn,TOut_(TIn,TOut).source'></a>

`source` [TIn](PropMap.TryMap.FDQX4ISJGHQ523XWTI0O14OV2.md#PropertyMapper.PropMap.TryMap_TIn,TOut_(TIn,TOut).TIn 'PropertyMapper\.PropMap\.TryMap\<TIn,TOut\>\(TIn, TOut\)\.TIn')

<a name='PropertyMapper.PropMap.TryMap_TIn,TOut_(TIn,TOut).result'></a>

`result` [TOut](PropMap.TryMap.FDQX4ISJGHQ523XWTI0O14OV2.md#PropertyMapper.PropMap.TryMap_TIn,TOut_(TIn,TOut).TOut 'PropertyMapper\.PropMap\.TryMap\<TIn,TOut\>\(TIn, TOut\)\.TOut')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')