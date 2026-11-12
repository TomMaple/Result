# Error.TryGetMetadata&lt;T&gt;(String, T) Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Error.cs#L398" target="_blank">Error.cs</a>

Gets the [Metadata](Error_Metadata.md) value of the specified key, converted to the type `T`.

```csharp
public bool TryGetMetadata<T>(string key, [MaybeNullWhen(false)] out T value);
```

### Type Parameters
#### `T`
The expected type of the value.

### Parameters
#### `key` [String](https://learn.microsoft.com/dotnet/api/system.string)
The key of the metadata entry.

#### `value` T
When this method returns, contains the value if the entry exists and can be converted to `T`; otherwise, the default value of `T`.

### Returns
[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)

`true` if the entry exists and its value is, or can be converted to, `T`; otherwise, `false`.

## Remarks
This method **never throws**. It returns `false` when the key is `null` or absent, when the value is `null`, or when the conversion fails.

Values of type `object` change their runtime type after a JSON round-trip: *System.Text.Json* returns a [JsonElement](https://learn.microsoft.com/dotnet/api/system.text.json.jsonelement), while *Newtonsoft.Json* returns `long`, `double`, `string`, `bool` or a `JToken`. This method hides that difference:

| Stored value | Conversion |
| ------------ | ---------- |
| Already a `T` | Returned as is. |
| [JsonElement](https://learn.microsoft.com/dotnet/api/system.text.json.jsonelement) | Converted to `string`, `bool`, integer and floating-point types, `DateTimeOffset` and `Guid` when the JSON kind matches and the value is in range; otherwise deserialized with `JsonElement.Deserialize<T>()`. |
| [IConvertible](https://learn.microsoft.com/dotnet/api/system.iconvertible) (e.g., `long`, `double`, Newtonsoft.Json `JValue`) | Converted with [Convert.ChangeType](https://learn.microsoft.com/dotnet/api/system.convert.changetype) using the invariant culture. |
| Anything else | Not converted (`false`). |

Nullable value types (e.g., `int?`) are supported.

> [!NOTE]
> A [JsonElement](https://learn.microsoft.com/dotnet/api/system.text.json.jsonelement) number is converted only if it fits the target type exactly (e.g., `3000000000` is not converted to `int`, and `1.5` is not converted to `int`). [Convert.ChangeType](https://learn.microsoft.com/dotnet/api/system.convert.changetype) rounds a floating-point value converted to an integer type.

## Examples
```csharp
var json = JsonSerializer.Serialize(
    Error.LimitExceeded(
            ErrorUri.Tag("tag:exampleapp.com,2026:errors:reports:limit-exceeded"),
            "Cannot generate the report.")
        .WithMetadata("limit", 100));

var error = JsonSerializer.Deserialize<Error>(json)!;

// error.Metadata["limit"] is a JsonElement here.
if (error.TryGetMetadata<int>("limit", out var limit))
{
    Console.WriteLine($"The limit is {limit}.");
}
```

## See Also
* [Metadata](Error_Metadata.md)
* [WithMetadata(String, Object)](Error_WithMetadata.md)
* [WithoutMetadata(String)](Error_WithoutMetadata.md)
