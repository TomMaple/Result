# ErrorBuilder.WithMetadata Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>

Adds or replaces entries of the [Metadata](../Error/Error_Metadata.md) of the error to build.

## Overloads
| Name | Description |
| ---- | ----------- |
| [WithMetadata(String, Object)](#withmetadatastring-object) | Adds or replaces a metadata entry. |
| [WithMetadata(IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)](#withmetadataienumerablekeyvaluepairstring-object) | Adds or replaces metadata entries. |

## Remarks
Keys are compared ordinally (case-sensitive); a repeated key replaces the value. Values can be `null`. See [Metadata](../Error/Error_Metadata.md#keys) for the rules for keys.


## WithMetadata(String, Object)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorBuilder.cs#L382" target="_blank">ErrorBuilder.cs</a>

Adds the specified metadata entry, or replaces its value if the key already exists.

```csharp
public ErrorBuilder WithMetadata(string key, object? value);
```

### Parameters
#### `key` [String](https://learn.microsoft.com/dotnet/api/system.string)
A mandatory key of the metadata entry.

#### `value` [Object](https://learn.microsoft.com/dotnet/api/system.object)
The value of the metadata entry; it can be `null`.

### Returns
[ErrorBuilder](ErrorBuilder.md)

The current instance of the [ErrorBuilder](ErrorBuilder.md) class.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `key` is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `key` is empty, consists only of white-space characters, or is [reserved](../Error/Error_Metadata.md#keys).

### Examples
```csharp
var error = ErrorBuilder.LimitExceeded(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:reports:limit-exceeded"),
                "Cannot generate the report.")
            .WithMetadata("limit", 100)
            .WithMetadata("remaining", 0)
            .Build();
```


## WithMetadata(IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorBuilder.cs#L407" target="_blank">ErrorBuilder.cs</a>

Adds the specified metadata entries. Entries with existing keys are replaced; the other existing entries are kept.

```csharp
public ErrorBuilder WithMetadata(IEnumerable<KeyValuePair<string, object?>> metadata);
```

### Parameters
#### `metadata` [IEnumerable](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable-1)&lt;[KeyValuePair](https://learn.microsoft.com/dotnet/api/system.collections.generic.keyvaluepair-2)&lt;[String](https://learn.microsoft.com/dotnet/api/system.string), [Object](https://learn.microsoft.com/dotnet/api/system.object)&gt;&gt;
A mandatory sequence of the metadata entries to add or replace; values can be `null`.

### Returns
[ErrorBuilder](ErrorBuilder.md)

The current instance of the [ErrorBuilder](ErrorBuilder.md) class.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `metadata` sequence or any of its keys is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

Any key is empty, consists only of white-space characters, or is [reserved](../Error/Error_Metadata.md#keys).

### Remarks
All keys are validated before any entry is applied, so the builder is unchanged when this method throws. The sequence is enumerated once.

### Examples
```csharp
var error = ErrorBuilder.LimitExceeded(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:reports:limit-exceeded"),
                "Cannot generate the report.")
            .WithMetadata(new Dictionary<string, object?> { ["limit"] = 100, ["remaining"] = 0 })
            .Build();
```

## See Also
* [ErrorBuilder](ErrorBuilder.md)
* [WithoutMetadata](ErrorBuilder_WithoutMetadata.md)
* [Error.Metadata](../Error/Error_Metadata.md)
