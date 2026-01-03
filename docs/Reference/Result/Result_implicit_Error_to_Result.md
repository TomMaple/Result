# Result.Implicit(Error to Result) Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Result.cs#L105" target="_blank">Result.cs</a>

Defines an implicit conversion of a given [Error](../Error/Error.md) to a failure [Result](Result.md) (without a value), with a provided error.

```csharp
public static implicit operator Result(Error error);
```

### Returns
[Result](Result.md)

A new instance of the [Result](Result.md) record (without a value) that represents a failed operation with provided [Error](../Error/Error.md).

## Remarks
> [!NOTE]
> This is a recommended way to create a new failure [Result](Result.md) instance (without a value). Another way is to use the [FromError(Error)](Result_FromError.md) static method.

## Examples
```csharp
Result result = Error.Failure(
                    new ErrorUri("https://example.com/errors/invalid-operation"),
                    "Invalid operation",
                    "The requested operation is invalid in the current context.");
```

## See Also
* [Error](../Error/Error.md)
