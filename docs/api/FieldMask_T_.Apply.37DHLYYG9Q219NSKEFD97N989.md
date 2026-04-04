## FieldMask\<T\>\.Apply\(T\) Method

Sets each excluded property on [instance](FieldMask_T_.Apply.37DHLYYG9Q219NSKEFD97N989.md#PropertyMapper.Masking.FieldMask_T_.Apply(T).instance 'PropertyMapper\.Masking\.FieldMask\<T\>\.Apply\(T\)\.instance') to its [default](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/default 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/default') value\.
Uses the pre\-compiled delegate array — no reflection overhead on the hot path\.

```csharp
public void Apply(T instance);
```
#### Parameters

<a name='PropertyMapper.Masking.FieldMask_T_.Apply(T).instance'></a>

`instance` [T](FieldMask_T_.md#PropertyMapper.Masking.FieldMask_T_.T 'PropertyMapper\.Masking\.FieldMask\<T\>\.T')

The mapped target instance to modify in\-place\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [instance](FieldMask_T_.Apply.37DHLYYG9Q219NSKEFD97N989.md#PropertyMapper.Masking.FieldMask_T_.Apply(T).instance 'PropertyMapper\.Masking\.FieldMask\<T\>\.Apply\(T\)\.instance') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.