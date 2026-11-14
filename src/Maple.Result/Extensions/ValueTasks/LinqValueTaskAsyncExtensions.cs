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
using System.Threading.Tasks;

namespace Maple.Result.Extensions.ValueTasks;

/// <summary>
///     The collection of LINQ extension methods for a <see cref="Result" /> instance that support asynchronous operations represented by
///     <see cref="ValueTask" />.
/// </summary>
/// <remarks>
///     These methods enable fluent chaining of asynchronous operations and using LINQ query syntax with asynchronous workflows.
///     They help simplify error handling and reduce boilerplate code when working with <see cref="Result{T}" />-based asynchronous workflows.
///     <para>
///         These methods are declared in the <c>Maple.Result.Extensions.ValueTasks</c> namespace, separately from their
///         <see cref="System.Threading.Tasks.Task" />-based counterparts in <c>Maple.Result.Extensions</c>, because both sets declare the same method names and
///         an asynchronous lambda is convertible to a <c>Func&lt;Task&gt;</c> as well as to a <c>Func&lt;ValueTask&gt;</c> delegate.
///         Importing only one of these namespaces keeps such calls unambiguous.
///     </para>
///     <para>
///         The query syntax cannot pass a <c>continueOnCapturedContext</c> argument, so these methods use
///         <see cref="ResultConfiguration.ContinueOnCapturedContext" /> to decide whether the selectors run on the captured context (e.g., the UI thread).
///     </para>
/// </remarks>
public static class LinqValueTaskAsyncExtensions
{
    /// <summary>
    ///     Projects the value of a successful <see cref="Result{T}" /> produced by the awaited <paramref name="result" /> into a new
    ///     <see cref="Result{TNext}" /> by applying the provided <paramref name="selector" /> function;
    ///     or returns a new <see cref="Result{TNext}" /> with the same error, otherwise.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the <see cref="Result{T}" /> value used to determine whether to execute the passed <paramref name="selector" /> function.
    ///     If successful, this is also the type of the parameter passed to that function.
    /// </typeparam>
    /// <typeparam name="TNext">
    ///     The output type of the passed <paramref name="selector" /> function and the type of the <see cref="Result{TNext}" /> value to return.
    /// </typeparam>
    /// <param name="result">
    ///     The <see cref="ValueTask{TResult}" /> that represents the initial result to transform if it is successful.
    /// </param>
    /// <param name="selector">A function that maps the value of the initial result to the final value.</param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents a successful <see cref="Result{TNext}" /> containing the value produced by
    ///     <paramref name="selector" /> if the <paramref name="result" /> is successful;
    ///     otherwise, a <see cref="ValueTask{TResult}" /> that represents a failed <see cref="Result{TNext}" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="selector" /> parameter is <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />.
    /// </exception>
    /// <remarks>
    ///     This method enables the single-clause LINQ query syntax (a <c>from … select …</c> query without an intermediate <c>from</c>) on
    ///     a <see cref="ValueTask{TResult}" /> of a <see cref="Result{T}" />, projecting a successful value while propagating an error unchanged.
    /// </remarks>
    public static ValueTask<Result<TNext>> Select<T, TNext>(this ValueTask<Result<T>> result, Func<T, TNext> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        return Core(result, selector);

        static async ValueTask<Result<TNext>> Core(ValueTask<Result<T>> result, Func<T, TNext> selector)
        {
            var resultValue = await result.ConfigureAwait(ResultConfiguration.ContinueOnCapturedContext);

            if (resultValue is null)
                throw Returned.NullAwaited(nameof(result));

            return resultValue.IfSuccess(selector);
        }
    }

