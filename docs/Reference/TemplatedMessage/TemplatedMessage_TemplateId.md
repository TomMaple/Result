# TemplatedMessage.TemplateId Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/TemplatedMessage.cs#L26" target="_blank">TemplatedMessage.cs</a>

Gets or sets a template identifier to be used when rendering the templated message.

```csharp
public string TemplateId { get; init; }
```

### Property Value
[string](https://learn.microsoft.com/dotnet/api/system.string)

A template identifier to be used when rendering the templated message.

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
This is a mandatory property.

> [!CAUTION]
> This value should not be assigned via constructor but through the factory methods of the [Error](../Error/Error.md) record (such as [NotFound(ErrorUri, String, String, ErrorUri, String, (String, Object)[])](Error_NotFound.md#notfounderroruri-string-string-erroruri-string-string-object) or [Failure(ErrorUri, String, String, ErrorUri, String, IEnumerable<KeyValuePair<String, Object>>)](Error_Failure.md#failureerroruri-string-string-erroruri-string-ienumerablekeyvaluepairstring-object)) or an object instance method like [AddDetail(String, String, String, (String, Object)[])](Error_AddDetail.md) which use this record internally.
> 
> For deserialization, the setter is marked as `init` to allow assignment during object initialization.

## See Also
* [Error](../Error/Error.md)
* [ErrorDetail](../ErrorDetail/ErrorDetail.md)
