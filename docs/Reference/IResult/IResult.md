# IResult interface
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Result.cs#L27" target="_blank">Result.cs</a>

Represents the outcome of an operation that can either succeed or fail with an [Error](../Error/Error.md).

```csharp
public interface IResult
```

Derived [Maple.Result.Result](../Result/Result.md), [Maple.Result.Result&lt;T&gt;](../ResultT/ResultT.md)

## Properties
| Name | Description |
| ---- | ----------- |
| [Error](IResult_Error.md) | Gets or sets the error details if the result represents a failure. |

## Methods
| Name | Description |
| ---- | ----------- |
| [IsSuccess()](IResult_IsSuccess.md) | Returns an indicator whether the result represents a successful operation. |

## Extension Methods
| Name | Description |
| ---- | ----------- |
| [IfError&lt;TResult&gt;(TResult, Action&lt;Error&gt;)](../Extensions/IfErrorExtensions_IfError.md#iferrortresulttresult-actionerror) | Executes the specified action, if the [IResult](IResult.md) represents a failure. |
| [IfError&lt;TResult&gt;(TResult, Func&lt;Error, Result&gt;)](../Extensions/IfErrorExtensions_IfError.md#iferrortresulttresult-funcerror-tresult) | Executes the specified function, if the [IResult](IResult.md) represents a failure. |
| [IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, Task&gt;, Boolean)](../Extensions/IfErrorTaskAsyncExtensions_IfErrorAsync.md#iferrorasynctresulttresult-funcerror-task-boolean) | Executes the specified asynchronous function, if the [IResult](IResult.md) represents a failure, as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, Task&lt;TResult&gt;&gt;, Boolean)](../Extensions/IfErrorTaskAsyncExtensions_IfErrorAsync.md#iferrorasynctresulttresult-funcerror-tasktresult-boolean) | Executes the specified asynchronous function, if the [IResult](IResult.md) represents a failure, as an asynchronous operation. |

## See also
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
