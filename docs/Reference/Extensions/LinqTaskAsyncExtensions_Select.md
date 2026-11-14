# LinqTaskAsyncExtensions.Select Methods
## Definition
Namespace: [Maple.Result.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Enables the single-clause LINQ query syntax (a `from … select …` query without an intermediate `from`) for [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;.

## Overloads
| Name | Description |
| ---- | ----------- |
| [Select&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, TNext&gt;)](#selectt-tnexttaskresultt-funct-tnext) | Projects the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) produced by the awaited `result` into a new [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; by applying the provided selector function; or returns a new [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; with the same error, otherwise. |
| [Select&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Error&gt;)](#selectttaskresultt-funct-error) | Applies the selector to the value of a successful result and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns. |

## Remarks
A selector that returns an [Error](../Error/Error.md) makes the query fail: such a query returns a failed [Result](../Result/Result.md) instead of a **successful** `Result<Error>`.


## Select&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L58" target="_blank">LinqTaskAsyncExtensions.cs</a>

Projects the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) produced by the awaited `result` into a new [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; by applying the provided selector function; or returns a new [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; with the same error, otherwise.

```csharp
public static async Task<Result<TNext>> Select<T, TNext>(
    this Task<Result<T>> result,
    Func<T, TNext> selector);
```

### Type Parameters
#### `T`
The type of the [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt; `result` value used to determine whether to execute the passed selector function. If successful, this is also the type of the parameter passed to that function.

#### `TNext`
The output type of the passed selector function and the type of the returned [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt; `result` value.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The task object representing the asynchronous operation that returns an instance of a [Result](../ResultT/ResultT.md) record to be evaluated.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, TNext&gt;
The function that maps the value of the initial result to the final value.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A task object representing the asynchronous operation. Its result is a successful [Result&lt;TNext&gt;](../ResultT/ResultT.md) containing the value produced by the selector if the `result` is successful; otherwise, a failed [Result&lt;TNext&gt;](../ResultT/ResultT.md) with the original [Error](../Error/Error.md).

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If any of the `result` or `selector` parameters are `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`.

### Remarks
This method is used to enable the single-clause LINQ query syntax (a `from … select …` query without an intermediate `from`) for [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;. It projects a successful value while propagating an error unchanged. To chain multiple asynchronous operations that each return a [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;, use [SelectMany](LinqTaskAsyncExtensions_SelectMany.md).

### Examples
```csharp
var displayNameResult =
    await from user in _userService.GetUserAsync(userId)
    select $"{user.FirstName} {user.LastName}";
```


## Select&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L103" target="_blank">LinqTaskAsyncExtensions.cs</a>

Applies the provided `selector` to the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) produced by the awaited `result` and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns; otherwise, returns a failed [Result](../Result/Result.md) with the original [Error](../Error/Error.md).

```csharp
public static async Task<Result> Select<T>(this Task<Result<T>> result, Func<T, Error> selector);
```

### Type Parameters
#### `T`
The type of the value of the initial result, passed to the `selector`.

### Parameters
#### `result` [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt;
The task object representing the asynchronous operation that returns the initial result.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Error](../Error/Error.md)&gt;
The function that takes the value of the initial result and returns the [Error](../Error/Error.md) of the failed result.

### Returns
#### [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result](../Result/Result.md)&gt;
The task object representing the asynchronous operation: a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `selector` if the `result` is successful; otherwise, a failed [Result](../Result/Result.md) with the original [Error](../Error/Error.md). The returned result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result`, `selector` parameter is `null`, or `selector` returns `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The asynchronous operation represented by `result` returns `null`.

### Remarks
This overload makes a query that selects an [Error](../Error/Error.md) return a failed [Result](../Result/Result.md). Without it, such a query would bind to [Select&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, TNext&gt;)](#selectt-tnexttaskresultt-funct-tnext) and return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `Select<T, Error>(…)`.

### Examples
```csharp
var result =
    await from user in _userService.GetUserAsync(userId)
    select Error.Conflict(typeUri, $"The user {user.Name} already exists.");
```


## See Also
* [Result&lt;T&gt;](../ResultT/ResultT.md)
* [LinqTaskAsyncExtensions.SelectMany](LinqTaskAsyncExtensions_SelectMany.md)
* [Write C# Linq Queries](https://learn.microsoft.com/en-us/dotnet/csharp/linq/get-started/write-linq-queries)
