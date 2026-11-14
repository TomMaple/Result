# Error.NotImplemented Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>

Creates a new instance of the [Error](Error.md) object of the *NotImplemented* category.

## Overloads
| Name | Description |
| ---- | ----------- |
| [NotImplemented(ErrorUri, String, String, ErrorUri, String, (String, Object)[])](#notimplementederroruri-string-string-erroruri-string-string-object) | Creates a new not implemented error with specified parameters. |
| [NotImplemented(ErrorUri, String, String, ErrorUri, String, IEnumerable<KeyValuePair<String, Object>>)](#notimplemented-string-string-erroruri-string-ienumerablekeyvaluepairstring-object) | Creates a new not implemented error with specified parameters. |

## Remarks
This error category is to report not implemented functionalities, cases or features, or the ones that are still in development.

It relates to `501` HTTP status code (Not Implemented).


## NotImplemented(ErrorUri, String, String, ErrorUri, String, (String, Object)[])
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Error.cs#L725" target="_blank">Error.cs</a>

Creates a new instance of the [Error](Error.md) object of the *NotImplemented* category with specified parameters.

```csharp
public static Error NotImplemented(
        ErrorUri typeUri,
        string title,
        string? detail = null,
        ErrorUri? instanceUri = null,
        string? detailTemplateId = null,
        params ReadOnlySpan<(string Key, object? Value)> detailNamedValues);
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

#### `detailNamedValues` [ReadOnlySpan](https://learn.microsoft.com/dotnet/api/system.readonlyspan-1)&lt;([String](https://learn.microsoft.com/dotnet/api/system.string), [Object](https://learn.microsoft.com/dotnet/api/system.object))&gt;
Optional named values (inline, an array or a collection expression) to be used when rendering the templated message. Values can be `null`; names must be unique and not `null`. Requires `detailTemplateId`: passing values without it throws an [ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception).

### Returns
[Error](Error.md)

A new instance of the [Error](Error.md) record of the *NotImplemented* [ErrorCategory](../ErrorCategory.md) initialized with provided parameters.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `title` or a name in `detailNamedValues` is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `title` or the `detailTemplateId` is empty or consists only of white-space characters, a name in `detailNamedValues` is repeated, or `detailNamedValues` are passed without a `detailTemplateId`.

### Remarks
> [!NOTE]
> To add individual error details, use [WithErrorDetail](Error_WithErrorDetail.md), which returns a copy, or build the error with an [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md) when adding several.

### Examples
```csharp
var error = Error.NotImplemented(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:payment:method.notimplemented"),
                "Payment method not implemented.",
                "Debit cards are not supported yet.",
                ErrorUri.Locator("https://api.exampleapp.com/errors/8754398/details"),
                "errors.payment.method.notimplemented",
                ("paymentId", "GVX235346"), ("cardType", "Visa Debit"));
```


## NotImplemented(ErrorUri, String, String, ErrorUri, String, IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Error.cs#L741" target="_blank">Error.cs</a>

Creates a new instance of the [Error](Error.md) object of the *NotImplemented* category with specified parameters.

```csharp
[OverloadResolutionPriority(1)]
public static Error NotImplemented(
        ErrorUri typeUri,
        string title,
        string? detail = null,
        ErrorUri? instanceUri = null,
        string? detailTemplateId = null,
        IEnumerable<KeyValuePair<string, object?>>? detailNamedValues = null);
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
An optional sequence of key-value pairs to be used when rendering the templated message. Values can be `null`; names must be unique and not `null`. Requires `detailTemplateId`: passing values without it throws an [ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception).

### Returns
[Error](Error.md)

A new instance of the [Error](Error.md) record of the *NotImplemented* [ErrorCategory](../ErrorCategory.md) initialized with provided parameters.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `title` or a name in `detailNamedValues` is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `title` or the `detailTemplateId` is empty or consists only of white-space characters, a name in `detailNamedValues` is repeated, or `detailNamedValues` are passed without a `detailTemplateId`.

### Remarks
> [!NOTE]
> To add individual error details, use [WithErrorDetail](Error_WithErrorDetail.md), which returns a copy, or build the error with an [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md) when adding several.

### Examples
```csharp
var error = Error.NotImplemented(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:payment:method.notimplemented"),
                "Payment method not implemented.",
                "Debit cards are not supported yet.",
                ErrorUri.Locator("https://api.exampleapp.com/errors/8754398/details"),
                "errors.payment.method.notimplemented",
                new Dictionary<string, object?> { ["paymentId"] = "GVX235346", ["cardType"] = "Visa Debit" });
```

## See Also
* [ErrorCategory](../ErrorCategory.md)
* [ErrorDetail](../ErrorDetail/ErrorDetail.md)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Problem Details for HTTP APIs - RFC 7807 is dead, long live RFC 9457](https://blog.frankel.ch/problem-details-http-apis/)
* [JSON Pointer](https://datatracker.ietf.org/doc/html/rfc6901)
