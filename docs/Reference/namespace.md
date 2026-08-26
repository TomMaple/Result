# Maple.Result Namespace
Provides records and interfaces with the types to use as a returned value of an operation—that can be either successful (with a value or without) or failure (with an error).

## Records
| Name | Description |
| ---- | ----------- |
| [Error](Error/Error.md) | Represents the error of the failed operation. |
| [ErrorDetail](ErrorDetail/ErrorDetail.md) | Represents one (of possibly many) problem details providing additional information about an error. |
| [ErrorUri](ErrorUri/ErrorUri.md) | Represents a URI used to identify the type or instance of an error. |
| [Result](Result/Result.md) | Represents the result of an operation without a value that can either be successful or failed. |
| [Result&lt;T&gt;](ResultT/ResultT.md) | Represents the result of an operation with a value that can either be successful (with the value) or failed (with an error). |
| [TemplatedMessage](TemplatedMessage/TemplatedMessage.md) | Represents a message template with named values to be used for rendering a localized message. |

## Interfaces
| Name | Description |
| ---- | ----------- |
| [IResult](IResult/IResult.md) | Represents the result of an operation (with or without a value) that can either be successful or failed. |

## Enums
| Name | Description |
| ---- | ----------- |
| [ErrorCategory](ErrorCategory.md) | Specifies the category of the error that occurred during an operation. |

## See Also
* [Maple.Result.Extensions namespace](Extensions/namespace.md)
* [Maple.Result.Extensions.ValueTasks namespace](Extensions/ValueTasks/namespace.md)
