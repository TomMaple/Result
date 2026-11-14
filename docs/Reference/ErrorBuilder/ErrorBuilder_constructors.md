# ErrorBuilder Constructor
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorBuilder.cs#L70" target="_blank">ErrorBuilder.cs</a>

Initializes a new instance of the [ErrorBuilder](ErrorBuilder.md) class with the mandatory members of an [Error](../Error/Error.md).

```csharp
public ErrorBuilder(ErrorCategory category, ErrorUri typeUri, string title);
```

### Parameters
#### `category` [ErrorCategory](../ErrorCategory.md)
The category of the error.

#### `typeUri` [ErrorUri](../ErrorUri/ErrorUri.md)
A record containing a *URI* reference that identifies the type of the problem. A default instance means `about:blank`.

#### `title` [String](https://learn.microsoft.com/dotnet/api/system.string)
A short, human-readable summary of the problem.

### Exceptions
[ArgumentOutOfRangeException](https://learn.microsoft.com/dotnet/api/system.argumentoutofrangeexception)

The `category` is not a defined [ErrorCategory](../ErrorCategory.md) value.

[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `title` is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `title` is empty or consists only of white-space characters.

## Remarks
The other members are empty: no detail, templated detail or instance *URI*, no error details and no metadata.

The static methods named after the categories (e.g., [Validation](ErrorBuilder_Validation.md)) are shorter when the category is known in advance.

## Examples
```csharp
var builder = new ErrorBuilder(
    category,
    ErrorUri.Tag("tag:exampleapp.com,2026:errors:report:generate"),
    "Cannot generate the report.");
```

## See Also
* [ErrorBuilder](ErrorBuilder.md)
* [ErrorCategory](../ErrorCategory.md)
