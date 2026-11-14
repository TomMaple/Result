# ErrorUri.Urn(String) Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorUri.cs#L148" target="_blank">ErrorUri.cs</a>

Creates a new instance of the [ErrorUri](ErrorUri.md) record representing a *URN* (*RFC 8141*).

```csharp
public static ErrorUri Urn(string urn);
```

### Parameters
#### `urn` [String](https://learn.microsoft.com/dotnet/api/system.string)
The *URN*, e.g., `urn:example:errors:out-of-credit`; non-ASCII characters must be percent-encoded.

### Returns
[ErrorUri](ErrorUri.md)

A new instance of the [ErrorUri](ErrorUri.md) record representing a *URN* initialized with the provided value.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `urn` is `null`.

[UriFormatException](https://learn.microsoft.com/dotnet/api/system.uriformatexception)

The `urn` is not a valid *URN*.

## Remarks
A *URN* is a persistent, non-dereferenceable name, which suits the [TypeUri](../Error/Error_TypeUri.md) of an error, as well as the [InstanceUri](../Error/Error_InstanceUri.md) (e.g., `urn:uuid:…`).

The method validates the value against the *RFC 8141* syntax:
* the `urn` scheme (case-insensitive);
* the namespace identifier (NID): 2 to 32 letters, digits or hyphens, starting and ending with a letter or a digit;
* the namespace-specific string (NSS): at least one character; letters, digits, `-._~!$&'()*+,;=:@/` and percent-encoded octets (`%XX`);
* the optional `?+` (resolution), `?=` (query) and `#` (fragment) components.

The value is stored as passed; it is not normalized.

## Examples
```csharp
var error = Error.LimitExceeded(
    ErrorUri.Urn("urn:example:errors:out-of-credit"),
    "You do not have enough credit.",
    "Your current balance is 30, but that costs 50.",
    ErrorUri.Relative("/account/12345/msgs/abc"));
```

## See Also
* [ErrorUri](ErrorUri.md)
* [Error](../Error/Error.md)
* [RFC 8141: Uniform Resource Names (URNs)](https://datatracker.ietf.org/doc/html/rfc8141)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
