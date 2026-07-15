# LinqExtensions.SelectMany(Result&lt;T&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;) Method
## Definition
Namespace: [Maple.Result.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs#L72" target="_blank">LinqTaskAsyncExtensions.cs</a>

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


## See Also
* [Result&lt;T&gt;](../ResultT/ResultT.md)
* [Write C# Linq Queries](https://learn.microsoft.com/en-us/dotnet/csharp/linq/get-started/write-linq-queries)
