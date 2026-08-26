# LinqValueTaskAsyncExtensions Class
## Definition
Namespace: [Maple.Result.Extensions.ValueTasks](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/ValueTasks/LinqValueTaskAsyncExtensions.cs" target="_blank">LinqValueTaskAsyncExtensions.cs</a>

Provides extension methods for the [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask)&lt;[Result&lt;T&gt;](../../ResultT/ResultT.md)&gt; to allow use it with asynchronous methods using LINQ query syntax.

```csharp
public static class LinqValueTaskAsyncExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → LinqValueTaskAsyncExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [Select(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, TNext&gt;)](LinqValueTaskAsyncExtensions_Select.md) | Projects the value of a successful [Result&lt;T&gt;](../../ResultT/ResultT.md) produced by the awaited result into a new [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;TNext&gt;](../../ResultT/ResultT.md)&gt; by applying the provided selector function; or returns a new [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;TNext&gt;](../../ResultT/ResultT.md)&gt; with the same error, otherwise. |
| [SelectMany(ValueTask&lt;Result&lt;T&gt;&gt;, Func&lt;T, ValueTask&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)](LinqValueTaskAsyncExtensions_SelectMany.md) | Returns a new instance of [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;TNext&gt;](../../ResultT/ResultT.md)&gt; instance by applying the provided selector functions if the current [Result&lt;T&gt;](../../ResultT/ResultT.md) if it is successful; or a new [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask-1)&lt;[Result&lt;TNext&gt;](../../ResultT/ResultT.md)&gt; instance with the same error, otherwise. |

## See Also
* [Result&lt;T&gt;](../../ResultT/ResultT.md)
* [Write C# Linq Queries](https://learn.microsoft.com/en-us/dotnet/csharp/linq/get-started/write-linq-queries)
