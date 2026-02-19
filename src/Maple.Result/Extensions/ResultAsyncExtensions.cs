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
///     The collection of asynchronous extension methods for the <see cref="Result{T}" /> type.
/// </summary>
public static class ResultAsyncExtensions
{
    /// <summary>
    ///     Asynchronously converts a task that produces a generic result into a non-generic result, preserving success or
    ///     <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     Use this method to convert a <see cref="Task{TResult}" /> that represents <see cref="Result{T}" /> to
    ///     a <see cref="Result{Task}" /> when the value is not needed
    ///     but the success or error state with the original <see cref="Error" /> must be preserved.
    /// </remarks>
    /// <typeparam name="T">The type of the value contained in the original result.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to attempt to marshal the continuation back to
    ///     the original context captured; otherwise, <see langword="false" />.
    /// </param>
    /// <returns>
    ///     A task that represents the <see cref="Result" /> without a value.
    ///     The result indicates success if the original result was successful;
    ///     otherwise, it contains the <see cref="Error" /> from the original result.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="resultTask" /> is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static async Task<Result> ToResultAsync<T>(this Task<Result<T>> resultTask,
        bool continueOnCapturedContext = false)
    {
        ArgumentNullException.ThrowIfNull(resultTask);

        var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

        if (result is null)
            throw new InvalidOperationException("The asynchronous operation returned null.");

        return result.IsSuccess()
            ? Result.Success()
            : Result.FromError(result.Error!);
    }
}