# Result.FromValue&lt;T&gt;(T) Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Result.cs#L132" target="_blank">Result.cs</a>

Creates a new instance of the generic [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents a successful operation with the specified value.

```csharp
public static Result<T> FromValue<T>(T value);
```

### Type Parameters
#### `T`
The type of the value to be associated with the successful result.

### Parameters
#### `value` T
The value to be associated with the created successful [Result&lt;T&gt;](../ResultT/ResultT.md) instance.

### Returns
[Result&lt;T&gt;](../ResultT/ResultT.md)

A new instance of the generic [Result&lt;T&gt;](../ResultT/ResultT.md) record with the provided *value* of the type `T`.

## Remarks
> [!NOTE]
> This is a recommended way to create a new [Result&lt;T&gt;](../ResultT/ResultT.md) instance with a value. Another way is to use the implicit operator.

## Examples
```csharp
return Result.FromValue(true);
```

## See Also
* [Result&lt;T&gt;](../ResultT/ResultT.md)
