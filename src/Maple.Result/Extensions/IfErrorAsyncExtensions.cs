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
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Maple.Result.Extensions;

/// <summary>
///     The collection of extension methods for executing asynchronous actions or functions conditionally based on
///     the error state of a <see cref="IResult" /> instance.
/// </summary>
/// <remarks>
///     These methods enable fluent handling of error scenarios by allowing to specify actions or functions
///     that should be executed only when the result indicates an error.
/// </remarks>
public static class IfErrorAsyncExtensions
{
    /// <summary>
    ///     Invokes the specified asynchronous action if the <paramref name="result" /> represents an error,
    ///     and returns the original <paramref name="result" />.
    /// </summary>
    /// <remarks>
    ///     Use this method to perform side effects, such as logging or error handling, when a <paramref name="result" />
    ///     indicates an error, without altering the <paramref name="result" /> itself. The method does not modify
    ///     the <paramref name="result" /> or handle the <see cref="Error" /> beyond invoking the specified asynchronous
    ///     action.
    /// </remarks>
    /// <typeparam name="TResult">The type of the result object, which must implement the <see cref="IResult" /> interface.</typeparam>
    /// <param name="result">The <paramref name="result" /> to inspect for an error. Must not be <see langword="null" />.</param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action to execute if the <paramref name="result" /> is an error.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this asynchronous action.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to
    ///     the original context captured; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{IResult}" /> that represents the original <paramref name="result" /> instance,
    ///     regardless of whether the action was invoked.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifErrorAction" />
    ///     parameters are <see langword="null" />.
    /// </exception>
    public static async Task<TResult> IfErrorAsync<TResult>(
        [DisallowNull] this TResult result,
        Func<Error, Task> ifErrorAction,
        bool continueOnCapturedContext = false
    )
        where TResult : IResult
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (!result.IsSuccess())
            await ifErrorAction(result.Error!).ConfigureAwait(continueOnCapturedContext);

        return result;
    }

    /// <summary>
    ///     Invokes the specified asynchronous function if the <paramref name="result" /> represents an error
    ///     and returns its outcome; otherwise, returns the original <paramref name="result" />.
    /// </summary>
    /// <remarks>
    ///     Use this method to handle error cases in a fluent, asynchronous manner without affecting successful results.
    ///     The <paramref name="ifErrorFunction" /> is only invoked if the <paramref name="result" /> indicates an error.
    /// </remarks>
    /// <typeparam name="TResult">
    ///     The type of the <paramref name="result" />, which must implement the <see cref="IResult" /> interface.
    /// </typeparam>
    /// <param name="result">The result to evaluate for success or error. Must not be <see langword="null" />.</param>
    /// <param name="ifErrorFunction">
    ///     The function to invoke asynchronously if the <paramref name="result" /> represents an error.
    ///     The function receives the error and returns a new result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to
    ///     the original context captured; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{IResult}" /> that represents the <see cref="IResult" /> outcome of the asynchronous operation.
    ///     The <see cref="Task{IResult}" /> is either the original <paramref name="result" /> if it indicates success, or
    ///     the result returned by the <paramref name="ifErrorFunction" /> if an <see cref="Error" /> is present.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifErrorFunction" />
    ///     parameters are <see langword="null" />.
    /// </exception>
    public static async Task<TResult> IfErrorAsync<TResult>(
        [DisallowNull] this TResult result,
        Func<Error, Task<TResult>> ifErrorFunction,
        bool continueOnCapturedContext = false
    )
        where TResult : IResult
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        if (result.IsSuccess())
            return result;

        return await ifErrorFunction(result.Error!).ConfigureAwait(continueOnCapturedContext);
    }

    /// <summary>
    ///     Invokes the specified asynchronous action if the <paramref name="resultTask" /> represents an error operation,
    ///     and returns its <typeparamref name="TResult" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Use this method to perform side effects, such as logging or error handling,
    ///         when a <paramref name="resultTask" /> indicates an asynchronous error operation,
    ///         without altering the operation result.
    ///     </para>
    ///     <para>
    ///         The method executes passed <paramref name="resultTask" /> asynchronous operation, does not modify its result.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TResult">
    ///     The type of the operation result object, which must implement the <see cref="IResult" /> interface.
    /// </typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action to execute if the <paramref name="resultTask" /> returns an error.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> result is passed to
    ///     this asynchronous action.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to
    ///     the original context captured; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{IResult}" /> that represents the outcome of the <paramref name="resultTask" /> asynchronous
    ///     operation,
    ///     regardless of whether the action was invoked.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" /> or <paramref name="ifErrorAction" />
    ///     parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static async Task<TResult> IfErrorAsync<TResult>(
        this Task<TResult> resultTask,
        Func<Error, Task> ifErrorAction,
        bool continueOnCapturedContext = false
    )
        where TResult : IResult
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

        if (result is null)
            throw new InvalidOperationException("The asynchronous operation returned null.");

        if (!result.IsSuccess())
            await ifErrorAction(result.Error!).ConfigureAwait(continueOnCapturedContext);

        return result;
    }

    /// <summary>
    ///     Invokes the specified asynchronous function if the <paramref name="resultTask" /> represents an error operation
    ///     and returns its outcome; otherwise, returns the outcome of the <paramref name="resultTask" /> asynchronous
    ///     operation.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Use this method to handle error cases in a fluent, asynchronous manner without affecting successful results.
    ///         The <paramref name="ifErrorFunction" /> is only invoked if the outcome of the <paramref name="resultTask" />
    ///         asynchronous operation indicates an error.
    ///     </para>
    ///     <para>
    ///         The method executes passed <paramref name="resultTask" /> asynchronous operation, does not modify its result.
    ///     </para>
    /// </remarks>
    /// <typeparam name="TResult">
    ///     The type of the operation result object, which must implement the <see cref="IResult" /> interface.
    /// </typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function to invoke asynchronously if the <paramref name="resultTask" /> returns an error.
    ///     The function receives the error and returns a new result. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to
    ///     the original context captured; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{IResult}" /> that represents the <see cref="IResult" /> outcome of the asynchronous operation.
    ///     The <see cref="Task{IResult}" /> is either the outcome of the original <paramref name="resultTask" /> if it
    ///     indicates success, or
    ///     the result returned by the <paramref name="ifErrorFunction" /> if an <see cref="Error" /> is present.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="resultTask" /> or <paramref name="ifErrorFunction" />
    ///     parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static async Task<TResult> IfErrorAsync<TResult>(
        this Task<TResult> resultTask,
        Func<Error, Task<TResult>> ifErrorFunction,
        bool continueOnCapturedContext = false
    )
        where TResult : IResult
    {
        ArgumentNullException.ThrowIfNull(resultTask);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

        if (result is null)
            throw new InvalidOperationException("The asynchronous operation returned null.");

        if (result.IsSuccess())
            return result;

        return await ifErrorFunction(result.Error!).ConfigureAwait(continueOnCapturedContext);
    }
}
