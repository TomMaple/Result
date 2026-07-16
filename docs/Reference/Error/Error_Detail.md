# Error.Detail Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Error.cs#L157" target="_blank">Error.cs</a>

Gets or sets a human-readable explanation specific to this occurrence of the problem.

```csharp
public string? Detail { get; init; }
```

### Property Value
[string](https://learn.microsoft.com/dotnet/api/system.string)

A human-readable explanation specific to this occurrence of the problem.

## Remarks
This is an optional property.

If present, it should focus on helping the client correct the problem, rather than giving debugging information.

> [!WARNING]
> Consumers SHOULD NOT parse the property value for information; the [DetailTemplated](Error_DetailTemplated.md) property is more suitable and less error-prone to obtain such information.

## Examples
```csharp
var error = Error.NotFound(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:not-found"),
                "The user has not been found.",
                // the value for the Detail property
                "The user with ID ‘12345’ was not found or has been deleted.");
```

## See Also
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Problem Details for HTTP APIs - RFC 7807 is dead, long live RFC 9457](https://blog.frankel.ch/problem-details-http-apis/)
