# IResult interface
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Result.cs#L23" target="_blank">Result.cs</a>

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

## See also
* [Result](../Result/Result.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
