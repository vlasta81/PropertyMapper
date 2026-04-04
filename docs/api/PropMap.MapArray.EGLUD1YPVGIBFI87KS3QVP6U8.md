## PropMap\.MapArray\<TIn,TOut\>\(TIn\[\]\) Method

Maps a `TIn[]` source array to a new `TOut[]`\.
Delegates to [MapBatch&lt;TIn,TOut&gt;\(ReadOnlySpan&lt;TIn&gt;\)](PropMap.MapBatch.AAPNY1A78JL9AD0VK3C7TBLPA.md 'PropertyMapper\.PropMap\.MapBatch\<TIn,TOut\>\(System\.ReadOnlySpan\<TIn\>\)') via a span:
single allocation, no enumerator overhead\.

```csharp
public TOut[] MapArray<TIn,TOut>(TIn[] source)
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.MapArray_TIn,TOut_(TIn[]).TIn'></a>

`TIn`

Source element type\.

<a name='PropertyMapper.PropMap.MapArray_TIn,TOut_(TIn[]).TOut'></a>

`TOut`

Target element type \(must have a parameterless constructor\)\.
#### Parameters

<a name='PropertyMapper.PropMap.MapArray_TIn,TOut_(TIn[]).source'></a>

`source` [TIn](PropMap.MapArray.EGLUD1YPVGIBFI87KS3QVP6U8.md#PropertyMapper.PropMap.MapArray_TIn,TOut_(TIn[]).TIn 'PropertyMapper\.PropMap\.MapArray\<TIn,TOut\>\(TIn\[\]\)\.TIn')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

Source array to map\.

#### Returns
[TOut](PropMap.MapArray.EGLUD1YPVGIBFI87KS3QVP6U8.md#PropertyMapper.PropMap.MapArray_TIn,TOut_(TIn[]).TOut 'PropertyMapper\.PropMap\.MapArray\<TIn,TOut\>\(TIn\[\]\)\.TOut')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')  
A new `TOut[]` containing the mapped elements,
or an empty array when [source](PropMap.MapArray.EGLUD1YPVGIBFI87KS3QVP6U8.md#PropertyMapper.PropMap.MapArray_TIn,TOut_(TIn[]).source 'PropertyMapper\.PropMap\.MapArray\<TIn,TOut\>\(TIn\[\]\)\.source') is empty\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [source](PropMap.MapArray.EGLUD1YPVGIBFI87KS3QVP6U8.md#PropertyMapper.PropMap.MapArray_TIn,TOut_(TIn[]).source 'PropertyMapper\.PropMap\.MapArray\<TIn,TOut\>\(TIn\[\]\)\.source') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.