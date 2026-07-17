# Error.AddDetail(String, String, String, (String, Object)[]) Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Error.cs#L235" target="_blank">Error.cs</a>

Adds details of an individual error occurrence found.

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

The current instance of the [Error](Error.md) record with the new error detail added.

## Remarks
> [!NOTE]
> This is a recommended way to add individual error details to an error instance, rather than directly using the [ErrorDetails](Error_ErrorDetails.md) property.

This method **mutates the current instance in place** and returns that same instance; it does not create a copy. The returned reference is the one it was called on, so a fluent chain such as `error.AddDetail(...).AddDetail(...)` accumulates the details on the original `error`.

> [!CAUTION]
> Because the instance is mutated rather than copied, every holder of that instance observes the added detail. Do not add details to an [Error](Error.md) that is shared or cached—for example, one held in a `static readonly` field and reused across requests. Doing so appends to the shared instance on every call, which both grows it without bound and leaks one caller's detail into another caller's error.
>
> To add a detail to a shared [Error](Error.md), first take an independent copy with a `with` expression, which deep-copies the [ErrorDetails](Error_ErrorDetails.md) collection:
>
> ```csharp
> var copy = sharedError with { };
> copy.AddDetail("#/id", "The user does not exist.");
> ```

> [!CAUTION]
> Because [GetHashCode()](Error_GetHashCode.md) incorporates the [ErrorDetails](Error_ErrorDetails.md) collection, adding a detail **changes the hash code** of the instance. An [Error](Error.md) must therefore be fully built before it is used as a key in a hash-based collection (such as a `Dictionary<TKey, TValue>` or a `HashSet<T>`), or in an operation that relies on hashing (such as `Distinct()` or `GroupBy()`). Adding a detail after insertion leaves the entry stored under its previous hash code, where a lookup will not find it—not even with the very same reference.

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
