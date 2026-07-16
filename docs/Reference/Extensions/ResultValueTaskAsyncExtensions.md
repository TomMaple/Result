# ResultValueTaskAsyncExtensions Class
## Definition
Namespace: [Maple.Result.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/ResultValueTaskAsyncExtensions.cs" target="_blank">ResultValueTaskAsyncExtensions.cs</a>

Provides asynchronous extension methods, represented by [ValueTask](https://learn.microsoft.com/dotnet/api/system.threading.tasks.valuetask), for the [Result&lt;T&gt;](../ResultT/ResultT.md) records.

```csharp
public static class ResultValueTaskAsyncExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → ResultValueTaskAsyncExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [ToResultAsync(ValueTask&lt;Result&lt;T&gt;&gt;)](ResultValueTaskAsyncExtensions_ToResultAsync.md) | Asynchronously converts the outcome of the asynchronous operation represented by the [Result&lt;T&gt;](../ResultT/ResultT.md) to the non-generic [Result](../Result/Result.md). |

## See Also
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
