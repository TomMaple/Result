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

namespace Maple.Result.Extensions;

/// <summary>
///     The collection of extension methods for executing actions or functions conditionally based on the error state of a
///     <see cref="IResult" /> instance.
/// </summary>
/// <remarks>
///     These methods enable fluent handling of error scenarios by allowing to specify actions or functions
///     that should be executed only when the result indicates an error.
/// </remarks>
public static class IfErrorExtensions
{
    /// <summary>
    ///     Invokes the specified action if the <paramref name="result" /> represents an error,
    ///     and returns the original <paramref name="result" />.
    /// </summary>
    /// <remarks>
    ///     Use this method to perform side effects, such as logging or error handling, when a <paramref name="result" />
    ///     indicates an error, without altering the <paramref name="result" /> itself. The method does not modify
    ///     the <paramref name="result" /> or handle the <see cref="Error" /> beyond invoking the specified action.
    /// </remarks>
    /// <typeparam name="TResult">The type of the result object, which must implement the <see cref="IResult" /> interface.</typeparam>
    /// <param name="result">The <paramref name="result" /> to inspect for an error. Must not be <see langword="null" />.</param>
    /// <param name="ifErrorAction">
    ///     The action to execute if the <paramref name="result" /> is an error. The <see cref="Error" /> associated with
    ///     the <paramref name="result" /> is passed to this action. Must not be <see langword="null" />.
    /// </param>
    /// <returns>The original <paramref name="result" /> instance, regardless of whether the action was invoked.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifErrorAction" />
    ///     parameters are <see langword="null" />.
    /// </exception>
    public static TResult IfError<TResult>(this TResult result, Action<Error> ifErrorAction)
        where TResult : IResult
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (!result.IsSuccess())
            ifErrorAction(result.Error!);

        return result;
    }

    /// <summary>
    ///     Invokes the specified function if the <paramref name="result" /> represents an error and returns its outcome;
    ///     otherwise, returns the original <paramref name="result" />.
    /// </summary>
    /// <typeparam name="TResult">The type of the result object, which must implement the <see cref="IResult" /> interface.</typeparam>
    /// <param name="result">The <paramref name="result" /> to inspect for an error. Must not be <see langword="null" />.</param>
    /// <param name="ifErrorFunction">
    ///     The function to execute if the <paramref name="result" /> is an error. The <see cref="Error" /> associated with
    ///     the <paramref name="result" /> is passed to this function, and the function’s return value is used as the outcome.
    ///     The function must return <see cref="TResult" />. Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     The original <paramref name="result" /> if it represents success;
    ///     otherwise, the outcome of the <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifErrorFunction" />
    ///     parameters are <see langword="null" />.
    /// </exception>
    public static TResult IfError<TResult>(this TResult result, Func<Error, TResult> ifErrorFunction)
        where TResult : IResult
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        if (result.IsSuccess())
            return result;

        return ifErrorFunction(result.Error!);
    }
}
