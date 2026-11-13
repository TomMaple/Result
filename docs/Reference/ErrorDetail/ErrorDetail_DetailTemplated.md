# ErrorDetail.DetailTemplated Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorDetail.cs#L43" target="_blank">ErrorDetail.cs</a>

A templated version of a human-readable explanation of the specific problem detail.

```csharp
public TemplatedMessage? DetailTemplated { get; }
```

### Property Value
[TemplatedMessage](../TemplatedMessage/TemplatedMessage.md)

A record that contains details of a templated version of the problem detail.

## Remarks
This is an optional property that allows to generate a proper, localized human-readable explanation of the specific problem detail.

> [!NOTE]
> [ErrorDetail](ErrorDetail.md) is immutable, so this property is set by the [constructor](ErrorDetail_constructors.md). It is usually created through [Error.WithErrorDetail](../Error/Error_WithErrorDetail.md) or [ErrorBuilder.AddErrorDetail](../ErrorBuilder/ErrorBuilder_AddErrorDetail.md).

## See Also
* [Error](../Error/Error.md)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Tag URI scheme](https://en.wikipedia.org/wiki/Tag_URI_scheme)
