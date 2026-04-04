## MappingValidationResult\.UnmappedTargetProperties Property

Names of writable target properties that have no matching source property\.
Empty when [IsValid](MappingValidationResult.IsValid.md 'PropertyMapper\.MappingValidationResult\.IsValid') is [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

```csharp
public string[] UnmappedTargetProperties { get; init; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')