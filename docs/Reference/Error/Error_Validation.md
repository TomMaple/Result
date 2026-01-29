# Error.Validation Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>

Creates a new instance of the [Error](Error.md) object of the *Validation* category.

## Overloads
| Name | Description |
| ---- | ----------- |
| [Validation(ErrorUri, String, String, ErrorUri, String, (String, Object)[])](#validationerroruri-string-string-erroruri-string-string-object) | Creates a new validation error with specified parameters. |
| [Validation(ErrorUri, String, String, ErrorUri, String, IEnumerable<KeyValuePair<String, Object>>)](#validationerroruri-string-string-erroruri-string-ienumerablekeyvaluepairstring-object) | Creates a new validation error with specified parameters. |

## Remarks
This error category represents validation errors that prevent the underlying service from completing.

It relates to `400` HTTP status code (Bad Request), but can also be used in other contexts where validation errors need to be reported.


## Validation(ErrorUri, String, String, ErrorUri, String, (String, Object)[])
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Error.cs#L219" target="_blank">Error.cs</a>

Creates a new instance of the [Error](Error.md) object of the *Validation* category with specified parameters.

```csharp
public static Error Validation(
        ErrorUri typeUri,
        string title,
        string? detail = null,
        ErrorUri? instanceUri = null,
        string? detailTemplateId = null,
        params (string Key, object Value)[] detailNamedValues);
```

### Parameters
#### `typeUri` [ErrorUri](../ErrorUri/ErrorUri.md)
A mandatory record containing a *URI* reference that identifies the type of the problem.

#### `title` [String](https://learn.microsoft.com/dotnet/api/system.string)
A mandatory short, human-readable summary of the problem.

#### `detail` [String](https://learn.microsoft.com/dotnet/api/system.string)
An optional, human-readable explanation specific to this occurrence of the problem.

#### `instanceUri` [ErrorUri](../ErrorUri/ErrorUri.md)
An optional *URI* reference that identifies the specific occurrence of the problem.

#### `detailTemplateId` [String](https://learn.microsoft.com/dotnet/api/system.string)
An optional identifier of the message template to generate custom, localized message.

#### `detailNamedValues` ([String](https://learn.microsoft.com/dotnet/api/system.string), [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object))[]
An optional array of named values to be used when rendering the templated message. This parameter will be ignored if `detailTemplateId` is `null`.

### Returns
[Error](Error.md)

A new instance of the [Error](Error.md) record of the *Validation* [ErrorCategory](../ErrorCategory.md) initialized with provided parameters.

### Exceptions
[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `typeUri` parameter and/or the `instanceUri` parameter are not a valid *URI Locator* or *URI Tag*.

### Remarks
> [!NOTE]
> To add individual error details to an error instance, use the [AddDetail(string, string, string, (string, object)[])](Error_AddDetail.md) method.

### Examples
```csharp
var error = Error.Validation(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:create:invalid_data"),
                "Invalid data to register a new user.",
                "The provided user details are invalid. Please update and retry.",
                ErrorUri.Locator("https://api.exampleapp.com/errors/7983750923784025/details"),
                "errors.user.create.invalid_data",
                ("entity", "user"), ("canRetry", true));

error.AddDetail(
    "#/username",
    "The username needs to be at least 3 characters long.",
    "error:user:create:username-length",
    ("username", "j"), ("minlength", 3));

error.AddDetail(
    "#/email",
    "The email address is required.",
    "error:user:create:email-required",
    ("email", ""));
```


## Validation(ErrorUri, String, String, ErrorUri, String, IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Error.cs#L258" target="_blank">Error.cs</a>

Creates a new instance of the [Error](Error.md) object of the *Validation* category with specified parameters.

```csharp
public static Error Validation(
        ErrorUri typeUri,
        string title,
        string? detail = null,
        ErrorUri? instanceUri = null,
        string? detailTemplateId = null,
        IEnumerable<KeyValuePair<string, object>>? detailNamedValues = null);
```

### Parameters
#### `typeUri` [ErrorUri](../ErrorUri/ErrorUri.md)
A mandatory record containing a *URI* reference that identifies the type of the problem.

#### `title` [String](https://learn.microsoft.com/dotnet/api/system.string)
A mandatory short, human-readable summary of the problem.

#### `detail` [String](https://learn.microsoft.com/dotnet/api/system.string)
An optional, human-readable explanation specific to this occurrence of the problem.

#### `instanceUri` [ErrorUri](../ErrorUri/ErrorUri.md)
An optional *URI* reference that identifies the specific occurrence of the problem.

#### `detailTemplateId` [String](https://learn.microsoft.com/dotnet/api/system.string)
An optional identifier of the message template to generate custom, localized message.

#### `detailNamedValues` [IEnumerable](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable-1)&lt;[KeyValuePair](https://learn.microsoft.com/dotnet/api/system.collections.generic.keyvaluepair-2)&lt;[String](https://learn.microsoft.com/dotnet/api/system.string), [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)&gt;&gt;
An optional sequence of key-value pairs to be used when rendering the templated message. This parameter will be ignored if `detailTemplateId` is `null`.

### Returns
[Error](Error.md)

A new instance of the [Error](Error.md) record of the *Validation* [ErrorCategory](../ErrorCategory.md) initialized with provided parameters.

### Exceptions
[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `typeUri` parameter and/or the `instanceUri` parameter are not a valid *URI Locator* or *URI Tag*.

### Remarks
> [!NOTE]
> To add individual error details to an error instance, use the [AddDetail(string, string, string, (string, object)[])](Error_AddDetail.md) method.

### Examples
```csharp
var error = Error.Validation(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:create:invalid_data"),
                "Invalid data to register a new user.",
                "The provided user details are invalid. Please update and retry.",
                ErrorUri.Locator("https://api.exampleapp.com/errors/7983750923784025/details"),
                "errors.user.create.invalid_data",
                new Dictionary<string, object> { ["entity"] = "user", ["canRetry"] = true });

error.AddDetail(
    "#/username",
    "The username needs to be at least 3 characters long.",
    "error:user:create:username-length",
    ("username", "j"), ("minlength", 3));

error.AddDetail(
    "#/email",
    "The email address is required.",
    "error:user:create:email-required",
    ("email", ""));
```

## See Also
* [ErrorCategory](../ErrorCategory.md)
* [ErrorDetail](../ErrorDetail/ErrorDetail.md)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Problem Details for HTTP APIs - RFC 7807 is dead, long live RFC 9457](https://blog.frankel.ch/problem-details-http-apis/)
* [JSON Pointer](https://datatracker.ietf.org/doc/html/rfc6901)
