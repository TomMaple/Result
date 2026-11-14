# ErrorUri record
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorUri.cs" target="_blank">ErrorUri.cs</a>

Provides a validated *URI* for the purpose of the *Error* record properties like [InstanceUri](../Error/Error_InstanceUri.md) and [TypeUri](../Error/Error_TypeUri.md).

```csharp
public readonly record struct ErrorUri
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → [ValueType](https://learn.microsoft.com/dotnet/api/system.valuetype) → ErrorUri


## Examples
```csharp
// A URI Tag identifies the problem type; a URI Locator points to its documentation.
var error = Error.Validation(
    ErrorUri.Tag("tag:exampleapp.com,2026:errors:signup:invalid-data"),
    "Provided data is invalid.",
    "Address the validation errors and try again.",
    ErrorUri.Locator("https://api.exampleapp.com/errors/7894375839/details"));
```

```csharp
// A URN names the problem type; a relative reference with the full path identifies the occurrence.
var error = Error.LimitExceeded(
    ErrorUri.Urn("urn:example:errors:out-of-credit"),
    "You do not have enough credit.",
    instanceUri: ErrorUri.Relative("/account/12345/msgs/abc"));
```

```csharp
// Use ErrorUri.None() when the problem type is not specified (it resolves to about:blank).
var error = Error.Failure(ErrorUri.None(), "The operation could not be completed.");
```

## Properties
| Name | Description |
| ---- | ----------- |
| [Value](ErrorUri_Value.md) | Gets the validated value of the <i>URI</i> used by the [Error](../Error/Error.md) record properties. |

## Methods
| Name | Description |
| ---- | ----------- |
| [Equals(ErrorUri)](ErrorUri_Equals.md) | Determines whether the specified [ErrorUri](ErrorUri.md) is equal to the current one, comparing the [Value](ErrorUri_Value.md) property. |
| [GetHashCode()](ErrorUri_GetHashCode.md) | Returns a hash code that is consistent with [Equals(ErrorUri)](ErrorUri_Equals.md). |
| [Locator(String)](ErrorUri_Locator.md) | Creates a new instance of the [ErrorUri](ErrorUri.md) record with a *URI Locator* value. |
| [None()](ErrorUri_None.md) |  Creates a new instance of the [ErrorUri](ErrorUri.md) record with the `about:blank` value. |
| [Relative(String)](ErrorUri_Relative.md) | Creates a new instance of the [ErrorUri](ErrorUri.md) record with a relative *URI* reference with the full path. |
| [Tag(String)](ErrorUri_Tag.md) | Creates a new instance of the [ErrorUri](ErrorUri.md) record with a *URI Tag* value. |
| [ToString()](ErrorUri_ToString.md) | Returns the *URI* [Value](ErrorUri_Value.md). |
| [Urn(String)](ErrorUri_Urn.md) | Creates a new instance of the [ErrorUri](ErrorUri.md) record with a *URN* value. |

## Remarks
[ErrorUri](ErrorUri.md) has no public constructor: create it with one of the factory methods, which validate the value. An [ErrorUri](ErrorUri.md) therefore always holds a valid *URI* reference.

| Factory method | Value | Typical use |
| -------------- | ----- | ----------- |
| [None()](ErrorUri_None.md) | `about:blank` | A type with no additional semantics beyond the HTTP status code; not allowed as an instance. |
| [Locator(String)](ErrorUri_Locator.md) | An absolute `http` or `https` *URI* | A type whose documentation can be dereferenced; an instance with details of the occurrence. |
| [Tag(String)](ErrorUri_Tag.md) | A `tag:` *URI* (*RFC 4151*) | A unique, non-dereferenceable type. |
| [Urn(String)](ErrorUri_Urn.md) | A `urn:` *URI* (*RFC 8141*) | A persistent, non-dereferenceable type or instance. |
| [Relative(String)](ErrorUri_Relative.md) | A relative reference with the full path (`/…`) | An instance resolved against the base *URI* of the response; *RFC 9457* recommends absolute *URI*s when possible. |

> [!NOTE]
> A default [ErrorUri](ErrorUri.md) instance is equivalent to the one returned by [None()](ErrorUri_None.md): its [Value](ErrorUri_Value.md) is `about:blank` and it compares equal to `ErrorUri.None()`.

> [!IMPORTANT]
> `about:blank` has a meaning only as a type (*RFC 9457*, section 4.2.1). Passing [None()](ErrorUri_None.md) or a default [ErrorUri](ErrorUri.md) as the instance of an error throws an [ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception); pass `null` when there is no instance.

## See Also
* [Error](../Error/Error.md)
* [tag URI scheme](https://en.wikipedia.org/wiki/Tag_URI_scheme)
* [RFC 1738: Uniform Resource Locators (URL)](https://datatracker.ietf.org/doc/html/rfc1738)
* [RFC 3986: Uniform Resource Identifier (URI): Generic Syntax](https://datatracker.ietf.org/doc/html/rfc3986)
* [RFC 4151: The 'tag' URI Scheme](https://datatracker.ietf.org/doc/html/rfc4151)
* [RFC 8141: Uniform Resource Names (URNs)](https://datatracker.ietf.org/doc/html/rfc8141)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Problem Details for HTTP APIs - RFC 7807 is dead, long live RFC 9457](https://blog.frankel.ch/problem-details-http-apis/)
