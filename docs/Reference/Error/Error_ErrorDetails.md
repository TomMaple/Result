# Error.ErrorDetails Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Error.cs#L158" target="_blank">Error.cs</a>

Gets or sets a collection of error details.

```csharp
public IReadOnlyList<ErrorDetail> ErrorDetails { get; init; }
```

### Property Value
[ErrorDetail](../ErrorDetail/ErrorDetail.md)

A collection of error details that provide more context about the problem.

## Remarks
This is an optional property that contains a collection of error details—more granular description of multiple issues that may have contributed to the overall error condition.

This is a collection of individual error occurrences found, or an empty collection if no such details are available.

Each item is a [ErrorDetail](../ErrorDetail/ErrorDetail.md) object that describes a specific aspect of the overall error condition. It contains properties that provide a human-readable message, and optionally a templated message (so than the client can localize it) and a property path indicating the specific part of the request that caused the error.

> [!NOTE]
> This property should be used for read-only purposes.
> 
> It should not be assigned directly (i.e., via the property setter) but rather through the [AddDetail(String, String, String, (String, Object)[])](Error_AddDetail.md) method of the [Error](Error.md) class, which uses [TemplatedMessage](../TemplatedMessage/TemplatedMessage.md) to validate and provide the value. For deserialization, the setter is marked as `init` to allow assignment during object initialization.

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
* [ErrorDetail](../ErrorDetail/ErrorDetail.md)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Tag URI scheme](https://en.wikipedia.org/wiki/Tag_URI_scheme)