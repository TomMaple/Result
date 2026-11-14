// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System;
using System.Threading.Tasks;

namespace Maple.Result.Extensions.ValueTasks;

/// <summary>
///     The collection of asynchronous extension methods, represented by <see cref="ValueTask" />, for the <see cref="Result{T}" /> type.
/// </summary>
/// <remarks>
///     These methods are declared in the <c>Maple.Result.Extensions.ValueTasks</c> namespace, separately from their
///     <see cref="System.Threading.Tasks.Task" />-based counterparts in <c>Maple.Result.Extensions</c>, because both sets declare the same method names and
///     an asynchronous lambda is convertible to a <c>Func&lt;Task&gt;</c> as well as to a <c>Func&lt;ValueTask&gt;</c> delegate.
///     Importing only one of these namespaces keeps such calls unambiguous.
/// </remarks>
public static class ResultValueTaskAsyncExtensions
{
    /// <summary>
    ///     Asynchronously converts a value task that produces a generic result into a non-generic result, preserving success or <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     Use this method to convert a <see cref="ValueTask{TResult}" /> that represents <see cref="Result{T}" /> to a <see cref="Result{ValueTask}" /> when
    ///     the value is not needed but the success or error state with the original <see cref="Error" /> must be preserved.
    /// </remarks>
    /// <typeparam name="T">The type of the value contained in the original result.</typeparam>
    /// <param name="resultTask">The asynchronous operation to be executed and which result is to be inspected.</param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A value task that represents the <see cref="Result" /> without a value. The result indicates success if the original result was successful;
    ///     otherwise, it contains the <see cref="Error" /> from the original result.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static async ValueTask<Result> ToResultAsync<T>(this ValueTask<Result<T>> resultTask, bool? continueOnCapturedContext = null)
    {
        var result = await resultTask.ConfigureAwait(continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        if (result is null)
            throw new InvalidOperationException("The asynchronous operation returned null.");

        return result.IsSuccess()
            ? Result.Success()
            : Result.FromError(result.Error);
    }
}
