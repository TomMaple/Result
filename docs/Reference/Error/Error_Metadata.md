# Error.Metadata Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Error.cs#L281" target="_blank">Error.cs</a>

Gets or sets the additional, machine-readable members of the problem.

```csharp
public IReadOnlyDictionary<string, object> Metadata { get; init; }
```

### Property Value
[IReadOnlyDictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlydictionary-2)&lt;[String](https://learn.microsoft.com/dotnet/api/system.string), [Object](https://learn.microsoft.com/dotnet/api/system.object)&gt;

The metadata entries of the error. Never `null`; empty by default.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

When set: any key or any value is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

When set: any key is empty, consists only of white-space characters, or is [reserved](#keys).

## Remarks
This property corresponds to the *extension members* of [RFC 9457](https://datatracker.ietf.org/doc/html/rfc9457#section-3.2) (section 3.2). It holds response-level values that consumers can read directly instead of parsing [Detail](Error_Detail.md) (which, according to section 3.1.4, they should not do), for example a retry delay or the value of an exhausted limit.

It is different from:
* [ErrorDetails](Error_ErrorDetails.md) — the RFC 9457 `errors` array, with one entry per invalid input property,
* [DetailTemplated](Error_DetailTemplated.md) — the parameters used only to render a localized message.

The library does **not** define any well-known keys; they are defined by the packages or applications that use them.

### Keys
* Keys are compared ordinally (case-sensitive).
* **Reserved (enforced):** keys must not match the standard problem details members `type`, `title`, `status`, `detail`, `instance`, `errors` and `detailTemplated`. The check **ignores case** (e.g., `Title` is also rejected), because clients often read JSON case-insensitively and serializer naming policies can change the case of a key. [WithMetadata(String, Object)](Error_WithMetadata.md) and the `init` setter (also during deserialization) throw an [ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception) for such a key, so no `Error` instance can hold one.
* **Recommended (not enforced):** keys should start with a letter, consist of ASCII letters, digits and `_`, and be at least 3 characters long ([RFC 9457, section 4](https://datatracker.ietf.org/doc/html/rfc9457#section-4)). The RFC states these as recommendations (*SHOULD*), and many APIs use other names (e.g., `id` or `trace-id`), so they are not checked.
* Deserializing a `Metadata` object with a reserved key fails: *System.Text.Json* throws the [ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception), and *Newtonsoft.Json* wraps it in a `JsonSerializationException`.

### Values
Values must not be `null`: [WithMetadata(String, Object)](Error_WithMetadata.md) and the `init` setter (also during deserialization, e.g., `"Metadata": {"limit": null}`) throw an [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception) for a `null` value (wrapped in a `JsonSerializationException` by *Newtonsoft.Json*).

Values should be JSON primitives (strings, numbers and booleans).

After deserialization, values of type `object` are returned as:
* [JsonElement](https://learn.microsoft.com/dotnet/api/system.text.json.jsonelement) by *System.Text.Json*,
* `long`, `double`, `string`, `bool`, `DateTime` (for ISO 8601 date strings, by default) or a `JToken` by *Newtonsoft.Json*.

Use [TryGetMetadata&lt;T&gt;(String, T)](Error_TryGetMetadata.md) to read a value as a specific type; it gives the same result for errors created in memory and for errors deserialized with either serializer.

> [!NOTE]
> This dictionary participates in the value-based equality of [Error](Error.md): it is compared **by content**, independent of the order of the entries. See [Equals(Error)](Error_Equals.md).
>
> [JsonElement](https://learn.microsoft.com/dotnet/api/system.text.json.jsonelement) values have no value equality, so an error may be *not equal* to its original after a JSON round-trip (the same limitation applies to [TemplatedMessage](../TemplatedMessage/TemplatedMessage.md) parameters).

> [!TIP]
> With *System.Text.Json*, the optional [Maple.Json.ObjectAsPrimitiveConverter](https://www.nuget.org/packages/Maple.Json.ObjectAsPrimitiveConverter) package makes values of type `object` deserialize as primitives (`int`, `long`, `decimal`, `string`, `bool`, …) instead of [JsonElement](https://learn.microsoft.com/dotnet/api/system.text.json.jsonelement). Values then compare by value, so an error with integer, string and boolean values is equal to its original after a round-trip. It also applies to [TemplatedMessage](../TemplatedMessage/TemplatedMessage.md) parameters.
>
> ```csharp
> var options = new JsonSerializerOptions();
> options.Converters.Add(new ObjectAsPrimitiveConverter());
>
> var error = JsonSerializer.Deserialize<Error>(json, options);
> ```
>
> Floating-point numbers are deserialized as `decimal` by default; a `double` value is therefore not equal to its original unless the converter is created with `FloatFormat.Double`. Arrays and objects are deserialized as `object?[]` and dictionaries, which compare by reference.

> [!CAUTION]
> Use [WithMetadata(String, Object)](Error_WithMetadata.md) and [WithoutMetadata(String)](Error_WithoutMetadata.md) to obtain a copy of the error with changed metadata. They never modify the current instance.
>
> The setter is marked as `init` to allow deserialization. It copies the provided entries (so later changes to the source dictionary are not reflected), validates them as [WithMetadata(String, Object)](Error_WithMetadata.md) does, and treats `null` (e.g., `"Metadata": null` in JSON) as an empty dictionary.
>
> The returned dictionary is a read-only wrapper that is never modified, so copies of an error (e.g., created by a `with` expression) share it safely. Casting it to `IDictionary<string, object>` does not allow changes: modifying methods throw a [NotSupportedException](https://learn.microsoft.com/dotnet/api/system.notsupportedexception).

### Order
Entries are enumerated, and serialized, in the order they were added. Replacing the value of an existing key keeps its position.

## Examples
```csharp
var error = Error.LimitExceeded(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:reports:limit-exceeded"),
                "Cannot generate the report.",
                "The monthly limit of 100 reports has been reached.")
            .WithMetadata("limit", 100)
            .WithMetadata("resetAt", "2026-12-01T00:00:00Z");

foreach (var (key, value) in error.Metadata)
{
    Console.WriteLine($"{key}: {value}");
}
```

## See Also
* [WithMetadata(String, Object)](Error_WithMetadata.md)
* [WithoutMetadata(String)](Error_WithoutMetadata.md)
* [TryGetMetadata&lt;T&gt;(String, T)](Error_TryGetMetadata.md)
* [RFC 9457: Problem Details for HTTP APIs — Extension Members](https://datatracker.ietf.org/doc/html/rfc9457#section-3.2)
