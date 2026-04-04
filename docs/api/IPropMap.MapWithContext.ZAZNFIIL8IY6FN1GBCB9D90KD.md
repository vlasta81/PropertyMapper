## IPropMap\.MapWithContext\<TIn,TOut,TCtx\>\(TIn, TCtx\) Method

Maps [source](IPropMap.MapWithContext.ZAZNFIIL8IY6FN1GBCB9D90KD.md#PropertyMapper.IPropMap.MapWithContext_TIn,TOut,TCtx_(TIn,TCtx).source 'PropertyMapper\.IPropMap\.MapWithContext\<TIn,TOut,TCtx\>\(TIn, TCtx\)\.source') and applies context\-aware setters registered via
[MapFromWithContext&lt;TCtx,TProp&gt;\(Expression&lt;Func&lt;TOut,TProp&gt;&gt;, Func&lt;TIn,TCtx,TProp&gt;\)](TypePairConfiguration_TIn,TOut_.MapFromWithContext.TFC4YI6R2TF03EF24S2XZQWZD.md 'PropertyMapper\.Configuration\.TypePairConfiguration\<TIn,TOut\>\.MapFromWithContext\<TCtx,TProp\>\(System\.Linq\.Expressions\.Expression\<System\.Func\<TOut,TProp\>\>, System\.Func\<TIn,TCtx,TProp\>\)')\.

```csharp
TOut MapWithContext<TIn,TOut,TCtx>(TIn source, TCtx context)
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.IPropMap.MapWithContext_TIn,TOut,TCtx_(TIn,TCtx).TIn'></a>

`TIn`

<a name='PropertyMapper.IPropMap.MapWithContext_TIn,TOut,TCtx_(TIn,TCtx).TOut'></a>

`TOut`

<a name='PropertyMapper.IPropMap.MapWithContext_TIn,TOut,TCtx_(TIn,TCtx).TCtx'></a>

`TCtx`
#### Parameters

<a name='PropertyMapper.IPropMap.MapWithContext_TIn,TOut,TCtx_(TIn,TCtx).source'></a>

`source` [TIn](IPropMap.MapWithContext.ZAZNFIIL8IY6FN1GBCB9D90KD.md#PropertyMapper.IPropMap.MapWithContext_TIn,TOut,TCtx_(TIn,TCtx).TIn 'PropertyMapper\.IPropMap\.MapWithContext\<TIn,TOut,TCtx\>\(TIn, TCtx\)\.TIn')

<a name='PropertyMapper.IPropMap.MapWithContext_TIn,TOut,TCtx_(TIn,TCtx).context'></a>

`context` [TCtx](IPropMap.MapWithContext.ZAZNFIIL8IY6FN1GBCB9D90KD.md#PropertyMapper.IPropMap.MapWithContext_TIn,TOut,TCtx_(TIn,TCtx).TCtx 'PropertyMapper\.IPropMap\.MapWithContext\<TIn,TOut,TCtx\>\(TIn, TCtx\)\.TCtx')

#### Returns
[TOut](IPropMap.MapWithContext.ZAZNFIIL8IY6FN1GBCB9D90KD.md#PropertyMapper.IPropMap.MapWithContext_TIn,TOut,TCtx_(TIn,TCtx).TOut 'PropertyMapper\.IPropMap\.MapWithContext\<TIn,TOut,TCtx\>\(TIn, TCtx\)\.TOut')