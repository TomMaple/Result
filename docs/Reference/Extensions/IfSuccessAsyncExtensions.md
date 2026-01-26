# IfSuccessAsyncExtensions Class
## Definition
Namespace: [Maple.Result.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/IfSuccessAsyncExtensions.cs" target="_blank">IfSuccessAsyncExtensions.cs</a>

Provides extension methods for the [Result](../Result/Result.md) and [Result&lt;T&gt;](../ResultT/ResultT.md) records to handle successful scenarios.

```csharp
public static class IfSuccessAsyncExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → IfSuccessAsyncExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [IfSuccessAsync(Result, Func&lt;Task&gt;, Boolean)](IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasyncresult-functask-boolean) | Executes the specified asynchronous action, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync(Result, Func&lt;Task&lt;Result&gt;&gt;, Boolean)](IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasyncresult-functaskresult-boolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(Result, Func&lt;Task&lt;T&gt;&gt;, Boolean)](IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctresult-functaskt-boolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(Result, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Boolean)](IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctresult-functaskresultt-boolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Task&gt;, Boolean)](IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctresultt-funct-task-boolean) | Executes the specified asynchronous action, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Boolean)](IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctresultt-funct-taskresult-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;TNext&gt;&gt;, Boolean)](IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasynct-tnextresultt-funct-tasktnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)](IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasynct-tnextresultt-funct-taskresulttnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |

## See Also
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
