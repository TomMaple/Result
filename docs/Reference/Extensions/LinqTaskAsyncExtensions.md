# LinqTaskAsyncExtensions Class
## Definition
Namespace: [Maple.Result.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/LinqTaskAsyncExtensions.cs" target="_blank">LinqTaskAsyncExtensions.cs</a>

Provides extension methods for the [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task)&lt;[Result&lt;T&gt;](../ResultT/ResultT.md)&gt; to allow use it with asynchronous methods using LINQ query syntax.

```csharp
public static class LinqTaskAsyncExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → LinqTaskAsyncExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [SelectMany(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&lt;TMiddle&gt;&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)](LinqTaskAsyncExtensions_SelectMany.md) | Returns a new instance of [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; instance by applying the provided selector functions if the current [Result&lt;T&gt;](../ResultT/ResultT.md) if it is successful; or a new [Task](https://learn.microsoft.com/dotnet/api/system.threading.tasks.task-1)&lt;[Result&lt;TNext&gt;](../ResultT/ResultT.md)&gt; instance with the same error, otherwise. |

## See Also
* [Result&lt;T&gt;](../ResultT/ResultT.md)
* [Write C# Linq Queries](https://learn.microsoft.com/en-us/dotnet/csharp/linq/get-started/write-linq-queries)
