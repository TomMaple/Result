# ErrorBuilder.WithoutMetadata Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>

Removes some or all entries of the [Metadata](../Error/Error_Metadata.md) of the error to build.

## Overloads
| Name | Description |
| ---- | ----------- |
| [WithoutMetadata()](#withoutmetadata) | Removes all metadata entries. |
| [WithoutMetadata(String)](#withoutmetadatastring) | Removes the metadata entry of the specified key. |


## WithoutMetadata()
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorBuilder.cs#L480" target="_blank">ErrorBuilder.cs</a>

Removes all entries from the metadata.

```csharp
public ErrorBuilder WithoutMetadata();
```

### Returns
[ErrorBuilder](ErrorBuilder.md)

The current instance of the [ErrorBuilder](ErrorBuilder.md) class.

### Examples
```csharp
// Keeps only the public values.
var publicError = error.ToBuilder()
    .WithoutMetadata()
    .WithMetadata("retryAfter", 30)
    .Build();
```


## WithoutMetadata(String)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorBuilder.cs#L465" target="_blank">ErrorBuilder.cs</a>

Removes the entry of the specified key from the metadata.

```csharp
public ErrorBuilder WithoutMetadata(string key);
```

### Parameters
#### `key` [String](https://learn.microsoft.com/dotnet/api/system.string)
A mandatory key of the metadata entry to remove. An absent key is not an error.

### Returns
[ErrorBuilder](ErrorBuilder.md)

The current instance of the [ErrorBuilder](ErrorBuilder.md) class.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `key` is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `key` is empty or consists only of white-space characters.

### Examples
```csharp
var publicError = error.ToBuilder()
    .WithoutMetadata("internalPolicyId")
    .Build();
```

## See Also
* [ErrorBuilder](ErrorBuilder.md)
* [WithMetadata](ErrorBuilder_WithMetadata.md)
* [Error.Metadata](../Error/Error_Metadata.md)
