## IPropMap\.MapOrDefault\<TIn,TOut\>\(TIn\) Method

Maps [source](IPropMap.MapOrDefault.I7NGZZCD4LEJCNL5CJ3F628B2.md#PropertyMapper.IPropMap.MapOrDefault_TIn,TOut_(TIn).source 'PropertyMapper\.IPropMap\.MapOrDefault\<TIn,TOut\>\(TIn\)\.source'), or returns [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null') when source is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

```csharp
TOut? MapOrDefault<TIn,TOut>(TIn? source)
    where TIn : class
    where TOut : class, new();
```
#### Type parameters

<a name='PropertyMapper.IPropMap.MapOrDefault_TIn,TOut_(TIn).TIn'></a>

`TIn`

<a name='PropertyMapper.IPropMap.MapOrDefault_TIn,TOut_(TIn).TOut'></a>

`TOut`
#### Parameters

<a name='PropertyMapper.IPropMap.MapOrDefault_TIn,TOut_(TIn).source'></a>

`source` [TIn](IPropMap.MapOrDefault.I7NGZZCD4LEJCNL5CJ3F628B2.md#PropertyMapper.IPropMap.MapOrDefault_TIn,TOut_(TIn).TIn 'PropertyMapper\.IPropMap\.MapOrDefault\<TIn,TOut\>\(TIn\)\.TIn')

#### Returns
[TOut](IPropMap.MapOrDefault.I7NGZZCD4LEJCNL5CJ3F628B2.md#PropertyMapper.IPropMap.MapOrDefault_TIn,TOut_(TIn).TOut 'PropertyMapper\.IPropMap\.MapOrDefault\<TIn,TOut\>\(TIn\)\.TOut')