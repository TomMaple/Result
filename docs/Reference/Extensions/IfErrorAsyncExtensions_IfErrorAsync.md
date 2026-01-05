# IfErrorAsyncExtensions.IfErrorAsync Methods
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>

Runs a specific action or function if the instance of `TResult` represents a failed operation (i.e., contains an [Error](../Error/Error.md)).

## Overloads
| Name                             | Description                                                |
| -------------------------------- | ---------------------------------------------------------- |
| [IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, Task&gt;)](#iferrorasynctresulttresult-actionerror)  | Executes the provided asynchronous action, if the `TResult` instance represents a failed operation, as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, Task&lt;TResult&gt;&gt;)](#iferrorasynctresulttresult-funcerror-tresult) | Executes the provided asynchronous function, if the `TResult` instance represents a failed operation and returns its result, as an asynchronous operation. |

## Remarks

> [!NOTE]
> When an asynchronous method awaits a [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task) directly, continuation usually occurs in the same thread that created the task, depending on the async context. This behavior can be costly in terms of performance and can result in a deadlock on the UI thread. To avoid these problems, the `Task.ConfigureAwait(false)` is being called; unless the caller explicitly requires the original context, then set the `continueOnCapturedContext` parameter to `true`.
> 
> For more information, see <a href="https://devblogs.microsoft.com/dotnet/configureawait-faq/" target="_blank">ConfigureAwait FAQ</a>.

# IfError&lt;TResult&gt;(TResult, Func&lt;Error, Task&gt;)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/IfErrorAsyncExtensions.cs#L55" target="_blank">IfErrorAsyncExtensions.cs</a>

Executes the provided asynchronous action, only if the instance of `TResult` represents a failed operation (i.e., contains an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
public static async Task<TResult> IfErrorAsync<TResult>(this TResult result, Func<Error, Task> ifErrorAction, bool continueOnCapturedContext = false)
    where TResult : IResult;
```

### Parameters
#### `result` TResult
The instance of `TResult` (that implements [IResult](../IResult/IResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record) that represents the result of the operation to be evaluated.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.action-1)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;
The asynchronous action to be executed only if the current instance of `TResult` represents a failed operation.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

## Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

await createUserResult.IfErrorAsync(async error =>
{
    await _auditService.LogErrorAsync("Failed to create user: {ErrorDetail}", error.Detail);
    Metrics.IncrementCounter("user_creation_failures");
});
```

# IfError&lt;TResult&gt;(TResult, Func&lt;Error, Task&lt;TResult&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/IfErrorAsyncExtensions.cs#L97" target="_blank">IfErrorExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of `TResult` represents a failed operation (i.e., contains an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
public static TResult IfError<TResult>(this TResult result, Func<Error, Task<TResult>> ifErrorFunction)
        where TResult : IResult;
```

### Parameters
#### `result` TResult
The instance of `TResult` (that implements [IResult](../IResult/IResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record) that represents the result of the operation to be evaluated.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;TResult&gt;&gt;
The asynchronous function to be executed, only if the current instance of `TResult` represents a failed operation, as an asynchronous operation.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

## Examples
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

## See Also
* [Error](../Error/Error.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
