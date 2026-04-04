#### [PropertyMapper](index.md 'index')
### [PropertyMapper](PropertyMapper.md 'PropertyMapper').[PropMap](PropMap.md 'PropertyMapper\.PropMap')

## PropMap\.Project Method

| Overloads | |
| :--- | :--- |
| [Project&lt;TIn,TOut&gt;\(IQueryable&lt;TIn&gt;\)](PropMap.Project.md#PropertyMapper.PropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_) 'PropertyMapper\.PropMap\.Project\<TIn,TOut\>\(System\.Linq\.IQueryable\<TIn\>\)') | Projects an [System\.Linq\.IQueryable&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1 'System\.Linq\.IQueryable\`1') using the compiled expression tree for [TIn](PropMap.md#PropertyMapper.PropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_).TIn 'PropertyMapper\.PropMap\.Project\<TIn,TOut\>\(System\.Linq\.IQueryable\<TIn\>\)\.TIn')→[TOut](PropMap.md#PropertyMapper.PropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_).TOut 'PropertyMapper\.PropMap\.Project\<TIn,TOut\>\(System\.Linq\.IQueryable\<TIn\>\)\.TOut')\. The expression is cached after the first call so subsequent calls are fast\. Compatible with EF Core and any LINQ provider that translates expression trees\. |
| [Project&lt;TIn,TOut&gt;\(IQueryable&lt;TIn&gt;, IFieldMask&lt;TOut&gt;\)](PropMap.Project.md#PropertyMapper.PropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_,PropertyMapper.Masking.IFieldMask_TOut_) 'PropertyMapper\.PropMap\.Project\<TIn,TOut\>\(System\.Linq\.IQueryable\<TIn\>, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)') | Projects an [System\.Linq\.IQueryable&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1 'System\.Linq\.IQueryable\`1') excluding fields specified by [mask](PropMap.md#PropertyMapper.PropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_,PropertyMapper.Masking.IFieldMask_TOut_).mask 'PropertyMapper\.PropMap\.Project\<TIn,TOut\>\(System\.Linq\.IQueryable\<TIn\>, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.mask') from the generated SQL `SELECT` clause\. Suitable for per\-request field\-level authorisation with EF Core or any LINQ provider that translates expression trees\. |

<a name='PropertyMapper.PropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_)'></a>

## PropMap\.Project\<TIn,TOut\>\(IQueryable\<TIn\>\) Method

Projects an [System\.Linq\.IQueryable&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1 'System\.Linq\.IQueryable\`1') using the compiled expression tree for
[TIn](PropMap.md#PropertyMapper.PropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_).TIn 'PropertyMapper\.PropMap\.Project\<TIn,TOut\>\(System\.Linq\.IQueryable\<TIn\>\)\.TIn')→[TOut](PropMap.md#PropertyMapper.PropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_).TOut 'PropertyMapper\.PropMap\.Project\<TIn,TOut\>\(System\.Linq\.IQueryable\<TIn\>\)\.TOut')\.
The expression is cached after the first call so subsequent calls are fast\.
Compatible with EF Core and any LINQ provider that translates expression trees\.

```csharp
public System.Linq.IQueryable<TOut> Project<TIn,TOut>(System.Linq.IQueryable<TIn> source)
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_).TIn'></a>

`TIn`

Source element type\.

<a name='PropertyMapper.PropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_).TOut'></a>

`TOut`

Target element type \(must have a parameterless constructor\)\.
#### Parameters

<a name='PropertyMapper.PropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_).source'></a>

`source` [System\.Linq\.IQueryable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1 'System\.Linq\.IQueryable\`1')[TIn](PropMap.md#PropertyMapper.PropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_).TIn 'PropertyMapper\.PropMap\.Project\<TIn,TOut\>\(System\.Linq\.IQueryable\<TIn\>\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1 'System\.Linq\.IQueryable\`1')

The queryable to project\.

#### Returns
[System\.Linq\.IQueryable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1 'System\.Linq\.IQueryable\`1')[TOut](PropMap.md#PropertyMapper.PropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_).TOut 'PropertyMapper\.PropMap\.Project\<TIn,TOut\>\(System\.Linq\.IQueryable\<TIn\>\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1 'System\.Linq\.IQueryable\`1')  
A new [System\.Linq\.IQueryable&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1 'System\.Linq\.IQueryable\`1') with the projection applied\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [source](PropMap.md#PropertyMapper.PropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_).source 'PropertyMapper\.PropMap\.Project\<TIn,TOut\>\(System\.Linq\.IQueryable\<TIn\>\)\.source') is null\.

<a name='PropertyMapper.PropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_,PropertyMapper.Masking.IFieldMask_TOut_)'></a>

## PropMap\.Project\<TIn,TOut\>\(IQueryable\<TIn\>, IFieldMask\<TOut\>\) Method

Projects an [System\.Linq\.IQueryable&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1 'System\.Linq\.IQueryable\`1') excluding fields specified by
[mask](PropMap.md#PropertyMapper.PropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_,PropertyMapper.Masking.IFieldMask_TOut_).mask 'PropertyMapper\.PropMap\.Project\<TIn,TOut\>\(System\.Linq\.IQueryable\<TIn\>, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.mask') from the generated SQL `SELECT` clause\.
Suitable for per\-request field\-level authorisation with EF Core or any LINQ provider
that translates expression trees\.

```csharp
public System.Linq.IQueryable<TOut> Project<TIn,TOut>(System.Linq.IQueryable<TIn> source, PropertyMapper.Masking.IFieldMask<TOut> mask)
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_,PropertyMapper.Masking.IFieldMask_TOut_).TIn'></a>

`TIn`

Source element type\.

<a name='PropertyMapper.PropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_,PropertyMapper.Masking.IFieldMask_TOut_).TOut'></a>

`TOut`

Target element type \(must have a parameterless constructor\)\.
#### Parameters

<a name='PropertyMapper.PropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_,PropertyMapper.Masking.IFieldMask_TOut_).source'></a>

`source` [System\.Linq\.IQueryable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1 'System\.Linq\.IQueryable\`1')[TIn](PropMap.md#PropertyMapper.PropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_,PropertyMapper.Masking.IFieldMask_TOut_).TIn 'PropertyMapper\.PropMap\.Project\<TIn,TOut\>\(System\.Linq\.IQueryable\<TIn\>, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1 'System\.Linq\.IQueryable\`1')

The queryable to project\.

<a name='PropertyMapper.PropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_,PropertyMapper.Masking.IFieldMask_TOut_).mask'></a>

`mask` [PropertyMapper\.Masking\.IFieldMask&lt;](IFieldMask_T_.md 'PropertyMapper\.Masking\.IFieldMask\<T\>')[TOut](PropMap.md#PropertyMapper.PropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_,PropertyMapper.Masking.IFieldMask_TOut_).TOut 'PropertyMapper\.PropMap\.Project\<TIn,TOut\>\(System\.Linq\.IQueryable\<TIn\>, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.TOut')[&gt;](IFieldMask_T_.md 'PropertyMapper\.Masking\.IFieldMask\<T\>')

Per\-request field mask applied to the SQL SELECT\.

#### Returns
[System\.Linq\.IQueryable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1 'System\.Linq\.IQueryable\`1')[TOut](PropMap.md#PropertyMapper.PropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_,PropertyMapper.Masking.IFieldMask_TOut_).TOut 'PropertyMapper\.PropMap\.Project\<TIn,TOut\>\(System\.Linq\.IQueryable\<TIn\>, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1 'System\.Linq\.IQueryable\`1')  
A new [System\.Linq\.IQueryable&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1 'System\.Linq\.IQueryable\`1') with masked fields excluded from the query\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [source](PropMap.md#PropertyMapper.PropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_,PropertyMapper.Masking.IFieldMask_TOut_).source 'PropertyMapper\.PropMap\.Project\<TIn,TOut\>\(System\.Linq\.IQueryable\<TIn\>, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.source') or [mask](PropMap.md#PropertyMapper.PropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_,PropertyMapper.Masking.IFieldMask_TOut_).mask 'PropertyMapper\.PropMap\.Project\<TIn,TOut\>\(System\.Linq\.IQueryable\<TIn\>, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.mask') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

---
Generated by [DefaultDocumentation](https://github.com/Doraku/DefaultDocumentation 'https://github\.com/Doraku/DefaultDocumentation')