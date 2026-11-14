# Error.TryGetMetadata&lt;T&gt;(String, T) Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Error.cs#L294" target="_blank">Error.cs</a>

Gets the [Metadata](Error_Metadata.md) value of the specified key if it is of the type `T`.

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
When this method returns, contains the value if the entry exists and is a `T`; otherwise, the default value of `T`.

### Returns
[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)

`true` if the entry exists and its value is a `T`; otherwise, `false`.

## Remarks
This method **never throws**. It returns `false` when the key is `null` or absent, when the value is `null`, or when the value is not a `T`.

The match is **strict**: it is the C# `value is T` test, and no conversion is performed.
* A value is returned when its runtime type is `T`, derives from `T`, or implements `T` (e.g., a `string` can be read as `string`, `object` or `IComparable`).
* A stored `int` can be read as `int` or `int?`, but **not** as `long`, `double` or an enum; a stored enum value is not read as `int`; the string `"100"` is not read as `int`.
* A `null` value never matches, also for a nullable `T` (e.g., `string?` or `int?`); check `Metadata.ContainsKey(key)` to tell a `null` value apart from a missing key.

Read a value with the same type it was stored with.

## Examples
```csharp
var error = Error.LimitExceeded(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:reports:limit-exceeded"),
                "Cannot generate the report.")
            .WithMetadata("limit", 100);

if (error.TryGetMetadata<int>("limit", out var limit))
{
    Console.WriteLine($"The limit is {limit}.");
}

error.TryGetMetadata<long>("limit", out _); // false: the value is an int, not a long
```

## See Also
* [Metadata](Error_Metadata.md)
* [WithMetadata(String, Object)](Error_WithMetadata.md)
* [WithoutMetadata](Error_WithoutMetadata.md)
