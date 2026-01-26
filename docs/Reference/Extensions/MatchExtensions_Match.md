# MatchExtensions.Match Methods
## Definition
Namespace: [Maple.Result.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Runs a specific action or function depending on whether the instance of [Result](../Result/Result.md) and [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful or a failed operation.

## Overloads
| Name | Description |
| ---- | ----------- |
| [Match(Result, Action, Action&lt;Error&gt;)](#matchresult-func-funcerror-boolean) | Executes one of the specified actions depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [Match(Result, Func&lt;Result&gt;, Action&lt;Error&gt;)](#matchresult-funcresult-funcerror-boolean) | Executes the specified function or action depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [Match(Result, Func&lt;Result&gt;, Func&lt;Error, Result&gt;)](#matchresult-funcresult-funcerror-result-boolean) | Executes one of the specified functions depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [Match&lt;T&gt;(Result, Func&lt;T&gt;, Action&lt;Error&gt;)](#matchtresult-funct-funcerror-boolean) | Executes the specified function or action depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [Match&lt;T&gt;(Result, Func&lt;T&gt;, Func&lt;Error, T&gt;)](#matchtresult-funct-funcerror-t-boolean) | Executes one of the specified functions depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [Match&lt;T&gt;(Result, Func&lt;Result&lt;T&gt;&gt;, Action&lt;Error&gt;)](#matchtresult-funcresultt-funcerror-boolean) | Executes the specified function or action depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [Match&lt;T&gt;(Result, Func&lt;Result&lt;T&gt;&gt;, Func&lt;Error, Result&lt;T&gt;&gt;)](#matchtresult-funcresultt-funcerror-resultt-boolean) | Executes one of the specified functions depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [Match&lt;T&gt;(Result&lt;T&gt;, Action&lt;T&gt;, Action&lt;Error&gt;)](#matchtresultt-result-funct-funcerror-boolean) |  Executes one of the specified actions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |
| [Match&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, TNext&gt;, Action&lt;Error&gt;)](#matcht-tnextresultt-result-funct-tnext-funcerror-boolean) |  Executes the specified function or action depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |
| [Match&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, TNext&gt;, Func&lt;Error, TNext&gt;)](#matcht-tnextresultt-result-funct-tnext-funcerror-tnext-boolean) |  Executes one of the specified functions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |
| [Match&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Result&lt;TNext&gt;&gt;, Action&lt;Error&gt;)](#matcht-tnextresultt-result-funct-resulttnext-funcerror-boolean) |  Executes the specified function or action depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |
| [Match&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Result&lt;TNext&gt;&gt;, Func&lt;Error, Result&lt;TNext&gt;&gt;)](#matcht-tnextresultt-result-funct-resulttnext-funcerror-resulttnext-boolean) |  Executes one of the specified functions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |

## Match(Result, Action, Action&lt;Error&gt;)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/MatchExtensions.cs#L53" target="_blank">MatchExtensions.cs</a>

Executes the provided `ifSuccessAction`, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), or the provided `ifErrorAction` otherwise.

```csharp
public static Result Match(this Result result, Action ifSuccessAction, Action<Error> ifErrorAction);
```

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessAction` [Action](https://learn.microsoft.com/dotnet/api/system.action)
The action to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation.

#### `ifErrorAction` [Action](https://learn.microsoft.com/dotnet/api/system.action-1)&lt;[Error](../Error/Error.md)&gt;
The action to be executed, only if the current instance of [Result](../Result/Result.md) represents a failed operation. The [Error](../Error/Error.md) instance contained in the `result` will be passed as a parameter to this action.

### Returns
#### [Result](../Result/Result.md)
The original `result` passed as a parameter. Can be used for method chaining.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessAction` or `ifErrorAction` parameter is `null`.

## Examples
```csharp
var createUserResult = _userService.CreateUser(userData);

createUserResult.Match(
    () => _auditService.LogUserAdded(userData),
    error => _auditService.LogError(error));
```


## Match(Result, Action&lt;Result&gt;, Action&lt;Error&gt;)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/MatchExtensions.cs#L91" target="_blank">MatchExtensions.cs</a>

Executes the provided `ifSuccessFunction`, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), or the provided `ifErrorAction` otherwise.

```csharp
public static Result Match(this Result result, Func<Result> ifSuccessFunction, Action<Error> ifErrorAction);
```

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Result](../Result/Result.md)&gt;
The function to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation.

