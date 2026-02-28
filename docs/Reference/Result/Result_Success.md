# Result.Success() Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Result.cs#L96" target="_blank">Result.cs</a>

Creates a new instance of the [Result](Result.md) record that represents a successful operation (without a value).

```csharp
public static Result Success();
```

### Returns
[Result](Result.md)

A new instance of the [Result](Result.md) record (without a value) that represents a successful operation.

## Remarks
> [!NOTE]
> This is a recommended way to create a new successful [Result](Result.md) instance (without a value).

## Examples
```csharp
return Result.Success();
```

## See Also
* [Result&lt;T&gt;](../ResultT/ResultT.md)
