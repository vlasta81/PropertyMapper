## MappingValidationResult Struct

Result of a [Validate&lt;TIn,TOut&gt;\(\)](PropMap.Validate_TIn,TOut_().md 'PropertyMapper\.PropMap\.Validate\<TIn,TOut\>\(\)') call\.

```csharp
public readonly record struct MappingValidationResult : System.IEquatable<PropertyMapper.MappingValidationResult>
```

Implements [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[MappingValidationResult](MappingValidationResult.md 'PropertyMapper\.MappingValidationResult')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')

| Constructors | |
| :--- | :--- |
| [MappingValidationResult\(bool, string\[\]\)](MappingValidationResult..ctor.Z43LCYNN25C2O0BGJJHURPR14.md 'PropertyMapper\.MappingValidationResult\.MappingValidationResult\(bool, string\[\]\)') | Result of a [Validate&lt;TIn,TOut&gt;\(\)](PropMap.Validate_TIn,TOut_().md 'PropertyMapper\.PropMap\.Validate\<TIn,TOut\>\(\)') call\. |

| Properties | |
| :--- | :--- |
| [IsValid](MappingValidationResult.IsValid.md 'PropertyMapper\.MappingValidationResult\.IsValid') | [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') when every writable property on `TOut` has a matching source binding\. |
| [UnmappedTargetProperties](MappingValidationResult.UnmappedTargetProperties.md 'PropertyMapper\.MappingValidationResult\.UnmappedTargetProperties') | Names of writable target properties that have no matching source property\. Empty when [IsValid](MappingValidationResult.IsValid.md 'PropertyMapper\.MappingValidationResult\.IsValid') is [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\. |
