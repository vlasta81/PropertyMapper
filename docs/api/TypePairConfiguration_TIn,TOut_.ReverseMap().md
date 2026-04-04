## TypePairConfiguration\<TIn,TOut\>\.ReverseMap\(\) Method

Instructs [Build\(\)](PropMapBuilder.Build().md 'PropertyMapper\.Configuration\.PropMapBuilder\.Build\(\)') to automatically register a
[TOut](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TOut 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TOut')→[TIn](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TIn 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TIn') mapping alongside this
[TIn](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TIn 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TIn')→[TOut](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TOut 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TOut') registration\.
Only name\-matched properties and [Ignore&lt;TProp&gt;\(Expression&lt;Func&lt;TOut,TProp&gt;&gt;\)](TypePairConfiguration_TIn,TOut_.Ignore.47Y5JYZI9VSIMEAAQAG2ALEBA.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.Ignore\<TProp\>\(System\.Linq\.Expressions\.Expression\<System\.Func\<TOut,TProp\>\>\)') exclusions are mirrored;
[MapFrom&lt;TProp&gt;\(Expression&lt;Func&lt;TOut,TProp&gt;&gt;, Func&lt;TIn,TProp&gt;\)](TypePairConfiguration_TIn,TOut_.MapFrom.EGYESNML8XAEXNQBK3Y0FF71C.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.MapFrom\<TProp\>\(System\.Linq\.Expressions\.Expression\<System\.Func\<TOut,TProp\>\>, System\.Func\<TIn,TProp\>\)') factories
cannot be automatically inverted\.

```csharp
public PropertyMapper.Configuration.TypePairConfiguration<TIn,TOut> ReverseMap();
```

#### Returns
[PropertyMapper\.Configuration\.TypePairConfiguration&lt;](TypePairConfiguration_TIn,TOut_.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>')[TIn](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TIn 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TIn')[,](TypePairConfiguration_TIn,TOut_.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>')[TOut](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TOut 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TOut')[&gt;](TypePairConfiguration_TIn,TOut_.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>')  
This instance for method chaining\.