# ErrorUri.Value Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/ErrorUri.cs#L67" target="_blank">ErrorUri.cs</a>

Gets a *URI* value that can be a *URI Locator*, *URI Tag*, or the `about:blank` value.

```csharp
public string Value { get; init; }
```

### Property Value
[string](https://learn.microsoft.com/dotnet/api/system.string)

A *URI* value that can be a *URI Locator*, *URI Tag*, or the `about:blank` value.

## Remarks
This is a mandatory property.

> [!CAUTION]
> This value should not be assigned via constructor but through the [Locator](ErrorUri_Locator.md), [Tag(String)](ErrorUri_Tag.md) or [None()](ErrorUri_None.md) method, which validates the value.

## See Also
* [Error](../Error/Error.md)
* [tag URI scheme](https://en.wikipedia.org/wiki/Tag_URI_scheme)
* [RFC 1738: Uniform Resource Locators (URL)](https://datatracker.ietf.org/doc/html/rfc1738)
* [RFC 3986: Uniform Resource Identifier (URI): Generic Syntax](https://datatracker.ietf.org/doc/html/rfc3986)
* [RFC 4151: The 'tag' URI Scheme](https://datatracker.ietf.org/doc/html/rfc4151)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Problem Details for HTTP APIs - RFC 7807 is dead, long live RFC 9457](https://blog.frankel.ch/problem-details-http-apis/)
