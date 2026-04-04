## PropMapBuilder\.Configure\<TIn,TOut\>\(Action\<TypePairConfiguration\<TIn,TOut\>\>\) Method

Registers per\-property configuration for the [TIn](PropMapBuilder.Configure.43H6JG58SLOXIQAGO3WRE3Z38.md#PropertyMapper.Configuration.PropMapBuilder.Configure_TIn,TOut_(System.Action_PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut__).TIn 'PropertyMapper\.Configuration\.PropMapBuilder\.Configure\<TIn,TOut\>\(System\.Action\<PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\>\)\.TIn')→[TOut](PropMapBuilder.Configure.43H6JG58SLOXIQAGO3WRE3Z38.md#PropertyMapper.Configuration.PropMapBuilder.Configure_TIn,TOut_(System.Action_PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut__).TOut 'PropertyMapper\.Configuration\.PropMapBuilder\.Configure\<TIn,TOut\>\(System\.Action\<PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\>\)\.TOut')
type pair\. If called multiple times for the same pair, the last registration wins\.

```csharp
public PropertyMapper.Configuration.PropMapBuilder Configure<TIn,TOut>(System.Action<PropertyMapper.Configuration.TypePairConfiguration<TIn,TOut>> configure)
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.Configuration.PropMapBuilder.Configure_TIn,TOut_(System.Action_PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut__).TIn'></a>

`TIn`

Source type\.

<a name='PropertyMapper.Configuration.PropMapBuilder.Configure_TIn,TOut_(System.Action_PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut__).TOut'></a>

`TOut`

Target type \(must have a parameterless constructor\)\.
#### Parameters

<a name='PropertyMapper.Configuration.PropMapBuilder.Configure_TIn,TOut_(System.Action_PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut__).configure'></a>

`configure` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[PropertyMapper\.Configuration\.TypePairConfiguration&lt;](TypePairConfiguration_TIn,TOut_.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>')[TIn](PropMapBuilder.Configure.43H6JG58SLOXIQAGO3WRE3Z38.md#PropertyMapper.Configuration.PropMapBuilder.Configure_TIn,TOut_(System.Action_PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut__).TIn 'PropertyMapper\.Configuration\.PropMapBuilder\.Configure\<TIn,TOut\>\(System\.Action\<PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\>\)\.TIn')[,](TypePairConfiguration_TIn,TOut_.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>')[TOut](PropMapBuilder.Configure.43H6JG58SLOXIQAGO3WRE3Z38.md#PropertyMapper.Configuration.PropMapBuilder.Configure_TIn,TOut_(System.Action_PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut__).TOut 'PropertyMapper\.Configuration\.PropMapBuilder\.Configure\<TIn,TOut\>\(System\.Action\<PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\>\)\.TOut')[&gt;](TypePairConfiguration_TIn,TOut_.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

Action that receives the configuration builder and applies the desired rules\.

#### Returns
[PropMapBuilder](PropMapBuilder.md 'PropertyMapper\.Configuration\.PropMapBuilder')  
This builder for method chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [configure](PropMapBuilder.Configure.43H6JG58SLOXIQAGO3WRE3Z38.md#PropertyMapper.Configuration.PropMapBuilder.Configure_TIn,TOut_(System.Action_PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut__).configure 'PropertyMapper\.Configuration\.PropMapBuilder\.Configure\<TIn,TOut\>\(System\.Action\<PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\>\)\.configure') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.