# Error Constructor
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Error.cs#L43" target="_blank">Error.cs</a>

Initializes a new instance of the [Error](Error.md) record of the Critical type (if not specified otherwise).

```csharp
public Error();
```

## Remarks
> [!CAUTION]
> Do not use this constructor directly, as it does NOT validate the values, which may also cause unwanted side effects.
>
> This constructor is required for deserialization.
> 
> Instead, use one of the static methods such as [Conflict(ErrorUri, String, String, ErrorUri, String, (String, Object)[])](Error_Conflict.md), [Failure(ErrorUri, String, String, ErrorUri, String, (String, Object)[])](Error_Failure.md), [NotFound(ErrorUri, String, String, ErrorUri, String, (String, Object)[])](Error_NotFound.md), [Unauthorized(ErrorUri, String, String, ErrorUri, String, (String, Object)[])](Error_Unauthorized.md) or others to create an instance of the [Error](Error.md) record with appropriate properties.

## See also
* [Result](../Result/Result.md)