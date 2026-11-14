# Error.DetailTemplated Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Error.cs#L140" target="_blank">Error.cs</a>

Gets a template to generate a localized, human-readable explanation specific to this occurrence of the problem.

```csharp
public TemplatedMessage? DetailTemplated { get; }
```

### Property Value
[TemplatedMessage](../TemplatedMessage/TemplatedMessage.md)

A template to generate a localized, human-readable explanation specific to this occurrence of the problem.

## Remarks
This is an optional property.

It contains the ID of the message template and an optional map of parameters to be used when rendering the template.

It allows clients to localize the error detail message based on the user’s locale and format it with relevant parameters.

It should contain equivalent information to the [Detail](Error_Detail.md) property but in a structured format suitable for localization and dynamic content generation.

> [!NOTE]
> [Error](Error.md) is immutable, so this property is set when the error is created: by the factory methods (e.g., [Validation](Error_Validation.md)) or an [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md). To change it, use [ToBuilder()](Error_ToBuilder.md).

## Examples
```csharp
var error = Error.NotFound(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:not-found"),
                "The user has not been found.",
                "The user with ID ‘12345’ was not found or has been deleted.",
                // sets the DetailTemplated property
                detailTemplateId: "error:user:notfound",
                detailNamedValues: ("userId", "12345"));
```

## See Also
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)
* [Problem Details for HTTP APIs - RFC 7807 is dead, long live RFC 9457](https://blog.frankel.ch/problem-details-http-apis/)
