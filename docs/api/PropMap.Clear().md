## PropMap\.Clear\(\) Method

Removes all cached mapping plans, compiled mapping delegates, and projection expression trees\.
The next call to any mapping method will recompile from scratch\.

```csharp
public void Clear();
```

### Remarks
This method acquires the compile lock, so it is safe to call concurrently with mapping operations\.
Any [PropertyMapper\.PropMap\.Configure&lt;&gt;](https://learn.microsoft.com/en-us/dotnet/api/propertymapper.propmap.configure--2 'PropertyMapper\.PropMap\.Configure\`\`2') registrations are preserved; only the compiled artefacts
are discarded\.