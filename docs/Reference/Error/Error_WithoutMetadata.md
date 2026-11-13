# Error.WithoutMetadata Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>

Returns a copy of the [Error](Error.md) without some or all of its [Metadata](Error_Metadata.md) entries.

## Overloads
| Name | Description |
| ---- | ----------- |
| [WithoutMetadata()](#withoutmetadata) | Returns a copy of the error without any metadata. |
| [WithoutMetadata(String)](#withoutmetadatastring) | Returns a copy of the error without the metadata entry of the specified key. |

## Remarks
These methods **do not modify** the current instance; they return a new [Error](Error.md). To remove several entries without copying the error for each one, use an [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md) (see [ErrorBuilder.WithoutMetadata](../ErrorBuilder/ErrorBuilder_WithoutMetadata.md)).


## WithoutMetadata()
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Error.cs#L370" target="_blank">Error.cs</a>

Returns a copy of the current error without any metadata.

```csharp
public Error WithoutMetadata();
```

### Returns
[Error](Error.md)

A new instance of the [Error](Error.md) record with an empty [Metadata](Error_Metadata.md) dictionary. The other members are unchanged.

### Examples
```csharp
// Removes all internal values before the error is returned to the client.
var publicError = error.WithoutMetadata();
```


## WithoutMetadata(String)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Error.cs#L387" target="_blank">Error.cs</a>

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

### Remarks
Keys are compared ordinally (case-sensitive). An absent key is not an error.

### Examples
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