#### `ifErrorAction` [Action](https://learn.microsoft.com/dotnet/api/system.action-1)&lt;[Error](../Error/Error.md)&gt;
The action to be executed, only if the current instance of [Result](../Result/Result.md) represents a failed operation. The [Error](../Error/Error.md) instance contained in the `result` will be passed as a parameter to this action.

### Returns
#### [Result](../Result/Result.md)
If the `result` is successful, the [Result](../Result/Result.md) record returned by the `ifSuccessFunction`; otherwise, the original failed [Result](../Result/Result.md) record with the original error.


### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorAction` parameter is `null`.

## Examples
```csharp
var createUserResult = _userService.CreateUser(userData);

var userAddedResult = createUserResult.Match(
    () => _companyService.AddUser(userData),
    error => _auditService.LogError(error));
```


## Match(Result, Func&lt;Result&gt;, Func&lt;Error, Result&gt;)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/MatchExtensions.cs#L127" target="_blank">MatchExtensions.cs</a>

Executes the provided `ifSuccessFunction`, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), or the provided `ifErrorFunction` otherwise.

```csharp
public static Result Match(this Result result, Func<Result> ifSuccessFunction,
    Func<Error, Result> ifErrorFunction);
```

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Result](../Result/Result.md)&gt;
The function to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Result](../Result/Result.md)&gt;
The function to be executed, only if the current instance of [Result](../Result/Result.md) represents a failed operation. The [Error](../Error/Error.md) instance contained in the `result` will be passed as a parameter to this function.

### Returns
#### [Result](../Result/Result.md)
The [Result](../Result/Result.md) returned by either the `ifSuccessFunction`, if the `result` is successful, or `ifErrorFunction` otherwise.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorFunction` parameter is `null`.

## Examples
```csharp
var authenticationResult = _loginService.Authenticate(loginData);

var userAddedResult = authenticationResult.Match(
    () => _companyService.AddUser(userData),
    error =>
    {
        _auditService.LogError(error);
        return _loginService.LockAccount(loginData.Username);
    });
```


## Match&lt;T&gt;(Result, Func&lt;T&gt;, Action&lt;Error&gt;)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/MatchExtensions.cs#L167" target="_blank">MatchExtensions.cs</a>

Executes the provided `ifSuccessFunction`, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), or the provided `ifErrorAction` otherwise.

```csharp
public static Result<T> Match<T>(this Result result, Func<T> ifSuccessFunction, Action<Error> ifErrorAction);
```

### Type Parameters
#### `T`
The type of the value returned by the `ifSuccessFunction`.

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;T&gt;
The function to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation.

#### `ifErrorAction` [Action](https://learn.microsoft.com/dotnet/api/system.action-1)&lt;[Error](../Error/Error.md)&gt;
The action to be executed, only if the current instance of [Result](../Result/Result.md) represents a failed operation. The [Error](../Error/Error.md) instance contained in the `result` will be passed as a parameter to this action.

### Returns
#### [Result](../ResultT/ResultT.md)&lt;T&gt;
The [Result](../ResultT/ResultT.md) with the value returned by the `ifSuccessFunction`, if the `result` is successful, or the [Result](../ResultT/ResultT.md) with the original [Error](../Error/Error.md) otherwise.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorAction` parameter is `null`.

## Examples
```csharp
var createUserResult = _userService.CreateUser(userData);

var companyResult = createUserResult.Match(
    user => _loginService.GetUserToken(user),
    error => _auditService.LogError(error));
```


## Match&lt;T&gt;(Result, Func&lt;T&gt;, Func&lt;Error, T&gt;)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/MatchExtensions.cs#L206" target="_blank">MatchExtensions.cs</a>

