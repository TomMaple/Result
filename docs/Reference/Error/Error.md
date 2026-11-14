# Error record
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Error.cs" target="_blank">Error.cs</a>

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
[Error](Error.md) provides value-based equality: two instances are equal when all of their properties are equal, including the [ErrorDetails](Error_ErrorDetails.md) collection, which is compared **by content** (order-sensitive) rather than by reference, and the [Metadata](Error_Metadata.md) dictionary, which is compared **by content** independent of the order of its entries. See [Equals(Error)](Error_Equals.md) and [GetHashCode()](Error_GetHashCode.md).

### Immutability
[Error](Error.md) is immutable: all its properties are get-only, and the [ErrorDetails](Error_ErrorDetails.md) and [Metadata](Error_Metadata.md) collections are read-only. It has no public constructor. Create it with:
* one of the factory methods named after the categories (e.g., [Validation](Error_Validation.md)), or
* an [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md), when the error is assembled step by step (e.g., one error detail per invalid field).

Methods such as [WithErrorDetail](Error_WithErrorDetail.md), [WithMetadata](Error_WithMetadata.md) and [WithoutMetadata](Error_WithoutMetadata.md) return a changed copy. To change any other member, or several at once, use [ToBuilder()](Error_ToBuilder.md) and [ErrorBuilder.Build()](../ErrorBuilder/ErrorBuilder_Build.md).

An [Error](Error.md) is not validated again after it is created: the title is never `null` or blank, and the category is always a defined [ErrorCategory](../ErrorCategory.md) value.

### Serialization
The default serializers (*System.Text.Json* and *Newtonsoft.Json*) can write an [Error](Error.md) as JSON. Reading it back requires a dedicated converter, because the type has no public constructor or setters.

### Metadata
[Metadata](Error_Metadata.md) holds machine-readable, response-level values (the *extension members* of RFC 9457). Use [WithMetadata(String, Object)](Error_WithMetadata.md) and [WithoutMetadata](Error_WithoutMetadata.md) to obtain a changed copy, and [TryGetMetadata&lt;T&gt;(String, T)](Error_TryGetMetadata.md) to read a value of a specific type.

### Text representation
`ToString()` writes all members, including the contents of [ErrorDetails](Error_ErrorDetails.md) and [Metadata](Error_Metadata.md), e.g., `Error { Category = NotFound, TypeUri = about:blank, Title = Missing, …, ErrorDetails = [], Metadata = {} }`.

## Properties
| Name | Description |
| ---- | ----------- |
| [Category](Error_Category.md) | Gets the category of the error. |
| [Detail](Error_Detail.md) | Gets a human-readable explanation of the specific occurrence of the problem. |
| [DetailTemplated](Error_DetailTemplated.md) | Gets a templated explanation of the specific occurrence of the problem. |
| [ErrorDetails](Error_ErrorDetails.md) | Gets a collection of error details. |
| [InstanceUri](Error_InstanceUri.md) | Gets a *URI* that identifies the specific occurrence of the problem. |
| [Metadata](Error_Metadata.md) | Gets the additional, machine-readable members of the problem. |
| [Title](Error_Title.md) | Gets a short, human-readable summary of the problem type. |
| [TypeUri](Error_TypeUri.md) | Gets a *URI* that identifies the type of the problem. |

## Methods
| Name | Description |
| ---- | ----------- |
| [Equals(Error)](Error_Equals.md) | Determines whether the specified error is equal to the current one, comparing the error details and the metadata by content. |
| [GetHashCode()](Error_GetHashCode.md) | Returns a hash code consistent with [Equals(Error)](Error_Equals.md), incorporating the error details and the metadata content. |
| [ToBuilder()](Error_ToBuilder.md) | Returns a new [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md) initialized with the members of the current error. |
| [TryGetMetadata&lt;T&gt;(String, T)](Error_TryGetMetadata.md) | Gets the metadata value of the specified key if it is of the requested type. |
| [WithErrorDetail(ErrorDetail)](Error_WithErrorDetail.md#witherrordetailerrordetail) | Returns a copy of the error with the specified error detail appended. |
| [WithErrorDetail(String, String, String, (String, Object)[])](Error_WithErrorDetail.md#witherrordetailstring-string-string-string-object) | Returns a copy of the error with a new error detail appended. |
| [WithMetadata(String, Object)](Error_WithMetadata.md#withmetadatastring-object) | Returns a copy of the error with the specified metadata entry added or replaced. |
| [WithMetadata(IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)](Error_WithMetadata.md#withmetadataienumerablekeyvaluepairstring-object) | Returns a copy of the error with the specified metadata entries added or replaced. |
| [WithMetadata(ReadOnlySpan&lt;(String, Object)&gt;)](Error_WithMetadata.md#withmetadatareadonlyspanstring-object) | Returns a copy of the error with the specified metadata entries, given as key-value tuples, added or replaced. |
| [WithoutMetadata()](Error_WithoutMetadata.md#withoutmetadata) | Returns a copy of the error without any metadata. |
| [WithoutMetadata(String)](Error_WithoutMetadata.md#withoutmetadatastring) | Returns a copy of the error without the metadata entry of the specified key. |
| [Conflict(ErrorUri, String, String, ErrorUri, String, (String, Object)[])](Error_Conflict.md#conflicterroruri-string-string-erroruri-string-string-object) | Creates a new conflict error with specified parameters. |
| [Conflict(ErrorUri, String, String, ErrorUri, String, IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)](Error_Conflict.md#conflicterroruri-string-string-erroruri-string-ienumerablekeyvaluepairstring-object) | Creates a new conflict error with specified parameters. |
| [Critical(ErrorUri, String, String, ErrorUri, String, (String, Object)[])](Error_Critical.md#criticalerroruri-string-string-erroruri-string-string-object) | Creates a new critical error with specified parameters. |
| [Critical(ErrorUri, String, String, ErrorUri, String, IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)](Error_Critical.md#criticalerroruri-string-string-erroruri-string-ienumerablekeyvaluepairstring-object) | Creates a new critical error with specified parameters. |
| [Failure(ErrorUri, String, String, ErrorUri, String, (String, Object)[])](Error_Failure.md#failureerroruri-string-string-erroruri-string-string-object) | Creates a new failure error with specified parameters. |
| [Failure(ErrorUri, String, String, ErrorUri, String, IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)](Error_Failure.md#failureerroruri-string-string-erroruri-string-ienumerablekeyvaluepairstring-object) | Creates a new failure error with specified parameters. |
| [LimitExceeded(ErrorUri, String, String, ErrorUri, String, (String, Object)[])](Error_LimitExceeded.md#limitexceedederroruri-string-string-erroruri-string-string-object) | Creates a new limit exceeded error with specified parameters. |
| [LimitExceeded(ErrorUri, String, String, ErrorUri, String, IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)](Error_LimitExceeded.md#limitexceedederroruri-string-string-erroruri-string-ienumerablekeyvaluepairstring-object) | Creates a new limit exceeded error with specified parameters. |
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
* [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md)
* [ErrorCategory](../ErrorCategory.md)
* [ErrorDetail](../ErrorDetail/ErrorDetail.md)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Problem Details for HTTP APIs - RFC 7807 is dead, long live RFC 9457](https://blog.frankel.ch/problem-details-http-apis/)
