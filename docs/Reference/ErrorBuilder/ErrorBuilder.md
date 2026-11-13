# ErrorBuilder class
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorBuilder.cs" target="_blank">ErrorBuilder.cs</a>

A mutable builder of an immutable [Error](../Error/Error.md).

```csharp
public sealed class ErrorBuilder
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → ErrorBuilder

## Examples
```csharp
public Result<User> CreateUser(NewUser newUser)
{
    var builder = ErrorBuilder.Validation(
        ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:create:invalid"),
        "Invalid data to register a new user.");

    if (string.IsNullOrWhiteSpace(newUser.Email))
        builder.AddErrorDetail("#/email", "The email address is required.", "error:user:create:email-required");

    if (newUser.UserName.Length < 3)
        builder.AddErrorDetail("#/username", "The username is too short.", "error:user:create:username-length", ("min", 3));

    var error = builder.Build();
    if (error.ErrorDetails.Count > 0)
        return error;

    // Proceed with user creation

    return SaveUser(newUser);
}
```

```csharp
// A changed copy of an existing error
var localized = error.ToBuilder()
    .WithTitle("Données invalides pour créer un nouvel utilisateur.")
    .WithoutMetadata()
    .Build();
```

## Remarks
[Error](../Error/Error.md) is immutable. Use an [ErrorBuilder](ErrorBuilder.md) to assemble an error step by step—for example, adding one [ErrorDetail](../ErrorDetail/ErrorDetail.md) per invalid field in a loop—and call [Build()](ErrorBuilder_Build.md) to get the [Error](../Error/Error.md).

Create a builder with:
* the [constructor](ErrorBuilder_constructors.md),
* one of the static methods named after the categories (e.g., [Validation](ErrorBuilder_Validation.md)), or
* [Error.ToBuilder()](../Error/Error_ToBuilder.md), to change an existing error.

### Validation
Every argument is validated when it is passed, so [Build()](ErrorBuilder_Build.md) never throws:
* the category must be a defined [ErrorCategory](../ErrorCategory.md) value,
* the title must not be `null`, empty or white-space only,
* metadata keys must not be `null`, blank or [reserved](../Error/Error_Metadata.md#keys); metadata values can be `null`.

A method that throws leaves the builder unchanged.

### Chaining and snapshots
Every method returns the same builder instance, so calls can be chained. [Build()](ErrorBuilder_Build.md) takes a snapshot: later changes to the builder do not affect an [Error](../Error/Error.md) already built, and the builder can still be used to build more errors.

A builder created by [Error.ToBuilder()](../Error/Error_ToBuilder.md) shares the read-only [ErrorDetails](../Error/Error_ErrorDetails.md) and [Metadata](../Error/Error_Metadata.md) collections of the source error until the first change to each of them.

### Thread safety
The builder is not thread-safe. Use one builder per thread, or synchronize access to it.

## Constructors
| Name | Description |
| ---- | ----------- |
| [ErrorBuilder(ErrorCategory, ErrorUri, String)](ErrorBuilder_constructors.md) | Initializes a new instance of the [ErrorBuilder](ErrorBuilder.md) class with the mandatory members of an error. |

## Methods
| Name | Description |
| ---- | ----------- |
| [AddErrorDetail(ErrorDetail)](ErrorBuilder_AddErrorDetail.md#adderrordetailerrordetail) | Appends the specified error detail. |
| [AddErrorDetail(String, String, String, (String, Object)[])](ErrorBuilder_AddErrorDetail.md#adderrordetailstring-string-string-string-object) | Appends a new error detail. |
| [Build()](ErrorBuilder_Build.md) | Returns a new [Error](../Error/Error.md) with the current members of the builder. |
| [Conflict(ErrorUri, String)](ErrorBuilder_Conflict.md) | Returns a new builder of the *Conflict* category. |
| [Critical(ErrorUri, String)](ErrorBuilder_Critical.md) | Returns a new builder of the *Critical* category. |
| [Failure(ErrorUri, String)](ErrorBuilder_Failure.md) | Returns a new builder of the *Failure* category. |
| [LimitExceeded(ErrorUri, String)](ErrorBuilder_LimitExceeded.md) | Returns a new builder of the *LimitExceeded* category. |
| [NotFound(ErrorUri, String)](ErrorBuilder_NotFound.md) | Returns a new builder of the *NotFound* category. |
| [NotImplemented(ErrorUri, String)](ErrorBuilder_NotImplemented.md) | Returns a new builder of the *NotImplemented* category. |
| [Timeout(ErrorUri, String)](ErrorBuilder_Timeout.md) | Returns a new builder of the *Timeout* category. |
| [Unauthenticated(ErrorUri, String)](ErrorBuilder_Unauthenticated.md) | Returns a new builder of the *Unauthenticated* category. |
| [Unauthorized(ErrorUri, String)](ErrorBuilder_Unauthorized.md) | Returns a new builder of the *Unauthorized* category. |
| [Unavailable(ErrorUri, String)](ErrorBuilder_Unavailable.md) | Returns a new builder of the *Unavailable* category. |
| [Validation(ErrorUri, String)](ErrorBuilder_Validation.md) | Returns a new builder of the *Validation* category. |
| [WithCategory(ErrorCategory)](ErrorBuilder_WithCategory.md) | Sets the category. |
| [WithDetail(String)](ErrorBuilder_WithDetail.md) | Sets the human-readable explanation of the problem occurrence. |
| [WithDetailTemplated(TemplatedMessage)](ErrorBuilder_WithDetailTemplated.md#withdetailtemplatedtemplatedmessage) | Sets the templated explanation of the problem occurrence. |
| [WithDetailTemplated(String, (String, Object)[])](ErrorBuilder_WithDetailTemplated.md#withdetailtemplatedstring-string-object) | Sets the templated explanation from a template identifier and named values. |
| [WithInstanceUri(ErrorUri)](ErrorBuilder_WithInstanceUri.md) | Sets the *URI* that identifies the problem occurrence. |
| [WithMetadata(String, Object)](ErrorBuilder_WithMetadata.md#withmetadatastring-object) | Adds or replaces a metadata entry. |
| [WithMetadata(IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)](ErrorBuilder_WithMetadata.md#withmetadataienumerablekeyvaluepairstring-object) | Adds or replaces metadata entries. |
| [WithoutMetadata()](ErrorBuilder_WithoutMetadata.md#withoutmetadata) | Removes all metadata entries. |
| [WithoutMetadata(String)](ErrorBuilder_WithoutMetadata.md#withoutmetadatastring) | Removes the metadata entry of the specified key. |
| [WithTitle(String)](ErrorBuilder_WithTitle.md) | Sets the short, human-readable summary of the problem type. |
| [WithTypeUri(ErrorUri)](ErrorBuilder_WithTypeUri.md) | Sets the *URI* that identifies the problem type. |

## See Also
* [Error](../Error/Error.md)
* [Error.ToBuilder()](../Error/Error_ToBuilder.md)
* [ErrorCategory](../ErrorCategory.md)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
