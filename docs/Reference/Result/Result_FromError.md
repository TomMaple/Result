# Result.FromError(Error) Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Result.cs#L123" target="_blank">Result.cs</a>

Creates a new instance of the [Result](Result.md) record (without value) that represents a failed operation with the specified [Error](../Error/Error.md).

```csharp
public static Result FromError(Error error);
```

### Parameters
#### `error` [Error](../Error/Error.md)
The [Error](../Error/Error.md) instance that represents the error that occurred. This error will be associated with the created [Result](Result.md) instance.

### Returns
[Result](Result.md)

A new instance of the failure [Result](Result.md) instance with the provided instance of the [Error](../Error/Error.md) record.

## Remarks
> [!NOTE]
> This is a recommended way to create a new [Result](Result.md) instance (without value) with an error. Another way is to use the [Implicit(Error to Result)](Result_implicit_Error_to_Result.md) operator.

## Examples
```csharp
var error = Error.Validation(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:create:invalid"),
                "Invalid data to register a new user.");

return Result.FromError(error);
```

## See Also
* [Error](../Error/Error.md)
