# Result.Error Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Result.cs#L76" target="_blank">Result.cs</a>

Gets or sets the error that caused the operation to fail.
```csharp
public Error? Error { get; init; }
```

### PropertyValue
[Error](../Error.md)

The error, if the result represents a failure; otherwise, `null`.

## Remarks
If this property has a value (other than `null`), it indicates that the operation has failed.

## See Also
* [Error](../Error.md)
