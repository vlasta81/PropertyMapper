## PropMapConfiguration\.WithMaxDepth\(int\) Method

Returns a new configuration with [MaxMappingDepth](PropMapConfiguration.MaxMappingDepth.md 'PropertyMapper\.Configuration\.PropMapConfiguration\.MaxMappingDepth') set to [depth](PropMapConfiguration.WithMaxDepth.5BI3XF0D0GFDMLM5G2XKXRVF.md#PropertyMapper.Configuration.PropMapConfiguration.WithMaxDepth(int).depth 'PropertyMapper\.Configuration\.PropMapConfiguration\.WithMaxDepth\(int\)\.depth')\.

```csharp
public PropertyMapper.Configuration.PropMapConfiguration WithMaxDepth(int depth);
```
#### Parameters

<a name='PropertyMapper.Configuration.PropMapConfiguration.WithMaxDepth(int).depth'></a>

`depth` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Maximum nesting depth \(1–256\) for recursive object mapping\.

#### Returns
[PropMapConfiguration](PropMapConfiguration.md 'PropertyMapper\.Configuration\.PropMapConfiguration')  
A new [PropMapConfiguration](PropMapConfiguration.md 'PropertyMapper\.Configuration\.PropMapConfiguration') with the updated value\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown when [depth](PropMapConfiguration.WithMaxDepth.5BI3XF0D0GFDMLM5G2XKXRVF.md#PropertyMapper.Configuration.PropMapConfiguration.WithMaxDepth(int).depth 'PropertyMapper\.Configuration\.PropMapConfiguration\.WithMaxDepth\(int\)\.depth') is outside the range 1–256\.