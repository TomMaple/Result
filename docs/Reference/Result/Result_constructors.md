# Result Constructor
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Result.cs#L66" target="_blank">Result.cs</a>

Initializes a new instance of the [Result](Result.md) record that indicates a successful operation that does not return a value.

```csharp
public Result();
```

## Remarks
> [!CAUTION]
> The constructor is not to be used directly—it is required for deserialization.
> 
> Instead, use one of the static methods such as [Success()](Result_Success.md) or [FromError(Error)](Result_FromError.md), or the [Implicit(Error to Result)](Result_implicit_Error_to_Result.md) operator to create an instance of the [Result](Result.md) record with appropriate properties.

## See also
* [Error](../Error/Error.md)
