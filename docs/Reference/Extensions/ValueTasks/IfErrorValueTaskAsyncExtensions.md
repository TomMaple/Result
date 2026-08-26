# IfErrorValueTaskAsyncExtensions Class
## Definition
Namespace: [Maple.Result.Extensions.ValueTasks](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/ValueTasks/IfErrorValueTaskAsyncExtensions.cs" target="_blank">IfErrorValueTaskAsyncExtensions.cs</a>

Provides extension methods, represented by [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask), for the [IResult](../../IResult/IResult.md) interface implementations to handle error scenarios.

```csharp
public static class IfErrorValueTaskAsyncExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → IfErrorValueTaskAsyncExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, ValueTask&gt;, Boolean)](IfErrorValueTaskAsyncExtensions_IfErrorAsync.md#iferrorasynctresulttresult-funcerror-valuetask-boolean)  | Executes the provided asynchronous action, if the `TResult` instance represents a failed operation, as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, ValueTask&lt;TResult&gt;&gt;, Boolean)](IfErrorValueTaskAsyncExtensions_IfErrorAsync.md#iferrorasynctresulttresult-funcerror-valuetasktresult-boolean) | Executes the provided asynchronous function, if the `TResult` instance represents a failed operation and returns its result, as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(ValueTask&lt;TResult&gt;, Func&lt;Error, ValueTask&gt;, Boolean)](IfErrorValueTaskAsyncExtensions_IfErrorAsync.md#iferrorasynctresultvaluetasktresult-funcerror-valuetask-boolean)  | Executes the provided asynchronous function, if the provided asynchronous operation returns a failed `TResult` instance, and returns its result as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(ValueTask&lt;TResult&gt;, Func&lt;Error, ValueTask&lt;TResult&gt;&gt;, Boolean)](IfErrorValueTaskAsyncExtensions_IfErrorAsync.md#iferrorasynctresultvaluetasktresult-funcerror-valuetasktresult-boolean) | Executes the provided asynchronous function, if the provided asynchronous operation returns a failed `TResult` instance, and returns its result as an asynchronous operation. |

## See Also
* [Error](../../Error/Error.md)
* [Result](../../Result/Result.md)
* [Result&lt;T&gt;](../../ResultT/ResultT.md)
