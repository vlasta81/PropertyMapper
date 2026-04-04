#### [PropertyMapper](index.md 'index')
### [PropertyMapper\.Extensions](PropertyMapper.Extensions.md 'PropertyMapper\.Extensions').[ServiceCollectionExtensions](ServiceCollectionExtensions.md 'PropertyMapper\.Extensions\.ServiceCollectionExtensions')

## ServiceCollectionExtensions\.AddPropertyMapper Method

| Overloads | |
| :--- | :--- |
| [AddPropertyMapper\(this IServiceCollection\)](ServiceCollectionExtensions.AddPropertyMapper.md#PropertyMapper.Extensions.ServiceCollectionExtensions.AddPropertyMapper(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection) 'PropertyMapper\.Extensions\.ServiceCollectionExtensions\.AddPropertyMapper\(this Microsoft\.Extensions\.DependencyInjection\.IServiceCollection\)') | Registers [IPropMap](IPropMap.md 'PropertyMapper\.IPropMap') as a singleton service with default configuration\. |
| [AddPropertyMapper\(this IServiceCollection, Action&lt;PropMapBuilder&gt;\)](ServiceCollectionExtensions.AddPropertyMapper.md#PropertyMapper.Extensions.ServiceCollectionExtensions.AddPropertyMapper(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Action_PropertyMapper.Configuration.PropMapBuilder_) 'PropertyMapper\.Extensions\.ServiceCollectionExtensions\.AddPropertyMapper\(this Microsoft\.Extensions\.DependencyInjection\.IServiceCollection, System\.Action\<PropertyMapper\.Configuration\.PropMapBuilder\>\)') | Registers [IPropMap](IPropMap.md 'PropertyMapper\.IPropMap') as a frozen singleton with full builder access: global configuration and per\-type\-pair rules \([TypePairConfiguration&lt;TIn,TOut&gt;](TypePairConfiguration_TIn,TOut_.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>')\)\. This is the recommended overload for production applications\. |
| [AddPropertyMapper\(this IServiceCollection, Action&lt;PropMapBuilder&gt;, Type\[\]\)](ServiceCollectionExtensions.AddPropertyMapper.md#PropertyMapper.Extensions.ServiceCollectionExtensions.AddPropertyMapper(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Action_PropertyMapper.Configuration.PropMapBuilder_,System.Type[]) 'PropertyMapper\.Extensions\.ServiceCollectionExtensions\.AddPropertyMapper\(this Microsoft\.Extensions\.DependencyInjection\.IServiceCollection, System\.Action\<PropertyMapper\.Configuration\.PropMapBuilder\>, System\.Type\[\]\)') | Registers [IPropMap](IPropMap.md 'PropertyMapper\.IPropMap') with full builder access and schedules warmup compilation of the specified type pairs during application startup\. |
| [AddPropertyMapper\(this IServiceCollection, Func&lt;PropMapConfiguration,PropMapConfiguration&gt;\)](ServiceCollectionExtensions.AddPropertyMapper.md#PropertyMapper.Extensions.ServiceCollectionExtensions.AddPropertyMapper(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Func_PropertyMapper.Configuration.PropMapConfiguration,PropertyMapper.Configuration.PropMapConfiguration_) 'PropertyMapper\.Extensions\.ServiceCollectionExtensions\.AddPropertyMapper\(this Microsoft\.Extensions\.DependencyInjection\.IServiceCollection, System\.Func\<PropertyMapper\.Configuration\.PropMapConfiguration,PropertyMapper\.Configuration\.PropMapConfiguration\>\)') | Registers [IPropMap](IPropMap.md 'PropertyMapper\.IPropMap') as a frozen singleton with custom global configuration\. |
| [AddPropertyMapper\(this IServiceCollection, Func&lt;PropMapConfiguration,PropMapConfiguration&gt;, Type\[\]\)](ServiceCollectionExtensions.AddPropertyMapper.md#PropertyMapper.Extensions.ServiceCollectionExtensions.AddPropertyMapper(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Func_PropertyMapper.Configuration.PropMapConfiguration,PropertyMapper.Configuration.PropMapConfiguration_,System.Type[]) 'PropertyMapper\.Extensions\.ServiceCollectionExtensions\.AddPropertyMapper\(this Microsoft\.Extensions\.DependencyInjection\.IServiceCollection, System\.Func\<PropertyMapper\.Configuration\.PropMapConfiguration,PropertyMapper\.Configuration\.PropMapConfiguration\>, System\.Type\[\]\)') | Registers [IPropMap](IPropMap.md 'PropertyMapper\.IPropMap') with custom global configuration and schedules warmup compilation of the specified type pairs during application startup via [PropertyMapper\.Extensions\.PropMapWarmupService](https://learn.microsoft.com/en-us/dotnet/api/propertymapper.extensions.propmapwarmupservice 'PropertyMapper\.Extensions\.PropMapWarmupService')\. |

<a name='PropertyMapper.Extensions.ServiceCollectionExtensions.AddPropertyMapper(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection)'></a>

## ServiceCollectionExtensions\.AddPropertyMapper\(this IServiceCollection\) Method

Registers [IPropMap](IPropMap.md 'PropertyMapper\.IPropMap') as a singleton service with default configuration\.

```csharp
public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddPropertyMapper(this Microsoft.Extensions.DependencyInjection.IServiceCollection services);
```
#### Parameters

<a name='PropertyMapper.Extensions.ServiceCollectionExtensions.AddPropertyMapper(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection).services'></a>

`services` [Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection')

The [Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection') to add the service to\.

#### Returns
[Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection')  
The same [Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection') for chaining\.

<a name='PropertyMapper.Extensions.ServiceCollectionExtensions.AddPropertyMapper(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Action_PropertyMapper.Configuration.PropMapBuilder_)'></a>

## ServiceCollectionExtensions\.AddPropertyMapper\(this IServiceCollection, Action\<PropMapBuilder\>\) Method

Registers [IPropMap](IPropMap.md 'PropertyMapper\.IPropMap') as a frozen singleton with full builder access:
global configuration and per\-type\-pair rules \([TypePairConfiguration&lt;TIn,TOut&gt;](TypePairConfiguration_TIn,TOut_.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>')\)\.
This is the recommended overload for production applications\.

```csharp
public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddPropertyMapper(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, System.Action<PropertyMapper.Configuration.PropMapBuilder> configure);
```
#### Parameters

<a name='PropertyMapper.Extensions.ServiceCollectionExtensions.AddPropertyMapper(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Action_PropertyMapper.Configuration.PropMapBuilder_).services'></a>

`services` [Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection')

The [Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection') to add the service to\.

<a name='PropertyMapper.Extensions.ServiceCollectionExtensions.AddPropertyMapper(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Action_PropertyMapper.Configuration.PropMapBuilder_).configure'></a>

`configure` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[PropMapBuilder](PropMapBuilder.md 'PropertyMapper\.Configuration\.PropMapBuilder')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

Action that receives a [PropMapBuilder](PropMapBuilder.md 'PropertyMapper\.Configuration\.PropMapBuilder') and applies global configuration
and/or per\-type\-pair rules before [Build\(\)](PropMapBuilder.Build().md 'PropertyMapper\.Configuration\.PropMapBuilder\.Build\(\)') is called\.

#### Returns
[Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection')  
The same [Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection') for chaining\.

### Example

```csharp
services.AddPropertyMapper(builder => builder
    .WithConfiguration(cfg => cfg.WithMaxDepth(64))
    .Configure<User, UserDto>(c => c.Ignore(x => x.Password))
    .Configure<Order, OrderDto>(c => c.MapFrom(x => x.Total, s => s.Subtotal + s.Tax)));
```

<a name='PropertyMapper.Extensions.ServiceCollectionExtensions.AddPropertyMapper(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Action_PropertyMapper.Configuration.PropMapBuilder_,System.Type[])'></a>

## ServiceCollectionExtensions\.AddPropertyMapper\(this IServiceCollection, Action\<PropMapBuilder\>, Type\[\]\) Method

Registers [IPropMap](IPropMap.md 'PropertyMapper\.IPropMap') with full builder access and schedules warmup compilation
of the specified type pairs during application startup\.

```csharp
public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddPropertyMapper(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, System.Action<PropertyMapper.Configuration.PropMapBuilder> configure, params System.Type[] warmupTypePairs);
```
#### Parameters

<a name='PropertyMapper.Extensions.ServiceCollectionExtensions.AddPropertyMapper(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Action_PropertyMapper.Configuration.PropMapBuilder_,System.Type[]).services'></a>

`services` [Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection')

The [Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection') to add the service to\.

<a name='PropertyMapper.Extensions.ServiceCollectionExtensions.AddPropertyMapper(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Action_PropertyMapper.Configuration.PropMapBuilder_,System.Type[]).configure'></a>

`configure` [System\.Action&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')[PropMapBuilder](PropMapBuilder.md 'PropertyMapper\.Configuration\.PropMapBuilder')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.action-1 'System\.Action\`1')

Action that configures the [PropMapBuilder](PropMapBuilder.md 'PropertyMapper\.Configuration\.PropMapBuilder')\.

<a name='PropertyMapper.Extensions.ServiceCollectionExtensions.AddPropertyMapper(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Action_PropertyMapper.Configuration.PropMapBuilder_,System.Type[]).warmupTypePairs'></a>

`warmupTypePairs` [System\.Type](https://learn.microsoft.com/en-us/dotnet/api/system.type 'System\.Type')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

An even\-length list of types in alternating source/target order\.

#### Returns
[Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection')  
The same [Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection') for chaining\.

<a name='PropertyMapper.Extensions.ServiceCollectionExtensions.AddPropertyMapper(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Func_PropertyMapper.Configuration.PropMapConfiguration,PropertyMapper.Configuration.PropMapConfiguration_)'></a>

## ServiceCollectionExtensions\.AddPropertyMapper\(this IServiceCollection, Func\<PropMapConfiguration,PropMapConfiguration\>\) Method

Registers [IPropMap](IPropMap.md 'PropertyMapper\.IPropMap') as a frozen singleton with custom global configuration\.

```csharp
public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddPropertyMapper(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, System.Func<PropertyMapper.Configuration.PropMapConfiguration,PropertyMapper.Configuration.PropMapConfiguration> configure);
```
#### Parameters

<a name='PropertyMapper.Extensions.ServiceCollectionExtensions.AddPropertyMapper(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Func_PropertyMapper.Configuration.PropMapConfiguration,PropertyMapper.Configuration.PropMapConfiguration_).services'></a>

`services` [Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection')

The [Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection') to add the service to\.

<a name='PropertyMapper.Extensions.ServiceCollectionExtensions.AddPropertyMapper(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Func_PropertyMapper.Configuration.PropMapConfiguration,PropertyMapper.Configuration.PropMapConfiguration_).configure'></a>

`configure` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[PropMapConfiguration](PropMapConfiguration.md 'PropertyMapper\.Configuration\.PropMapConfiguration')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[PropMapConfiguration](PropMapConfiguration.md 'PropertyMapper\.Configuration\.PropMapConfiguration')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A factory function that receives the default [PropMapConfiguration](PropMapConfiguration.md 'PropertyMapper\.Configuration\.PropMapConfiguration') and returns
the configured instance\. Use the fluent API or object\-initialiser syntax\.

#### Returns
[Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection')  
The same [Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection') for chaining\.

<a name='PropertyMapper.Extensions.ServiceCollectionExtensions.AddPropertyMapper(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Func_PropertyMapper.Configuration.PropMapConfiguration,PropertyMapper.Configuration.PropMapConfiguration_,System.Type[])'></a>

## ServiceCollectionExtensions\.AddPropertyMapper\(this IServiceCollection, Func\<PropMapConfiguration,PropMapConfiguration\>, Type\[\]\) Method

Registers [IPropMap](IPropMap.md 'PropertyMapper\.IPropMap') with custom global configuration and schedules warmup
compilation of the specified type pairs during application startup via
[PropertyMapper\.Extensions\.PropMapWarmupService](https://learn.microsoft.com/en-us/dotnet/api/propertymapper.extensions.propmapwarmupservice 'PropertyMapper\.Extensions\.PropMapWarmupService')\.

```csharp
public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddPropertyMapper(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, System.Func<PropertyMapper.Configuration.PropMapConfiguration,PropertyMapper.Configuration.PropMapConfiguration> configure, params System.Type[] warmupTypePairs);
```
#### Parameters

<a name='PropertyMapper.Extensions.ServiceCollectionExtensions.AddPropertyMapper(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Func_PropertyMapper.Configuration.PropMapConfiguration,PropertyMapper.Configuration.PropMapConfiguration_,System.Type[]).services'></a>

`services` [Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection')

The [Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection') to add the service to\.

<a name='PropertyMapper.Extensions.ServiceCollectionExtensions.AddPropertyMapper(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Func_PropertyMapper.Configuration.PropMapConfiguration,PropertyMapper.Configuration.PropMapConfiguration_,System.Type[]).configure'></a>

`configure` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[PropMapConfiguration](PropMapConfiguration.md 'PropertyMapper\.Configuration\.PropMapConfiguration')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[PropMapConfiguration](PropMapConfiguration.md 'PropertyMapper\.Configuration\.PropMapConfiguration')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A factory function that produces the active [PropMapConfiguration](PropMapConfiguration.md 'PropertyMapper\.Configuration\.PropMapConfiguration')\.

<a name='PropertyMapper.Extensions.ServiceCollectionExtensions.AddPropertyMapper(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Func_PropertyMapper.Configuration.PropMapConfiguration,PropertyMapper.Configuration.PropMapConfiguration_,System.Type[]).warmupTypePairs'></a>

`warmupTypePairs` [System\.Type](https://learn.microsoft.com/en-us/dotnet/api/system.type 'System\.Type')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

An even\-length list of types in alternating source/target order
\(e\.g\. `typeof(Foo), typeof(FooDto), typeof(Bar), typeof(BarDto)`\)\.

#### Returns
[Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection')  
The same [Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection') for chaining\.

---
Generated by [DefaultDocumentation](https://github.com/Doraku/DefaultDocumentation 'https://github\.com/Doraku/DefaultDocumentation')