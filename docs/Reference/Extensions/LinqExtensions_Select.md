# LinqExtensions.Select&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, TNext&gt;) Method
## Definition
Namespace: [Maple.Result.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/LinqExtensions.cs#L53" target="_blank">LinqExtensions.cs</a>

Projects the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) into a new [Result&lt;TNext&gt;](../ResultT/ResultT.md) by applying the provided selector function; or returns a new [Result&lt;TNext&gt;](../ResultT/ResultT.md) with the same error, otherwise.


```csharp
public static Result<TNext> Select<T, TNext>(
    this Result<T> result,
    Func<T, TNext> selector);
```

### Type Parameters
#### `T`
The type of the [Result](../ResultT/ResultT.md)&lt;T&gt; `result` value used to determine whether to execute the passed selector function. If successful, this is also the type of the parameter passed to that function.

#### `TNext`
The output type of the passed selector function and the type of the returned [Result](../ResultT/ResultT.md)&lt;TNext&gt; `result` value.

### Parameters
#### `result` [Result](../ResultT/ResultT.md)&lt;T&gt;
The instance of [Result](../ResultT/ResultT.md) record that represents the result of the operation to be evaluated.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, TNext&gt;
The function that maps the value of the initial result to the final value.

### Returns
#### [Result](../ResultT/ResultT.md)&lt;TNext&gt;
A successful [Result&lt;TNext&gt;](../ResultT/ResultT.md) containing the value produced by the selector if the `result` is successful; otherwise, a failed [Result&lt;TNext&gt;](../ResultT/ResultT.md) with the original [Error](../Error/Error.md).

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
If any of the `result` or `selector` parameters are `null`.

### Remarks
This method is used to enable the single-clause LINQ query syntax (a `from … select …` query without an intermediate `from`) for [Result&lt;T&gt;](../ResultT/ResultT.md) records. It projects a successful value while propagating an error unchanged. To chain multiple operations that each return a [Result&lt;T&gt;](../ResultT/ResultT.md), use [SelectMany](LinqExtensions_SelectMany.md).

### Examples
```csharp
var displayNameResult =
    from user in _userService.GetUser(userId)
    select $"{user.FirstName} {user.LastName}";
```


## See Also
* [Result&lt;T&gt;](../ResultT/ResultT.md)
* [LinqExtensions.SelectMany](LinqExtensions_SelectMany.md)
* [Write C# Linq Queries](https://learn.microsoft.com/en-us/dotnet/csharp/linq/get-started/write-linq-queries)
