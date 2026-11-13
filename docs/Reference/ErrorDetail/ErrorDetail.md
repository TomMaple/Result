# ErrorDetail record
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorDetail.cs" target="_blank">ErrorDetail.cs</a>

Represents an individual error occurrence that contains a problem detail in a [Error](../Error/Error.md) record with optional properties for localization and property pointer.

```csharp
public sealed record ErrorDetail
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → ErrorDetail

## Remarks
The record is immutable: its properties are get-only, and `with` expressions cannot change them. [Detail](ErrorDetail_Detail.md) must not be `null`.


## Examples
```csharp
var error = Error.Validation(ErrorUri.Tag("tag:test.com,2024:SignupErrors-InvalidData"), "Provided data is invalid.")
    .WithErrorDetail("#/email", "Email is invalid", "errors-email-invalid",
        ("required_format", "address@domain.com"), ("min_length", 9));
```

## Constructors
| Name | Description |
| ---- | ----------- |
| [ErrorDetail(String, String, TemplatedMessage)](ErrorDetail_constructors.md) | Initializes a new instance of the [ErrorDetail](ErrorDetail.md) record. |

## Properties
| Name | Description |
| ---- | ----------- |
| [Detail](ErrorDetail_Detail.md) | Gets a human-readable explanation of the specific problem detail. |
| [DetailTemplated](ErrorDetail_DetailTemplated.md) | Gets a templated explanation of the specific problem detail. |
| [PropertyPointer](ErrorDetail_PropertyPointer.md) | Gets a JSON Pointer which identifies invalid value in the input data. |


## See Also
* [Error](../Error/Error.md)
* [TemplatedMessage](../TemplatedMessage/TemplatedMessage.md)
* [RFC 6901: JavaScript Object Notation (JSON) Pointer](https://datatracker.ietf.org/doc/html/rfc6901)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Problem Details for HTTP APIs - RFC 7807 is dead, long live RFC 9457](https://blog.frankel.ch/problem-details-http-apis/)
