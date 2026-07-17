# ErrorDetail Constructor
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/ErrorDetail.cs#L22" target="_blank">ErrorDetail.cs</a>

Initializes a new instance of the [ErrorDetail](ErrorDetail.md) record.

```csharp
public sealed record ErrorDetail(string? PropertyPointer, string Detail, TemplatedMessage? DetailTemplated = null);
```

## Remarks
> [!NOTE]
> To create a new instance, the constructor or the [AddDetail](../Error/Error_AddDetail.md) method of the [Error](../Error/Error.md) record can be used.

## See also
* [Error](../Error/Error.md)
* [TemplatedMessage](../TemplatedMessage/TemplatedMessage.md)
* [RFC 6901: JavaScript Object Notation (JSON) Pointer](https://datatracker.ietf.org/doc/html/rfc6901)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Problem Details for HTTP APIs - RFC 7807 is dead, long live RFC 9457](https://blog.frankel.ch/problem-details-http-apis/)
