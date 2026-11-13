# ErrorBuilder.WithDetail(String) Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorBuilder.cs#L255" target="_blank">ErrorBuilder.cs</a>

Sets the [Detail](../Error/Error_Detail.md) of the error to build.

```csharp
public ErrorBuilder WithDetail(string? detail);
```

### Parameters
#### `detail` [String](https://learn.microsoft.com/dotnet/api/system.string)
A human-readable explanation specific to this occurrence of the problem (the RFC 9457 "detail" member), or `null` to clear it.

### Returns
[ErrorBuilder](ErrorBuilder.md)

The current instance of the [ErrorBuilder](ErrorBuilder.md) class.

## Remarks
To add an individual error occurrence (an [ErrorDetail](../ErrorDetail/ErrorDetail.md)), use [AddErrorDetail](ErrorBuilder_AddErrorDetail.md).

## Examples
```csharp
var error = ErrorBuilder.NotFound(typeUri, "Cannot update the account.")
    .WithDetail("The account with the provided ID has not been found.")
    .Build();
```

## See Also
* [ErrorBuilder](ErrorBuilder.md)
* [Detail](../Error/Error_Detail.md)
