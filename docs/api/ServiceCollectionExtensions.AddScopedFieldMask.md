#### [PropertyMapper](index.md 'index')
### [PropertyMapper\.Extensions](PropertyMapper.Extensions.md 'PropertyMapper\.Extensions').[ServiceCollectionExtensions](ServiceCollectionExtensions.md 'PropertyMapper\.Extensions\.ServiceCollectionExtensions')

## ServiceCollectionExtensions\.AddScopedFieldMask Method

| Overloads | |
| :--- | :--- |
| [AddScopedFieldMask&lt;T&gt;\(this IServiceCollection, string\[\]\)](ServiceCollectionExtensions.AddScopedFieldMask.md#PropertyMapper.Extensions.ServiceCollectionExtensions.AddScopedFieldMask_T_(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,string[]) 'PropertyMapper\.Extensions\.ServiceCollectionExtensions\.AddScopedFieldMask\<T\>\(this Microsoft\.Extensions\.DependencyInjection\.IServiceCollection, string\[\]\)') | Registers [FieldMask&lt;T&gt;](FieldMask_T_.md 'PropertyMapper\.Masking\.FieldMask\<T\>') \(and [IFieldMask&lt;T&gt;](IFieldMask_T_.md 'PropertyMapper\.Masking\.IFieldMask\<T\>')\) as a `Scoped` service with a fixed set of excluded property names\. |
| [AddScopedFieldMask&lt;T&gt;\(this IServiceCollection, Func&lt;IServiceProvider,FieldMask&lt;T&gt;&gt;\)](ServiceCollectionExtensions.AddScopedFieldMask.md#PropertyMapper.Extensions.ServiceCollectionExtensions.AddScopedFieldMask_T_(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Func_System.IServiceProvider,PropertyMapper.Masking.FieldMask_T__) 'PropertyMapper\.Extensions\.ServiceCollectionExtensions\.AddScopedFieldMask\<T\>\(this Microsoft\.Extensions\.DependencyInjection\.IServiceCollection, System\.Func\<System\.IServiceProvider,PropertyMapper\.Masking\.FieldMask\<T\>\>\)') | Registers [FieldMask&lt;T&gt;](FieldMask_T_.md 'PropertyMapper\.Masking\.FieldMask\<T\>') \(and [IFieldMask&lt;T&gt;](IFieldMask_T_.md 'PropertyMapper\.Masking\.IFieldMask\<T\>')\) as a `Scoped` service using a per\-request factory delegate\. |

<a name='PropertyMapper.Extensions.ServiceCollectionExtensions.AddScopedFieldMask_T_(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,string[])'></a>

## ServiceCollectionExtensions\.AddScopedFieldMask\<T\>\(this IServiceCollection, string\[\]\) Method

Registers [FieldMask&lt;T&gt;](FieldMask_T_.md 'PropertyMapper\.Masking\.FieldMask\<T\>') \(and [IFieldMask&lt;T&gt;](IFieldMask_T_.md 'PropertyMapper\.Masking\.IFieldMask\<T\>')\) as a
`Scoped` service with a fixed set of excluded property names\.

```csharp
public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddScopedFieldMask<T>(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, params string[] excludedPropertyNames);
```
#### Type parameters

<a name='PropertyMapper.Extensions.ServiceCollectionExtensions.AddScopedFieldMask_T_(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,string[]).T'></a>

`T`

The target DTO type whose fields this mask controls\.
#### Parameters

<a name='PropertyMapper.Extensions.ServiceCollectionExtensions.AddScopedFieldMask_T_(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,string[]).services'></a>

`services` [Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection')

The [Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection') to add the service to\.

<a name='PropertyMapper.Extensions.ServiceCollectionExtensions.AddScopedFieldMask_T_(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,string[]).excludedPropertyNames'></a>

`excludedPropertyNames` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

Names of properties to zero out after mapping\.

#### Returns
[Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection')  
The same [Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection') for chaining\.

### Example

```csharp
services.AddScopedFieldMask<UserDto>("Password", "SecurityStamp");
```

### Remarks
Use this overload when the same fields are always excluded regardless of the current
user \(e\.g\. a field that should never be exposed in a particular endpoint\)\.
The [FieldMask&lt;T&gt;](FieldMask_T_.md 'PropertyMapper\.Masking\.FieldMask\<T\>') instance is immutable after construction
\(clearing delegates are compiled once\); it is nonetheless registered as `Scoped`
to maintain a consistent lifetime model and to allow easy migration to the
[AddScopedFieldMask&lt;T&gt;\(this IServiceCollection, Func&lt;IServiceProvider,FieldMask&lt;T&gt;&gt;\)](ServiceCollectionExtensions.AddScopedFieldMask.md#PropertyMapper.Extensions.ServiceCollectionExtensions.AddScopedFieldMask_T_(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Func_System.IServiceProvider,PropertyMapper.Masking.FieldMask_T__) 'PropertyMapper\.Extensions\.ServiceCollectionExtensions\.AddScopedFieldMask\<T\>\(this Microsoft\.Extensions\.DependencyInjection\.IServiceCollection, System\.Func\<System\.IServiceProvider,PropertyMapper\.Masking\.FieldMask\<T\>\>\)')
factory overload when dynamic field selection is needed in the future\.

<a name='PropertyMapper.Extensions.ServiceCollectionExtensions.AddScopedFieldMask_T_(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Func_System.IServiceProvider,PropertyMapper.Masking.FieldMask_T__)'></a>

## ServiceCollectionExtensions\.AddScopedFieldMask\<T\>\(this IServiceCollection, Func\<IServiceProvider,FieldMask\<T\>\>\) Method

Registers [FieldMask&lt;T&gt;](FieldMask_T_.md 'PropertyMapper\.Masking\.FieldMask\<T\>') \(and [IFieldMask&lt;T&gt;](IFieldMask_T_.md 'PropertyMapper\.Masking\.IFieldMask\<T\>')\) as a
`Scoped` service using a per\-request factory delegate\.

```csharp
public static Microsoft.Extensions.DependencyInjection.IServiceCollection AddScopedFieldMask<T>(this Microsoft.Extensions.DependencyInjection.IServiceCollection services, System.Func<System.IServiceProvider,PropertyMapper.Masking.FieldMask<T>> factory);
```
#### Type parameters

<a name='PropertyMapper.Extensions.ServiceCollectionExtensions.AddScopedFieldMask_T_(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Func_System.IServiceProvider,PropertyMapper.Masking.FieldMask_T__).T'></a>

`T`

The target DTO type whose fields this mask controls\.
#### Parameters

<a name='PropertyMapper.Extensions.ServiceCollectionExtensions.AddScopedFieldMask_T_(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Func_System.IServiceProvider,PropertyMapper.Masking.FieldMask_T__).services'></a>

`services` [Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection')

The [Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection') to add the service to\.

<a name='PropertyMapper.Extensions.ServiceCollectionExtensions.AddScopedFieldMask_T_(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Func_System.IServiceProvider,PropertyMapper.Masking.FieldMask_T__).factory'></a>

`factory` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.IServiceProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iserviceprovider 'System\.IServiceProvider')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[PropertyMapper\.Masking\.FieldMask&lt;](FieldMask_T_.md 'PropertyMapper\.Masking\.FieldMask\<T\>')[T](ServiceCollectionExtensions.md#PropertyMapper.Extensions.ServiceCollectionExtensions.AddScopedFieldMask_T_(thisMicrosoft.Extensions.DependencyInjection.IServiceCollection,System.Func_System.IServiceProvider,PropertyMapper.Masking.FieldMask_T__).T 'PropertyMapper\.Extensions\.ServiceCollectionExtensions\.AddScopedFieldMask\<T\>\(this Microsoft\.Extensions\.DependencyInjection\.IServiceCollection, System\.Func\<System\.IServiceProvider,PropertyMapper\.Masking\.FieldMask\<T\>\>\)\.T')[&gt;](FieldMask_T_.md 'PropertyMapper\.Masking\.FieldMask\<T\>')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

Per\-request factory that resolves scoped services and returns a
[FieldMask&lt;T&gt;](FieldMask_T_.md 'PropertyMapper\.Masking\.FieldMask\<T\>') configured for the current user\.

#### Returns
[Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection')  
The same [Microsoft\.Extensions\.DependencyInjection\.IServiceCollection](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection 'Microsoft\.Extensions\.DependencyInjection\.IServiceCollection') for chaining\.

### Example

```csharp
services.AddScopedFieldMask<UserDto>(sp =>
{
    var user = sp.GetRequiredService<ICurrentUser>();
    return user.IsAdmin
        ? new FieldMask<UserDto>()                           // no fields hidden
        : new FieldMask<UserDto>("Salary", "NationalId");    // sensitive fields zeroed
});
```

### Remarks
Use this overload when the excluded fields depend on the current request context
\(e\.g\. the authenticated user's permissions\)\. The factory receives the request\-scoped
[System\.IServiceProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iserviceprovider 'System\.IServiceProvider') so it can resolve scoped services such as
`ICurrentUser` or `IAuthorizationService`\.

Both `FieldMask<T>` and `IFieldMask<T>` are registered so
either type can be injected. [MapThenApplyMask&lt;TIn,TOut&gt;\(TIn, IFieldMask&lt;TOut&gt;\)](PropMap.MapThenApplyMask.MUC09OC030SY5KUWU0OU4JU0F.md 'PropertyMapper\.PropMap\.MapThenApplyMask\<TIn,TOut\>\(TIn, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)') uses
the concrete type for its zero-reflection fast path.

---
Generated by [DefaultDocumentation](https://github.com/Doraku/DefaultDocumentation 'https://github\.com/Doraku/DefaultDocumentation')