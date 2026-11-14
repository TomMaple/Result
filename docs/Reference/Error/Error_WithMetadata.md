# Error.WithMetadata Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>

Returns a copy of the [Error](Error.md) with the specified [Metadata](Error_Metadata.md) entries added or replaced.

## Overloads
| Name | Description |
| ---- | ----------- |
| [WithMetadata(String, Object)](#withmetadatastring-object) | Returns a copy of the error with the specified metadata entry added or replaced. |
| [WithMetadata(IEnumerable<KeyValuePair<String, Object>>)](#withmetadataienumerablekeyvaluepairstring-object) | Returns a copy of the error with the specified metadata entries added or replaced. |

## Remarks
These methods **do not modify** the current instance; they return a new [Error](Error.md) with a new, read-only [Metadata](Error_Metadata.md) dictionary. To change several entries without copying the error for each one, use an [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md) (see [ErrorBuilder.WithMetadata](../ErrorBuilder/ErrorBuilder_WithMetadata.md)).

Keys are compared ordinally (case-sensitive); values can be `null`. See [Metadata](Error_Metadata.md) for the rules for keys.


## WithMetadata(String, Object)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Error.cs#L337" target="_blank">Error.cs</a>

Returns a copy of the current error with the specified metadata entry added, or replaced if the key already exists.

```csharp
public Error WithMetadata(string key, object? value);
```

### Parameters
#### `key` [String](https://learn.microsoft.com/dotnet/api/system.string)
A mandatory key of the metadata entry.

#### `value` [Object](https://learn.microsoft.com/dotnet/api/system.object)
The value of the metadata entry; it can be `null`.

### Returns
[Error](Error.md)

A new instance of the [Error](Error.md) record with the entry added or replaced. The current instance is not modified.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `key` is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `key` is empty, consists only of white-space characters, or is [reserved](Error_Metadata.md#keys).

### Examples
```csharp
var error = Error.LimitExceeded(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:reports:limit-exceeded"),
                "Cannot generate the report.")
            .WithMetadata("limit", 100)
            .WithMetadata("remaining", 0);
```


## WithMetadata(IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Error.cs#L358" target="_blank">Error.cs</a>

Returns a copy of the current error with the specified metadata entries added. Entries with existing keys are replaced; the other existing entries are kept.

```csharp
public Error WithMetadata(IEnumerable<KeyValuePair<string, object?>> metadata);
```

### Parameters
#### `metadata` [IEnumerable](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable-1)&lt;[KeyValuePair](https://learn.microsoft.com/dotnet/api/system.collections.generic.keyvaluepair-2)&lt;[String](https://learn.microsoft.com/dotnet/api/system.string), [Object](https://learn.microsoft.com/dotnet/api/system.object)&gt;&gt;
A mandatory sequence of the metadata entries to add or replace; values can be `null`.

### Returns
[Error](Error.md)

A new instance of the [Error](Error.md) record with the entries added or replaced. The current instance is not modified.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `metadata` sequence or any of its keys is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

Any key is empty, consists only of white-space characters, or is [reserved](Error_Metadata.md#keys).

### Examples
```csharp
var error = Error.LimitExceeded(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:reports:limit-exceeded"),
                "Cannot generate the report.")
            .WithMetadata(new Dictionary<string, object?> { ["limit"] = 100, ["remaining"] = 0 });
```

## See Also
* [Metadata](Error_Metadata.md)
* [WithoutMetadata](Error_WithoutMetadata.md)
* [TryGetMetadata&lt;T&gt;(String, T)](Error_TryGetMetadata.md)
