# ErrorBuilder.AddErrorDetail Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>

Appends an individual error occurrence to the [ErrorDetails](../Error/Error_ErrorDetails.md) of the error to build.

## Overloads
| Name | Description |
| ---- | ----------- |
| [AddErrorDetail(ErrorDetail)](#adderrordetailerrordetail) | Appends the specified error detail. |
| [AddErrorDetail(String, String, String, (String, Object)[])](#adderrordetailstring-string-string-string-object) | Appends a new error detail. |

## Remarks
The details are kept in the order they were added. Unlike [Error.WithErrorDetail](../Error/Error_WithErrorDetail.md), which copies the error on every call, these methods append in place, so they are the better choice when adding several details (e.g., in a loop).


## AddErrorDetail(ErrorDetail)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorBuilder.cs#L325" target="_blank">ErrorBuilder.cs</a>

Appends the specified individual error occurrence.

```csharp
public ErrorBuilder AddErrorDetail(ErrorDetail errorDetail);
```

### Parameters
#### `errorDetail` [ErrorDetail](../ErrorDetail/ErrorDetail.md)
A mandatory error occurrence to append.

### Returns
[ErrorBuilder](ErrorBuilder.md)

The current instance of the [ErrorBuilder](ErrorBuilder.md) class.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `errorDetail` is `null`.

### Examples
```csharp
var builder = ErrorBuilder.Validation(typeUri, "Invalid data to register a new user.");

foreach (var errorDetail in validationResult.ErrorDetails)
    builder.AddErrorDetail(errorDetail);
```


## AddErrorDetail(String, String, String, (String, Object)[])
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorBuilder.cs#L358" target="_blank">ErrorBuilder.cs</a>

Appends a new individual error occurrence.

```csharp
public ErrorBuilder AddErrorDetail(
        string? propertyPointer,
        string detail,
        string? detailTemplateId = null,
        params (string Key, object? Value)[] detailNamedValues);
```

### Parameters
#### `propertyPointer` [String](https://learn.microsoft.com/dotnet/api/system.string)
An optional [JSON Pointer](https://datatracker.ietf.org/doc/html/rfc6901) to the specific part of the request that caused the error.

#### `detail` [String](https://learn.microsoft.com/dotnet/api/system.string)
A mandatory human-readable message that describes the specific error occurrence.

#### `detailTemplateId` [String](https://learn.microsoft.com/dotnet/api/system.string)
An optional identifier of the message template to generate localized message. When it is `null`, no [DetailTemplated](../ErrorDetail/ErrorDetail_DetailTemplated.md) is set.

#### `detailNamedValues` ([String](https://learn.microsoft.com/dotnet/api/system.string), [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object))[]
An optional array of named values to be used when rendering the templated message. Values can be `null`; names must be unique. Requires `detailTemplateId`.

### Returns
[ErrorBuilder](ErrorBuilder.md)

The current instance of the [ErrorBuilder](ErrorBuilder.md) class.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `detail` or a name in `detailNamedValues` is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `detailTemplateId` is empty or consists only of white-space characters, a name in `detailNamedValues` is repeated, or `detailNamedValues` are passed without a `detailTemplateId`.

In both cases nothing is appended.

### Examples
```csharp
var error = ErrorBuilder.Validation(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:create:invalid"),
                "Invalid data to register a new user.")
            .AddErrorDetail(
                "#/username",
                "The username needs to be at least 3 characters long.",
                "error:user:create:username-length",
                ("username", "j"), ("minLength", 3))
            .AddErrorDetail(
                "#/email",
                "The email address is required.",
                "error:user:create:email-required")
            .Build();
```

## See Also
* [ErrorBuilder](ErrorBuilder.md)
* [ErrorDetail](../ErrorDetail/ErrorDetail.md)
* [Error.WithErrorDetail](../Error/Error_WithErrorDetail.md)
* [JSON Pointer](https://datatracker.ietf.org/doc/html/rfc6901)
