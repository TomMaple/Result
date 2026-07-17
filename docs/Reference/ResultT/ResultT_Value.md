# Result&lt;T&gt;.Value Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Result.cs#L210" target="_blank">Result.cs</a>

Gets or sets the value associated with a successful operation.

```csharp
public T? Value { get; init; }
```

### Property Value
T

The value if the result represents a success; otherwise, the default value of `T`—`null` for a reference type, or `default(T)` for a value type (for example, `0` for a `Result<int>`). A `null` does not by itself indicate failure: a successful result of a nullable `T` (a nullable reference type or `Nullable<T>`) may itself hold `null`, while a failed `Result<int>` reports `0` rather than `null`. Use [IsSuccess()](ResultT_IsSuccess.md) to tell success from failure rather than checking this property against `null`.

## Remarks
> [!CAUTION]
> Always call the [IsSuccess()](ResultT_IsSuccess.md) method and confirm it returns `true` before reading the [Value](ResultT_Value.md) property. On a failed result, [Value](ResultT_Value.md) holds the default value of `T`, which for a value type is not `null` (for example, `0` for a `Result<int>`), so a `null` check alone cannot tell success from failure.


> [!NOTE]
> The setter is `init`-only and intended for deserialization. A successful result of a nullable `T` may carry a `null` value, so assigning `null` records a **present** (null) value rather than being ignored. Assigning a value while an [Error](ResultT_Error.md) is already set throws an [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception), because a result can never hold both a value and an error.

> [!NOTE]
> Whether a value has been assigned is tracked separately from the value itself, so that an absent value is told apart both from a value type's `default` (for example, `0` for a `Result<int>`) and from a nullable `T`'s successful `null`. A `Result<int>` created by the [parameterless constructor](ResultT_constructors.md) and one created by `Result.FromValue(0)` both report `0` from this property; only the latter is a successful result, and [IsSuccess()](ResultT_IsSuccess.md) tells them apart.

## See Also
* [Error](../Error/Error.md)