    /// <summary>
    ///     Applies the provided <paramref name="selector" /> to the value of a successful <see cref="Result{T}" /> produced by the awaited
    ///     <paramref name="result" /> and returns a failed <see cref="Result" /> with the <see cref="Error" /> it returns;
    ///     otherwise, returns a failed <see cref="Result" /> with the original <see cref="Error" />.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the <see cref="Result{T}" /> value used to determine whether to execute the passed <paramref name="selector" /> function.
    ///     If successful, this is also the type of the parameter passed to that function.
    /// </typeparam>
    /// <param name="result">The <see cref="ValueTask{TResult}" /> that represents the initial result.</param>
    /// <param name="selector">A function that takes the value of the initial result and returns the <see cref="Error" /> of the failed result.</param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents a failed <see cref="Result" /> with the <see cref="Error" /> returned by
    ///     <paramref name="selector" /> if the <paramref name="result" /> is successful;
    ///     otherwise, a failed <see cref="Result" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="selector" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />,
    ///     or <paramref name="selector" /> returns <see langword="null" />.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method makes a query that selects an <see cref="Error" />, e.g., <c>from user in GetUserAsync(id) select Error.Conflict(…)</c>,
    ///         return a failed <see cref="Result" />.
    ///     </para>
    ///     <para>
    ///         Without this overload, such a query would return a <b>successful</b> <c>Result&lt;Error&gt;</c>.
    ///     </para>
    /// </remarks>
    public static ValueTask<Result> Select<T>(this ValueTask<Result<T>> result, Func<T, Error> selector)
    {
        ArgumentNullException.ThrowIfNull(selector);

        return Core(result, selector);

        static async ValueTask<Result> Core(ValueTask<Result<T>> result, Func<T, Error> selector)
        {
            var resultValue = await result.ConfigureAwait(ResultConfiguration.ContinueOnCapturedContext);

            if (resultValue is null)
                throw Returned.NullAwaited(nameof(result));

            if (!resultValue.IsSuccess())
                return resultValue.Error;

            return Result.FromError(Returned.NotNull(selector(resultValue.Value), nameof(selector)));
        }
    }

