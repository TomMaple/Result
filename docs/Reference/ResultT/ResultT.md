# Result&lt;T&gt; Record
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/Result.cs#L44" target="_blank">Result.cs</a>

Represents the outcome of an operation that can either succeed or fail with an [Error](../Error/Error.md).

```csharp
public sealed record Result<T> : IResult
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → [ValueType](https://learn.microsoft.com/dotnet/api/system.valuetype) → [Record](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/record) → Result&lt;T&gt;

Implements [IResult](../IResult/IResult.md)

## Remarks
> [!NOTE]
> This type is used to represent the result of an operation that can return a value.
> 
> For operations that does not return a value, consider using [Result](../Result/Result.md) record.

If the operation is successful, the [IsSuccess](ResultT_IsSuccess.md) property will be `true` and the [Value](ResultT_Value.md) property will hold the returned value. If the operation has failed, the [IsSuccess](ResultT_IsSuccess.md) property will be `false`, and the [Error](Result_Error.md) property will contain details about the failure.

> [!CAUTION]
> Use static factory methods to create instances of this record, such as [Result.FromValue(T)](ResultT_FromValue.md) and [Result.FromError(Error)](Result_FromError.md), or the operator [Implicit(Error to Result&lt;T&gt;)](ResultT_implicit_Error_to_ResultT.md) or the [Implicit(T to Result&lt;T&gt;)](ResultT_implicit_Error_to_ResultT.md) rather than using the constructor directly.

## Examples
```csharp
public Result<User> GetUser(int userId)
{
    try
    {
        var user = await _userRepository.GetById(userId);
        return user is null
            ? Result<User>.FromError(
                Error.NotFound(
                    ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:not-found"),
                    "User has not been found.",
                    "User with the specified ID does not exist in the system.",
                    ErrorUri.Locator("https://api.exampleapp.com/error/e-1234-5678-9012-345678901234/details"),
                    "errors-user-notfound",
                    ("userId", userId)))
            : Result<User>.FromValue(user);
    }
    catch (Exception ex)
    {
        // Log exception details here
        // Map the exception to a proper error

        return Result<User>.FromError(error);
    }
```

```csharp
public Result<User> GetUser(int userId)
{
    try
    {
        var user = await _userRepository.GetById(userId);
        if (user is null)
        {
            return Error.NotFound(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:not-found"),
                "User has not been found.",
                "User with the specified ID does not exist in the system.",
                ErrorUri.Locator("https://api.exampleapp.com/error/e-1234-5678-9012-345678901234/details"),
                "errors-user-notfound",
                ("userId", userId));
        }

        return user;
    }
    catch (Exception ex)
    {
        // Log exception details here
        // Map the exception to a proper error

        return Result<User>.FromError(error);
    }
```

## Constructors
| Name | Description |
| ---- | ----------- |
| [Result()](ResultT_constructors.md) | Initializes a new instance of the [Result&lt;T&gt;](ResultT.md) record. |

## Properties
| Name | Description |
| ---- | ----------- |
| [Error](ResultT_Error.md) | Gets or sets the error details if the result represents a failure. |
| [Value](ResultT_Value.md) | Gets or sets the operation value if the result represents a successful operation. |

## Methods
| Name | Description |
| ---- | ----------- |
| [FromError(Error)](Result_FromError.md)        | Creates a new failure [Result](Result.md) from the specified [Error](../Error/Error.md). |
| [FromValue(T)](Result_FromValue_T.md) | Creates a new successful [Result&lt;T&gt;](Result_T.md) containing the specified value. |
| [IsSuccess()](Result_IsSuccess.md)             | Returns an indicator whether the result represents a successful operation. |

## Operators
| Name | Description |
| ---- | ----------- |
| [Implicit(Error to Result&lt;T&gt;)](ResultT_implicit_Error_to_ResultT.md) | Defines an implicit conversion of a given [Error](../Error/Error.md) to a failure [Result&lt;T&gt;](ResultT.md). |
| [Implicit(T to Result&lt;T&gt;)](ResultT_implicit_T_to_ResultT.md) | Defines an implicit conversion of a given value of type `T` to a successful [Result&lt;T&gt;](ResultT.md). |

## Extension Methods
| Name | Description |
| ---- | ----------- |
| [IfError&lt;TResult&gt;(TResult, Action&lt;Error&gt;)](../Extensions/IfErrorExtensions_IfError.md#iferrortresulttresult-actionerror) | Executes the specified action, if the [Result](Result.md) represents a failure. |
| [IfError&lt;TResult&gt;(TResult, Func&lt;Error, Result&gt;)](../Extensions/IfErrorExtensions_IfError.md#iferrortresulttresult-funcerror-tresult) | Executes the specified function, if the [Result](Result.md) represents a failure. |
| [IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, Task&gt;, Boolean)](../Extensions/IfErrorAsyncExtensions_IfErrorAsync.md#iferrorasynctresulttresult-funcerror-task) | Executes the specified asynchronous function, if the [Result](Result.md) represents a failure, as an asynchronous operation. |
| [IfErrorAsync&lt;TResult&gt;(TResult, Func&lt;Error, Task&lt;TResult&gt;&gt;, Boolean)](../Extensions/IfErrorAsyncExtensions_IfErrorAsync.md#iferrorasynctresulttresult-funcerror-tasktresult) | Executes the specified asynchronous function, if the [Result](Result.md) represents a failure, as an asynchronous operation. |
| [IfSuccess&lt;T&gt;(Result&lt;T&gt;, Action&lt;T&gt;)](../Extensions/IfSuccessExtensions_IfSuccess.md#ifsuccesstresultt-funct-) | Executes the specified action, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. |
| [IfSuccess&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Result&gt;)](../Extensions/IfSuccessExtensions_IfSuccess.md#ifsuccesstresultt-funct-result) | Executes the specified function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. |
| [IfSuccess&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, TNext&gt;)](../Extensions/IfSuccessExtensions_IfSuccess.md#ifsuccesst-tnextresultt-funct-tnext) | Executes the specified function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. |
| [IfSuccess&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Result&lt;TNext&gt;&gt;)](../Extensions/IfSuccessExtensions_IfSuccess.md#ifsuccesst-tnextresultt-funct-resulttnext) | Executes the specified function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation. |
| [IfSuccessAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Task&gt;, Boolean)](../Extensions/IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctresultt-funct-task-boolean) | Executes the specified asynchronous action, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&gt;&gt;, Boolean)](../Extensions/IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasynctresultt-funct-taskresult-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;TNext&gt;&gt;, Boolean)](../Extensions/IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasynct-tnextresultt-funct-tasktnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [IfSuccessAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)](../Extensions/IfSuccessAsyncExtensions_IfSuccessAsync.md#ifsuccessasynct-tnextresultt-funct-taskresulttnext-boolean) | Executes the specified asynchronous function, if the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a successful operation, as an asynchronous operation. |
| [Match&lt;T&gt;(Result&lt;T&gt;, Action&lt;T&gt;, Action&lt;Error&gt;)](../Extensions/MatchExtensions_Match.md#matchtresultt-result-funct-funcerror-boolean) |  Executes one of the specified actions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |
| [Match&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, TNext&gt;, Action&lt;Error&gt;)](../Extensions/MatchExtensions_Match.md#matcht-tnextresultt-result-funct-tnext-funcerror-boolean) |  Executes the specified function or action depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |
| [Match&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, TNext&gt;, Func&lt;Error, TNext&gt;)](../Extensions/MatchExtensions_Match.md#matcht-tnextresultt-result-funct-tnext-funcerror-tnext-boolean) |  Executes one of the specified functions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |
| [Match&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Result&lt;TNext&gt;&gt;, Action&lt;Error&gt;)](../Extensions/MatchExtensions_Match.md#matcht-tnextresultt-result-funct-resulttnext-funcerror-boolean) |  Executes the specified function or action depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |
| [Match&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Result&lt;TNext&gt;&gt;, Func&lt;Error, Result&lt;TNext&gt;&gt;)](../Extensions/MatchExtensions_Match.md#matcht-tnextresultt-result-funct-resulttnext-funcerror-resulttnext-boolean) |  Executes one of the specified functions depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |
| [MatchAsync&lt;T&gt;(Result&lt;T&gt;, Func&lt;T, Task&gt;, Func&lt;Error, Task&gt;, Boolean)](../Extensions/MatchAsyncExtensions_MatchAsync.md#matchasynctresultt-result-funct-task-funcerror-task-boolean) |  Executes one of the specified asynchronous actions as an asynchronous operation depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |
| [MatchAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;TNext&gt;&gt;, Func&lt;Error, Task&gt;, Boolean)](../Extensions/MatchAsyncExtensions_MatchAsync.md#matchasynct-tnextresultt-result-funct-tasktnext-funcerror-task-boolean) |  Executes the specified asynchronous function or action as an asynchronous operation depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |
| [MatchAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;TNext&gt;&gt;, Func&lt;Error, Task&lt;TNext&gt;&gt;, Boolean)](../Extensions/MatchAsyncExtensions_MatchAsync.md#matchasynct-tnextresultt-result-funct-tasktnext-funcerror-tasktnext-boolean) |  Executes one of the specified asynchronous functions as an asynchronous operation depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |
| [MatchAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Func&lt;Error, Task&gt;, Boolean)](../Extensions/MatchAsyncExtensions_MatchAsync.md#matchasynct-tnextresultt-result-funct-taskresulttnext-funcerror-task-boolean) |  Executes the specified asynchronous function or action as an asynchronous operation depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |
| [MatchAsync&lt;T, TNext&gt;(Result&lt;T&gt;, Func&lt;T, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Func&lt;Error, Task&lt;Result&lt;TNext&gt;&gt;&gt;, Boolean)](../Extensions/MatchAsyncExtensions_MatchAsync.md#matchasynct-tnextresultt-result-funct-taskresulttnext-funcerror-taskresulttnext-boolean) |  Executes one of the specified asynchronous functions as an asynchronous operation depending on whether the [Result&lt;T&gt;](../ResultT/ResultT.md) represents a success or a failure. |

## See Also
* [IResult](../IResult/IResult.md)
* [Result](../Result/Result.md)
