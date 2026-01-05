# IfErrorAsyncExtensions Class
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/IfErrorAsyncExtensions.cs" target="_blank">IfErrorExtensions.cs</a>

Provides extension methods for the [IResult](../IResult/IResult.md) interface implementations to handle error scenarios.

```csharp
public static class IfErrorAsyncExtensions;
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → IfErrorAsyncExtensions

## Methods
| Name                             | Description                                                |
| -------------------------------- | ---------------------------------------------------------- |
| [IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, Task&gt;, Boolean)](IfErrorAsyncExtensions_IfError.md#iferrorasynctresulttresult-actionerror)  | Executes the provided asynchronous action, if the `TResult` instance represents a failed operation, as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, Task&lt;TResult&gt;&gt;, Boolean)](IfErrorExtensions_IfError.md#iferrorasynctresulttresult-funcerror-tresult) | Executes the provided asynchronous function, if the `TResult` instance represents a failed operation and returns its result, as an asynchronous operation. |

## See Also
* [Error](../Error/Error.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
