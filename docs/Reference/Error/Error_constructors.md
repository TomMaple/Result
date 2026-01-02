# Error Constructor
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Error.cs#L30" target="_blank">Error.cs</a>

Initializes a new instance of the [Error](Error.md) record of the Validation type (if not specified otherwise).

```csharp
public Error();
```

## Remarks
> [!NOTE]
> The constructor is not to be used directly—it is required for deserialization.
> 
> Instead, use one of the static methods such as [Conflict(ErrorUri, String, String, ErrorUri, String, (String, Object)[])](Error_Conflict.md), [Failure(ErrorUri, String, String, ErrorUri, String, (String, Object)[])](Error_Failure.md), [NotFound(ErrorUri, String, String, ErrorUri, String, (String, Object)[])](Error_NotFound.md), [Unauthorized(ErrorUri, String, String, ErrorUri, String, (String, Object)[])](Error_Unauthorized.md) or others to create an instance of the [Error](Error.md) class with appropriate properties.
