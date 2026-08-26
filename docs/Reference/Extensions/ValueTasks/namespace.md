# Maple.Result.Extensions.ValueTasks Namespace
Provides classes with the [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)-based asynchronous extension methods for the [Result](../../Result/Result.md) and [Result&lt;T&gt;](../../ResultT/ResultT.md) types.

These extension methods are kept in a separate namespace on purpose. Their [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)-based counterparts in the [Maple.Result.Extensions](../namespace.md) namespace declare the same method names, and an `async` lambda passed to them is convertible to both a `Func<Task>` and a `Func<ValueTask>` delegate—so having both sets in scope makes such a call ambiguous (*CS0121*). Import the namespace matching the style you use:

```csharp
using Maple.Result.Extensions;             // Task-based MatchAsync, IfSuccessAsync, …
```

```csharp
using Maple.Result.Extensions.ValueTasks;  // ValueTask-based MatchAsync, IfSuccessAsync, …
```

If both namespaces have to be imported in one file, disambiguate the call by using the static method syntax—for example `MatchValueTaskAsyncExtensions.MatchAsync(result, …)`—or by passing an explicitly typed delegate.

## Classes
| Name | Description |
| ---- | ----------- |
| [IfErrorValueTaskAsyncExtensions](IfErrorValueTaskAsyncExtensions.md) | Provides asynchronous extension methods, represented by [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask), for the [IResult](../../IResult/IResult.md) interface implementations to handle error scenarios. |
| [IfSuccessValueTaskAsyncExtensions](IfSuccessValueTaskAsyncExtensions.md) | Provides asynchronous extension methods, represented by [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask), for the [Result](../../Result/Result.md) and [Result&lt;T&gt;](../../ResultT/ResultT.md) records to handle success scenarios. |
| [LinqValueTaskAsyncExtensions](LinqValueTaskAsyncExtensions.md) | Provides extension methods for the ValueTask&lt;[Result&lt;T&gt;](../../ResultT/ResultT.md)&gt; to allow using them with LINQ query syntax. |
| [MatchValueTaskAsyncExtensions](MatchValueTaskAsyncExtensions.md) | Provides asynchronous extension methods, represented by [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask), for the [Result](../../Result/Result.md) and [Result&lt;T&gt;](../../ResultT/ResultT.md) records to handle both success and error scenarios. |
| [ResultValueTaskAsyncExtensions](ResultValueTaskAsyncExtensions.md) | Provides asynchronous extension methods, represented by [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask), for the [Result&lt;T&gt;](../../ResultT/ResultT.md) records. |

## See Also
* [Maple.Result namespace](../../namespace.md)
* [Maple.Result.Extensions namespace](../namespace.md)
