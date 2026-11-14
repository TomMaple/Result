# ErrorBuilder.NotImplemented(ErrorUri, String) Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorBuilder.cs#L178" target="_blank">ErrorBuilder.cs</a>

Returns a new [ErrorBuilder](ErrorBuilder.md) of the *NotImplemented* [ErrorCategory](../ErrorCategory.md).

```csharp
public static ErrorBuilder NotImplemented(ErrorUri typeUri, string title);
```

### Parameters
#### `typeUri` [ErrorUri](../ErrorUri/ErrorUri.md)
A mandatory record containing a *URI* reference that identifies the type of the problem.

#### `title` [String](https://learn.microsoft.com/dotnet/api/system.string)
A mandatory short, human-readable summary of the problem.

### Returns
[ErrorBuilder](ErrorBuilder.md)

A new instance of the [ErrorBuilder](ErrorBuilder.md) class for a not implemented error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `title` is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `title` is empty or consists only of white-space characters.

## Remarks
It is equivalent to `new ErrorBuilder(ErrorCategory.NotImplemented, typeUri, title)`. See [Error.NotImplemented](../Error/Error_NotImplemented.md) for when to use this category.

## Examples
```csharp
var error = ErrorBuilder.NotImplemented(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:export:not-implemented"),
                "The export format is not supported yet.")
            .WithDetail("A human-readable explanation of this occurrence.")
            .Build();
```

## See Also
* [ErrorBuilder](ErrorBuilder.md)
* [Error.NotImplemented](../Error/Error_NotImplemented.md)
* [ErrorCategory](../ErrorCategory.md)
