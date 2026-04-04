## TypePairConfiguration\<TIn,TOut\>\.AfterMap\(Action\<TIn,TOut\>\) Method

Registers a post\-map hook that is invoked after all property copies and custom
[MapFrom&lt;TProp&gt;\(Expression&lt;Func&lt;TOut,TProp&gt;&gt;, Func&lt;TIn,TProp&gt;\)](TypePairConfiguration_TIn,TOut_.MapFrom.EGYESNML8XAEXNQBK3Y0FF71C.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.MapFrom\<TProp\>\(System\.Linq\.Expressions\.Expression\<System\.Func\<TOut,TProp\>\>, System\.Func\<TIn,TProp\>\)') setters
have completed\. Use it to apply cross\-property logic or validation on the finished target\.

```csharp
public PropertyMapper.Configuration.TypePairConfiguration<TIn,TOut> AfterMap(System.Action<TIn,TOut> action);
```
#### Parameters

<a name='PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.AfterMap(System.Action_TIn,TOut_).action'></a>

`action` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')[TIn](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TIn 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TIn')[,](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')[TOut](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TOut 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-2 'System\.Action\`2')

Action receiving the source and the fully populated target\.

#### Returns
[PropertyMapper\.Configuration\.TypePairConfiguration&lt;](TypePairConfiguration_TIn,TOut_.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>')[TIn](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TIn 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TIn')[,](TypePairConfiguration_TIn,TOut_.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>')[TOut](TypePairConfiguration_TIn,TOut_.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.TOut 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.TOut')[&gt;](TypePairConfiguration_TIn,TOut_.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>')  
This instance for method chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [action](TypePairConfiguration_TIn,TOut_.AfterMap.61IIN053O14L03C04MEAJQ8CB.md#PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut_.AfterMap(System.Action_TIn,TOut_).action 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.AfterMap\(System\.Action\<TIn,TOut\>\)\.action') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.