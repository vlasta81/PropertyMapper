## IPropMap\.Validate\<TIn,TOut\>\(\) Method

Validates the mapping plan and reports unmapped target properties\.

```csharp
PropertyMapper.MappingValidationResult Validate<TIn,TOut>()
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.IPropMap.Validate_TIn,TOut_().TIn'></a>

`TIn`

<a name='PropertyMapper.IPropMap.Validate_TIn,TOut_().TOut'></a>

`TOut`

#### Returns
[MappingValidationResult](MappingValidationResult.md 'PropertyMapper\.MappingValidationResult')