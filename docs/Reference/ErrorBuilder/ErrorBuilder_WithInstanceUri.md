# ErrorBuilder.WithInstanceUri(ErrorUri) Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorBuilder.cs#L307" target="_blank">ErrorBuilder.cs</a>

Sets the [InstanceUri](../Error/Error_InstanceUri.md) of the error to build.

```csharp
public ErrorBuilder WithInstanceUri(ErrorUri? instanceUri);
```

### Parameters
#### `instanceUri` [ErrorUri](../ErrorUri/ErrorUri.md)
A *URI* reference that identifies the specific occurrence of the problem, or `null` to clear it.

### Returns
[ErrorBuilder](ErrorBuilder.md)

The current instance of the [ErrorBuilder](ErrorBuilder.md) class.

### Exceptions
[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `instanceUri` is `about:blank` ([ErrorUri.None()](../ErrorUri/ErrorUri_None.md) or a default [ErrorUri](../ErrorUri/ErrorUri.md)). The builder is not changed.

## Remarks
`about:blank` has a meaning only as a type (*RFC 9457*, section 4.2.1), so it is rejected as an instance; pass `null` when there is no instance.

## Examples
```csharp
var error = ErrorBuilder.Failure(typeUri, "Cannot generate the report.")
    .WithInstanceUri(ErrorUri.Locator("https://api.exampleapp.com/errors/6283365904847834/details"))
    .Build();
```

## See Also
* [ErrorBuilder](ErrorBuilder.md)
* [InstanceUri](../Error/Error_InstanceUri.md)
