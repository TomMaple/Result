# IfErrorExtensions Class
## Definition
Namespace: [Maple.Result.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/IfErrorExtensions.cs" target="_blank">IfErrorExtensions.cs</a>

Provides extension methods for the [IResult](../IResult/IResult.md) interface implementations to handle error scenarios.

```csharp
public static class IfErrorExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → IfErrorExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [IfError&lt;TResult&gt;(TResult, Action&lt;Error&gt;)](IfErrorExtensions_IfError.md#iferrortresulttresult-actionerror) | Executes the provided action if the `TResult` instance represents a failed operation. |
| [IfError&lt;TResult&gt;(TResult, Func&lt;Error, TResult&gt;)](IfErrorExtensions_IfError.md#iferrortresulttresult-funcerror-tresult) | Executes the provided function if the `TResult` instance represents a failed operation and returns its result. |

## See Also
* [Error](../Error/Error.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
