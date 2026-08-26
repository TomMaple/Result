# ErrorDetail.PropertyPointer Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/ErrorDetail.cs#L22" target="_blank">ErrorDetail.cs</a>

Gets or sets a *JSON Pointer* that identifies the input value field that the [ErrorDetail](ErrorDetail.md) record describes.

```csharp
public string? PropertyPointer { get; init; }
```

### Property Value
[string](https://learn.microsoft.com/dotnet/api/system.string)

A *JSON Pointer* that identifies the field of the input value.

## Remarks
This is an optional property that contains a text value of a *JSON Pointer* that identifies the input value field that the [ErrorDetail](ErrorDetail.md) record describes.

> [!NOTE]
> The value is not validated to be a proper *JSON Pointer*.
> 
> More information about *JSON Pointer* can be found in [RFC 6901: JSON Pointer](https://datatracker.ietf.org/doc/html/rfc6901).

## Examples
```csharp
var error = Error.Validation(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:create:invalid"),
                "Invalid data to register a new user.");

error.AddDetail(
    "#/username",
    "The username is already taken.",
    "error:user:create:username-taken",
    ("username", "john_doe"));

error.AddDetail(
    "#/email",
    "The email address is required.",
    "error:user:create:email-required",
    ("email", ""));
```

## See Also
* [Error](../Error/Error.md)
* [RFC 6901: JavaScript Object Notation (JSON) Pointer](https://datatracker.ietf.org/doc/html/rfc6901)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Problem Details for HTTP APIs - RFC 7807 is dead, long live RFC 9457](https://blog.frankel.ch/problem-details-http-apis/)
