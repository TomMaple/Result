# ResultExtensions.ToResult(Result&lt;T&gt;) Method
## Definition
Namespace: [Maple.Result.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/ResultExtensions.csL32" target="_blank">ResultExtensions.cs</a>

Converts the generic [Result&lt;T&gt;](../ResultT/ResultT.md)  to the non-generic [Result](../Result/Result.md).


```csharp
public static Result ToResult<T>(this Result<T> result);
```

### Type Parameters
#### `T`
The type of the [Result](../ResultT/ResultT.md)&lt;T&gt; `result` value to be converted.

### Parameters
#### `result` [Result](../ResultT/ResultT.md)&lt;T&gt;
The instance of [Result](../ResultT/ResultT.md) record to be converted.

### Returns
#### [Result](../Result/Result.md)
A [Result](../Result/Result.md) that does not contain the value if the provided `result` is successful; otherwise, a failed result without the value but with the original [Error](../Error/Error.md).

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `result` is `null`.

### Remarks
This method does not modify the original `result`.

### Examples
```csharp
var userResult = _userService.CreateUser(addUser);

var result = userResult.ToResult();
```


## See Also
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
