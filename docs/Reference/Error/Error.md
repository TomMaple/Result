# Error record
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Error.cs" target="_blank">Error.cs</a>

Represents an error in a [Result](../Result/Result.md) with optional properties for localization and multiple error details.

```csharp
public sealed record Error
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → Error

## Examples
```csharp
public Result DeleteUser(int userId)
{
    if (!UserExists(userId))
    {
        return Error.NotFound(
            ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:not-found"),
            "User has not been found.",
            "User with the specified ID does not exist in the system.",
            ErrorUri.Locator("https://api.exampleapp.com/error/e-1234-5678-9012-345678901234/details"),
            "errors-user-notfound",
            ("userId", userId));
    }

    // Proceed with user deletion

    return Result.Success();
}
```

## Remarks
It follows the [RFC 9457](https://datatracker.ietf.org/doc/html/rfc9457) standard for error responses.

### Equality
[Error](Error.md) provides value-based equality: two instances are equal when all of their properties are equal, including the [ErrorDetails](Error_ErrorDetails.md) collection, which is compared **by content** (order-sensitive) rather than by reference. See [Equals(Error)](Error_Equals.md) and [GetHashCode()](Error_GetHashCode.md).

### Copying with `with`
Copying an [Error](Error.md) through a `with` expression produces an **independent** copy: the [ErrorDetails](Error_ErrorDetails.md) collection is deep-copied, so adding a detail (via [AddDetail(String, String, String, (String, Object)[])](Error_AddDetail.md)) to the copy does not affect the original, and vice versa.

## Constructors
| Name | Description |
| ---- | ----------- |
| [Error()](Error_constructors.md) | Initializes a new instance of the [Error](Error.md) record. |

## Properties
| Name | Description |
| ---- | ----------- |
| [Category](Error_Category.md) | Gets or sets the category of the error. |
| [Detail](Error_Detail.md) | Gets or sets a human-readable explanation of the specific occurrence of the problem. |
| [DetailTemplated](Error_DetailTemplated.md) | Gets or sets a templated explanation of the specific occurrence of the problem. |
| [ErrorDetails](Error_ErrorDetails.md) | Gets or sets a collection of error details. |
| [InstanceUri](Error_InstanceUri.md) | Gets or sets a *URI* that identifies the specific occurrence of the problem. |
| [Title](Error_Title.md) | Gets or sets a short, human-readable summary of the problem type. |
| [TypeUri](Error_TypeUri.md) | Gets or sets a *URI* that identifies the type of the problem. |

## Methods
| Name | Description |
| ---- | ----------- |
| [AddDetail(String, String, String, (String, Object)[])](Error_AddDetail.md) | Adds a new set of error details to the current error and returns the current instance. |
| [Equals(Error)](Error_Equals.md) | Determines whether the specified error is equal to the current one, comparing the error details by content. |
| [GetHashCode()](Error_GetHashCode.md) | Returns a hash code consistent with [Equals(Error)](Error_Equals.md), incorporating the error details content. |
| [Conflict(ErrorUri, String, String, ErrorUri, String, (String, Object)[])](Error_Conflict.md#conflicterroruri-string-string-erroruri-string-string-object) | Creates a new conflict error with specified parameters. |
| [Conflict(ErrorUri, String, String, ErrorUri, String, IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)](Error_Conflict.md#conflicterroruri-string-string-erroruri-string-ienumerablekeyvaluepairstring-object) | Creates a new conflict error with specified parameters. |
| [Critical(ErrorUri, String, String, ErrorUri, String, (String, Object)[])](Error_Critical.md#criticalerroruri-string-string-erroruri-string-string-object) | Creates a new critical error with specified parameters. |
| [Critical(ErrorUri, String, String, ErrorUri, String, IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)](Error_Critical.md#criticalerroruri-string-string-erroruri-string-ienumerablekeyvaluepairstring-object) | Creates a new critical error with specified parameters. |
| [Failure(ErrorUri, String, String, ErrorUri, String, (String, Object)[])](Error_Failure.md#failureerroruri-string-string-erroruri-string-string-object) | Creates a new failure error with specified parameters. |
| [Failure(ErrorUri, String, String, ErrorUri, String, IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)](Error_Failure.md#failureerroruri-string-string-erroruri-string-ienumerablekeyvaluepairstring-object) | Creates a new failure error with specified parameters. |
| [NotFound(ErrorUri, String, String, ErrorUri, String, (String, Object)[])](Error_NotFound.md#notfounderroruri-string-string-erroruri-string-string-object) | Creates a new not found error with specified parameters. |
| [NotFound(ErrorUri, String, String, ErrorUri, String, IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)](Error_NotFound.md#notfounderroruri-string-string-erroruri-string-ienumerablekeyvaluepairstring-object) | Creates a new not found error with specified parameters. |
| [NotImplemented(ErrorUri, String, String, ErrorUri, String, (String, Object)[])](Error_NotImplemented.md#notimplementederroruri-string-string-erroruri-string-string-object) | Creates a new not implemented error with specified parameters. |
| [NotImplemented(ErrorUri, String, String, ErrorUri, String, IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)](Error_NotImplemented.md#notimplementederroruri-string-string-erroruri-string-ienumerablekeyvaluepairstring-object) | Creates a new not implemented error with specified parameters. |
| [Timeout(ErrorUri, String, String, ErrorUri, String, (String, Object)[])](Error_Timeout.md#timeouterroruri-string-string-erroruri-string-string-object) | Creates a new timeout error with specified parameters. |
| [Timeout(ErrorUri, String, String, ErrorUri, String, IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)](Error_Timeout.md#timeouterroruri-string-string-erroruri-string-ienumerablekeyvaluepairstring-object) | Creates a new timeout error with specified parameters. |
| [Unauthenticated(ErrorUri, String, String, ErrorUri, String, (String, Object)[])](Error_Unauthenticated.md#unauthenticatederroruri-string-string-erroruri-string-string-object) | Creates a new unauthenticated error with specified parameters. |
| [Unauthenticated(ErrorUri, String, String, ErrorUri, String, IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)](Error_Unauthenticated.md#unauthenticatederroruri-string-string-erroruri-string-ienumerablekeyvaluepairstring-object) | Creates a new unauthenticated error with specified parameters. |
| [Unauthorized(ErrorUri, String, String, ErrorUri, String, (String, Object)[])](Error_Unauthorized.md#unauthorizederroruri-string-string-erroruri-string-string-object) | Creates a new unauthorized error with specified parameters. |
| [Unauthorized(ErrorUri, String, String, ErrorUri, String, IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)](Error_Unauthorized.md#unauthorizederroruri-string-string-erroruri-string-ienumerablekeyvaluepairstring-object) | Creates a new unauthorized error with specified parameters. |
| [Unavailable(ErrorUri, String, String, ErrorUri, String, (String, Object)[])](Error_Unavailable.md#unavailableerroruri-string-string-erroruri-string-string-object) | Creates a new unavailable error with specified parameters. |
| [Unavailable(ErrorUri, String, String, ErrorUri, String, IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)](Error_Unavailable.md#unavailableerroruri-string-string-erroruri-string-ienumerablekeyvaluepairstring-object) | Creates a new unavailable error with specified parameters. |
| [Validation(ErrorUri, String, String, ErrorUri, String, (String, Object)[])](Error_Validation.md#validationerroruri-string-string-erroruri-string-string-object) | Creates a new validation error with specified parameters. |
| [Validation(ErrorUri, String, String, ErrorUri, String, IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)](Error_Validation.md#validationerroruri-string-string-erroruri-string-ienumerablekeyvaluepairstring-object) | Creates a new validation error with specified parameters. |

## See Also
* [ErrorCategory](../ErrorCategory.md)
* [ErrorDetail](../ErrorDetail/ErrorDetail.md)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Problem Details for HTTP APIs - RFC 7807 is dead, long live RFC 9457](https://blog.frankel.ch/problem-details-http-apis/)
