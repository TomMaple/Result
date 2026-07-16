# Error.GetHashCode() Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Error.cs#L262" target="_blank">Error.cs</a>

Returns a hash code that is consistent with [Equals(Error)](Error_Equals.md), incorporating the content of the [ErrorDetails](Error_ErrorDetails.md) collection.

```csharp
public override int GetHashCode();
```

### Returns
[Int32](https://learn.microsoft.com/dotnet/api/system.int32)

A hash code for the current [Error](Error.md).

## Remarks
The hash code is computed from all properties of the [Error](Error.md), including each item of the [ErrorDetails](Error_ErrorDetails.md) collection. This keeps the hash code consistent with the value-based [Equals(Error)](Error_Equals.md): two [Error](Error.md) instances that are equal always produce the same hash code.

> [!NOTE]
> Because the hash code depends on the (mutable) [ErrorDetails](Error_ErrorDetails.md) collection, it changes when a detail is added via [AddDetail(String, String, String, (String, Object)[])](Error_AddDetail.md). Avoid using an [Error](Error.md) as a key in a hash-based collection while it is still being populated with details.

## See Also
* [Equals(Error)](Error_Equals.md)
* [ErrorDetails](Error_ErrorDetails.md)
