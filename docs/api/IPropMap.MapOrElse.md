#### [PropertyMapper](index.md 'index')
### [PropertyMapper](PropertyMapper.md 'PropertyMapper').[IPropMap](IPropMap.md 'PropertyMapper\.IPropMap')

## IPropMap\.MapOrElse Method

| Overloads | |
| :--- | :--- |
| [MapOrElse&lt;TIn,TOut&gt;\(TIn, Func&lt;TOut&gt;\)](IPropMap.MapOrElse.md#PropertyMapper.IPropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_) 'PropertyMapper\.IPropMap\.MapOrElse\<TIn,TOut\>\(TIn, System\.Func\<TOut\>\)') | Maps [source](IPropMap.md#PropertyMapper.IPropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_).source 'PropertyMapper\.IPropMap\.MapOrElse\<TIn,TOut\>\(TIn, System\.Func\<TOut\>\)\.source'), or invokes [fallbackFactory](IPropMap.md#PropertyMapper.IPropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_).fallbackFactory 'PropertyMapper\.IPropMap\.MapOrElse\<TIn,TOut\>\(TIn, System\.Func\<TOut\>\)\.fallbackFactory') when source is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\. |
| [MapOrElse&lt;TIn,TOut&gt;\(TIn, TOut\)](IPropMap.MapOrElse.md#PropertyMapper.IPropMap.MapOrElse_TIn,TOut_(TIn,TOut) 'PropertyMapper\.IPropMap\.MapOrElse\<TIn,TOut\>\(TIn, TOut\)') | Maps [source](IPropMap.md#PropertyMapper.IPropMap.MapOrElse_TIn,TOut_(TIn,TOut).source 'PropertyMapper\.IPropMap\.MapOrElse\<TIn,TOut\>\(TIn, TOut\)\.source'), or returns [fallback](IPropMap.md#PropertyMapper.IPropMap.MapOrElse_TIn,TOut_(TIn,TOut).fallback 'PropertyMapper\.IPropMap\.MapOrElse\<TIn,TOut\>\(TIn, TOut\)\.fallback') when source is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\. |

<a name='PropertyMapper.IPropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_)'></a>

## IPropMap\.MapOrElse\<TIn,TOut\>\(TIn, Func\<TOut\>\) Method

Maps [source](IPropMap.md#PropertyMapper.IPropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_).source 'PropertyMapper\.IPropMap\.MapOrElse\<TIn,TOut\>\(TIn, System\.Func\<TOut\>\)\.source'), or invokes [fallbackFactory](IPropMap.md#PropertyMapper.IPropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_).fallbackFactory 'PropertyMapper\.IPropMap\.MapOrElse\<TIn,TOut\>\(TIn, System\.Func\<TOut\>\)\.fallbackFactory') when source is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

```csharp
TOut MapOrElse<TIn,TOut>(TIn? source, System.Func<TOut> fallbackFactory)
    where TIn : class
    where TOut : class, new();
```
#### Type parameters

<a name='PropertyMapper.IPropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_).TIn'></a>

`TIn`

<a name='PropertyMapper.IPropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_).TOut'></a>

`TOut`
#### Parameters

<a name='PropertyMapper.IPropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_).source'></a>

`source` [TIn](IPropMap.md#PropertyMapper.IPropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_).TIn 'PropertyMapper\.IPropMap\.MapOrElse\<TIn,TOut\>\(TIn, System\.Func\<TOut\>\)\.TIn')

<a name='PropertyMapper.IPropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_).fallbackFactory'></a>

`fallbackFactory` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-1 'System\.Func\`1')[TOut](IPropMap.md#PropertyMapper.IPropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_).TOut 'PropertyMapper\.IPropMap\.MapOrElse\<TIn,TOut\>\(TIn, System\.Func\<TOut\>\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-1 'System\.Func\`1')

#### Returns
[TOut](IPropMap.md#PropertyMapper.IPropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_).TOut 'PropertyMapper\.IPropMap\.MapOrElse\<TIn,TOut\>\(TIn, System\.Func\<TOut\>\)\.TOut')

<a name='PropertyMapper.IPropMap.MapOrElse_TIn,TOut_(TIn,TOut)'></a>

## IPropMap\.MapOrElse\<TIn,TOut\>\(TIn, TOut\) Method

Maps [source](IPropMap.md#PropertyMapper.IPropMap.MapOrElse_TIn,TOut_(TIn,TOut).source 'PropertyMapper\.IPropMap\.MapOrElse\<TIn,TOut\>\(TIn, TOut\)\.source'), or returns [fallback](IPropMap.md#PropertyMapper.IPropMap.MapOrElse_TIn,TOut_(TIn,TOut).fallback 'PropertyMapper\.IPropMap\.MapOrElse\<TIn,TOut\>\(TIn, TOut\)\.fallback') when source is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

```csharp
TOut MapOrElse<TIn,TOut>(TIn? source, TOut fallback)
    where TIn : class
    where TOut : class, new();
```
#### Type parameters

<a name='PropertyMapper.IPropMap.MapOrElse_TIn,TOut_(TIn,TOut).TIn'></a>

`TIn`

<a name='PropertyMapper.IPropMap.MapOrElse_TIn,TOut_(TIn,TOut).TOut'></a>

`TOut`
#### Parameters

<a name='PropertyMapper.IPropMap.MapOrElse_TIn,TOut_(TIn,TOut).source'></a>

`source` [TIn](IPropMap.md#PropertyMapper.IPropMap.MapOrElse_TIn,TOut_(TIn,TOut).TIn 'PropertyMapper\.IPropMap\.MapOrElse\<TIn,TOut\>\(TIn, TOut\)\.TIn')

<a name='PropertyMapper.IPropMap.MapOrElse_TIn,TOut_(TIn,TOut).fallback'></a>

`fallback` [TOut](IPropMap.md#PropertyMapper.IPropMap.MapOrElse_TIn,TOut_(TIn,TOut).TOut 'PropertyMapper\.IPropMap\.MapOrElse\<TIn,TOut\>\(TIn, TOut\)\.TOut')

#### Returns
[TOut](IPropMap.md#PropertyMapper.IPropMap.MapOrElse_TIn,TOut_(TIn,TOut).TOut 'PropertyMapper\.IPropMap\.MapOrElse\<TIn,TOut\>\(TIn, TOut\)\.TOut')

---
Generated by [DefaultDocumentation](https://github.com/Doraku/DefaultDocumentation 'https://github\.com/Doraku/DefaultDocumentation')