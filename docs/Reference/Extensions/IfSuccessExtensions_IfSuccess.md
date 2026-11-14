# IfSuccessExtensions.IfSuccess Methods
## Definition
Namespace: [Maple.Result.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Runs a specific action or function, if the instance of [Result](../Result/Result.md) and [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)).

## Overloads
| Name | Description |
| ---- | ----------- |
| [IfSuccess(Result, Action)](#ifsuccessresult-action) | Executes the specified action, if the [Result](../Result/Result.md) represents a successful operation. |
| [IfSuccess(Result, Func&lt;Result&gt;)](#ifsuccessresult-funcresult) | Executes the specified function, if the [Result](../Result/Result.md) represents a successful operation. |
| [IfSuccess(Result, Func&lt;Error&gt;)](#ifsuccessresult-funcerror) | Executes the specified function, if the [Result](../Result/Result.md) represents a successful operation, and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns. |
| [IfSuccess&lt;T&gt;(Result, Func&lt;T&gt;)](#ifsuccesstresult-funct) | Executes the specified function, if the [Result](../Result/Result.md) represents a successful operation. |
| [IfSuccess&lt;T&gt;(Result, Func&lt;Result&lt;T&gt;&gt;)](#ifsuccesstresult-funcresultt) | Executes the specified function, if the [Result](../Result/Result.md) represents a successful operation. |
| [IfSuccess&lt;T&gt;(Result&lt;T&gt;, Action&lt;T&gt;)](#ifsuccesstresultt-actiont) | Executes the specified action, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. |
| [IfSuccess&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Result&gt;)](#ifsuccesstresultt-funct-result) | Executes the specified function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. |
| [IfSuccess&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Error&gt;)](#ifsuccesstresultt-funct-error) | Executes the specified function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns. |
| [IfSuccess&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, TNext&gt;)](#ifsuccesst-tnextresultt-funct-tnext) | Executes the specified function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. |
| [IfSuccess&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Result&lt;TNext&gt;&gt;)](#ifsuccesst-tnextresultt-funct-resulttnext) | Executes the specified function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. |

## Remarks
A function that returns an [Error](../Error/Error.md) makes the result fail: see [IfSuccess(Result, Func&lt;Error&gt;)](#ifsuccessresult-funcerror) and [IfSuccess&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Error&gt;)](#ifsuccesstresultt-funct-error).

A function that returns a `Task`, `Task<T>`, `ValueTask` or `ValueTask<T>` is not supported: `IfSuccess` does not await it, so such a call is a compile error (CS0619). Use [IfSuccessAsync](IfSuccessTaskAsyncExtensions_IfSuccessAsync.md) instead, or specify the type arguments (e.g., `IfSuccess<Task>(…)` or `IfSuccess<T, Task>(…)`) to keep the task in the result on purpose.

## IfSuccess(Result, Action)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/IfSuccessExtensions.cs#L47" target="_blank">IfSuccessExtensions.cs</a>

Executes the provided action, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)).

```csharp
public static Result IfSuccess(this Result result, Action ifSuccessAction);
```

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessAction` [Action](https://learn.microsoft.com/dotnet/api/system.action)
The action to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation.

### Returns
#### [Result](../Result/Result.md)
The original `result` passed as a parameter. Can be used for method chaining.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessAction` parameter is `null`.

### Examples
```csharp
var createUserResult = _userService.CreateUser(userData);

createUserResult.IfSuccess(() =>
{
    _auditService.LogUserAdded(userData);
});
```


## IfSuccess(Result, Func&lt;Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/IfSuccessExtensions.cs#L79" target="_blank">IfSuccessExtensions.cs</a>

Executes the provided function only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)).

```csharp
public static Result IfSuccess(this Result result, Func<Result> ifSuccessFunction);
```

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Result](../Result/Result.md)&gt;
The function to be executed only if the current instance of [Result](../Result/Result.md) represents a successful operation.

### Returns
#### [Result](../Result/Result.md)
The original `result` passed as a parameter. Can be used for method chaining.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns `null`.

### Examples
```csharp
var createUserResult = _userService.CreateUser(userData);

var userAddedResult = createUserResult.IfSuccess(() =>
{
    _companyService.AddUser(userData);
});
```


## IfSuccess(Result, Func&lt;Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/IfSuccessExtensions.cs#L117" target="_blank">IfSuccessExtensions.cs</a>

Executes the provided function only if the instance of [Result](../Result/Result.md) represents a successful operation, and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the function.

```csharp
public static Result IfSuccess(this Result result, Func<Error> ifSuccessFunction);
```

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Error](../Error/Error.md)&gt;
The function to be executed only if the `result` represents a successful operation. It returns the [Error](../Error/Error.md) of the failed result.

### Returns
#### [Result](../Result/Result.md)
A failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `ifSuccessFunction` if the `result` is successful; otherwise, the original `result`. The returned result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns `null`.

### Remarks
Without this overload, a function returning an [Error](../Error/Error.md) would bind to [IfSuccess&lt;T&gt;(Result, Func&lt;T&gt;)](#ifsuccesstresult-funct) and return a **successful** `Result<Error>`. To get that on purpose, specify the type argument: `IfSuccess<Error>(…)`.

### Examples
```csharp
var result = _userService.DeleteUser(userId)
    .IfSuccess(() => Error.Conflict(typeUri, "The user cannot be deleted."));
```


## IfSuccess&lt;T&gt;(Result, Func&lt;T&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/IfSuccessExtensions.cs#L149" target="_blank">IfSuccessExtensions.cs</a>

Executes the provided function only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)).

