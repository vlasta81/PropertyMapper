## IPropMap\.MapThenApplyMask\<TIn,TOut\>\(TIn, IFieldMask\<TOut\>\) Method

Maps [source](IPropMap.MapThenApplyMask.ACKN4X1ULBVMQP8ZU967K3OB9.md#PropertyMapper.IPropMap.MapThenApplyMask_TIn,TOut_(TIn,PropertyMapper.Masking.IFieldMask_TOut_).source 'PropertyMapper\.IPropMap\.MapThenApplyMask\<TIn,TOut\>\(TIn, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.source'), then applies [mask](IPropMap.MapThenApplyMask.ACKN4X1ULBVMQP8ZU967K3OB9.md#PropertyMapper.IPropMap.MapThenApplyMask_TIn,TOut_(TIn,PropertyMapper.Masking.IFieldMask_TOut_).mask 'PropertyMapper\.IPropMap\.MapThenApplyMask\<TIn,TOut\>\(TIn, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.mask') to zero out excluded fields\.

```csharp
TOut MapThenApplyMask<TIn,TOut>(TIn source, PropertyMapper.Masking.IFieldMask<TOut> mask)
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.IPropMap.MapThenApplyMask_TIn,TOut_(TIn,PropertyMapper.Masking.IFieldMask_TOut_).TIn'></a>

`TIn`

<a name='PropertyMapper.IPropMap.MapThenApplyMask_TIn,TOut_(TIn,PropertyMapper.Masking.IFieldMask_TOut_).TOut'></a>

`TOut`
#### Parameters

<a name='PropertyMapper.IPropMap.MapThenApplyMask_TIn,TOut_(TIn,PropertyMapper.Masking.IFieldMask_TOut_).source'></a>

`source` [TIn](IPropMap.MapThenApplyMask.ACKN4X1ULBVMQP8ZU967K3OB9.md#PropertyMapper.IPropMap.MapThenApplyMask_TIn,TOut_(TIn,PropertyMapper.Masking.IFieldMask_TOut_).TIn 'PropertyMapper\.IPropMap\.MapThenApplyMask\<TIn,TOut\>\(TIn, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.TIn')

<a name='PropertyMapper.IPropMap.MapThenApplyMask_TIn,TOut_(TIn,PropertyMapper.Masking.IFieldMask_TOut_).mask'></a>

`mask` [PropertyMapper\.Masking\.IFieldMask&lt;](IFieldMask_T_.md 'PropertyMapper\.Masking\.IFieldMask\<T\>')[TOut](IPropMap.MapThenApplyMask.ACKN4X1ULBVMQP8ZU967K3OB9.md#PropertyMapper.IPropMap.MapThenApplyMask_TIn,TOut_(TIn,PropertyMapper.Masking.IFieldMask_TOut_).TOut 'PropertyMapper\.IPropMap\.MapThenApplyMask\<TIn,TOut\>\(TIn, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.TOut')[&gt;](IFieldMask_T_.md 'PropertyMapper\.Masking\.IFieldMask\<T\>')

#### Returns
[TOut](IPropMap.MapThenApplyMask.ACKN4X1ULBVMQP8ZU967K3OB9.md#PropertyMapper.IPropMap.MapThenApplyMask_TIn,TOut_(TIn,PropertyMapper.Masking.IFieldMask_TOut_).TOut 'PropertyMapper\.IPropMap\.MapThenApplyMask\<TIn,TOut\>\(TIn, PropertyMapper\.Masking\.IFieldMask\<TOut\>\)\.TOut')