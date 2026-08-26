# LinqValueTaskAsyncExtensions.Select&lt;T, TNext&gt;(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, TNext&gt;) Method
## Definition
Namespace: [Maple.Result.Extensions.ValueTasks](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/ValueTasks/LinqValueTaskAsyncExtensions.cs#L63" target="_blank">LinqValueTaskAsyncExtensions.cs</a>

Projects the value of a successful [Result&lt;T&gt;](../../ResultT/ResultT.md) produced by the awaited `result` into a new [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;TNext&gt;](../../ResultT/ResultT.md)&gt; by applying the provided selector function; or returns a new [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;TNext&gt;](../../ResultT/ResultT.md)&gt; with the same error, otherwise.


```csharp
public static async ValueTask<Result<TNext>> Select<T, TNext>(
    this ValueTask<Result<T>> result,
    Func<T, TNext> selector);
```

### Type Parameters
#### `T`
The type of the [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../../ResultT/ResultT.md)&gt; `result` value used to determine whether to execute the passed selector function. If successful, this is also the type of the parameter passed to that function.

#### `TNext`
The output type of the passed selector function and the type of the returned [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../ResultT/ResultT.md)&lt;TNext&gt;&gt; `result` value.

### Parameters
#### `result` [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../../ResultT/ResultT.md)&gt;
The value task object representing the asynchronous operation that returns an instance of a [Result](../../ResultT/ResultT.md) record to be evaluated.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, TNext&gt;
The function that maps the value of the initial result to the final value.

### Returns
#### [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result](../../ResultT/ResultT.md)&lt;TNext&gt;&gt;
A value task object representing the asynchronous operation. Its result is a successful [Result&lt;TNext&gt;](../../ResultT/ResultT.md) containing the value produced by the selector if the `result` is successful; otherwise, a failed [Result&lt;TNext&gt;](../../ResultT/ResultT.md) with the original [Error](../../Error/Error.md).

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If the `selector` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
If the asynchronous operation represented by `result` returns `null`.

### Remarks
This method is used to enable the single-clause LINQ query syntax (a `from … select …` query without an intermediate `from`) for [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../../ResultT/ResultT.md)&gt;. It projects a successful value while propagating an error unchanged. To chain multiple asynchronous operations that each return a [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;T&gt;](../../ResultT/ResultT.md)&gt;, use [SelectMany](LinqValueTaskAsyncExtensions_SelectMany.md).

### Examples
```csharp
var displayNameResult =
    await from user in _userService.GetUserAsync(userId)
    select $"{user.FirstName} {user.LastName}";
```


## See Also
* [Result&lt;T&gt;](../../ResultT/ResultT.md)
* [LinqValueTaskAsyncExtensions.SelectMany](LinqValueTaskAsyncExtensions_SelectMany.md)
* [Write C# Linq Queries](https://learn.microsoft.com/en-us/dotnet/csharp/linq/get-started/write-linq-queries)
