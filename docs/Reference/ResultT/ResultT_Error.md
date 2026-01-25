# Result&lt;T&gt;.Error Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Result.cs#L208" target="_blank">Result.cs</a>

Gets or sets the error that caused the operation to fail.

```csharp
public Error? Error { get; init; }
```

### Property Value
[Error](../Error/Error.md)

The error, if the result represents a failure; otherwise, `null`.

## Remarks
> [!CAUTION]
> Use the [IsSuccess()](IResult_IsSuccess.md) method to check whether the result represents a success or a failure before accessing this property.

## See Also
* [Error](../Error/Error.md)
