## PropMapConfiguration\.WithGlobalIgnore\(string\[\]\) Method

Returns a new configuration where the supplied property names are excluded from every
[PropertyMapper\.Core\.MappingPlan](https://learn.microsoft.com/en-us/dotnet/api/propertymapper.core.mappingplan 'PropertyMapper\.Core\.MappingPlan'), across all type pairs\.
Useful for audit fields such as `CreatedAt`, `UpdatedAt`, or `RowVersion`
that should never flow through any mapping\.

```csharp
public PropertyMapper.Configuration.PropMapConfiguration WithGlobalIgnore(params string[] propertyNames);
```
#### Parameters

<a name='PropertyMapper.Configuration.PropMapConfiguration.WithGlobalIgnore(string[]).propertyNames'></a>

`propertyNames` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

One or more property names to ignore globally \(ordinal, case\-sensitive\)\.

#### Returns
[PropMapConfiguration](PropMapConfiguration.md 'PropertyMapper\.Configuration\.PropMapConfiguration')  
A new [PropMapConfiguration](PropMapConfiguration.md 'PropertyMapper\.Configuration\.PropMapConfiguration') with the updated value\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [propertyNames](PropMapConfiguration.WithGlobalIgnore.7WFL2XP0DZDND1DDYSW9SZ9E1.md#PropertyMapper.Configuration.PropMapConfiguration.WithGlobalIgnore(string[]).propertyNames 'PropertyMapper\.Configuration\.PropMapConfiguration\.WithGlobalIgnore\(string\[\]\)\.propertyNames') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.