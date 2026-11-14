# LinqExtensions.Select Methods
## Definition
Namespace: [Maple.Result.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>

Enables the single-clause LINQ query syntax (a `from … select …` query without an intermediate `from`) for [Result&lt;T&gt;](../ResultT/ResultT.md).

## Overloads
| Name | Description |
| ---- | ----------- |
| [Select&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, TNext&gt;)](#selectt-tnextresultt-funct-tnext) | Projects the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) into a new [Result&lt;TNext&gt;](../ResultT/ResultT.md) by applying the provided selector function; or returns a new [Result&lt;TNext&gt;](../ResultT/ResultT.md) with the same error, otherwise. |
| [Select&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Error&gt;)](#selecttresultt-funct-error) | Applies the selector to the value of a successful result and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns. |

## Remarks
A selector that returns an [Error](../Error/Error.md) makes the query fail: such a query returns a failed [Result](../Result/Result.md) instead of a **successful** `Result<Error>`.


## Select&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, TNext&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqExtensions.cs#L49" target="_blank">LinqExtensions.cs</a>

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


## Select&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Error&gt;)
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/Extensions/LinqExtensions.cs#L86" target="_blank">LinqExtensions.cs</a>

Applies the provided `selector` to the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) and returns a failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) it returns; otherwise, returns a failed [Result](../Result/Result.md) with the original [Error](../Error/Error.md).

```csharp
public static Result Select<T>(this Result<T> result, Func<T, Error> selector);
```

### Type Parameters
#### `T`
The type of the value of the initial result, passed to the `selector`.

### Parameters
#### `result` [Result&lt;T&gt;](../ResultT/ResultT.md)
The initial result.

#### `selector` [Func](https://learn.microsoft.com/dotnet/api/system.func-2)&lt;T, [Error](../Error/Error.md)&gt;
The function that takes the value of the initial result and returns the [Error](../Error/Error.md) of the failed result.

### Returns
#### [Result](../Result/Result.md)
A failed [Result](../Result/Result.md) with the [Error](../Error/Error.md) returned by `selector` if the `result` is successful; otherwise, a failed [Result](../Result/Result.md) with the original [Error](../Error/Error.md). The returned result is always failed.

### Exceptions
#### [ArgumentNullException](https://learn.microsoft.com/dotnet/api/system.argumentnullexception)
The `result`, `selector` parameter is `null`.

#### [InvalidOperationException](https://learn.microsoft.com/dotnet/api/system.invalidoperationexception)
The `selector` function returns `null`.

### Remarks
This overload makes a query that selects an [Error](../Error/Error.md) return a failed [Result](../Result/Result.md). Without it, such a query would bind to [Select&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, TNext&gt;)](#selectt-tnextresultt-funct-tnext) and return a **successful** `Result<Error>`. To get that on purpose, specify the type arguments: `Select<T, Error>(…)`.

### Examples
```csharp
var result =
    from user in _userService.GetUser(userId)
    select Error.Conflict(typeUri, $"The user {user.Name} already exists.");
```


## See Also
* [Result&lt;T&gt;](../ResultT/ResultT.md)
* [LinqExtensions.SelectMany](LinqExtensions_SelectMany.md)
* [Write C# Linq Queries](https://learn.microsoft.com/en-us/dotnet/csharp/linq/get-started/write-linq-queries)
