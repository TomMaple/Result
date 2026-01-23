# IfSuccessAsyncExtensions.IfSuccessAsync Methods
## Definition
Namespace: [Maple.Result.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Runs a specific asynchronous action or function, if the instance of [Result](../Result/Result.md) and [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), as an asynchronous operation.

## Overloads
| Name                             | Description                                                |
| -------------------------------- | ---------------------------------------------------------- |
| [IfSuccessAsync(Result, Func&lt;Task&gt;, Boolean)](#ifsuccessasyncresult-functask-boolean) | Executes the specified asynchronous action, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync(Result, Func&lt;Task&lt;Result&gt;&gt;, Boolean)](#ifsuccessasyncresult-functaskresult-boolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(Result, Func&lt;Task&lt;T&gt;&gt;, Boolean)](#ifsuccessasynctresult-functaskt-boolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(Result, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Boolean)](#ifsuccessasynctresult-functaskresultt-boolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Task&gt;, Boolean)](#ifsuccessasynctresultt-funct-task-boolean) | Executes the specified asynchronous action, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Boolean)](#ifsuccessasynctresultt-funct-taskresult-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;TNext&gt;&gt;, Boolean)](#ifsuccessasynct-tnextresultt-funct-tasktnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)](#ifsuccessasynct-tnextresultt-funct-taskresulttnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |

## Remarks

> [!NOTE]
> When an asynchronous method awaits a [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task) directly, continuation usually occurs in the same thread that created the task, depending on the async context. This behavior can be costly in terms of performance and can result in a deadlock on the UI thread. To avoid these problems, the `Task.ConfigureAwait(false)` is being called; unless the caller explicitly requires the original context, then set the `continueOnCapturedContext` parameter to `true`.
> 
> For more information, see <a href="https://devblogs.microsoft.com/dotnet/configureawait-faq/" target="_blank">ConfigureAwait FAQ</a>.


## IfSuccessAsync(Result, Func&lt;Task&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/IfSuccessAsyncExtensions.cs#L55" target="_blank">IfSuccessAsyncExtensions.cs</a>

Executes the provided asynchronous action, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
public static async Task<Result> IfSuccessAsync(this Result result, Func<Task> ifSuccessAction,
    bool continueOnCapturedContext = false);
```

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;
The asynchronous action to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessAction` parameter is `null`.

## Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

await createUserResult.IfSuccessAsync(async () =>
{
    await _auditService.LogUserAddedAsync(userData);
});
```


## IfSuccessAsync(Result, Func&lt;Task&lt;Result&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/IfSuccessAsyncExtensions.cs#L95" target="_blank">IfSuccessAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
public static async Task<Result> IfSuccessAsync(this Result result, Func<Task<Result>> ifSuccessFunction,
    bool continueOnCapturedContext = false);
```

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

## Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var userAddedResult = await createUserResult.IfSuccessAsync(async () =>
{
    await _companyService.AddUserAsync(userData);
});
```


## IfSuccessAsync&lt;T&gt;(Result, Func&lt;Task&lt;T&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/IfSuccessAsyncExtensions.cs#L138" target="_blank">IfSuccessAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
public static async Task<Result<T>> IfSuccessAsync<T>(this Result result, Func<Task<T>> ifSuccessFunction,
    bool continueOnCapturedContext = false)
```

### Type Parameters
#### `T`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;T&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

## Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.IfSuccessAsync(async () =>
{
    return await _companyService.GetCompanyByIdAsync(userData.CompanyId);
});
```


## IfSuccessAsync&lt;T&gt;(Result, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/IfSuccessAsyncExtensions.cs#L181" target="_blank">IfSuccessAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
public static async Task<Result<T>> IfSuccessAsync<T>(this Result result, Func<Task<Result<T>>> ifSuccessFunction,
    bool continueOnCapturedContext = false);
```

### Type Parameters
#### `T`
The type of the [Result&lt;T&gt;](../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

## Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.IfSuccessAsync(async () =>
{
    return await _companyService.GetCompanyByIdAsync(userData.CompanyId);
});
```


## IfSuccessAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Task&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/IfSuccessAsyncExtensions.cs#L223" target="_blank">IfSuccessAsyncExtensions.cs</a>

Executes the provided asynchronous action, only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
public static async Task<Result<T>> IfSuccessAsync<T>(this Result<T> result, Func<T, Task> ifSuccessAction,
    bool continueOnCapturedContext = false);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation with a value to be evaluated.

#### `ifSuccessAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&gt;
The asynchronous action to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessAction` parameter is `null`.

## Examples
```csharp
var userResult = await _userService.CreateUserAsync(userData);

await userResult.IfSuccessAsync(async (user) =>
{
    await _auditService.LogUserAddedAsync(user);
});
```


## IfSuccessAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/IfSuccessAsyncExtensions.cs#L263" target="_blank">IfSuccessAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
public static async Task<Result> IfSuccessAsync<T>(this Result<T> result, Func<T, Task<Result>> ifSuccessFunction,
    bool continueOnCapturedContext = false);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation with a value to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

## Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var userAddedResult = await createUserResult.IfSuccessAsync(async (user) =>
{
    return await _companyService.AddUserAsync(user);
});
```


## IfSuccessAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;TNext&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/IfSuccessAsyncExtensions.cs#L313" target="_blank">IfSuccessAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
public static async Task<Result<TNext>> IfSuccessAsync<T, TNext>(this Result<T> result,
    Func<T, Task<TNext>> ifSuccessFunction, bool continueOnCapturedContext = false);
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

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

## Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.IfSuccessAsync(async (user) =>
{
    return await _companyService.GetCompanyByIdAsync(user.CompanyId);
});
```


## IfSuccessAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/IfSuccessAsyncExtensions.cs#L363" target="_blank">IfSuccessAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), as an asynchronous operation.

```csharp
public static async Task<Result<TNext>> IfSuccessAsync<T, TNext>(this Result<T> result,
    Func<T, Task<Result<TNext>>> ifSuccessFunction, bool continueOnCapturedContext = false);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the [Result&lt;TNext&gt;](../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Task](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)
`true` to attempt to marshal the continuation back to the original context captured; otherwise, `false`.

The default value is: `false`.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

## Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.IfSuccessAsync(async (user) =>
{
    return await _companyService.GetCompanyByIdAsync(user.CompanyId);
});
```


## See Also
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
