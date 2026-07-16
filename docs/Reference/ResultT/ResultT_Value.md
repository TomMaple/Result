# Result&lt;T&gt;.Value Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Result.cs#L202" target="_blank">Result.cs</a>

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

> [!NOTE]
> The setter is `init`-only and intended for deserialization. A `null` assignment is **ignored** rather than throwing, so that deserializing a failed result (where the value is absent) leaves the value unset instead of overwriting an already-populated member. Assigning a non-null value while an [Error](ResultT_Error.md) is already set throws an [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception), because a result can never hold both a value and an error.

## See Also
* [Error](../Error/Error.md)
