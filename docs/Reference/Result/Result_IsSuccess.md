# Result.IsSuccess() Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Result.cs#L87" target="_blank">Result.cs</a>

Indicates whether the instance of the [Result](Result.md) record represents a successful operation.

```csharp
public bool IsSuccess();
```

### Returns
[Boolean](https://learn.microsoft.com/en-ca/dotnet/api/system.boolean)

`true` if the instance of the [Result](Result.md) record represents a successful operation; otherwise, `false`.

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
* [Result&lt;T&gt;](../ResultT/ResultT.md)
