## PropMap\.MapWithContext\<TIn,TOut,TCtx\>\(TIn, TCtx\) Method

Maps [source](PropMap.MapWithContext.VBYRHLVUYW6CLL935CECSDD7A.md#PropertyMapper.PropMap.MapWithContext_TIn,TOut,TCtx_(TIn,TCtx).source 'PropertyMapper\.PropMap\.MapWithContext\<TIn,TOut,TCtx\>\(TIn, TCtx\)\.source') to a new [TOut](PropMap.MapWithContext.VBYRHLVUYW6CLL935CECSDD7A.md#PropertyMapper.PropMap.MapWithContext_TIn,TOut,TCtx_(TIn,TCtx).TOut 'PropertyMapper\.PropMap\.MapWithContext\<TIn,TOut,TCtx\>\(TIn, TCtx\)\.TOut') instance, then applies
all context\-aware setters registered via
[MapFromWithContext&lt;TCtx,TProp&gt;\(Expression&lt;Func&lt;TOut,TProp&gt;&gt;, Func&lt;TIn,TCtx,TProp&gt;\)](TypePairConfiguration_TIn,TOut_.MapFromWithContext.TFC4YI6R2TF03EF24S2XZQWZD.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.MapFromWithContext\<TCtx,TProp\>\(System\.Linq\.Expressions\.Expression\<System\.Func\<TOut,TProp\>\>, System\.Func\<TIn,TCtx,TProp\>\)') using the
supplied [context](PropMap.MapWithContext.VBYRHLVUYW6CLL935CECSDD7A.md#PropertyMapper.PropMap.MapWithContext_TIn,TOut,TCtx_(TIn,TCtx).context 'PropertyMapper\.PropMap\.MapWithContext\<TIn,TOut,TCtx\>\(TIn, TCtx\)\.context') value\.

```csharp
public TOut MapWithContext<TIn,TOut,TCtx>(TIn source, TCtx context)
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.MapWithContext_TIn,TOut,TCtx_(TIn,TCtx).TIn'></a>

`TIn`

Source type\.

<a name='PropertyMapper.PropMap.MapWithContext_TIn,TOut,TCtx_(TIn,TCtx).TOut'></a>

`TOut`

Target type \(must have a parameterless constructor\)\.

<a name='PropertyMapper.PropMap.MapWithContext_TIn,TOut,TCtx_(TIn,TCtx).TCtx'></a>

`TCtx`

Context type — passed per call, not cached\.
#### Parameters

<a name='PropertyMapper.PropMap.MapWithContext_TIn,TOut,TCtx_(TIn,TCtx).source'></a>

`source` [TIn](PropMap.MapWithContext.VBYRHLVUYW6CLL935CECSDD7A.md#PropertyMapper.PropMap.MapWithContext_TIn,TOut,TCtx_(TIn,TCtx).TIn 'PropertyMapper\.PropMap\.MapWithContext\<TIn,TOut,TCtx\>\(TIn, TCtx\)\.TIn')

Source object to map\.

<a name='PropertyMapper.PropMap.MapWithContext_TIn,TOut,TCtx_(TIn,TCtx).context'></a>

`context` [TCtx](PropMap.MapWithContext.VBYRHLVUYW6CLL935CECSDD7A.md#PropertyMapper.PropMap.MapWithContext_TIn,TOut,TCtx_(TIn,TCtx).TCtx 'PropertyMapper\.PropMap\.MapWithContext\<TIn,TOut,TCtx\>\(TIn, TCtx\)\.TCtx')

Per\-call context passed to all registered context\-aware setters\.

#### Returns
[TOut](PropMap.MapWithContext.VBYRHLVUYW6CLL935CECSDD7A.md#PropertyMapper.PropMap.MapWithContext_TIn,TOut,TCtx_(TIn,TCtx).TOut 'PropertyMapper\.PropMap\.MapWithContext\<TIn,TOut,TCtx\>\(TIn, TCtx\)\.TOut')  
A new [TOut](PropMap.MapWithContext.VBYRHLVUYW6CLL935CECSDD7A.md#PropertyMapper.PropMap.MapWithContext_TIn,TOut,TCtx_(TIn,TCtx).TOut 'PropertyMapper\.PropMap\.MapWithContext\<TIn,TOut,TCtx\>\(TIn, TCtx\)\.TOut') with all matched properties and context\-derived values applied\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [source](PropMap.MapWithContext.VBYRHLVUYW6CLL935CECSDD7A.md#PropertyMapper.PropMap.MapWithContext_TIn,TOut,TCtx_(TIn,TCtx).source 'PropertyMapper\.PropMap\.MapWithContext\<TIn,TOut,TCtx\>\(TIn, TCtx\)\.source') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
When [context](PropMap.MapWithContext.VBYRHLVUYW6CLL935CECSDD7A.md#PropertyMapper.PropMap.MapWithContext_TIn,TOut,TCtx_(TIn,TCtx).context 'PropertyMapper\.PropMap\.MapWithContext\<TIn,TOut,TCtx\>\(TIn, TCtx\)\.context') is an [System\.IServiceProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iserviceprovider 'System\.IServiceProvider') or [Microsoft\.Extensions\.DependencyInjection\.IServiceScopeFactory](https://learn.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicescopefactory 'Microsoft\.Extensions\.DependencyInjection\.IServiceScopeFactory')\.

### Remarks

The base IL mapping (name-matched properties) executes first; context-aware setters run after.
[context](PropMap.MapWithContext.VBYRHLVUYW6CLL935CECSDD7A.md#PropertyMapper.PropMap.MapWithContext_TIn,TOut,TCtx_(TIn,TCtx).context 'PropertyMapper\.PropMap\.MapWithContext\<TIn,TOut,TCtx\>\(TIn, TCtx\)\.context') is passed on each call and is <b>never</b> compiled into the cached
IL delegate — the mapper remains stateless and thread-safe.

Typical uses: currency conversion, tenant-specific transforms, per-request user preferences.