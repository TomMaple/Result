# TemplatedMessage Constructors
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>

Initializes a new instance of the [TemplatedMessage](TemplatedMessage.md) record.

## Overloads
| Name | Description |
| ---- | ----------- |
| [TemplatedMessage(String, IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)](#templatedmessagestring-ienumerablekeyvaluepairstring-object) | Initializes a new instance with the parameters given as key-value pairs (e.g., a dictionary). |
| [TemplatedMessage(String, ReadOnlySpan&lt;(String, Object)&gt;)](#templatedmessagestring-readonlyspanstring-object) | Initializes a new instance with the parameters given as name-value tuples. |

## Remarks
Both constructors copy the parameters into one read-only dictionary with an ordinal (case-sensitive) comparer, so later changes to the source are not reflected. Parameter values can be `null`; parameter names must be unique and not `null`.

> [!NOTE]
> An instance is usually created by the factory methods of the [Error](../Error/Error.md) record (the `detailTemplateId` and `detailNamedValues` parameters, e.g., [NotFound](../Error/Error_NotFound.md)), by [Error.WithErrorDetail](../Error/Error_WithErrorDetail.md), or by the [ErrorBuilder](../ErrorBuilder/ErrorBuilder.md) methods [WithDetailTemplated](../ErrorBuilder/ErrorBuilder_WithDetailTemplated.md) and [AddErrorDetail](../ErrorBuilder/ErrorBuilder_AddErrorDetail.md).


## TemplatedMessage(String, IEnumerable&lt;KeyValuePair&lt;String, Object&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/TemplatedMessage.cs#L56" target="_blank">TemplatedMessage.cs</a>

Initializes a new instance of the [TemplatedMessage](TemplatedMessage.md) record with the parameters given as key-value pairs (e.g., a dictionary).

```csharp
[OverloadResolutionPriority(1)]
public TemplatedMessage(string templateId, IEnumerable<KeyValuePair<string, object?>>? parameters = null);
```

### Parameters
#### `templateId` [String](https://learn.microsoft.com/dotnet/api/system.string)
A mandatory identifier of the message template.

#### `parameters` [IEnumerable](https://learn.microsoft.com/dotnet/api/system.collections.generic.ienumerable-1)&lt;[KeyValuePair](https://learn.microsoft.com/dotnet/api/system.collections.generic.keyvaluepair-2)&lt;[String](https://learn.microsoft.com/dotnet/api/system.string), [Object](https://learn.microsoft.com/dotnet/api/system.object)&gt;&gt;
The optional parameters (names and values); values can be `null`. When `null`, [Params](TemplatedMessage_Params.md) is `null`.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `templateId` or a parameter name is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `templateId` is empty or consists only of white-space characters, or a parameter name is repeated.

### Remarks
`null` and an empty collection expression (`[]`) as the second argument bind to this constructor.

### Examples
```csharp
var message = new TemplatedMessage(
    "errors.account.notFound",
    new Dictionary<string, object?> { ["accountId"] = "D12345" });
```


## TemplatedMessage(String, ReadOnlySpan&lt;(String, Object)&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/TemplatedMessage.cs#L76" target="_blank">TemplatedMessage.cs</a>

Initializes a new instance of the [TemplatedMessage](TemplatedMessage.md) record with the parameters given as name-value tuples.

```csharp
public TemplatedMessage(string templateId, params ReadOnlySpan<(string Key, object? Value)> parameters);
```

### Parameters
#### `templateId` [String](https://learn.microsoft.com/dotnet/api/system.string)
A mandatory identifier of the message template.

#### `parameters` [ReadOnlySpan](https://learn.microsoft.com/dotnet/api/system.readonlyspan-1)&lt;([String](https://learn.microsoft.com/dotnet/api/system.string), [Object](https://learn.microsoft.com/dotnet/api/system.object))&gt;
The parameters (names and values), given inline, as an array or as a collection expression; values can be `null`. When none are passed, [Params](TemplatedMessage_Params.md) is an empty dictionary.

### Exceptions
[ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)

The `templateId` or a parameter name is `null`.

[ArgumentException](https://learn.microsoft.com/dotnet/api/system.argumentexception)

The `templateId` is empty or consists only of white-space characters, or a parameter name is repeated.

### Examples
```csharp
var message = new TemplatedMessage("errors.username.length", ("min", 3), ("max", 20));
```

## See also
* [TemplatedMessage](TemplatedMessage.md)
* [Error](../Error/Error.md)
* [ErrorDetail](../ErrorDetail/ErrorDetail.md)
