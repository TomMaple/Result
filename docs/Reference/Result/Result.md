# Result Record
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Result.cs#L49" target="_blank">Result.cs</a>

Represents the outcome of an operation that can either succeed or fail with an [Error](../Error/Error.md).

```csharp
public sealed record Result : IResult
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → Result

Implements [IResult](../IResult/IResult.md)

## Remarks
> [!NOTE]
> This type is used to represent the result of an operation that does not return a value.
> 
> For operations that return a value, consider using the generic [Result&lt;T&gt;](../ResultT/ResultT.md) record.

If the operation is successful, the [IsSuccess()](Result_IsSuccess.md) method will return `true`; otherwise, it will return `false`, and the [Error](Result_Error.md) property will contain details about the failure.

> [!CAUTION]
> Use static factory methods to create instances of this record, such as [Result.Success()](Result_Success.md) and [Result.FromError(Error)](Result_FromError.md), or the [Implicit(Error to Result)](Result_implicit_Error_to_Result.md) operator rather than using the constructor directly.

## Examples
```csharp
public Result DeleteUser(int userId)
{
    if (!UserExists(userId))
    {
        return Error.NotFound(
            ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:not-found"),
            "User has not been found.",
            "User with the specified ID does not exist in the system.",
            ErrorUri.Locator("https://api.exampleapp.com/error/e-1234-5678-9012-345678901234/details"),
            "errors-user-notfound",
            ("userId", userId));
    }

    // Proceed with user deletion

    return Result.Success();
}
```

## Constructors
| Name | Description |
| ---- | ----------- |
| [Result()](Result_constructors.md) | Initializes a new instance of the [Result](Result.md) record. |

## Properties
| Name | Description |
| ---- | ----------- |
| [Error](Result_Error.md) | Gets or sets the error details if the result represents a failure. |

## Methods
| Name | Description |
| ---- | ----------- |
| [FromError(Error)](Result_FromError.md) | Creates a new failure [Result](Result.md) from the specified [Error](../Error/Error.md). |
| [FromValue&lt;T&gt;(T)](Result_FromValue.md) | Creates a new successful [Result&lt;T&gt;](../ResultT/ResultT.md) containing the specified value. |
| [IsSuccess()](Result_IsSuccess.md) | Returns an indicator whether the result represents a successful operation. |
| [Success()](Result_Success.md) | Creates a new instance of successful [Result](Result.md). |

## Operators
| Name | Description |
| ---- | ----------- |
| [Implicit(Error to Result)](Result_implicit_Error_to_Result.md) | Defines an implicit conversion of a given [Error](../Error/Error.md) to a failure [Result](Result.md). |

## Extension Methods
| Name | Description |
| ---- | ----------- |
| [IfError&lt;TResult&gt;(TResult, Action&lt;Error&gt;)](../Extensions/IfErrorExtensions_IfError.md#iferrortresulttresult-actionerror) | Executes the provided action if the [Result](../Result/Result.md) instance represents a failed operation. |
| [IfError&lt;TResult&gt;(TResult, Func&lt;Error, TResult&gt;)](../Extensions/IfErrorExtensions_IfError.md#iferrortresulttresult-funcerror-tresult) | Executes the provided function if the [Result](../Result/Result.md) instance represents a failed operation and returns its result. |
| [IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, Task&gt;, Boolean)](../Extensions/IfErrorTaskAsyncExtensions_IfErrorAsync.md#iferrorasynctresulttresult-funcerror-task-boolean)  | Executes the provided asynchronous action, if the [Result](../Result/Result.md) instance represents a failed operation, as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, Task&lt;TResult&gt;&gt;, Boolean)](../Extensions/IfErrorTaskAsyncExtensions_IfErrorAsync.md#iferrorasynctresulttresult-funcerror-tasktresult-boolean) | Executes the provided asynchronous function, if the [Result](../Result/Result.md) instance represents a failed operation and returns its result, as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(Task&lt;TResult&gt;, Func&lt;Error, Task&gt;, Boolean)](../Extensions/IfErrorTaskAsyncExtensions_IfErrorAsync.md#iferrorasynctresulttasktresult-funcerror-task-boolean)  | Executes the provided asynchronous function, if the provided asynchronous operation returns a failed [Result](../Result/Result.md) instance, and returns its result as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(Task&lt;TResult&gt;, Func&lt;Error, Task&lt;TResult&gt;&gt;, Boolean)](../Extensions/IfErrorTaskAsyncExtensions_IfErrorAsync.md#iferrorasynctresulttasktresult-funcerror-tasktresult-boolean) | Executes the provided asynchronous function, if the provided asynchronous operation returns a failed [Result](../Result/Result.md) instance, and returns its result as an asynchronous operation. |
| [IfSuccess(Result, Action)](../Extensions/IfSuccessExtensions_IfSuccess.md#ifsuccessresult-action) | Executes the specified action, if the [Result](../Result/Result.md) represents a successful operation. |
| [IfSuccess(Result, Func&lt;Result&gt;)](../Extensions/IfSuccessExtensions_IfSuccess.md#ifsuccessresult-funcresult) | Executes the specified function, if the [Result](../Result/Result.md) represents a successful operation. |
| [IfSuccess&lt;T&gt;(Result, Func&lt;T&gt;)](../Extensions/IfSuccessExtensions_IfSuccess.md#ifsuccesstresult-funct) | Executes the specified function, if the [Result](../Result/Result.md) represents a successful operation. |
| [IfSuccess&lt;T&gt;(Result, Func&lt;Result&lt;T&gt;&gt;)](../Extensions/IfSuccessExtensions_IfSuccess.md#ifsuccesstresult-funcresultt) | Executes the specified function, if the [Result](../Result/Result.md) represents a successful operation. |
| [IfSuccessAsync(Result, Func&lt;Task&gt;, Boolean)](../Extensions/IfSuccessTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasyncresult-functask-boolean) | Executes the specified asynchronous action, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync(Result, Func&lt;Task&lt;Result&gt;&gt;, Boolean)](../Extensions/IfSuccessTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasyncresult-functaskresult-boolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(Result, Func&lt;Task&lt;T&gt;&gt;, Boolean)](../Extensions/IfSuccessTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctresult-functaskt-boolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(Result, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Boolean)](../Extensions/IfSuccessTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctresult-functaskresultt-boolean) | Executes the specified asynchronous function, if the [Result](../Result/Result.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync(Task&lt;Result&gt;, Func&lt;Task&gt;, Boolean)](../Extensions/IfSuccessTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctaskresult-functask-boolean) | Executes the specified asynchronous action, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [IfSuccessAsync(Task&lt;Result&gt;, Func&lt;Task&lt;Result&gt;&gt;, Boolean)](../Extensions/IfSuccessTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctaskresult-functaskresult-boolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(Task&lt;Result&gt;, Func&lt;Task&lt;T&gt;&gt;, Boolean)](../Extensions/IfSuccessTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasyncttaskresult-functaskt-boolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(Task&lt;Result&gt;, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Boolean)](../Extensions/IfSuccessTaskAsyncExtensions_IfSuccessAsync.md#ifsuccessasyncttaskresult-functaskresultt-boolean) | Executes the specified asynchronous function, if the provided asynchronous operation returns a successful [Result](../Result/Result.md), as an asynchronous operation. |
| [Match(Result, Action, Action&lt;Error&gt;)](../Extensions/MatchExtensions_Match.md#matchresult-action-actionerror) | Executes one of the specified actions depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [Match(Result, Func&lt;Result&gt;, Action&lt;Error&gt;)](../Extensions/MatchExtensions_Match.md#matchresult-actionresult-actionerror) | Executes the specified function or action depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [Match(Result, Func&lt;Result&gt;, Func&lt;Error, Result&gt;)](../Extensions/MatchExtensions_Match.md#matchresult-funcresult-funcerror-result) | Executes one of the specified functions depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [Match&lt;T&gt;(Result, Func&lt;T&gt;, Action&lt;Error&gt;)](../Extensions/MatchExtensions_Match.md#matchtresult-funct-actionerror) | Executes the specified function or action depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [Match&lt;T&gt;(Result, Func&lt;T&gt;, Func&lt;Error, T&gt;)](../Extensions/MatchExtensions_Match.md#matchtresult-funct-funcerror-t) | Executes one of the specified functions depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [Match&lt;T&gt;(Result, Func&lt;Result&lt;T&gt;&gt;, Action&lt;Error&gt;)](../Extensions/MatchExtensions_Match.md#matchtresult-funcresultt-actionerror) | Executes the specified function or action depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [Match&lt;T&gt;(Result, Func&lt;Result&lt;T&gt;&gt;, Func&lt;Error, Result&lt;T&gt;&gt;)](../Extensions/MatchExtensions_Match.md#matchtresult-funcresultt-funcerror-resultt) | Executes one of the specified functions depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [MatchAsync(Result, Func&lt;Task&gt;, Func&lt;Error, Task&gt;, Boolean)](../Extensions/MatchTaskAsyncExtensions_MatchAsync.md#matchasyncresult-functask-funcerror-task-boolean) | Executes one of the specified asynchronous actions as an asynchronous operation depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [MatchAsync(Result, Func&lt;Task&lt;Result&gt;&gt;, Func&lt;Error, Task&gt;, Boolean)](../Extensions/MatchTaskAsyncExtensions_MatchAsync.md#matchasyncresult-functaskresult-funcerror-task-boolean) | Executes the specified asynchronous function or action as an asynchronous operation depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [MatchAsync(Result, Func&lt;Task&lt;Result&gt;&gt;, Func&lt;Error, Task&lt;Result&gt;&gt;, Boolean)](../Extensions/MatchTaskAsyncExtensions_MatchAsync.md#matchasyncresult-functaskresult-funcerror-taskresult-boolean) | Executes one of the specified asynchronous functions as an asynchronous operation depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(Result, Func&lt;Task&lt;T&gt;&gt;, Func&lt;Error, Task&gt;, Boolean)](../Extensions/MatchTaskAsyncExtensions_MatchAsync.md#matchasynctresult-functaskt-funcerror-task-boolean) | Executes the specified asynchronous function or action as an asynchronous operation depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(Result, Func&lt;Task&lt;T&gt;&gt;, Func&lt;Error, Task&lt;T&gt;&gt;, Boolean)](../Extensions/MatchTaskAsyncExtensions_MatchAsync.md#matchasynctresult-functaskt-funcerror-taskt-boolean) | Executes one of the specified asynchronous functions as an asynchronous operation depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(Result, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Func&lt;Error, Task&gt;, Boolean)](../Extensions/MatchTaskAsyncExtensions_MatchAsync.md#matchasynctresult-functaskresultt-funcerror-task-boolean) | Executes the specified asynchronous function or action as an asynchronous operation depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(Result, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Func&lt;Error, Task&lt;Result&lt;T&gt;&gt;&gt;, Boolean)](../Extensions/MatchTaskAsyncExtensions_MatchAsync.md#matchasynctresult-functaskresultt-funcerror-taskresultt-boolean) | Executes one of the specified asynchronous functions as an asynchronous operation depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [MatchAsync(Task&lt;Result&gt;, Func&lt;Task&gt;, Func&lt;Error, Task&gt;, Boolean)](../Extensions/MatchTaskAsyncExtensions_MatchAsync.md#matchasynctaskresult-functask-funcerror-task-boolean) | Executes one of the specified asynchronous actions as an asynchronous operation depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [MatchAsync(Task&lt;Result&gt;, Func&lt;Task&lt;Result&gt;&gt;, Func&lt;Error, Task&gt;, Boolean)](../Extensions/MatchTaskAsyncExtensions_MatchAsync.md#matchasynctaskresult-functaskresult-funcerror-task-boolean) | Executes the specified asynchronous function or action as an asynchronous operation depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [MatchAsync(Task&lt;Result&gt;, Func&lt;Task&lt;Result&gt;&gt;, Func&lt;Error, Task&lt;Result&gt;&gt;, Boolean)](../Extensions/MatchTaskAsyncExtensions_MatchAsync.md#matchasynctaskresult-functaskresult-funcerror-taskresult-boolean) | Executes one of the specified asynchronous functions as an asynchronous operation depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(Task&lt;Result&gt;, Func&lt;Task&lt;T&gt;&gt;, Func&lt;Error, Task&gt;, Boolean)](../Extensions/MatchTaskAsyncExtensions_MatchAsync.md#matchasyncttaskresult-functaskt-funcerror-task-boolean) | Executes the specified asynchronous function or action as an asynchronous operation depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(Task&lt;Result&gt;, Func&lt;Task&lt;T&gt;&gt;, Func&lt;Error, Task&lt;T&gt;&gt;, Boolean)](../Extensions/MatchTaskAsyncExtensions_MatchAsync.md#matchasyncttaskresult-functaskt-funcerror-taskt-boolean) | Executes one of the specified asynchronous functions as an asynchronous operation depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(Task&lt;Result&gt;, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Func&lt;Error, Task&gt;, Boolean)](../Extensions/MatchTaskAsyncExtensions_MatchAsync.md#matchasyncttaskresult-functaskresultt-funcerror-task-boolean) | Executes the specified asynchronous function or action as an asynchronous operation depending on whether the [Result](../Result/Result.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(Task&lt;Result&gt;, Func&lt;Task&lt;Result&lt;T&gt;&gt;&gt;, Func&lt;Error, Task&lt;Result&lt;T&gt;&gt;&gt;, Boolean)](../Extensions/MatchTaskAsyncExtensions_MatchAsync.md#matchasyncttaskresult-functaskresultt-funcerror-taskresultt-boolean) | Executes one of the specified asynchronous functions as an asynchronous operation depending on whether the [Result](../Result/Result.md) represents a success or a failure. |

## See Also
* [IResult](../IResult/IResult.md)
* [Result&lt;T&gt;](../ResultT/ResultT.md)
