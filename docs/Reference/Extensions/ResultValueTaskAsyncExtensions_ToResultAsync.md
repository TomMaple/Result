# ResultValueTaskAsyncExtensions.ToResultAsync(ValueTask&lt;Result&lt;T&gt;&gt;) Method
## Definition
Namespace: [Maple.Result.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/ResultValueTaskAsyncExtensions.cs#L45" target="_blank">ResultValueTaskAsyncExtensions.cs</a>

Asynchronously converts the outcome of the asynchronous operation represented by the [Result&lt;T&gt;](../ResultT/ResultT.md) to the non-generic [Result](../Result/Result.md).


```csharp
public static async ValueTask<Result> ToResultAsync<T>(this ValueTask<Result<T>> resultTask,
    bool continueOnCapturedContext = false);
```

### Type Parameters
#### `T`
The type of the [Result](../ResultT/ResultT.md)&lt;T&gt; `result` value to be converted.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../Result/Result.md)&gt;
The value task that represents the asynchronous operation producing the [Result](../ResultT/ResultT.md) record to be converted.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The value task object representing the asynchronous operation.

### Exceptions
#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
This `ToResult`method awaits the provided asynchronous operation represented by `resultTask` to evaluate its result.

### Examples
```csharp
var result = await _userService.CreateUserAsync(addUser)
    .ToResultAsync();
```


## See Also
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