Executes the provided `ifSuccessFunction`, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), or the provided `ifErrorFunction` otherwise.

```csharp
public static Result<T> Match<T>(this Result result, Func<T> ifSuccessFunction, Func<Error, T> ifErrorFunction);
```

### Type Parameters
#### `T`
The type of the value returned by the `ifSuccessFunction` and `ifErrorFunction`.

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;T&gt;
The function to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), T&gt;
The function to be executed, only if the current instance of [Result](../Result/Result.md) represents a failed operation. The [Error](../Error/Error.md) instance contained in the `result` will be passed as a parameter to this function.

### Returns
#### T
The value returned by either the `ifSuccessFunction` if the `result` is successful or `ifErrorFunction` otherwise.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorFunction` parameter is `null`.

## Examples
```csharp
var createUserResult = _userService.CreateUser(userData);

var viewModel = createUserResult.Match(
    user =>
    {
        var userDetails = _userService.GetUserDetails(user);
        return _userMapper.Map(userDetails);
    },
    error => return _userMapper.MapErrorViewModel(error));
```


## Match&lt;T&gt;(Result, Func&lt;Result&lt;T&gt;&gt;, Action&lt;Error&gt;)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/MatchExtensions.cs#L245" target="_blank">MatchExtensions.cs</a>

Executes the provided `ifSuccessFunction`, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), or the provided `ifErrorAction` otherwise.

```csharp
public static Result<T> Match<T>(this Result result, Func<Result<T>> ifSuccessFunction,
    Action<Error> ifErrorAction);
```

### Type Parameters
#### `T`
The type of the [Result&lt;T&gt;](../ResultT/ResultT.md) value returned by the `ifSuccessFunction`.

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The function to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation.

#### `ifErrorAction` [Action](https://learn.microsoft.com/dotnet/api/system.action-1)&lt;[Error](../Error/Error.md)&gt;
The action to be executed, only if the current instance of [Result](../Result/Result.md) represents a failed operation. The [Error](../Error/Error.md) instance contained in the `result` will be passed as a parameter to this action.

### Returns
#### [Result](../ResultT/ResultT.md)&lt;T&gt;
The value returned by the `ifSuccessFunction`, if the `result` is successful, or the [Result](../ResultT/ResultT.md)&lt;T&gt; with the original error otherwise.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorAction` parameter is `null`.

## Examples
```csharp
var createUserResult = _userService.CreateUser(userData);

var userDetailsResult = createUserResult.Match(
    user => _userService.GetUserDetails(user),
    error => _userMapper.MapErrorViewModel(error));
```


## Match&lt;T&gt;(Result, Func&lt;Result&lt;T&gt;&gt;, Func&lt;Error, Result&lt;T&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/MatchExtensions.cs#L285" target="_blank">MatchExtensions.cs</a>

Executes the provided `ifSuccessFunction`, only if the instance of [Result](../Result/Result.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), or the provided `ifErrorFunction` otherwise.

```csharp
public static Result<T> Match<T>(this Result result, Func<Result<T>> ifSuccessFunction,
    Func<Error, Result<T>> ifErrorFunction);
```

### Type Parameters
#### `T`
The type of the [Result&lt;T&gt;](../ResultT/ResultT.md) value returned by the `ifSuccessFunction` and `ifErrorFunction`.

### Parameters
#### `result` [Result](../Result/Result.md)
The instance of [Result](../Result/Result.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The function to be executed, only if the current instance of [Result](../Result/Result.md) represents a successful operation.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Result](../ResultT/ResultT.md)&lt;T&gt;&gt;
The function to be executed, only if the current instance of [Result](../Result/Result.md) represents a failed operation. The [Error](../Error/Error.md) instance contained in the `result` will be passed as a parameter to this function.

### Returns
#### [Result](../ResultT/ResultT.md)&lt;T&gt;
The [Result](../ResultT/ResultT.md) with the value of type `T` returned by the `ifSuccessFunction`, if the `result` is successful, or the [Result](../ResultT/ResultT.md) with the value of type `T` returned by the `ifErrorFunction` otherwise.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorFunction` parameter is `null`.

## Examples
```csharp
var createUserResult = _userService.CreateUser(userData);

