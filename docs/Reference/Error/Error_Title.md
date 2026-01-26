# Error.Title Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Error.cs#L105" target="_blank">Error.cs</a>

Gets or sets a short, human-readable summary of the problem type.

```csharp
public string Title { get; init; }
```

### Property Value
[string](https://learn.microsoft.com/dotnet/api/system.string)

A short, human-readable summary of the problem type.

## Remarks
This is a mandatory property.

> [!NOTE]
> It **SHOULD NOT** change from occurrence to occurrence of the problem, except for localization.

The value is advisory and is included only for users who are unaware of and cannot discover the semantics of the type *URI* (e.g., during offline log analysis).

> [!CAUTION]
> It should not be assigned directly (i.e., via the property setter) but rather through the factory methods of the [Error](Error.md) record, which use [ErrorUri](../ErrorUri/ErrorUri.md) to validate and provide the value. For deserialization, the setter is marked as `init` to allow assignment during object initialization.

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
