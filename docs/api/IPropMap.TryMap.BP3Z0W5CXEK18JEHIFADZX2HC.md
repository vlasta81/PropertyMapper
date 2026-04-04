## IPropMap\.TryMap\<TIn,TOut\>\(TIn, TOut\) Method

Try\-map pattern; returns [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') and sets [result](IPropMap.TryMap.BP3Z0W5CXEK18JEHIFADZX2HC.md#PropertyMapper.IPropMap.TryMap_TIn,TOut_(TIn,TOut).result 'PropertyMapper\.IPropMap\.TryMap\<TIn,TOut\>\(TIn, TOut\)\.result') to [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null') when source is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

```csharp
bool TryMap<TIn,TOut>(TIn? source, out TOut? result)
    where TIn : class
    where TOut : class, new();
```
#### Type parameters

<a name='PropertyMapper.IPropMap.TryMap_TIn,TOut_(TIn,TOut).TIn'></a>

`TIn`

<a name='PropertyMapper.IPropMap.TryMap_TIn,TOut_(TIn,TOut).TOut'></a>

`TOut`
#### Parameters

<a name='PropertyMapper.IPropMap.TryMap_TIn,TOut_(TIn,TOut).source'></a>

`source` [TIn](IPropMap.TryMap.BP3Z0W5CXEK18JEHIFADZX2HC.md#PropertyMapper.IPropMap.TryMap_TIn,TOut_(TIn,TOut).TIn 'PropertyMapper\.IPropMap\.TryMap\<TIn,TOut\>\(TIn, TOut\)\.TIn')

<a name='PropertyMapper.IPropMap.TryMap_TIn,TOut_(TIn,TOut).result'></a>

`result` [TOut](IPropMap.TryMap.BP3Z0W5CXEK18JEHIFADZX2HC.md#PropertyMapper.IPropMap.TryMap_TIn,TOut_(TIn,TOut).TOut 'PropertyMapper\.IPropMap\.TryMap\<TIn,TOut\>\(TIn, TOut\)\.TOut')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')