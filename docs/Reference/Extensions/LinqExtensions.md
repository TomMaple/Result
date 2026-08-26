# LinqExtensions Class
## Definition
Namespace: [Maple.Result.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/LinqExtensions.cs" target="_blank">LinqExtensions.cs</a>

Provides extension methods for the [Result&lt;T&gt;](../ResultT/ResultT.md) records to allow using them with LINQ query syntax.

```csharp
public static class LinqAsyncExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → LinqExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [Select(Result&lt;T&gt;, Func&lt;T, TNext&gt;)](LinqExtensions_Select.md) | Projects the value of a successful [Result&lt;T&gt;](../ResultT/ResultT.md) into a new [Result&lt;TNext&gt;](../ResultT/ResultT.md) by applying the provided selector function; or returns a new [Result&lt;TNext&gt;](../ResultT/ResultT.md) with the same error, otherwise. |
| [SelectMany(Result&lt;T&gt;, Func&lt;T, Result&lt;TMiddle&gt;&gt;, Func&lt;T, TMiddle, TNext&gt;)](LinqExtensions_SelectMany.md) | Returns a new instance of [Result&lt;TNext&gt;](../ResultT/ResultT.md) instance by applying the provided selector functions if the current [Result&lt;T&gt;](../ResultT/ResultT.md) if it is successful, or a new [Result&lt;TNext&gt;](../ResultT/ResultT.md) instance with the same error otherwise. |

## See Also
* [Result&lt;T&gt;](../ResultT/ResultT.md)
* [Write C# Linq Queries](https://learn.microsoft.com/en-us/dotnet/csharp/linq/get-started/write-linq-queries)
