# MatchAsyncExtensions.MatchAsync Methods
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>

Runs a specific asynchronous action or function depending on whether the instance of [Result](../Result/Result.md) and [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful or a failed operation, as an asynchronous operation.

## Overloads
| Name                             | Description                                                |
| -------------------------------- | ---------------------------------------------------------- |
| [MatchAsync(Result, Func&lt;Task&gt;, Func&lt;Error, Task&gt;, Boolean)](#matchasyncresult-functask-funcerror-task-boolean) | Executes one of the specified asynchronous actions as an asynchronous operation depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [MatchAsync(Result, Func&lt;Task&lt;Result&gt;&gt;, Func&lt;Error, Task&gt;, Boolean)](#matchasyncresult-functaskresult-funcerror-task-boolean) | Executes the specified asynchronous function or action as an asynchronous operation depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [MatchAsync(Result, Func&lt;Task&lt;Result&gt;&gt;, Func&lt;Error, Task&lt;Result&gt;&gt;, Boolean)](#matchasyncresult-functaskresult-funcerror-taskresult-boolean) | Executes one of the specified asynchronous functions as an asynchronous operation depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(Result, Func&lt;Task&lt;T&gt;&gt;, Func&lt;Error, Task&gt;, Boolean)](#matchasynctresult-functaskt-funcerror-task-boolean) | Executes the specified asynchronous function or action as an asynchronous operation depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(Result, Func&lt;Task&lt;T&gt;&gt;, Func&lt;Error, Task&lt;T&gt;&gt;, Boolean)](#matchasynctresult-functaskt-funcerror-taskt-boolean) | Executes one of the specified asynchronous functions as an asynchronous operation depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(Result, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Func&lt;Error, Task&gt;, Boolean)](#matchasynctresult-functaskresultt-funcerror-task-boolean) | Executes the specified asynchronous function or action as an asynchronous operation depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(Result, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Func&lt;Error, Task&lt;Result&lt;T&gt;&gt;&gt;, Boolean)](#matchasynctresult-functaskresultt-funcerror-taskresultt-boolean) | Executes one of the specified asynchronous functions as an asynchronous operation depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(Result&lt;T&gt; result, Func&lt;T, Task&gt;, Func&lt;Error, Task&gt;, Boolean)](#matchasynctresultt-result-funct-task-funcerror-task-boolean) |  Executes one of the specified asynchronous actions as an asynchronous operation depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |
| [MatchAsync&lt;T, TNext&gt;(Result&lt;T&gt; result, Func&lt;T, Task&lt;TNext&gt;&gt;, Func&lt;Error, Task&gt;, Boolean)](#matchasynct-tnextresultt-result-funct-tasktnext-funcerror-task-boolean) |  Executes the specified asynchronous function or action as an asynchronous operation depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |
| [MatchAsync&lt;T, TNext&gt;(Result&lt;T&gt; result, Func&lt;T, Task&lt;TNext&gt;&gt;, Func&lt;Error, Task&lt;TNext&gt;&gt;, Boolean)](#matchasynct-tnextresultt-result-funct-tasktnext-funcerror-tasktnext-boolean) |  Executes one of the specified asynchronous functions as an asynchronous operation depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |
| [MatchAsync&lt;T, TNext&gt;(Result&lt;T&gt; result, Func&lt;T, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Func&lt;Error, Task&gt;, Boolean)](#matchasynct-tnextresultt-result-funct-taskresulttnext-funcerror-task-boolean) |  Executes the specified asynchronous function or action as an asynchronous operation depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |
| [MatchAsync&lt;T, TNext&gt;(Result&lt;T&gt; result, Func&lt;T, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Func&lt;Error, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)](#matchasynct-tnextresultt-result-funct-taskresulttnext-funcerror-taskresulttnext-boolean) |  Executes one of the specified asynchronous functions as an asynchronous operation depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |

## Remarks

> [!NOTE]
> When an asynchronous method awaits a [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task) directly, continuation usually occurs in the same thread that created the task, depending on the async context. This behavior can be costly in terms of performance and can result in a deadlock on the UI thread. To avoid these problems, the `Task.ConfigureAwait(false)` is being called; unless the caller explicitly requires the original context, then set the `continueOnCapturedContext` parameter to `true`.
> 
> For more information, see <a href="https://devblogs.microsoft.com/dotnet/configureawait-faq/" target="_blank">ConfigureAwait FAQ</a>.


## MatchAsync(Result, Func&lt;Task&gt;, Func&lt;Error, Task&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/MatchAsyncExtensions.cs#L60" target="_blank">MatchAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessAction`, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), or the provided asynchronous `ifErrorAction` otherwise, as an asynchronous operation.

```csharp
public static async Task<Result> MatchAsync(this Result result, Func<Task> ifSuccessAction,
    Func<Error, Task> ifErrorAction, bool continueOnCapturedContext = false);
```

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;
The asynchronous action to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;
The asynchronous action to be executed, only if the current instance of [Result](../Result/Result.md) represents a failed operation, as an asynchronous operation. The [Error](../Error/Error.md) instance contained in the `result` will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessAction` or `ifErrorAction` parameter is `null`.

## Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

await createUserResult.MatchAsync(
    () => _auditService.LogUserAddedAsync(userData),
    error => _auditService.LogError(error));
```


## MatchAsync(Result, Func&lt;Task&lt;Result&gt;&gt;, Func&lt;Error, Task&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/MatchAsyncExtensions.cs#L103" target="_blank">MatchAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), or the provided asynchronous `ifErrorAction` otherwise, as an asynchronous operation.

```csharp
public static async Task<Result> MatchAsync(this Result result, Func<Task<Result>> ifSuccessFunction,
    Func<Error, Task> ifErrorAction, bool continueOnCapturedContext = false);
```

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;
The asynchronous action to be executed, only if the current instance of [Result](../Result/Result.md) represents a failed operation, as an asynchronous operation. The [Error](../Error/Error.md) instance contained in the `result` will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorAction` parameter is `null`.

## Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var userAddedResult = await createUserResult.MatchAsync(
    () => _companyService.AddUserAsync(userData),
    error => _auditService.LogError(error));
```


## MatchAsync(Result, Func&lt;Task&lt;Result&gt;&gt;, Func&lt;Error, Task&lt;Result&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/MatchAsyncExtensions.cs#L145" target="_blank">MatchAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), or the provided asynchronous `ifErrorFunction` otherwise, as an asynchronous operation.

```csharp
public static async Task<Result> MatchAsync(this Result result, Func<Task<Result>> ifSuccessFunction,
    Func<Error, Task<Result>> ifErrorFunction, bool continueOnCapturedContext = false);
```

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../Result/Result.md) represents a failed operation, as an asynchronous operation. The [Error](../Error/Error.md) instance contained in the `result` will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorFunction` parameter is `null`.

## Examples
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


## MatchAsync&lt;T&gt;(Result, Func&lt;Task&lt;T&gt;&gt;, Func&lt;Error, Task&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/MatchAsyncExtensions.cs#L190" target="_blank">MatchAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), or the provided asynchronous `ifErrorAction` otherwise, as an asynchronous operation.

```csharp
public static async Task<Result<T>> MatchAsync<T>(this Result result, Func<Task<T>> ifSuccessFunction,
    Func<Error, Task> ifErrorAction, bool continueOnCapturedContext = false);
```

### Type Parameters
#### `T`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;T&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;
The asynchronous action to be executed, only if the current instance of [Result](../Result/Result.md) represents a failed operation, as an asynchronous operation. The [Error](../Error/Error.md) instance contained in the `result` will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorAction` parameter is `null`.

## Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.MatchAsync(
    async (user) => await _loginService.GetUserTokenAsync(user),
    async (error) => await _auditService.LogErrorAsync(error));
```


## MatchAsync&lt;T&gt;(Result, Func&lt;Task&lt;T&gt;&gt;, Func&lt;Error, Task&lt;T&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/MatchAsyncExtensions.cs#L236" target="_blank">MatchAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), or the provided asynchronous `ifErrorFunction` otherwise, as an asynchronous operation.

```csharp
public static async Task<T> MatchAsync<T>(this Result result, Func<Task<T>> ifSuccessFunction,
    Func<Error, Task<T>> ifErrorFunction, bool continueOnCapturedContext = false);
```

### Type Parameters
#### `T`
The type of the value returned by the asynchronous `ifSuccessFunction` and `ifErrorFunction`.

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;T&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;T&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../Result/Result.md) represents a failed operation, as an asynchronous operation. The [Error](../Error/Error.md) instance contained in the `result` will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;T&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorFunction` parameter is `null`.

## Examples
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


## MatchAsync&lt;T&gt;(Result, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Func&lt;Error, Task&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/MatchAsyncExtensions.cs#L281" target="_blank">MatchAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), or the provided asynchronous `ifErrorAction` otherwise, as an asynchronous operation.

```csharp
public static async Task<Result<T>> MatchAsync<T>(this Result result, Func<Task<Result<T>>> ifSuccessFunction,
    Func<Error, Task> ifErrorAction, bool continueOnCapturedContext = false);
```

### Type Parameters
#### `T`
The type of the [Result&lt;T&gt;](../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;
The asynchronous action to be executed, only if the current instance of [Result](../Result/Result.md) represents a failed operation, as an asynchronous operation. The [Error](../Error/Error.md) instance contained in the `result` will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorAction` parameter is `null`.

## Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var userDetailsResult = await createUserResult.MatchAsync(
    user => _userService.GetUserDetailsAsync(user),
    error => Task.FromResult(_userMapper.MapErrorViewModel(error)));
```


## MatchAsync&lt;T&gt;(Result, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Func&lt;Error, Task&lt;Result&lt;T&gt;&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/MatchAsyncExtensions.cs#L327" target="_blank">MatchAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), or the provided asynchronous `ifErrorFunction` otherwise, as an asynchronous operation.

```csharp
public static async Task<Result<T>> MatchAsync<T>(this Result result, Func<Task<Result<T>>> ifSuccessFunction,
    Func<Error, Task<Result<T>>> ifErrorFunction, bool continueOnCapturedContext = false);
```

### Type Parameters
#### `T`
The type of the [Result&lt;T&gt;](../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction` and `ifErrorFunction`.

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../Result/Result.md) represents a failed operation, as an asynchronous operation. The [Error](../Error/Error.md) instance contained in the `result` will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorFunction` parameter is `null`.

## Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var userDetailsResult = await createUserResult.MatchAsync(
    user => _userService.GetUserDetailsAsync(user),
    error => _userMapper.MapErrorViewModel(error));
```


## MatchAsync&lt;T&gt;(Result&lt;T&gt; result, Func&lt;T, Task&gt;, Func&lt;Error, Task&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/MatchAsyncExtensions.cs#L376" target="_blank">MatchAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessAction`, only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), or the provided asynchronous `ifErrorAction` otherwise, as an asynchronous operation.

```csharp
public static async Task<Result<T>> MatchAsync<T>(this Result<T> result, Func<T, Task> ifSuccessAction,
    Func<Error, Task> ifErrorAction, bool continueOnCapturedContext = false);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the asynchronous `ifSuccessAction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation with a value to be evaluated.

#### `ifSuccessAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;
The asynchronous action to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this action.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;
The asynchronous action to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a failed operation, as an asynchronous operation. The [Error](../Error/Error.md) instance contained in the `result` will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessAction` or `ifErrorAction` parameter is `null`.

## Examples
```csharp
var userResult = await _userService.CreateUserAsync(userData);

await userResult.MatchAsync(
    user => _auditService.LogUserAddedAsync(user),
    error => _auditService.LogErrorAsync(error));
```


## MatchAsync&lt;T, TNext&gt;(Result&lt;T&gt; result, Func&lt;T, Task&lt;TNext&gt;&gt;, Func&lt;Error, Task&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/MatchAsyncExtensions.cs#L426" target="_blank">MatchAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), or the provided asynchronous `ifErrorAction` otherwise, as an asynchronous operation.

```csharp
public static async Task<Result<TNext>> MatchAsync<T, TNext>(
    this Result<T> result,
    Func<T, Task<TNext>> ifSuccessFunction,
    Func<Error, Task> ifErrorAction,
    bool continueOnCapturedContext = false);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation with a value to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;TNext&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this function.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;
The asynchronous action to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a failed operation, as an asynchronous operation. The [Error](../Error/Error.md) instance contained in the `result` will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../Result/Result.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorAction` parameter is `null`.

## Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var userAddedResult = await createUserResult.MatchAsync(
    user => _companyService.AddUserAsync(user),
    error => _auditService.LogErrorAsync(error));
```


## MatchAsync&lt;T, TNext&gt;(Result&lt;T&gt; result, Func&lt;T, Task&lt;TNext&gt;&gt;, Func&lt;Error, Task&lt;TNext&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/MatchAsyncExtensions.cs#L477" target="_blank">MatchAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), or the provided asynchronous `ifErrorFunction` otherwise, as an asynchronous operation.

```csharp
public static async Task<TNext> MatchAsync<T, TNext>(
    this Result<T> result,
    Func<T, Task<TNext>> ifSuccessFunction,
    Func<Error, Task<TNext>> ifErrorFunction,
    bool continueOnCapturedContext = false);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the value returned by the asynchronous `ifSuccessFunction` and `ifErrorFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation with a value to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;TNext&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this function.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;TNext&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a failed operation, as an asynchronous operation. The [Error](../Error/Error.md) instance contained in the `result` will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;TNext&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorFunction` parameter is `null`.

## Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.MatchAsync(
    async (user) =>
    {
        await _companyService.GetCompanyByIdAsync(user.CompanyId);
        return _mapper.Map(user);
    },
    async (error) => Task.FromResult(_mapper.Map(error));
```


## MatchAsync&lt;T, TNext&gt;(Result&lt;T&gt; result, Func&lt;T, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Func&lt;Error, Task&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/MatchAsyncExtensions.cs#L527" target="_blank">MatchAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), or the provided asynchronous `ifErrorAction` otherwise, as an asynchronous operation.

```csharp
public static async Task<Result<TNext>> MatchAsync<T, TNext>(
    this Result<T> result,
    Func<T, Task<Result<TNext>>> ifSuccessFunction,
    Func<Error, Task> ifErrorAction,
    bool continueOnCapturedContext = false);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the [Result&lt;TNext&gt;](../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this function.

#### `ifErrorAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;
The asynchronous action to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a failed operation, as an asynchronous operation. The [Error](../Error/Error.md) instance contained in the `result` will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorAction` parameter is `null`.

## Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.MatchAsync(
    user => _companyService.GetCompanyByIdAsync(user.CompanyId),
    error => _auditService.LogErrorAsync(error));
```


## MatchAsync&lt;T, TNext&gt;(Result&lt;T&gt; result, Func&lt;T, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Func&lt;Error, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/MatchAsyncExtensions.cs#L578" target="_blank">MatchAsyncExtensions.cs</a>

Executes the provided asynchronous `ifSuccessFunction`, only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), or the provided asynchronous `ifErrorFunction` otherwise, as an asynchronous operation.

```csharp
public static async Task<Result<TNext>> MatchAsync<T, TNext>(
    this Result<T> result,
    Func<T, Task<Result<TNext>>> ifSuccessFunction,
    Func<Error, Task<Result<TNext>>> ifErrorFunction,
    bool continueOnCapturedContext = false);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the [Result&lt;TNext&gt;](../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction` and `ifErrorFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this function.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a failed operation, as an asynchronous operation. The [Error](../Error/Error.md) instance contained in the `result` will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorAction` parameter is `null`.

## Examples
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


## See Also
* [Error](../Error/Error.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
