# ErrorBuilder.Critical(ErrorUri, String) Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorBuilder.cs#L167" target="_blank">ErrorBuilder.cs</a>

Returns a new [ErrorBuilder](ErrorBuilder.md) of the *Critical* [ErrorCategory](../ErrorCategory.md).

```csharp
public static ErrorBuilder Critical(ErrorUri typeUri, string title);
```

### Parameters
#### `typeUri` [ErrorUri](../ErrorUri/ErrorUri.md)
A mandatory record containing a *URI* reference that identifies the type of the problem.

#### `title` [String](https://learn.microsoft.com/dotnet/api/system.string)
A mandatory short, human-readable summary of the problem.

### Returns
[ErrorBuilder](ErrorBuilder.md)

A new instance of the [ErrorBuilder](ErrorBuilder.md) class for a critical error.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `title` is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `title` is empty or consists only of white-space characters.

## Remarks
It is equivalent to `new ErrorBuilder(ErrorCategory.Critical, typeUri, title)`. See [Error.Critical](../Error/Error_Critical.md) for when to use this category.

## Examples
```csharp
var error = ErrorBuilder.Critical(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:system:critical"),
                "The service cannot continue.")
            .WithDetail("A human-readable explanation of this occurrence.")
            .Build();
```

## See Also
* [ErrorBuilder](ErrorBuilder.md)
* [Error.Critical](../Error/Error_Critical.md)
* [ErrorCategory](../ErrorCategory.md)
