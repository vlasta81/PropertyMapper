## PropMapConfiguration Class

Controls how [PropMap](PropMap.md 'PropertyMapper\.PropMap') resolves and executes property mappings\.
Use object\-initialiser syntax or the fluent `WithX()` API to customise behaviour\.

```csharp
public sealed record PropMapConfiguration : System.IEquatable<PropertyMapper.Configuration.PropMapConfiguration>
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; PropMapConfiguration

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[PropMapConfiguration](PropMapConfiguration.md 'PropertyMapper\.Configuration\.PropMapConfiguration')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Properties | |
| :--- | :--- |
| [GlobalIgnoredProperties](PropMapConfiguration.GlobalIgnoredProperties.md 'PropertyMapper\.Configuration\.PropMapConfiguration\.GlobalIgnoredProperties') | Property names that are globally excluded from every [PropertyMapper\.Core\.MappingPlan](https://learn.microsoft.com/en-us/dotnet/api/propertymapper.core.mappingplan 'PropertyMapper\.Core\.MappingPlan'), regardless of type pair\. Set via [WithGlobalIgnore\(string\[\]\)](PropMapConfiguration.WithGlobalIgnore.7WFL2XP0DZDND1DDYSW9SZ9E1.md 'PropertyMapper\.Configuration\.PropMapConfiguration\.WithGlobalIgnore\(string\[\]\)')\. [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null') means no global ignores are active \(zero\-overhead hot path\)\. |
| [MaxMappingDepth](PropMapConfiguration.MaxMappingDepth.md 'PropertyMapper\.Configuration\.PropMapConfiguration\.MaxMappingDepth') | Maximum depth for nested object mapping \(prevents stack overflow\)\. Default: 32\. |
| [ThrowOnUnmappedProperties](PropMapConfiguration.ThrowOnUnmappedProperties.md 'PropertyMapper\.Configuration\.PropMapConfiguration\.ThrowOnUnmappedProperties') | Throws exception when source has properties not present on target\. Default: false\. |

| Methods | |
| :--- | :--- |
| [ThrowOnUnmapped\(bool\)](PropMapConfiguration.ThrowOnUnmapped.2N9MG22NSSCJSCENNVHRUY6D8.md 'PropertyMapper\.Configuration\.PropMapConfiguration\.ThrowOnUnmapped\(bool\)') | Returns a new configuration with [ThrowOnUnmappedProperties](PropMapConfiguration.ThrowOnUnmappedProperties.md 'PropertyMapper\.Configuration\.PropMapConfiguration\.ThrowOnUnmappedProperties') set to [enabled](PropMapConfiguration.ThrowOnUnmapped.2N9MG22NSSCJSCENNVHRUY6D8.md#PropertyMapper.Configuration.PropMapConfiguration.ThrowOnUnmapped(bool).enabled 'PropertyMapper\.Configuration\.PropMapConfiguration\.ThrowOnUnmapped\(bool\)\.enabled')\. |
| [WithGlobalIgnore\(string\[\]\)](PropMapConfiguration.WithGlobalIgnore.7WFL2XP0DZDND1DDYSW9SZ9E1.md 'PropertyMapper\.Configuration\.PropMapConfiguration\.WithGlobalIgnore\(string\[\]\)') | Returns a new configuration where the supplied property names are excluded from every [PropertyMapper\.Core\.MappingPlan](https://learn.microsoft.com/en-us/dotnet/api/propertymapper.core.mappingplan 'PropertyMapper\.Core\.MappingPlan'), across all type pairs\. Useful for audit fields such as `CreatedAt`, `UpdatedAt`, or `RowVersion` that should never flow through any mapping\. |
| [WithMaxDepth\(int\)](PropMapConfiguration.WithMaxDepth.5BI3XF0D0GFDMLM5G2XKXRVF.md 'PropertyMapper\.Configuration\.PropMapConfiguration\.WithMaxDepth\(int\)') | Returns a new configuration with [MaxMappingDepth](PropMapConfiguration.MaxMappingDepth.md 'PropertyMapper\.Configuration\.PropMapConfiguration\.MaxMappingDepth') set to [depth](PropMapConfiguration.WithMaxDepth.5BI3XF0D0GFDMLM5G2XKXRVF.md#PropertyMapper.Configuration.PropMapConfiguration.WithMaxDepth(int).depth 'PropertyMapper\.Configuration\.PropMapConfiguration\.WithMaxDepth\(int\)\.depth')\. |
