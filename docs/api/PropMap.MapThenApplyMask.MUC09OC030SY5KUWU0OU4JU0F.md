## PropMap\.MapThenApplyMask\<TIn,TOut\>\(TIn, IFieldMask\<TOut\>\) Method

Maps [source](PropMap.MapThenApplyMask.MUC09OC030SY5KUWU0OU4JU0F.md#PropertyMapper.PropMap.MapThenApplyMask_TIn,TOut_(TIn,PropertyMapper.Masking.IFieldMask_TOut_).source 'PropertyMapper\.PropMap\.MapThenApplyMask\<TIn,TOut\>\(TIn, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.source') to a new [TOut](PropMap.MapThenApplyMask.MUC09OC030SY5KUWU0OU4JU0F.md#PropertyMapper.PropMap.MapThenApplyMask_TIn,TOut_(TIn,PropertyMapper.Masking.IFieldMask_TOut_).TOut 'PropertyMapper\.PropMap\.MapThenApplyMask\<TIn,TOut\>\(TIn, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.TOut') using the compiled
singleton delegate, then applies [mask](PropMap.MapThenApplyMask.MUC09OC030SY5KUWU0OU4JU0F.md#PropertyMapper.PropMap.MapThenApplyMask_TIn,TOut_(TIn,PropertyMapper.Masking.IFieldMask_TOut_).mask 'PropertyMapper\.PropMap\.MapThenApplyMask\<TIn,TOut\>\(TIn, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.mask') to zero out fields the current
caller is not authorised to see\.

```csharp
public TOut MapThenApplyMask<TIn,TOut>(TIn source, PropertyMapper.Masking.IFieldMask<TOut> mask)
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.MapThenApplyMask_TIn,TOut_(TIn,PropertyMapper.Masking.IFieldMask_TOut_).TIn'></a>

`TIn`

Source type\.

<a name='PropertyMapper.PropMap.MapThenApplyMask_TIn,TOut_(TIn,PropertyMapper.Masking.IFieldMask_TOut_).TOut'></a>

`TOut`

Target type \(must have a parameterless constructor\)\.
#### Parameters

<a name='PropertyMapper.PropMap.MapThenApplyMask_TIn,TOut_(TIn,PropertyMapper.Masking.IFieldMask_TOut_).source'></a>

`source` [TIn](PropMap.MapThenApplyMask.MUC09OC030SY5KUWU0OU4JU0F.md#PropertyMapper.PropMap.MapThenApplyMask_TIn,TOut_(TIn,PropertyMapper.Masking.IFieldMask_TOut_).TIn 'PropertyMapper\.PropMap\.MapThenApplyMask\<TIn,TOut\>\(TIn, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.TIn')

Source object to map\.

<a name='PropertyMapper.PropMap.MapThenApplyMask_TIn,TOut_(TIn,PropertyMapper.Masking.IFieldMask_TOut_).mask'></a>

`mask` [PropertyMapper\.Masking\.IFieldMask&lt;](IFieldMask_T_.md 'PropertyMapper\.Masking\.IFieldMask\<T\>')[TOut](PropMap.MapThenApplyMask.MUC09OC030SY5KUWU0OU4JU0F.md#PropertyMapper.PropMap.MapThenApplyMask_TIn,TOut_(TIn,PropertyMapper.Masking.IFieldMask_TOut_).TOut 'PropertyMapper\.PropMap\.MapThenApplyMask\<TIn,TOut\>\(TIn, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.TOut')[&gt;](IFieldMask_T_.md 'PropertyMapper\.Masking\.IFieldMask\<T\>')

Per\-request field visibility mask applied after mapping\.

#### Returns
[TOut](PropMap.MapThenApplyMask.MUC09OC030SY5KUWU0OU4JU0F.md#PropertyMapper.PropMap.MapThenApplyMask_TIn,TOut_(TIn,PropertyMapper.Masking.IFieldMask_TOut_).TOut 'PropertyMapper\.PropMap\.MapThenApplyMask\<TIn,TOut\>\(TIn, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.TOut')  
A new [TOut](PropMap.MapThenApplyMask.MUC09OC030SY5KUWU0OU4JU0F.md#PropertyMapper.PropMap.MapThenApplyMask_TIn,TOut_(TIn,PropertyMapper.Masking.IFieldMask_TOut_).TOut 'PropertyMapper\.PropMap\.MapThenApplyMask\<TIn,TOut\>\(TIn, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.TOut') with excluded fields set to their default value\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [source](PropMap.MapThenApplyMask.MUC09OC030SY5KUWU0OU4JU0F.md#PropertyMapper.PropMap.MapThenApplyMask_TIn,TOut_(TIn,PropertyMapper.Masking.IFieldMask_TOut_).source 'PropertyMapper\.PropMap\.MapThenApplyMask\<TIn,TOut\>\(TIn, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.source') or [mask](PropMap.MapThenApplyMask.MUC09OC030SY5KUWU0OU4JU0F.md#PropertyMapper.PropMap.MapThenApplyMask_TIn,TOut_(TIn,PropertyMapper.Masking.IFieldMask_TOut_).mask 'PropertyMapper\.PropMap\.MapThenApplyMask\<TIn,TOut\>\(TIn, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.mask') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

### Remarks

This is the <em>post-map filter</em> pattern: the singleton compiled delegate always
maps all fields at maximum performance; a per-request [IFieldMask&lt;T&gt;](IFieldMask_T_.md 'PropertyMapper\.Masking\.IFieldMask\<T\>')
zeroes sensitive fields <em>after</em> construction. The mapper itself remains stateless
and thread-safe — the per-request sensitivity is confined entirely to the mask instance.

Register [FieldMask&lt;T&gt;](FieldMask_T_.md 'PropertyMapper\.Masking\.FieldMask\<T\>') as a `Scoped` service so each HTTP request
receives a mask populated with the current user's field-level permissions.

For [System\.Linq\.IQueryable&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1 'System\.Linq\.IQueryable\`1') / EF Core scenarios where sensitive fields
must not be loaded from the database at all, use
[Project&lt;TIn,TOut&gt;\(IQueryable&lt;TIn&gt;, IFieldMask&lt;TOut&gt;\)](PropMap.Project.md#PropertyMapper.PropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_,PropertyMapper.Masking.IFieldMask_TOut_) 'PropertyMapper\.PropMap\.Project\<TIn,TOut\>\(System\.Linq\.IQueryable\<TIn\>, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)') instead.