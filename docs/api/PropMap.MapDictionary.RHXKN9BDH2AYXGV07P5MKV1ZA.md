## PropMap\.MapDictionary\<TInKey,TInValue,TOutKey,TOutValue\>\(Dictionary\<TInKey,TInValue\>\) Method

Maps a source dictionary, applying the compiled value mapper to each entry\.
When [TInKey](PropMap.MapDictionary.RHXKN9BDH2AYXGV07P5MKV1ZA.md#PropertyMapper.PropMap.MapDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).TInKey 'PropertyMapper\.PropMap\.MapDictionary\<TInKey,TInValue,TOutKey,TOutValue\>\(System\.Collections\.Generic\.Dictionary\<TInKey,TInValue\>\)\.TInKey') and [TOutKey](PropMap.MapDictionary.RHXKN9BDH2AYXGV07P5MKV1ZA.md#PropertyMapper.PropMap.MapDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).TOutKey 'PropertyMapper\.PropMap\.MapDictionary\<TInKey,TInValue,TOutKey,TOutValue\>\(System\.Collections\.Generic\.Dictionary\<TInKey,TInValue\>\)\.TOutKey') differ, the key is cast
via the validated [PropertyMapper\.PropMap\.KeyCast&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/propertymapper.propmap.keycast-2 'PropertyMapper\.PropMap\.KeyCast\`2') delegate; otherwise a direct cast is used\.

```csharp
public System.Collections.Generic.Dictionary<TOutKey,TOutValue> MapDictionary<TInKey,TInValue,TOutKey,TOutValue>(System.Collections.Generic.Dictionary<TInKey,TInValue> source)
    where TInKey : notnull
    where TOutKey : notnull
    where TOutValue : new();
```
#### Type parameters

<a name='PropertyMapper.PropMap.MapDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).TInKey'></a>

`TInKey`

Source key type\.

<a name='PropertyMapper.PropMap.MapDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).TInValue'></a>

`TInValue`

Source value type\.

<a name='PropertyMapper.PropMap.MapDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).TOutKey'></a>

`TOutKey`

Target key type\.

<a name='PropertyMapper.PropMap.MapDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).TOutValue'></a>

`TOutValue`

Target value type \(must have a parameterless constructor\)\.
#### Parameters

<a name='PropertyMapper.PropMap.MapDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).source'></a>

`source` [System\.Collections\.Generic\.Dictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[TInKey](PropMap.MapDictionary.RHXKN9BDH2AYXGV07P5MKV1ZA.md#PropertyMapper.PropMap.MapDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).TInKey 'PropertyMapper\.PropMap\.MapDictionary\<TInKey,TInValue,TOutKey,TOutValue\>\(System\.Collections\.Generic\.Dictionary\<TInKey,TInValue\>\)\.TInKey')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[TInValue](PropMap.MapDictionary.RHXKN9BDH2AYXGV07P5MKV1ZA.md#PropertyMapper.PropMap.MapDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).TInValue 'PropertyMapper\.PropMap\.MapDictionary\<TInKey,TInValue,TOutKey,TOutValue\>\(System\.Collections\.Generic\.Dictionary\<TInKey,TInValue\>\)\.TInValue')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')

The source dictionary to map\.

#### Returns
[System\.Collections\.Generic\.Dictionary&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[TOutKey](PropMap.MapDictionary.RHXKN9BDH2AYXGV07P5MKV1ZA.md#PropertyMapper.PropMap.MapDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).TOutKey 'PropertyMapper\.PropMap\.MapDictionary\<TInKey,TInValue,TOutKey,TOutValue\>\(System\.Collections\.Generic\.Dictionary\<TInKey,TInValue\>\)\.TOutKey')[,](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')[TOutValue](PropMap.MapDictionary.RHXKN9BDH2AYXGV07P5MKV1ZA.md#PropertyMapper.PropMap.MapDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).TOutValue 'PropertyMapper\.PropMap\.MapDictionary\<TInKey,TInValue,TOutKey,TOutValue\>\(System\.Collections\.Generic\.Dictionary\<TInKey,TInValue\>\)\.TOutValue')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2')  
A new [System\.Collections\.Generic\.Dictionary&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.dictionary-2 'System\.Collections\.Generic\.Dictionary\`2') with mapped keys and values,
pre\-sized to `.Count`\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
When [source](PropMap.MapDictionary.RHXKN9BDH2AYXGV07P5MKV1ZA.md#PropertyMapper.PropMap.MapDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).source 'PropertyMapper\.PropMap\.MapDictionary\<TInKey,TInValue,TOutKey,TOutValue\>\(System\.Collections\.Generic\.Dictionary\<TInKey,TInValue\>\)\.source') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

[System\.InvalidOperationException](https://learn.microsoft.com/en-us/dotnet/api/system.invalidoperationexception 'System\.InvalidOperationException')  
When [TInKey](PropMap.MapDictionary.RHXKN9BDH2AYXGV07P5MKV1ZA.md#PropertyMapper.PropMap.MapDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).TInKey 'PropertyMapper\.PropMap\.MapDictionary\<TInKey,TInValue,TOutKey,TOutValue\>\(System\.Collections\.Generic\.Dictionary\<TInKey,TInValue\>\)\.TInKey') and [TOutKey](PropMap.MapDictionary.RHXKN9BDH2AYXGV07P5MKV1ZA.md#PropertyMapper.PropMap.MapDictionary_TInKey,TInValue,TOutKey,TOutValue_(System.Collections.Generic.Dictionary_TInKey,TInValue_).TOutKey 'PropertyMapper\.PropMap\.MapDictionary\<TInKey,TInValue,TOutKey,TOutValue\>\(System\.Collections\.Generic\.Dictionary\<TInKey,TInValue\>\)\.TOutKey') are unrelated types
that cannot be cast to each other\. Detected on the first use of the key type pair via
[PropertyMapper\.PropMap\.KeyCast&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/propertymapper.propmap.keycast-2 'PropertyMapper\.PropMap\.KeyCast\`2')\.