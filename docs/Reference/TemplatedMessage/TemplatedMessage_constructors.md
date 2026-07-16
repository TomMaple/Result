# TemplatedMessage Constructor
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/TemplatedMessage.cs#L27" target="_blank">TemplatedMessage.cs</a>
Initializes a new instance of the [TemplatedMessage](TemplatedMessage.md) record.

```csharp
public record TemplatedMessage(string TemplateId, IReadOnlyDictionary<string, object>? Params = null);
```

## Remarks
> [!NOTE]
> This constructor is required for deserialization.
> 
> Instead of constructor, rather use one of the static methods of an [Error](../Error/Error.md) record (such as [NotFound(ErrorUri, String, String, ErrorUri, String, (String, Object)[])](Error_NotFound.md#notfounderroruri-string-string-erroruri-string-string-object) or [Failure(ErrorUri, String, String, ErrorUri, String, IEnumerable<KeyValuePair<String, Object>>)](Error_Failure.md#failureerroruri-string-string-erroruri-string-ienumerablekeyvaluepairstring-object)) or an object instance method like [AddDetail(String, String, String, (String, Object)[])](Error_AddDetail.md) to create an instance of the [TemplatedMessage](TemplatedMessage.md) record with propriate values.

## See also
* [Error](../Error/Error.md)
* [ErrorDetail](../ErrorDetail/ErrorDetail.md)
