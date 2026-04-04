#### [PropertyMapper](index.md 'index')
### [PropertyMapper](PropertyMapper.md 'PropertyMapper').[PropMap](PropMap.md 'PropertyMapper\.PropMap')

## PropMap\.GetProjectionExpression Method

| Overloads | |
| :--- | :--- |
| [GetProjectionExpression&lt;TIn,TOut&gt;\(\)](PropMap.GetProjectionExpression.md#PropertyMapper.PropMap.GetProjectionExpression_TIn,TOut_() 'PropertyMapper\.PropMap\.GetProjectionExpression\<TIn,TOut\>\(\)') | Returns a cached [System\.Linq\.Expressions\.Expression&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression-1 'System\.Linq\.Expressions\.Expression\`1') that projects a [TIn](PropMap.md#PropertyMapper.PropMap.GetProjectionExpression_TIn,TOut_().TIn 'PropertyMapper\.PropMap\.GetProjectionExpression\<TIn,TOut\>\(\)\.TIn') to a new [TOut](PropMap.md#PropertyMapper.PropMap.GetProjectionExpression_TIn,TOut_().TOut 'PropertyMapper\.PropMap\.GetProjectionExpression\<TIn,TOut\>\(\)\.TOut') by copying all matched properties as a [System\.Linq\.Expressions\.MemberInitExpression](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.memberinitexpression 'System\.Linq\.Expressions\.MemberInitExpression')\. The expression can be passed directly to [System\.Linq\.Queryable\.Select&lt;&gt;\.Linq\.IQueryable\{&lt;&gt;\.Linq\.Expressions\.Expression\{System\.Func\{&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.queryable.select--2#system-linq-queryable-select--2(system-linq-iqueryable{--0}-system-linq-expressions-expression{system-func{--0---1}}) 'System\.Linq\.Queryable\.Select\`\`2\(System\.Linq\.IQueryable\{\`\`0\},System\.Linq\.Expressions\.Expression\{System\.Func\{\`\`0,\`\`1\}\}\)') or used with ORM frameworks such as EF Core that translate expression trees to SQL\. Any [PropertyMapper\.PropMap\.Configure&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/propertymapper.propmap.configure--2 'PropertyMapper\.PropMap\.Configure\`\`2') rules \([Ignore&lt;TProp&gt;\(Expression&lt;Func&lt;TOut,TProp&gt;&gt;\)](TypePairConfiguration_TIn,TOut_.Ignore.47Y5JYZI9VSIMEAAQAG2ALEBA.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.Ignore\<TProp\>\(System\.Linq\.Expressions\.Expression\<System\.Func\<TOut,TProp\>\>\)'), [MapFromExpression&lt;TProp&gt;\(Expression&lt;Func&lt;TOut,TProp&gt;&gt;, Expression&lt;Func&lt;TIn,TProp&gt;&gt;\)](TypePairConfiguration_TIn,TOut_.MapFromExpression.3312TDR6G2XRKAXKYGABEGPN2.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.MapFromExpression\<TProp\>\(System\.Linq\.Expressions\.Expression\<System\.Func\<TOut,TProp\>\>, System\.Linq\.Expressions\.Expression\<System\.Func\<TIn,TProp\>\>\)')\) are applied to the expression\. |
| [GetProjectionExpression&lt;TIn,TOut&gt;\(IFieldMask&lt;TOut&gt;\)](PropMap.GetProjectionExpression.md#PropertyMapper.PropMap.GetProjectionExpression_TIn,TOut_(PropertyMapper.Masking.IFieldMask_TOut_) 'PropertyMapper\.PropMap\.GetProjectionExpression\<TIn,TOut\>\(PropertyMapper\.Masking\.IFieldMask\<TOut\>\)') | Returns a per\-request \(uncached\) [System\.Linq\.Expressions\.Expression&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression-1 'System\.Linq\.Expressions\.Expression\`1') that projects [TIn](PropMap.md#PropertyMapper.PropMap.GetProjectionExpression_TIn,TOut_(PropertyMapper.Masking.IFieldMask_TOut_).TIn 'PropertyMapper\.PropMap\.GetProjectionExpression\<TIn,TOut\>\(PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.TIn') to [TOut](PropMap.md#PropertyMapper.PropMap.GetProjectionExpression_TIn,TOut_(PropertyMapper.Masking.IFieldMask_TOut_).TOut 'PropertyMapper\.PropMap\.GetProjectionExpression\<TIn,TOut\>\(PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.TOut'), omitting every field listed in [mask](PropMap.md#PropertyMapper.PropMap.GetProjectionExpression_TIn,TOut_(PropertyMapper.Masking.IFieldMask_TOut_).mask 'PropertyMapper\.PropMap\.GetProjectionExpression\<TIn,TOut\>\(PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.mask') from the `MemberInitExpression`\. |

<a name='PropertyMapper.PropMap.GetProjectionExpression_TIn,TOut_()'></a>

## PropMap\.GetProjectionExpression\<TIn,TOut\>\(\) Method

Returns a cached [System\.Linq\.Expressions\.Expression&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression-1 'System\.Linq\.Expressions\.Expression\`1') that projects a [TIn](PropMap.md#PropertyMapper.PropMap.GetProjectionExpression_TIn,TOut_().TIn 'PropertyMapper\.PropMap\.GetProjectionExpression\<TIn,TOut\>\(\)\.TIn')
to a new [TOut](PropMap.md#PropertyMapper.PropMap.GetProjectionExpression_TIn,TOut_().TOut 'PropertyMapper\.PropMap\.GetProjectionExpression\<TIn,TOut\>\(\)\.TOut') by copying all matched properties as a
[System\.Linq\.Expressions\.MemberInitExpression](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.memberinitexpression 'System\.Linq\.Expressions\.MemberInitExpression')\.
The expression can be passed directly to
[System\.Linq\.Queryable\.Select&lt;&gt;\.Linq\.IQueryable\{&lt;&gt;\.Linq\.Expressions\.Expression\{System\.Func\{&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.queryable.select--2#system-linq-queryable-select--2(system-linq-iqueryable{--0}-system-linq-expressions-expression{system-func{--0---1}}) 'System\.Linq\.Queryable\.Select\`\`2\(System\.Linq\.IQueryable\{\`\`0\},System\.Linq\.Expressions\.Expression\{System\.Func\{\`\`0,\`\`1\}\}\)')
or used with ORM frameworks such as EF Core that translate expression trees to SQL\.
Any [PropertyMapper\.PropMap\.Configure&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/propertymapper.propmap.configure--2 'PropertyMapper\.PropMap\.Configure\`\`2') rules \([Ignore&lt;TProp&gt;\(Expression&lt;Func&lt;TOut,TProp&gt;&gt;\)](TypePairConfiguration_TIn,TOut_.Ignore.47Y5JYZI9VSIMEAAQAG2ALEBA.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.Ignore\<TProp\>\(System\.Linq\.Expressions\.Expression\<System\.Func\<TOut,TProp\>\>\)'),
[MapFromExpression&lt;TProp&gt;\(Expression&lt;Func&lt;TOut,TProp&gt;&gt;, Expression&lt;Func&lt;TIn,TProp&gt;&gt;\)](TypePairConfiguration_TIn,TOut_.MapFromExpression.3312TDR6G2XRKAXKYGABEGPN2.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.MapFromExpression\<TProp\>\(System\.Linq\.Expressions\.Expression\<System\.Func\<TOut,TProp\>\>, System\.Linq\.Expressions\.Expression\<System\.Func\<TIn,TProp\>\>\)')\) are applied to the expression\.

```csharp
public System.Linq.Expressions.Expression<System.Func<TIn,TOut>> GetProjectionExpression<TIn,TOut>()
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.GetProjectionExpression_TIn,TOut_().TIn'></a>

`TIn`

Source type\.

<a name='PropertyMapper.PropMap.GetProjectionExpression_TIn,TOut_().TOut'></a>

`TOut`

Target type \(must have a parameterless constructor\)\.

#### Returns
[System\.Linq\.Expressions\.Expression&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression-1 'System\.Linq\.Expressions\.Expression\`1')[System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[TIn](PropMap.md#PropertyMapper.PropMap.GetProjectionExpression_TIn,TOut_().TIn 'PropertyMapper\.PropMap\.GetProjectionExpression\<TIn,TOut\>\(\)\.TIn')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[TOut](PropMap.md#PropertyMapper.PropMap.GetProjectionExpression_TIn,TOut_().TOut 'PropertyMapper\.PropMap\.GetProjectionExpression\<TIn,TOut\>\(\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression-1 'System\.Linq\.Expressions\.Expression\`1')  
The cached `src => new TOut { … }` expression\.

<a name='PropertyMapper.PropMap.GetProjectionExpression_TIn,TOut_(PropertyMapper.Masking.IFieldMask_TOut_)'></a>

## PropMap\.GetProjectionExpression\<TIn,TOut\>\(IFieldMask\<TOut\>\) Method

Returns a per\-request \(uncached\) [System\.Linq\.Expressions\.Expression&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression-1 'System\.Linq\.Expressions\.Expression\`1') that projects
[TIn](PropMap.md#PropertyMapper.PropMap.GetProjectionExpression_TIn,TOut_(PropertyMapper.Masking.IFieldMask_TOut_).TIn 'PropertyMapper\.PropMap\.GetProjectionExpression\<TIn,TOut\>\(PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.TIn') to [TOut](PropMap.md#PropertyMapper.PropMap.GetProjectionExpression_TIn,TOut_(PropertyMapper.Masking.IFieldMask_TOut_).TOut 'PropertyMapper\.PropMap\.GetProjectionExpression\<TIn,TOut\>\(PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.TOut'), omitting every field
listed in [mask](PropMap.md#PropertyMapper.PropMap.GetProjectionExpression_TIn,TOut_(PropertyMapper.Masking.IFieldMask_TOut_).mask 'PropertyMapper\.PropMap\.GetProjectionExpression\<TIn,TOut\>\(PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.mask') from the `MemberInitExpression`\.

```csharp
public System.Linq.Expressions.Expression<System.Func<TIn,TOut>> GetProjectionExpression<TIn,TOut>(PropertyMapper.Masking.IFieldMask<TOut> mask)
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.GetProjectionExpression_TIn,TOut_(PropertyMapper.Masking.IFieldMask_TOut_).TIn'></a>

`TIn`

Source type\.

<a name='PropertyMapper.PropMap.GetProjectionExpression_TIn,TOut_(PropertyMapper.Masking.IFieldMask_TOut_).TOut'></a>

`TOut`

Target type \(must have a parameterless constructor\)\.
#### Parameters

<a name='PropertyMapper.PropMap.GetProjectionExpression_TIn,TOut_(PropertyMapper.Masking.IFieldMask_TOut_).mask'></a>

`mask` [PropertyMapper\.Masking\.IFieldMask&lt;](IFieldMask_T_.md 'PropertyMapper\.Masking\.IFieldMask\<T\>')[TOut](PropMap.md#PropertyMapper.PropMap.GetProjectionExpression_TIn,TOut_(PropertyMapper.Masking.IFieldMask_TOut_).TOut 'PropertyMapper\.PropMap\.GetProjectionExpression\<TIn,TOut\>\(PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.TOut')[&gt;](IFieldMask_T_.md 'PropertyMapper\.Masking\.IFieldMask\<T\>')

Per\-request field mask; its [ExcludedFields](IFieldMask_T_.ExcludedFields.md 'PropertyMapper\.Masking\.IFieldMask\<T\>\.ExcludedFields') are
            omitted from the generated expression tree\.

#### Returns
[System\.Linq\.Expressions\.Expression&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression-1 'System\.Linq\.Expressions\.Expression\`1')[System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[TIn](PropMap.md#PropertyMapper.PropMap.GetProjectionExpression_TIn,TOut_(PropertyMapper.Masking.IFieldMask_TOut_).TIn 'PropertyMapper\.PropMap\.GetProjectionExpression\<TIn,TOut\>\(PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.TIn')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[TOut](PropMap.md#PropertyMapper.PropMap.GetProjectionExpression_TIn,TOut_(PropertyMapper.Masking.IFieldMask_TOut_).TOut 'PropertyMapper\.PropMap\.GetProjectionExpression\<TIn,TOut\>\(PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression-1 'System\.Linq\.Expressions\.Expression\`1')  
An uncached `src => new TOut { … }` expression with masked fields omitted\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [mask](PropMap.md#PropertyMapper.PropMap.GetProjectionExpression_TIn,TOut_(PropertyMapper.Masking.IFieldMask_TOut_).mask 'PropertyMapper\.PropMap\.GetProjectionExpression\<TIn,TOut\>\(PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.mask') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

### Remarks

Unlike the parameterless [GetProjectionExpression&lt;TIn,TOut&gt;\(\)](PropMap.GetProjectionExpression.md#PropertyMapper.PropMap.GetProjectionExpression_TIn,TOut_() 'PropertyMapper\.PropMap\.GetProjectionExpression\<TIn,TOut\>\(\)') overload,
this result is <b>not cached</b> because the excluded field set may differ per request
(e.g. per user role). The underlying [PropertyMapper\.Core\.MappingPlan](https://learn.microsoft.com/en-us/dotnet/api/propertymapper.core.mappingplan 'PropertyMapper\.Core\.MappingPlan') is still reused
from the internal cache.

When passed to an ORM such as EF Core, masked fields are excluded from the SQL
`SELECT` clause so sensitive data is never loaded from the database.

---
Generated by [DefaultDocumentation](https://github.com/Doraku/DefaultDocumentation 'https://github\.com/Doraku/DefaultDocumentation')