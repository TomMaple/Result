# IfErrorValueTaskAsyncExtensions.IfErrorAsync Methods
## Definition
Namespace: [Maple.Result.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Runs a specific asynchronous action or function, if the instance of `TResult` represents a failed operation (i.e., contains an [Error](../Error/Error.md)), as an asynchronous operation.

## Overloads
| Name | Description |
| ---- | ----------- |
| [IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, ValueTask&gt;, Boolean)](#iferrorasynctresulttresult-funcerror-valuetask-boolean)  | Executes the provided asynchronous action, if the `TResult` instance represents a failed operation, as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, ValueTask&lt;TResult&gt;&gt;, Boolean)](#iferrorasynctresulttresult-funcerror-valuetasktresult-boolean) | Executes the provided asynchronous function, if the `TResult` instance represents a failed operation and returns its result, as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(ValueTask&lt;TResult&gt;, Func&lt;Error, ValueTask&gt;, Boolean)](#iferrorasynctresultvaluetasktresult-funcerror-valuetask-boolean)  | Executes the provided asynchronous function, if the provided asynchronous operation returns a failed `TResult` instance, and returns its result as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(ValueTask&lt;TResult&gt;, Func&lt;Error, ValueTask&lt;TResult&gt;&gt;, Boolean)](#iferrorasynctresultvaluetasktresult-funcerror-valuetasktresult-boolean) | Executes the provided asynchronous function, if the provided asynchronous operation returns a failed `TResult` instance, and returns its result as an asynchronous operation. |

## Remarks

> [!NOTE]
> When an asynchronous method awaits a [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask) directly, continuation usually occurs in the same thread that created the operation, depending on the async context. This behavior can be costly in terms of performance and can result in a deadlock on the UI thread. To avoid these problems, the `ValueTask.ConfigureAwait(false)` is being called; unless the caller explicitly requires the original context, then set the `continueOnCapturedContext` parameter to `true`.
> 
> For more information, see <a href="https://devblogs.microsoft.com/dotnet/configureawait-faq/" target="_blank">ConfigureAwait FAQ</a>.

> [!NOTE]
> A [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask) can only be awaited once. Because of this, the overloads accepting `this ValueTask<TResult> resultTask` consume the passed operation as part of evaluating its result, and they do not perform a `null` check on it (a `ValueTask<TResult>` is a non-nullable value type).


## IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, ValueTask&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/IfErrorValueTaskAsyncExtensions.cs#L56" target="_blank">IfErrorValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous action, only if the instance of `TResult` represents a failed operation (i.e., contains an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
public static async ValueTask<TResult> IfErrorAsync<TResult>(this TResult result, Func<Error, ValueTask> ifErrorAction, bool continueOnCapturedContext = false)
    where TResult : IResult;
```

### Type Parameters
#### `TResult`
The type of the `result` that implements the [IResult](../IResult/IResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record.

### Parameters
#### `result` TResult
The instance of `TResult` (that implements [IResult](../IResult/IResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record) that represents the result of the operation to be evaluated.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed only if the current instance of `TResult` represents a failed operation. The [Error](../Error/Error.md) instance contained in the `TResult` will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;TResult&gt;
The value task object representing the asynchronous operation.

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


## IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, ValueTask&lt;TResult&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/IfErrorValueTaskAsyncExtensions.cs#L103" target="_blank">IfErrorValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of `TResult` represents a failed operation (i.e., contains an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
public static async ValueTask<TResult> IfErrorAsync<TResult>(this TResult result, Func<Error, ValueTask<TResult>> ifErrorFunction, bool continueOnCapturedContext = false)
        where TResult : IResult;
```

### Type Parameters
#### `TResult`
The type of the `result` that implements the [IResult](../IResult/IResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record.

### Parameters
#### `result` TResult
The instance of `TResult` (that implements [IResult](../IResult/IResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record) that represents the result of the operation to be evaluated.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TResult&gt;&gt;
The asynchronous function to be executed, only if the current instance of `TResult` represents a failed operation, as an asynchronous operation. The [Error](../Error/Error.md) instance contained in the `TResult` will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;TResult&gt;
The value task object representing the asynchronous operation.

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


## IfErrorAsync&lt;TResult&gt;(ValueTask&lt;TResult&gt;, Func&lt;Error, ValueTask&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/IfErrorValueTaskAsyncExtensions.cs#L157" target="_blank">IfErrorValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, if the provided asynchronous operation returns a failed`TResult` instance (i.e., contains an [Error](../Error/Error.md)), and returns its result as an asynchronous operation.

```csharp
public static async ValueTask<TResult> IfErrorAsync<TResult>(
    this ValueTask<TResult> resultTask,
    Func<Error, ValueTask> ifErrorAction,
    bool continueOnCapturedContext = false)
    where TResult : IResult;
```

### Type Parameters
#### `TResult`
The type of the `resultTask` that implements the [IResult](../IResult/IResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record.

### Parameters
#### `resultTask` TResult
The asynchronous operation that represents an instance of `TResult` (that implements [IResult](../IResult/IResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record) to be executed and which result is to be evaluated.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed only if the `resultTask` represents a failed operation. The [Error](../Error/Error.md) instance contained in the outcome will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;TResult&gt;
The value task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `IfErrorAsync` method awaits the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .IfErrorAsync(async error =>
    {
        await _auditService.LogErrorAsync("Failed to create user: {ErrorDetail}", error.Detail);
        Metrics.IncrementCounter("user_creation_failures");
    });
```


## IfErrorAsync&lt;TResult&gt;(ValueTask&lt;TResult&gt;, Func&lt;Error, ValueTask&lt;TResult&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/IfErrorValueTaskAsyncExtensions.cs#L216" target="_blank">IfErrorValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, if the provided asynchronous operation returns a failed`TResult` instance (i.e., contains an [Error](../Error/Error.md)), and returns its result as an asynchronous operation.

```csharp
public static async ValueTask<TResult> IfErrorAsync<TResult>(this ValueTask<TResult> resultTask,
    Func<Error, ValueTask<TResult>> ifErrorFunction, bool continueOnCapturedContext = false)
    where TResult : IResult;
```

### Type Parameters
#### `TResult`
The type of the `resultTask` that implements the [IResult](../IResult/IResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record.

### Parameters
#### `resultTask` TResult
The asynchronous operation that represents an instance of `TResult` (that implements [IResult](../IResult/IResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record) to be executed and which result is to be evaluated.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;TResult&gt;&gt;
The asynchronous function to be executed, only if the current instance of `TResult` represents a failed operation, as an asynchronous operation. The [Error](../Error/Error.md) instance contained in the outcome will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;TResult&gt;
The value task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifErrorFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `IfErrorAsync` method awaits the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

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
