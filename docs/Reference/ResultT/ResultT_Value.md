# Result&lt;T&gt;.Value Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Result.cs#L192" target="_blank">Result.cs</a>

Gets or sets the value associated with a successful operation.

```csharp
public T? Value { get; init; }
```

### Property Value
T

The value if the result represents a success; otherwise, `null`.

## Remarks
> [!CAUTION]
> Use the [IsSuccess()](IResult_IsSuccess.md) method to check whether the result represents a success or a failure before accessing this property.

## See Also
* [Error](../Error/Error.md)
