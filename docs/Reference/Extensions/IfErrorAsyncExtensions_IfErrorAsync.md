# IfErrorAsyncExtensions.IfErrorAsync Methods
## Definition
Namespace: [Maple.Result.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Runs a specific asynchronous action or function, if the instance of `TResult` represents a failed operation (i.e., contains an [Error](../Error/Error.md)), as an asynchronous operation.

## Overloads
| Name | Description |
| ---- | ----------- |
| [IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, Task&gt;, Boolean)](#iferrorasynctresulttresult-funcerror-task-boolean)  | Executes the provided asynchronous action, if the `TResult` instance represents a failed operation, as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, Task&lt;TResult&gt;&gt;, Boolean)](#iferrorasynctresulttresult-funcerror-tasktresult-boolean) | Executes the provided asynchronous function, if the `TResult` instance represents a failed operation and returns its result, as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(Task&lt;TResult&gt;, Func&lt;Error, Task&gt;, Boolean)](#iferrorasynctresulttasktresult-funcerror-task-boolean)  | Executes the provided asynchronous function, if the provided asynchronous operation returns a failed `TResult` instance, and returns its result as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(Task&lt;TResult&gt;, Func&lt;Error, Task&lt;TResult&gt;&gt;, Boolean)](#iferrorasynctresulttasktresult-funcerror-tasktresult-boolean) | Executes the provided asynchronous function, if the provided asynchronous operation returns a failed `TResult` instance, and returns its result as an asynchronous operation. |

## Remarks

> [!NOTE]
> When an asynchronous method awaits a [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task) directly, continuation usually occurs in the same thread that created the task, depending on the async context. This behavior can be costly in terms of performance and can result in a deadlock on the UI thread. To avoid these problems, the `Task.ConfigureAwait(false)` is being called; unless the caller explicitly requires the original context, then set the `continueOnCapturedContext` parameter to `true`.
> 
> For more information, see <a href="https://devblogs.microsoft.com/dotnet/configureawait-faq/" target="_blank">ConfigureAwait FAQ</a>.


## IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, Task&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/IfErrorAsyncExtensions.cs#L55" target="_blank">IfErrorAsyncExtensions.cs</a>

Executes the provided asynchronous action, only if the instance of `TResult` represents a failed operation (i.e., contains an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
public static async Task<TResult> IfErrorAsync<TResult>(this TResult result, Func<Error, Task> ifErrorAction, bool continueOnCapturedContext = false)
    where TResult : IResult;
```

### Type Parameters
#### `TResult`
The type of the `result` that implements the [IResult](../IResult/IResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record.

### Parameters
#### `result` TResult
The instance of `TResult` (that implements [IResult](../IResult/IResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record) that represents the result of the operation to be evaluated.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;
The asynchronous action to be executed only if the current instance of `TResult` represents a failed operation. The [Error](../Error/Error.md) instance contained in the `TResult` will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;TResult&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifErrorAction` parameter is `null`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

await createUserResult.IfErrorAsync(async error =>
{
    await _auditService.LogErrorAsync("Failed to create user: {ErrorDetail}", error.Detail);
    Metrics.IncrementCounter("user_creation_failures");
});
```


## IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, Task&lt;TResult&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/IfErrorAsyncExtensions.cs#L97" target="_blank">IfErrorExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of `TResult` represents a failed operation (i.e., contains an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
public static async Task<TResult> IfErrorAsync<TResult>(this TResult result, Func<Error, Task<TResult>> ifErrorFunction, bool continueOnCapturedContext = false)
        where TResult : IResult;
```

### Type Parameters
#### `TResult`
The type of the `result` that implements the [IResult](../IResult/IResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record.

### Parameters
#### `result` TResult
The instance of `TResult` (that implements [IResult](../IResult/IResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record) that represents the result of the operation to be evaluated.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;TResult&gt;&gt;
The asynchronous function to be executed, only if the current instance of `TResult` represents a failed operation, as an asynchronous operation. The [Error](../Error/Error.md) instance contained in the `TResult` will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;TResult&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifErrorFunction` parameter is `null`.

### Examples
```csharp
var userResult = await _userService.GetUserAsync(userData);

await userResult.IfErrorAsync(async error =>
{
    if (error.Category == ErrorCategory.NotFound)
    {
        // return a default user when not found
        return Result.FromValue(Users.NotFoundDefaultUser());
    }

    await _auditService.LogErrorAsync("Failed to create user: {ErrorDetail}", error.Detail);
    Metrics.IncrementCounter("user_retrieval_failures");

    return error;
});
```


## IfErrorAsync&lt;TResult&gt;(Task&lt;TResult&gt;, Func&lt;Error, Task&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/IfErrorAsyncExtensions.cs#L55" target="_blank">IfErrorAsyncExtensions.cs</a>

Executes the provided asynchronous function, if the provided asynchronous operation returns a failed`TResult` instance (i.e., contains an [Error](../Error/Error.md)), and returns its result as an asynchronous operation.

```csharp
public static async Task<TResult> IfErrorAsync<TResult>(
    this Task<TResult> resultTask,
    Func<Error, Task> ifErrorAction,
    bool continueOnCapturedContext = false)
    where TResult : IResult;
```

### Type Parameters
#### `TResult`
The type of the `resultTask` that implements the [IResult](../IResult/IResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record.

### Parameters
#### `resultTask` TResult
The asynchronous operation that represents an instance of `TResult` (that implements [IResult](../IResult/IResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record) to be executed and which result is to be evaluated.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;
The asynchronous action to be executed only if the `resultTask` represents a failed operation. The [Error](../Error/Error.md) instance contained in the outcome will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;TResult&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` parameter is `null` or `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `IfErrorAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `Task<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .IfErrorAsync(async error =>
    {
        await _auditService.LogErrorAsync("Failed to create user: {ErrorDetail}", error.Detail);
        Metrics.IncrementCounter("user_creation_failures");
    });
```


## IfErrorAsync&lt;TResult&gt;(Task&lt;TResult&gt;, Func&lt;Error, Task&lt;TResult&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/IfErrorAsyncExtensions.cs#L97" target="_blank">IfErrorExtensions.cs</a>

Executes the provided asynchronous function, if the provided asynchronous operation returns a failed`TResult` instance (i.e., contains an [Error](../Error/Error.md)), and returns its result as an asynchronous operation.

```csharp
public static async Task<TResult> IfErrorAsync<TResult>(this Task<TResult> resultTask,
    Func<Error, Task<TResult>> ifErrorFunction, bool continueOnCapturedContext = false)
    where TResult : IResult;
```

### Type Parameters
#### `TResult`
The type of the `resultTask` that implements the [IResult](../IResult/IResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record.

### Parameters
#### `resultTask` TResult
The asynchronous operation that represents an instance of `TResult` (that implements [IResult](../IResult/IResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record) to be executed and which result is to be evaluated.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;TResult&gt;&gt;
The asynchronous function to be executed, only if the current instance of `TResult` represents a failed operation, as an asynchronous operation. The [Error](../Error/Error.md) instance contained in the outcome will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;TResult&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` parameter is `null` or `ifErrorFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `IfErrorAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `Task<TResult>`.

### Examples
```csharp
var userResult = await _userService.GetUserAsync(userData)
    .IfErrorAsync(async error =>
    {
        if (error.Category == ErrorCategory.NotFound)
        {
            // return a default user when not found
            return Result.FromValue(Users.NotFoundDefaultUser());
        }

        await _auditService.LogErrorAsync("Failed to create user: {ErrorDetail}", error.Detail);
        Metrics.IncrementCounter("user_retrieval_failures");

        return error;
    });
```

## See Also
* [Error](../Error/Error.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
