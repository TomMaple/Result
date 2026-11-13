# Error.ErrorDetails Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Error.cs#L177" target="_blank">Error.cs</a>

Gets a collection of error details.

```csharp
public IReadOnlyList<ErrorDetail> ErrorDetails { get; }
```

### Property Value
[IReadOnlyList](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlylist-1)&lt;[ErrorDetail](../ErrorDetail/ErrorDetail.md)&gt;

A collection of error details that provide more context about the problem. Never `null`; empty by default.

## Remarks
This is an optional property that contains a collection of error details—more granular description of multiple issues that may have contributed to the overall error condition.

This is a collection of individual error occurrences found, or an empty collection if no such details are available.

Each item is a [ErrorDetail](../ErrorDetail/ErrorDetail.md) object that describes a specific aspect of the overall error condition. It contains properties that provide a human-readable message, and optionally a templated message (so than the client can localize it) and a property path indicating the specific part of the request that caused the error.

The collection is read-only and cannot be modified, also through a cast (modifying methods of `IList<ErrorDetail>` throw a [NotSupportedException](https://learn.microsoft.com/dotnet/api/system.notsupportedexception)). To add details:
* use [WithErrorDetail](Error_WithErrorDetail.md), which returns a copy of the error with the detail appended, or
* use an [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md) and its [AddErrorDetail](../ErrorBuilder/ErrorBuilder_AddErrorDetail.md) method to add several details without copying the error for each one.

> [!NOTE]
> This collection participates in the value-based equality of [Error](Error.md): it is compared **by content** (order-sensitive), so two errors with identically-built details are considered equal. See [Equals(Error)](Error_Equals.md).

## Examples
```csharp
var error = ErrorBuilder.Validation(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:create:invalid"),
                "Invalid data to register a new user.")
            .AddErrorDetail(
                "#/username",
                "The username is already taken.",
                "error:user:create:username-taken",
                ("username", "john_doe"))
            .AddErrorDetail(
                "#/email",
                "The email address is required.",
                "error:user:create:email-required",
                ("email", ""))
            .Build();

foreach (var errorDetail in error.ErrorDetails)
{
    Console.WriteLine($"{errorDetail.PropertyPointer}: {errorDetail.Detail}");
}
```

## See Also
* [ErrorDetail](../ErrorDetail/ErrorDetail.md)
* [WithErrorDetail](Error_WithErrorDetail.md)
* [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Tag URI scheme](https://en.wikipedia.org/wiki/Tag_URI_scheme)
