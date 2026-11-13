# Error.TryGetMetadata&lt;T&gt;(String, T) Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Error.cs#L452" target="_blank">Error.cs</a>

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

Values of type `object` change their runtime type after a JSON round-trip: *System.Text.Json* returns a [JsonElement](https://learn.microsoft.com/dotnet/api/system.text.json.jsonelement), while *Newtonsoft.Json* returns `long`, `double`, `string`, `bool`, `DateTime` (for ISO 8601 date strings, by default) or a `JToken`. This method hides that difference: every value goes through a **single conversion path**, so the result depends only on the value, not on whether the error was created in memory or deserialized with either serializer.

1. A value that already is a `T` is returned as is.
2. Any other value that is not a [JsonElement](https://learn.microsoft.com/dotnet/api/system.text.json.jsonelement) is first written as a JSON primitive, exactly as *System.Text.Json* would serialize it: strings, `char`, `DateTime`, `DateTimeOffset` and `Guid` as JSON strings, numbers and enums as JSON numbers, and `bool` as a JSON boolean. Other values (e.g., arbitrary objects) are not converted (`false`).
3. The [JsonElement](https://learn.microsoft.com/dotnet/api/system.text.json.jsonelement) is converted to `T` with these strict rules:

| Target type `T` | Accepted JSON value |
| --------------- | ------------------- |
| `string` | A string. |
| `DateTime`, `DateTimeOffset` | A string in the ISO 8601-1 extended format (e.g., `"2026-12-01T00:00:00Z"`). |
| `Guid` | A string in a standard `Guid` format. |
| `bool` | `true` or `false`. |
| Integer and floating-point types, `decimal` | A number that fits the target type exactly. |
| Enums | A number that fits the underlying type of the enum. |
| Anything else | Deserialized with `JsonElement.Deserialize<T>()`. |

Nullable value types (e.g., `int?`) are supported.

> [!NOTE]
> Values are never converted between JSON kinds: a number is not read as `string`, the string `"true"` is not read as `bool`, and the string `"100"` is not read as `int`. A number is converted only if it fits the target type exactly (e.g., `3000000000` is not converted to `int`, and `1.5` is not converted to `int`).

> [!NOTE]
> By default, *Newtonsoft.Json* converts an ISO 8601 date string with an offset other than `Z` to a local `DateTime`. Such a value is read as the same instant, but as a `string` it is formatted with the local offset of the machine (e.g., `"2026-12-01T10:30:00+02:00"` may become `"2026-12-01T09:30:00+01:00"`). Set `DateParseHandling.None` in the `JsonSerializerSettings` to keep date strings unchanged.

> [!NOTE]
> **Native AOT and trimming:** the fallback `JsonElement.Deserialize<T>()` (used for the types not listed above, e.g., arrays or objects) relies on reflection-based serialization. It is not available in Native AOT and trimmed applications (when `JsonSerializerIsReflectionEnabledByDefault` is `false`), so this method returns `false` for such types there. The conversions of the types listed above do not use reflection and work everywhere.

> [!TIP]
> To get primitive values instead of [JsonElement](https://learn.microsoft.com/dotnet/api/system.text.json.jsonelement) instances from *System.Text.Json*, register the [ObjectAsPrimitiveConverter](https://www.nuget.org/packages/Maple.Json.ObjectAsPrimitiveConverter) in the `JsonSerializerOptions` used to deserialize the error. See [Metadata](Error_Metadata.md#values).

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
