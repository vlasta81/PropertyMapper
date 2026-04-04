## PropMapBuilder\.Build\(\) Method

Creates a frozen, immutable [PropMap](PropMap.md 'PropertyMapper\.PropMap') from the current builder state\.
The builder's internal state is snapshotted at this point; subsequent calls to
[Configure&lt;TIn,TOut&gt;\(Action&lt;TypePairConfiguration&lt;TIn,TOut&gt;&gt;\)](PropMapBuilder.Configure.43H6JG58SLOXIQAGO3WRE3Z38.md 'PropertyMapper\.Configuration\.PropMapBuilder\.Configure\<TIn,TOut\>\(System\.Action\<PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\>\)') or [WithConfiguration\(PropMapConfiguration\)](PropMapBuilder.WithConfiguration.md#PropertyMapper.Configuration.PropMapBuilder.WithConfiguration(PropertyMapper.Configuration.PropMapConfiguration) 'PropertyMapper\.Configuration\.PropMapBuilder\.WithConfiguration\(PropertyMapper\.Configuration\.PropMapConfiguration\)')
on this builder do not affect the returned instance\.

```csharp
public PropertyMapper.PropMap Build();
```

#### Returns
[PropMap](PropMap.md 'PropertyMapper\.PropMap')  
A new frozen [PropMap](PropMap.md 'PropertyMapper\.PropMap') ready for use as a singleton\.