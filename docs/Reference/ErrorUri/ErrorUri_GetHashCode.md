# ErrorUri.GetHashCode() Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/ErrorUri.cs#L127" target="_blank">ErrorUri.cs</a>

Returns a hash code that is consistent with [Equals(ErrorUri)](ErrorUri_Equals.md).

```csharp
public override int GetHashCode();
```

### Returns
[Int32](https://learn.microsoft.com/dotnet/api/system.int32)

A hash code for the current [ErrorUri](ErrorUri.md).

## Remarks
The hash code is computed from the [Value](ErrorUri_Value.md) property rather than the underlying field. This keeps it consistent with [Equals(ErrorUri)](ErrorUri_Equals.md): two [ErrorUri](ErrorUri.md) instances that are equal always produce the same hash code, so a default instance and the one returned by [None()](ErrorUri_None.md) can be used interchangeably as keys in a hash-based collection.

> [!NOTE]
> Unlike [Error.GetHashCode()](../Error/Error_GetHashCode.md), this hash code is stable: an [ErrorUri](ErrorUri.md) is immutable once created.

## See Also
* [Equals(ErrorUri)](ErrorUri_Equals.md)
* [Value](ErrorUri_Value.md)
* [None()](ErrorUri_None.md)
