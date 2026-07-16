# ErrorUri Constructor
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/ErrorUri.cs#L61" target="_blank">ErrorUri.cs</a>

Initializes a new instance of the [ErrorUri](ErrorUri.md) record.

```csharp
public ErrorUri(string value);
```

## Remarks
> [!CAUTION]
> Do not use this constructor directly, as it does NOT validate the value, which may also cause unwanted side effects.
>
> This constructor is required for deserialization.
> 
> Instead, use one of the static methods such as [Locator(String)](ErrorUri_Locator.md), [Tag(String)](ErrorUri_Tag.md) or [None()](ErrorUri_None.md) to create an instance of the [ErrorUri](ErrorUri.md) record with an appropriate value.

> [!NOTE]
> Passing a `null` value does not throw. The resulting instance reports `about:blank` from its [Value](ErrorUri_Value.md) property, exactly as a default instance does.

## See also
* [Error](../Error/Error.md)
* [tag URI scheme](https://en.wikipedia.org/wiki/Tag_URI_scheme)
* [RFC 1738: Uniform Resource Locators (URL)](https://datatracker.ietf.org/doc/html/rfc1738)
* [RFC 3986: Uniform Resource Identifier (URI): Generic Syntax](https://datatracker.ietf.org/doc/html/rfc3986)
* [RFC 4151: The 'tag' URI Scheme](https://datatracker.ietf.org/doc/html/rfc4151)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Problem Details for HTTP APIs - RFC 7807 is dead, long live RFC 9457](https://blog.frankel.ch/problem-details-http-apis/)
