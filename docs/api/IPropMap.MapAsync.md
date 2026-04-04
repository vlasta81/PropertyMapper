#### [PropertyMapper](index.md 'index')
### [PropertyMapper](PropertyMapper.md 'PropertyMapper').[IPropMap](IPropMap.md 'PropertyMapper\.IPropMap')

## IPropMap\.MapAsync Method

| Overloads | |
| :--- | :--- |
| [MapAsync&lt;TIn,TOut&gt;\(IEnumerable&lt;TIn&gt;, CancellationToken\)](IPropMap.MapAsync.md#PropertyMapper.IPropMap.MapAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Threading.CancellationToken) 'PropertyMapper\.IPropMap\.MapAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, System\.Threading\.CancellationToken\)') | Asynchronously maps all items in [source](IPropMap.md#PropertyMapper.IPropMap.MapAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Threading.CancellationToken).source 'PropertyMapper\.IPropMap\.MapAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, System\.Threading\.CancellationToken\)\.source')\. |
| [MapAsync&lt;TIn,TOut&gt;\(TIn, CancellationToken\)](IPropMap.MapAsync.md#PropertyMapper.IPropMap.MapAsync_TIn,TOut_(TIn,System.Threading.CancellationToken) 'PropertyMapper\.IPropMap\.MapAsync\<TIn,TOut\>\(TIn, System\.Threading\.CancellationToken\)') | Asynchronously maps a single [source](IPropMap.md#PropertyMapper.IPropMap.MapAsync_TIn,TOut_(TIn,System.Threading.CancellationToken).source 'PropertyMapper\.IPropMap\.MapAsync\<TIn,TOut\>\(TIn, System\.Threading\.CancellationToken\)\.source') item\. |

<a name='PropertyMapper.IPropMap.MapAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Threading.CancellationToken)'></a>

## IPropMap\.MapAsync\<TIn,TOut\>\(IEnumerable\<TIn\>, CancellationToken\) Method

Asynchronously maps all items in [source](IPropMap.md#PropertyMapper.IPropMap.MapAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Threading.CancellationToken).source 'PropertyMapper\.IPropMap\.MapAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, System\.Threading\.CancellationToken\)\.source')\.

```csharp
System.Threading.Tasks.Task<System.Collections.Generic.List<TOut>> MapAsync<TIn,TOut>(System.Collections.Generic.IEnumerable<TIn> source, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken))
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.IPropMap.MapAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Threading.CancellationToken).TIn'></a>

`TIn`

<a name='PropertyMapper.IPropMap.MapAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Threading.CancellationToken).TOut'></a>

`TOut`
#### Parameters

<a name='PropertyMapper.IPropMap.MapAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Threading.CancellationToken).source'></a>

`source` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[TIn](IPropMap.md#PropertyMapper.IPropMap.MapAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Threading.CancellationToken).TIn 'PropertyMapper\.IPropMap\.MapAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, System\.Threading\.CancellationToken\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='PropertyMapper.IPropMap.MapAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[TOut](IPropMap.md#PropertyMapper.IPropMap.MapAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Threading.CancellationToken).TOut 'PropertyMapper\.IPropMap\.MapAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, System\.Threading\.CancellationToken\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')

<a name='PropertyMapper.IPropMap.MapAsync_TIn,TOut_(TIn,System.Threading.CancellationToken)'></a>

## IPropMap\.MapAsync\<TIn,TOut\>\(TIn, CancellationToken\) Method

Asynchronously maps a single [source](IPropMap.md#PropertyMapper.IPropMap.MapAsync_TIn,TOut_(TIn,System.Threading.CancellationToken).source 'PropertyMapper\.IPropMap\.MapAsync\<TIn,TOut\>\(TIn, System\.Threading\.CancellationToken\)\.source') item\.

```csharp
System.Threading.Tasks.Task<TOut> MapAsync<TIn,TOut>(TIn source, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken))
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.IPropMap.MapAsync_TIn,TOut_(TIn,System.Threading.CancellationToken).TIn'></a>

`TIn`

<a name='PropertyMapper.IPropMap.MapAsync_TIn,TOut_(TIn,System.Threading.CancellationToken).TOut'></a>

`TOut`
#### Parameters

<a name='PropertyMapper.IPropMap.MapAsync_TIn,TOut_(TIn,System.Threading.CancellationToken).source'></a>

`source` [TIn](IPropMap.md#PropertyMapper.IPropMap.MapAsync_TIn,TOut_(TIn,System.Threading.CancellationToken).TIn 'PropertyMapper\.IPropMap\.MapAsync\<TIn,TOut\>\(TIn, System\.Threading\.CancellationToken\)\.TIn')

<a name='PropertyMapper.IPropMap.MapAsync_TIn,TOut_(TIn,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[TOut](IPropMap.md#PropertyMapper.IPropMap.MapAsync_TIn,TOut_(TIn,System.Threading.CancellationToken).TOut 'PropertyMapper\.IPropMap\.MapAsync\<TIn,TOut\>\(TIn, System\.Threading\.CancellationToken\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')

---
Generated by [DefaultDocumentation](https://github.com/Doraku/DefaultDocumentation 'https://github\.com/Doraku/DefaultDocumentation')