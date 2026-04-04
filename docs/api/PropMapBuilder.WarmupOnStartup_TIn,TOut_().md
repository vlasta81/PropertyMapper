## PropMapBuilder\.WarmupOnStartup\<TIn,TOut\>\(\) Method

Schedules the [TIn](PropMapBuilder.WarmupOnStartup_TIn,TOut_().md#PropertyMapper.Configuration.PropMapBuilder.WarmupOnStartup_TIn,TOut_().TIn 'PropertyMapper\.Configuration\.PropMapBuilder\.WarmupOnStartup\<TIn,TOut\>\(\)\.TIn')→[TOut](PropMapBuilder.WarmupOnStartup_TIn,TOut_().md#PropertyMapper.Configuration.PropMapBuilder.WarmupOnStartup_TIn,TOut_().TOut 'PropertyMapper\.Configuration\.PropMapBuilder\.WarmupOnStartup\<TIn,TOut\>\(\)\.TOut') delegate for pre\-compilation
during application startup when the mapper is registered via
[AddPropertyMapper\(this IServiceCollection, Action&lt;PropMapBuilder&gt;\)](ServiceCollectionExtensions.AddPropertyMapper.md#PropertyMapper.Extensions.ServiceCollectionExtensions.AddPropertyMapper(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Action_PropertyMapper.Configuration.PropMapBuilder_) 'PropertyMapper\.Extensions\.ServiceCollectionExtensions\.AddPropertyMapper\(this Microsoft\.Extensions\.DependencyInjection\.IServiceCollection, System\.Action\<PropertyMapper\.Configuration\.PropMapBuilder\>\)')\.
Multiple calls accumulate; duplicates are silently ignored at warmup time\.

```csharp
public PropertyMapper.Configuration.PropMapBuilder WarmupOnStartup<TIn,TOut>()
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.Configuration.PropMapBuilder.WarmupOnStartup_TIn,TOut_().TIn'></a>

`TIn`

Source type\.

<a name='PropertyMapper.Configuration.PropMapBuilder.WarmupOnStartup_TIn,TOut_().TOut'></a>

`TOut`

Target type \(must have a parameterless constructor\)\.

#### Returns
[PropMapBuilder](PropMapBuilder.md 'PropertyMapper\.Configuration\.PropMapBuilder')  
This builder for method chaining\.

### Example

```csharp
services.AddPropertyMapper(builder => builder
    .Configure<User, UserDto>(c => c.Ignore(x => x.Password))
    .WarmupOnStartup<User, UserDto>()
    .WarmupOnStartup<Order, OrderDto>());
```