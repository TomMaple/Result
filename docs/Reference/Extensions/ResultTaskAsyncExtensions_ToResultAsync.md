# ResultTaskAsyncExtensions.ToResultAsync(Task&lt;Result&lt;T&gt;&gt;, Boolean) Method
## Definition
Namespace: [Maple.Result.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ResultTaskAsyncExtensions.cs#L48" target="_blank">ResultTaskAsyncExtensions.cs</a>

Asynchronously converts the outcome of the asynchronous operation represented by the [Result&lt;T&gt;](../ResultT/ResultT.md) to the non-generic [Result](../Result/Result.md).


```csharp
public static async Task<Result> ToResultAsync<T>(this Task<Result<T>> resultTask,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the [Result](../ResultT/ResultT.md)&lt;T&gt; `result` value to be converted.

### Parameters
#### `resultTask` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The task that represents the asynchronous operation producing the [Result](../ResultT/ResultT.md) record to be converted.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

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
