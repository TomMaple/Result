# ErrorDetail.Detail Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/ErrorDetail.cs#L22" target="_blank">ErrorDetail.cs</a>

Gets or sets a human-readable explanation of the specific problem detail.

```csharp
public string Detail { get; init; }
```

### Property Value
[string](https://learn.microsoft.com/dotnet/api/system.string)

A description of the specific problem detail of the [Error](../Error/Error.md).

## Remarks
This is a mandatory property that contains a human-readable description of the specific problem detail.

> [!NOTE]
> It should not be assigned directly (i.e., via the property setter) but rather through the [AddDetail](../Error/Error_AddDetail.md) methods of the [Error](../Error/Error.md) record, which builds the instance of the [ErrorDetail](ErrorDetail.md) record. For deserialization, the setter is marked as `init` to allow assignment during object initialization.

## See Also
* [Error](../Error/Error.md)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Tag URI scheme](https://en.wikipedia.org/wiki/Tag_URI_scheme)