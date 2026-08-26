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
///     The collection of extension methods for executing actions or functions based on the state of
///     a <see cref="Result" /> instance.
/// </summary>
/// <remarks>
///     These methods enable fluent chaining of operations by specifying operations that will be executed depending on
///     the status of the <see cref="Result" /> instance. These helpers centralize success/error handling logic by
///     accepting delegates for each branch, enabling concise match-like patterns across synchronous result workflows.
/// </remarks>
public static class MatchExtensions
{
    #region Result

    /// <summary>
    ///     Invokes one of the provided actions depending on whether the <paramref name="result" /> is successful.
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
    ///     The action to execute if the <paramref name="result" /> indicates success.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The action to execute if the <paramref name="result" /> indicates failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this action.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <returns>The original <paramref name="result" />, enabling fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessAction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    public static Result Match(this Result result, Action ifSuccessAction, Action<Error> ifErrorAction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsSuccess())
            ifSuccessAction();
        else
            ifErrorAction(result.Error);

        return result;
    }

    /// <summary>
    ///     Invokes the provided function and returns its <see cref="Result" /> output
    ///     if the <paramref name="result" /> is successful, or the provided action and
    ///     returns <see cref="Result" /> with the original error.
    /// </summary>
    /// <param name="result">The <see cref="Result" /> whose outcome is inspected. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The function invoked when the <paramref name="result" /> represents success.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The action invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this function.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Result" /> returned by either <paramref name="ifSuccessFunction" />
    ///     if <paramref name="result" /> is successful, or the original <paramref name="result" /> after invoking
    ///     <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    public static Result Match(this Result result, Func<Result> ifSuccessFunction,
        Action<Error> ifErrorAction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsSuccess())
            return ifSuccessFunction();

        ifErrorAction(result.Error);
        return result;
    }

    /// <summary>
    ///     Invokes one of the provided functions depending on whether the <paramref name="result" /> is successful and
    ///     returns the produced <see cref="Result" />.
    /// </summary>
    /// <param name="result">The <see cref="Result" /> whose outcome is inspected. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunction">
    ///     The function invoked when the <paramref name="result" /> represents success.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The function invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this function.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Result" /> returned by either <paramref name="ifSuccessFunction" /> or
    ///     <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    public static Result Match(this Result result, Func<Result> ifSuccessFunction,
        Func<Error, Result> ifErrorFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return result.IsSuccess()
            ? ifSuccessFunction()
            : ifErrorFunction(result.Error);
    }

    /// <summary>
    ///     Invokes the provided function and returns its value as a <see cref="Result{T}" /> instance
    ///     if the <paramref name="result" /> is successful, or the provided action and
    ///     returns <see cref="Result{T}" /> with the original error.
    /// </summary>
    /// <typeparam name="T">The return type produced by the successful branch.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result" /> whose outcome dictates which function is invoked.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The function invoked when the <paramref name="result" /> represents success.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The action invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this action.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Result{T}" /> with the value returned by either the <paramref name="ifSuccessFunction" />
    ///     if <paramref name="result" /> is successful, or the <see cref="Result{T}" /> with
    ///     the original <paramref name="result" /> after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    public static Result<T> Match<T>(this Result result, Func<T> ifSuccessFunction, Action<Error> ifErrorAction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsSuccess())
            return ifSuccessFunction();

        ifErrorAction(result.Error);
        return result.Error;
    }

    /// <summary>
    ///     Invokes one of the provided functions depending on whether the <paramref name="result" /> is successful and
    ///     returns their value as a <see cref="Result{T}" /> instance.
    /// </summary>
    /// <typeparam name="T">The return type produced by either branch.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result" /> whose outcome dictates which function is invoked.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The function invoked when the <paramref name="result" /> represents success.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The function invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this function.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     The value of the <typeparamref name="T"/> produced by either <paramref name="ifSuccessFunction" />
    ///     or <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    public static T Match<T>(this Result result, Func<T> ifSuccessFunction, Func<Error, T> ifErrorFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return result.IsSuccess()
            ? ifSuccessFunction()
            : ifErrorFunction(result.Error);
    }

    /// <summary>
    ///     Invokes the provided function and returns its <see cref="Result{T}" /> output
    ///     if the <paramref name="result" /> is successful, or the provided action and
    ///     returns <see cref="Result{T}" /> with the original error.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Result{T}" /> returned by the matching branch.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result" /> whose state determines which function is invoked.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The function invoked when the <paramref name="result" /> represents success.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The action invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this function.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Result{T}" /> returned by the <paramref name="ifSuccessFunction" />
    ///     if <paramref name="result" /> is successful, or the <see cref="Result{T}" /> with
    ///     the original <paramref name="result" /> after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    public static Result<T> Match<T>(this Result result, Func<Result<T>> ifSuccessFunction,
        Action<Error> ifErrorAction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsSuccess())
            return ifSuccessFunction();

        ifErrorAction(result.Error);
        return result.Error;
    }

    /// <summary>
    ///     Invokes one of the provided functions depending on whether the <paramref name="result" /> is successful and
    ///     returns their output as a <see cref="Result{T}" /> instance.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Result{T}" /> returned by the matching branch.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result" /> whose state determines which function is invoked.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The function invoked when the <paramref name="result" /> represents success.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The function invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this function.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Result{T}" /> produced by either <paramref name="ifSuccessFunction" /> or
    ///     <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    public static Result<T> Match<T>(this Result result, Func<Result<T>> ifSuccessFunction,
        Func<Error, Result<T>> ifErrorFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return result.IsSuccess()
            ? ifSuccessFunction()
            : ifErrorFunction(result.Error);
    }

    #endregion

    #region Result<T>

    /// <summary>
    ///     Invokes one of the provided actions depending on whether the <paramref name="result" /> is successful.
    /// </summary>
    /// <remarks>
    ///     Use this overload when the desired outcome is a side effect, and you want to keep
    ///     the existing <see cref="Result{T}" /> instance for further chaining.
    /// </remarks>
    /// <typeparam name="T">The type wrapped by the <see cref="Result{T}" />.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result{T}" /> whose outcome determines which action is invoked.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessAction">
    ///     The action to execute when the <paramref name="result" /> represents success.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The action to execute when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this action.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <returns>The original <paramref name="result" /> for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessAction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    public static Result<T> Match<T>(this Result<T> result, Action<T> ifSuccessAction, Action<Error> ifErrorAction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsSuccess())
            ifSuccessAction(result.Value);
        else
            ifErrorAction(result.Error);

        return result;
    }

    /// <summary>
    ///     Invokes the provided function and returns its value as a <see cref="Result{TNext}" /> instance
    ///     if the <paramref name="result" /> is successful, or the provided action and
    ///     returns <see cref="Result{TNext}" /> with the original error.
    /// </summary>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNext">The type produced by either branch.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result{T}" /> whose outcome selects the function to invoke.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The function executed when the <paramref name="result" /> represents success.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The action invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this action.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Result{TNext}" /> with the value returned by <paramref name="ifSuccessFunction" />
    ///     if <paramref name="result" /> is successful, or the <see cref="Result{TNext}" /> with
    ///     the original <paramref name="result" /> after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    public static Result<TNext> Match<T, TNext>(this Result<T> result, Func<T, TNext> ifSuccessFunction,
        Action<Error> ifErrorAction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsSuccess())
            return ifSuccessFunction(result.Value);

        ifErrorAction(result.Error);
        return result.Error;
    }

    /// <summary>
    ///     Invokes one of the provided functions depending on whether the <paramref name="result" /> is successful and
    ///     returns their output as a <see cref="Result{TNext}" /> instance.
    /// </summary>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNext">The type produced by either branch.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result{T}" /> whose outcome selects the function to invoke.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The function executed when the <paramref name="result" /> represents success.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The function executed when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this function.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     The value of the <typeparamref name="TNext"/> type returned by
    ///     either <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    public static TNext Match<T, TNext>(this Result<T> result, Func<T, TNext> ifSuccessFunction,
        Func<Error, TNext> ifErrorFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return result.IsSuccess()
            ? ifSuccessFunction(result.Value)
            : ifErrorFunction(result.Error);
    }

    /// <summary>
    ///     Invokes the provided function and returns its <see cref="Result{TNext}" /> output
    ///     if the <paramref name="result" /> is successful, or the provided action and
    ///     returns <see cref="Result{TNext}" /> with the original error.
    /// </summary>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNext">The type of the <see cref="Result{TNext}" /> returned by the invoked function.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result{T}" /> whose outcome selects the function to invoke.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The function executed when the <paramref name="result" /> represents success.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The action invoked when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this action.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Result{TNext}" /> returned by <paramref name="ifSuccessFunction" />
    ///     if the <paramref name="result" /> is successful, or the <see cref="Result{TNext}" /> with
    ///     the original <paramref name="result" /> after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    public static Result<TNext> Match<T, TNext>(this Result<T> result, Func<T, Result<TNext>> ifSuccessFunction,
        Action<Error> ifErrorAction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsSuccess())
            return ifSuccessFunction(result.Value);

        ifErrorAction(result.Error);
        return result.Error;
    }

    /// <summary>
    ///     Executes one of the provided functions depending on whether the <paramref name="result" /> is successful and
    ///     returns their output as a <see cref="Result{TNext}" /> instance.
    /// </summary>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNext">The type of the <see cref="Result{TNext}" /> returned by the invoked function.</typeparam>
    /// <param name="result">
    ///     The <see cref="Result{T}" /> whose outcome selects the function to invoke.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The function executed when the <paramref name="result" /> represents success.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The function executed when the <paramref name="result" /> represents failure.
    ///     The <see cref="Error" /> associated with the <paramref name="result" /> is passed to this function.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     The <see cref="Result{TNext}" /> returned by either <paramref name="ifSuccessFunction" /> or
    ///     <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" />, <paramref name="ifSuccessFunction" />,
    ///     or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    public static Result<TNext> Match<T, TNext>(this Result<T> result, Func<T, Result<TNext>> ifSuccessFunction,
        Func<Error, Result<TNext>> ifErrorFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return result.IsSuccess()
            ? ifSuccessFunction(result.Value)
            : ifErrorFunction(result.Error);
    }

    #endregion
}
