# Error.GetHashCode() Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Error.cs#L283" target="_blank">Error.cs</a>

Returns a hash code that is consistent with [Equals(Error)](Error_Equals.md), incorporating the content of the [ErrorDetails](Error_ErrorDetails.md) collection.

```csharp
public override int GetHashCode();
```

### Returns
[Int32](https://learn.microsoft.com/dotnet/api/system.int32)

A hash code for the current [Error](Error.md).

## Remarks
The hash code is computed from all properties of the [Error](Error.md), including each item of the [ErrorDetails](Error_ErrorDetails.md) collection. This keeps the hash code consistent with the value-based [Equals(Error)](Error_Equals.md): two [Error](Error.md) instances that are equal always produce the same hash code.

> [!CAUTION]
> This hash code is **not stable over the lifetime of an instance**. It depends on the mutable [ErrorDetails](Error_ErrorDetails.md) collection, and [AddDetail(String, String, String, (String, Object)[])](Error_AddDetail.md) mutates that collection in place rather than returning a copy—so adding a detail changes the hash code of an [Error](Error.md) that already exists.
>
> Hash-based collections read a key's hash code once, when the entry is inserted, and never recompute it. If a detail is added to an [Error](Error.md) after it was used as a key, the entry stays filed under the old hash code and a lookup probes the wrong bucket: the entry becomes unreachable by key, even when searching with the very same reference. The same applies to `HashSet<T>` and to LINQ operators that hash, such as `Distinct()` and `GroupBy()`.
>
> An [Error](Error.md) must therefore be **fully built before** it is used as a key or added to a set. If more details may still be added, key on a stable value instead (for example, [TypeUri](Error_TypeUri.md)), or take an independent copy with a `with` expression and add the detail to that copy.

## See Also
* [Equals(Error)](Error_Equals.md)
* [ErrorDetails](Error_ErrorDetails.md)
