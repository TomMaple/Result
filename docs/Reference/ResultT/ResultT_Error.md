# Result&lt;T&gt;.Error Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Result.cs#L239" target="_blank">Result.cs</a>

Gets or sets the error that caused the operation to fail.

```csharp
public Error? Error { get; init; }
```

### Property Value
[Error](../Error/Error.md)

The error, if the result represents a failure; otherwise, `null`.

## Remarks
> [!CAUTION]
> Use the [IsSuccess()](ResultT_IsSuccess.md) method to check whether the result represents a success or a failure before accessing this property.

> [!NOTE]
> The setter is `init`-only and intended for deserialization. A `null` assignment is **ignored** rather than throwing, so that deserializing a successful result (where the error is absent) leaves the error unset instead of overwriting an already-populated member. Assigning a non-null error while a [Value](ResultT_Value.md) is already set throws an [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception), because a result can never hold both a value and an error.

## See Also
* [Error](../Error/Error.md)
