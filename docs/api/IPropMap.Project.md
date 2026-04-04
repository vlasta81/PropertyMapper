#### [PropertyMapper](index.md 'index')
### [PropertyMapper](PropertyMapper.md 'PropertyMapper').[IPropMap](IPropMap.md 'PropertyMapper\.IPropMap')

## IPropMap\.Project Method

| Overloads | |
| :--- | :--- |
| [Project&lt;TIn,TOut&gt;\(IQueryable&lt;TIn&gt;\)](IPropMap.Project.md#PropertyMapper.IPropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_) 'PropertyMapper\.IPropMap\.Project\<TIn,TOut\>\(System\.Linq\.IQueryable\<TIn\>\)') | Projects an [System\.Linq\.IQueryable&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1 'System\.Linq\.IQueryable\`1') using the cached expression tree\. |
| [Project&lt;TIn,TOut&gt;\(IQueryable&lt;TIn&gt;, IFieldMask&lt;TOut&gt;\)](IPropMap.Project.md#PropertyMapper.IPropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_,PropertyMapper.Masking.IFieldMask_TOut_) 'PropertyMapper\.IPropMap\.Project\<TIn,TOut\>\(System\.Linq\.IQueryable\<TIn\>, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)') | Projects an [System\.Linq\.IQueryable&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1 'System\.Linq\.IQueryable\`1') with masked fields excluded from the SQL SELECT\. |

<a name='PropertyMapper.IPropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_)'></a>

## IPropMap\.Project\<TIn,TOut\>\(IQueryable\<TIn\>\) Method

Projects an [System\.Linq\.IQueryable&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1 'System\.Linq\.IQueryable\`1') using the cached expression tree\.

```csharp
System.Linq.IQueryable<TOut> Project<TIn,TOut>(System.Linq.IQueryable<TIn> source)
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.IPropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_).TIn'></a>

`TIn`

<a name='PropertyMapper.IPropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_).TOut'></a>

`TOut`
#### Parameters

<a name='PropertyMapper.IPropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_).source'></a>

`source` [System\.Linq\.IQueryable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1 'System\.Linq\.IQueryable\`1')[TIn](IPropMap.md#PropertyMapper.IPropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_).TIn 'PropertyMapper\.IPropMap\.Project\<TIn,TOut\>\(System\.Linq\.IQueryable\<TIn\>\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1 'System\.Linq\.IQueryable\`1')

#### Returns
[System\.Linq\.IQueryable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1 'System\.Linq\.IQueryable\`1')[TOut](IPropMap.md#PropertyMapper.IPropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_).TOut 'PropertyMapper\.IPropMap\.Project\<TIn,TOut\>\(System\.Linq\.IQueryable\<TIn\>\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1 'System\.Linq\.IQueryable\`1')

<a name='PropertyMapper.IPropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_,PropertyMapper.Masking.IFieldMask_TOut_)'></a>

## IPropMap\.Project\<TIn,TOut\>\(IQueryable\<TIn\>, IFieldMask\<TOut\>\) Method

Projects an [System\.Linq\.IQueryable&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1 'System\.Linq\.IQueryable\`1') with masked fields excluded from the SQL SELECT\.

```csharp
System.Linq.IQueryable<TOut> Project<TIn,TOut>(System.Linq.IQueryable<TIn> source, PropertyMapper.Masking.IFieldMask<TOut> mask)
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.IPropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_,PropertyMapper.Masking.IFieldMask_TOut_).TIn'></a>

`TIn`

<a name='PropertyMapper.IPropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_,PropertyMapper.Masking.IFieldMask_TOut_).TOut'></a>

`TOut`
#### Parameters

<a name='PropertyMapper.IPropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_,PropertyMapper.Masking.IFieldMask_TOut_).source'></a>

`source` [System\.Linq\.IQueryable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1 'System\.Linq\.IQueryable\`1')[TIn](IPropMap.md#PropertyMapper.IPropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_,PropertyMapper.Masking.IFieldMask_TOut_).TIn 'PropertyMapper\.IPropMap\.Project\<TIn,TOut\>\(System\.Linq\.IQueryable\<TIn\>, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1 'System\.Linq\.IQueryable\`1')

<a name='PropertyMapper.IPropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_,PropertyMapper.Masking.IFieldMask_TOut_).mask'></a>

`mask` [PropertyMapper\.Masking\.IFieldMask&lt;](IFieldMask_T_.md 'PropertyMapper\.Masking\.IFieldMask\<T\>')[TOut](IPropMap.md#PropertyMapper.IPropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_,PropertyMapper.Masking.IFieldMask_TOut_).TOut 'PropertyMapper\.IPropMap\.Project\<TIn,TOut\>\(System\.Linq\.IQueryable\<TIn\>, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.TOut')[&gt;](IFieldMask_T_.md 'PropertyMapper\.Masking\.IFieldMask\<T\>')

#### Returns
[System\.Linq\.IQueryable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1 'System\.Linq\.IQueryable\`1')[TOut](IPropMap.md#PropertyMapper.IPropMap.Project_TIn,TOut_(System.Linq.IQueryable_TIn_,PropertyMapper.Masking.IFieldMask_TOut_).TOut 'PropertyMapper\.IPropMap\.Project\<TIn,TOut\>\(System\.Linq\.IQueryable\<TIn\>, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.iqueryable-1 'System\.Linq\.IQueryable\`1')

---
Generated by [DefaultDocumentation](https://github.com/Doraku/DefaultDocumentation 'https://github\.com/Doraku/DefaultDocumentation')