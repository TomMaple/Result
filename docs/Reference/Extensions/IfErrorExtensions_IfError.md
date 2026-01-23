# IfErrorExtensions.IfError Methods
## Definition
Namespace: [Maple.Result.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Runs a specific action or function if the instance of `TResult` represents a failed operation (i.e., contains an [Error](../Error/Error.md)).

## Overloads
| Name                             | Description                                                |
| -------------------------------- | ---------------------------------------------------------- |
| [IfError&lt;TResult&gt;(TResult, Action&lt;Error&gt;)](#iferrortresulttresult-actionerror) | Executes the provided action if the `TResult` instance represents a failed operation. |
| [IfError&lt;TResult&gt;(TResult, Func&lt;Error, TResult&gt;)](#iferrortresulttresult-funcerror-tresult) | Executes the provided function if the `TResult` instance represents a failed operation and returns its result. |


## IfError&lt;TResult&gt;(TResult, Action&lt;Error&gt;)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/IfErrorExtensions.cs#L45" target="_blank">IfErrorExtensions.cs</a>

Executes the provided action only if the instance of `TResult` represents a failed operation (i.e., contains an [Error](../Error/Error.md)).

```csharp
public static void IfError<TResult>(this TResult result, Action<Error> errorAction)
    where TResult : IResult;
```

### Type Parameters
#### `TResult`
The type of the `result` that implements the [IResult](../IResult/IResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record.

### Parameters
#### `result` TResult
The instance of `TResult` (that implements [IResult](../IResult/IResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record) that represents the result of the operation to be evaluated.

#### `ifErrorAction` [Action](https://learn.microsoft.com/dotnet/api/system.action-1)&lt;[Error](../Error/Error.md)&gt;

The action to be executed only if the current instance of `TResult` represents a failed operation. The [Error](../Error/Error.md) instance contained in the `TResult` will be passed as a parameter to this action.

### Returns
#### TResult
The original `TResult` instance passed as a `result` parameter.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifErrorAction` parameter is `null`.

## Examples
```csharp
var createUserResult = _userService.CreateUser(userData);

createUserResult.IfError(error =>
{
    _logger.LogError("Failed to create user: {ErrorDetail}", error.Detail);
    Metrics.IncrementCounter("user_creation_failures");
});
```


## IfError&lt;TResult&gt;(TResult, Func&lt;Error, TResult&gt;)
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/IfErrorExtensions.cs#L76" target="_blank">IfErrorExtensions.cs</a>

Executes the provided function only if the instance of `TResult` represents a failed operation (i.e., contains an [Error](../Error/Error.md)).

```csharp
public static TResult IfError<TResult>(this TResult result, Func<Error, TResult> ifErrorFunction)
    where TResult : IResult;
```

### Type Parameters
#### `TResult`
The type of the `result` that implements the [IResult](../IResult/IResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record.

### Parameters
#### `result` TResult
The instance of `TResult` (that implements [IResult](../IResult/IResult.md) interface—[Result](../Result/Result.md) or [Result&lt;T&gt;](../ResultT/ResultT.md) record) that represents the result of the operation to be evaluated.

#### `ifErrorFunction` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;[Error](../Error/Error.md), TResult&gt;

The function to be executed only if the current instance of `TResult` represents a failed operation. The [Error](../Error/Error.md) instance contained in the `TResult` will be passed as a parameter to this function.

### Returns
#### TResult
The original `TResult` instance passed as a `result` parameter.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result` parameter is `null` or `ifErrorFunction` parameter is `null`.

## Examples
```csharp
var userResult = _userService.GetUser(userData);

userResult.IfError(error =>
{
    _logger.LogError("Failed to get user: {ErrorDetail}", error.Detail);
    Metrics.IncrementCounter("user_retrieval_failures");
});
```

## See Also
* [Error](../Error/Error.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
