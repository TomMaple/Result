# Error.TypeUri Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Error.cs#L95" target="_blank">Error.cs</a>

Gets or sets a *URI* that identifies the type of the problem.

```csharp
public string TypeUri { get; init; }
```

### Property Value
[string](https://learn.microsoft.com/dotnet/api/system.string)

A text value of a URI that identifies the type of the problem.

## Remarks
This is a mandatory property that contains a text value of a URI that identifies the type of the problem.

> [!NOTE]
> It should not be assigned directly (i.e., via the property setter) but rather through the factory methods of the [Error](Error.md) class, which use [ErrorUri](../ErrorUri/ErrorUri.md) to validate and provide the value. For deserialization, the setter is marked as `init` to allow assignment during object initialization.

The value of this property should be:
* `about:blank`,
* a URI Locator, or
* a URI Tag.

If the value is a *URI Locator*, it is a *URL* (with an “http” or “https” scheme) that, when dereferenced, provides human-readable documentation about the problem type. The locator should use absolute URIs to avoid confusion and malfunctions. However, consumers **should not** automatically dereference the type *URI*, unless they do so when providing information to developers (e.g., when a debugging tool is in use).

If the value is a *URI Tag*, it is not dereferenceable and uniquely represents the different types of problems.

## Examples
```csharp
var error = Error.NotFound(
                // uses the ErrorUri class to provide value for the TypeUri property
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:not-found"),
                "User has not been found.");
```

## See Also
* [ErrorUri](../ErrorUri/ErrorUri.md)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Tag URI scheme](https://en.wikipedia.org/wiki/Tag_URI_scheme)