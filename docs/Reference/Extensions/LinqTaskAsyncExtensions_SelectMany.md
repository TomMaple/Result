# LinqTaskAsyncExtensions.SelectMany Methods
## Definition
Namespace: [Maple.Result.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Enables the LINQ query syntax with more than one `from` clause for [Result&lt;T&gt;](../ResultT/ResultT.md) and [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;, including queries that mix synchronous and asynchronous `from` clauses.

## Overloads
| Name | Description |
| ---- | ----------- |
| [SelectMany&lt;T, TMiddle, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)](#selectmanyt-tmiddle-tnexttaskresultt-funct-taskresulttmiddle-funct-tmiddle-tnext) | Applies an asynchronous selector to the value of an asynchronous result. |
| [SelectMany&lt;T, TMiddle, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)](#selectmanyt-tmiddle-tnexttaskresultt-funct-resulttmiddle-funct-tmiddle-tnext) | Applies a synchronous selector to the value of an asynchronous result. |
| [SelectMany&lt;T, TMiddle, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)](#selectmanyt-tmiddle-tnextresultt-funct-taskresulttmiddle-funct-tmiddle-tnext) | Applies an asynchronous selector to the value of a synchronous result. |
| [SelectMany&lt;T, TMiddle&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, Error&gt;)](#selectmanyt-tmiddletaskresultt-funct-taskresulttmiddle-funct-tmiddle-error) | Applies the asynchronous selector and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the result selector. |
| [SelectMany&lt;T, TMiddle&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, Error&gt;)](#selectmanyt-tmiddletaskresultt-funct-resulttmiddle-funct-tmiddle-error) | Applies the synchronous selector and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the result selector. |
| [SelectMany&lt;T, TMiddle&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, Error&gt;)](#selectmanyt-tmiddleresultt-funct-taskresulttmiddle-funct-tmiddle-error) | Applies the asynchronous selector and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by the result selector. |

## Remarks
A result selector that returns an [Error](../Error/Error.md) makes the query fail: such a query returns a failed [Result](../Result/Result.md) instead of a **successful** `Result<Error>`.

The query syntax needs a value in every `from` clause, so it works with [Result&lt;T&gt;](../ResultT/ResultT.md) but not with the non-generic [Result](../Result/Result.md). A `Task` and a `ValueTask` cannot be mixed in one query.


## SelectMany&lt;T, TMiddle, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L152" target="_blank">LinqTaskAsyncExtensions.cs</a>

Returns a new instance of [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; instance by applying the provided selector functions if the current [Result&lt;T&gt;](../ResultT/ResultT.md) if it is successful; or a new [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; instance with the same error, otherwise.


```csharp
public static async Task<Result<TNext>> SelectMany<T, TMiddle, TNext>(
    this Task<Result<T>> result,
    Func<T, Task<Result<TMiddle>>> collectionSelector,
    Func<T, TMiddle, TNext> resultSelector);
```

### Type Parameters
#### `T`
The type of the [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt; `result` value used to determine whether to execute the passed selector functions. If successful, this is also the type of the parameter passed to that function.

#### `TMiddle`
The type of the [Result](../ResultT/ResultT.md)&lt;TMiddle&gt; `result` output type of the passed collection selector and the type of one of the parameters passed to the result selector.

#### `TNext`
The type of the returned [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt; `result` value.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The task object representing the asynchronous operation that returns an instance of a [Result](../ResultT/ResultT.md) record to be evaluated.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The function that takes the value of the initial result and returns a task object representing the asynchronous operation that returns a new result representing an intermediate value.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, TNext&gt;
The function that combines the value from the initial result and the intermediate value to produce the final value.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A task object representing the asynchronous operation.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If any of the `result` or `collectionSelector` or `resultSelector` parameters are `null`.

### Remarks
This method is used to enable LINQ query syntax for [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt; records. It allows chaining multiple asynchronous operations that return [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt; while maintaining the error state, if any of the operations fail.

### Examples
```csharp
var emailResult =
    await from addUser in _validationService.ValidateUserDataAsync(addUserRequest)
    from user in _userService.CreateUserAsync(addUser)
    from userGroup in _groupService.AddUserToGroupAsync(user)
    from sendWelcomeEmailResult in _emailService.SendWelcomeEmailAsync(user)
    select sendWelcomeEmailResult;
```


## SelectMany&lt;T, TMiddle, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L205" target="_blank">LinqTaskAsyncExtensions.cs</a>

Applies the synchronous `collectionSelector` and the `resultSelector` to the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) produced by the awaited `result`; otherwise, returns a failed result with the original [Error](../Error/Error.md).

```csharp
public static async Task<Result<TNext>> SelectMany<T, TMiddle, TNext>(
    this Task<Result<T>> result,
    Func<T, Result<TMiddle>> collectionSelector,
    Func<T, TMiddle, TNext> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the initial result. If successful, this is also the type of the parameter passed to the selectors.

#### `TMiddle`
The type of the value of the intermediate result returned by `collectionSelector`.

#### `TNext`
The type of the value of the final result, produced by `resultSelector`.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The task object representing the asynchronous operation that returns the initial result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;
The synchronous function that takes the value of the initial result and returns a result with an intermediate value.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, TNext&gt;
The function that combines the value of the initial result and the intermediate value to produce the final value.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A task object representing the asynchronous operation: the result of applying the selectors if both the initial and the intermediate results are successful; otherwise, a failed result with the first [Error](../Error/Error.md).

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If any of the `result`, `collectionSelector` or `resultSelector` parameters are `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `result` returns `null`.

### Remarks
This overload enables a query in which an asynchronous `from` clause is followed by a synchronous one.

### Examples
```csharp
var addressResult =
    await from user in _userService.GetUserAsync(userId)    // asynchronous
    from address in _addressService.GetAddress(user)        // synchronous
    select address;
```


## SelectMany&lt;T, TMiddle, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L250" target="_blank">LinqTaskAsyncExtensions.cs</a>

Applies the asynchronous `collectionSelector` and the `resultSelector` to the value of a successful `result`; otherwise, returns a failed result with the original [Error](../Error/Error.md).

```csharp
public static async Task<Result<TNext>> SelectMany<T, TMiddle, TNext>(
    this Result<T> result,
    Func<T, Task<Result<TMiddle>>> collectionSelector,
    Func<T, TMiddle, TNext> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the initial result. If successful, this is also the type of the parameter passed to the selectors.

#### `TMiddle`
The type of the value of the intermediate result returned by `collectionSelector`.

#### `TNext`
The type of the value of the final result, produced by `resultSelector`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The initial result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The asynchronous function that takes the value of the initial result and returns a result with an intermediate value.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, TNext&gt;
The function that combines the value of the initial result and the intermediate value to produce the final value.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A task object representing the asynchronous operation: the result of applying the selectors if both the initial and the intermediate results are successful; otherwise, a failed result with the first [Error](../Error/Error.md).

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If any of the `result`, `collectionSelector` or `resultSelector` parameters are `null`.

### Remarks
This overload enables a query in which a synchronous `from` clause is followed by an asynchronous one.

### Examples
```csharp
var ordersResult =
    await from user in _userService.GetUser(userId)         // synchronous
    from orders in _orderService.GetOrdersAsync(user)       // asynchronous
    select orders;
```


## SelectMany&lt;T, TMiddle&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L304" target="_blank">LinqTaskAsyncExtensions.cs</a>

Applies the asynchronous `collectionSelector` and the `resultSelector` to the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) produced by the awaited `result` and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `resultSelector`; otherwise, returns a failed [Result](../Result/Result.md) with the first [Error](../Error/Error.md).

```csharp
public static async Task<Result> SelectMany<T, TMiddle>(
    this Task<Result<T>> result,
    Func<T, Task<Result<TMiddle>>> collectionSelector,
    Func<T, TMiddle, Error> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the initial result. If successful, this is also the type of the parameter passed to the selectors.

#### `TMiddle`
The type of the value of the intermediate result returned by `collectionSelector`.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The task object representing the asynchronous operation that returns the initial result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The asynchronous function that takes the value of the initial result and returns a result with an intermediate value.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, [Error](../Error/Error.md)&gt;
The function that takes the value of the initial result and the intermediate value, and returns the [Error](../Error/Error.md) of the failed result.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `resultSelector` if both the initial and the intermediate results are successful; otherwise, a failed [Result](../Result/Result.md) with the first [Error](../Error/Error.md). The returned result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result`, `collectionSelector` or `resultSelector` parameter is `null`, or `resultSelector` returns `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `result` returns `null`.

### Remarks
This overload makes a query with more than one `from` clause that selects an [Error](../Error/Error.md) return a failed [Result](../Result/Result.md). Without it, such a query would bind to [SelectMany&lt;T, TMiddle, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)](#selectmanyt-tmiddle-tnexttaskresultt-funct-taskresulttmiddle-funct-tmiddle-tnext) and return a **successful** `Result<Error>`.

### Examples
```csharp
var result =
    await from user in _userService.GetUserAsync(userId)
    from order in _orderService.GetOrderAsync(user, orderId)
    select Error.Conflict(typeUri, $"The order {order.Number} is already shipped.");
```


## SelectMany&lt;T, TMiddle&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L363" target="_blank">LinqTaskAsyncExtensions.cs</a>

Applies the synchronous `collectionSelector` and the `resultSelector` to the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) produced by the awaited `result` and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `resultSelector`; otherwise, returns a failed [Result](../Result/Result.md) with the first [Error](../Error/Error.md).

```csharp
public static async Task<Result> SelectMany<T, TMiddle>(
    this Task<Result<T>> result,
    Func<T, Result<TMiddle>> collectionSelector,
    Func<T, TMiddle, Error> resultSelector);
```

### Type Parameters
#### `T`
The type of the value of the initial result. If successful, this is also the type of the parameter passed to the selectors.

#### `TMiddle`
The type of the value of the intermediate result returned by `collectionSelector`.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The task object representing the asynchronous operation that returns the initial result.

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;
The synchronous function that takes the value of the initial result and returns a result with an intermediate value.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, [Error](../Error/Error.md)&gt;
The function that takes the value of the initial result and the intermediate value, and returns the [Error](../Error/Error.md) of the failed result.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `resultSelector` if both the initial and the intermediate results are successful; otherwise, a failed [Result](../Result/Result.md) with the first [Error](../Error/Error.md). The returned result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result`, `collectionSelector` or `resultSelector` parameter is `null`, or `resultSelector` returns `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `result` returns `null`.

### Remarks
This overload makes a query with more than one `from` clause that selects an [Error](../Error/Error.md) return a failed [Result](../Result/Result.md). Without it, such a query would bind to [SelectMany&lt;T, TMiddle, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)](#selectmanyt-tmiddle-tnexttaskresultt-funct-resulttmiddle-funct-tmiddle-tnext) and return a **successful** `Result<Error>`.

### Examples
```csharp
var result =
    await from user in _userService.GetUserAsync(userId)
    from order in _orderService.GetOrder(user, orderId)
    select Error.Conflict(typeUri, $"The order {order.Number} is already shipped.");
```


## SelectMany&lt;T, TMiddle&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L414" target="_blank">LinqTaskAsyncExtensions.cs</a>

Applies the asynchronous `collectionSelector` and the `resultSelector` to the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `resultSelector`; otherwise, returns a failed [Result](../Result/Result.md) with the first [Error](../Error/Error.md).

```csharp
public static async Task<Result> SelectMany<T, TMiddle>(
    this Result<T> result,
    Func<T, Task<Result<TMiddle>>> collectionSelector,
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

#### `collectionSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TMiddle&gt;&gt;&gt;
The asynchronous function that takes the value of the initial result and returns a result with an intermediate value.

#### `resultSelector` [Func](https://learn.microsoft.com/dotnet/api/system.func-3)&lt;T, TMiddle, [Error](../Error/Error.md)&gt;
The function that takes the value of the initial result and the intermediate value, and returns the [Error](../Error/Error.md) of the failed result.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `resultSelector` if both the initial and the intermediate results are successful; otherwise, a failed [Result](../Result/Result.md) with the first [Error](../Error/Error.md). The returned result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result`, `collectionSelector` or `resultSelector` parameter is `null`, or `resultSelector` returns `null`.

### Remarks
This overload makes a query with more than one `from` clause that selects an [Error](../Error/Error.md) return a failed [Result](../Result/Result.md). Without it, such a query would bind to [SelectMany&lt;T, TMiddle, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)](#selectmanyt-tmiddle-tnextresultt-funct-taskresulttmiddle-funct-tmiddle-tnext) and return a **successful** `Result<Error>`.

### Examples
```csharp
var result =
    await from user in _userService.GetUser(userId)
    from order in _orderService.GetOrderAsync(user, orderId)
    select Error.Conflict(typeUri, $"The order {order.Number} is already shipped.");
```


## See Also
* [Result&lt;T&gt;](../ResultT/ResultT.md)
* [Write C# Linq Queries](https://learn.microsoft.com/en-us/dotnet/csharp/linq/get-started/write-linq-queries)
