## TypePairConfiguration\<TIn,TOut\>\.Ignore\<TProp\>\(Expression\<Func\<TOut,TProp\>\>\) Method

Excludes the specified target property from the IL\-generated mapping\.
The property on [TOut](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TOut 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TOut') will retain its default value after mapping\.

```csharp
public PropertyMapper.Configuration.TypePairConfiguration<TIn,TOut> Ignore<TProp>(System.Linq.Expressions.Expression<System.Func<TOut,TProp>> property);
```
#### Type parameters

<a name='PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.Ignore_TProp_(System.Linq.Expressions.Expression_System.Func_TOut,TProp__).TProp'></a>

`TProp`

Property type\.
#### Parameters

<a name='PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.Ignore_TProp_(System.Linq.Expressions.Expression_System.Func_TOut,TProp__).property'></a>

`property` [System\.Linq\.Expressions\.Expression&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression-1 'System\.Linq\.Expressions\.Expression\`1')[System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[TOut](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TOut 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TOut')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[TProp](TypePairConfiguration_TIn,TOut_.Ignore.47Y5JYZI9VSIMEAAQAG2ALEBA.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.Ignore_TProp_(System.Linq.Expressions.Expression_System.Func_TOut,TProp__).TProp 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.Ignore\<TProp\>\(System\.Linq\.Expressions\.Expression\<System\.Func\<TOut,TProp\>\>\)\.TProp')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression-1 'System\.Linq\.Expressions\.Expression\`1')

Expression selecting the target property to ignore \(e\.g\. `x => x.Name`\)\.

#### Returns
[PropertyMapper\.Configuration\.TypePairConfiguration&lt;](TypePairConfiguration_TIn,TOut_.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>')[TIn](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TIn 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TIn')[,](TypePairConfiguration_TIn,TOut_.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>')[TOut](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TOut 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TOut')[&gt;](TypePairConfiguration_TIn,TOut_.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>')  
This instance for method chaining\.

#### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
When the expression does not resolve to a simple property access\.