```csharp
public static Result<T> IfSuccess<T>(this Result result, Func<T> ifSuccessFunction);
```

### Type Parameters
#### `T`
The type of the value returned by the `ifSuccessFunction`.

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;T&gt;
The function to be executed only if the current instance of [Result](../Result/Result.md) represents a successful operation.

### Returns
#### [Result](../ResultT/ResultT.md)&lt;T&gt;
If the `result` is successful, the [Result](../ResultT/ResultT.md)&lt;T&gt; record with the value returned by the `ifSuccessFunction`; otherwise, the failed [Result](../ResultT/ResultT.md)&lt;T&gt; record with the original error.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

### Examples
```csharp
var createUserResult = _userService.CreateUser(userData);

var companyResult = createUserResult.IfSuccess(() =>
{
    return _companyService.GetCompanyById(userData.CompanyId);
});
```


## IfSuccess&lt;T&gt;(Result, Func&lt;Result&lt;T&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/IfSuccessExtensions.cs#L182" target="_blank">IfSuccessExtensions.cs</a>

Executes the provided function only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)).

```csharp
public static Result<T> IfSuccess<T>(this Result result, Func<Result<T>> ifSuccessFunction);
```

### Type Parameters
#### `T`
The type of the [Result&lt;T&gt;](../ResultT/ResultT.md) value returned by the `ifSuccessFunction`.

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The function to be executed only if the current instance of [Result](../Result/Result.md) represents a successful operation.

### Returns
#### [Result](../ResultT/ResultT.md)&lt;T&gt;
If the `result` is successful, the [Result](../ResultT/ResultT.md)&lt;T&gt; record returned by the `ifSuccessFunction`; otherwise, the failed [Result](../ResultT/ResultT.md)&lt;T&gt; record with the original error.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns `null`.

### Examples
```csharp
var createUserResult = _userService.CreateUser(userData);

var companyResult = createUserResult.IfSuccess(() =>
{
    return _companyService.GetCompanyById(userData.CompanyId);
});
```


## IfSuccess&lt;T&gt;(Result&lt;T&gt;, Action&lt;T&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/IfSuccessExtensions.cs#L215" target="_blank">IfSuccessExtensions.cs</a>

Executes the provided action, only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)).

```csharp
public static Result<T> IfSuccess<T>(this Result<T> result, Action<T> ifSuccessAction);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation with a value to be evaluated.

#### `ifSuccessAction` [Action](https://learn.microsoft.com/dotnet/api/system.action-1)&lt;T&gt;
The action to be executed only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this action.

### Returns
#### [Result](../ResultT/ResultT.md)&lt;T&gt;
The original `result` passed as a parameter. Can be used for method chaining.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessAction` parameter is `null`.

### Examples
```csharp
var userResult = _userService.CreateUser(userData);

userResult.IfSuccess(user =>
{
    _auditService.LogUserAdded(user);
});
```


