# TemplatedMessage.Params Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/TemplatedMessage.cs#L101" target="_blank">TemplatedMessage.cs</a>

Gets a read-only dictionary of named values to be used when rendering the templated message.

```csharp
public IReadOnlyDictionary<string, object?>? Params { get; }
```

### Property Value
[IReadOnlyDictionary](https://learn.microsoft.com/dotnet/api/system.collections.generic.ireadonlydictionary-2)&lt;[String](https://learn.microsoft.com/dotnet/api/system.string), [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object)&gt;?

A collection of named values to be used when rendering the templated message.

## Examples
```csharp
var error = Error.NotFound(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:account:notfound"),
                "Cannot update the account.",
                "The account with the provided ID has not been found.",
                ErrorUri.Locator("https://api.exampleapp.com/errors/8783927589734857/details"),
                "errors.account.notFound",
                ("accountId", "D12345"), ("action", "account:update"));
```


## Remarks
This is an optional property.

It is a read-only dictionary built from the parameters passed to the [constructor](TemplatedMessage_constructors.md), with keys compared ordinally (case-sensitive). It is `null` when `null` was passed as key-value pairs (or no parameters at all), and empty when an empty collection was passed. Later changes to the source are not reflected, and the dictionary cannot be modified, also through a cast. Values can be `null`.

> [!NOTE]
> An instance is usually created by the factory methods of the [Error](../Error/Error.md) record (the `detailTemplateId` and `detailNamedValues` parameters, e.g., [NotFound](../Error/Error_NotFound.md)), by [Error.WithErrorDetail](../Error/Error_WithErrorDetail.md), or by the [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md) methods [WithDetailTemplated](../ErrorBuilder/ErrorBuilder_WithDetailTemplated.md) and [AddErrorDetail](../ErrorBuilder/ErrorBuilder_AddErrorDetail.md).


## See Also
* [Error](../Error/Error.md)
* [ErrorDetail](../ErrorDetail/ErrorDetail.md)
