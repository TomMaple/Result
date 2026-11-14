// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Result.Helpers;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Maple.Result.Extensions;

/// <summary>
///     The collection of extension methods for executing actions or functions conditionally based on the success state of a <see cref="Result" /> instance.
/// </summary>
/// <remarks>
///     These methods enable fluent chaining of operations that should only be performed if a preceding <see cref="Result" /> indicates success.
///     They help simplify error handling and reduce boilerplate code when working with <see cref="Result" />-based workflows.
/// </remarks>
public static class IfSuccessExtensions
{
    #region Result extensions

    /// <summary>
    ///     Invokes the specified action if the <paramref name="result" /> represents a successful outcome.
    /// </summary>
    /// <remarks>
    ///     This method enables fluent handling of successful <see cref="Result" />s by executing the provided action only when the <paramref name="result" />
    ///     indicates success. The action is not invoked if the <paramref name="result" /> is not successful.
    /// </remarks>
    /// <param name="result">The <see cref="Result" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessAction">
    ///     The action to execute if the <paramref name="result" /> is successful. Cannot be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     The original <paramref name="result" />, allowing for method chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessAction" /> parameters are <see langword="null" />.
    /// </exception>
    public static Result IfSuccess(this Result result, Action ifSuccessAction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);

        if (result.IsSuccess())
            ifSuccessAction();

