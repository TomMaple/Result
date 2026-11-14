# ErrorBuilder.WithTitle(String) Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorBuilder.cs#L239" target="_blank">ErrorBuilder.cs</a>

Sets the [Title](../Error/Error_Title.md) of the error to build.

```csharp
public ErrorBuilder WithTitle(string title);
```

### Parameters
#### `title` [String](https://learn.microsoft.com/dotnet/api/system.string)
A short, human-readable summary of the problem.

### Returns
[ErrorBuilder](ErrorBuilder.md)

The current instance of the [ErrorBuilder](ErrorBuilder.md) class.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `title` is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `title` is empty or consists only of white-space characters.

In both cases the builder is unchanged.

## Examples
```csharp
var localized = error.ToBuilder()
    .WithTitle("Impossible de mettre à jour le compte.")
    .Build();
```

## See Also
* [ErrorBuilder](ErrorBuilder.md)
* [Title](../Error/Error_Title.md)