var userDetailsResult = createUserResult.Match(
    user => _userService.GetUserDetails(user),
    error => _userMapper.MapErrorViewModel(error));
```


## Match&lt;T&gt;(Result&lt;T&gt;, Action&lt;T&gt;, Action&lt;Error&gt;)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/MatchExtensions.cs#L328" target="_blank">MatchExtensions.cs</a>

Executes the provided `ifSuccessAction`, only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), or the provided `ifErrorAction` otherwise.

```csharp
public static Result<T> Match<T>(this Result<T> result, Action<T> ifSuccessAction, Action<Error> ifErrorAction);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the `ifSuccessAction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation with a value to be evaluated.

#### `ifSuccessAction` [Action](https://learn.microsoft.com/dotnet/api/system.action-1)&lt;T&gt;
The action to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this action.

#### `ifErrorAction` [Action](https://learn.microsoft.com/dotnet/api/system.action-1)&lt;[Error](../Error/Error.md)&gt;
The action to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a failed operation. The [Error](../Error/Error.md) instance contained in the `result` will be passed as a parameter to this action.

### Returns
#### [Result](../ResultT/ResultT.md)&lt;T&gt;
The original `result` passed as a parameter. Can be used for method chaining.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessAction` or `ifErrorAction` parameter is `null`.

## Examples
```csharp
var userResult = _userService.CreateUser(userData);

userResult.Match(
    user => _auditService.LogUserAdded(user),
    error => _auditService.LogError(error));
```


## Match&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, TNext&gt;, Action&lt;Error&gt;)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/MatchExtensions.cs#L372" target="_blank">MatchExtensions.cs</a>

Executes the provided `ifSuccessFunction`, only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), or the provided `ifErrorAction` otherwise.

```csharp
public static Result<TNext> Match<T, TNext>(this Result<T> result, Func<T, TNext> ifSuccessFunction,
    Action<Error> ifErrorAction);
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
The function to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this function.

#### `ifErrorAction` [Action](https://learn.microsoft.com/dotnet/api/system.action-1)&lt;[Error](../Error/Error.md)&gt;
The action to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a failed operation. The [Error](../Error/Error.md) instance contained in the `result` will be passed as a parameter to this action.

### Returns
#### [Result](../Result/Result.md)&lt;TNext&gt;
The [Result](../Result/Result.md)&lt;TNext&gt; with the value returned by the `ifSuccessFunction`, if the `result` is successful, or the [Result](../Result/Result.md)&lt;TNext&gt; with the original [Error](../Error/Error.md) otherwise.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorAction` parameter is `null`.

## Examples
```csharp
var createUserResult = _userService.CreateUser(userData);

var userAddedResult = createUserResult.Match(
    user => _companyService.AddUser(user),
    error => _auditService.LogError(error));
```


## Match&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, TNext&gt;, Func&lt;Error, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/MatchExtensions.cs#L414" target="_blank">MatchExtensions.cs</a>

Executes the provided `ifSuccessFunction`, only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), or the provided `ifErrorFunction` otherwise.

```csharp
public static TNext Match<T, TNext>(this Result<T> result, Func<T, TNext> ifSuccessFunction,
    Func<Error, TNext> ifErrorFunction);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the `ifSuccessFunction`.

#### `TNext`
The type of the value returned by the `ifSuccessFunction` and `ifErrorFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation with a value to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, TNext&gt;
The function to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this function.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), TNext&gt;
The function to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a failed operation. The [Error](../Error/Error.md) instance contained in the `result` will be passed as a parameter to this function.

### Returns
#### TNext
The value of the `TNext` type returned by either the `ifSuccessFunction`, if the `result` is successful, or the `ifErrorFunction`, otherwise.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorFunction` parameter is `null`.

## Examples
```csharp
var createUserResult = _userService.CreateUser(userData);