        return result;
    }

    /// <summary>
    ///     Invokes the specified function if the <paramref name="result" /> represents a successful outcome;
    ///     otherwise, returns an error <see cref="Result" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     This method enables chaining additional operations that should only execute if the previous <see cref="Result" /> was successful.
    ///     If <paramref name="result" /> is not successful, the provided function is not invoked.
    /// </remarks>
    /// <param name="result">The <see cref="Result" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The function to execute if <paramref name="result" /> is successful.
    ///     The function must return a <see cref="Result" /> and must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Result" /> returned by <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is successful;
    ///     otherwise, the original <paramref name="result" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">If <paramref name="ifSuccessFunction" /> returns <see langword="null" />.</exception>
    public static Result IfSuccess(this Result result, Func<Result> ifSuccessFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        if (result.IsSuccess())
            return Returned.NotNull(ifSuccessFunction(), nameof(ifSuccessFunction));

        return result;
    }

    /// <summary>
    ///     Invokes the specified function if the <paramref name="result" /> represents a successful outcome and returns a failed <see cref="Result" />
    ///     with the <see cref="Error" /> it returns; otherwise, returns the original <paramref name="result" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Use it to turn a successful <see cref="Result" /> into a failure, e.g., <c>result.IfSuccess(() => Error.Conflict(…))</c>.
    ///         The returned <see cref="Result" /> is always failed.
    ///     </para>
    ///     <para>
    ///         Without this overload, such a call would bind to <see cref="IfSuccess{T}(Result, Func{T})" /> and return a <b>successful</b>
    ///         <c>Result&lt;Error&gt;</c>. To get that on purpose, specify the type argument: <c>IfSuccess&lt;Error&gt;(…)</c>.
    ///     </para>
    /// </remarks>
    /// <param name="result">The <see cref="Result" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The function to execute if <paramref name="result" /> is successful.
    ///     It returns the <see cref="Error" /> of the failed <see cref="Result" />. Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A failed <see cref="Result" /> with the <see cref="Error" /> returned by <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is
    ///     successful; otherwise, the original <paramref name="result" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">If <paramref name="ifSuccessFunction" /> returns <see langword="null" />.</exception>
    public static Result IfSuccess(this Result result, Func<Error> ifSuccessFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        if (result.IsSuccess())
            return Result.FromError(Returned.NotNull(ifSuccessFunction(), nameof(ifSuccessFunction)));

        return result;
    }

    /// <summary>
    ///     Invokes the specified function and returns its output as a <see cref="Result{T}" /> if the operation represented by the <paramref name="result" />
    ///     is successful; otherwise, returns a failed <see cref="Result{T}" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     This method enables chaining additional operations that should only execute if the previous <see cref="Result" /> was successful.
    ///     If <paramref name="result" /> is not successful, the provided function is not invoked.
    /// </remarks>
    /// <typeparam name="T">The type of the <see cref="Result{T}" /> value to return if the operation is successful.</typeparam>
    /// <param name="result">The <see cref="Result" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The function to execute if the <paramref name="result" /> is successful.
    ///     The function’s return value is used as the successful <see cref="Result{T}" />. Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A successful <see cref="Result{T}" /> containing the value returned by <paramref name="ifSuccessFunction" />
    ///     if <paramref name="result" /> is successful; otherwise, a failed <see cref="Result{T}" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    public static Result<T> IfSuccess<T>(this Result result, Func<T> ifSuccessFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        if (result.IsSuccess())
            return ifSuccessFunction();

        return result.Error;
    }

    /// <summary>
    ///     Invokes the specified function and returns a <see cref="Result{T}" /> with its output if the operation represented by the <paramref name="result" />
    ///     is successful; otherwise, returns a failed <see cref="Result{T}" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     This method enables chaining additional operations that should only execute if the previous <see cref="Result" /> was successful.
    ///     If <paramref name="result" /> is not successful, the provided function is not invoked.
    /// </remarks>
    /// <typeparam name="T">The type of the <see cref="Result{T}" /> value to return if the operation is successful.</typeparam>
    /// <param name="result">The <see cref="Result" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The function to execute if the <paramref name="result" /> is successful.
    ///     The function’s return value is used as the successful <see cref="Result{T}" />. Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A successful <see cref="Result{T}" /> returned by <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is successful;
    ///     otherwise, a failed <see cref="Result{T}" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">If <paramref name="ifSuccessFunction" /> returns <see langword="null" />.</exception>
    public static Result<T> IfSuccess<T>(this Result result, Func<Result<T>> ifSuccessFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        if (result.IsSuccess())
            return Returned.NotNull(ifSuccessFunction(), nameof(ifSuccessFunction));

        return result.Error;
    }

    #endregion

    #region Result<T> extensions

    /// <summary>
    ///     Invokes the specified action if the <paramref name="result" /> represents a successful outcome.
    /// </summary>
    /// <remarks>
    ///     This method enables fluent handling of successful <see cref="Result{T}" />s by executing the provided action only when
    ///     the <paramref name="result" /> indicates success. The action is not invoked if the <paramref name="result" /> is not successful.
    /// </remarks>
    /// <param name="result">The <see cref="Result{T}" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessAction">
    ///     The action to execute if the <paramref name="result" /> is successful.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter. Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     The original <paramref name="result" />, allowing for method chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessAction" /> parameters are <see langword="null" />.
    /// </exception>
    public static Result<T> IfSuccess<T>(this Result<T> result, Action<T> ifSuccessAction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);

        if (result.IsSuccess())
            ifSuccessAction(result.Value);

        return result;
    }

    /// <summary>
    ///     Invokes the specified function if the <paramref name="result" /> represents a successful outcome;
    ///     otherwise, returns an error <see cref="Result" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     This method enables chaining additional operations that should only execute if the previous <see cref="Result" /> was successful.
    ///     If <paramref name="result" /> is not successful, the provided function is not invoked.
    /// </remarks>
    /// <param name="result">The <see cref="Result{T}" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The function to execute if <paramref name="result" /> is successful. The value of the successful <see cref="Result{T}" /> is passed as a parameter.
    ///     The function must return a <see cref="Result" /> and must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Result" /> returned by <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is successful;
    ///     otherwise, a failed <see cref="Result" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">If <paramref name="ifSuccessFunction" /> returns <see langword="null" />.</exception>
    public static Result IfSuccess<T>(this Result<T> result, Func<T, Result> ifSuccessFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        if (result.IsSuccess())
            return Returned.NotNull(ifSuccessFunction(result.Value), nameof(ifSuccessFunction));

        return result.Error;
    }

    /// <summary>
    ///     Invokes the specified function if the <paramref name="result" /> represents a successful outcome and returns a failed <see cref="Result" />
    ///     with the <see cref="Error" /> it returns; otherwise, returns a failed <see cref="Result" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Use it to turn a successful <see cref="Result{T}" /> into a failure, e.g., <c>result.IfSuccess(user => Error.Conflict(…))</c>.
    ///         The returned <see cref="Result" /> is always failed.
    ///     </para>
    ///     <para>
    ///         Without this overload, such a call would bind to <see cref="IfSuccess{T, TNext}(Result{T}, Func{T, TNext})" /> and return
    ///         a <b>successful</b> <c>Result&lt;Error&gt;</c>. To get that on purpose, specify the type arguments: <c>IfSuccess&lt;T, Error&gt;(…)</c>.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type of the value of the <paramref name="result" />.</typeparam>
    /// <param name="result">The <see cref="Result{T}" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The function to execute if <paramref name="result" /> is successful. The value of the successful <see cref="Result{T}" /> is passed as a parameter.
    ///     It returns the <see cref="Error" /> of the failed <see cref="Result" />. Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A failed <see cref="Result" /> with the <see cref="Error" /> returned by <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is
    ///     successful; otherwise, a failed <see cref="Result" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">If <paramref name="ifSuccessFunction" /> returns <see langword="null" />.</exception>
    public static Result IfSuccess<T>(this Result<T> result, Func<T, Error> ifSuccessFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        if (result.IsSuccess())
            return Result.FromError(Returned.NotNull(ifSuccessFunction(result.Value), nameof(ifSuccessFunction)));

        return result.Error;
    }

    /// <summary>
    ///     Invokes the specified function and returns its output as a <see cref="Result{TNext}" /> if the operation represented by
    ///     the <paramref name="result" /> is successful; otherwise, returns a failed <see cref="Result{TNext}" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     This method enables chaining additional operations that should only execute if the previous <see cref="Result{T}" /> was successful.
    ///     If <paramref name="result" /> is not successful, the provided function is not invoked.
    /// </remarks>
    /// <typeparam name="T">
    ///     The type of the <see cref="Result{T}" /> value used to determine whether to execute the passed function.
    ///     If successful, this is also the type of the parameter passed to that function.
    /// </typeparam>
    /// <typeparam name="TNext">
    ///     The output type of the passed function and the type of the <see cref="Result{TNext}" /> value to return.
    /// </typeparam>
    /// <param name="result">The <see cref="Result{T}" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The function to execute if the <paramref name="result" /> is successful. The value of the successful <see cref="Result{T}" /> is passed as
    ///     a parameter. The function’s return value is used as the successful <see cref="Result{TNext}" />. Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A successful <see cref="Result{TNext}" /> containing the value returned by <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is
    ///     successful; otherwise, a failed <see cref="Result{TNext}" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    public static Result<TNext> IfSuccess<T, TNext>(this Result<T> result, Func<T, TNext> ifSuccessFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        if (result.IsSuccess())
            return ifSuccessFunction(result.Value);

        return result.Error;
    }

    /// <summary>
    ///     Invokes the specified function and returns a <see cref="Result{TNext}" /> with its output if the operation represented by
    ///     the <paramref name="result" /> is successful; otherwise, returns a failed <see cref="Result{TNext}" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     This method enables chaining additional operations that should only execute if the previous <see cref="Result{T}" /> was successful.
    ///     If <paramref name="result" /> is not successful, the provided function is not invoked.
    /// </remarks>
    /// <typeparam name="T">
    ///     The type of the <see cref="Result{T}" /> value used to determine whether to execute the passed function.
    ///     If successful, this is also the type of the parameter passed to that function.
    /// </typeparam>
    /// <typeparam name="TNext">
    ///     The type of the <see cref="Result{TNext}" /> value to return and of the output of the passed function.
    /// </typeparam>
    /// <param name="result">The <see cref="Result{T}" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The function to execute if the <paramref name="result" /> is successful. The value of the successful <see cref="Result{T}" /> is passed as
    ///     a parameter. The function’s must return <see cref="Result{TNext}" />. Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A successful <see cref="Result{TNext}" /> returned by <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is successful;
    ///     otherwise, a failed <see cref="Result{TNext}" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">If <paramref name="ifSuccessFunction" /> returns <see langword="null" />.</exception>
    public static Result<TNext> IfSuccess<T, TNext>(this Result<T> result, Func<T, Result<TNext>> ifSuccessFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);

        if (result.IsSuccess())
            return Returned.NotNull(ifSuccessFunction(result.Value), nameof(ifSuccessFunction));

        return result.Error;
    }

    #endregion

    #region Not supported: functions returning a task

    // These overloads turn a call with a function that returns a task into a compile error (CS0619). Without them, the call binds to the
    // generic overload, which wraps the task in a successful result without awaiting it (issue #119). They are more specific than the
    // generic overloads, so they are chosen first; an explicit type argument excludes them and selects the generic overload on purpose.
    // The Task overloads have a higher priority than the ValueTask ones: an async lambda converts to both, which would be ambiguous (CS0121).

    private const string TaskNotAwaitedMessage =
        "The function returns a task, which IfSuccess does not await. Use IfSuccessAsync, or specify the type arguments " +
        "(e.g., IfSuccess<Task>(…) or IfSuccess<T, Task>(…)) to keep the task.";

    /// <summary>
    ///     Not supported: the function returns a task, which <c>IfSuccess</c> does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result IfSuccess(this Result result, Func<Task> ifSuccessFunction)
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>IfSuccess</c> does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result IfSuccess<TNext>(this Result result, Func<Task<TNext>> ifSuccessFunction)
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>IfSuccess</c> does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    public static Result IfSuccess(this Result result, Func<ValueTask> ifSuccessFunction)
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>IfSuccess</c> does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    public static Result IfSuccess<TNext>(this Result result, Func<ValueTask<TNext>> ifSuccessFunction)
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>IfSuccess</c> does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result IfSuccess<T>(this Result<T> result, Func<T, Task> ifSuccessFunction)
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>IfSuccess</c> does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    [OverloadResolutionPriority(1)]
    public static Result IfSuccess<T, TNext>(this Result<T> result, Func<T, Task<TNext>> ifSuccessFunction)
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>IfSuccess</c> does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    public static Result IfSuccess<T>(this Result<T> result, Func<T, ValueTask> ifSuccessFunction)
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    /// <summary>
    ///     Not supported: the function returns a task, which <c>IfSuccess</c> does not await; use <c>IfSuccessAsync</c>.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    [Obsolete(TaskNotAwaitedMessage, error: true)]
    public static Result IfSuccess<T, TNext>(this Result<T> result, Func<T, ValueTask<TNext>> ifSuccessFunction)
    {
        throw new NotSupportedException(TaskNotAwaitedMessage);
    }

    #endregion
}
