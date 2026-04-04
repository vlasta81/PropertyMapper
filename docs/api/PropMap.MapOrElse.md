#### [PropertyMapper](index.md 'index')
### [PropertyMapper](PropertyMapper.md 'PropertyMapper').[PropMap](PropMap.md 'PropertyMapper\.PropMap')

## PropMap\.MapOrElse Method

| Overloads | |
| :--- | :--- |
| [MapOrElse&lt;TIn,TOut&gt;\(TIn, Func&lt;TOut&gt;\)](PropMap.MapOrElse.md#PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_) 'PropertyMapper\.PropMap\.MapOrElse\<TIn,TOut\>\(TIn, System\.Func\<TOut\>\)') | Maps [source](PropMap.md#PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_).source 'PropertyMapper\.PropMap\.MapOrElse\<TIn,TOut\>\(TIn, System\.Func\<TOut\>\)\.source'), or invokes [fallbackFactory](PropMap.md#PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_).fallbackFactory 'PropertyMapper\.PropMap\.MapOrElse\<TIn,TOut\>\(TIn, System\.Func\<TOut\>\)\.fallbackFactory') and returns its result when [source](PropMap.md#PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_).source 'PropertyMapper\.PropMap\.MapOrElse\<TIn,TOut\>\(TIn, System\.Func\<TOut\>\)\.source') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\. The factory is only called when needed \(lazy evaluation\)\. |
| [MapOrElse&lt;TIn,TOut&gt;\(TIn, TOut\)](PropMap.MapOrElse.md#PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,TOut) 'PropertyMapper\.PropMap\.MapOrElse\<TIn,TOut\>\(TIn, TOut\)') | Maps [source](PropMap.md#PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,TOut).source 'PropertyMapper\.PropMap\.MapOrElse\<TIn,TOut\>\(TIn, TOut\)\.source'), or returns the pre\-built [fallback](PropMap.md#PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,TOut).fallback 'PropertyMapper\.PropMap\.MapOrElse\<TIn,TOut\>\(TIn, TOut\)\.fallback') instance when [source](PropMap.md#PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,TOut).source 'PropertyMapper\.PropMap\.MapOrElse\<TIn,TOut\>\(TIn, TOut\)\.source') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\. |

<a name='PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_)'></a>

## PropMap\.MapOrElse\<TIn,TOut\>\(TIn, Func\<TOut\>\) Method

Maps [source](PropMap.md#PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_).source 'PropertyMapper\.PropMap\.MapOrElse\<TIn,TOut\>\(TIn, System\.Func\<TOut\>\)\.source'), or invokes [fallbackFactory](PropMap.md#PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_).fallbackFactory 'PropertyMapper\.PropMap\.MapOrElse\<TIn,TOut\>\(TIn, System\.Func\<TOut\>\)\.fallbackFactory') and returns its result
when [source](PropMap.md#PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_).source 'PropertyMapper\.PropMap\.MapOrElse\<TIn,TOut\>\(TIn, System\.Func\<TOut\>\)\.source') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.
The factory is only called when needed \(lazy evaluation\)\.

```csharp
public TOut MapOrElse<TIn,TOut>(TIn? source, System.Func<TOut> fallbackFactory)
    where TIn : class
    where TOut : class, new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_).TIn'></a>

`TIn`

<a name='PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_).TOut'></a>

`TOut`
#### Parameters

<a name='PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_).source'></a>

`source` [TIn](PropMap.md#PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_).TIn 'PropertyMapper\.PropMap\.MapOrElse\<TIn,TOut\>\(TIn, System\.Func\<TOut\>\)\.TIn')

<a name='PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_).fallbackFactory'></a>

`fallbackFactory` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-1 'System\.Func\`1')[TOut](PropMap.md#PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_).TOut 'PropertyMapper\.PropMap\.MapOrElse\<TIn,TOut\>\(TIn, System\.Func\<TOut\>\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-1 'System\.Func\`1')

#### Returns
[TOut](PropMap.md#PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_).TOut 'PropertyMapper\.PropMap\.MapOrElse\<TIn,TOut\>\(TIn, System\.Func\<TOut\>\)\.TOut')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [fallbackFactory](PropMap.md#PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,System.Func_TOut_).fallbackFactory 'PropertyMapper\.PropMap\.MapOrElse\<TIn,TOut\>\(TIn, System\.Func\<TOut\>\)\.fallbackFactory') is null\.

<a name='PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,TOut)'></a>

## PropMap\.MapOrElse\<TIn,TOut\>\(TIn, TOut\) Method

Maps [source](PropMap.md#PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,TOut).source 'PropertyMapper\.PropMap\.MapOrElse\<TIn,TOut\>\(TIn, TOut\)\.source'), or returns the pre\-built [fallback](PropMap.md#PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,TOut).fallback 'PropertyMapper\.PropMap\.MapOrElse\<TIn,TOut\>\(TIn, TOut\)\.fallback') instance
when [source](PropMap.md#PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,TOut).source 'PropertyMapper\.PropMap\.MapOrElse\<TIn,TOut\>\(TIn, TOut\)\.source') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

```csharp
public TOut MapOrElse<TIn,TOut>(TIn? source, TOut fallback)
    where TIn : class
    where TOut : class, new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,TOut).TIn'></a>

`TIn`

<a name='PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,TOut).TOut'></a>

`TOut`
#### Parameters

<a name='PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,TOut).source'></a>

`source` [TIn](PropMap.md#PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,TOut).TIn 'PropertyMapper\.PropMap\.MapOrElse\<TIn,TOut\>\(TIn, TOut\)\.TIn')

<a name='PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,TOut).fallback'></a>

`fallback` [TOut](PropMap.md#PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,TOut).TOut 'PropertyMapper\.PropMap\.MapOrElse\<TIn,TOut\>\(TIn, TOut\)\.TOut')

#### Returns
[TOut](PropMap.md#PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,TOut).TOut 'PropertyMapper\.PropMap\.MapOrElse\<TIn,TOut\>\(TIn, TOut\)\.TOut')

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [fallback](PropMap.md#PropertyMapper.PropMap.MapOrElse_TIn,TOut_(TIn,TOut).fallback 'PropertyMapper\.PropMap\.MapOrElse\<TIn,TOut\>\(TIn, TOut\)\.fallback') is null\.

---
Generated by [DefaultDocumentation](https://github.com/Doraku/DefaultDocumentation 'https://github\.com/Doraku/DefaultDocumentation')