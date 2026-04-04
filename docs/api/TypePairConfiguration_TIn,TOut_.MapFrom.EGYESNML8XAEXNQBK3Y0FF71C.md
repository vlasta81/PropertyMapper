## TypePairConfiguration\<TIn,TOut\>\.MapFrom\<TProp\>\(Expression\<Func\<TOut,TProp\>\>, Func\<TIn,TProp\>\) Method

Maps a target property using a custom value factory, bypassing the default name\-match logic\.
The property is first excluded from IL emission \(as if [Ignore&lt;TProp&gt;\(Expression&lt;Func&lt;TOut,TProp&gt;&gt;\)](TypePairConfiguration_TIn,TOut_.Ignore.47Y5JYZI9VSIMEAAQAG2ALEBA.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.Ignore\<TProp\>\(System\.Linq\.Expressions\.Expression\<System\.Func\<TOut,TProp\>\>\)') were called\),
then the compiled setter is invoked after the base mapping completes\.

```csharp
public PropertyMapper.Configuration.TypePairConfiguration<TIn,TOut> MapFrom<TProp>(System.Linq.Expressions.Expression<System.Func<TOut,TProp>> target, System.Func<TIn,TProp> mapping);
```
#### Type parameters

<a name='PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.MapFrom_TProp_(System.Linq.Expressions.Expression_System.Func_TOut,TProp__,System.Func_TIn,TProp_).TProp'></a>

`TProp`

Property type\.
#### Parameters

<a name='PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.MapFrom_TProp_(System.Linq.Expressions.Expression_System.Func_TOut,TProp__,System.Func_TIn,TProp_).target'></a>

`target` [System\.Linq\.Expressions\.Expression&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression-1 'System\.Linq\.Expressions\.Expression\`1')[System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[TOut](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TOut 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TOut')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[TProp](TypePairConfiguration_TIn,TOut_.MapFrom.EGYESNML8XAEXNQBK3Y0FF71C.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.MapFrom_TProp_(System.Linq.Expressions.Expression_System.Func_TOut,TProp__,System.Func_TIn,TProp_).TProp 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.MapFrom\<TProp\>\(System\.Linq\.Expressions\.Expression\<System\.Func\<TOut,TProp\>\>, System\.Func\<TIn,TProp\>\)\.TProp')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression-1 'System\.Linq\.Expressions\.Expression\`1')

Expression selecting the target property \(e\.g\. `x => x.FullName`\)\.

<a name='PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.MapFrom_TProp_(System.Linq.Expressions.Expression_System.Func_TOut,TProp__,System.Func_TIn,TProp_).mapping'></a>

`mapping` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[TIn](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TIn 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TIn')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[TProp](TypePairConfiguration_TIn,TOut_.MapFrom.EGYESNML8XAEXNQBK3Y0FF71C.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.MapFrom_TProp_(System.Linq.Expressions.Expression_System.Func_TOut,TProp__,System.Func_TIn,TProp_).TProp 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.MapFrom\<TProp\>\(System\.Linq\.Expressions\.Expression\<System\.Func\<TOut,TProp\>\>, System\.Func\<TIn,TProp\>\)\.TProp')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

Factory that derives the value from a [TIn](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TIn 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TIn') source instance\.

#### Returns
[PropertyMapper\.Configuration\.TypePairConfiguration&lt;](TypePairConfiguration_TIn,TOut_.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>')[TIn](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TIn 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TIn')[,](TypePairConfiguration_TIn,TOut_.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>')[TOut](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TOut 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TOut')[&gt;](TypePairConfiguration_TIn,TOut_.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>')  
This instance for method chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [mapping](TypePairConfiguration_TIn,TOut_.MapFrom.EGYESNML8XAEXNQBK3Y0FF71C.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.MapFrom_TProp_(System.Linq.Expressions.Expression_System.Func_TOut,TProp__,System.Func_TIn,TProp_).mapping 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.MapFrom\<TProp\>\(System\.Linq\.Expressions\.Expression\<System\.Func\<TOut,TProp\>\>, System\.Func\<TIn,TProp\>\)\.mapping') is null\.

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
When the expression does not resolve to a simple property access\.