## PropMap\.Clone\<T\>\(T\) Method

Creates a shallow property\-copy clone of [source](PropMap.Clone.EFQITBIZ24A837MBN4YES00BD.md#PropertyMapper.PropMap.Clone_T_(T).source 'PropertyMapper\.PropMap\.Clone\<T\>\(T\)\.source')\.
Returns a new [T](PropMap.Clone.EFQITBIZ24A837MBN4YES00BD.md#PropertyMapper.PropMap.Clone_T_(T).T 'PropertyMapper\.PropMap\.Clone\<T\>\(T\)\.T') instance with all matching properties copied\.
Nested reference\-type properties are shared \(shallow clone\); nested value\-type properties are copied by value\.

```csharp
public T Clone<T>(T source)
    where T : class, new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.Clone_T_(T).T'></a>

`T`

Object type to clone \(must have a parameterless constructor\)\.
#### Parameters

<a name='PropertyMapper.PropMap.Clone_T_(T).source'></a>

`source` [T](PropMap.Clone.EFQITBIZ24A837MBN4YES00BD.md#PropertyMapper.PropMap.Clone_T_(T).T 'PropertyMapper\.PropMap\.Clone\<T\>\(T\)\.T')

The object to clone\.

#### Returns
[T](PropMap.Clone.EFQITBIZ24A837MBN4YES00BD.md#PropertyMapper.PropMap.Clone_T_(T).T 'PropertyMapper\.PropMap\.Clone\<T\>\(T\)\.T')  
A new [T](PropMap.Clone.EFQITBIZ24A837MBN4YES00BD.md#PropertyMapper.PropMap.Clone_T_(T).T 'PropertyMapper\.PropMap\.Clone\<T\>\(T\)\.T') with properties copied from [source](PropMap.Clone.EFQITBIZ24A837MBN4YES00BD.md#PropertyMapper.PropMap.Clone_T_(T).source 'PropertyMapper\.PropMap\.Clone\<T\>\(T\)\.source')\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [source](PropMap.Clone.EFQITBIZ24A837MBN4YES00BD.md#PropertyMapper.PropMap.Clone_T_(T).source 'PropertyMapper\.PropMap\.Clone\<T\>\(T\)\.source') is null\.