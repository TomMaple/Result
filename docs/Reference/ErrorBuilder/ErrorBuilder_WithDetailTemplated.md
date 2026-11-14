# ErrorBuilder.WithDetailTemplated Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>

Sets the [DetailTemplated](../Error/Error_DetailTemplated.md) of the error to build.

## Overloads
| Name | Description |
| ---- | ----------- |
| [WithDetailTemplated(TemplatedMessage)](#withdetailtemplatedtemplatedmessage) | Sets the templated explanation of the problem occurrence. |
| [WithDetailTemplated(String, (String, Object)[])](#withdetailtemplatedstring-string-object) | Sets the templated explanation from a template identifier and named values. |

## Remarks
`WithDetailTemplated(null)` binds to [WithDetailTemplated(TemplatedMessage)](#withdetailtemplatedtemplatedmessage) and clears the value.


## WithDetailTemplated(TemplatedMessage)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorBuilder.cs#L271" target="_blank">ErrorBuilder.cs</a>

Sets the [DetailTemplated](../Error/Error_DetailTemplated.md) of the error to build.

```csharp
[OverloadResolutionPriority(1)]
public ErrorBuilder WithDetailTemplated(TemplatedMessage? detailTemplated);
```

### Parameters
#### `detailTemplated` [TemplatedMessage](../TemplatedMessage/TemplatedMessage.md)
The templated explanation, or `null` to clear it.

### Returns
[ErrorBuilder](ErrorBuilder.md)

The current instance of the [ErrorBuilder](ErrorBuilder.md) class.

### Examples
```csharp
var withoutTemplate = error.ToBuilder()
    .WithDetailTemplated(null)
    .Build();
```


## WithDetailTemplated(String, (String, Object)[])
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorBuilder.cs#L292" target="_blank">ErrorBuilder.cs</a>

Sets the [DetailTemplated](../Error/Error_DetailTemplated.md) of the error to build to a new [TemplatedMessage](../TemplatedMessage/TemplatedMessage.md).

```csharp
public ErrorBuilder WithDetailTemplated(string templateId, params ReadOnlySpan<(string Key, object? Value)> namedValues);
```

### Parameters
#### `templateId` [String](https://learn.microsoft.com/dotnet/api/system.string)
A mandatory identifier of the message template.

#### `namedValues` [ReadOnlySpan](https://learn.microsoft.com/dotnet/api/system.readonlyspan-1)&lt;([String](https://learn.microsoft.com/dotnet/api/system.string), [Object](https://learn.microsoft.com/dotnet/api/system.object))&gt;
Optional named values (inline, an array or a collection expression) to be used when rendering the templated message. Values can be `null`.

### Returns
[ErrorBuilder](ErrorBuilder.md)

The current instance of the [ErrorBuilder](ErrorBuilder.md) class.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `templateId` or a parameter name is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `templateId` is empty or consists only of white-space characters, or a parameter name is repeated.

In both cases the builder is unchanged.

### Examples
```csharp
var error = ErrorBuilder.NotFound(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:account:notfound"),
                "Cannot update the account.")
            .WithDetailTemplated("errors.account.notFound", ("accountId", "D12345"))
            .Build();
```

## See Also
* [ErrorBuilder](ErrorBuilder.md)
* [TemplatedMessage](../TemplatedMessage/TemplatedMessage.md)
* [DetailTemplated](../Error/Error_DetailTemplated.md)
