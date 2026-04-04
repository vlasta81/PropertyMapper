## PropMapConfiguration\.ThrowOnUnmapped\(bool\) Method

Returns a new configuration with [ThrowOnUnmappedProperties](PropMapConfiguration.ThrowOnUnmappedProperties.md 'PropertyMapper\.Configuration\.PropMapConfiguration\.ThrowOnUnmappedProperties') set to [enabled](PropMapConfiguration.ThrowOnUnmapped.2N9MG22NSSCJSCENNVHRUY6D8.md#PropertyMapper.Configuration.PropMapConfiguration.ThrowOnUnmapped(bool).enabled 'PropertyMapper\.Configuration\.PropMapConfiguration\.ThrowOnUnmapped\(bool\)\.enabled')\.

```csharp
public PropertyMapper.Configuration.PropMapConfiguration ThrowOnUnmapped(bool enabled=true);
```
#### Parameters

<a name='PropertyMapper.Configuration.PropMapConfiguration.ThrowOnUnmapped(bool).enabled'></a>

`enabled` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

When [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool'), mapping throws if any source property has no matching target property\.

#### Returns
[PropMapConfiguration](PropMapConfiguration.md 'PropertyMapper\.Configuration\.PropMapConfiguration')  
A new [PropMapConfiguration](PropMapConfiguration.md 'PropertyMapper\.Configuration\.PropMapConfiguration') with the updated value\.