# ErrorUri.ToString() Method
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ErrorUri.cs#L190" target="_blank">ErrorUri.cs</a>

Returns the *URI* [Value](ErrorUri_Value.md).

```csharp
public override string ToString();
```

### Returns
[String](https://learn.microsoft.com/dotnet/api/system.string)

The *URI* value, e.g., `about:blank` or `tag:exampleapp.com,2026:errors:user:not-found`.

## Remarks
It overrides the compiler-generated `ToString()` of the record struct, which would return `ErrorUri { Value = … }`, so that an [ErrorUri](ErrorUri.md) can be logged or interpolated directly. A default instance returns `about:blank`.

## Examples
```csharp
var typeUri = ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:not-found");

Console.WriteLine($"Problem type: {typeUri}"); // Problem type: tag:exampleapp.com,2026:errors:user:not-found
```

## See Also
* [Value](ErrorUri_Value.md)
