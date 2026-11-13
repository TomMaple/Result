# ErrorDetail.Detail Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorDetail.cs#L38" target="_blank">ErrorDetail.cs</a>

Gets a human-readable explanation of the specific problem detail.

```csharp
public string Detail { get; }
```

### Property Value
[string](https://learn.microsoft.com/dotnet/api/system.string)

A description of the specific problem detail of the [Error](../Error/Error.md).

## Remarks
This is a mandatory property that contains a human-readable description of the specific problem detail. It is never `null`.

> [!NOTE]
> [ErrorDetail](ErrorDetail.md) is immutable, so this property is set by the [constructor](ErrorDetail_constructors.md). It is usually created through [Error.WithErrorDetail](../Error/Error_WithErrorDetail.md) or [ErrorBuilder.AddErrorDetail](../ErrorBuilder/ErrorBuilder_AddErrorDetail.md).

## See Also
* [Error](../Error/Error.md)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Tag URI scheme](https://en.wikipedia.org/wiki/Tag_URI_scheme)
