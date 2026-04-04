## IPropMap\.MapToFrozenDictionary\<TInKey,TInValue,TOutKey,TOutValue\>\(Dictionary\<TInKey,TInValue\>\) Method

Maps a dictionary and returns the result as a [System\.Collections\.Frozen\.FrozenDictionary&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.frozen.frozendictionary-2 'System\.Collections\.Frozen\.FrozenDictionary\`2')\.

```csharp
System.Collections.Frozen.FrozenDictionary<TOutKey,TOutValue> MapToFrozenDictionary<TInKey,TInValue,TOutKey,TOutValue>(System.Collections.Generic.Dictionary<TInKey,TInValue> source)
    where TInKey : notnull
    where TOutKey : notnull
    where TOutValue : new();
```
#### Type parameters

<a name='PropertyMapper.IPropMap.MapToFrozenDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).TInKey'></a>

`TInKey`

<a name='PropertyMapper.IPropMap.MapToFrozenDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).TInValue'></a>

`TInValue`

<a name='PropertyMapper.IPropMap.MapToFrozenDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).TOutKey'></a>

`TOutKey`

<a name='PropertyMapper.IPropMap.MapToFrozenDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).TOutValue'></a>

`TOutValue`
#### Parameters

<a name='PropertyMapper.IPropMap.MapToFrozenDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).source'></a>

`source` [System\.Collections\.Generic\.Dictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[TInKey](IPropMap.MapToFrozenDictionary.X1OO2OVUAKVKUTKEKOC7B6V52.md#PropertyMapper.IPropMap.MapToFrozenDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).TInKey 'PropertyMapper\.IPropMap\.MapToFrozenDictionary\<TInKey,TInValue,TOutKey,TOutValue\>\(System\.Collections\.Generic\.Dictionary\<TInKey,TInValue\>\)\.TInKey')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[TInValue](IPropMap.MapToFrozenDictionary.X1OO2OVUAKVKUTKEKOC7B6V52.md#PropertyMapper.IPropMap.MapToFrozenDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).TInValue 'PropertyMapper\.IPropMap\.MapToFrozenDictionary\<TInKey,TInValue,TOutKey,TOutValue\>\(System\.Collections\.Generic\.Dictionary\<TInKey,TInValue\>\)\.TInValue')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')

#### Returns
[System\.Collections\.Frozen\.FrozenDictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.frozen.frozendictionary-2 'System\.Collections\.Frozen\.FrozenDictionary\`2')[TOutKey](IPropMap.MapToFrozenDictionary.X1OO2OVUAKVKUTKEKOC7B6V52.md#PropertyMapper.IPropMap.MapToFrozenDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).TOutKey 'PropertyMapper\.IPropMap\.MapToFrozenDictionary\<TInKey,TInValue,TOutKey,TOutValue\>\(System\.Collections\.Generic\.Dictionary\<TInKey,TInValue\>\)\.TOutKey')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.frozen.frozendictionary-2 'System\.Collections\.Frozen\.FrozenDictionary\`2')[TOutValue](IPropMap.MapToFrozenDictionary.X1OO2OVUAKVKUTKEKOC7B6V52.md#PropertyMapper.IPropMap.MapToFrozenDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).TOutValue 'PropertyMapper\.IPropMap\.MapToFrozenDictionary\<TInKey,TInValue,TOutKey,TOutValue\>\(System\.Collections\.Generic\.Dictionary\<TInKey,TInValue\>\)\.TOutValue')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.frozen.frozendictionary-2 'System\.Collections\.Frozen\.FrozenDictionary\`2')