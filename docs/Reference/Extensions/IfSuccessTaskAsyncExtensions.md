# IfSuccessTaskAsyncExtensions Class
## Definition
Namespace: [Maple.Result.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/IfSuccessTaskAsyncExtensions.cs" target="_blank">IfSuccessTaskAsyncExtensions.cs</a>

Provides extension methods for the [Result](../Result/Result.md) and [Result&lt;T&gt;](../ResultT/ResultT.md) records to handle successful scenarios.

```csharp
public static class IfSuccessTaskAsyncExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → IfSuccessTaskAsyncExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [IfSuccessAsync(Result, Func&lt;Task&gt;, Boolean)](IfSuccessTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasyncresult-functask-boolean) | Executes the specified asynchronous action, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync(Result, Func&lt;Task&lt;Result&gt;&gt;, Boolean)](IfSuccessTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasyncresult-functaskresult-boolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(Result, Func&lt;Task&lt;T&gt;&gt;, Boolean)](IfSuccessTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctresult-functaskt-boolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(Result, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Boolean)](IfSuccessTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctresult-functaskresultt-boolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Task&gt;, Boolean)](IfSuccessTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctresultt-funct-task-boolean) | Executes the specified asynchronous action, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Boolean)](IfSuccessTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctresultt-funct-taskresult-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;TNext&gt;&gt;, Boolean)](IfSuccessTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasynct-tnextresultt-funct-tasktnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)](IfSuccessTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasynct-tnextresultt-funct-taskresulttnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync(Task&lt;Result&gt;, Func&lt;Task&gt;, Boolean)](IfSuccessTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctaskresult-functask-boolean) | Executes the specified asynchronous action, if the provided asynchronous operation returns a successful [Result](../Result/Result.md) represents a successful operation. |
| [IfSuccessAsync(Task&lt;Result&gt;, Func&lt;Task&lt;Result&gt;&gt;, Boolean)](IfSuccessTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctaskresult-functaskresult-boolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../Result/Result.md) represents a successful operation. |
| [IfSuccessAsync&lt;T&gt;(Task&lt;Result&gt;, Func&lt;Task&lt;T&gt;&gt;, Boolean)](IfSuccessTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasyncttaskresult-functaskt-boolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../Result/Result.md) represents a successful operation. |
| [IfSuccessAsync&lt;T&gt;(Task&lt;Result&gt;, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Boolean)](IfSuccessTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasyncttaskresult-functaskresultt-boolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../Result/Result.md) represents a successful operation. |
| [IfSuccessAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&gt;, Boolean)](IfSuccessTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasyncttaskresultt-funct-task-boolean) | Executes the specified asynchronous action, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Boolean)](IfSuccessTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasyncttaskresultt-funct-taskresult-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;TNext&gt;&gt;, Boolean)](IfSuccessTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasynct-tnexttaskresultt-funct-tasktnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T, TNext&gt;(Task&lt;Result&lt;T&gt;&gt;, Func&lt;T, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)](IfSuccessTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasynct-tnexttaskresultt-funct-taskresulttnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |

## See Also
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
