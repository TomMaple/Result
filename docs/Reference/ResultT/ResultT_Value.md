# Result&lt;T&gt;.Value Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.cs#L176" target="_blank">Result.cs</a>

Gets the value associated with a successful operation.

```csharp
public T? Value { get; }
```

### Property Value
T

The value if the result represents a success; otherwise, the default value of `T`—`null` for a reference type, or `default(T)` for a value type (for example, `0` for a `Result<int>`). A `null` does not by itself indicate failure: a successful result of a nullable `T` (a nullable reference type or `Nullable<T>`) may itself hold `null`, while a failed `Result<int>` reports `0` rather than `null`. Use [IsSuccess()](ResultT_IsSuccess.md) to tell success from failure rather than checking this property against `null`.

## Remarks
> [!CAUTION]
> Always call the [IsSuccess()](ResultT_IsSuccess.md) method and confirm it returns `true` before reading the [Value](ResultT_Value.md) property. On a failed result, [Value](ResultT_Value.md) holds the default value of `T`, which for a value type is not `null` (for example, `0` for a `Result<int>`), so a `null` check alone cannot tell success from failure.


## See Also
* [Error](../Error/Error.md)
