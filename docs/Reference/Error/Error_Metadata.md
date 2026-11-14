# Error.Metadata Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Error.cs#L210" target="_blank">Error.cs</a>

Gets the additional, machine-readable members of the problem.

```csharp
public IReadOnlyDictionary<string, object?> Metadata { get; }
```

### Property Value
[IReadOnlyDictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlydictionary-2)&lt;[String](https://learn.microsoft.com/dotnet/api/system.string), [Object](https://learn.microsoft.com/dotnet/api/system.object)&gt;

The metadata entries of the error. Never `null`; empty by default.

## Remarks
This property corresponds to the *extension members* of [RFC 9457](https://datatracker.ietf.org/doc/html/rfc9457#section-3.2) (section 3.2). It holds response-level values that consumers can read directly instead of parsing [Detail](Error_Detail.md) (which, according to section 3.1.4, they should not do), for example a retry delay or the value of an exhausted limit.

It is different from:
* [ErrorDetails](Error_ErrorDetails.md) — the RFC 9457 `errors` array, with one entry per invalid input property,
* [DetailTemplated](Error_DetailTemplated.md) — the parameters used only to render a localized message.

The library does **not** define any well-known keys; they are defined by the packages or applications that use them.

### Keys
* Keys are compared ordinally (case-sensitive).
* **Reserved (enforced):** keys must not match the standard problem details members `type`, `title`, `status`, `detail`, `instance`, `errors` and `detailTemplated`. The check **ignores case** (e.g., `Title` is also rejected), because clients often read JSON case-insensitively and serializer naming policies can change the case of a key. [WithMetadata(String, Object)](Error_WithMetadata.md) and [ErrorBuilder.WithMetadata](../ErrorBuilder/ErrorBuilder_WithMetadata.md) throw an [ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception) for such a key, so no `Error` instance can hold one.
* **Recommended (not enforced):** keys should start with a letter, consist of ASCII letters, digits and `_`, and be at least 3 characters long ([RFC 9457, section 4](https://datatracker.ietf.org/doc/html/rfc9457#section-4)). The RFC states these as recommendations (*SHOULD*), and many APIs use other names (e.g., `id` or `trace-id`), so they are not checked.

### Values
Values can be `null`. Use [TryGetMetadata&lt;T&gt;(String, T)](Error_TryGetMetadata.md) to read a value as a specific type.

### Changing the metadata
The dictionary is read-only and cannot be modified, also through a cast (modifying methods of `IDictionary<string, object?>` throw a [NotSupportedException](https://learn.microsoft.com/dotnet/api/system.notsupportedexception)). To change it:
* use [WithMetadata(String, Object)](Error_WithMetadata.md), [WithoutMetadata(String)](Error_WithoutMetadata.md#withoutmetadatastring) or [WithoutMetadata()](Error_WithoutMetadata.md#withoutmetadata), which return a copy of the error, or
* use an [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md) to change several entries at once.

The order of the entries is not defined.

> [!NOTE]
> This dictionary participates in the value-based equality of [Error](Error.md): it is compared **by content**, independent of the order of the entries. See [Equals(Error)](Error_Equals.md).

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
* [WithoutMetadata](Error_WithoutMetadata.md)
* [TryGetMetadata&lt;T&gt;(String, T)](Error_TryGetMetadata.md)
* [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md)
* [RFC 9457: Problem Details for HTTP APIs — Extension Members](https://datatracker.ietf.org/doc/html/rfc9457#section-3.2)
