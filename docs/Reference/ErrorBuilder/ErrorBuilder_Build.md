# ErrorBuilder.Build() Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorBuilder.cs#L485" target="_blank">ErrorBuilder.cs</a>

Returns a new [Error](../Error/Error.md) with the current members of the builder.

```csharp
public Error Build();
```

### Returns
[Error](../Error/Error.md)

A new instance of the [Error](../Error/Error.md) record.

## Remarks
This method **never throws**: every argument was validated when it was passed to the builder.

The returned error is a **snapshot**: later changes to the builder do not affect it, and the builder can still be used, e.g., to build another, slightly different error. Calling this method again without changes returns an equal error that shares the same read-only collections.

There is no implicit conversion from [ErrorBuilder](ErrorBuilder.md) to [Error](../Error/Error.md); call this method to get the error.

## Examples
```csharp
var builder = ErrorBuilder.Validation(
    ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:create:invalid"),
    "Invalid data to register a new user.");

foreach (var field in invalidFields)
    builder.AddErrorDetail($"#/{field}", $"The {field} is invalid.");

Error error = builder.Build();
```

## See Also
* [ErrorBuilder](ErrorBuilder.md)
* [Error](../Error/Error.md)
