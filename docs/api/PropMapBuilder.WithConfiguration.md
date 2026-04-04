#### [PropertyMapper](index.md 'index')
### [PropertyMapper\.Configuration](PropertyMapper.Configuration.md 'PropertyMapper\.Configuration').[PropMapBuilder](PropMapBuilder.md 'PropertyMapper\.Configuration\.PropMapBuilder')

## PropMapBuilder\.WithConfiguration Method

| Overloads | |
| :--- | :--- |
| [WithConfiguration\(PropMapConfiguration\)](PropMapBuilder.WithConfiguration.md#PropertyMapper.Configuration.PropMapBuilder.WithConfiguration(PropertyMapper.Configuration.PropMapConfiguration) 'PropertyMapper\.Configuration\.PropMapBuilder\.WithConfiguration\(PropertyMapper\.Configuration\.PropMapConfiguration\)') | Sets the global [PropMapConfiguration](PropMapConfiguration.md 'PropertyMapper\.Configuration\.PropMapConfiguration') applied to all type pairs\. Replaces any previously set value\. |
| [WithConfiguration\(Func&lt;PropMapConfiguration,PropMapConfiguration&gt;\)](PropMapBuilder.WithConfiguration.md#PropertyMapper.Configuration.PropMapBuilder.WithConfiguration(System.Func_PropertyMapper.Configuration.PropMapConfiguration,PropertyMapper.Configuration.PropMapConfiguration_) 'PropertyMapper\.Configuration\.PropMapBuilder\.WithConfiguration\(System\.Func\<PropertyMapper\.Configuration\.PropMapConfiguration,PropertyMapper\.Configuration\.PropMapConfiguration\>\)') | Sets the global [PropMapConfiguration](PropMapConfiguration.md 'PropertyMapper\.Configuration\.PropMapConfiguration') via a factory applied to the default instance\. |

<a name='PropertyMapper.Configuration.PropMapBuilder.WithConfiguration(PropertyMapper.Configuration.PropMapConfiguration)'></a>

## PropMapBuilder\.WithConfiguration\(PropMapConfiguration\) Method

Sets the global [PropMapConfiguration](PropMapConfiguration.md 'PropertyMapper\.Configuration\.PropMapConfiguration') applied to all type pairs\.
Replaces any previously set value\.

```csharp
public PropertyMapper.Configuration.PropMapBuilder WithConfiguration(PropertyMapper.Configuration.PropMapConfiguration configuration);
```
#### Parameters

<a name='PropertyMapper.Configuration.PropMapBuilder.WithConfiguration(PropertyMapper.Configuration.PropMapConfiguration).configuration'></a>

`configuration` [PropMapConfiguration](PropMapConfiguration.md 'PropertyMapper\.Configuration\.PropMapConfiguration')

The configuration to use\.

#### Returns
[PropMapBuilder](PropMapBuilder.md 'PropertyMapper\.Configuration\.PropMapBuilder')  
This builder for method chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [configuration](PropMapBuilder.md#PropertyMapper.Configuration.PropMapBuilder.WithConfiguration(PropertyMapper.Configuration.PropMapConfiguration).configuration 'PropertyMapper\.Configuration\.PropMapBuilder\.WithConfiguration\(PropertyMapper\.Configuration\.PropMapConfiguration\)\.configuration') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

<a name='PropertyMapper.Configuration.PropMapBuilder.WithConfiguration(System.Func_PropertyMapper.Configuration.PropMapConfiguration,PropertyMapper.Configuration.PropMapConfiguration_)'></a>

## PropMapBuilder\.WithConfiguration\(Func\<PropMapConfiguration,PropMapConfiguration\>\) Method

Sets the global [PropMapConfiguration](PropMapConfiguration.md 'PropertyMapper\.Configuration\.PropMapConfiguration') via a factory applied to the default instance\.

```csharp
public PropertyMapper.Configuration.PropMapBuilder WithConfiguration(System.Func<PropertyMapper.Configuration.PropMapConfiguration,PropertyMapper.Configuration.PropMapConfiguration> configure);
```
#### Parameters

<a name='PropertyMapper.Configuration.PropMapBuilder.WithConfiguration(System.Func_PropertyMapper.Configuration.PropMapConfiguration,PropertyMapper.Configuration.PropMapConfiguration_).configure'></a>

`configure` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[PropMapConfiguration](PropMapConfiguration.md 'PropertyMapper\.Configuration\.PropMapConfiguration')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[PropMapConfiguration](PropMapConfiguration.md 'PropertyMapper\.Configuration\.PropMapConfiguration')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A factory that receives the default [PropMapConfiguration](PropMapConfiguration.md 'PropertyMapper\.Configuration\.PropMapConfiguration') and returns
the customised version\. Use the fluent `WithX()` API or object\-initialiser syntax\.

#### Returns
[PropMapBuilder](PropMapBuilder.md 'PropertyMapper\.Configuration\.PropMapBuilder')  
This builder for method chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [configure](PropMapBuilder.md#PropertyMapper.Configuration.PropMapBuilder.WithConfiguration(System.Func_PropertyMapper.Configuration.PropMapConfiguration,PropertyMapper.Configuration.PropMapConfiguration_).configure 'PropertyMapper\.Configuration\.PropMapBuilder\.WithConfiguration\(System\.Func\<PropertyMapper\.Configuration\.PropMapConfiguration,PropertyMapper\.Configuration\.PropMapConfiguration\>\)\.configure') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

---
Generated by [DefaultDocumentation](https://github.com/Doraku/DefaultDocumentation 'https://github\.com/Doraku/DefaultDocumentation')