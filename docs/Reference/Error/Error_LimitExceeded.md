# Error.LimitExceeded Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>

Creates a new instance of the [Error](Error.md) object of the *LimitExceeded* category.

## Overloads
| Name | Description |
| ---- | ----------- |
| [LimitExceeded(ErrorUri, String, String, ErrorUri, String, (String, Object)[])](#limitexceedederroruri-string-string-erroruri-string-string-object) | Creates a new limit exceeded error with specified parameters. |
| [LimitExceeded(ErrorUri, String, String, ErrorUri, String, IEnumerable<KeyValuePair<String, Object>>)](#limitexceedederroruri-string-string-erroruri-string-ienumerablekeyvaluepairstring-object) | Creates a new limit exceeded error with specified parameters. |

## Remarks
This error category is used when the request is valid and permitted, but a usage limit, quota or rate has been exhausted. The operation may succeed later or once the limit is raised.

It applies to business limits (e.g. a plan quota or a maximum number of items per account) as well as throttling (rate limiting). Do not use it for input-size checks; use [Validation](Error_Validation.md) instead.

It relates to `429` HTTP status code (Too Many Requests), but can also be used in other contexts where a limit was exceeded and needs to be reported.


## LimitExceeded(ErrorUri, String, String, ErrorUri, String, (String, Object)[])
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Error.cs#L1474" target="_blank">Error.cs</a>

Creates a new instance of the [Error](Error.md) object of the *LimitExceeded* category with specified parameters.

```csharp
public static Error LimitExceeded(
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

A new instance of the [Error](Error.md) record of the *LimitExceeded* [ErrorCategory](../ErrorCategory.md) initialized with provided parameters.

### Exceptions
[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `typeUri` parameter resolves to an empty or whitespace-only value.

### Remarks
> [!NOTE]
> To add individual error details to an error instance, use the [AddDetail(string, string, string, (string, object)[])](Error_AddDetail.md) method.

### Examples
```csharp
var error = Error.LimitExceeded(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:reports:limit-exceeded"),
                "Cannot generate the report.",
                "The monthly limit of 100 reports has been reached.",
                ErrorUri.Locator("https://api.exampleapp.com/errors/546523452/details"),
                "errors.reports.limitexceeded",
                ("accountId", "D12345"), ("limit", 100));
```


## LimitExceeded(ErrorUri, String, String, ErrorUri, String, IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Error.cs#L1513" target="_blank">Error.cs</a>

Creates a new instance of the [Error](Error.md) object of the *LimitExceeded* category with specified parameters.

```csharp
public static Error LimitExceeded(
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

A new instance of the [Error](Error.md) record of the *LimitExceeded* [ErrorCategory](../ErrorCategory.md) initialized with provided parameters.

### Exceptions
[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `typeUri` parameter resolves to an empty or whitespace-only value.

### Remarks
> [!NOTE]
> To add individual error details to an error instance, use the [AddDetail(string, string, string, (string, object)[])](Error_AddDetail.md) method.

### Examples
```csharp
var error = Error.LimitExceeded(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:reports:limit-exceeded"),
                "Cannot generate the report.",
                "The monthly limit of 100 reports has been reached.",
                ErrorUri.Locator("https://api.exampleapp.com/errors/546523452/details"),
                "errors.reports.limitexceeded",
                new Dictionary<string, object> { ["accountId"] = "D12345", ["limit"] = 100 });
```

## See Also
* [ErrorCategory](../ErrorCategory.md)
* [ErrorDetail](../ErrorDetail/ErrorDetail.md)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Problem Details for HTTP APIs - RFC 7807 is dead, long live RFC 9457](https://blog.frankel.ch/problem-details-http-apis/)
