# Result&lt;T&gt;.IsSuccess() Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.cs#L199" target="_blank">Result.cs</a>

Indicates whether the instance of the [Result&lt;T&gt;](ResultT.md) record represents a successful operation.

```csharp
public bool IsSuccess();
```

### Returns
[Boolean](https://learn.microsoft.com/en-ca/dotnet/api/system.boolean)

`true` if the instance of the [Result&lt;T&gt;](ResultT.md) record represents a successful operation; otherwise, `false`.

## Remarks
This method is annotated with `[MemberNotNullWhen(true, nameof(Value))]` and `[MemberNotNullWhen(false, nameof(Error))]`, so the compiler treats [Value](ResultT_Value.md) as non-null after this method returns `true`, and [Error](ResultT_Error.md) as non-null after it returns `false`. That is what allows `result.Value` to be used without a null check inside a success branch.

A result is successful exactly when its [Error](ResultT_Error.md) is `null`. This method never throws.

> [!NOTE]
> When `T` is nullable (a nullable reference type or `Nullable<T>`), a successful result may carry a `null` value (e.g., `Result<int?>.FromValue(null)`), so the `[MemberNotNullWhen(true, nameof(Value))]` hint is a best effort for those types.

## Examples
```csharp
if (result.IsSuccess())
{
    Console.WriteLine("Operation succeeded.");
}
else
{
    Console.WriteLine("Operation failed.");
}
```

## See Also
* [Result](../Result/Result.md)
