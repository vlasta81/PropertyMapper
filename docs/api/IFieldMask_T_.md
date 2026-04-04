## IFieldMask\<T\> Interface

Per\-request field visibility mask that suppresses specific [T](IFieldMask_T_.md#PropertyMapper.Masking.IFieldMask_T_.T 'PropertyMapper\.Masking\.IFieldMask\<T\>\.T') properties
after mapping or excludes them from expression\-tree projections\.

```csharp
public interface IFieldMask<T>
```
#### Type parameters

<a name='PropertyMapper.Masking.IFieldMask_T_.T'></a>

`T`

The target DTO type whose fields this mask controls\.

Derived  
&#8627; [FieldMask&lt;T&gt;](FieldMask_T_.md 'PropertyMapper\.Masking\.FieldMask\<T\>')

### Remarks

Register as a `Scoped` service so that each HTTP request receives an instance
populated with the current user's field-level permissions.

<b>In-memory mapping:</b>
            Pass to [MapThenApplyMask&lt;TIn,TOut&gt;\(TIn, IFieldMask&lt;TOut&gt;\)](PropMap.MapThenApplyMask.MUC09OC030SY5KUWU0OU4JU0F.md 'PropertyMapper\.PropMap\.MapThenApplyMask\<TIn,TOut\>\(TIn, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)').
            The singleton compiled delegate maps all fields; the mask zeroes out excluded ones
            on the returned instance. Sensitive data is produced and then cleared in-process.

<b>IQueryable / EF Core:</b>
            Pass to [GetProjectionExpression&lt;TIn,TOut&gt;\(IFieldMask&lt;TOut&gt;\)](PropMap.GetProjectionExpression.md#PropertyMapper.PropMap.GetProjectionExpression_TIn,TOut_(PropertyMapper.Masking.IFieldMask_TOut_) 'PropertyMapper\.PropMap\.GetProjectionExpression\<TIn,TOut\>\(PropertyMapper\.Masking\.IFieldMask\<TOut\>\)') or
            [Project&lt;TIn,TOut&gt;\(IQueryable&lt;TIn&gt;, IFieldMask&lt;TOut&gt;\)](PropMap.Project.md#PropertyMapper.PropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_,PropertyMapper.Masking.IFieldMask_TOut_) 'PropertyMapper\.PropMap\.Project\<TIn,TOut\>\(System\.Linq\.IQueryable\<TIn\>, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)').
            Excluded fields are omitted from the generated SQL `SELECT` clause so sensitive
            data is never loaded from the database.

| Properties | |
| :--- | :--- |
| [ExcludedFields](IFieldMask_T_.ExcludedFields.md 'PropertyMapper\.Masking\.IFieldMask\<T\>\.ExcludedFields') | Names of [T](IFieldMask_T_.md#PropertyMapper.Masking.IFieldMask_T_.T 'PropertyMapper\.Masking\.IFieldMask\<T\>\.T') properties that should be suppressed after mapping or excluded from expression\-tree projections\. |
