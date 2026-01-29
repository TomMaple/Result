# Result&lt;T&gt;.FromValue(T) Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Result.cs#L259" target="_blank">Result.cs</a>

Creates a new instance of the generic [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents a successful operation with the specified value.

```csharp
public static Result<T> FromValue(T value);
```

### Type Parameters
#### `T`
The type of the `value` to be associated with the successful result.

### Parameters
#### `value` T
The value to be associated with the created successful [Result&lt;T&gt;](../ResultT/ResultT.md) instance.

### Returns
[Result&lt;T&gt;](../ResultT/ResultT.md)

A new instance of the generic [Result&lt;T&gt;](../ResultT/ResultT.md) record with the provided *value* of the type `T`.

## Remarks
> [!NOTE]
> This is a recommended way to create a new [Result&lt;T&gt;](../ResultT/ResultT.md) instance with a value. Another way is to use the [Implicit(T to Result&lt;T&gt;)](ResultT_implicit_T_to_ResultT.md) operator.

## Examples
```csharp
return Result<User>.FromValue(user);
```

## See Also
* [Result&lt;T&gt;](../ResultT/ResultT.md)
