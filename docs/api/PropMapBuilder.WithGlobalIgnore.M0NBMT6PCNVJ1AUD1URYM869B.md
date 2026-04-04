## PropMapBuilder\.WithGlobalIgnore\(string\[\]\) Method

Excludes the supplied property names from every mapping plan built by this instance,
across all type pairs\. Useful for audit fields such as `CreatedAt`, `UpdatedAt`,
or `RowVersion` that should never flow through any mapping\.
Equivalent to calling [WithGlobalIgnore\(string\[\]\)](PropMapConfiguration.WithGlobalIgnore.7WFL2XP0DZDND1DDYSW9SZ9E1.md 'PropertyMapper\.Configuration\.PropMapConfiguration\.WithGlobalIgnore\(string\[\]\)') on
the global configuration\.

```csharp
public PropertyMapper.Configuration.PropMapBuilder WithGlobalIgnore(params string[] propertyNames);
```
#### Parameters

<a name='PropertyMapper.Configuration.PropMapBuilder.WithGlobalIgnore(string[]).propertyNames'></a>

`propertyNames` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

One or more property names to ignore \(ordinal, case\-sensitive\)\.

#### Returns
[PropMapBuilder](PropMapBuilder.md 'PropertyMapper\.Configuration\.PropMapBuilder')  
This builder for method chaining\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [propertyNames](PropMapBuilder.WithGlobalIgnore.M0NBMT6PCNVJ1AUD1URYM869B.md#PropertyMapper.Configuration.PropMapBuilder.WithGlobalIgnore(string[]).propertyNames 'PropertyMapper\.Configuration\.PropMapBuilder\.WithGlobalIgnore\(string\[\]\)\.propertyNames') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.