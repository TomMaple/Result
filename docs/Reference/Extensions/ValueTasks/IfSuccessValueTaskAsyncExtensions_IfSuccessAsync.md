# IfSuccessValueTaskAsyncExtensions.IfSuccessAsync Methods
## Definition
Namespace: [Maple.Result.Extensions.ValueTasks](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Runs a specific asynchronous action or function, if the instance of [Result](../../Result/Result.md) and [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), as an asynchronous operation.

## Overloads
| Name | Description |
| ---- | ----------- |
| [IfSuccessAsync(Result, Func&lt;ValueTask&gt;, Boolean)](#ifsuccessasyncresult-funcvaluetask-boolean) | Executes the specified asynchronous action, if the [Result](../../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync(Result, Func&lt;ValueTask&lt;Result&gt;&gt;, Boolean)](#ifsuccessasyncresult-funcvaluetaskresult-boolean) | Executes the specified asynchronous function, if the [Result](../../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync(Result, Func&lt;ValueTask&lt;Error&gt;&gt;, Boolean)](#ifsuccessasyncresult-funcvaluetaskerror-boolean) | Executes the specified asynchronous function, if the [Result](../../Result/Result.md) represents a successful operation, and returns a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) it returns. |
| [IfSuccessAsync&lt;T&gt;(Result, Func&lt;ValueTask&lt;T&gt;&gt;, Boolean)](#ifsuccessasynctresult-funcvaluetaskt-boolean) | Executes the specified asynchronous function, if the [Result](../../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(Result, Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Boolean)](#ifsuccessasynctresult-funcvaluetaskresultt-boolean) | Executes the specified asynchronous function, if the [Result](../../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&gt;, Boolean)](#ifsuccessasynctresultt-funct-valuetask-boolean) | Executes the specified asynchronous action, if the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Boolean)](#ifsuccessasynctresultt-funct-valuetaskresult-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Error&gt;&gt;, Boolean)](#ifsuccessasynctresultt-funct-valuetaskerror-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, and returns a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) it returns. |
| [IfSuccessAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;TNext&gt;&gt;, Boolean)](#ifsuccessasynct-tnextresultt-funct-valuetasktnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)](#ifsuccessasynct-tnextresultt-funct-valuetaskresulttnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&gt;, Boolean)](#ifsuccessasyncvaluetaskresult-funcvaluetask-boolean) | Executes the specified asynchronous action, if the provided asynchronous operation returns a successful [Result](../../Result/Result.md), as an asynchronous operation. |
| [IfSuccessAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Result&gt;&gt;, Boolean)](#ifsuccessasyncvaluetaskresult-funcvaluetaskresult-boolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../../Result/Result.md), as an asynchronous operation. |
| [IfSuccessAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Error&gt;&gt;, Boolean)](#ifsuccessasyncvaluetaskresult-funcvaluetaskerror-boolean) | Executes the specified asynchronous function, if the asynchronous operation `resultTask` represents a successful [Result](../../Result/Result.md), and returns a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) it returns. |
| [IfSuccessAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;T&gt;&gt;, Boolean)](#ifsuccessasynctvaluetaskresult-funcvaluetaskt-boolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../../Result/Result.md), as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Boolean)](#ifsuccessasynctvaluetaskresult-funcvaluetaskresultt-boolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../../Result/Result.md), as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&gt;, Boolean)](#ifsuccessasynctvaluetaskresultt-funct-valuetask-boolean) | Executes the specified asynchronous action, if the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Boolean)](#ifsuccessasynctvaluetaskresultt-funct-valuetaskresult-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Error&gt;&gt;, Boolean)](#ifsuccessasynctvaluetaskresultt-funct-valuetaskerror-boolean) | Executes the specified asynchronous function, if the asynchronous operation `resultTask` represents a successful [Result&lt;T&gt;](../../ResultT/ResultT.md), and returns a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) it returns. |
| [IfSuccessAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;TNext&gt;&gt;, Boolean)](#ifsuccessasynct-tnextvaluetaskresultt-funct-valuetasktnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)](#ifsuccessasynct-tnextvaluetaskresultt-funct-valuetaskresulttnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |

## Remarks

> [!NOTE]
> When an asynchronous method awaits a [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask) directly, continuation usually occurs in the same thread that created the task, depending on the async context. This behavior can be costly in terms of performance and can result in a deadlock on the UI thread. To avoid these problems, `ConfigureAwait(false)` is used by default, so in the methods that take a `ValueTask` of a result the passed delegates run on a thread-pool thread. When the delegates need the original context (e.g., to update UI elements), set the `continueOnCapturedContext` parameter to `true`, or set [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) to `true` once at startup.
> 
> For more information, see <a href="https://devblogs.microsoft.com/dotnet/configureawait-faq/" target="_blank">ConfigureAwait FAQ</a>.

> [!NOTE]
> A [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask) can only be awaited once. Because of this, the overloads accepting `this ValueTask<TResult> resultTask` consume the passed operation as part of evaluating its result, and they do not perform a `null` check on it (a `ValueTask<TResult>` is a non-nullable value type).


## IfSuccessAsync(Result, Func&lt;ValueTask&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/IfSuccessValueTaskAsyncExtensions.cs#L38" target="_blank">IfSuccessValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous action, only if the instance of [Result](../../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), as an asynchronous operation.

```csharp
public static async ValueTask<Result> IfSuccessAsync(this Result result, Func<ValueTask> ifSuccessAction,
    bool? continueOnCapturedContext = null);
```

### Parameters
#### `result` [Result](../../Result/Result.md)
The instance of [Result](../../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed, only if the current instance of [Result](../../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessAction` parameter is `null`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

await createUserResult.IfSuccessAsync(async () =>
{
    await _auditService.LogUserAddedAsync(userData);
});
```


## IfSuccessAsync(Result, Func&lt;ValueTask&lt;Result&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/IfSuccessValueTaskAsyncExtensions.cs#L74" target="_blank">IfSuccessValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of [Result](../../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), as an asynchronous operation.

```csharp
public static async ValueTask<Result> IfSuccessAsync(this Result result, Func<ValueTask<Result>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Parameters
#### `result` [Result](../../Result/Result.md)
The instance of [Result](../../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var userAddedResult = await createUserResult.IfSuccessAsync(async () =>
{
    await _companyService.AddUserAsync(userData);
});
```


## IfSuccessAsync(Result, Func&lt;ValueTask&lt;Error&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/IfSuccessValueTaskAsyncExtensions.cs#L117" target="_blank">IfSuccessValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function only if the [Result](../../Result/Result.md) represents a successful operation, and returns a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) returned by the function.

```csharp
public static async ValueTask<Result> IfSuccessAsync(this Result result, Func<ValueTask<Error>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Parameters
#### `result` [Result](../../Result/Result.md)
The instance of [Result](../../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Error](../../Error/Error.md)&gt;&gt;
The asynchronous function to be executed only if the `result` represents a successful operation. It returns the [Error](../../Error/Error.md) of the failed result.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) returned by `ifSuccessFunction` if the result is successful; otherwise, a failed [Result](../../Result/Result.md) with the original [Error](../../Error/Error.md). The result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` parameter is `null`, or `ifSuccessFunction` returns `null`.

### Remarks
Without this overload, an asynchronous function returning an [Error](../../Error/Error.md) would return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `IfSuccessAsync<Error>(…)`.

### Examples
```csharp
var result = await _userService.DeleteUser(userId)
    .IfSuccessAsync(async () => await _policyService.GetDeletionErrorAsync(userId));
```


## IfSuccessAsync&lt;T&gt;(Result, Func&lt;ValueTask&lt;T&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/IfSuccessValueTaskAsyncExtensions.cs#L139" target="_blank">IfSuccessValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of [Result](../../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), as an asynchronous operation.

```csharp
public static async ValueTask<Result<T>> IfSuccessAsync<T>(this Result result, Func<ValueTask<T>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result](../../Result/Result.md)
The instance of [Result](../../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;T&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.IfSuccessAsync(async () =>
{
    return await _companyService.GetCompanyByIdAsync(userData.CompanyId);
});
```


## IfSuccessAsync&lt;T&gt;(Result, Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/IfSuccessValueTaskAsyncExtensions.cs#L156" target="_blank">IfSuccessValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of [Result](../../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), as an asynchronous operation.

```csharp
public static async ValueTask<Result<T>> IfSuccessAsync<T>(this Result result, Func<ValueTask<Result<T>>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the [Result&lt;T&gt;](../../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result](../../Result/Result.md)
The instance of [Result](../../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result](../../Result/Result.md) represents a successful operation, as an asynchronous operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.IfSuccessAsync(async () =>
{
    return await _companyService.GetCompanyByIdAsync(userData.CompanyId);
});
```


## IfSuccessAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/IfSuccessValueTaskAsyncExtensions.cs#L176" target="_blank">IfSuccessValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous action, only if the instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), as an asynchronous operation.

```csharp
public static async ValueTask<Result<T>> IfSuccessAsync<T>(this Result<T> result, Func<T, ValueTask> ifSuccessAction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) record that represents the result of the operation with a value to be evaluated.

#### `ifSuccessAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed, only if the current instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. The value of type `T` contained in the [Result&lt;T&gt;](../../ResultT/ResultT.md) instance will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessAction` parameter is `null`.

### Examples
```csharp
var userResult = await _userService.CreateUserAsync(userData);

await userResult.IfSuccessAsync(async (user) =>
{
    await _auditService.LogUserAddedAsync(user);
});
```


## IfSuccessAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/IfSuccessValueTaskAsyncExtensions.cs#L217" target="_blank">IfSuccessValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), as an asynchronous operation.

```csharp
public static async ValueTask<Result> IfSuccessAsync<T>(this Result<T> result, Func<T, ValueTask<Result>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) record that represents the result of the operation with a value to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. The value of type `T` contained in the [Result&lt;T&gt;](../../ResultT/ResultT.md) instance will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var userAddedResult = await createUserResult.IfSuccessAsync(async (user) =>
{
    return await _companyService.AddUserAsync(user);
});
```


## IfSuccessAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Error&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/IfSuccessValueTaskAsyncExtensions.cs#L264" target="_blank">IfSuccessValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function only if the [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, and returns a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) returned by the function.

```csharp
public static async ValueTask<Result> IfSuccessAsync<T>(this Result<T> result,
    Func<T, ValueTask<Error>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the `result`, passed to the `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Error](../../Error/Error.md)&gt;&gt;
The asynchronous function to be executed only if the `result` represents a successful operation. The value of the `result` is passed as a parameter. It returns the [Error](../../Error/Error.md) of the failed result.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) returned by `ifSuccessFunction` if the result is successful; otherwise, a failed [Result](../../Result/Result.md) with the original [Error](../../Error/Error.md). The result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` parameter is `null`, or `ifSuccessFunction` returns `null`.

### Remarks
Without this overload, an asynchronous function returning an [Error](../../Error/Error.md) would return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `IfSuccessAsync<T, Error>(…)`.

### Examples
```csharp
var result = await _userService.GetUser(userId)
    .IfSuccessAsync(async user => await _policyService.GetDeletionErrorAsync(user));
```


## IfSuccessAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;TNext&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/IfSuccessValueTaskAsyncExtensions.cs#L285" target="_blank">IfSuccessValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), as an asynchronous operation.

```csharp
public static async ValueTask<Result<TNext>> IfSuccessAsync<T, TNext>(this Result<T> result,
    Func<T, ValueTask<TNext>> ifSuccessFunction, bool? continueOnCapturedContext = null);
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

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.IfSuccessAsync(async (user) =>
{
    return await _companyService.GetCompanyByIdAsync(user.CompanyId);
});
```


## IfSuccessAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, ValueTask&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/IfSuccessValueTaskAsyncExtensions.cs#L330" target="_blank">IfSuccessValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../../Error/Error.md)), as an asynchronous operation.

```csharp
public static async ValueTask<Result<TNext>> IfSuccessAsync<T, TNext>(this Result<T> result,
    Func<T, ValueTask<Result<TNext>>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the [Result&lt;TNext&gt;](../../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous function to be executed, only if the current instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. The value of type `T` contained in the [Result&lt;T&gt;](../../ResultT/ResultT.md) instance will be passed as a parameter to this function.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData);

var companyResult = await createUserResult.IfSuccessAsync(async (user) =>
{
    return await _companyService.GetCompanyByIdAsync(user.CompanyId);
});
```


## IfSuccessAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/IfSuccessValueTaskAsyncExtensions.cs#L379" target="_blank">IfSuccessValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous action, only if the provided asynchronous operation `resultTask` returns a successful instance of [Result](../../Result/Result.md).

```csharp
public static async ValueTask<Result> IfSuccessAsync(this ValueTask<Result> resultTask, Func<ValueTask> ifSuccessAction,
    bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../../Result/Result.md) record to be evaluated.

#### `ifSuccessAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed, only if the `resultTask` represents a successful operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `IfSuccessAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .IfSuccessAsync(async () =>
    {
        await _auditService.LogUserAddedAsync(userData);
    });
```


## IfSuccessAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Result&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/IfSuccessValueTaskAsyncExtensions.cs#L428" target="_blank">IfSuccessValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result](../../Result/Result.md).

```csharp
public static async ValueTask<Result> IfSuccessAsync(this ValueTask<Result> resultTask, Func<ValueTask<Result>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `IfSuccessAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .IfSuccessAsync(async () =>
    {
        await _companyService.AddUserAsync(userData);
    });
```


## IfSuccessAsync(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Error&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/IfSuccessValueTaskAsyncExtensions.cs#L481" target="_blank">IfSuccessValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function only if the asynchronous operation `resultTask` represents a successful [Result](../../Result/Result.md), and returns a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) returned by the function.

```csharp
public static async ValueTask<Result> IfSuccessAsync(this ValueTask<Result> resultTask,
    Func<ValueTask<Error>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Error](../../Error/Error.md)&gt;&gt;
The asynchronous function to be executed only if the `resultTask` represents a successful operation. It returns the [Error](../../Error/Error.md) of the failed result.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) returned by `ifSuccessFunction` if the result is successful; otherwise, a failed [Result](../../Result/Result.md) with the original [Error](../../Error/Error.md). The result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` or `ifSuccessFunction` parameter is `null`, or `ifSuccessFunction` returns `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
Without this overload, an asynchronous function returning an [Error](../../Error/Error.md) would return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `IfSuccessAsync<Error>(…)`.

### Examples
```csharp
var result = await _userService.DeleteUserAsync(userId)
    .IfSuccessAsync(async () => await _policyService.GetDeletionErrorAsync(userId));
```


## IfSuccessAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;T&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/IfSuccessValueTaskAsyncExtensions.cs#L533" target="_blank">IfSuccessValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result](../../Result/Result.md).

```csharp
public static async ValueTask<Result<T>> IfSuccessAsync<T>(this ValueTask<Result> resultTask, Func<ValueTask<T>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;T&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `IfSuccessAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .IfSuccessAsync(async () =>
    {
        return await _companyService.GetCompanyByIdAsync(userData.CompanyId);
    });
```


## IfSuccessAsync&lt;T&gt;(ValueTask&lt;Result&gt;, Func&lt;ValueTask&lt;Result&lt;T&gt;&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/IfSuccessValueTaskAsyncExtensions.cs#L586" target="_blank">IfSuccessValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result](../../Result/Result.md).

```csharp
public static async ValueTask<Result<T>> IfSuccessAsync<T>(
    this ValueTask<Result> resultTask,
    Func<ValueTask<Result<T>>> ifSuccessFunction,
    bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the [Result&lt;T&gt;](../../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous function to be executed, only if the `resultTask` represents a successful operation.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `IfSuccessAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .IfSuccessAsync(async () =>
    {
        return await _companyService.GetCompanyByIdAsync(userData.CompanyId);
    });
```


## IfSuccessAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/IfSuccessValueTaskAsyncExtensions.cs#L645" target="_blank">IfSuccessValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result&lt;T&gt;](../../ResultT/ResultT.md).

```csharp
public static async ValueTask<Result<T>> IfSuccessAsync<T>(this ValueTask<Result<T>> resultTask,
    Func<T, ValueTask> ifSuccessAction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `resultTask` and passed to the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../../Result/Result.md) record to be evaluated.

#### `ifSuccessAction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&gt;
The asynchronous action to be executed, only if the `resultTask` represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../../ResultT/ResultT.md) instance will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessAction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `IfSuccessAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var userResult = await _userService.CreateUserAsync(userData)
    .IfSuccessAsync(async (user) =>
    {
        await _auditService.LogUserAddedAsync(user);
    });
```


## IfSuccessAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/IfSuccessValueTaskAsyncExtensions.cs#L699" target="_blank">IfSuccessValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result&lt;T&gt;](../../ResultT/ResultT.md).

```csharp
public static async ValueTask<Result> IfSuccessAsync<T>(this ValueTask<Result<T>> resultTask,
    Func<T, ValueTask<Result>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `resultTask` and passed to the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;&gt;
The asynchronous action to be executed, only if the `resultTask` represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../../ResultT/ResultT.md) instance will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../Result/Result.md)&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `IfSuccessAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .IfSuccessAsync(async (user) =>
    {
        return await _companyService.AddUserAsync(user);
    });
```


## IfSuccessAsync&lt;T&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Error&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/IfSuccessValueTaskAsyncExtensions.cs#L756" target="_blank">IfSuccessValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function only if the asynchronous operation `resultTask` represents a successful [Result&lt;T&gt;](../../ResultT/ResultT.md), and returns a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) returned by the function.

```csharp
public static async ValueTask<Result> IfSuccessAsync<T>(this ValueTask<Result<T>> resultTask,
    Func<T, ValueTask<Error>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value of the result of `resultTask`, passed to the `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../../ResultT/ResultT.md)&gt;
The asynchronous operation that represents an instance of [Result&lt;T&gt;](../../ResultT/ResultT.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Error](../../Error/Error.md)&gt;&gt;
The asynchronous function to be executed only if the `resultTask` represents a successful operation. The value of the result is passed as a parameter. It returns the [Error](../../Error/Error.md) of the failed result.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../../Result/Result.md) with the [Error](../../Error/Error.md) returned by `ifSuccessFunction` if the result is successful; otherwise, a failed [Result](../../Result/Result.md) with the original [Error](../../Error/Error.md). The result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `resultTask` or `ifSuccessFunction` parameter is `null`, or `ifSuccessFunction` returns `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
Without this overload, an asynchronous function returning an [Error](../../Error/Error.md) would return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `IfSuccessAsync<T, Error>(…)`.

### Examples
```csharp
var result = await _userService.GetUserAsync(userId)
    .IfSuccessAsync(async user => await _policyService.GetDeletionErrorAsync(user));
```


## IfSuccessAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;TNext&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/IfSuccessValueTaskAsyncExtensions.cs#L818" target="_blank">IfSuccessValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result&lt;T&gt;](../../ResultT/ResultT.md).

```csharp
public static async ValueTask<Result<TNext>> IfSuccessAsync<T, TNext>(this ValueTask<Result<T>> resultTask,
    Func<T, ValueTask<TNext>> ifSuccessFunction, bool? continueOnCapturedContext = null);
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
The asynchronous action to be executed, only if the `resultTask` represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../../ResultT/ResultT.md) instance will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `IfSuccessAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .IfSuccessAsync(async (user) =>
    {
        return await _companyService.GetCompanyByIdAsync(user.CompanyId);
    });
```


## IfSuccessAsync&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/ValueTasks/IfSuccessValueTaskAsyncExtensions.cs#L876" target="_blank">IfSuccessValueTaskAsyncExtensions.cs</a>

Executes the provided asynchronous function, only if the provided asynchronous operation `resultTask` represents a successful instance of [Result&lt;T&gt;](../../ResultT/ResultT.md).

```csharp
public static async ValueTask<Result<TNext>> IfSuccessAsync<T, TNext>(this ValueTask<Result<T>> resultTask,
    Func<T, ValueTask<Result<TNext>>> ifSuccessFunction, bool? continueOnCapturedContext = null);
```

### Type Parameters
#### `T`
The type of the value associated with successful `resultTask` and passed to the asynchronous `ifSuccessFunction`.

#### `TNext`
The type of the [Result&lt;TNext&gt;](../../ResultT/ResultT.md) value returned by the asynchronous `ifSuccessFunction`.

### Parameters
#### `resultTask` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../Result/Result.md)&gt;
The asynchronous operation that represents an instance of [Result](../../Result/Result.md) record to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[ValueTask](https://learn.microsoft.com/en-ca/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../ResultT/ResultT.md)&lt;T&gt;&gt;&gt;
The asynchronous action to be executed, only if the `resultTask` represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../../ResultT/ResultT.md) instance will be passed as a parameter to this action.

#### `continueOnCapturedContext` [Nullable](https://learn.microsoft.com/dotnet/api/system.nullable-1)&lt;[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)&gt;
`true` to run the continuations, including the passed delegates when they run after an `await`, on the captured context (e.g., the UI thread); `false` to run them on any thread; `null` to use [ResultConfiguration.ContinueOnCapturedContext](../../ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md) (`false` unless changed).

The default value is: `null`.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result](../../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `resultTask` returns `null`.

### Remarks
The `IfSuccessAsync` method executes the provided asynchronous operation represented by `resultTask` to evaluate its result. This enables chaining multiple asynchronous operations that return `ValueTask<TResult>`.

### Examples
```csharp
var createUserResult = await _userService.CreateUserAsync(userData)
    .IfSuccessAsync(async (user) =>
    {
        return await _companyService.GetCompanyByIdAsync(user.CompanyId);
    });
```


## See Also
* [Result](../../Result/Result.md)
* [Result&lt;T&gt;](../../ResultT/ResultT.md)
