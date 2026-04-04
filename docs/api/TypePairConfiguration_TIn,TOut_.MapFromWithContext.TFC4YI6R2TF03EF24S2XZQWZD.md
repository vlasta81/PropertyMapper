## TypePairConfiguration\<TIn,TOut\>\.MapFromWithContext\<TCtx,TProp\>\(Expression\<Func\<TOut,TProp\>\>, Func\<TIn,TCtx,TProp\>\) Method

Maps a target property using a context\-aware factory\.
Unlike [MapFrom&lt;TProp&gt;\(Expression&lt;Func&lt;TOut,TProp&gt;&gt;, Func&lt;TIn,TProp&gt;\)](TypePairConfiguration_TIn,TOut_.MapFrom.EGYESNML8XAEXNQBK3Y0FF71C.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.MapFrom\<TProp\>\(System\.Linq\.Expressions\.Expression\<System\.Func\<TOut,TProp\>\>, System\.Func\<TIn,TProp\>\)'),
the factory receives both the source object and a per\-call context value of type
[TCtx](TypePairConfiguration_TIn,TOut_.MapFromWithContext.TFC4YI6R2TF03EF24S2XZQWZD.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.MapFromWithContext_TCtx,TProp_(System.Linq.Expressions.Expression_System.Func_TOut,TProp__,System.Func_TIn,TCtx,TProp_).TCtx 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.MapFromWithContext\<TCtx,TProp\>\(System\.Linq\.Expressions\.Expression\<System\.Func\<TOut,TProp\>\>, System\.Func\<TIn,TCtx,TProp\>\)\.TCtx') supplied to
[MapWithContext&lt;TIn,TOut,TCtx&gt;\(TIn, TCtx\)](PropMap.MapWithContext.VBYRHLVUYW6CLL935CECSDD7A.md 'PropertyMapper\.PropMap\.MapWithContext\<TIn,TOut,TCtx\>\(TIn, TCtx\)')\.
The context is \<b\>not\</b\> compiled into the IL delegate — it is passed on each call,
making this pattern safe for per\-request state \(exchange rates, tenant settings, etc\.\)\.

```csharp
public PropertyMapper.Configuration.TypePairConfiguration<TIn,TOut> MapFromWithContext<TCtx,TProp>(System.Linq.Expressions.Expression<System.Func<TOut,TProp>> target, System.Func<TIn,TCtx,TProp> mapping);
```
#### Type parameters

<a name='PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.MapFromWithContext_TCtx,TProp_(System.Linq.Expressions.Expression_System.Func_TOut,TProp__,System.Func_TIn,TCtx,TProp_).TCtx'></a>

`TCtx`

Context type supplied per call\.

<a name='PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.MapFromWithContext_TCtx,TProp_(System.Linq.Expressions.Expression_System.Func_TOut,TProp__,System.Func_TIn,TCtx,TProp_).TProp'></a>

`TProp`

Property type\.
#### Parameters

<a name='PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.MapFromWithContext_TCtx,TProp_(System.Linq.Expressions.Expression_System.Func_TOut,TProp__,System.Func_TIn,TCtx,TProp_).target'></a>

`target` [System\.Linq\.Expressions\.Expression&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression-1 'System\.Linq\.Expressions\.Expression\`1')[System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[TOut](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TOut 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TOut')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[TProp](TypePairConfiguration_TIn,TOut_.MapFromWithContext.TFC4YI6R2TF03EF24S2XZQWZD.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.MapFromWithContext_TCtx,TProp_(System.Linq.Expressions.Expression_System.Func_TOut,TProp__,System.Func_TIn,TCtx,TProp_).TProp 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.MapFromWithContext\<TCtx,TProp\>\(System\.Linq\.Expressions\.Expression\<System\.Func\<TOut,TProp\>\>, System\.Func\<TIn,TCtx,TProp\>\)\.TProp')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression-1 'System\.Linq\.Expressions\.Expression\`1')

Expression selecting the target property\.

<a name='PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.MapFromWithContext_TCtx,TProp_(System.Linq.Expressions.Expression_System.Func_TOut,TProp__,System.Func_TIn,TCtx,TProp_).mapping'></a>

`mapping` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[TIn](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TIn 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TIn')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[TCtx](TypePairConfiguration_TIn,TOut_.MapFromWithContext.TFC4YI6R2TF03EF24S2XZQWZD.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.MapFromWithContext_TCtx,TProp_(System.Linq.Expressions.Expression_System.Func_TOut,TProp__,System.Func_TIn,TCtx,TProp_).TCtx 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.MapFromWithContext\<TCtx,TProp\>\(System\.Linq\.Expressions\.Expression\<System\.Func\<TOut,TProp\>\>, System\.Func\<TIn,TCtx,TProp\>\)\.TCtx')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')[TProp](TypePairConfiguration_TIn,TOut_.MapFromWithContext.TFC4YI6R2TF03EF24S2XZQWZD.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.MapFromWithContext_TCtx,TProp_(System.Linq.Expressions.Expression_System.Func_TOut,TProp__,System.Func_TIn,TCtx,TProp_).TProp 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.MapFromWithContext\<TCtx,TProp\>\(System\.Linq\.Expressions\.Expression\<System\.Func\<TOut,TProp\>\>, System\.Func\<TIn,TCtx,TProp\>\)\.TProp')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-3 'System\.Func\`3')

Factory receiving source and context, returning the property value\.

#### Returns
[PropertyMapper\.Configuration\.TypePairConfiguration&lt;](TypePairConfiguration_TIn,TOut_.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>')[TIn](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TIn 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TIn')[,](TypePairConfiguration_TIn,TOut_.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>')[TOut](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TOut 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TOut')[&gt;](TypePairConfiguration_TIn,TOut_.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>')  
This instance for method chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [mapping](TypePairConfiguration_TIn,TOut_.MapFromWithContext.TFC4YI6R2TF03EF24S2XZQWZD.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.MapFromWithContext_TCtx,TProp_(System.Linq.Expressions.Expression_System.Func_TOut,TProp__,System.Func_TIn,TCtx,TProp_).mapping 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.MapFromWithContext\<TCtx,TProp\>\(System\.Linq\.Expressions\.Expression\<System\.Func\<TOut,TProp\>\>, System\.Func\<TIn,TCtx,TProp\>\)\.mapping') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
When the expression does not resolve to a simple property access\.