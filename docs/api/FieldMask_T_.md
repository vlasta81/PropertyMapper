## FieldMask\<T\> Class

Default [IFieldMask&lt;T&gt;](IFieldMask_T_.md 'PropertyMapper\.Masking\.IFieldMask\<T\>') implementation backed by compile\-time expression trees\.
Setter\-clearing actions are compiled once on construction and reused for every
subsequent [Apply\(T\)](FieldMask_T_.Apply.37DHLYYG9Q219NSKEFD97N989.md 'PropertyMapper\.Masking\.FieldMask\<T\>\.Apply\(T\)') call, making the hot path allocation\-free\.

```csharp
public sealed class FieldMask<T> : PropertyMapper.Masking.IFieldMask<T>
```
#### Type parameters

<a name='PropertyMapper.Masking.FieldMask_T_.T'></a>

`T`

The target DTO type\.

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') &#129106; FieldMask\<T\>

Implements [PropertyMapper\.Masking\.IFieldMask&lt;](IFieldMask_T_.md 'PropertyMapper\.Masking\.IFieldMask\<T\>')[T](FieldMask_T_.md#PropertyMapper.Masking.FieldMask_T_.T 'PropertyMapper\.Masking\.FieldMask\<T\>\.T')[&gt;](IFieldMask_T_.md 'PropertyMapper\.Masking\.IFieldMask\<T\>')

### Remarks

Works only with <b>writable</b> (non-`init`) properties on mutable class DTOs.
For records or `init`-only DTOs, use the
[GetProjectionExpression&lt;TIn,TOut&gt;\(IFieldMask&lt;TOut&gt;\)](PropMap.GetProjectionExpression.md#PropertyMapper.PropMap.GetProjectionExpression_TIn,TOut_(PropertyMapper.Masking.IFieldMask_TOut_) 'PropertyMapper\.PropMap\.GetProjectionExpression\<TIn,TOut\>\(PropertyMapper\.Masking\.IFieldMask\<TOut\>\)') overload instead,
which excludes fields at the query level so they are never populated.

Non-existent or read-only property names are silently ignored during construction.

| Constructors | |
| :--- | :--- |
| [FieldMask\(string\[\]\)](FieldMask_T_..ctor.AI6VFKG77KXARTSBPJRG4ARAA.md 'PropertyMapper\.Masking\.FieldMask\<T\>\.FieldMask\(string\[\]\)') | Initializes a new [FieldMask&lt;T&gt;](FieldMask_T_.md 'PropertyMapper\.Masking\.FieldMask\<T\>') that will zero out the specified properties\. Clearing actions are compiled to expression\-tree delegates immediately\. |

| Properties | |
| :--- | :--- |
| [ExcludedFields](FieldMask_T_.ExcludedFields.md 'PropertyMapper\.Masking\.FieldMask\<T\>\.ExcludedFields') | Names of [T](FieldMask_T_.md#PropertyMapper.Masking.FieldMask_T_.T 'PropertyMapper\.Masking\.FieldMask\<T\>\.T') properties that should be suppressed after mapping or excluded from expression\-tree projections\. |

| Methods | |
| :--- | :--- |
| [Apply\(T\)](FieldMask_T_.Apply.37DHLYYG9Q219NSKEFD97N989.md 'PropertyMapper\.Masking\.FieldMask\<T\>\.Apply\(T\)') | Sets each excluded property on [instance](FieldMask_T_.Apply.37DHLYYG9Q219NSKEFD97N989.md#PropertyMapper.Masking.FieldMask_T_.Apply(T).instance 'PropertyMapper\.Masking\.FieldMask\<T\>\.Apply\(T\)\.instance') to its [default](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/default 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/default') value\. Uses the pre\-compiled delegate array — no reflection overhead on the hot path\. |
