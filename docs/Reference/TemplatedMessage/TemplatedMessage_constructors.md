# TemplatedMessage Constructor
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/TemplatedMessage.cs#L35" target="_blank">TemplatedMessage.cs</a>
Initializes a new instance of the [TemplatedMessage](TemplatedMessage.md) record.

```csharp
public sealed record TemplatedMessage(string TemplateId, IReadOnlyDictionary<string, object?>? Params = null);
```

### Parameters
#### `TemplateId` [String](https://learn.microsoft.com/dotnet/api/system.string)
A mandatory identifier of the message template.

#### `Params` [IReadOnlyDictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlydictionary-2)&lt;[String](https://learn.microsoft.com/dotnet/api/system.string), [Object](https://learn.microsoft.com/dotnet/api/system.object)&gt;
An optional dictionary of named values; values can be `null`. It is copied, so later changes to it are not reflected in the instance.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `TemplateId` is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `TemplateId` is empty or consists only of white-space characters.

## Remarks
> [!NOTE]
> An instance is usually created by the factory methods of the [Error](../Error/Error.md) record (the `detailTemplateId` and `detailNamedValues` parameters, e.g., [NotFound](../Error/Error_NotFound.md)), by [Error.WithErrorDetail](../Error/Error_WithErrorDetail.md), or by the [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md) methods [WithDetailTemplated](../ErrorBuilder/ErrorBuilder_WithDetailTemplated.md) and [AddErrorDetail](../ErrorBuilder/ErrorBuilder_AddErrorDetail.md).

## See also
* [Error](../Error/Error.md)
* [ErrorDetail](../ErrorDetail/ErrorDetail.md)
