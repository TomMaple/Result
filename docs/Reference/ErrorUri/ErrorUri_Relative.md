# ErrorUri.Relative(String) Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorUri.cs#L155" target="_blank">ErrorUri.cs</a>

Creates a new instance of the [ErrorUri](ErrorUri.md) record representing a relative *URI* reference with the full path (*RFC 3986*, section 4.2).

```csharp
public static ErrorUri Relative(string relativeReference);
```

### Parameters
#### `relativeReference` [String](https://learn.microsoft.com/dotnet/api/system.string)
The relative reference with the full path (e.g., `/account/12345/msgs/abc`), optionally followed by a query and a fragment; non-ASCII characters must be percent-encoded.

### Returns
[ErrorUri](ErrorUri.md)

A new instance of the [ErrorUri](ErrorUri.md) record representing the relative reference initialized with the provided value.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `relativeReference` is `null`.

[UriFormatException](https://learn.microsoft.com/dotnet/api/system.uriformatexception)

The `relativeReference` is not a valid relative reference with the full path.

## Remarks
A relative reference is resolved against the base *URI* of the response. *RFC 9457* (sections 3.1.1 and 3.1.5) allows it in both the [TypeUri](../Error/Error_TypeUri.md) and the [InstanceUri](../Error/Error_InstanceUri.md), but recommends absolute *URI*s when possible, because relative ones can cause confusion and might not be handled correctly by all implementations. When a relative reference is used, it should include the full path.

Therefore, the method accepts only a reference that starts with a single `/`:

| Value | Accepted |
| ----- | -------- |
| `/account/12345/msgs/abc`, `/types/123?lang=en#details` | yes |
| `msgs/abc` (a relative path) | no |
| `//example.com/a` (a network-path reference) | no |
| an empty string, `?a=b`, `#a` | no |
| `/a b`, `/a%zz`, `/café` (a space, invalid percent-encoding, a literal non-ASCII character) | no |

The value is stored as passed; it is not normalized.

## Examples
```csharp
// The example from RFC 9457, section 3.
var error = Error.LimitExceeded(
    ErrorUri.Locator("https://example.com/probs/out-of-credit"),
    "You do not have enough credit.",
    "Your current balance is 30, but that costs 50.",
    ErrorUri.Relative("/account/12345/msgs/abc"));
```

## See Also
* [ErrorUri](ErrorUri.md)
* [Error](../Error/Error.md)
* [RFC 3986: Uniform Resource Identifier (URI): Generic Syntax](https://datatracker.ietf.org/doc/html/rfc3986#section-4.2)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
