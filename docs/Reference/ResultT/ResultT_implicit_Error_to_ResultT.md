# Result&lt;T&gt;.Implicit(Error to Result&lt;T&gt;) Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Result.cs#L252" target="_blank">Result.cs</a>

Defines an implicit conversion of a given [Error](../Error/Error.md) to a failure [Result&lt;T&gt;](ResultT.md) (with a value), with a provided error.

```csharp
public static implicit operator Result<T>(Error error);
```

### Type Parameters
#### `T`
The type of the value associated with a successful [Result&lt;T&gt;](ResultT.md).

### Parameters
#### `error` [Error](../Error/Error.md)
An [Error](../Error/Error.md) to implicitly convert.

### Returns
[Result&lt;T&gt;](ResultT.md)

A new instance of the [Result&lt;T&gt;](ResultT.md) record (with a value) that represents a failed operation with the provided [Error](../Error/Error.md).

## Remarks
> [!NOTE]
> This is a recommended way to create a new failure [Result&lt;T&gt;](ResultT.md) instance (with a value). Another way is to use the [FromError(Error)](ResultT_FromError.md) static method.

## Examples
```csharp
Result<User> result = Error.Failure(
                    new ErrorUri("https://example.com/errors/invalid-operation"),
                    "Invalid operation",
                    "The requested operation is invalid in the current context.");
```

## See Also
* [Error](../Error/Error.md)
