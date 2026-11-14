# ErrorUri.Tag(String) Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorUri.cs#L132" target="_blank">ErrorUri.cs</a>

Creates a new instance of the [ErrorUri](ErrorUri.md) record representing a *URI Tag*.

```csharp
public static ErrorUri Tag(string uriTag);
```

### Parameters
#### `uriTag` [String](https://learn.microsoft.com/dotnet/api/system.string)
The *URI Tag* (*RFC 4151*), e.g., `tag:example.com,2026:errors:out-of-credit`; non-ASCII characters must be percent-encoded, and an internationalized domain name must use its punycode (`xn--`) form.

### Returns
[ErrorUri](ErrorUri.md)

A new instance of the [ErrorUri](ErrorUri.md) record representing a *URI Tag* initialized with the provided value.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `uriTag` is `null`.

[UriFormatException](https://learn.microsoft.com/dotnet/api/system.uriformatexception)

The `uriTag` is not a valid *URI Tag*.

## Remarks
The method validates the value against the *RFC 4151* syntax, `tag:` *authority* `,` *date* `:` *specific* [`#` *fragment*]:
* the authority is a domain name (in its punycode form if internationalized) or an email address;
* the date is `YYYY`, `YYYY-MM` or `YYYY-MM-DD`, with a year starting with `1` or `2`, a month `01`–`12` and a day `01`–`31`;
* the specific part and the fragment consist of letters, digits, `-._~!$&'()*+,;=:@/?` and percent-encoded octets (`%XX`).

The value is stored as passed, without normalization, so white space (including a trailing new line), invalid percent-encoding (e.g., `%zz`) and literal non-ASCII characters are rejected.

## Examples
```csharp
var errorUri = ErrorUri.Tag("tag:exampleapp.com,2026:errors:account:notfound");
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
* [RFC 1738: Uniform Resource Locators (URL)](https://datatracker.ietf.org/doc/html/rfc1738)
* [RFC 3986: Uniform Resource Identifier (URI): Generic Syntax](https://datatracker.ietf.org/doc/html/rfc3986)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Problem Details for HTTP APIs - RFC 7807 is dead, long live RFC 9457](https://blog.frankel.ch/problem-details-http-apis/)
