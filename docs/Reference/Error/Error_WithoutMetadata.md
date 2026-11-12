# Error.WithoutMetadata(String) Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Error.cs#L392" target="_blank">Error.cs</a>

Returns a copy of the current error without the [Metadata](Error_Metadata.md) entry of the specified key.

```csharp
public Error WithoutMetadata(string key);
```

### Parameters
#### `key` [String](https://learn.microsoft.com/dotnet/api/system.string)
A mandatory key of the metadata entry to remove.

### Returns
[Error](Error.md)

A new instance of the [Error](Error.md) record without the entry. If the key is absent, the returned copy is equal to the current instance.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `key` is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `key` is empty or consists only of white-space characters.

## Remarks
This method **does not modify** the current instance; it returns a new [Error](Error.md) with a new, read-only [Metadata](Error_Metadata.md) dictionary (or, if the key is absent, sharing the current one). The order of the remaining entries is kept.

Keys are compared ordinally (case-sensitive).

## Examples
```csharp
var error = Error.LimitExceeded(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:reports:limit-exceeded"),
                "Cannot generate the report.")
            .WithMetadata("limit", 100)
            .WithMetadata("internalPolicyId", "P-42");

// Removes the internal value before the error is returned to the client.
var publicError = error.WithoutMetadata("internalPolicyId");
```

## See Also
* [Metadata](Error_Metadata.md)
* [WithMetadata(String, Object)](Error_WithMetadata.md)
* [TryGetMetadata&lt;T&gt;(String, T)](Error_TryGetMetadata.md)
