# Result.Success() Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Result.cs#L83" target="_blank">Result.cs</a>

Returns an instance of the [Result](Result.md) record that represents a successful operation (without a value).

```csharp
public static Result Success();
```

### Returns
[Result](Result.md)

An instance of the [Result](Result.md) record (without a value) that represents a successful operation. It is always the same, shared instance.

## Remarks
> [!NOTE]
> This is the only way to get a successful [Result](Result.md) instance (without a value). Because [Result](Result.md) is immutable, every call returns the same instance.

## Examples
```csharp
return Result.Success();
```

## See Also
* [Result&lt;T&gt;](../ResultT/ResultT.md)
