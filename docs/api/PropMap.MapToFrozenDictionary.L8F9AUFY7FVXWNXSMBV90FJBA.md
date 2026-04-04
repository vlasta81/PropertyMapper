## PropMap\.MapToFrozenDictionary\<TInKey,TInValue,TOutKey,TOutValue\>\(Dictionary\<TInKey,TInValue\>\) Method

Maps a dictionary and returns the result as a [System\.Collections\.Frozen\.FrozenDictionary&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.frozen.frozendictionary-2 'System\.Collections\.Frozen\.FrozenDictionary\`2')\.
Equivalent to [MapDictionary&lt;TInKey,TInValue,TOutKey,TOutValue&gt;\(Dictionary&lt;TInKey,TInValue&gt;\)](PropMap.MapDictionary.RHXKN9BDH2AYXGV07P5MKV1ZA.md 'PropertyMapper\.PropMap\.MapDictionary\<TInKey,TInValue,TOutKey,TOutValue\>\(System\.Collections\.Generic\.Dictionary\<TInKey,TInValue\>\)') followed by
`ToFrozenDictionary()`, but expressed as a single call\.

```csharp
public System.Collections.Frozen.FrozenDictionary<TOutKey,TOutValue> MapToFrozenDictionary<TInKey,TInValue,TOutKey,TOutValue>(System.Collections.Generic.Dictionary<TInKey,TInValue> source)
    where TInKey : notnull
    where TOutKey : notnull
    where TOutValue : new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.MapToFrozenDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).TInKey'></a>

`TInKey`

Source key type\.

<a name='PropertyMapper.PropMap.MapToFrozenDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).TInValue'></a>

`TInValue`

Source value type\.

<a name='PropertyMapper.PropMap.MapToFrozenDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).TOutKey'></a>

`TOutKey`

Target key type\.

<a name='PropertyMapper.PropMap.MapToFrozenDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).TOutValue'></a>

`TOutValue`

Target value type \(must have a parameterless constructor\)\.
#### Parameters

<a name='PropertyMapper.PropMap.MapToFrozenDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).source'></a>

`source` [System\.Collections\.Generic\.Dictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[TInKey](PropMap.MapToFrozenDictionary.L8F9AUFY7FVXWNXSMBV90FJBA.md#PropertyMapper.PropMap.MapToFrozenDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).TInKey 'PropertyMapper\.PropMap\.MapToFrozenDictionary\<TInKey,TInValue,TOutKey,TOutValue\>\(System\.Collections\.Generic\.Dictionary\<TInKey,TInValue\>\)\.TInKey')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[TInValue](PropMap.MapToFrozenDictionary.L8F9AUFY7FVXWNXSMBV90FJBA.md#PropertyMapper.PropMap.MapToFrozenDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).TInValue 'PropertyMapper\.PropMap\.MapToFrozenDictionary\<TInKey,TInValue,TOutKey,TOutValue\>\(System\.Collections\.Generic\.Dictionary\<TInKey,TInValue\>\)\.TInValue')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')

The dictionary to map\.

#### Returns
[System\.Collections\.Frozen\.FrozenDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.frozen.frozendictionary-2 'System\.Collections\.Frozen\.FrozenDictionary\`2')[TOutKey](PropMap.MapToFrozenDictionary.L8F9AUFY7FVXWNXSMBV90FJBA.md#PropertyMapper.PropMap.MapToFrozenDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).TOutKey 'PropertyMapper\.PropMap\.MapToFrozenDictionary\<TInKey,TInValue,TOutKey,TOutValue\>\(System\.Collections\.Generic\.Dictionary\<TInKey,TInValue\>\)\.TOutKey')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.frozen.frozendictionary-2 'System\.Collections\.Frozen\.FrozenDictionary\`2')[TOutValue](PropMap.MapToFrozenDictionary.L8F9AUFY7FVXWNXSMBV90FJBA.md#PropertyMapper.PropMap.MapToFrozenDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).TOutValue 'PropertyMapper\.PropMap\.MapToFrozenDictionary\<TInKey,TInValue,TOutKey,TOutValue\>\(System\.Collections\.Generic\.Dictionary\<TInKey,TInValue\>\)\.TOutValue')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.frozen.frozendictionary-2 'System\.Collections\.Frozen\.FrozenDictionary\`2')  
A new [System\.Collections\.Frozen\.FrozenDictionary&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.frozen.frozendictionary-2 'System\.Collections\.Frozen\.FrozenDictionary\`2') with mapped keys and values\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [source](PropMap.MapToFrozenDictionary.L8F9AUFY7FVXWNXSMBV90FJBA.md#PropertyMapper.PropMap.MapToFrozenDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).source 'PropertyMapper\.PropMap\.MapToFrozenDictionary\<TInKey,TInValue,TOutKey,TOutValue\>\(System\.Collections\.Generic\.Dictionary\<TInKey,TInValue\>\)\.source') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.