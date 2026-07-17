# Result&lt;T&gt;.IsSuccess() Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Result.cs#L334" target="_blank">Result.cs</a>

Indicates whether the instance of the [Result&lt;T&gt;](ResultT.md) record represents a successful operation.

```csharp
public bool IsSuccess();
```

### Returns
[Boolean](https://learn.microsoft.com/en-ca/dotnet/api/system.boolean)

`true` if the instance of the [Result&lt;T&gt;](ResultT.md) record represents a successful operation; otherwise, `false`.

### Exceptions
[InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)

The [Result&lt;T&gt;](ResultT.md) holds neither a [Value](ResultT_Value.md) nor an [Error](ResultT_Error.md).

## Remarks
This method is annotated with `[MemberNotNullWhen(true, nameof(Value))]` and `[MemberNotNullWhen(false, nameof(Error))]`, so the compiler treats [Value](ResultT_Value.md) as non-null after this method returns `true`, and [Error](ResultT_Error.md) as non-null after it returns `false`. That is what allows `result.Value` to be used without a null check inside a success branch.

> [!NOTE]
> A [Result&lt;T&gt;](ResultT.md) is either successful (it holds a [Value](ResultT_Value.md)) or failed (it holds an [Error](ResultT_Error.md)). An instance holding neither cannot honour the annotation above, so this method throws an [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception) instead of returning a misleading `true`—which would let a `null` flow into a success branch with the compiler vouching for it.
>
> Such an instance can only be produced by the [parameterless constructor](ResultT_constructors.md), which is intended for deserialization, when neither member is subsequently populated. It is not reachable through [FromValue(T)](ResultT_FromValue.md), [FromError(Error)](ResultT_FromError.md) or the implicit operators.

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
