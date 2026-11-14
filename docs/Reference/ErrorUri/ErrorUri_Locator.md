# ErrorUri.Locator(String) Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorUri.cs#L111" target="_blank">ErrorUri.cs</a>

Creates a new instance of the [ErrorUri](ErrorUri.md) record representing a *URI Locator*.

```csharp
public static ErrorUri Locator(string uriLocator);
```

### Parameters
#### `uriLocator` [String](https://learn.microsoft.com/dotnet/api/system.string)
The absolute `http` or `https` *URI*, e.g., `https://example.com/probs/out-of-credit`; non-ASCII characters must be percent-encoded, and an internationalized domain name must use its punycode (`xn--`) form.

### Returns
[ErrorUri](ErrorUri.md)

A new instance of the [ErrorUri](ErrorUri.md) record representing a *URI Locator* initialized with the provided value.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `uriLocator` is `null`.

[UriFormatException](https://learn.microsoft.com/dotnet/api/system.uriformatexception)

The `uriLocator` is not a valid *URI Locator*.

## Remarks
The method validates that the value is an absolute `http` or `https` *URI*. The value is stored as passed, without normalization, so it must already be a valid *URI* (*RFC 3986*):

| Value | Accepted |
| ----- | -------- |
| `https://example.com/probs/out-of-credit`, `https://example.com:8443/a?b=c#d`, `http://[::1]/a` | yes |
| `https://example.com/caf%C3%A9`, `https://xn--mnchen-3ya.de/a` (percent-encoded, punycode) | yes |
| `" https://example.com/a"`, `"https://example.com/a\n"` (leading or trailing white space) | no |
| `https://example.com/a b`, `https://example.com/a<b>` (characters that need escaping) | no |
| `https://example.com/a%zz`, `https://example.com/a%` (invalid percent-encoding) | no |
| `https://example.com/café`, `https://münchen.de/a` (literal non-ASCII characters) | no |
| `https://example.com/a#b#c` (a second `#`) | no |
| `mailto:…`, `tag:…`, `urn:…`, `/a/b` (another scheme or a relative reference) | no; use [Tag](ErrorUri_Tag.md), [Urn](ErrorUri_Urn.md) or [Relative](ErrorUri_Relative.md) |

## Examples
```csharp
var errorUri = ErrorUri.Locator("https://api.exampleapp.com/error/e-1234-5678-9012-345678901234/details");
```

```csharp
var error = Error.NotFound(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:account:notfound"),
                "Cannot update the account.",
                "The account with the provided ID has not been found.",
                ErrorUri.Locator("https://api.exampleapp.com/errors/8783927589734857/details"),
                "errors.account.notFound",
                ("accountId", "D12345"), ("action", "account:update"));
```

## See Also
* [Error](../Error/Error.md)
* [tag URI scheme](https://en.wikipedia.org/wiki/Tag_URI_scheme)
* [RFC 3986: Uniform Resource Identifier (URI): Generic Syntax](https://datatracker.ietf.org/doc/html/rfc3986)
* [RFC 4151: The 'tag' URI Scheme](https://datatracker.ietf.org/doc/html/rfc4151)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Problem Details for HTTP APIs - RFC 7807 is dead, long live RFC 9457](https://blog.frankel.ch/problem-details-http-apis/)
