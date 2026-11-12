# Error.GetHashCode() Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Error.cs#L652" target="_blank">Error.cs</a>

Returns a hash code that is consistent with [Equals(Error)](Error_Equals.md), incorporating the content of the [ErrorDetails](Error_ErrorDetails.md) collection and of the [Metadata](Error_Metadata.md) dictionary.

```csharp
public override int GetHashCode();
```

### Returns
[Int32](https://learn.microsoft.com/dotnet/api/system.int32)

A hash code for the current [Error](Error.md).

## Remarks
The hash code is computed from all properties of the [Error](Error.md), including each item of the [ErrorDetails](Error_ErrorDetails.md) collection and the entries of the [Metadata](Error_Metadata.md) dictionary (combined independently of their order). This keeps the hash code consistent with the value-based [Equals(Error)](Error_Equals.md): two [Error](Error.md) instances that are equal always produce the same hash code.

> [!CAUTION]
> This hash code is **not stable over the lifetime of an instance**. It depends on the mutable [ErrorDetails](Error_ErrorDetails.md) collection, and [AddDetail(String, String, String, (String, Object)[])](Error_AddDetail.md) mutates that collection in place rather than returning a copy—so adding a detail changes the hash code of an [Error](Error.md) that already exists.
>
> Hash-based collections read a key's hash code once, when the entry is inserted, and never recompute it. If a detail is added to an [Error](Error.md) after it was used as a key, the entry stays filed under the old hash code and a lookup probes the wrong bucket: the entry becomes unreachable by key, even when searching with the very same reference. The same applies to `HashSet<T>` and to LINQ operators that hash, such as `Distinct()` and `GroupBy()`.
>
> An [Error](Error.md) must therefore be **fully built before** it is used as a key or added to a set. If more details may still be added, key on a stable value instead (for example, [TypeUri](Error_TypeUri.md)), or take an independent copy with a `with` expression and add the detail to that copy.

> [!NOTE]
> [Metadata](Error_Metadata.md) does not affect the stability of the hash code: [WithMetadata(String, Object)](Error_WithMetadata.md) and [WithoutMetadata(String)](Error_WithoutMetadata.md) return a new instance and never modify an existing one.

## See Also
* [Equals(Error)](Error_Equals.md)
* [ErrorDetails](Error_ErrorDetails.md)
* [Metadata](Error_Metadata.md)
