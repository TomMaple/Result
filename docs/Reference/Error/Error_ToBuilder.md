# Error.ToBuilder() Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Error.cs#L221" target="_blank">Error.cs</a>

Returns a new [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md) initialized with the members of the current error.

```csharp
public ErrorBuilder ToBuilder();
```

### Returns
[ErrorBuilder](../ErrorBuilder/ErrorBuilder.md)

A new instance of the [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md) class with all the members of the current error.

## Remarks
[Error](Error.md) is immutable. Use this method to obtain a changed copy of an error: change the members on the builder and call [Build()](../ErrorBuilder/ErrorBuilder_Build.md). The current instance is never modified.

It replaces the `with` expression of earlier versions (e.g., `error with { Title = "…" }`), which no longer compiles because the properties of [Error](Error.md) are get-only.

The builder shares the [ErrorDetails](Error_ErrorDetails.md) and [Metadata](Error_Metadata.md) collections of the current error until the first change to each of them, so `error.ToBuilder().Build()` does not copy them.

## Examples
```csharp
var error = Error.NotFound(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:account:notfound"),
                "Cannot update the account.");

var localizedError = error.ToBuilder()
    .WithTitle("Impossible de mettre à jour le compte.")
    .WithInstanceUri(ErrorUri.Locator("https://api.exampleapp.com/errors/8783927589734857/details"))
    .Build();
```

## See Also
* [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md)
* [WithErrorDetail](Error_WithErrorDetail.md)
* [WithMetadata](Error_WithMetadata.md)
