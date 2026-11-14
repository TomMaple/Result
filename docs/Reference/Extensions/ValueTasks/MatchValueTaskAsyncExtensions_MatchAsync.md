# MatchValueTaskAsyncExtensions.MatchAsync Methods
## Definition
Namespace: [Maple.Result.Extensions.ValueTasks](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Runs a specific asynchronous action or function depending on whether the instance of [Result](../../Result/Result.md) and [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful or a failed operation, as an asynchronous operation.

## Overloads
| Name | Description |
| ---- | ----------- |
| [MatchAsync(Result, Func&lt;ValueTask&gt;, Func&lt;Error, ValueTask&gt;, Boolean)](#matchasyncresult-funcvaluetask-funcerror-valuetask-boolean) | Executes one of the specified asynchronous actions as an asynchronous operation depending on whether the [Result](../../Result/Result.md) represents a success or a failure. |
| [MatchAsync(Result, Func&lt;ValueTask&lt;Result&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)](#matchasyncresult-funcvaluetaskresult-funcerror-valuetask-boolean) | Executes the specified asynchronous function or action as an asynchronous operation depending on whether the [Result](../../Result/Result.md) represents a success or a failure. |
| [MatchAsync(Result, Func&lt;ValueTask&lt;Result&gt;&gt;, Func&lt;Error, ValueTask&lt;Result&gt;&gt;, Boolean)](#matchasyncresult-funcvaluetaskresult-funcerror-valuetaskresult-boolean) | Executes one of the specified asynchronous functions as an asynchronous operation depending on whether the [Result](../../Result/Result.md) represents a success or a failure. |
| [MatchAsync(Result, Func&lt;ValueTask&lt;Error&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)](#matchasyncresult-funcvaluetaskerror-funcerror-valuetask-boolean) | Executes the specified asynchronous function or action depending on whether the [Result](../../Result/Result.md) represents a success or a failure, and returns a failed [Result](../../Result/Result.md). |
| [MatchAsync&lt;T&gt;(Result, Func&lt;ValueTask&lt;T&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)](#matchasynctresult-funcvaluetaskt-funcerror-valuetask-boolean) | Executes the specified asynchronous function or action as an asynchronous operation depending on whether the [Result](../../Result/Result.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(Result, Func&lt;ValueTask&lt;T&gt;&gt;, Func&lt;Error, ValueTask&lt;T&gt;&gt;, Boolean)](#matchasynctresult-funcvaluetaskt-funcerror-valuetaskt-boolean) | Executes one of the specified asynchronous functions as an asynchronous operation depending on whether the [Result](../../Result/Result.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(Result, Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)](#matchasynctresult-funcvaluetaskresultt-funcerror-valuetask-boolean) | Executes the specified asynchronous function or action as an asynchronous operation depending on whether the [Result](../../Result/Result.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(Result, Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Func&lt;Error, ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Boolean)](#matchasynctresult-funcvaluetaskresultt-funcerror-valuetaskresultt-boolean) | Executes one of the specified asynchronous functions as an asynchronous operation depending on whether the [Result](../../Result/Result.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&gt;, Func&lt;Error, ValueTask&gt;, Boolean)](#matchasynctresultt-funct-valuetask-funcerror-valuetask-boolean) |  Executes one of the specified asynchronous actions as an asynchronous operation depending on whether the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Error&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)](#matchasynctresultt-funct-valuetaskerror-funcerror-valuetask-boolean) | Executes the specified asynchronous function or action depending on whether the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a success or a failure, and returns a failed [Result](../../Result/Result.md). |
| [MatchAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;TNext&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)](#matchasynct-tnextresultt-funct-valuetasktnext-funcerror-valuetask-boolean) |  Executes the specified asynchronous function or action as an asynchronous operation depending on whether the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a success or a failure. |
| [MatchAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;TNext&gt;&gt;, Func&lt;Error, ValueTask&lt;TNext&gt;&gt;, Boolean)](#matchasynct-tnextresultt-funct-valuetasktnext-funcerror-valuetasktnext-boolean) |  Executes one of the specified asynchronous functions as an asynchronous operation depending on whether the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a success or a failure. |
| [MatchAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&lt;TNext&gt;&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)](#matchasynct-tnextresultt-funct-valuetaskresulttnext-funcerror-valuetask-boolean) |  Executes the specified asynchronous function or action as an asynchronous operation depending on whether the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a success or a failure. |
| [MatchAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&lt;TNext&gt;&gt;&gt;, Func&lt;Error, ValueTask&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)](#matchasynct-tnextresultt-funct-valuetaskresulttnext-funcerror-valuetaskresulttnext-boolean) |  Executes one of the specified asynchronous functions as an asynchronous operation depending on whether the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a success or a failure. |
| [MatchAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&gt;, Func&lt;Error, ValueTask&gt;, Boolean)](#matchasyncvaluetaskresult-funcvaluetask-funcerror-valuetask-boolean) | Executes one of the specified asynchronous actions as an asynchronous operation depending on whether the [Result](../../Result/Result.md) represents a success or a failure. |
| [MatchAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Result&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)](#matchasyncvaluetaskresult-funcvaluetaskresult-funcerror-valuetask-boolean) | Executes the specified asynchronous function or action as an asynchronous operation depending on whether the [Result](../../Result/Result.md) represents a success or a failure. |
| [MatchAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Result&gt;&gt;, Func&lt;Error, ValueTask&lt;Result&gt;&gt;, Boolean)](#matchasyncvaluetaskresult-funcvaluetaskresult-funcerror-valuetaskresult-boolean) | Executes one of the specified asynchronous functions as an asynchronous operation depending on whether the [Result](../../Result/Result.md) represents a success or a failure. |
| [MatchAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Error&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)](#matchasyncvaluetaskresult-funcvaluetaskerror-funcerror-valuetask-boolean) | Executes the specified asynchronous function or action depending on whether the asynchronous operation that returns a [Result](../../Result/Result.md) represents a success or a failure, and returns a failed [Result](../../Result/Result.md). |
| [MatchAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;T&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)](#matchasynctvaluetaskresult-funcvaluetaskt-funcerror-valuetask-boolean) | Executes the specified asynchronous function or action as an asynchronous operation depending on whether the [Result](../../Result/Result.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;T&gt;&gt;, Func&lt;Error, ValueTask&lt;T&gt;&gt;, Boolean)](#matchasynctvaluetaskresult-funcvaluetaskt-funcerror-valuetaskt-boolean) | Executes one of the specified asynchronous functions as an asynchronous operation depending on whether the [Result](../../Result/Result.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)](#matchasynctvaluetaskresult-funcvaluetaskresultt-funcerror-valuetask-boolean) | Executes the specified asynchronous function or action as an asynchronous operation depending on whether the [Result](../../Result/Result.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Func&lt;Error, ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Boolean)](#matchasynctvaluetaskresult-funcvaluetaskresultt-funcerror-valuetaskresultt-boolean) | Executes one of the specified asynchronous functions as an asynchronous operation depending on whether the [Result](../../Result/Result.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&gt;, Func&lt;Error, ValueTask&gt;, Boolean)](#matchasynctvaluetaskresultt-funct-valuetask-funcerror-valuetask-boolean) |  Executes one of the specified asynchronous actions as an asynchronous operation depending on whether the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Error&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)](#matchasynctvaluetaskresultt-funct-valuetaskerror-funcerror-valuetask-boolean) | Executes the specified asynchronous function or action depending on whether the asynchronous operation that returns a [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a success or a failure, and returns a failed [Result](../../Result/Result.md). |
| [MatchAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;TNext&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)](#matchasynct-tnextvaluetaskresultt-funct-valuetasktnext-funcerror-valuetask-boolean) |  Executes the specified asynchronous function or action as an asynchronous operation depending on whether the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a success or a failure. |
| [MatchAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;TNext&gt;&gt;, Func&lt;Error, ValueTask&lt;TNext&gt;&gt;, Boolean)](#matchasynct-tnextvaluetaskresultt-funct-valuetasktnext-funcerror-valuetasktnext-boolean) |  Executes one of the specified asynchronous functions as an asynchronous operation depending on whether the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a success or a failure. |
| [MatchAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&lt;TNext&gt;&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)](#matchasynct-tnextvaluetaskresultt-funct-valuetaskresulttnext-funcerror-valuetask-boolean) |  Executes the specified asynchronous function or action as an asynchronous operation depending on whether the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a success or a failure. |
| [MatchAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&lt;TNext&gt;&gt;&gt;, Func&lt;Error, ValueTask&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)](#matchasynct-tnextvaluetaskresultt-funct-valuetaskresulttnext-funcerror-valuetaskresulttnext-boolean) |  Executes one of the specified asynchronous functions as an asynchronous operation depending on whether the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a success or a failure. |


## Remarks

An asynchronous success function that returns an [Error](../../Error/Error.md) makes the result fail: see [MatchAsync(Result, Func&lt;ValueTask&lt;Error&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)](#matchasyncresult-funcvaluetaskerror-funcerror-valuetask-boolean), [MatchAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Error&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)](#matchasynctresultt-funct-valuetaskerror-funcerror-valuetask-boolean), [MatchAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Error&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)](#matchasyncvaluetaskresult-funcvaluetaskerror-funcerror-valuetask-boolean), [MatchAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Error&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)](#matchasynctvaluetaskresultt-funct-valuetaskerror-funcerror-valuetask-boolean).

> [!NOTE]
> When an asynchronous method awaits a [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask) directly, continuation usually occurs in the same thread that created the task, depending on the async context. This behavior can be costly in terms of performance and can result in a deadlock on the UI thread. To avoid these problems, `ConfigureAwait(false)` is used by default, so in the methods that take a `ValueTask` of a result the passed delegates run on a thread-pool thread. When the delegates need the original context (e.g., to update UI elements), set the `continueOnCapturedContext` parameter to `true`, or set [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) to `true` once at startup.
> 
> For more information, see <a href="https://devblogs.microsoft.com/dotnet/configureawait-faq/" target="_blank">ConfigureAwait FAQ</a>.

> [!NOTE]
> A [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask) can only be awaited once. Because of this, the overloads accepting `this ValueTask<Result> resultTask` / `this ValueTask<Result<T>> resultTask` consume the passed operation as part of evaluating its result, and they do not perform a `null` check on it (a `ValueTask<TResult>` is a non-nullable value type).


## MatchAsync(Result, Func&lt;ValueTask&gt;, Func&lt;Error, ValueTask&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs#L38" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessAction`, only if the instance of [Result](../../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), or the provided asynchronous `ifErrorAction` otherwise, as an asynchronous operation.

```csharp
public static async ValueTask<Result> MatchAsync(this Result result, Func<ValueTask> ifSuccessAction,
    Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null);
```

### Parameters
#### `result` [Result](../../Result/Result.md)
The instance of [Result](../../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed, only if the current instance of [Result](../../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed, only if the current instance of [Result](../../Result/Result.md) represents a failed operation, as an asynchronous operation. The [Error](../../Error/Error.md) instance contained in the `result` will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessAction` or `ifErrorAction` parameter is `null`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

await createUserResult.MatchAsync(
    () => _auditService.LogUserAddedAsync(userData),
    error => _auditService.LogError(error));
```


## MatchAsync(Result, Func&lt;ValueTask&lt;Result&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs#L58" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the instance of [Result](../../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), or the provided asynchronous `ifErrorAction` otherwise, as an asynchronous operation.

```csharp
public static async ValueTask<Result> MatchAsync(this Result result, Func<ValueTask<Result>> ifSuccessFunction,
    Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null);
```

### Parameters
#### `result` [Result](../../Result/Result.md)
The instance of [Result](../../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed, only if the current instance of [Result](../../Result/Result.md) represents a failed operation, as an asynchronous operation. The [Error](../../Error/Error.md) instance contained in the `result` will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorAction` parameter is `null`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var userAddedResult = await createUserResult.MatchAsync(
    () => _companyService.AddUserAsync(userData),
    error => _auditService.LogError(error));
```


## MatchAsync(Result, Func&lt;ValueTask&lt;Result&gt;&gt;, Func&lt;Error, ValueTask&lt;Result&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs#L77" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the instance of [Result](../../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), or the provided asynchronous `ifErrorFunction` otherwise, as an asynchronous operation.

```csharp
public static async ValueTask<Result> MatchAsync(this Result result, Func<ValueTask<Result>> ifSuccessFunction,
    Func<Error, ValueTask<Result>> ifErrorFunction, bool? continueOnCapturedContext = null);
```

### Parameters
#### `result` [Result](../../Result/Result.md)
The instance of [Result](../../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../../Result/Result.md) represents a failed operation, as an asynchronous operation. The [Error](../../Error/Error.md) instance contained in the `result` will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorFunction` parameter is `null`.

### Examples
```csharp
var authenticationResult = await _loginService.AuthenticateAsync(loginData);

var userAddedResult = await authenticationResult.MatchAsync(
    async () => await _companyService.AddUserAsync(userData),
    async error =>
    {
        await _auditService.LogErrorAsync(error);
        return await _loginService.LockAccountAsync(loginData.Username);
    });
```


## MatchAsync(Result, Func&lt;ValueTask&lt;Error&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs#L115" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction` only if the instance of [Result](../../Result/Result.md) represents a successful operation, and returns a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) returned by the function; or executes the provided asynchronous `ifErrorAction` otherwise.

```csharp
public static async ValueTask<Result> MatchAsync(this Result result, Func<ValueTask<Error>> ifSuccessFunction,
    Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null);
```

### Parameters
#### `result` [Result](../../Result/Result.md)
The instance of [Result](../../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Error](../../Error/Error.md)&gt;&gt;
The asynchronous function to be executed only if the `result` represents a successful operation. It returns the [Error](../../Error/Error.md) of the failed result.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed only if the `result` represents a failed operation. The [Error](../../Error/Error.md) instance will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The value task object representing the asynchronous operation: a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) returned by `ifSuccessFunction` if the `result` is successful; otherwise, the original result. The returned result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result`, `ifSuccessFunction` or `ifErrorAction` parameter is `null`, or `ifSuccessFunction` returns `null`.

### Remarks
Without this overload, an asynchronous function returning an [Error](../../Error/Error.md) would return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `MatchAsync<Error>(…)`.

### Examples
```csharp
var result = await _userService.DeleteUser(userId)
    .MatchAsync(
        async () => await _policyService.GetDeletionErrorAsync(userId),
        async (error) => await _auditService.LogErrorAsync(error));
```


## MatchAsync&lt;T&gt;(Result, Func&lt;ValueTask&lt;T&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs#L95" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the instance of [Result](../../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), or the provided asynchronous `ifErrorAction` otherwise, as an asynchronous operation.

```csharp
public static async ValueTask<Result<T>> MatchAsync<T>(this Result result, Func<ValueTask<T>> ifSuccessFunction,
    Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result](../../Result/Result.md)
The instance of [Result](../../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;T&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed, only if the current instance of [Result](../../Result/Result.md) represents a failed operation, as an asynchronous operation. The [Error](../../Error/Error.md) instance contained in the `result` will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorAction` parameter is `null`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.MatchAsync(
    async (user) => await _loginService.GetUserTokenAsync(user),
    async (error) => await _auditService.LogErrorAsync(error));
```


## MatchAsync&lt;T&gt;(Result, Func&lt;ValueTask&lt;T&gt;&gt;, Func&lt;Error, ValueTask&lt;T&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs#L134" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the instance of [Result](../../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), or the provided asynchronous `ifErrorFunction` otherwise, as an asynchronous operation.

```csharp
public static async ValueTask<T> MatchAsync<T>(this Result result, Func<ValueTask<T>> ifSuccessFunction,
    Func<Error, ValueTask<T>> ifErrorFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value returned by the asynchronous `ifSuccessFunction` and `ifErrorFunction`.

### Parameters
#### `result` [Result](../../Result/Result.md)
The instance of [Result](../../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;T&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;T&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../../Result/Result.md) represents a failed operation, as an asynchronous operation. The [Error](../../Error/Error.md) instance contained in the `result` will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;T&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorFunction` parameter is `null`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var viewModel = await createUserResult.MatchAsync(
    async (user) =>
    {
        var userDetails = await _userService.GetUserDetailsAsync(user);
        return _userMapper.Map(userDetails);
    },
    async (error) => return _userMapper.MapErrorViewModel(error));
```


## MatchAsync&lt;T&gt;(Result, Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs#L152" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the instance of [Result](../../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), or the provided asynchronous `ifErrorAction` otherwise, as an asynchronous operation.

```csharp
public static async ValueTask<Result<T>> MatchAsync<T>(this Result result, Func<ValueTask<Result<T>>> ifSuccessFunction,
    Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the [Result&lt;T&gt;](../../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result](../../Result/Result.md)
The instance of [Result](../../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed, only if the current instance of [Result](../../Result/Result.md) represents a failed operation, as an asynchronous operation. The [Error](../../Error/Error.md) instance contained in the `result` will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorAction` parameter is `null`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var userDetailsResult = await createUserResult.MatchAsync(
    user => _userService.GetUserDetailsAsync(user),
    error => ValueTask.FromResult(_userMapper.MapErrorViewModel(error)));
```


## MatchAsync&lt;T&gt;(Result, Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Func&lt;Error, ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs#L171" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the instance of [Result](../../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), or the provided asynchronous `ifErrorFunction` otherwise, as an asynchronous operation.

```csharp
public static async ValueTask<Result<T>> MatchAsync<T>(this Result result, Func<ValueTask<Result<T>>> ifSuccessFunction,
    Func<Error, ValueTask<Result<T>>> ifErrorFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the [Result&lt;T&gt;](../../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction` and `ifErrorFunction`.

### Parameters
#### `result` [Result](../../Result/Result.md)
The instance of [Result](../../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../../Result/Result.md) represents a failed operation, as an asynchronous operation. The [Error](../../Error/Error.md) instance contained in the `result` will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorFunction` parameter is `null`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var userDetailsResult = await createUserResult.MatchAsync(
    user => _userService.GetUserDetailsAsync(user),
    error => _userMapper.MapErrorViewModel(error));
```


## MatchAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&gt;, Func&lt;Error, ValueTask&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs#L191" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessAction`, only if the instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), or the provided asynchronous `ifErrorAction` otherwise, as an asynchronous operation.

```csharp
public static async ValueTask<Result<T>> MatchAsync<T>(this Result<T> result, Func<T, ValueTask> ifSuccessAction,
    Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the asynchronous `ifSuccessAction`.

### Parameters
#### `result` [Result&lt;T&gt;](../../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) record that represents the result of the operation with a value to be evaluated.

#### `ifSuccessAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed, only if the current instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. The value of type `T` contained in the [Result&lt;T&gt;](../../ResultT/ResultT.md) instance will be passed as a parameter to this action.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed, only if the current instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a failed operation, as an asynchronous operation. The [Error](../../Error/Error.md) instance contained in the `result` will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessAction` or `ifErrorAction` parameter is `null`.

### Examples
```csharp
var userResult = await _userService.CreateUserAsync(userData);

await userResult.MatchAsync(
    user => _auditService.LogUserAddedAsync(user),
    error => _auditService.LogErrorAsync(error));
```


## MatchAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Error&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs#L235" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction` only if the instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, and returns a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) returned by the function; or executes the provided asynchronous `ifErrorAction` otherwise.

```csharp
public static async ValueTask<Result> MatchAsync<T>(
    this Result<T> result,
    Func<T, ValueTask<Error>> ifSuccessFunction,
    Func<Error, ValueTask> ifErrorAction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result, passed to the `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Error](../../Error/Error.md)&gt;&gt;
The asynchronous function to be executed only if the `result` represents a successful operation. The value of the result is passed as a parameter. It returns the [Error](../../Error/Error.md) of the failed result.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed only if the `result` represents a failed operation. The [Error](../../Error/Error.md) instance will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The value task object representing the asynchronous operation: a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) returned by `ifSuccessFunction` if the `result` is successful; otherwise, a failed [Result](../../Result/Result.md) with the original [Error](../../Error/Error.md). The returned result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result`, `ifSuccessFunction` or `ifErrorAction` parameter is `null`, or `ifSuccessFunction` returns `null`.

### Remarks
Without this overload, an asynchronous function returning an [Error](../../Error/Error.md) would return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `MatchAsync<T, Error>(…)`.

### Examples
```csharp
var result = await _userService.GetUser(userId)
    .MatchAsync(
        async (user) => await _policyService.GetDeletionErrorAsync(user),
        async (error) => await _auditService.LogErrorAsync(error));
```


## MatchAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;TNext&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs#L212" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), or the provided asynchronous `ifErrorAction` otherwise, as an asynchronous operation.

```csharp
public static async ValueTask<Result<TNext>> MatchAsync<T, TNext>(
    this Result<T> result,
    Func<T, ValueTask<TNext>> ifSuccessFunction,
    Func<Error, ValueTask> ifErrorAction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) record that represents the result of the operation with a value to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;TNext&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. The value of type `T` contained in the [Result&lt;T&gt;](../../ResultT/ResultT.md) instance will be passed as a parameter to this function.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed, only if the current instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a failed operation, as an asynchronous operation. The [Error](../../Error/Error.md) instance contained in the `result` will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../Result/Result.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorAction` parameter is `null`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var userAddedResult = await createUserResult.MatchAsync(
    user => _companyService.AddUserAsync(user),
    error => _auditService.LogErrorAsync(error));
```


## MatchAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;TNext&gt;&gt;, Func&lt;Error, ValueTask&lt;TNext&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs#L260" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), or the provided asynchronous `ifErrorFunction` otherwise, as an asynchronous operation.

```csharp
public static async ValueTask<TNext> MatchAsync<T, TNext>(
    this Result<T> result,
    Func<T, ValueTask<TNext>> ifSuccessFunction,
    Func<Error, ValueTask<TNext>> ifErrorFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the value returned by the asynchronous `ifSuccessFunction` and `ifErrorFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) record that represents the result of the operation with a value to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;TNext&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. The value of type `T` contained in the [Result&lt;T&gt;](../../ResultT/ResultT.md) instance will be passed as a parameter to this function.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TNext&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a failed operation, as an asynchronous operation. The [Error](../../Error/Error.md) instance contained in the `result` will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;TNext&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorFunction` parameter is `null`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.MatchAsync(
    async (user) =>
    {
        await _companyService.GetCompanyByIdAsync(user.CompanyId);
        return _mapper.Map(user);
    },
    async (error) => ValueTask.FromResult(_mapper.Map(error));
```


## MatchAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&lt;TNext&gt;&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs#L281" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), or the provided asynchronous `ifErrorAction` otherwise, as an asynchronous operation.

```csharp
public static async ValueTask<Result<TNext>> MatchAsync<T, TNext>(
    this Result<T> result,
    Func<T, ValueTask<Result<TNext>>> ifSuccessFunction,
    Func<Error, ValueTask> ifErrorAction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the [Result&lt;TNext&gt;](../../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../ResultT/ResultT.md)&lt;TNext&gt;&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. The value of type `T` contained in the [Result&lt;T&gt;](../../ResultT/ResultT.md) instance will be passed as a parameter to this function.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed, only if the current instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a failed operation, as an asynchronous operation. The [Error](../../Error/Error.md) instance contained in the `result` will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorAction` parameter is `null`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.MatchAsync(
    user => _companyService.GetCompanyByIdAsync(user.CompanyId),
    error => _auditService.LogErrorAsync(error));
```


## MatchAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&lt;TNext&gt;&gt;&gt;, Func&lt;Error, ValueTask&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs#L303" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), or the provided asynchronous `ifErrorFunction` otherwise, as an asynchronous operation.

```csharp
public static async ValueTask<Result<TNext>> MatchAsync<T, TNext>(
    this Result<T> result,
    Func<T, ValueTask<Result<TNext>>> ifSuccessFunction,
    Func<Error, ValueTask<Result<TNext>>> ifErrorFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the [Result&lt;TNext&gt;](../../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction` and `ifErrorFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../ResultT/ResultT.md)&lt;TNext&gt;&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. The value of type `T` contained in the [Result&lt;T&gt;](../../ResultT/ResultT.md) instance will be passed as a parameter to this function.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../ResultT/ResultT.md)&lt;TNext&gt;&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a failed operation, as an asynchronous operation. The [Error](../../Error/Error.md) instance contained in the `result` will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorAction` parameter is `null`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var viewModel = await createUserResult.MatchAsync(
    user => _companyService.GetCompanyByIdAsync(user.CompanyId),
    async (error) =>
    {
        await _auditService.LogErrorAsync(error);
        return _mapper.Map(error);
    });
```


## MatchAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&gt;, Func&lt;Error, ValueTask&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs#L358" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessAction`, only if the asynchronous operation `resultTask` represents a successful instance of [Result](../../Result/Result.md); or the provided asynchronous `ifErrorAction`, otherwise.

```csharp
public static async ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask, Func<ValueTask> ifSuccessAction,
    Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../../Result/Result.md) record to be evaluated.

#### `ifSuccessAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed, only if the `resultTask` represents a successful operation.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed, only if the `resultTask` represents a failed operation. The [Error](../../Error/Error.md) instance contained in the outcome of the `resultTask` will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessAction` or `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `MatchAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .MatchAsync(
        () => _auditService.LogUserAddedAsync(userData),
        error => _auditService.LogError(error));
```


## MatchAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Result&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs#L411" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the asynchronous operation `resultTask` represents a successful instance of [Result](../../Result/Result.md); or the provided asynchronous `ifErrorAction`, otherwise.

```csharp
public static async ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask, Func<ValueTask<Result>> ifSuccessFunction,
    Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed, only if the `resultTask` represents a failed operation. The [Error](../../Error/Error.md) instance contained in the outcome of the `resultTask` will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` or `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `MatchAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .MatchAsync(
        () => _companyService.AddUserAsync(userData),
        error => _auditService.LogError(error));
```


## MatchAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Result&gt;&gt;, Func&lt;Error, ValueTask&lt;Result&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs#L461" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the asynchronous operation `resultTask` represents a successful instance of [Result](../../Result/Result.md); or the provided asynchronous `ifErrorFunction`, otherwise.

```csharp
public static async ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask, Func<ValueTask<Result>> ifSuccessFunction,
    Func<Error, ValueTask<Result>> ifErrorFunction, bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a failed operation. The [Error](../../Error/Error.md) instance contained in the outcome of the `resultTask` will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` or `ifErrorFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `MatchAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var authenticationResult = await _loginService.AuthenticateAsync(loginData)
    .MatchAsync(
        async () => await _companyService.AddUserAsync(userData),
        async error =>
        {
            await _auditService.LogErrorAsync(error);
            return await _loginService.LockAccountAsync(loginData.Username);
        });
```


## MatchAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Error&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs#L576" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction` only if the asynchronous operation `resultTask` represents a successful instance of [Result](../../Result/Result.md) represents a successful operation, and returns a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) returned by the function; or executes the provided asynchronous `ifErrorAction` otherwise.

```csharp
public static async ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask, Func<ValueTask<Error>> ifSuccessFunction,
    Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Error](../../Error/Error.md)&gt;&gt;
The asynchronous function to be executed only if the `resultTask` represents a successful operation. It returns the [Error](../../Error/Error.md) of the failed result.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed only if the `resultTask` represents a failed operation. The [Error](../../Error/Error.md) instance will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The value task object representing the asynchronous operation: a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) returned by `ifSuccessFunction` if the `resultTask` is successful; otherwise, the original result. The returned result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` or `ifErrorAction` parameter is `null`, or `ifSuccessFunction` returns `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
Without this overload, an asynchronous function returning an [Error](../../Error/Error.md) would return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `MatchAsync<Error>(…)`.

### Examples
```csharp
var result = await _userService.DeleteUserAsync(userId)
    .MatchAsync(
        async () => await _policyService.GetDeletionErrorAsync(userId),
        async (error) => await _auditService.LogErrorAsync(error));
```


## MatchAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;T&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs#L512" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the asynchronous operation `resultTask` represents a successful instance of [Result](../../Result/Result.md); or the provided asynchronous `ifErrorAction`, otherwise.

```csharp
public static async ValueTask<Result<T>> MatchAsync<T>(this ValueTask<Result> resultTask, Func<ValueTask<T>> ifSuccessFunction,
    Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;T&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed, only if the `resultTask` represents a failed operation. The [Error](../../Error/Error.md) instance contained in the outcome of the `resultTask` will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` or `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `MatchAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .MatchAsync(
        async (user) => await _loginService.GetUserTokenAsync(user),
        async (error) => await _auditService.LogErrorAsync(error));
```


## MatchAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;T&gt;&gt;, Func&lt;Error, ValueTask&lt;T&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs#L627" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the asynchronous operation `resultTask` represents a successful instance of [Result](../../Result/Result.md); or the provided asynchronous `ifErrorFunction`, otherwise.

```csharp
public static async ValueTask<T> MatchAsync<T>(this ValueTask<Result> resultTask, Func<ValueTask<T>> ifSuccessFunction,
    Func<Error, ValueTask<T>> ifErrorFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value returned by the asynchronous `ifSuccessFunction` and `ifErrorFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;T&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;T&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a failed operation. The [Error](../../Error/Error.md) instance contained in the outcome of the `resultTask` will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;T&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` or `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `MatchAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .MatchAsync(
        async (user) =>
        {
            var userDetails = await _userService.GetUserDetailsAsync(user);
            return _userMapper.Map(userDetails);
        },
        async (error) => return _userMapper.MapErrorViewModel(error));
```


## MatchAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs#L678" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the asynchronous operation `resultTask` represents a successful instance of [Result](../../Result/Result.md); or the provided asynchronous `ifErrorAction`, otherwise.

```csharp
public static async ValueTask<Result<T>> MatchAsync<T>(
    this ValueTask<Result> resultTask, Func<ValueTask<Result<T>>> ifSuccessFunction,
    Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the [Result&lt;T&gt;](../../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed, only if the `resultTask` represents a failed operation. The [Error](../../Error/Error.md) instance contained in the outcome of the `resultTask` will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` or `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `MatchAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .MatchAsync(
        user => _userService.GetUserDetailsAsync(user),
        error => ValueTask.FromResult(_userMapper.MapErrorViewModel(error)));
```


## MatchAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Func&lt;Error, ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs#L730" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the asynchronous operation `resultTask` represents a successful instance of [Result](../../Result/Result.md); or the provided asynchronous `ifErrorFunction`, otherwise.

```csharp
public static async ValueTask<Result<T>> MatchAsync<T>(
    this ValueTask<Result> resultTask, Func<ValueTask<Result<T>>> ifSuccessFunction,
    Func<Error, ValueTask<Result<T>>> ifErrorFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the [Result&lt;T&gt;](../../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction` and `ifErrorFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a failed operation. The [Error](../../Error/Error.md) instance contained in the outcome of the `resultTask` will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` or `ifErrorFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `MatchAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .MatchAsync(
        user => _userService.GetUserDetailsAsync(user),
        error => _userMapper.MapErrorViewModel(error));
```


## MatchAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&gt;, Func&lt;Error, ValueTask&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs#L790" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessAction`, only if the asynchronous operation `resultTask` represents a successful instance of [Result&lt;T&gt;](../../ResultT/ResultT.md); or the provided asynchronous `ifErrorAction`, otherwise.

```csharp
public static async ValueTask<Result<T>> MatchAsync<T>(this ValueTask<Result<T>> resultTask, Func<T, ValueTask> ifSuccessAction,
    Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `resultTask` and passed to the asynchronous `ifSuccessAction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../../Result/Result.md) record to be evaluated.

#### `ifSuccessAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed, only if the `resultTask` represents a successful operation.
The value of type `T` contained in the [Result&lt;T&gt;](../../ResultT/ResultT.md) outcome of `resultTask` will be passed as a parameter to this action.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed, only if the `resultTask` represents a failed operation. The [Error](../../Error/Error.md) instance contained in the outcome of the `resultTask` will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessAction` or `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `MatchAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var userResult = await _userService.CreateUserAsync(userData)
    .MatchAsync(
        user => _auditService.LogUserAddedAsync(user),
        error => _auditService.LogErrorAsync(error));
```


## MatchAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Error&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs#L915" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction` only if the asynchronous operation `resultTask` represents a successful instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, and returns a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) returned by the function; or executes the provided asynchronous `ifErrorAction` otherwise.

```csharp
public static async ValueTask<Result> MatchAsync<T>(
    this ValueTask<Result<T>> resultTask,
    Func<T, ValueTask<Error>> ifSuccessFunction,
    Func<Error, ValueTask> ifErrorAction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result, passed to the `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../../ResultT/ResultT.md)&gt;
The asynchronous operation that represents an instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Error](../../Error/Error.md)&gt;&gt;
The asynchronous function to be executed only if the `resultTask` represents a successful operation. The value of the result is passed as a parameter. It returns the [Error](../../Error/Error.md) of the failed result.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed only if the `resultTask` represents a failed operation. The [Error](../../Error/Error.md) instance will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The value task object representing the asynchronous operation: a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) returned by `ifSuccessFunction` if the `resultTask` is successful; otherwise, a failed [Result](../../Result/Result.md) with the original [Error](../../Error/Error.md). The returned result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` or `ifErrorAction` parameter is `null`, or `ifSuccessFunction` returns `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
Without this overload, an asynchronous function returning an [Error](../../Error/Error.md) would return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `MatchAsync<T, Error>(…)`.

### Examples
```csharp
var result = await _userService.GetUserAsync(userId)
    .MatchAsync(
        async (user) => await _policyService.GetDeletionErrorAsync(user),
        async (error) => await _auditService.LogErrorAsync(error));
```


## MatchAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;TNext&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs#L846" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the asynchronous operation `resultTask` represents a successful instance of [Result&lt;T&gt;](../../ResultT/ResultT.md); or the provided asynchronous `ifErrorAction`, otherwise.

```csharp
public static async ValueTask<Result<TNext>> MatchAsync<T, TNext>(
    this ValueTask<Result<T>> resultTask,
    Func<T, ValueTask<TNext>> ifSuccessFunction,
    Func<Error, ValueTask> ifErrorAction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `resultTask` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;TNext&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.
The value of type `T` contained in the [Result&lt;T&gt;](../../ResultT/ResultT.md) outcome of `resultTask` will be passed as a parameter to this function.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed, only if the `resultTask` represents a failed operation. The [Error](../../Error/Error.md) instance contained in the outcome of the `resultTask` will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../Result/Result.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessAction` or `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `MatchAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .MatchAsync(
        user => _companyService.AddUserAsync(user),
        error => _auditService.LogErrorAsync(error));
```


## MatchAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;TNext&gt;&gt;, Func&lt;Error, ValueTask&lt;TNext&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs#L972" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), or the provided asynchronous `ifErrorFunction` otherwise, as an asynchronous operation.

```csharp
public static async ValueTask<TNext> MatchAsync<T, TNext>(
    this ValueTask<Result<T>> resultTask,
    Func<T, ValueTask<TNext>> ifSuccessFunction,
    Func<Error, ValueTask<TNext>> ifErrorFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `resultTask` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the value returned by the asynchronous `ifSuccessFunction` and `ifErrorFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;TNext&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.
The value of type `T` contained in the [Result&lt;T&gt;](../../ResultT/ResultT.md) outcome of `resultTask` will be passed as a parameter to this function.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;TNext&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a failed operation. The [Error](../../Error/Error.md) instance contained in the outcome of the `resultTask` will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;TNext&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessAction` or `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `MatchAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .MatchAsync(
        async (user) =>
        {
            await _companyService.GetCompanyByIdAsync(user.CompanyId);
            return _mapper.Map(user);
        },
        async (error) => ValueTask.FromResult(_mapper.Map(error));
```


## MatchAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&lt;TNext&gt;&gt;&gt;, Func&lt;Error, ValueTask&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs#L1028" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the asynchronous operation `resultTask` represents a successful instance of [Result&lt;T&gt;](../../ResultT/ResultT.md); or the provided asynchronous `ifErrorAction`, otherwise.

```csharp
public static async ValueTask<Result<TNext>> MatchAsync<T, TNext>(
    this ValueTask<Result<T>> resultTask,
    Func<T, ValueTask<Result<TNext>>> ifSuccessFunction,
    Func<Error, ValueTask> ifErrorAction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `resultTask` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the [Result&lt;TNext&gt;](../../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../ResultT/ResultT.md)&lt;TNext&gt;&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.
The value of type `T` contained in the [Result&lt;T&gt;](../../ResultT/ResultT.md) outcome of `resultTask` will be passed as a parameter to this function.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed, only if the `resultTask` represents a failed operation. The [Error](../../Error/Error.md) instance contained in the outcome of the `resultTask` will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessAction` or `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `MatchAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .MatchAsync(
        user => _companyService.GetCompanyByIdAsync(user.CompanyId),
        error => _auditService.LogErrorAsync(error));
```


## MatchAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&lt;TNext&gt;&gt;&gt;, Func&lt;Error, ValueTask&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/MatchValueTaskAsyncExtensions.cs#L1084" target="_blank">MatchValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the asynchronous operation `resultTask` represents a successful instance of [Result&lt;T&gt;](../../ResultT/ResultT.md); or the provided asynchronous `ifErrorFunction`, otherwise.

```csharp
public static async ValueTask<Result<TNext>> MatchAsync<T, TNext>(
    this ValueTask<Result<T>> resultTask,
    Func<T, ValueTask<Result<TNext>>> ifSuccessFunction,
    Func<Error, ValueTask<Result<TNext>>> ifErrorFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `resultTask` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the [Result&lt;TNext&gt;](../../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction` and `ifErrorFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../ResultT/ResultT.md)&lt;TNext&gt;&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.
The value of type `T` contained in the [Result&lt;T&gt;](../../ResultT/ResultT.md) outcome of `resultTask` will be passed as a parameter to this function.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../../Error/Error.md), [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../ResultT/ResultT.md)&lt;TNext&gt;&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a failed operation. The [Error](../../Error/Error.md) instance contained in the outcome of the `resultTask` will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessAction` or `ifErrorAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `MatchAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .MatchAsync(
        user => _companyService.GetCompanyByIdAsync(user.CompanyId),
        async (error) =>
        {
            await _auditService.LogErrorAsync(error);
            return _mapper.Map(error);
        });
```


## See Also
* [Error](../../Error/Error.md)
* [Result](../../Result/Result.md)
* [Result&lt;T&gt;](../../ResultT/ResultT.md)
