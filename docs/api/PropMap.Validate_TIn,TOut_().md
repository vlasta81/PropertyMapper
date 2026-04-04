## PropMap\.Validate\<TIn,TOut\>\(\) Method

Validates the [TIn](PropMap.Validate_TIn,TOut_().md#PropertyMapper.PropMap.Validate_TIn,TOut_().TIn 'PropertyMapper\.PropMap\.Validate\<TIn,TOut\>\(\)\.TIn')→[TOut](PropMap.Validate_TIn,TOut_().md#PropertyMapper.PropMap.Validate_TIn,TOut_().TOut 'PropertyMapper\.PropMap\.Validate\<TIn,TOut\>\(\)\.TOut') mapping without executing it\.
Builds the mapping plan and reports which target properties have no matching source property\.

```csharp
public PropertyMapper.MappingValidationResult Validate<TIn,TOut>()
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.Validate_TIn,TOut_().TIn'></a>

`TIn`

Source type\.

<a name='PropertyMapper.PropMap.Validate_TIn,TOut_().TOut'></a>

`TOut`

Target type \(must have a parameterless constructor\)\.

#### Returns
[MappingValidationResult](MappingValidationResult.md 'PropertyMapper\.MappingValidationResult')  
A [MappingValidationResult](MappingValidationResult.md 'PropertyMapper\.MappingValidationResult') describing any unmapped target properties\.

### Remarks
No IL is emitted and no delegate is cached\. Use at application startup or in unit tests
to catch misconfigured mappings early\.