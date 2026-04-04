## PropMap\.Warmup\<TIn,TOut\>\(\) Method

Pre\-compiles the mapping delegate for the [TIn](PropMap.Warmup_TIn,TOut_().md#PropertyMapper.PropMap.Warmup_TIn,TOut_().TIn 'PropertyMapper\.PropMap\.Warmup\<TIn,TOut\>\(\)\.TIn')→[TOut](PropMap.Warmup_TIn,TOut_().md#PropertyMapper.PropMap.Warmup_TIn,TOut_().TOut 'PropertyMapper\.PropMap\.Warmup\<TIn,TOut\>\(\)\.TOut')
type pair and caches it, eliminating first\-call JIT and IL\-emit overhead\.

```csharp
public void Warmup<TIn,TOut>()
    where TOut : new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.Warmup_TIn,TOut_().TIn'></a>

`TIn`

Source type\.

<a name='PropertyMapper.PropMap.Warmup_TIn,TOut_().TOut'></a>

`TOut`

Target type \(must have a parameterless constructor\)\.

### Remarks
Call during application startup for each type pair used on hot request paths\.
For multiple pairs, prefer [WarmupBatch\(Type\[\]\)](PropMap.WarmupBatch.G8U901V5CV76SI3XPQ8A9C5H7.md 'PropertyMapper\.PropMap\.WarmupBatch\(System\.Type\[\]\)') which processes them all
in a single sequential pass and is more efficient than repeated [Warmup&lt;TIn,TOut&gt;\(\)](PropMap.Warmup_TIn,TOut_().md 'PropertyMapper\.PropMap\.Warmup\<TIn,TOut\>\(\)') calls\.