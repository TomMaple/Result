# Result&lt;T&gt;.FromError(Error) Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Result.cs#L298" target="_blank">Result.cs</a>

Creates a new instance of the [Result&lt;T&gt;](ResultT.md) record (with value) that represents a failed operation with the specified [Error](../Error/Error.md).

```csharp
public static Result<T> FromError(Error error);
```

### Type Parameters
#### `T`
The type of the value associated with a successful [Result&lt;T&gt;](ResultT.md).

### Parameters
#### `error` [Error](../Error/Error.md)
The [Error](../Error/Error.md) instance that represents the error that occurred. This error will be associated with the created [Result&lt;T&gt;](ResultT.md) instance.

### Returns
[Result&lt;T&gt;](ResultT.md)

A new instance of the [Result&lt;T&gt;](ResultT.md) with the provided instance of the [Error](../Error/Error.md) record.

## Remarks
> [!NOTE]
> This is a recommended way to create a new [Result&lt;T&gt;](../ResultT/ResultT.md) instance (with value) with an error. Another way is to use the [Implicit(Error to Result&lt;T&gt;)](ResultT_implicit_Error_to_ResultT.md) operator.

## Examples
```csharp
var error = Error.Validation(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:create:invalid"),
                "Invalid data to register a new user.");

return Result<User>.FromError(error);
```

## See Also
* [Error](../Error/Error.md)
