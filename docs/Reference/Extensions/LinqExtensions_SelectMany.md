# LinqExtensions.SelectMany Methods
## Definition
Namespace: [Maple.Result.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Enables the LINQ query syntax with more than one `from` clause for [Result&lt;T&gt;](../ResultT/ResultT.md).

## Overloads
| Name | Description |
| ---- | ----------- |
| [SelectMany&lt;T, TMiddle, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)](#selectmanyt-tmiddle-tnextresultt-funct-resulttmiddle-funct-tmiddle-tnext) | Returns a new instance of [Result&lt;TNext&gt;](../ResultT/ResultT.md) instance by applying the provided selector functions if the current [Result&lt;T&gt;](../ResultT/ResultT.md) if it is successful, or a new [Result&lt;TNext&gt;](../ResultT/ResultT.md) instance with the same error otherwise. |
| [SelectMany&lt;T, TMiddle&gt;(Result&lt;T&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, Error&gt;)](#selectmanyt-tmiddleresultt-funct-resulttmiddle-funct-tmiddle-error) | Applies the synchronous selector and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the result selector. |

## Remarks
A result selector that returns an [Error](../Error/Error.md) makes the query fail: such a query returns a failed [Result](../Result/Result.md) instead of a **successful** `Result<Error>`.


## SelectMany&lt;T, TMiddle, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqExtensions.cs#L126" target="_blank">LinqExtensions.cs</a>

Returns a new instance of [Result&lt;TNext&gt;](../ResultT/ResultT.md) instance by applying the provided selector functions if the current [Result&lt;T&gt;](../ResultT/ResultT.md) if it is successful, or a new [Result&lt;TNext&gt;](../ResultT/ResultT.md) instance with the same error otherwise.

```csharp
public static Result<TNext> SelectMany<T, TMiddle, TNext>(
    this Result<T> result,
    Func<T, Result<TMiddle>> collectionSelector,
    Func<T, TMiddle, TNext> resultSelector);
```

### Type Parameters
#### `T`
The type of the [Result](../ResultT/ResultT.md)&lt;T&gt; `result` value used to determine whether to execute the passed selector functions. If successful, this is also the type of the parameter passed to that function.

#### `TMiddle`
The type of the [Result](../ResultT/ResultT.md)&lt;TMiddle&gt; `result` output type of the passed collection selector and the type of one of the parameters passed to the result selector.

#### `TNext`
The type of the returned [Result](../ResultT/ResultT.md)&lt;TNext&gt; `result` value.

### Parameters
#### `result` [Result](../ResultT/ResultT.md)&lt;T&gt;
The instance of [Result](../ResultT/ResultT.md) record that represents the result of the operation to be evaluated.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;
The function that takes the value of the initial result and returns a new result representing an intermediate value.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, TNext&gt;
The function that combines the value from the initial result and the intermediate value to produce the final value.

### Returns
#### [Result](../ResultT/ResultT.md)&lt;TNext&gt;
A [Result&lt;T&gt;](../ResultT/ResultT.md) containing the value produced by the result selector if both the initial result and the intermediate result are successful; otherwise, a failed result.

A [Result&lt;T&gt;](../ResultT/ResultT.md) instance created by applying the provided collection selector and a result selector functions, if the current `result` is successful; or a new [Result&lt;T&gt;](../ResultT/ResultT.md) instance with the same error, otherwise.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If any of the `result` or `collectionSelector` or `resultSelector` parameters are `null`.

### Remarks
This method is used to enable LINQ query syntax for [Result&lt;T&gt;](../ResultT/ResultT.md) records. It allows chaining multiple operations that return [Result&lt;T&gt;](../ResultT/ResultT.md) instances while maintaining the error state, if any of the operations fail.

### Examples
```csharp
var emailResult =
    from addUser in _validationService.ValidateUserData(addUserRequest)
    from user in _userService.CreateUser(addUser)
    from userGroup in _groupService.AddUserToGroup(user)
    from sendWelcomeEmailResult in _emailService.SendWelcomeEmail(user)
    select sendWelcomeEmailResult;
```


## SelectMany&lt;T, TMiddle&gt;(Result&lt;T&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqExtensions.cs#L177" target="_blank">LinqExtensions.cs</a>

Applies the synchronous `collectionSelector` and the `resultSelector` to the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `resultSelector`; otherwise, returns a failed [Result](../Result/Result.md) with the first [Error](../Error/Error.md).

```csharp
public static Result SelectMany<T, TMiddle>(
    this Result<T> result,
    Func<T, Result<TMiddle>> collectionSelector,
    Func<T, TMiddle, Error> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the initial result. If successful, this is also the type of the parameter passed to the selectors.

#### `TMiddle`
The type of the value of the intermediate result returned by `collectionSelector`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The initial result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;
The synchronous function that takes the value of the initial result and returns a result with an intermediate value.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, [Error](../Error/Error.md)&gt;
The function that takes the value of the initial result and the intermediate value, and returns the [Error](../Error/Error.md) of the failed result.

### Returns
#### [Result](../Result/Result.md)
A failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `resultSelector` if both the initial and the intermediate results are successful; otherwise, a failed [Result](../Result/Result.md) with the first [Error](../Error/Error.md). The returned result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result`, `collectionSelector` or `resultSelector` parameter is `null`, or `resultSelector` returns `null`.

### Remarks
This overload makes a query with more than one `from` clause that selects an [Error](../Error/Error.md) return a failed [Result](../Result/Result.md). Without it, such a query would bind to [SelectMany&lt;T, TMiddle, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)](#selectmanyt-tmiddle-tnextresultt-funct-resulttmiddle-funct-tmiddle-tnext) and return a **successful** `Result<Error>`.

### Examples
```csharp
var result =
    from user in _userService.GetUser(userId)
    from order in _orderService.GetOrder(user, orderId)
    select Error.Conflict(typeUri, $"The order {order.Number} is already shipped.");
```


## See Also
* [Result&lt;T&gt;](../ResultT/ResultT.md)
* [Write C# Linq Queries](https://learn.microsoft.com/en-us/dotnet/csharp/linq/get-started/write-linq-queries)
