#### [PropertyMapper](index.md 'index')
### [PropertyMapper](PropertyMapper.md 'PropertyMapper').[PropMap](PropMap.md 'PropertyMapper\.PropMap')

## PropMap\.MapAsync Method

| Overloads | |
| :--- | :--- |
| [MapAsync&lt;TIn,TOut&gt;\(IEnumerable&lt;TIn&gt;, CancellationToken\)](PropMap.MapAsync.md#PropertyMapper.PropMap.MapAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Threading.CancellationToken) 'PropertyMapper\.PropMap\.MapAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, System\.Threading\.CancellationToken\)') | Asynchronously maps a collection of items\. Offloads the entire synchronous mapping loop to a thread\-pool thread in a single [System\.Threading\.Tasks\.Task\.Run&lt;&gt;\.Func\{&lt;&gt;\.Threading\.CancellationToken\)](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.run--1#system-threading-tasks-task-run--1(system-func{--0}-system-threading-cancellationtoken) 'System\.Threading\.Tasks\.Task\.Run\`\`1\(System\.Func\{\`\`0\},System\.Threading\.CancellationToken\)') call, avoiding per\-item scheduling overhead\. |
| [MapAsync&lt;TIn,TOut&gt;\(TIn, CancellationToken\)](PropMap.MapAsync.md#PropertyMapper.PropMap.MapAsync_TIn,TOut_(TIn,System.Threading.CancellationToken) 'PropertyMapper\.PropMap\.MapAsync\<TIn,TOut\>\(TIn, System\.Threading\.CancellationToken\)') | Offloads the synchronous [Map&lt;TIn,TOut&gt;\(TIn\)](PropMap.Map.GHQD3IQP9BO7WZYKLBLGJIYSE.md 'PropertyMapper\.PropMap\.Map\<TIn,TOut\>\(TIn\)') call to a thread\-pool thread so the caller's `await` does not block\. |

<a name='PropertyMapper.PropMap.MapAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Threading.CancellationToken)'></a>

## PropMap\.MapAsync\<TIn,TOut\>\(IEnumerable\<TIn\>, CancellationToken\) Method

Asynchronously maps a collection of items\.
Offloads the entire synchronous mapping loop to a thread\-pool thread in a single
[System\.Threading\.Tasks\.Task\.Run&lt;&gt;\.Func\{&lt;&gt;\.Threading\.CancellationToken\)](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.run--1#system-threading-tasks-task-run--1(system-func{--0}-system-threading-cancellationtoken) 'System\.Threading\.Tasks\.Task\.Run\`\`1\(System\.Func\{\`\`0\},System\.Threading\.CancellationToken\)') call, avoiding
per\-item scheduling overhead\.

```csharp
public System.Threading.Tasks.Task<System.Collections.Generic.List<TOut>> MapAsync<TIn,TOut>(System.Collections.Generic.IEnumerable<TIn> source, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken))
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.MapAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Threading.CancellationToken).TIn'></a>

`TIn`

Source element type\.

<a name='PropertyMapper.PropMap.MapAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Threading.CancellationToken).TOut'></a>

`TOut`

Target element type \(must have a parameterless constructor\)\.
#### Parameters

<a name='PropertyMapper.PropMap.MapAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Threading.CancellationToken).source'></a>

`source` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[TIn](PropMap.md#PropertyMapper.PropMap.MapAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Threading.CancellationToken).TIn 'PropertyMapper\.PropMap\.MapAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, System\.Threading\.CancellationToken\)\.TIn')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The sequence of items to map\.

<a name='PropertyMapper.PropMap.MapAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token observed inside the mapping loop to support cooperative cancellation\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[System\.Collections\.Generic\.List&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[TOut](PropMap.md#PropertyMapper.PropMap.MapAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Threading.CancellationToken).TOut 'PropertyMapper\.PropMap\.MapAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, System\.Threading\.CancellationToken\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that resolves to a [System\.Collections\.Generic\.List&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1 'System\.Collections\.Generic\.List\`1') of mapped [TOut](PropMap.md#PropertyMapper.PropMap.MapAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Threading.CancellationToken).TOut 'PropertyMapper\.PropMap\.MapAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, System\.Threading\.CancellationToken\)\.TOut') instances\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [source](PropMap.md#PropertyMapper.PropMap.MapAsync_TIn,TOut_(System.Collections.Generic.IEnumerable_TIn_,System.Threading.CancellationToken).source 'PropertyMapper\.PropMap\.MapAsync\<TIn,TOut\>\(System\.Collections\.Generic\.IEnumerable\<TIn\>, System\.Threading\.CancellationToken\)\.source') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

<a name='PropertyMapper.PropMap.MapAsync_TIn,TOut_(TIn,System.Threading.CancellationToken)'></a>

## PropMap\.MapAsync\<TIn,TOut\>\(TIn, CancellationToken\) Method

Offloads the synchronous [Map&lt;TIn,TOut&gt;\(TIn\)](PropMap.Map.GHQD3IQP9BO7WZYKLBLGJIYSE.md 'PropertyMapper\.PropMap\.Map\<TIn,TOut\>\(TIn\)') call to a thread\-pool thread
so the caller's `await` does not block\.

```csharp
public System.Threading.Tasks.Task<TOut> MapAsync<TIn,TOut>(TIn source, System.Threading.CancellationToken cancellationToken=default(System.Threading.CancellationToken))
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.MapAsync_TIn,TOut_(TIn,System.Threading.CancellationToken).TIn'></a>

`TIn`

Source type\.

<a name='PropertyMapper.PropMap.MapAsync_TIn,TOut_(TIn,System.Threading.CancellationToken).TOut'></a>

`TOut`

Target type \(must have a parameterless constructor\)\.
#### Parameters

<a name='PropertyMapper.PropMap.MapAsync_TIn,TOut_(TIn,System.Threading.CancellationToken).source'></a>

`source` [TIn](PropMap.md#PropertyMapper.PropMap.MapAsync_TIn,TOut_(TIn,System.Threading.CancellationToken).TIn 'PropertyMapper\.PropMap\.MapAsync\<TIn,TOut\>\(TIn, System\.Threading\.CancellationToken\)\.TIn')

Source object to map\.

<a name='PropertyMapper.PropMap.MapAsync_TIn,TOut_(TIn,System.Threading.CancellationToken).cancellationToken'></a>

`cancellationToken` [System\.Threading\.CancellationToken](https://learn.microsoft.com/en-us/dotnet/api/system.threading.cancellationtoken 'System\.Threading\.CancellationToken')

Token that can cancel the queued work item before it starts\.

#### Returns
[System\.Threading\.Tasks\.Task&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')[TOut](PropMap.md#PropertyMapper.PropMap.MapAsync_TIn,TOut_(TIn,System.Threading.CancellationToken).TOut 'PropertyMapper\.PropMap\.MapAsync\<TIn,TOut\>\(TIn, System\.Threading\.CancellationToken\)\.TOut')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task-1 'System\.Threading\.Tasks\.Task\`1')  
A task that resolves to a newly mapped [TOut](PropMap.md#PropertyMapper.PropMap.MapAsync_TIn,TOut_(TIn,System.Threading.CancellationToken).TOut 'PropertyMapper\.PropMap\.MapAsync\<TIn,TOut\>\(TIn, System\.Threading\.CancellationToken\)\.TOut') instance\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [source](PropMap.md#PropertyMapper.PropMap.MapAsync_TIn,TOut_(TIn,System.Threading.CancellationToken).source 'PropertyMapper\.PropMap\.MapAsync\<TIn,TOut\>\(TIn, System\.Threading\.CancellationToken\)\.source') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

---
Generated by [DefaultDocumentation](https://github.com/Doraku/DefaultDocumentation 'https://github\.com/Doraku/DefaultDocumentation')