    /// <summary>
    ///     Returns a new instance of the <see cref="ValueTask{TResult}" /> that represents the result of applying the provided collection selector and
    ///     a result selector functions if the current <see cref="Result{T}" /> is successful,
    ///     or the new <see cref="ValueTask{TResult}" /> that represents a new <see cref="Result{TNext}" /> instance with the same error otherwise.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the <see cref="Result{T}" /> value used to determine whether to execute the passed <paramref name="collectionSelector" /> and
    ///     <paramref name="resultSelector" /> functions. If successful, this is also the type of the parameter passed to that function.
    /// </typeparam>
    /// <typeparam name="TMiddle">
    ///     The type of the <see cref="Result{TMiddle}" /> output type of the passed <paramref name="collectionSelector" /> function and the type of one of
    ///     the parameters passed to the <paramref name="resultSelector" /> function.
    /// </typeparam>
    /// <typeparam name="TNext">
    ///     The output type of the <see cref="Result{TNext}" /> value to return.
    /// </typeparam>
    /// <param name="result">
    ///     The <see cref="ValueTask{TResult}" /> that represents the initial result to transform if it is successful.
    /// </param>
    /// <param name="collectionSelector">
    ///     A function that takes the value of the initial result and returns a new result representing an intermediate value.
    /// </param>
    /// <param name="resultSelector">
    ///     A function that combines the value from the initial result and the intermediate value to produce the final value.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="collectionSelector" /> or <paramref name="resultSelector" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />,
    ///     or <paramref name="collectionSelector" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents the result of applying the provided collection selector and a result selector functions
    ///     if the current <paramref name="result" /> is successful; otherwise, a <see cref="ValueTask{TResult}" /> that represents a failed result.
    /// </returns>
    public static ValueTask<Result<TNext>> SelectMany<T, TMiddle, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, TNext> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result<TNext>> Core(
            ValueTask<Result<T>> result,
            Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
            Func<T, TMiddle, TNext> resultSelector)
        {
            var resultValue = await result.ConfigureAwait(ResultConfiguration.ContinueOnCapturedContext);

            if (resultValue is null)
                throw Returned.NullAwaited(nameof(result));

            return await resultValue.IfSuccessAsync(async ValueTask<Result<TNext>> (x) =>
            {
                var collectionResult = await Returned.NotNullResult(
                    collectionSelector(x), nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);
                return collectionResult.IfSuccess(y => resultSelector(x, y));
            }).ConfigureAwait(ResultConfiguration.ContinueOnCapturedContext);
        }
    }

    /// <summary>
    ///     Applies the synchronous <paramref name="collectionSelector" /> and the <paramref name="resultSelector" /> to the value of a successful
    ///     <see cref="Result{T}" /> produced by the awaited <paramref name="result" />;
    ///     otherwise, returns a failed <see cref="Result{TNext}" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     This method enables LINQ query syntax that mixes an asynchronous <c>from</c> clause followed by a synchronous one,
    ///     e.g., <c>from user in GetUserAsync(id) from address in GetAddress(user) select …</c>.
    /// </remarks>
    /// <typeparam name="T">
    ///     The type of the value of the initial result. If successful, this is also the type of the parameter passed to the selectors.
    /// </typeparam>
    /// <typeparam name="TMiddle">The type of the value of the intermediate result returned by <paramref name="collectionSelector" />.</typeparam>
    /// <typeparam name="TNext">The type of the value of the final result, produced by <paramref name="resultSelector" />.</typeparam>
    /// <param name="result">The <see cref="ValueTask{TResult}" /> that represents the initial result.</param>
    /// <param name="collectionSelector">
    ///     A synchronous function that takes the value of the initial result and returns a result with an intermediate value.
    /// </param>
    /// <param name="resultSelector">
    ///     A function that combines the value of the initial result and the intermediate value to produce the final value.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents the result of applying the selectors if both the initial and the intermediate results are
    ///     successful; otherwise, a failed result with the first <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />,
    ///     <paramref name="collectionSelector" /> or <paramref name="resultSelector" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />,
    ///     or <paramref name="collectionSelector" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result<TNext>> SelectMany<T, TMiddle, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, Result<TMiddle>> collectionSelector,
        Func<T, TMiddle, TNext> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result<TNext>> Core(
            ValueTask<Result<T>> result,
            Func<T, Result<TMiddle>> collectionSelector,
            Func<T, TMiddle, TNext> resultSelector)
        {
            var resultValue = await result.ConfigureAwait(ResultConfiguration.ContinueOnCapturedContext);

            if (resultValue is null)
                throw Returned.NullAwaited(nameof(result));

            return resultValue.SelectMany(collectionSelector, resultSelector);
        }
    }

    /// <summary>
    ///     Applies the asynchronous <paramref name="collectionSelector" /> and the <paramref name="resultSelector" /> to the value of a successful
    ///     <paramref name="result" />; otherwise, returns a failed <see cref="Result{TNext}" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     This method enables LINQ query syntax that mixes a synchronous <c>from</c> clause followed by an asynchronous one,
    ///     e.g., <c>from user in GetUser(id) from orders in GetOrdersAsync(user) select …</c>.
    /// </remarks>
    /// <typeparam name="T">
    ///     The type of the value of the initial result. If successful, this is also the type of the parameter passed to the selectors.
    /// </typeparam>
    /// <typeparam name="TMiddle">The type of the value of the intermediate result returned by <paramref name="collectionSelector" />.</typeparam>
    /// <typeparam name="TNext">The type of the value of the final result, produced by <paramref name="resultSelector" />.</typeparam>
    /// <param name="result">The initial result.</param>
    /// <param name="collectionSelector">
    ///     An asynchronous function that takes the value of the initial result and returns a result with an intermediate value.
    /// </param>
    /// <param name="resultSelector">
    ///     A function that combines the value of the initial result and the intermediate value to produce the final value.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents the result of applying the selectors if both the initial and the intermediate results are
    ///     successful; otherwise, a failed result with the first <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />,
    ///     <paramref name="collectionSelector" /> or <paramref name="resultSelector" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="collectionSelector" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    public static ValueTask<Result<TNext>> SelectMany<T, TMiddle, TNext>(
        this Result<T> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, TNext> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result<TNext>> Core(
            Result<T> result,
            Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
            Func<T, TMiddle, TNext> resultSelector)
        {
            if (!result.IsSuccess())
                return result.Error;

            var collectionResult = await Returned.NotNullResult(
                collectionSelector(result.Value), nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);
            return collectionResult.IfSuccess(y => resultSelector(result.Value, y));
        }
    }

    /// <summary>
    ///     Applies the asynchronous <paramref name="collectionSelector" /> and the <paramref name="resultSelector" /> to the value of a successful
    ///     <see cref="Result{T}" /> produced by the awaited <paramref name="result" /> and returns a failed <see cref="Result" /> with
    ///     the <see cref="Error" /> returned by <paramref name="resultSelector" />; otherwise, returns a failed <see cref="Result" /> with
    ///     the first <see cref="Error" />.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the value of the initial result. If successful, this is also the type of the parameter passed to the selectors.
    /// </typeparam>
    /// <typeparam name="TMiddle">The type of the value of the intermediate result returned by <paramref name="collectionSelector" />.</typeparam>
    /// <param name="result">The <see cref="ValueTask{TResult}" /> that represents the initial result.</param>
    /// <param name="collectionSelector">
    ///     An asynchronous function that takes the value of the initial result and returns a result with an intermediate value.
    /// </param>
    /// <param name="resultSelector">
    ///     A function that takes the value of the initial result and the intermediate value, and returns the <see cref="Error" /> of the failed result.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents a failed <see cref="Result" /> with the <see cref="Error" /> returned by
    ///     <paramref name="resultSelector" /> if both the initial and the intermediate results are successful;
    ///     otherwise, a failed <see cref="Result" /> with the first <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="collectionSelector" /> or <paramref name="resultSelector" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />,
    ///     <paramref name="collectionSelector" /> returns a value task with a <see langword="null" /> result,
    ///     or <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method makes a query with more than one <c>from</c> clause that selects an <see cref="Error" />,
    ///         e.g., <c>from user in GetUserAsync(id) from order in GetOrderAsync(user) select Error.Conflict(…)</c>, return a failed <see cref="Result" />.
    ///     </para>
    ///     <para>
    ///         Without this overload, such a query would return a <b>successful</b> <c>Result&lt;Error&gt;</c>.
    ///     </para>
    /// </remarks>
    public static ValueTask<Result> SelectMany<T, TMiddle>(
        this ValueTask<Result<T>> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, Error> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result> Core(
            ValueTask<Result<T>> result,
            Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
            Func<T, TMiddle, Error> resultSelector)
        {
            var resultValue = await result.ConfigureAwait(ResultConfiguration.ContinueOnCapturedContext);

            if (resultValue is null)
                throw Returned.NullAwaited(nameof(result));

            if (!resultValue.IsSuccess())
                return resultValue.Error;

            var collectionResult = await Returned.NotNullResult(
                collectionSelector(resultValue.Value), nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);
            return collectionResult.IfSuccess(y => Returned.NotNull(resultSelector(resultValue.Value, y), nameof(resultSelector)));
        }
    }

    /// <summary>
    ///     Applies the synchronous <paramref name="collectionSelector" /> and the <paramref name="resultSelector" /> to the value of a successful
    ///     <see cref="Result{T}" /> produced by the awaited <paramref name="result" /> and returns a failed <see cref="Result" /> with
    ///     the <see cref="Error" /> returned by <paramref name="resultSelector" />; otherwise, returns a failed <see cref="Result" /> with
    ///     the first <see cref="Error" />.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the value of the initial result. If successful, this is also the type of the parameter passed to the selectors.
    /// </typeparam>
    /// <typeparam name="TMiddle">The type of the value of the intermediate result returned by <paramref name="collectionSelector" />.</typeparam>
    /// <param name="result">The <see cref="ValueTask{TResult}" /> that represents the initial result.</param>
    /// <param name="collectionSelector">
    ///     A synchronous function that takes the value of the initial result and returns a result with an intermediate value.
    /// </param>
    /// <param name="resultSelector">
    ///     A function that takes the value of the initial result and the intermediate value, and returns the <see cref="Error" /> of the failed result.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents a failed <see cref="Result" /> with the <see cref="Error" /> returned by
    ///     <paramref name="resultSelector" /> if both the initial and the intermediate results are successful;
    ///     otherwise, a failed <see cref="Result" /> with the first <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="collectionSelector" /> or <paramref name="resultSelector" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="result" /> returns <see langword="null" />,
    ///     <paramref name="collectionSelector" /> returns <see langword="null" />, or <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method makes a query with more than one <c>from</c> clause that selects an <see cref="Error" />,
    ///         e.g., <c>from user in GetUserAsync(id) from order in GetOrder(user) select Error.Conflict(…)</c>, return a failed <see cref="Result" />.
    ///     </para>
    ///     <para>
    ///         Without this overload, such a query would return a <b>successful</b> <c>Result&lt;Error&gt;</c>.
    ///     </para>
    /// </remarks>
    public static ValueTask<Result> SelectMany<T, TMiddle>(
        this ValueTask<Result<T>> result,
        Func<T, Result<TMiddle>> collectionSelector,
        Func<T, TMiddle, Error> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result> Core(ValueTask<Result<T>> result, Func<T, Result<TMiddle>> collectionSelector, Func<T, TMiddle, Error> resultSelector)
        {
            var resultValue = await result.ConfigureAwait(ResultConfiguration.ContinueOnCapturedContext);

            if (resultValue is null)
                throw Returned.NullAwaited(nameof(result));

            return resultValue.SelectMany(collectionSelector, resultSelector);
        }
    }

    /// <summary>
    ///     Applies the asynchronous <paramref name="collectionSelector" /> and the <paramref name="resultSelector" /> to the value of a successful
    ///     <paramref name="result" /> and returns a failed <see cref="Result" /> with the <see cref="Error" /> returned by <paramref name="resultSelector" />;
    ///     otherwise, returns a failed <see cref="Result" /> with the first <see cref="Error" />.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the value of the initial result. If successful, this is also the type of the parameter passed to the selectors.
    /// </typeparam>
    /// <typeparam name="TMiddle">The type of the value of the intermediate result returned by <paramref name="collectionSelector" />.</typeparam>
    /// <param name="result">The initial result.</param>
    /// <param name="collectionSelector">
    ///     An asynchronous function that takes the value of the initial result and returns a result with an intermediate value.
    /// </param>
    /// <param name="resultSelector">
    ///     A function that takes the value of the initial result and the intermediate value, and returns the <see cref="Error" /> of the failed result.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{TResult}" /> that represents a failed <see cref="Result" /> with the <see cref="Error" /> returned by
    ///     <paramref name="resultSelector" /> if both the initial and the intermediate results are successful;
    ///     otherwise, a failed <see cref="Result" /> with the first <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="collectionSelector" /> or <paramref name="resultSelector" /> parameters are <see
    ///     langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="collectionSelector" /> returns a value task with a <see langword="null" /> result,
    ///     or <paramref name="resultSelector" /> returns <see langword="null" />.
    /// </exception>
    /// <remarks>
    ///     <para>
    ///         This method makes a query with more than one <c>from</c> clause that selects an <see cref="Error" />,
    ///         e.g., <c>from user in GetUser(id) from order in GetOrderAsync(user) select Error.Conflict(…)</c>, return a failed <see cref="Result" />.
    ///     </para>
    ///     <para>
    ///         Without this overload, such a query would return a <b>successful</b> <c>Result&lt;Error&gt;</c>.
    ///     </para>
    /// </remarks>
    public static ValueTask<Result> SelectMany<T, TMiddle>(
        this Result<T> result,
        Func<T, ValueTask<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, Error> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        return Core(result, collectionSelector, resultSelector);

        static async ValueTask<Result> Core(Result<T> result, Func<T, ValueTask<Result<TMiddle>>> collectionSelector, Func<T, TMiddle, Error> resultSelector)
        {
            if (!result.IsSuccess())
                return result.Error;

            var collectionResult = await Returned.NotNullResult(
                collectionSelector(result.Value), nameof(collectionSelector), ResultConfiguration.ContinueOnCapturedContext);
            return collectionResult.IfSuccess(y => Returned.NotNull(resultSelector(result.Value, y), nameof(resultSelector)));
        }
    }
}
