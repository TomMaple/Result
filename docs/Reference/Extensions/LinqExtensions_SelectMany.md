# LinqExtensions.SelectMany(Result&lt;T&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;) Method
## Definition
Namespace: [Maple.Result.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/LinqExtensions.cs#L100" target="_blank">LinqExtensions.cs</a>

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


## See Also
* [Result&lt;T&gt;](../ResultT/ResultT.md)
* [Write C# Linq Queries](https://learn.microsoft.com/en-us/dotnet/csharp/linq/get-started/write-linq-queries)
