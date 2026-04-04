## PropMapConfiguration\.GlobalIgnoredProperties Property

Property names that are globally excluded from every [PropertyMapper\.Core\.MappingPlan](https://learn.microsoft.com/en-us/dotnet/api/propertymapper.core.mappingplan 'PropertyMapper\.Core\.MappingPlan'),
regardless of type pair\. Set via [WithGlobalIgnore\(string\[\]\)](PropMapConfiguration.WithGlobalIgnore.7WFL2XP0DZDND1DDYSW9SZ9E1.md 'PropertyMapper\.Configuration\.PropMapConfiguration\.WithGlobalIgnore\(string\[\]\)')\.
[null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null') means no global ignores are active \(zero\-overhead hot path\)\.

```csharp
public System.Collections.Generic.IReadOnlySet<string>? GlobalIgnoredProperties { get; init; }
```

#### Property Value
[System\.Collections\.Generic\.IReadOnlySet&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlyset-1 'System\.Collections\.Generic\.IReadOnlySet\`1')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ireadonlyset-1 'System\.Collections\.Generic\.IReadOnlySet\`1')