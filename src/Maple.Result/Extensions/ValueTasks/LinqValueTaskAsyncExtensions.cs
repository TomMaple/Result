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

namespace Maple.Result.Extensions.ValueTasks;

/// <summary>
///     The collection of LINQ extension methods for a <see cref="Result" /> instance that support asynchronous
///     operations represented by <see cref="ValueTask" />.
/// </summary>
/// <remarks>
///     These methods enable fluent chaining of asynchronous operations and using LINQ query syntax
///     with asynchronous workflows.
///     They help simplify error handling and reduce boilerplate code when working with
///     <see cref="Result{T}" />-based asynchronous workflows.
///     <para>
///         These methods are declared in the <c>Maple.Result.Extensions.ValueTasks</c> namespace, separately from
///         their <see cref="System.Threading.Tasks.Task" />-based counterparts in <c>Maple.Result.Extensions</c>,
///         because both sets declare the same method names and an asynchronous lambda is convertible to a
///         <c>Func&lt;Task&gt;</c> as well as to a <c>Func&lt;ValueTask&gt;</c> delegate. Importing only one of these
///         namespaces keeps such calls unambiguous.
///     </para>
/// </remarks>
public static class LinqValueTaskAsyncExtensions
{
    /// <summary>
    ///     Projects the value of a successful <see cref="Result{T}" /> produced by the awaited <paramref name="result" />
    ///     into a new <see cref="Result{TNext}" /> by applying the provided <paramref name="selector" /> function; or
    ///     returns a new <see cref="Result{TNext}" /> with the same error, otherwise.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the <see cref="Result{T}" /> value used to determine whether to execute the passed
    ///     <paramref name="selector" /> function. If successful, this is also the type of the parameter passed to
    ///     that function.
    /// </typeparam>
    /// <typeparam name="TNext">
    ///     The output type of the passed <paramref name="selector" /> function and the type of the
    ///     <see cref="Result{TNext}" /> value to return.
    /// </typeparam>
    /// <param name="result">
    ///     The <see cref="ValueTask{TResult}" /> that represents the initial result to transform if it is successful.
    /// </param>
    /// <param name="selector">A function that maps the value of the initial result to the final value.</param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents a successful <see cref="Result{TNext}" /> containing the
    ///     value produced by <paramref name="selector" /> if the <paramref name="result" /> is successful; otherwise, a
    ///     <see cref="ValueTask{TResult}" /> that represents a failed <see cref="Result{TNext}" /> with the original
    ///     <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="selector" /> parameter is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />.
    /// </exception>
    /// <remarks>
    ///     This method enables the single-clause LINQ query syntax (a <c>from … select …</c> query without an intermediate
    ///     <c>from</c>) on a <see cref="ValueTask{TResult}" /> of a <see cref="Result{T}" />, projecting a successful value
    ///     while propagating an error unchanged.
    /// </remarks>
    public static async ValueTask<Result<TNext>> Select<T, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, TNext> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        var resultValue = await result.ConfigureAwait(false);

        if (resultValue is null)
            throw new InvalidOperationException("The asynchronous operation represented by ‘result’ returned null.");

        return resultValue.IfSuccess(selector);
    }

    /// <summary>
    ///     Returns a new instance of the <see cref="ValueTask{TResult}" /> that represents the result of applying
    ///     the provided collection selector and a result selector functions if the current <see cref="Result{T}" />
    ///     is successful, or the new <see cref="ValueTask{TResult}" /> that represents a new <see cref="Result{TNext}" />
    ///     instance with the same error otherwise.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the <see cref="Result{T}" /> value used to determine whether to execute
    ///     the passed <paramref name="collectionSelector" /> and <paramref name="resultSelector" /> functions.
    ///     If successful, this is also the type of the parameter passed to that function.
    /// </typeparam>
    /// <typeparam name="TMiddle">
    ///     The type of the <see cref="Result{TMiddle}" /> output type of the passed <paramref name="collectionSelector" />
    ///     function and the type of one of the parameters passed to the <paramref name="resultSelector" /> function.
    /// </typeparam>
    /// <typeparam name="TNext">
    ///     The output type of the <see cref="Result{TNext}" /> value to return.
    /// </typeparam>
    /// <param name="result">
    ///     The <see cref="ValueTask{TResult}" /> that represents the initial result to transform if it is
    ///     successful.
    /// </param>
    /// <param name="collectionSelector">
    ///     A function that takes the value of the initial result and
    ///     returns a new result representing an intermediate value.
    /// </param>
    /// <param name="resultSelector">
    ///     A function that combines the value from the initial result and the intermediate value to produce the final value.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="collectionSelector" /> or
    ///     <paramref name="resultSelector" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />.
    /// </exception>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents the result of applying the provided collection selector and
    ///     a result selector functions if the current <paramref name="result" /> is successful;
    ///     otherwise, a <see cref="ValueTask{TResult}" /> that represents a failed result.
    /// </returns>
    public static async ValueTask<Result<TNext>> SelectMany<T, TMiddle, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, TNext> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        var resultValue = await result.ConfigureAwait(false);

        if (resultValue is null)
            throw new InvalidOperationException("The asynchronous operation represented by ‘result’ returned null.");

        return await resultValue.IfSuccessAsync(async ValueTask<Result<TNext>> (x) =>
        {
            var collectionResult = await collectionSelector(x).ConfigureAwait(false);
            return collectionResult.IfSuccess(y => resultSelector(x, y));
        }).ConfigureAwait(false);
    }
}
