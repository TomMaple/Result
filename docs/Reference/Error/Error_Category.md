# Error.Category Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Error.cs#L75" target="_blank">Error.cs</a>

Gets the category of the error.
```csharp
public ErrorCategory Category { get; }
```

### Property Value
[ErrorCategory](../ErrorCategory.md)

The category of the error.

## Remarks
This is a mandatory property. It is always a defined [ErrorCategory](../ErrorCategory.md) value.

If not specified, the category defaults to *Validation*.

## See Also
* [ErrorCategory](../ErrorCategory.md) — for the list of defined error categories.
