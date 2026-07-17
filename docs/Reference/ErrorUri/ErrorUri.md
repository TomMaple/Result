# ErrorUri record
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/ErrorUri.cs" target="_blank">ErrorUri.cs</a>

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

error.AddDetail("#/email", "Email is invalid", "errors-email-invalid",
    ("required_format", "address@domain.com"), ("min_length", "9"));
```

```csharp
// Use ErrorUri.None() when the problem type is not specified (it resolves to about:blank).
var error = Error.Failure(ErrorUri.None(), "The operation could not be completed.");
```

## Constructors
| Name | Description |
| ---- | ----------- |
| [ErrorUri(String)](ErrorUri_constructors.md) | Initializes a new instance of the [ErrorUri](ErrorUri.md) record. |

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
| [Tag(String)](ErrorUri_Tag.md) | Creates a new instance of the [ErrorUri](ErrorUri.md) record with a *URI Tag* value. |

## Remarks
> [!NOTE]
> A default [ErrorUri](ErrorUri.md) instance is equivalent to the one returned by [None()](ErrorUri_None.md): its [Value](ErrorUri_Value.md) is `about:blank` and it compares equal to `ErrorUri.None()`.

## See Also
* [Error](../Error/Error.md)
* [tag URI scheme](https://en.wikipedia.org/wiki/Tag_URI_scheme)
* [RFC 1738: Uniform Resource Locators (URL)](https://datatracker.ietf.org/doc/html/rfc1738)
* [RFC 3986: Uniform Resource Identifier (URI): Generic Syntax](https://datatracker.ietf.org/doc/html/rfc3986)
* [RFC 4151: The 'tag' URI Scheme](https://datatracker.ietf.org/doc/html/rfc4151)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Problem Details for HTTP APIs - RFC 7807 is dead, long live RFC 9457](https://blog.frankel.ch/problem-details-http-apis/)
