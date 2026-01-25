# IResult.Error Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Result.cs#L29" target="_blank">Result.cs</a>

Gets the [Error](../Error/Error.md) that caused the operation to fail.

```csharp
public Error? Error { get; }
```

### PropertyValue
[Error](../Error/Error.md)

The error, if the result represents a failure; otherwise, `null`.

## Remarks
If this property has a value (other than `null`), it indicates that the operation has failed.

> [!CAUTION]
> Better use the [IsSuccess()](IResult_IsSuccess.md) method to check whether the result represents a success or a failure before accessing this property.

## See Also
* [Error](../Error/Error.md)
