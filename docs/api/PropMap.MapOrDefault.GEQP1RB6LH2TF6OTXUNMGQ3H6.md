## PropMap\.MapOrDefault\<TIn,TOut\>\(TIn\) Method

Maps [source](PropMap.MapOrDefault.GEQP1RB6LH2TF6OTXUNMGQ3H6.md#PropertyMapper.PropMap.MapOrDefault_TIn,TOut_(TIn).source 'PropertyMapper\.PropMap\.MapOrDefault\<TIn,TOut\>\(TIn\)\.source') to a new [TOut](PropMap.MapOrDefault.GEQP1RB6LH2TF6OTXUNMGQ3H6.md#PropertyMapper.PropMap.MapOrDefault_TIn,TOut_(TIn).TOut 'PropertyMapper\.PropMap\.MapOrDefault\<TIn,TOut\>\(TIn\)\.TOut') instance, or returns
[null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null') when [source](PropMap.MapOrDefault.GEQP1RB6LH2TF6OTXUNMGQ3H6.md#PropertyMapper.PropMap.MapOrDefault_TIn,TOut_(TIn).source 'PropertyMapper\.PropMap\.MapOrDefault\<TIn,TOut\>\(TIn\)\.source') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.
Never throws [System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')\.

```csharp
public TOut? MapOrDefault<TIn,TOut>(TIn? source)
    where TIn : class
    where TOut : class, new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.MapOrDefault_TIn,TOut_(TIn).TIn'></a>

`TIn`

Source type \(must be a reference type\)\.

<a name='PropertyMapper.PropMap.MapOrDefault_TIn,TOut_(TIn).TOut'></a>

`TOut`

Target type \(must have a parameterless constructor\)\.
#### Parameters

<a name='PropertyMapper.PropMap.MapOrDefault_TIn,TOut_(TIn).source'></a>

`source` [TIn](PropMap.MapOrDefault.GEQP1RB6LH2TF6OTXUNMGQ3H6.md#PropertyMapper.PropMap.MapOrDefault_TIn,TOut_(TIn).TIn 'PropertyMapper\.PropMap\.MapOrDefault\<TIn,TOut\>\(TIn\)\.TIn')

Source object to map, or [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

#### Returns
[TOut](PropMap.MapOrDefault.GEQP1RB6LH2TF6OTXUNMGQ3H6.md#PropertyMapper.PropMap.MapOrDefault_TIn,TOut_(TIn).TOut 'PropertyMapper\.PropMap\.MapOrDefault\<TIn,TOut\>\(TIn\)\.TOut')  
A mapped [TOut](PropMap.MapOrDefault.GEQP1RB6LH2TF6OTXUNMGQ3H6.md#PropertyMapper.PropMap.MapOrDefault_TIn,TOut_(TIn).TOut 'PropertyMapper\.PropMap\.MapOrDefault\<TIn,TOut\>\(TIn\)\.TOut') instance, or [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null') when [source](PropMap.MapOrDefault.GEQP1RB6LH2TF6OTXUNMGQ3H6.md#PropertyMapper.PropMap.MapOrDefault_TIn,TOut_(TIn).source 'PropertyMapper\.PropMap\.MapOrDefault\<TIn,TOut\>\(TIn\)\.source') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.