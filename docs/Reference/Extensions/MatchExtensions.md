# MatchExtensions Class
## Definition
Namespace: [Maple.Result.Extensions](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Extensions/MatchExtensions.cs" target="_blank">MatchExtensions.cs</a>

Provides extension methods for the [Result](../Result/Result.md) and [Result&lt;T&gt;](../ResultT/ResultT.md) records to handle both successful and failure scenarios.

```csharp
public static class MatchExtensions
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → MatchExtensions

## Methods
| Name | Description |
| ---- | ----------- |
| [Match(Result, Action, Action&lt;Error&gt;)](MatchExtensions_Match.md#matchresult-func-funcerror-boolean) | Executes one of the specified actions depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [Match(Result, Func&lt;Result&gt;, Action&lt;Error&gt;)](MatchExtensions_Match.md#matchresult-funcresult-funcerror-boolean) | Executes the specified function or action depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [Match(Result, Func&lt;Result&gt;, Func&lt;Error, Result&gt;)](MatchExtensions_Match.md#matchresult-funcresult-funcerror-result-boolean) | Executes one of the specified functions depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [Match&lt;T&gt;(Result, Func&lt;T&gt;, Action&lt;Error&gt;)](MatchExtensions_Match.md#matchtresult-funct-funcerror-boolean) | Executes the specified function or action depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [Match&lt;T&gt;(Result, Func&lt;T&gt;, Func&lt;Error, T&gt;)](MatchExtensions_Match.md#matchtresult-funct-funcerror-t-boolean) | Executes one of the specified functions depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [Match&lt;T&gt;(Result, Func&lt;Result&lt;T&gt;&gt;, Action&lt;Error&gt;)](MatchExtensions_Match.md#matchtresult-funcresultt-funcerror-boolean) | Executes the specified function or action depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [Match&lt;T&gt;(Result, Func&lt;Result&lt;T&gt;&gt;, Func&lt;Error, Result&lt;T&gt;&gt;)](MatchExtensions_Match.md#matchtresult-funcresultt-funcerror-resultt-boolean) | Executes one of the specified functions depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [Match&lt;T&gt;(Result&lt;T&gt;, Action&lt;T&gt;, Action&lt;Error&gt;)](MatchExtensions_Match.md#matchtresultt-result-funct-funcerror-boolean) |  Executes one of the specified actions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |
| [Match&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, TNext&gt;, Action&lt;Error&gt;)](MatchExtensions_Match.md#matcht-tnextresultt-result-funct-tnext-funcerror-boolean) |  Executes the specified function or action depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |
| [Match&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, TNext&gt;, Func&lt;Error, TNext&gt;)](MatchExtensions_Match.md#matcht-tnextresultt-result-funct-tnext-funcerror-tnext-boolean) |  Executes one of the specified functions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |
| [Match&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Result&lt;TNext&gt;&gt;, Action&lt;Error&gt;)](MatchExtensions_Match.md#matcht-tnextresultt-result-funct-resulttnext-funcerror-boolean) |  Executes the specified function or action depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |
| [Match&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Result&lt;TNext&gt;&gt;, Func&lt;Error, Result&lt;TNext&gt;&gt;)](MatchExtensions_Match.md#matcht-tnextresultt-result-funct-resulttnext-funcerror-resulttnext-boolean) |  Executes one of the specified functions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |

## See Also
* [Error](../Error/Error.md)
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
