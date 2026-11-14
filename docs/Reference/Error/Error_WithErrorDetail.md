# Error.WithErrorDetail Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>

Returns a copy of the [Error](Error.md) with an individual error occurrence appended to its [ErrorDetails](Error_ErrorDetails.md).

## Overloads
| Name | Description |
| ---- | ----------- |
| [WithErrorDetail(ErrorDetail)](#witherrordetailerrordetail) | Returns a copy of the error with the specified error detail appended. |
| [WithErrorDetail(String, String, String, (String, Object)[])](#witherrordetailstring-string-string-string-object) | Returns a copy of the error with a new error detail appended. |

## Remarks
These methods **do not modify** the current instance; they return a new [Error](Error.md). The other members, including the existing error details, are kept.

Each call copies the error. To add several details, use [ToBuilder()](Error_ToBuilder.md) (or start with an [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md)) and [AddErrorDetail](../ErrorBuilder/ErrorBuilder_AddErrorDetail.md), which appends the details in place and builds the error once.


## WithErrorDetail(ErrorDetail)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Error.cs#L240" target="_blank">Error.cs</a>

Returns a copy of the current error with the specified individual error occurrence appended to the [ErrorDetails](Error_ErrorDetails.md).

```csharp
public Error WithErrorDetail(ErrorDetail errorDetail);
```

### Parameters
#### `errorDetail` [ErrorDetail](../ErrorDetail/ErrorDetail.md)
A mandatory error occurrence to append.

### Returns
[Error](Error.md)

A new instance of the [Error](Error.md) record with the error detail appended. The current instance is not modified.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `errorDetail` is `null`.

### Examples
```csharp
var errorDetail = new ErrorDetail("#/username", "The username is already taken.");

var error = Error.Validation(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:create:invalid"),
                "Invalid data to register a new user.")
            .WithErrorDetail(errorDetail);
```


## WithErrorDetail(String, String, String, (String, Object)[])
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Error.cs#L280" target="_blank">Error.cs</a>

Returns a copy of the current error with a new individual error occurrence appended to the [ErrorDetails](Error_ErrorDetails.md).

```csharp
public Error WithErrorDetail(
        string? propertyPointer,
        string detail,
        string? detailTemplateId = null,
        params ReadOnlySpan<(string Key, object? Value)> detailNamedValues);
```

### Parameters
#### `propertyPointer` [String](https://learn.microsoft.com/dotnet/api/system.string)
An optional [JSON Pointer](https://datatracker.ietf.org/doc/html/rfc6901) to the specific part of the request that caused the error.

#### `detail` [String](https://learn.microsoft.com/dotnet/api/system.string)
A mandatory human-readable message that describes the specific error occurrence.

#### `detailTemplateId` [String](https://learn.microsoft.com/dotnet/api/system.string)
An optional identifier of the message template to generate localized message. When it is `null`, no [DetailTemplated](../ErrorDetail/ErrorDetail_DetailTemplated.md) is set.

#### `detailNamedValues` [ReadOnlySpan](https://learn.microsoft.com/dotnet/api/system.readonlyspan-1)&lt;([String](https://learn.microsoft.com/dotnet/api/system.string), [Object](https://learn.microsoft.com/dotnet/api/system.object))&gt;
Optional named values (inline, an array or a collection expression) to be used when rendering the templated message. Values can be `null`; names must be unique. Requires `detailTemplateId`.

### Returns
[Error](Error.md)

A new instance of the [Error](Error.md) record with the error detail appended. The current instance is not modified.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `detail` or a name in `detailNamedValues` is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `detailTemplateId` is empty or consists only of white-space characters, a name in `detailNamedValues` is repeated, or `detailNamedValues` are passed without a `detailTemplateId`.

### Examples
```csharp
var error = Error.Validation(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:create:invalid"),
                "Invalid data to register a new user.")
            .WithErrorDetail(
                "#/username",
                "The username is already taken.",
                "error:user:create:username-taken",
                ("username", "john_doe"));
```

## See Also
* [ErrorDetails](Error_ErrorDetails.md)
* [ErrorDetail](../ErrorDetail/ErrorDetail.md)
* [ErrorBuilder.AddErrorDetail](../ErrorBuilder/ErrorBuilder_AddErrorDetail.md)
* [JSON Pointer](https://datatracker.ietf.org/doc/html/rfc6901)
