# Error.AddDetail(String, String, String, (String, Object)[]) Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Error.cs#L180" target="_blank">Error.cs</a>

Adds details of an individual error occurence found.

```csharp
public Error AddDetail(
        string? propertyPointer,
        string detail,
        string? messageId = null,
        params (string key, object value)[] namedValues);
```

### Parameters
#### `propertyPointer` [String](https://learn.microsoft.com/dotnet/api/system.string)
An optional [JSON Pointer](https://datatracker.ietf.org/doc/html/rfc6901) to the specific part of the request that caused the error.

#### `detail` [String](https://learn.microsoft.com/dotnet/api/system.string)
A mandatory human-readable message that describes the specific error occurrence.

#### `messageId` [String](https://learn.microsoft.com/dotnet/api/system.string)
An optional identifier of the message template to generate localized message.

#### `namedValues` ([String](https://learn.microsoft.com/dotnet/api/system.string), [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object))[]
An optional array of named values to be used when rendering the templated message.

### Returns
[Error](Error.md)

The current instance of the [Error](Error.md) class with the new error detail added.

## Remarks
> [!NOTE]
> This is a recommended way to add individual error details to an error instance, rather than directly using the [ErrorDetails](Error_ErrorDetails.md) property.

This is a collection of individual error occurrences found, or an empty collection if no such details are available.

Each item is a [ErrorDetail](../ErrorDetail/ErrorDetail.md) object that describes a specific aspect of the overall error condition. It contains properties that provide a human-readable message, and optionally a templated message (so than the client can localize it) and a property path indicating the specific part of the request that caused the error.

It should not be assigned directly (i.e., via the property setter) but rather through the [AddDetail(String, String, String, (String, Object)[])](Error_AddDetail.md) method of the [Error](Error.md) class, which uses [TemplatedMessage](../TemplatedMessage/TemplatedMessage.md) to validate and provide the value. For deserialization, the setter is marked as `init` to allow assignment during object initialization.

## Examples
```csharp
var error = Error.Validation(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:create:invalid"),
                "Invalid data to register a new user.");

error.AddDetail(
    "#/username",
    "The username is already taken.",
    "error:user:create:username-taken",
    ("username", "john_doe"));

error.AddDetail(
    "#/email",
    "The email address is required.",
    "error:user:create:email-required",
    ("email", ""));
```

## See Also
* [ErrorDetail](../ErrorDetail/ErrorDetail.md)
* [JSON Pointer](https://datatracker.ietf.org/doc/html/rfc6901)