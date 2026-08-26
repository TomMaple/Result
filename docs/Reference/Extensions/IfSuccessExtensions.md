# IfSuccessExtensions Class
## Definition
Namespace: [Maple.Result.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/IfSuccessExtensions.cs" target="_blank">IfSuccessExtensions.cs</a>

Provides extension methods for the [Result](../Result/Result.md) and [Result&lt;T&gt;](../ResultT/ResultT.md) records to handle successful scenarios.

```csharp
public static class IfSuccessExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → IfSuccessExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [IfSuccess(Result, Action)](IfSuccessExtensions_IfSuccess.md#ifsuccessresult-action) | Executes the specified action, if the [Result](../Result/Result.md) represents a successful operation. |
| [IfSuccess(Result, Func&lt;Result&gt;)](IfSuccessExtensions_IfSuccess.md#ifsuccessresult-funcresult) | Executes the specified function, if the [Result](../Result/Result.md) represents a successful operation. |
| [IfSuccess&lt;T&gt;(Result, Func&lt;T&gt;)](IfSuccessExtensions_IfSuccess.md#ifsuccesstresult-funct) | Executes the specified function, if the [Result](../Result/Result.md) represents a successful operation. |
| [IfSuccess&lt;T&gt;(Result, Func&lt;Result&lt;T&gt;&gt;)](IfSuccessExtensions_IfSuccess.md#ifsuccesstresult-funcresultt) | Executes the specified function, if the [Result](../Result/Result.md) represents a successful operation. |
| [IfSuccess&lt;T&gt;(Result&lt;T&gt;, Action&lt;T&gt;)](IfSuccessExtensions_IfSuccess.md#ifsuccesstresultt-actiont) | Executes the specified action, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. |
| [IfSuccess&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Result&gt;)](IfSuccessExtensions_IfSuccess.md#ifsuccesstresultt-funct-result) | Executes the specified function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. |
| [IfSuccess&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, TNext&gt;)](IfSuccessExtensions_IfSuccess.md#ifsuccesst-tnextresultt-funct-tnext) | Executes the specified function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. |
| [IfSuccess&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Result&lt;TNext&gt;&gt;)](IfSuccessExtensions_IfSuccess.md#ifsuccesst-tnextresultt-funct-resulttnext) | Executes the specified function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. |

## See Also
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
