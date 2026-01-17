# MatchAsyncExtensions Class
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/MatchAsyncExtensions.cs" target="_blank">MatchAsyncExtensions.cs</a>

Provides extension methods for the [Result](../Result/Result.md) and [Result&lt;T&gt;](../ResultT/ResultT.md) records to handle both successful and failure scenarios.

```csharp
public static class MatchAsyncExtensions;
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → MatchAsyncExtensions

## Methods
| Name                             | Description                                                |
| -------------------------------- | ---------------------------------------------------------- |
| [MatchAsync(Result, Func&lt;Task&gt;, Func&lt;Error, Task&gt;, Boolean)](MatchAsyncExtensions_MatchAsync.md#matchasyncresult-functask-funcerror-task-boolean) | Executes one of the specified asynchronous actions as an asynchronous operation depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [MatchAsync(Result, Func&lt;Task&lt;Result&gt;&gt;, Func&lt;Error, Task&gt;, Boolean)](MatchAsyncExtensions_MatchAsync.md#matchasyncresult-functaskresult-funcerror-task-boolean) | Executes the specified asynchronous function or action as an asynchronous operation depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [MatchAsync(Result, Func&lt;Task&lt;Result&gt;&gt;, Func&lt;Error, Task&lt;Result&gt;&gt;, Boolean)](MatchAsyncExtensions_MatchAsync.md#matchasyncresult-functaskresult-funcerror-taskresult-boolean) | Executes one of the specified asynchronous functions as an asynchronous operation depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(Result, Func&lt;Task&lt;T&gt;&gt;, Func&lt;Error, Task&gt;, Boolean)](MatchAsyncExtensions_MatchAsync.md#matchasynctresult-functaskt-funcerror-task-boolean) | Executes the specified asynchronous function or action as an asynchronous operation depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(Result, Func&lt;Task&lt;T&gt;&gt;, Func&lt;Error, Task&lt;T&gt;&gt;, Boolean)](MatchAsyncExtensions_MatchAsync.md#matchasynctresult-functaskt-funcerror-taskt-boolean) | Executes one of the specified asynchronous functions as an asynchronous operation depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(Result, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Func&lt;Error, Task&gt;, Boolean)](MatchAsyncExtensions_MatchAsync.md#matchasynctresult-functaskresultt-funcerror-task-boolean) | Executes the specified asynchronous function or action as an asynchronous operation depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(Result, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Func&lt;Error, Task&lt;Result&lt;T&gt;&gt;&gt;, Boolean)](MatchAsyncExtensions_MatchAsync.md#matchasynctresult-functaskresultt-funcerror-taskresultt-boolean) | Executes one of the specified asynchronous functions as an asynchronous operation depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(Result&lt;T&gt; result, Func&lt;T, Task&gt;, Func&lt;Error, Task&gt;, Boolean)](MatchAsyncExtensions_MatchAsync.md#matchasynctresultt-result-funct-task-funcerror-task-boolean) |  Executes one of the specified asynchronous actions as an asynchronous operation depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |
| [MatchAsync&lt;T, TNext&gt;(Result&lt;T&gt; result, Func&lt;T, Task&lt;TNext&gt;&gt;, Func&lt;Error, Task&gt;, Boolean)](MatchAsyncExtensions_MatchAsync.md#matchasynct-tnextresultt-result-funct-tasktnext-funcerror-task-boolean) |  Executes the specified asynchronous function or action as an asynchronous operation depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |
| [MatchAsync&lt;T, TNext&gt;(Result&lt;T&gt; result, Func&lt;T, Task&lt;TNext&gt;&gt;, Func&lt;Error, Task&lt;TNext&gt;&gt;, Boolean)](MatchAsyncExtensions_MatchAsync.md#matchasynct-tnextresultt-result-funct-tasktnext-funcerror-tasktnext-boolean) |  Executes one of the specified asynchronous functions as an asynchronous operation depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |
| [MatchAsync&lt;T, TNext&gt;(Result&lt;T&gt; result, Func&lt;T, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Func&lt;Error, Task&gt;, Boolean)](MatchAsyncExtensions_MatchAsync.md#matchasynct-tnextresultt-result-funct-taskresulttnext-funcerror-task-boolean) |  Executes the specified asynchronous function or action as an asynchronous operation depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |
| [MatchAsync&lt;T, TNext&gt;(Result&lt;T&gt; result, Func&lt;T, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Func&lt;Error, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)](MatchAsyncExtensions_MatchAsync.md#matchasynct-tnextresultt-result-funct-taskresulttnext-funcerror-taskresulttnext-boolean) |  Executes one of the specified asynchronous functions as an asynchronous operation depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |

## See Also
* [Error](../Error/Error.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
