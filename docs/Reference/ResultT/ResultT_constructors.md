# Result&lt;T&gt; Constructor
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Result.cs#L165" target="_blank">Result.cs</a>

Initializes a new instance of the [Result&lt;T&gt;](ResultT.md) record that indicates a successful operation that return a value.

```csharp
public Result();
```

## Remarks
> [!CAUTION]
> The constructor is not to be used directly—it is required for deserialization.
> 
> Instead, use one of the factory methods [FromValue(T)](ResultT_FromValue.md) or [FromError(Error)](ResultT_FromError.md), or one of the operators [Implicit(T to Result&lt;T&gt;](ResultT_implicit_T_to_ResultT.md) or [Implicit(Error to Result)](ResultT_implicit_Error_to_ResultT.md) to create an instance of the [ResultT](ResultT.md) record with appropriate properties.


## See also
* [Error](../Error/Error.md)