var companyResult = createUserResult.Match(
    user =>
    {
        _companyService.GetCompanyById(user.CompanyId);
        return _mapper.Map(user);
    },
    error => _mapper.Map(error);
```


## Match&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Result&lt;TNext&gt;&gt;, Action&lt;Error&gt;)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/MatchExtensions.cs#L456" target="_blank">MatchExtensions.cs</a>

Executes the provided `ifSuccessFunction`, only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), or the provided `ifErrorAction` otherwise.

```csharp
public static Result<TNext> Match<T, TNext>(this Result<T> result, Func<T, Result<TNext>> ifSuccessFunction,
    Action<Error> ifErrorAction);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the `ifSuccessFunction`.

#### `TNext`
The type of the [Result&lt;TNext&gt;](../ResultT/ResultT.md) value returned by the `ifSuccessFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this function.

#### `ifErrorAction` [Action](https://learn.microsoft.com/dotnet/api/system.action-1)&lt;[Error](../Error/Error.md)&gt;
The action to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a failed operation. The [Error](../Error/Error.md) instance contained in the `result` will be passed as a parameter to this action.

### Returns
#### [Result](../ResultT/ResultT.md)&lt;TNext&gt;
The [Result](../ResultT/ResultT.md)&lt;TNext&gt; with the value returned by the `ifSuccessFunction`, if the `result` is successful, or the [Result](../ResultT/ResultT.md)&lt;TNext&gt; with the original [Error](../Error/Error.md) otherwise.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorAction` parameter is `null`.

## Examples
```csharp
var createUserResult = _userService.CreateUser(userData);

var companyResult = createUserResult.Match(
    user => _companyService.GetCompanyById(user.CompanyId),
    error => _auditService.LogError(error));
```


## Match&lt;T, TNext&gt;(Result&lt;T&gt; result, Func&lt;T, Result&lt;TNext&gt;&gt;, Func&lt;Error, Result&lt;TNext&gt;&gt;)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/MatchExtensions.cs#L498" target="_blank">MatchExtensions.cs</a>

Executes the provided `ifSuccessFunction`, only if the instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation (i.e., does not contain an [Error](../Error/Error.md)), or the provided `ifErrorFunction` otherwise.

```csharp
public static Result<TNext> Match<T, TNext>(this Result<T> result, Func<T, Result<TNext>> ifSuccessFunction,
    Func<Error, Result<TNext>> ifErrorFunction);
```

### Type Parameters
#### `T`
The type of the value associated with successful `result` and passed to the `ifSuccessFunction`.

#### `TNext`
The type of the [Result&lt;TNext&gt;](../ResultT/ResultT.md) value returned by the `ifSuccessFunction` and `ifErrorFunction`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The instance of [Result&lt;T&gt;](../ResultT/ResultT.md) record that represents the result of the operation to be evaluated.

#### `ifSuccessFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. The value of type `T` contained in the [Result&lt;T&gt;](../ResultT/ResultT.md) instance will be passed as a parameter to this function.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), [Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
The function to be executed, only if the current instance of [Result&lt;T&gt;](../ResultT/ResultT.md) represents a failed operation. The [Error](../Error/Error.md) instance contained in the `result` will be passed as a parameter to this function.

### Returns
#### [Result](../ResultT/ResultT.md)&lt;TNext&gt;
The [Result](../ResultT/ResultT.md)&lt;TNext&gt; with the value of type `TNext` returned by the `ifSuccessFunction`, if the `result` is successful, or the [Result](../ResultT/ResultT.md)&lt;TNext&gt; with the value of type `TNext` returned by the `ifErrorFunction` otherwise.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` or `ifSuccessFunction` or `ifErrorAction` parameter is `null`.

## Examples
```csharp
var createUserResult = _userService.CreateUser(userData);

var viewModel = createUserResult.Match(
    user => _companyService.GetCompanyById(user.CompanyId),
    error =>
    {
        _auditService.LogError(error);
        return _mapper.Map(error);
    });
```


## See Also
* [Error](../Error/Error.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
