# Error.Title Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Error.cs#L119" target="_blank">Error.cs</a>

Gets a short, human-readable summary of the problem type.

```csharp
public string Title { get; }
```

### Property Value
[string](https://learn.microsoft.com/dotnet/api/system.string)

A short, human-readable summary of the problem type.

## Remarks
This is a mandatory property. It is never `null`, empty or white-space only: the factory methods and [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md) reject such a title.

> [!NOTE]
> It **SHOULD NOT** change from occurrence to occurrence of the problem, except for localization.

The value is advisory and is included only for users who are unaware of and cannot discover the semantics of the type *URI* (e.g., during offline log analysis).

> [!NOTE]
> [Error](Error.md) is immutable, so this property is set when the error is created: by the factory methods (e.g., [Validation](Error_Validation.md)) or an [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md). To change it, use [ToBuilder()](Error_ToBuilder.md).

## Examples
```csharp
var error = Error.NotFound(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:not-found"),
                // the value for the Title property
                "User has not been found.",
                "User with ID ‘12345’ was not found or has been deleted.");
```

## See Also
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Problem Details for HTTP APIs - RFC 7807 is dead, long live RFC 9457](https://blog.frankel.ch/problem-details-http-apis/)