## IfSuccess&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Result&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/IfSuccessExtensions.cs#L247" target="_blank">IfSuccessExtensions.cs</a>

Executes the provided function only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)).

```csharp
public static Result IfSuccess<T>(this Result<T> result, Func<T, Result> ifSuccessFunction);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation with a value to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../Result/Result.md)&gt;
The function to be executed only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this function.

### Returns
#### [Result](../Result/Result.md)
If the `result` is successful, the [Result](../Result/Result.md) returned by the `ifSuccessFunction`; otherwise, the failed [Result](../Result/Result.md) with the original error.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns `null`.

### Examples
```csharp
var createUserResult = _userService.CreateUser(userData);

var userAddedResult = createUserResult.IfSuccess(user =>
{
    return _companyService.AddUser(user);
});
```


## IfSuccess&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/IfSuccessExtensions.cs#L286" target="_blank">IfSuccessExtensions.cs</a>

Executes the provided function only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the function.

```csharp
public static Result IfSuccess<T>(this Result<T> result, Func<T, Error> ifSuccessFunction);
```

### Type Parameters
#### `T`
The type of the value of the `result`, passed to the `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Error](../Error/Error.md)&gt;
The function to be executed only if the `result` represents a successful operation. The value of the `result` is passed as a parameter. It returns the [Error](../Error/Error.md) of the failed result.

### Returns
#### [Result](../Result/Result.md)
A failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `ifSuccessFunction` if the `result` is successful; otherwise, a failed [Result](../Result/Result.md) with the original [Error](../Error/Error.md). The returned result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns `null`.

### Remarks
Without this overload, a function returning an [Error](../Error/Error.md) would bind to [IfSuccess&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, TNext&gt;)](#ifsuccesst-tnextresultt-funct-tnext) and return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `IfSuccess<T, Error>(…)`.

### Examples
```csharp
var result = _userService.GetUser(userId)
    .IfSuccess(user => Error.Conflict(typeUri, $"The user ‘{user.Name}’ already exists."));
```


## IfSuccess&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/IfSuccessExtensions.cs#L324" target="_blank">IfSuccessExtensions.cs</a>

Executes the provided function only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)).

```csharp
public static Result<TNext> IfSuccess<T, TNext>(this Result<T> result, Func<T, TNext> ifSuccessFunction);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the `ifSuccessFunction`.

#### `TNext`
The type of the value returned by the `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation with a value to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, TNext&gt;
The function to be executed only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this function.

### Returns
#### [Result](../ResultT/ResultT.md)&lt;TNext&gt;
If the `result` is successful, the [Result&lt;TNext&gt;](../ResultT/ResultT.md) with the value returned by the `ifSuccessFunction`; otherwise, the failed [Result&lt;TNext&gt;](../ResultT/ResultT.md) with the original error.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

### Examples
```csharp
var createUserResult = _userService.CreateUser(userData);

var companyResult = createUserResult.IfSuccess(user =>
{
    return _companyService.GetCompanyById(user.CompanyId);
});
```


## IfSuccess&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/IfSuccessExtensions.cs#L363" target="_blank">IfSuccessExtensions.cs</a>

Executes the provided function only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)).

```csharp
public static Result<TNext> IfSuccess<T, TNext>(this Result<T> result, Func<T, Result<TNext>> ifSuccessFunction);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the `ifSuccessFunction`.

#### `TNext`
The type of the [Result&lt;TNext&gt;](../ResultT/ResultT.md) value returned by the `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The function to be executed only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this function.

### Returns
#### [Result](../ResultT/ResultT.md)&lt;TNext&gt;
If the `result` is successful, the [Result&lt;TNext&gt;](../ResultT/ResultT.md) returned by the `ifSuccessFunction`; otherwise, the failed [Result&lt;TNext&gt;](../ResultT/ResultT.md) with the original error.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifSuccessFunction` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `ifSuccessFunction` function returns `null`.

### Examples
```csharp
var createUserResult = _userService.CreateUser(userData);

var companyResult = createUserResult.IfSuccess(user =>
{
    return _companyService.GetCompanyById(user.CompanyId);
});
```


## See Also
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
