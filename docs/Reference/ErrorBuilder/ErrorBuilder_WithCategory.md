# ErrorBuilder.WithCategory(ErrorCategory) Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorBuilder.cs#L212" target="_blank">ErrorBuilder.cs</a>

Sets the [Category](../Error/Error_Category.md) of the error to build.

```csharp
public ErrorBuilder WithCategory(ErrorCategory category);
```

### Parameters
#### `category` [ErrorCategory](../ErrorCategory.md)
The category of the error.

### Returns
[ErrorBuilder](ErrorBuilder.md)

The current instance of the [ErrorBuilder](ErrorBuilder.md) class.

### Exceptions
[ArgumentOutOfRangeException](https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception)

The `category` is not a defined [ErrorCategory](../ErrorCategory.md) value. The builder is unchanged.

## Examples
```csharp
var error = ErrorBuilder.Failure(typeUri, "Cannot generate the report.")
    .WithCategory(isTransient ? ErrorCategory.Unavailable : ErrorCategory.Failure)
    .Build();
```

## See Also
* [ErrorBuilder](ErrorBuilder.md)
* [Category](../Error/Error_Category.md)
