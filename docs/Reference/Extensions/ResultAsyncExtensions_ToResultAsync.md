# ResultAsyncExtensions.ToResultAsync(Task&lt;Result&lt;T&gt;&gt;) Method
## Definition
Namespace: [Maple.Result.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/ResultAsyncExtensions.csL50" target="_blank">ResultAsyncExtensions.cs</a>

Asynchronously converts the outcome of the asynchronous operation represented by the [Result&lt;T&gt;](../ResultT/ResultT.md) to the non-generic [Result](../Result/Result.md).


```csharp
public static async Task<Result> ToResultAsync<T>(this Task<Result<T>> resultTask,
    bool continueOnCapturedContext = false);
```

### Type Parameters
#### `T`
The type of the [Result](../ResultT/ResultT.md)&lt;T&gt; `result` value to be converted.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The task that represents the asynchronous operation producing the [Result](../ResultT/ResultT.md) record to be converted.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
This `ToResult`method executes the provided asynchronous operation represented by `resultTask` to evaluate its result.

### Examples
```csharp
var result = await _userService.CreateUserAsync(addUser)
    .ToResultAsync();
```


## See Also
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
