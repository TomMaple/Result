# Error.GetHashCode() Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Error.cs#L428" target="_blank">Error.cs</a>

Returns a hash code that is consistent with [Equals(Error)](Error_Equals.md), incorporating the content of the [ErrorDetails](Error_ErrorDetails.md) collection and of the [Metadata](Error_Metadata.md) dictionary.

```csharp
public override int GetHashCode();
```

### Returns
[Int32](https://learn.microsoft.com/dotnet/api/system.int32)

A hash code for the current [Error](Error.md).

## Remarks
The hash code is computed from all properties of the [Error](Error.md), including each item of the [ErrorDetails](Error_ErrorDetails.md) collection and the entries of the [Metadata](Error_Metadata.md) dictionary (combined independently of their order). This keeps the hash code consistent with the value-based [Equals(Error)](Error_Equals.md): two [Error](Error.md) instances that are equal always produce the same hash code.

[Error](Error.md) is immutable, so its hash code never changes over the lifetime of an instance, and an error can be safely used as a key in a hash-based collection (such as a `Dictionary<TKey, TValue>` or a `HashSet<T>`). Methods such as [WithErrorDetail](Error_WithErrorDetail.md) and [WithMetadata](Error_WithMetadata.md) return a new instance.

> [!NOTE]
> The hash code of a [Metadata](Error_Metadata.md) value or a [TemplatedMessage](../TemplatedMessage/TemplatedMessage.md) parameter comes from the value itself. Storing a mutable object (e.g., a list) and changing it later changes the hash code of the error; store immutable values.

## See Also
* [Equals(Error)](Error_Equals.md)
* [ErrorDetails](Error_ErrorDetails.md)
* [Metadata](Error_Metadata.md)
