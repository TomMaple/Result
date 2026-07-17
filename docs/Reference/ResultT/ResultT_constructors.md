# Result&lt;T&gt; Constructor
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Result.cs#L180" target="_blank">Result.cs</a>

Initializes a new instance of the [Result&lt;T&gt;](ResultT.md) record that holds neither a [Value](ResultT_Value.md) nor an [Error](ResultT_Error.md), so that a deserializer can populate one of them afterwards.

```csharp
public Result();
```

## Remarks
> [!CAUTION]
> The constructor is not to be used directly—it is required for deserialization.
> 
> Instead, use one of the factory methods [FromValue(T)](ResultT_FromValue.md) or [FromError(Error)](ResultT_FromError.md), or one of the operators [Implicit(T to Result&lt;T&gt;](ResultT_implicit_T_to_ResultT.md) or [Implicit(Error to Result)](ResultT_implicit_Error_to_ResultT.md) to create an instance of the [ResultT](ResultT.md) record with appropriate properties.

> [!CAUTION]
> The instance this constructor produces is **not usable until either [Value](ResultT_Value.md) or [Error](ResultT_Error.md) is assigned**. A [Result&lt;T&gt;](ResultT.md) is either successful (it holds a value) or failed (it holds an error); one holding neither is in a state that no valid operation can produce.
>
> Calling [IsSuccess()](ResultT_IsSuccess.md) on such an instance throws an [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception) rather than reporting a misleading `true`. This applies to every extension method as well, since they all inspect the state through [IsSuccess()](ResultT_IsSuccess.md).

> [!NOTE]
> Unlike [Result](../Result/Result.md), whose parameterless constructor legitimately produces a successful result (there is no value to carry), [Result&lt;T&gt;](ResultT.md) requires a value to be successful. That is why the two constructors differ in behaviour despite looking alike.


## See also
* [Error](../Error/Error.md)
