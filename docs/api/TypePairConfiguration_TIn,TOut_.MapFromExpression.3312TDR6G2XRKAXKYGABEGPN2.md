## TypePairConfiguration\<TIn,TOut\>\.MapFromExpression\<TProp\>\(Expression\<Func\<TOut,TProp\>\>, Expression\<Func\<TIn,TProp\>\>\) Method

Maps a target property using a custom expression\-based factory, suitable for both regular mapping
and [System\.Linq\.IQueryable&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1 'System\.Linq\.IQueryable\`1') projection \(e\.g\. EF Core\)\.
Unlike [MapFrom&lt;TProp&gt;\(Expression&lt;Func&lt;TOut,TProp&gt;&gt;, Func&lt;TIn,TProp&gt;\)](TypePairConfiguration_TIn,TOut_.MapFrom.EGYESNML8XAEXNQBK3Y0FF71C.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.MapFrom\<TProp\>\(System\.Linq\.Expressions\.Expression\<System\.Func\<TOut,TProp\>\>, System\.Func\<TIn,TProp\>\)'),
the factory is expressed as an [System\.Linq\.Expressions\.Expression&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression-1 'System\.Linq\.Expressions\.Expression\`1') so that LINQ providers
can translate it server\-side\.

```csharp
public PropertyMapper.Configuration.TypePairConfiguration<TIn,TOut> MapFromExpression<TProp>(System.Linq.Expressions.Expression<System.Func<TOut,TProp>> target, System.Linq.Expressions.Expression<System.Func<TIn,TProp>> mapping);
```
#### Type parameters

<a name='PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.MapFromExpression_TProp_(System.Linq.Expressions.Expression_System.Func_TOut,TProp__,System.Linq.Expressions.Expression_System.Func_TIn,TProp__).TProp'></a>

`TProp`

Property type\.
#### Parameters

<a name='PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.MapFromExpression_TProp_(System.Linq.Expressions.Expression_System.Func_TOut,TProp__,System.Linq.Expressions.Expression_System.Func_TIn,TProp__).target'></a>

`target` [System\.Linq\.Expressions\.Expression&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression-1 'System\.Linq\.Expressions\.Expression\`1')[System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[TOut](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TOut 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TOut')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[TProp](TypePairConfiguration_TIn,TOut_.MapFromExpression.3312TDR6G2XRKAXKYGABEGPN2.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.MapFromExpression_TProp_(System.Linq.Expressions.Expression_System.Func_TOut,TProp__,System.Linq.Expressions.Expression_System.Func_TIn,TProp__).TProp 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.MapFromExpression\<TProp\>\(System\.Linq\.Expressions\.Expression\<System\.Func\<TOut,TProp\>\>, System\.Linq\.Expressions\.Expression\<System\.Func\<TIn,TProp\>\>\)\.TProp')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression-1 'System\.Linq\.Expressions\.Expression\`1')

Expression selecting the target property \(e\.g\. `x => x.TotalPrice`\)\.

<a name='PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.MapFromExpression_TProp_(System.Linq.Expressions.Expression_System.Func_TOut,TProp__,System.Linq.Expressions.Expression_System.Func_TIn,TProp__).mapping'></a>

`mapping` [System\.Linq\.Expressions\.Expression&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression-1 'System\.Linq\.Expressions\.Expression\`1')[System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[TIn](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TIn 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TIn')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[TProp](TypePairConfiguration_TIn,TOut_.MapFromExpression.3312TDR6G2XRKAXKYGABEGPN2.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.MapFromExpression_TProp_(System.Linq.Expressions.Expression_System.Func_TOut,TProp__,System.Linq.Expressions.Expression_System.Func_TIn,TProp__).TProp 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.MapFromExpression\<TProp\>\(System\.Linq\.Expressions\.Expression\<System\.Func\<TOut,TProp\>\>, System\.Linq\.Expressions\.Expression\<System\.Func\<TIn,TProp\>\>\)\.TProp')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression-1 'System\.Linq\.Expressions\.Expression\`1')

Expression factory that derives the value from a [TIn](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TIn 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TIn') source instance\.

#### Returns
[PropertyMapper\.Configuration\.TypePairConfiguration&lt;](TypePairConfiguration_TIn,TOut_.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>')[TIn](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TIn 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TIn')[,](TypePairConfiguration_TIn,TOut_.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>')[TOut](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TOut 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TOut')[&gt;](TypePairConfiguration_TIn,TOut_.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>')  
This instance for method chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [mapping](TypePairConfiguration_TIn,TOut_.MapFromExpression.3312TDR6G2XRKAXKYGABEGPN2.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.MapFromExpression_TProp_(System.Linq.Expressions.Expression_System.Func_TOut,TProp__,System.Linq.Expressions.Expression_System.Func_TIn,TProp__).mapping 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.MapFromExpression\<TProp\>\(System\.Linq\.Expressions\.Expression\<System\.Func\<TOut,TProp\>\>, System\.Linq\.Expressions\.Expression\<System\.Func\<TIn,TProp\>\>\)\.mapping') is null\.

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
When the expression does not resolve to a simple property access,
            or the target property is not found on [TOut](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TOut 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TOut')\.