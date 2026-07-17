# TemplatedMessage record
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/TemplatedMessage.cs" target="_blank">TemplatedMessage.cs</a>

Provides a set of values to generate a message based on the template for the purpose of [ErrorDetail](../ErrorDetail/ErrorDetail.md) type used in the [Error](../Error/Error.md).

```csharp
public sealed record TemplatedMessage
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → [ValueType](https://learn.microsoft.com/dotnet/api/system.valuetype) → [Record](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/record) → ErrorDetail


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
> [!CAUTION]
> The [TemplatedMessage](TemplatedMessage.md) record is primarily used internally by the [Error](../Error/Error.md) and [ErrorDetail](../ErrorDetail/ErrorDetail.md) records for storing the data required for generating **localized** error messages. It is not typically instantiated directly in the code.


## Constructors
| Name | Description |
| ---- | ----------- |
| [TemplatedMessage(String, IReadOnlyDictionary&lt;String, Object&gt;)](TemplatedMessage_constructors.md) | Initializes a new instance of the [TemplatedMessage](TemplatedMessage.md) record. |


## Properties
| Name | Description |
| ---- | ----------- |
| [Params](TemplatedMessage_Params.md) | Gets a read-only dictionary of named values to be used when rendering the templated message. |
| [TemplateId](TemplatedMessage_TemplateId.md) | Gets the template identifier to be used to generate an error message. |


## See Also
* [Error](../Error/Error.md)
* [ErrorDetail](../ErrorDetail/ErrorDetail.md)
