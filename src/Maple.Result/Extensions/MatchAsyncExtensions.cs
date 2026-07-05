// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System;
using System.Threading.Tasks;

namespace Maple.Result.Extensions;

/// <summary>
///     The collection of extension methods for executing asynchronous actions or functions based on the state of
///     a <see cref="Result" /> instance.
/// </summary>
/// <remarks>
///     These methods enable fluent chaining of operations by specifying operations that will be executed depending on
///     the status of the <see cref="Result" /> instance. These helpers centralize success/error handling logic by
///     accepting delegates for each branch, enabling concise match-like patterns across synchronous result workflows.
/// </remarks>
public static class MatchAsyncExtensions
{
    #region Result

    /// <summary>
    ///     Invokes one of the provided asynchronous actions depending on whether the <paramref name="result" /> is successful.
    /// </summary>
    /// <remarks>
    ///     Use this overload when the desired outcome is a side effect, and you want to keep the existing
    ///     <see cref="Result" /> instance for further chaining.
    /// </remarks>
    /// <param name="result">
    ///     The <see cref="Result" /> whose state determines which action is invoked.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessAction">
    ///     The asynchronous action to execute if the <paramref name="result" /> indicates success.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action to execute if the <paramref name="result" /> indicates failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this action.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to
    ///     the original context captured; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{Result}" /> that represents the original <paramref name="result" />, enabling fluent chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessAction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    public static async Task<Result> MatchAsync(this Result result, Func<Task> ifSuccessAction,
        Func<Error, Task> ifErrorAction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsSuccess())
            await ifSuccessAction().ConfigureAwait(continueOnCapturedContext);
        else
            await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);

        return result;
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its <see cref="Result" /> output
    ///     if the <paramref name="result" /> is successful, or the provided asynchronous action and
    ///     returns <see cref="Result" /> with the original error.
    /// </summary>
    /// <param name="result">The <see cref="Result" /> whose outcome is inspected. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="result" /> represents success.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this function.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to
    ///     the original context captured; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{Result}" /> that represents the <see cref="Result" />
    ///     returned by either <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is successful,
    ///     or the original <paramref name="result" /> after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    public static async Task<Result> MatchAsync(this Result result, Func<Task<Result>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsSuccess())
            return await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext);

        await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
        return result;
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="result" />
    ///     is successful and returns the produced <see cref="Result" />.
    /// </summary>
    /// <param name="result">The <see cref="Result" /> whose outcome is inspected. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="result" /> represents success.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this function.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to
    ///     the original context captured; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{Result}" /> which represents the <see cref="Result" /> returned by
    ///     either <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    public static async Task<Result> MatchAsync(this Result result, Func<Task<Result>> ifSuccessFunction,
        Func<Error, Task<Result>> ifErrorFunction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return result.IsSuccess()
            ? await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext)
            : await ifErrorFunction(result.Error).ConfigureAwait(continueOnCapturedContext);
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its value as a <see cref="Result{T}" /> instance
    ///     if the <paramref name="result" /> is successful, or the provided asynchronous action and
    ///     returns <see cref="Result{T}" /> with the original error.
    /// </summary>
    /// <typeparam name="T">The return type produced by the successful branch.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result" /> whose outcome dictates which function is invoked.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="result" /> represents success.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this action.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to
    ///     the original context captured; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{Result}" /> that represents the <see cref="Result{T}" /> with the value returned by
    ///     either the <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is successful, or
    ///     the <see cref="Result{T}" /> with the original <paramref name="result" />
    ///     after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    public static async Task<Result<T>> MatchAsync<T>(this Result result, Func<Task<T>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsSuccess())
            return await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext);

        await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
        return result.Error;
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="result" />
    ///     is successful and returns their value as a <see cref="Result{T}" /> instance.
    /// </summary>
    /// <typeparam name="T">The return type produced by either branch.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result" /> whose outcome dictates which function is invoked.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="result" /> represents success.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this function.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to
    ///     the original context captured; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{T}" /> that represents the value of type<typeparamref name="T" />
    ///     produced by either <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    public static async Task<T> MatchAsync<T>(this Result result, Func<Task<T>> ifSuccessFunction,
        Func<Error, Task<T>> ifErrorFunction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return result.IsSuccess()
            ? await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext)
            : await ifErrorFunction(result.Error).ConfigureAwait(continueOnCapturedContext);
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its <see cref="Result{T}" /> output
    ///     if the <paramref name="result" /> is successful, or the provided asynchronous action and
    ///     returns <see cref="Result{T}" /> with the original error.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Result{T}" /> returned by the matching branch.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result" /> whose state determines which function is invoked.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="result" /> represents success.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this function.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to
    ///     the original context captured; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{TResult}" /> that represents the <see cref="Result{T}" /> returned by
    ///     the <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is successful,
    ///     or the <see cref="Result{T}" /> with the original <paramref name="result" />
    ///     after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    public static async Task<Result<T>> MatchAsync<T>(this Result result, Func<Task<Result<T>>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsSuccess())
            return await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext);

        await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
        return result.Error;
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="result" />
    ///     is successful and returns their output as a <see cref="Result{T}" /> instance.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Result{T}" /> returned by the matching branch.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result" /> whose state determines which function is invoked.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="result" /> represents success.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this function.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to
    ///     the original context captured; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{TResult}" /> that represents the <see cref="Result{T}" /> produced by
    ///     either <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    public static async Task<Result<T>> MatchAsync<T>(this Result result, Func<Task<Result<T>>> ifSuccessFunction,
        Func<Error, Task<Result<T>>> ifErrorFunction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return result.IsSuccess()
            ? await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext)
            : await ifErrorFunction(result.Error).ConfigureAwait(continueOnCapturedContext);
    }


    /// <summary>
    ///     Invokes one of the provided asynchronous actions depending on whether the <paramref name="result" /> is successful.
    /// </summary>
    public static async Task<Result> MatchAsync(this Result result, Func<ValueTask> ifSuccessAction,
        Func<Error, ValueTask> ifErrorAction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsSuccess())
            await ifSuccessAction().ConfigureAwait(continueOnCapturedContext);
        else
            await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);

        return result;
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its <see cref="Result" /> output
    ///     if the <paramref name="result" /> is successful, or the provided asynchronous action and returns <see cref="Result" /> with the original error.
    /// </summary>
    public static async Task<Result> MatchAsync(this Result result, Func<ValueTask<Result>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsSuccess())
            return await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext);

        await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
        return result;
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="result" />
    ///     is successful and returns the produced <see cref="Result" />.
    /// </summary>
    public static async Task<Result> MatchAsync(this Result result, Func<ValueTask<Result>> ifSuccessFunction,
        Func<Error, ValueTask<Result>> ifErrorFunction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return result.IsSuccess()
            ? await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext)
            : await ifErrorFunction(result.Error).ConfigureAwait(continueOnCapturedContext);
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its value as a <see cref="Result{T}" /> instance
    ///     if the <paramref name="result" /> is successful, or the provided asynchronous action and returns <see cref="Result{T}" /> with the original error.
    /// </summary>
    public static async Task<Result<T>> MatchAsync<T>(this Result result, Func<ValueTask<T>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsSuccess())
            return await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext);

        await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
        return result.Error;
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="result" />
    ///     is successful and returns their value as a <see cref="Result{T}" /> instance.
    /// </summary>
    public static async Task<T> MatchAsync<T>(this Result result, Func<ValueTask<T>> ifSuccessFunction,
        Func<Error, ValueTask<T>> ifErrorFunction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return result.IsSuccess()
            ? await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext)
            : await ifErrorFunction(result.Error).ConfigureAwait(continueOnCapturedContext);
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its <see cref="Result{T}" /> output
    ///     if the <paramref name="result" /> is successful, or the provided asynchronous action and returns <see cref="Result{T}" /> with the original error.
    /// </summary>
    public static async Task<Result<T>> MatchAsync<T>(this Result result, Func<ValueTask<Result<T>>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsSuccess())
            return await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext);

        await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
        return result.Error;
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="result" />
    ///     is successful and returns their output as a <see cref="Result{T}" /> instance.
    /// </summary>
    public static async Task<Result<T>> MatchAsync<T>(this Result result, Func<ValueTask<Result<T>>> ifSuccessFunction,
        Func<Error, ValueTask<Result<T>>> ifErrorFunction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return result.IsSuccess()
            ? await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext)
            : await ifErrorFunction(result.Error).ConfigureAwait(continueOnCapturedContext);
    }

    #endregion

    #region Result<T>

    /// <summary>
    ///     Invokes one of the provided asynchronous actions depending on whether the <paramref name="result" /> is successful.
    /// </summary>
    /// <remarks>
    ///     Use this overload when the desired outcome is a side effect, and you want to keep
    ///     the existing <see cref="Result{T}" /> instance for further chaining.
    /// </remarks>
    /// <typeparam name="T">The type wrapped by the <see cref="Result{T}" />.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result{T}" /> whose outcome determines which asynchronous action is invoked.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessAction">
    ///     The asynchronous action to execute when the <paramref name="result" /> represents success.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action to execute when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this action.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to
    ///     the original context captured; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{TResult}" /> that represents the original <paramref name="result" /> for fluent chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessAction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    public static async Task<Result<T>> MatchAsync<T>(this Result<T> result, Func<T, Task> ifSuccessAction,
        Func<Error, Task> ifErrorAction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsSuccess())
            await ifSuccessAction(result.Value).ConfigureAwait(continueOnCapturedContext);
        else
            await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);

        return result;
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its value as a <see cref="Result{TNext}" /> instance
    ///     if the <paramref name="result" /> is successful, or the provided asynchronous action and
    ///     returns <see cref="Result{TNext}" /> with the original error.
    /// </summary>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNext">The type produced by either branch.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result{T}" /> whose outcome selects the function to invoke.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function executed when the <paramref name="result" /> represents success.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this action.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to
    ///     the original context captured; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{TResult}" /> that represents the <see cref="Result{TNext}" /> with the value
    ///     returned by <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is successful,
    ///     or the <see cref="Result{TNext}" /> with the original <paramref name="result" />
    ///     after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    public static async Task<Result<TNext>> MatchAsync<T, TNext>(
        this Result<T> result,
        Func<T, Task<TNext>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction,
        bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsSuccess())
            return await ifSuccessFunction(result.Value).ConfigureAwait(continueOnCapturedContext);

        await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
        return result.Error;
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="result" />
    ///     is successful and returns their output as a <see cref="Result{TNext}" /> instance.
    /// </summary>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNext">The type produced by either branch.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result{T}" /> whose outcome selects the function to invoke.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function executed when the <paramref name="result" /> represents success.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function executed when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this function.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to
    ///     the original context captured; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{TNext}" /> that represents the value of the <typeparamref name="TNext" /> type
    ///     returned by either <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    public static async Task<TNext> MatchAsync<T, TNext>(
        this Result<T> result,
        Func<T, Task<TNext>> ifSuccessFunction,
        Func<Error, Task<TNext>> ifErrorFunction,
        bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return result.IsSuccess()
            ? await ifSuccessFunction(result.Value).ConfigureAwait(continueOnCapturedContext)
            : await ifErrorFunction(result.Error).ConfigureAwait(continueOnCapturedContext);
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its <see cref="Result{TNext}" /> output
    ///     if the <paramref name="result" /> is successful, or the provided asynchronous action and
    ///     returns <see cref="Result{TNext}" /> with the original error.
    /// </summary>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNext">The type of the <see cref="Result{TNext}" /> returned by the invoked function.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result{T}" /> whose outcome selects the function to invoke.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function executed when the <paramref name="result" /> represents success.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this action.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to
    ///     the original context captured; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{TResult}" /> that represents the <see cref="Result{TNext}" />
    ///     returned by <paramref name="ifSuccessFunction" /> if the <paramref name="result" /> is successful,
    ///     or the <see cref="Result{TNext}" /> with the original <paramref name="result" />
    ///     after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    public static async Task<Result<TNext>> MatchAsync<T, TNext>(
        this Result<T> result,
        Func<T, Task<Result<TNext>>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction,
        bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsSuccess())
            return await ifSuccessFunction(result.Value).ConfigureAwait(continueOnCapturedContext);

        await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
        return result.Error;
    }

    /// <summary>
    ///     Executes one of the provided asynchronous functions depending on whether the <paramref name="result" />
    ///     is successful and returns their output as a <see cref="Result{TNext}" /> instance.
    /// </summary>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNext">The type of the <see cref="Result{TNext}" /> returned by the invoked function.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result{T}" /> whose outcome selects the function to invoke.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function executed when the <paramref name="result" /> represents success.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function executed when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this function.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to
    ///     the original context captured; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{TResult}" /> that represents the <see cref="Result{TNext}" /> returned by either
    ///     <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    public static async Task<Result<TNext>> MatchAsync<T, TNext>(
        this Result<T> result,
        Func<T, Task<Result<TNext>>> ifSuccessFunction,
        Func<Error, Task<Result<TNext>>> ifErrorFunction,
        bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return result.IsSuccess()
            ? await ifSuccessFunction(result.Value).ConfigureAwait(continueOnCapturedContext)
            : await ifErrorFunction(result.Error).ConfigureAwait(continueOnCapturedContext);
    }


    /// <summary>
    ///     Invokes one of the provided asynchronous actions depending on whether the <paramref name="result" /> is successful.
    /// </summary>
    public static async Task<Result<T>> MatchAsync<T>(this Result<T> result, Func<T, ValueTask> ifSuccessAction,
        Func<Error, ValueTask> ifErrorAction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsSuccess())
            await ifSuccessAction(result.Value).ConfigureAwait(continueOnCapturedContext);
        else
            await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);

        return result;
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its value as a <see cref="Result{TNext}" /> instance
    ///     if the <paramref name="result" /> is successful, or the provided asynchronous action and returns <see cref="Result{TNext}" /> with the original error.
    /// </summary>
    public static async Task<Result<TNext>> MatchAsync<T, TNext>(
        this Result<T> result,
        Func<T, ValueTask<TNext>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction,
        bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsSuccess())
            return await ifSuccessFunction(result.Value).ConfigureAwait(continueOnCapturedContext);

        await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
        return result.Error;
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="result" />
    ///     is successful and returns their output as a <see cref="Result{TNext}" /> instance.
    /// </summary>
    public static async Task<TNext> MatchAsync<T, TNext>(
        this Result<T> result,
        Func<T, ValueTask<TNext>> ifSuccessFunction,
        Func<Error, ValueTask<TNext>> ifErrorFunction,
        bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return result.IsSuccess()
            ? await ifSuccessFunction(result.Value).ConfigureAwait(continueOnCapturedContext)
            : await ifErrorFunction(result.Error).ConfigureAwait(continueOnCapturedContext);
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its <see cref="Result{TNext}" /> output
    ///     if the <paramref name="result" /> is successful, or the provided asynchronous action and returns <see cref="Result{TNext}" /> with the original error.
    /// </summary>
    public static async Task<Result<TNext>> MatchAsync<T, TNext>(
        this Result<T> result,
        Func<T, ValueTask<Result<TNext>>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction,
        bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsSuccess())
            return await ifSuccessFunction(result.Value).ConfigureAwait(continueOnCapturedContext);

        await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
        return result.Error;
    }

    /// <summary>
    ///     Executes one of the provided asynchronous functions depending on whether the <paramref name="result" />
    ///     is successful and returns their output as a <see cref="Result{TNext}" /> instance.
    /// </summary>
    public static async Task<Result<TNext>> MatchAsync<T, TNext>(
        this Result<T> result,
        Func<T, ValueTask<Result<TNext>>> ifSuccessFunction,
        Func<Error, ValueTask<Result<TNext>>> ifErrorFunction,
        bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return result.IsSuccess()
            ? await ifSuccessFunction(result.Value).ConfigureAwait(continueOnCapturedContext)
            : await ifErrorFunction(result.Error).ConfigureAwait(continueOnCapturedContext);
    }

    #endregion

    #region Task<Result>

    /// <summary>
    ///     Invokes one of the provided asynchronous actions depending on
    ///     whether the <paramref name="resultTask" /> represents a successful operation.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Use this overload when the desired outcome is a side effect, and you want to keep the existing
    ///         <see cref="Result" /> instance for further chaining.
    ///     </para>
    ///     <para>
    ///         The method executes passed <paramref name="resultTask" /> asynchronous operation, does not modify its result.
    ///     </para>
    /// </remarks>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessAction">
    ///     The asynchronous action to execute if the <paramref name="resultTask" /> indicates
    ///     a successful operation.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action to execute if the <paramref name="resultTask" /> indicates
    ///     a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> outcome is passed to this action.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to
    ///     the original context captured; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{Result}" /> that represents the original <paramref name="resultTask" />,
    ///     enabling fluent chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessAction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static async Task<Result> MatchAsync(this Task<Result> resultTask, Func<Task> ifSuccessAction,
        Func<Error, Task> ifErrorAction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

        if (result is null)
            throw new InvalidOperationException("The asynchronous operation returned null.");

        if (result.IsSuccess())
            await ifSuccessAction().ConfigureAwait(continueOnCapturedContext);
        else
            await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);

        return result;
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its <see cref="Result" /> output
    ///     as an asynchronous operation, if the <paramref name="resultTask" /> represents a successful operation,
    ///     or the provided asynchronous action and returns <see cref="Result" /> with the original error
    ///     as an asynchronous operation.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation, does not modify its result.
    /// </remarks>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="resultTask" /> represents a successful operation.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="resultTask" /> represents a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> outcome is passed to this function.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to the original context captured;
    ///     otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{Result}" /> that represents the <see cref="Result" />
    ///     returned by either <paramref name="ifSuccessFunction" /> if <paramref name="resultTask" /> is
    ///     a successful operation,
    ///     or the original <see cref="Result" />, the outcome of the <paramref name="resultTask" /> operation,
    ///     after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static async Task<Result> MatchAsync(this Task<Result> resultTask, Func<Task<Result>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

        if (result is null)
            throw new InvalidOperationException("The asynchronous operation returned null.");

        if (result.IsSuccess())
            return await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext);

        await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
        return result;
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="resultTask" />
    ///     is a successful operation and returns the produced <see cref="Result" />.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked if the <paramref name="resultTask" /> represents a successful operation.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function invoked if the <paramref name="resultTask" /> represents a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> operation outcome is passed to
    ///     this function.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to
    ///     the original context captured; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{Result}" /> which represents the <see cref="Result" /> returned by
    ///     either <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static async Task<Result> MatchAsync(this Task<Result> resultTask, Func<Task<Result>> ifSuccessFunction,
        Func<Error, Task<Result>> ifErrorFunction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

        if (result is null)
            throw new InvalidOperationException("The asynchronous operation returned null.");

        return result.IsSuccess()
            ? await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext)
            : await ifErrorFunction(result.Error).ConfigureAwait(continueOnCapturedContext);
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its value as an asynchronous operation
    ///     that returns a <see cref="Result{T}" /> instance
    ///     if the <paramref name="resultTask" /> is a successful operation, or the provided asynchronous action and
    ///     returns <see cref="Result{T}" /> with the original error as an asynchronous operation.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <typeparam name="T">The return type produced by the successful branch.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="resultTask" /> represents a successful operation.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="resultTask" /> represents a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> outcome is passed to this action.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to
    ///     the original context captured; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{Result}" /> that represents the <see cref="Result{T}" /> with the value returned by
    ///     either the <paramref name="ifSuccessFunction" /> if <paramref name="resultTask" /> is a successful operation, or
    ///     the original <see cref="Result{T}" /> after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static async Task<Result<T>> MatchAsync<T>(this Task<Result> resultTask, Func<Task<T>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

        if (result is null)
            throw new InvalidOperationException("The asynchronous operation returned null.");

        if (result.IsSuccess())
            return await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext);

        await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
        return result.Error;
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="resultTask" />
    ///     is a successful operation and returns their value as an asynchronous operation
    ///     that returns a <see cref="Result{T}" /> instance.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <typeparam name="T">The return type produced by either branch.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="resultTask" /> represents a successful operation.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function invoked if the <paramref name="resultTask" /> represents a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> operation outcome is passed to
    ///     this function.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to
    ///     the original context captured; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{T}" /> that represents the value of type<typeparamref name="T" />
    ///     produced by either <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static async Task<T> MatchAsync<T>(this Task<Result> resultTask, Func<Task<T>> ifSuccessFunction,
        Func<Error, Task<T>> ifErrorFunction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

        if (result is null)
            throw new InvalidOperationException("The asynchronous operation returned null.");

        return result.IsSuccess()
            ? await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext)
            : await ifErrorFunction(result.Error).ConfigureAwait(continueOnCapturedContext);
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its <see cref="Result{T}" /> output as
    ///     an asynchronous operation if the <paramref name="resultTask" /> is a successful operation,
    ///     or the provided asynchronous action and returns <see cref="Result{T}" /> with the original error
    ///     as an asynchronous operation.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <typeparam name="T">The type of the <see cref="Result{T}" /> returned by the matching branch.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="resultTask" /> represents a successful operation.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="resultTask" /> represents a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> outcome is passed to this action.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to
    ///     the original context captured; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{TResult}" /> that represents the <see cref="Result{T}" /> returned by
    ///     the <paramref name="ifSuccessFunction" /> if <paramref name="resultTask" /> is a successful operation,
    ///     or the <see cref="Result{T}" /> with the original outcome of the <paramref name="resultTask" /> operation
    ///     after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static async Task<Result<T>> MatchAsync<T>(
        this Task<Result> resultTask, Func<Task<Result<T>>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

        if (result is null)
            throw new InvalidOperationException("The asynchronous operation returned null.");

        if (result.IsSuccess())
            return await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext);

        await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
        return result.Error;
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="resultTask" />
    ///     is a successful operation and returns their output as a <see cref="Result{T}" /> instance.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <typeparam name="T">The type of the <see cref="Result{T}" /> returned by the matching branch.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="resultTask" /> represents a successful operation.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function invoked if the <paramref name="resultTask" /> represents a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> operation outcome
    ///     is passed to this function.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to
    ///     the original context captured; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{TResult}" /> that represents the <see cref="Result{T}" /> produced by
    ///     either <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static async Task<Result<T>> MatchAsync<T>(
        this Task<Result> resultTask, Func<Task<Result<T>>> ifSuccessFunction,
        Func<Error, Task<Result<T>>> ifErrorFunction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

        if (result is null)
            throw new InvalidOperationException("The asynchronous operation returned null.");

        return result.IsSuccess()
            ? await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext)
            : await ifErrorFunction(result.Error).ConfigureAwait(continueOnCapturedContext);
    }

    #endregion


    #region ValueTask<Result>

    /// <summary>
    ///     Invokes one of the provided asynchronous actions depending on whether the <paramref name="resultTask" /> represents a successful operation.
    /// </summary>
    public static async Task<Result> MatchAsync(this ValueTask<Result> resultTask, Func<Task> ifSuccessAction,
        Func<Error, Task> ifErrorAction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

        if (result is null)
            throw new InvalidOperationException("The asynchronous operation returned null.");

        if (result.IsSuccess())
            await ifSuccessAction().ConfigureAwait(continueOnCapturedContext);
        else
            await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);

        return result;
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its <see cref="Result" /> output as an asynchronous operation,
    ///     if the <paramref name="resultTask" /> represents a successful operation.
    /// </summary>
    public static async Task<Result> MatchAsync(this ValueTask<Result> resultTask, Func<Task<Result>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

        if (result is null)
            throw new InvalidOperationException("The asynchronous operation returned null.");

        if (result.IsSuccess())
            return await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext);

        await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
        return result;
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="resultTask" /> is a successful operation and returns the produced <see cref="Result" />.
    /// </summary>
    public static async Task<Result> MatchAsync(this ValueTask<Result> resultTask, Func<Task<Result>> ifSuccessFunction,
        Func<Error, Task<Result>> ifErrorFunction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

        if (result is null)
            throw new InvalidOperationException("The asynchronous operation returned null.");

        return result.IsSuccess()
            ? await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext)
            : await ifErrorFunction(result.Error).ConfigureAwait(continueOnCapturedContext);
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its value as a <see cref="Result{T}" /> instance
    ///     if the <paramref name="resultTask" /> is successful, or the provided asynchronous action and returns <see cref="Result{T}" /> with the original error.
    /// </summary>
    public static async Task<Result<T>> MatchAsync<T>(this ValueTask<Result> resultTask, Func<Task<T>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

        if (result is null)
            throw new InvalidOperationException("The asynchronous operation returned null.");

        if (result.IsSuccess())
            return await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext);

        await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
        return result.Error;
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="resultTask" /> is successful and returns their value as a <see cref="Result{T}" /> instance.
    /// </summary>
    public static async Task<T> MatchAsync<T>(this ValueTask<Result> resultTask, Func<Task<T>> ifSuccessFunction,
        Func<Error, Task<T>> ifErrorFunction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

        if (result is null)
            throw new InvalidOperationException("The asynchronous operation returned null.");

        return result.IsSuccess()
            ? await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext)
            : await ifErrorFunction(result.Error).ConfigureAwait(continueOnCapturedContext);
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its <see cref="Result{T}" /> output
    ///     if the <paramref name="resultTask" /> is successful, or the provided asynchronous action and returns <see cref="Result{T}" /> with the original error.
    /// </summary>
    public static async Task<Result<T>> MatchAsync<T>(this ValueTask<Result> resultTask, Func<Task<Result<T>>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

        if (result is null)
            throw new InvalidOperationException("The asynchronous operation returned null.");

        if (result.IsSuccess())
            return await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext);

        await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
        return result.Error;
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="resultTask" />
    ///     is successful and returns their output as a <see cref="Result{T}" /> instance.
    /// </summary>
    public static async Task<Result<T>> MatchAsync<T>(this ValueTask<Result> resultTask, Func<Task<Result<T>>> ifSuccessFunction,
        Func<Error, Task<Result<T>>> ifErrorFunction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

        if (result is null)
            throw new InvalidOperationException("The asynchronous operation returned null.");

        return result.IsSuccess()
            ? await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext)
            : await ifErrorFunction(result.Error).ConfigureAwait(continueOnCapturedContext);
    }

    #endregion

    #region Task<Result<T>>

    /// <summary>
    ///     Invokes one of the provided asynchronous actions depending on whether the <paramref name="resultTask" />
    ///     is a successful operation.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Use this overload when the desired outcome is a side effect, and you want to keep
    ///         the existing <see cref="Result{T}" /> instance for further chaining.
    ///     </para>
    ///     <para>
    ///         The method executes passed <paramref name="resultTask" /> asynchronous operation, does not modify its result.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type wrapped by the <see cref="Result{T}" />.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation, which returns <see cref="Result{T}" />, to be executed and which result
    ///     is to be inspected to select which function to invoke.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessAction">
    ///     The asynchronous action to execute if the <paramref name="resultTask" /> indicates
    ///     a successful operation.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action to execute if the <paramref name="resultTask" /> indicates
    ///     a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> outcome is passed to this action.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to
    ///     the original context captured; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{TResult}" /> that represents the outcome of the <paramref name="resultTask" />
    ///     for fluent chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessAction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static async Task<Result<T>> MatchAsync<T>(this Task<Result<T>> resultTask, Func<T, Task> ifSuccessAction,
        Func<Error, Task> ifErrorAction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

        if (result is null)
            throw new InvalidOperationException("The asynchronous operation returned null.");

        if (result.IsSuccess())
            await ifSuccessAction(result.Value).ConfigureAwait(continueOnCapturedContext);
        else
            await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);

        return result;
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its value as a <see cref="Result{TNext}" /> instance
    ///     if the <paramref name="resultTask" /> is a successful operation, or the provided asynchronous action and
    ///     returns <see cref="Result{TNext}" /> with the original error.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNext">The type produced by either branch.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation, which returns <see cref="Result{T}" />, to be executed
    ///     and which result is to be inspected to select which function to invoke.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="resultTask" /> represents a successful operation.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="resultTask" /> represents a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> outcome is passed to this function.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to
    ///     the original context captured; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{TResult}" /> that represents the <see cref="Result{TNext}" /> with the value
    ///     returned by <paramref name="ifSuccessFunction" /> if <paramref name="resultTask" /> is a successful operation,
    ///     or the <see cref="Result{TNext}" /> with the outcome of the original <paramref name="resultTask" />
    ///     after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static async Task<Result<TNext>> MatchAsync<T, TNext>(
        this Task<Result<T>> resultTask,
        Func<T, Task<TNext>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction,
        bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

        if (result is null)
            throw new InvalidOperationException("The asynchronous operation returned null.");

        if (result.IsSuccess())
            return await ifSuccessFunction(result.Value).ConfigureAwait(continueOnCapturedContext);

        await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
        return result.Error;
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="resultTask" />
    ///     is a successful operation and returns their output as an asynchronous operation
    ///     that represents a <see cref="Result{TNext}" /> instance.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNext">The type produced by either branch.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation, which returns <see cref="Result{T}" />, to be executed and
    ///     which result is to be inspected to select which function to invoke.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked if the <paramref name="resultTask" /> represents a successful operation.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function invoked if the <paramref name="resultTask" /> represents a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> operation outcome is passed to
    ///     this function.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to
    ///     the original context captured; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{TNext}" /> that represents the value of the <typeparamref name="TNext" /> type
    ///     returned by either <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    public static async Task<TNext> MatchAsync<T, TNext>(
        this Task<Result<T>> resultTask,
        Func<T, Task<TNext>> ifSuccessFunction,
        Func<Error, Task<TNext>> ifErrorFunction,
        bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

        if (result is null)
            throw new InvalidOperationException("The asynchronous operation returned null.");

        return result.IsSuccess()
            ? await ifSuccessFunction(result.Value).ConfigureAwait(continueOnCapturedContext)
            : await ifErrorFunction(result.Error).ConfigureAwait(continueOnCapturedContext);
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its value as an asynchronous operation
    ///     that returns a <see cref="Result{TNext}" /> instance if the <paramref name="resultTask" /> is
    ///     a successful operation, or the provided asynchronous action and returns <see cref="Result{TNext}" />
    ///     with the original error as an asynchronous operation.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNext">The type of the <see cref="Result{TNext}" /> returned by the invoked function.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="resultTask" /> represents a successful operation.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="resultTask" /> represents a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> outcome is passed to this action.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to
    ///     the original context captured; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{TResult}" /> that represents the <see cref="Result{TNext}" /> value
    ///     returned by <paramref name="ifSuccessFunction" /> if the <paramref name="resultTask" /> is a successful operation,
    ///     or the <see cref="Result{TNext}" /> with the original outcome of the <paramref name="resultTask" /> operation
    ///     after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static async Task<Result<TNext>> MatchAsync<T, TNext>(
        this Task<Result<T>> resultTask,
        Func<T, Task<Result<TNext>>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction,
        bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

        if (result is null)
            throw new InvalidOperationException("The asynchronous operation returned null.");

        if (result.IsSuccess())
            return await ifSuccessFunction(result.Value).ConfigureAwait(continueOnCapturedContext);

        await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
        return result.Error;
    }

    /// <summary>
    ///     Executes one of the provided asynchronous functions depending on whether the <paramref name="resultTask" />
    ///     is a successful operation and returns their output as a <see cref="Result{TNext}" /> instance.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNext">The type of the <see cref="Result{TNext}" /> returned by the invoked function.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="resultTask" /> represents a successful operation.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous action invoked when the <paramref name="resultTask" /> represents a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> outcome is passed to this action.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to
    ///     the original context captured; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Task{TResult}" /> that represents the <see cref="Result{TNext}" /> returned by either
    ///     <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static async Task<Result<TNext>> MatchAsync<T, TNext>(
        this Task<Result<T>> resultTask,
        Func<T, Task<Result<TNext>>> ifSuccessFunction,
        Func<Error, Task<Result<TNext>>> ifErrorFunction,
        bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

        if (result is null)
            throw new InvalidOperationException("The asynchronous operation returned null.");

        return result.IsSuccess()
            ? await ifSuccessFunction(result.Value).ConfigureAwait(continueOnCapturedContext)
            : await ifErrorFunction(result.Error).ConfigureAwait(continueOnCapturedContext);
    }

    #endregion

    #region ValueTask<Result<T>>

    /// <summary>
    ///     Invokes one of the provided asynchronous actions depending on whether the <paramref name="resultTask" /> is a successful operation.
    /// </summary>
    public static async Task<Result<T>> MatchAsync<T>(this ValueTask<Result<T>> resultTask, Func<T, Task> ifSuccessAction,
        Func<Error, Task> ifErrorAction, bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

        if (result is null)
            throw new InvalidOperationException("The asynchronous operation returned null.");

        if (result.IsSuccess())
            await ifSuccessAction(result.Value).ConfigureAwait(continueOnCapturedContext);
        else
            await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);

        return result;
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its value as a <see cref="Result{TNext}" /> instance if the <paramref name="resultTask" /> is a successful operation.
    /// </summary>
    public static async Task<Result<TNext>> MatchAsync<T, TNext>(
        this ValueTask<Result<T>> resultTask,
        Func<T, Task<TNext>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction,
        bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

        if (result is null)
            throw new InvalidOperationException("The asynchronous operation returned null.");

        if (result.IsSuccess())
            return await ifSuccessFunction(result.Value).ConfigureAwait(continueOnCapturedContext);

        await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
        return result.Error;
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="resultTask" /> is a successful operation and returns their output.
    /// </summary>
    public static async Task<TNext> MatchAsync<T, TNext>(
        this ValueTask<Result<T>> resultTask,
        Func<T, Task<TNext>> ifSuccessFunction,
        Func<Error, Task<TNext>> ifErrorFunction,
        bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

        if (result is null)
            throw new InvalidOperationException("The asynchronous operation returned null.");

        return result.IsSuccess()
            ? await ifSuccessFunction(result.Value).ConfigureAwait(continueOnCapturedContext)
            : await ifErrorFunction(result.Error).ConfigureAwait(continueOnCapturedContext);
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its value as an asynchronous operation that returns a <see cref="Result{TNext}" /> instance if the <paramref name="resultTask" /> is a successful operation.
    /// </summary>
    public static async Task<Result<TNext>> MatchAsync<T, TNext>(
        this ValueTask<Result<T>> resultTask,
        Func<T, Task<Result<TNext>>> ifSuccessFunction,
        Func<Error, Task> ifErrorAction,
        bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

        if (result is null)
            throw new InvalidOperationException("The asynchronous operation returned null.");

        if (result.IsSuccess())
            return await ifSuccessFunction(result.Value).ConfigureAwait(continueOnCapturedContext);

        await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
        return result.Error;
    }

    /// <summary>
    ///     Executes one of the provided asynchronous functions depending on whether the <paramref name="resultTask" /> is a successful operation and returns their output as a <see cref="Result{TNext}" /> instance.
    /// </summary>
    public static async Task<Result<TNext>> MatchAsync<T, TNext>(
        this ValueTask<Result<T>> resultTask,
        Func<T, Task<Result<TNext>>> ifSuccessFunction,
        Func<Error, Task<Result<TNext>>> ifErrorFunction,
        bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

        if (result is null)
            throw new InvalidOperationException("The asynchronous operation returned null.");

        return result.IsSuccess()
            ? await ifSuccessFunction(result.Value).ConfigureAwait(continueOnCapturedContext)
            : await ifErrorFunction(result.Error).ConfigureAwait(continueOnCapturedContext);
    }

    #endregion

}
