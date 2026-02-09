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
///     The collection of LINQ extension methods for a <see cref="Result" /> instance.
/// </summary>
/// <remarks>
///     These methods enable fluent chaining of operations and using LINQ query syntax.
///     They help simplify error handling and reduce boilerplate code when working with
///     <see cref="Result{T}" />-based workflows.
/// </remarks>
public static class LinqExtensions
{
    /// <summary>
    ///     Returns a new <see cref="Result{TNext}" /> instance by applying the provided selector function to the value
    ///     if the current <see cref="Result{T}" /> if it is successful,
    ///     or a new <see cref="Result{TNext}" /> instance with the same error if it is not successful.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the <see cref="Result{T}" /> value used to determine whether to execute the passed
    ///     <paramref name="selector" /> function.
    ///     If successful, this is also the type of the parameter passed to that function.
    /// </typeparam>
    /// <typeparam name="TNext">
    ///     The output type of the passed function and the type of the <see cref="Result{TNext}" />
    ///     value to return.
    /// </typeparam>
    /// <param name="result">The <see cref="Result{T}" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="selector">
    ///     The function to execute if the <paramref name="result" /> is successful.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter.
    ///     The function’s return value is used as the successful <see cref="Result{TNext}" />.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A successful <see cref="Result{TNext}" /> containing the value returned by <paramref name="selector" />
    ///     if <paramref name="result" /> is successful;
    ///     otherwise, a failed <see cref="Result{TNext}" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="selector" />
    ///     parameters are <see langword="null" />.
    /// </exception>
    /// <remarks>
    ///     This method enables chaining of operations on successful results while propagating errors in LINQ query syntax
    ///     without additional boilerplate code.
    /// </remarks>
    public static Result<TNext> Select<T, TNext>(this Result<T> result, Func<T, TNext> selector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(selector);

        return result.IfSuccess(selector);
    }

    /// <summary>
    ///     Returns a new <see cref="Result{TNext}" /> instance by applying the provided collection selector and
    ///     a result selector functions if the current <see cref="Result{T}" /> if it is successful, or
    ///     a new <see cref="Result{TNext}" /> instance with the same error if it is not successful.
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
    /// <param name="result">The initial result to transform if it is successful.</param>
    /// <param name="collectionSelector">
    ///     A function that takes the value of the initial result and
    ///     returns a new result representing an intermediate value.
    /// </param>
    /// <param name="resultSelector">
    ///     A function that combines the value from the initial result and the intermediate value to produce the final value.
    /// </param>
    /// <returns>
    ///     A result containing the value produced by the result selector if both the initial result and
    ///     the intermediate result are successful; otherwise, a failed result.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="collectionSelector" /> or
    ///     <paramref name="resultSelector" /> parameters are <see langword="null" />.
    /// </exception>
    /// <remarks>
    ///     This method enables chaining of operations on successful results while propagating errors in LINQ query syntax
    ///     without additional boilerplate code.
    /// </remarks>
    public static Result<TNext> SelectMany<T, TMiddle, TNext>(
        this Result<T> result,
        Func<T, Result<TMiddle>> collectionSelector,
        Func<T, TMiddle, TNext> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return result.IfSuccess(x =>
        {
            var collectionResult = collectionSelector(x);
            return collectionResult.IfSuccess(y => resultSelector(x, y));
        });
    }
}