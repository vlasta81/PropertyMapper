## PropMapBuilder Class

Fluent builder that assembles a frozen, thread\-safe [PropMap](PropMap.md 'PropertyMapper\.PropMap') singleton\.

```csharp
public sealed class PropMapBuilder
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; PropMapBuilder

### Example

```csharp
PropMap mapper = new PropMapBuilder()
    .WithConfiguration(cfg => cfg.WithMaxDepth(64))
    .Configure<User, UserDto>(c => c.Ignore(x => x.Password))
    .Configure<Order, OrderDto>(c => c.MapFrom(x => x.Total, src => src.Subtotal + src.Tax))
    .Build();
```

### Remarks

The [PropMap](PropMap.md 'PropertyMapper\.PropMap') produced by [Build\(\)](PropMapBuilder.Build().md 'PropertyMapper\.Configuration\.PropMapBuilder\.Build\(\)') is <em>frozen</em>: all
type-pair configurations are applied at build time and runtime calls to
`Configure` throw [System\.InvalidOperationException](https://learn.microsoft.com/en-us/dotnet/api/system.invalidoperationexception 'System\.InvalidOperationException').
This eliminates the cross-request configuration interference that would occur if
`Configure` were called on a shared singleton during request processing.

For dependency injection, prefer the
[AddPropertyMapper\(this IServiceCollection, Action&lt;PropMapBuilder&gt;\)](ServiceCollectionExtensions.AddPropertyMapper.md#PropertyMapper.Extensions.ServiceCollectionExtensions.AddPropertyMapper(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Action_PropertyMapper.Configuration.PropMapBuilder_) 'PropertyMapper\.Extensions\.ServiceCollectionExtensions\.AddPropertyMapper\(this Microsoft\.Extensions\.DependencyInjection\.IServiceCollection, System\.Action\<PropertyMapper\.Configuration\.PropMapBuilder\>\)')
overload which uses this builder internally.

| Methods | |
| :--- | :--- |
| [Build\(\)](PropMapBuilder.Build().md 'PropertyMapper\.Configuration\.PropMapBuilder\.Build\(\)') | Creates a frozen, immutable [PropMap](PropMap.md 'PropertyMapper\.PropMap') from the current builder state\. The builder's internal state is snapshotted at this point; subsequent calls to [Configure&lt;TIn,TOut&gt;\(Action&lt;TypePairConfiguration&lt;TIn,TOut&gt;&gt;\)](PropMapBuilder.Configure.43H6JG58SLOXIQAGO3WRE3Z38.md 'PropertyMapper\.Configuration\.PropMapBuilder\.Configure\<TIn,TOut\>\(System\.Action\<PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\>\)') or [WithConfiguration\(PropMapConfiguration\)](PropMapBuilder.WithConfiguration.md#PropertyMapper.Configuration.PropMapBuilder.WithConfiguration(PropertyMapper.Configuration.PropMapConfiguration) 'PropertyMapper\.Configuration\.PropMapBuilder\.WithConfiguration\(PropertyMapper\.Configuration\.PropMapConfiguration\)') on this builder do not affect the returned instance\. |
| [Configure&lt;TIn,TOut&gt;\(Action&lt;TypePairConfiguration&lt;TIn,TOut&gt;&gt;\)](PropMapBuilder.Configure.43H6JG58SLOXIQAGO3WRE3Z38.md 'PropertyMapper\.Configuration\.PropMapBuilder\.Configure\<TIn,TOut\>\(System\.Action\<PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\>\)') | Registers per\-property configuration for the [TIn](PropMapBuilder.Configure.43H6JG58SLOXIQAGO3WRE3Z38.md#PropertyMapper.Configuration.PropMapBuilder.Configure_TIn,TOut_(System.Action_PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut__).TIn 'PropertyMapper\.Configuration\.PropMapBuilder\.Configure\<TIn,TOut\>\(System\.Action\<PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\>\)\.TIn')→[TOut](PropMapBuilder.Configure.43H6JG58SLOXIQAGO3WRE3Z38.md#PropertyMapper.Configuration.PropMapBuilder.Configure_TIn,TOut_(System.Action_PropertyMapper.Configuration.TypePairConfiguration_TIn,TOut__).TOut 'PropertyMapper\.Configuration\.PropMapBuilder\.Configure\<TIn,TOut\>\(System\.Action\<PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\>\)\.TOut') type pair\. If called multiple times for the same pair, the last registration wins\. |
| [WarmupOnStartup&lt;TIn,TOut&gt;\(\)](PropMapBuilder.WarmupOnStartup_TIn,TOut_().md 'PropertyMapper\.Configuration\.PropMapBuilder\.WarmupOnStartup\<TIn,TOut\>\(\)') | Schedules the [TIn](PropMapBuilder.WarmupOnStartup_TIn,TOut_().md#PropertyMapper.Configuration.PropMapBuilder.WarmupOnStartup_TIn,TOut_().TIn 'PropertyMapper\.Configuration\.PropMapBuilder\.WarmupOnStartup\<TIn,TOut\>\(\)\.TIn')→[TOut](PropMapBuilder.WarmupOnStartup_TIn,TOut_().md#PropertyMapper.Configuration.PropMapBuilder.WarmupOnStartup_TIn,TOut_().TOut 'PropertyMapper\.Configuration\.PropMapBuilder\.WarmupOnStartup\<TIn,TOut\>\(\)\.TOut') delegate for pre\-compilation during application startup when the mapper is registered via [AddPropertyMapper\(this IServiceCollection, Action&lt;PropMapBuilder&gt;\)](ServiceCollectionExtensions.AddPropertyMapper.md#PropertyMapper.Extensions.ServiceCollectionExtensions.AddPropertyMapper(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Action_PropertyMapper.Configuration.PropMapBuilder_) 'PropertyMapper\.Extensions\.ServiceCollectionExtensions\.AddPropertyMapper\(this Microsoft\.Extensions\.DependencyInjection\.IServiceCollection, System\.Action\<PropertyMapper\.Configuration\.PropMapBuilder\>\)')\. Multiple calls accumulate; duplicates are silently ignored at warmup time\. |
| [WithConfiguration\(PropMapConfiguration\)](PropMapBuilder.WithConfiguration.md#PropertyMapper.Configuration.PropMapBuilder.WithConfiguration(PropertyMapper.Configuration.PropMapConfiguration) 'PropertyMapper\.Configuration\.PropMapBuilder\.WithConfiguration\(PropertyMapper\.Configuration\.PropMapConfiguration\)') | Sets the global [PropMapConfiguration](PropMapConfiguration.md 'PropertyMapper\.Configuration\.PropMapConfiguration') applied to all type pairs\. Replaces any previously set value\. |
| [WithConfiguration\(Func&lt;PropMapConfiguration,PropMapConfiguration&gt;\)](PropMapBuilder.WithConfiguration.md#PropertyMapper.Configuration.PropMapBuilder.WithConfiguration(System.Func_PropertyMapper.Configuration.PropMapConfiguration,PropertyMapper.Configuration.PropMapConfiguration_) 'PropertyMapper\.Configuration\.PropMapBuilder\.WithConfiguration\(System\.Func\<PropertyMapper\.Configuration\.PropMapConfiguration,PropertyMapper\.Configuration\.PropMapConfiguration\>\)') | Sets the global [PropMapConfiguration](PropMapConfiguration.md 'PropertyMapper\.Configuration\.PropMapConfiguration') via a factory applied to the default instance\. |
| [WithGlobalIgnore\(string\[\]\)](PropMapBuilder.WithGlobalIgnore.M0NBMT6PCNVJ1AUD1URYM869B.md 'PropertyMapper\.Configuration\.PropMapBuilder\.WithGlobalIgnore\(string\[\]\)') | Excludes the supplied property names from every mapping plan built by this instance, across all type pairs\. Useful for audit fields such as `CreatedAt`, `UpdatedAt`, or `RowVersion` that should never flow through any mapping\. Equivalent to calling [WithGlobalIgnore\(string\[\]\)](PropMapConfiguration.WithGlobalIgnore.7WFL2XP0DZDND1DDYSW9SZ9E1.md 'PropertyMapper\.Configuration\.PropMapConfiguration\.WithGlobalIgnore\(string\[\]\)') on the global configuration\. |
