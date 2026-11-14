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
///     The collection of extension methods for executing asynchronous actions or functions based on the state of a <see cref="Result" /> instance.
/// </summary>
/// <remarks>
///     These methods enable fluent chaining of operations by specifying operations that will be executed depending on the status of the <see cref="Result" />
///     instance. These helpers centralize success/error handling logic by accepting delegates for each branch,
///     enabling concise match-like patterns across synchronous result workflows.
///     <para>
///         These methods are declared in the <c>Maple.Result.Extensions.ValueTasks</c> namespace, separately from their
///         <see cref="System.Threading.Tasks.Task" />-based counterparts in <c>Maple.Result.Extensions</c>, because both sets declare the same method names and
///         an asynchronous lambda is convertible to a <c>Func&lt;Task&gt;</c> as well as to a <c>Func&lt;ValueTask&gt;</c> delegate.
///         Importing only one of these namespaces keeps such calls unambiguous.
///     </para>
/// </remarks>
public static class MatchValueTaskAsyncExtensions
{
    #region Result

    /// <returns>
    ///     A <see cref="ValueTask{Result}" /> that represents the original <paramref name="result" />, enabling fluent chaining.
    /// </returns>
    /// <inheritdoc cref="MatchTaskAsyncExtensions.MatchAsync(Result, Func{Task}, Func{Error, Task}, bool?)" />
    public static ValueTask<Result> MatchAsync(this Result result, Func<ValueTask> ifSuccessAction,
        Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(result, ifSuccessAction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result> Core(
            Result result,
            Func<ValueTask> ifSuccessAction,
            Func<Error, ValueTask> ifErrorAction,
            bool continueOnCapturedContext)
        {
            if (result.IsSuccess())
                await ifSuccessAction().ConfigureAwait(continueOnCapturedContext);
            else
                await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);

            return result;
        }
    }

    /// <returns>
    ///     The <see cref="ValueTask{Result}" /> that represents the <see cref="Result" /> returned by either <paramref name="ifSuccessFunction" />
    ///     if <paramref name="result" /> is successful, or the original <paramref name="result" /> after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    /// <inheritdoc cref="MatchTaskAsyncExtensions.MatchAsync(Result, Func{Task{Result}}, Func{Error, Task}, bool?)" />
    public static ValueTask<Result> MatchAsync(this Result result, Func<ValueTask<Result>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(result, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result> Core(
            Result result,
            Func<ValueTask<Result>> ifSuccessFunction,
            Func<Error, ValueTask> ifErrorAction,
            bool continueOnCapturedContext)
        {
            if (result.IsSuccess())
                return await Returned.NotNullResult(ifSuccessFunction(), nameof(ifSuccessFunction), continueOnCapturedContext);

            await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
            return result;
        }
    }

    /// <returns>
    ///     The <see cref="ValueTask{Result}" /> which represents the <see cref="Result" /> returned by either <paramref name="ifSuccessFunction" /> or
    ///     <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a value task with a <see langword="null" /> result,
    ///     or <paramref name="ifErrorFunction" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    /// <inheritdoc cref="MatchTaskAsyncExtensions.MatchAsync(Result, Func{Task{Result}}, Func{Error, Task{Result}}, bool?)" />
    public static ValueTask<Result> MatchAsync(this Result result, Func<ValueTask<Result>> ifSuccessFunction,
        Func<Error, ValueTask<Result>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(result, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result> Core(
            Result result,
            Func<ValueTask<Result>> ifSuccessFunction,
            Func<Error, ValueTask<Result>> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            return result.IsSuccess()
                ? await Returned.NotNullResult(ifSuccessFunction(), nameof(ifSuccessFunction), continueOnCapturedContext)
                : await Returned.NotNullResult(ifErrorFunction(result.Error), nameof(ifErrorFunction), continueOnCapturedContext);
        }
    }

    /// <returns>
    ///     The <see cref="ValueTask{Result}" /> that represents the <see cref="Result{T}" /> with the value returned by either
    ///     the <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is successful,
    ///     or the <see cref="Result{T}" /> with the original <paramref name="result" /> after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <inheritdoc cref="MatchTaskAsyncExtensions.MatchAsync{T}(Result, Func{Task{T}}, Func{Error, Task}, bool?)" />
    public static ValueTask<Result<T>> MatchAsync<T>(this Result result, Func<ValueTask<T>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(result, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result<T>> Core(
            Result result,
            Func<ValueTask<T>> ifSuccessFunction,
            Func<Error, ValueTask> ifErrorAction,
            bool continueOnCapturedContext)
        {
            if (result.IsSuccess())
                return await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext);

            await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
            return result.Error;
        }
    }

    /// <returns>
    ///     A <see cref="ValueTask{Result}" /> that represents a failed <see cref="Result" /> with the <see cref="Error" /> returned by
    ///     <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is successful;
    ///     otherwise, the original <paramref name="result" /> after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    /// <inheritdoc cref="MatchTaskAsyncExtensions.MatchAsync(Result, Func{Task{Error}}, Func{Error, Task}, bool?)" />
    public static ValueTask<Result> MatchAsync(this Result result, Func<ValueTask<Error>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(result, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result> Core(
            Result result,
            Func<ValueTask<Error>> ifSuccessFunction,
            Func<Error, ValueTask> ifErrorAction,
            bool continueOnCapturedContext)
        {
            if (result.IsSuccess())
                return Result.FromError(await Returned.NotNullResult(ifSuccessFunction(), nameof(ifSuccessFunction), continueOnCapturedContext));

            await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
            return result;
        }
    }

    /// <returns>
    ///     The <see cref="ValueTask{T}" /> that represents the value of type<typeparamref name="T" /> produced by either <paramref name="ifSuccessFunction" />
    ///     or <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <inheritdoc cref="MatchTaskAsyncExtensions.MatchAsync{T}(Result, Func{Task{T}}, Func{Error, Task{T}}, bool?)" />
    public static ValueTask<T> MatchAsync<T>(this Result result, Func<ValueTask<T>> ifSuccessFunction,
        Func<Error, ValueTask<T>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(result, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<T> Core(
            Result result,
            Func<ValueTask<T>> ifSuccessFunction,
            Func<Error, ValueTask<T>> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            return result.IsSuccess()
                ? await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext)
                : await ifErrorFunction(result.Error).ConfigureAwait(continueOnCapturedContext);
        }
    }

    /// <returns>
    ///     The <see cref="ValueTask{TResult}" /> that represents the <see cref="Result{T}" /> returned by the <paramref name="ifSuccessFunction" />
    ///     if <paramref name="result" /> is successful,
    ///     or the <see cref="Result{T}" /> with the original <paramref name="result" /> after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    /// <inheritdoc cref="MatchTaskAsyncExtensions.MatchAsync{T}(Result, Func{Task{Result{T}}}, Func{Error, Task}, bool?)" />
    public static ValueTask<Result<T>> MatchAsync<T>(this Result result, Func<ValueTask<Result<T>>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(result, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result<T>> Core(
            Result result,
            Func<ValueTask<Result<T>>> ifSuccessFunction,
            Func<Error, ValueTask> ifErrorAction,
            bool continueOnCapturedContext)
        {
            if (result.IsSuccess())
                return await Returned.NotNullResult(ifSuccessFunction(), nameof(ifSuccessFunction), continueOnCapturedContext);

            await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
            return result.Error;
        }
    }

    /// <returns>
    ///     The <see cref="ValueTask{TResult}" /> that represents the <see cref="Result{T}" /> produced by either <paramref name="ifSuccessFunction" /> or
    ///     <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a value task with a <see langword="null" /> result,
    ///     or <paramref name="ifErrorFunction" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    /// <inheritdoc cref="MatchTaskAsyncExtensions.MatchAsync{T}(Result, Func{Task{Result{T}}}, Func{Error, Task{Result{T}}}, bool?)" />
    public static ValueTask<Result<T>> MatchAsync<T>(this Result result, Func<ValueTask<Result<T>>> ifSuccessFunction,
        Func<Error, ValueTask<Result<T>>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(result, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result<T>> Core(
            Result result,
            Func<ValueTask<Result<T>>> ifSuccessFunction,
            Func<Error, ValueTask<Result<T>>> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            return result.IsSuccess()
                ? await Returned.NotNullResult(ifSuccessFunction(), nameof(ifSuccessFunction), continueOnCapturedContext)
                : await Returned.NotNullResult(ifErrorFunction(result.Error), nameof(ifErrorFunction), continueOnCapturedContext);
        }
    }

    #endregion

    #region Result<T>

    /// <returns>
    ///     The <see cref="ValueTask{TResult}" /> that represents the original <paramref name="result" /> for fluent chaining.
    /// </returns>
    /// <inheritdoc cref="MatchTaskAsyncExtensions.MatchAsync{T}(Result{T}, Func{T, Task}, Func{Error, Task}, bool?)" />
    public static ValueTask<Result<T>> MatchAsync<T>(this Result<T> result, Func<T, ValueTask> ifSuccessAction,
        Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(result, ifSuccessAction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result<T>> Core(
            Result<T> result,
            Func<T, ValueTask> ifSuccessAction,
            Func<Error, ValueTask> ifErrorAction,
            bool continueOnCapturedContext)
        {
            if (result.IsSuccess())
                await ifSuccessAction(result.Value).ConfigureAwait(continueOnCapturedContext);
            else
                await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);

            return result;
        }
    }

    /// <returns>
    ///     The <see cref="ValueTask{TResult}" /> that represents the <see cref="Result{TNext}" /> with the value returned by
    ///     <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is successful,
    ///     or the <see cref="Result{TNext}" /> with the original <paramref name="result" /> after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <inheritdoc cref="MatchTaskAsyncExtensions.MatchAsync{T, TNext}(Result{T}, Func{T, Task{TNext}}, Func{Error, Task}, bool?)" />
    public static ValueTask<Result<TNext>> MatchAsync<T, TNext>(
        this Result<T> result,
        Func<T, ValueTask<TNext>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(result, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result<TNext>> Core(
            Result<T> result,
            Func<T, ValueTask<TNext>> ifSuccessFunction,
            Func<Error, ValueTask> ifErrorAction,
            bool continueOnCapturedContext)
        {
            if (result.IsSuccess())
                return await ifSuccessFunction(result.Value).ConfigureAwait(continueOnCapturedContext);

            await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
            return result.Error;
        }
    }

    /// <returns>
    ///     A <see cref="ValueTask{Result}" /> that represents a failed <see cref="Result" /> with the <see cref="Error" /> returned by
    ///     <paramref name="ifSuccessFunction" /> if <paramref name="result" /> is successful;
    ///     otherwise, a failed <see cref="Result" /> with the original <see cref="Error" /> after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    /// <inheritdoc cref="MatchTaskAsyncExtensions.MatchAsync{T}(Result{T}, Func{T, Task{Error}}, Func{Error, Task}, bool?)" />
    public static ValueTask<Result> MatchAsync<T>(
        this Result<T> result,
        Func<T, ValueTask<Error>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(result, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result> Core(
            Result<T> result,
            Func<T, ValueTask<Error>> ifSuccessFunction,
            Func<Error, ValueTask> ifErrorAction,
            bool continueOnCapturedContext)
        {
            if (result.IsSuccess())
            {
                return Result.FromError(await Returned.NotNullResult(ifSuccessFunction(result.Value), nameof(ifSuccessFunction), continueOnCapturedContext));
            }

            await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
            return result.Error;
        }
    }

    /// <returns>
    ///     The <see cref="ValueTask{TNext}" /> that represents the value of the <typeparamref name="TNext" /> type returned by either
    ///     <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <inheritdoc cref="MatchTaskAsyncExtensions.MatchAsync{T, TNext}(Result{T}, Func{T, Task{TNext}}, Func{Error, Task{TNext}}, bool?)" />
    public static ValueTask<TNext> MatchAsync<T, TNext>(
        this Result<T> result,
        Func<T, ValueTask<TNext>> ifSuccessFunction,
        Func<Error, ValueTask<TNext>> ifErrorFunction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(result, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<TNext> Core(
            Result<T> result,
            Func<T, ValueTask<TNext>> ifSuccessFunction,
            Func<Error, ValueTask<TNext>> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            return result.IsSuccess()
                ? await ifSuccessFunction(result.Value).ConfigureAwait(continueOnCapturedContext)
                : await ifErrorFunction(result.Error).ConfigureAwait(continueOnCapturedContext);
        }
    }

    /// <returns>
    ///     The <see cref="ValueTask{TResult}" /> that represents the <see cref="Result{TNext}" /> returned by <paramref name="ifSuccessFunction" />
    ///     if the <paramref name="result" /> is successful,
    ///     or the <see cref="Result{TNext}" /> with the original <paramref name="result" /> after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    /// <inheritdoc cref="MatchTaskAsyncExtensions.MatchAsync{T, TNext}(Result{T}, Func{T, Task{Result{TNext}}}, Func{Error, Task}, bool?)" />
    public static ValueTask<Result<TNext>> MatchAsync<T, TNext>(
        this Result<T> result,
        Func<T, ValueTask<Result<TNext>>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(result, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result<TNext>> Core(
            Result<T> result,
            Func<T, ValueTask<Result<TNext>>> ifSuccessFunction,
            Func<Error, ValueTask> ifErrorAction,
            bool continueOnCapturedContext)
        {
            if (result.IsSuccess())
                return await Returned.NotNullResult(ifSuccessFunction(result.Value), nameof(ifSuccessFunction), continueOnCapturedContext);

            await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
            return result.Error;
        }
    }

    /// <returns>
    ///     The <see cref="ValueTask{TResult}" /> that represents the <see cref="Result{TNext}" /> returned by either <paramref name="ifSuccessFunction" /> or
    ///     <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     If <paramref name="ifSuccessFunction" /> returns a value task with a <see langword="null" /> result,
    ///     or <paramref name="ifErrorFunction" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    /// <inheritdoc cref="MatchTaskAsyncExtensions.MatchAsync{T, TNext}(Result{T}, Func{T, Task{Result{TNext}}}, Func{Error, Task{Result{TNext}}}, bool?)" />
    public static ValueTask<Result<TNext>> MatchAsync<T, TNext>(
        this Result<T> result,
        Func<T, ValueTask<Result<TNext>>> ifSuccessFunction,
        Func<Error, ValueTask<Result<TNext>>> ifErrorFunction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(result, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result<TNext>> Core(
            Result<T> result,
            Func<T, ValueTask<Result<TNext>>> ifSuccessFunction,
            Func<Error, ValueTask<Result<TNext>>> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            return result.IsSuccess()
                ? await Returned.NotNullResult(ifSuccessFunction(result.Value), nameof(ifSuccessFunction), continueOnCapturedContext)
                : await Returned.NotNullResult(ifErrorFunction(result.Error), nameof(ifErrorFunction), continueOnCapturedContext);
        }
    }

    #endregion

    #region ValueTask<Result>

    /// <summary>
    ///     Invokes one of the provided asynchronous actions depending on whether the <paramref name="resultTask" /> represents a successful operation.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Use this overload when the desired outcome is a side effect,
    ///         and you want to keep the existing <see cref="Result" /> instance for further chaining.
    ///     </para>
    ///     <para>
    ///         The method executes passed <paramref name="resultTask" /> asynchronous operation, does not modify its result.
    ///     </para>
    /// </remarks>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected.
    /// </param>
    /// <param name="ifSuccessAction">
    ///     The asynchronous action to execute if the <paramref name="resultTask" /> indicates a successful operation. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action to execute if the <paramref name="resultTask" /> indicates a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> outcome is passed to this action. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{Result}" /> that represents the original <paramref name="resultTask" />, enabling fluent chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="ifSuccessAction" />, or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask, Func<ValueTask> ifSuccessAction,
        Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessAction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result> Core(
            ValueTask<Result> resultTask,
            Func<ValueTask> ifSuccessAction,
            Func<Error, ValueTask> ifErrorAction,
            bool continueOnCapturedContext)
        {
            var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

            if (result is null)
                throw Returned.NullAwaited(nameof(resultTask));

            if (result.IsSuccess())
                await ifSuccessAction().ConfigureAwait(continueOnCapturedContext);
            else
                await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);

            return result;
        }
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its <see cref="Result" /> output as an asynchronous operation,
    ///     if the <paramref name="resultTask" /> represents a successful operation,
    ///     or the provided asynchronous action and returns <see cref="Result" /> with the original error as an asynchronous operation.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation, does not modify its result.
    /// </remarks>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="resultTask" /> represents a successful operation. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="resultTask" /> represents a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> outcome is passed to this function. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="ValueTask{Result}" /> that represents the <see cref="Result" /> returned by either <paramref name="ifSuccessFunction" />
    ///     if <paramref name="resultTask" /> is a successful operation, or the original <see cref="Result" />,
    ///     the outcome of the <paramref name="resultTask" /> operation, after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="ifSuccessFunction" />, or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     or <paramref name="ifSuccessFunction" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    public static ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask, Func<ValueTask<Result>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result> Core(
            ValueTask<Result> resultTask,
            Func<ValueTask<Result>> ifSuccessFunction,
            Func<Error, ValueTask> ifErrorAction,
            bool continueOnCapturedContext)
        {
            var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

            if (result is null)
                throw Returned.NullAwaited(nameof(resultTask));

            if (result.IsSuccess())
                return await Returned.NotNullResult(ifSuccessFunction(), nameof(ifSuccessFunction), continueOnCapturedContext);

            await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
            return result;
        }
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="resultTask" /> is a successful operation and
    ///     returns the produced <see cref="Result" />.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked if the <paramref name="resultTask" /> represents a successful operation. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function invoked if the <paramref name="resultTask" /> represents a failed operation. The <see cref="Error" /> associated with
    ///     the <paramref name="resultTask" /> operation outcome is passed to this function. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="ValueTask{Result}" /> which represents the <see cref="Result" /> returned by either <paramref name="ifSuccessFunction" /> or
    ///     <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="ifSuccessFunction" />, or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     <paramref name="ifSuccessFunction" /> returns a value task with a <see langword="null" /> result,
    ///     or <paramref name="ifErrorFunction" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    public static ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask, Func<ValueTask<Result>> ifSuccessFunction,
        Func<Error, ValueTask<Result>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result> Core(
            ValueTask<Result> resultTask,
            Func<ValueTask<Result>> ifSuccessFunction,
            Func<Error, ValueTask<Result>> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

            if (result is null)
                throw Returned.NullAwaited(nameof(resultTask));

            return result.IsSuccess()
                ? await Returned.NotNullResult(ifSuccessFunction(), nameof(ifSuccessFunction), continueOnCapturedContext)
                : await Returned.NotNullResult(ifErrorFunction(result.Error), nameof(ifErrorFunction), continueOnCapturedContext);
        }
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its value as an asynchronous operation that returns a <see cref="Result{T}" /> instance
    ///     if the <paramref name="resultTask" /> is a successful operation,
    ///     or the provided asynchronous action and returns <see cref="Result{T}" /> with the original error as an asynchronous operation.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <typeparam name="T">The return type produced by the successful branch.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="resultTask" /> represents a successful operation. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="resultTask" /> represents a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> outcome is passed to this action. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="ValueTask{Result}" /> that represents the <see cref="Result{T}" /> with the value returned by either
    ///     the <paramref name="ifSuccessFunction" /> if <paramref name="resultTask" /> is a successful operation,
    ///     or the original <see cref="Result{T}" /> after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="ifSuccessFunction" />, or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result<T>> MatchAsync<T>(this ValueTask<Result> resultTask, Func<ValueTask<T>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result<T>> Core(
            ValueTask<Result> resultTask,
            Func<ValueTask<T>> ifSuccessFunction,
            Func<Error, ValueTask> ifErrorAction,
            bool continueOnCapturedContext)
        {
            var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

            if (result is null)
                throw Returned.NullAwaited(nameof(resultTask));

            if (result.IsSuccess())
                return await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext);

            await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
            return result.Error;
        }
    }

    /// <summary>
    ///     Invokes the provided asynchronous function if the <paramref name="resultTask" /> is a successful operation and returns a failed
    ///     <see cref="Result" /> with the <see cref="Error" /> it returns; otherwise, invokes the provided asynchronous action and returns
    ///     the original result.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Use it to turn a successful result into a failure, e.g.,
    ///         <c>resultTask.MatchAsync(async () => Error.Conflict(…), e => LogAsync(e))</c>. The returned <see cref="Result" /> is always failed.
    ///     </para>
    ///     <para>
    ///         Without this overload, such a call would return a <b>successful</b> <c>Result&lt;Error&gt;</c>.
    ///         To get that on purpose, specify the type argument: <c>MatchAsync&lt;Error&gt;(…)</c>.
    ///     </para>
    ///     <para>
    ///         The method executes passed <paramref name="resultTask" /> asynchronous operation.
    ///     </para>
    /// </remarks>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="resultTask" /> represents a successful operation.
    ///     It returns the <see cref="Error" /> of the failed <see cref="Result" />. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="resultTask" /> represents a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> outcome is passed to this action. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{Result}" /> that represents a failed <see cref="Result" /> with the <see cref="Error" /> returned by
    ///     <paramref name="ifSuccessFunction" /> if the <paramref name="resultTask" /> operation is successful;
    ///     otherwise, the original result after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     or <paramref name="ifSuccessFunction" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    public static ValueTask<Result> MatchAsync(this ValueTask<Result> resultTask, Func<ValueTask<Error>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result> Core(
            ValueTask<Result> resultTask,
            Func<ValueTask<Error>> ifSuccessFunction,
            Func<Error, ValueTask> ifErrorAction,
            bool continueOnCapturedContext)
        {
            var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

            if (result is null)
                throw Returned.NullAwaited(nameof(resultTask));

            if (result.IsSuccess())
                return Result.FromError(await Returned.NotNullResult(ifSuccessFunction(), nameof(ifSuccessFunction), continueOnCapturedContext));

            await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
            return result;
        }
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="resultTask" /> is a successful operation and returns
    ///     their value as an asynchronous operation that returns a <see cref="Result{T}" /> instance.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <typeparam name="T">The return type produced by either branch.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="resultTask" /> represents a successful operation. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function invoked if the <paramref name="resultTask" /> represents a failed operation. The <see cref="Error" /> associated with
    ///     the <paramref name="resultTask" /> operation outcome is passed to this function. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="ValueTask{T}" /> that represents the value of type<typeparamref name="T" /> produced by either <paramref name="ifSuccessFunction" />
    ///     or <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="ifSuccessFunction" />, or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<T> MatchAsync<T>(this ValueTask<Result> resultTask, Func<ValueTask<T>> ifSuccessFunction,
        Func<Error, ValueTask<T>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<T> Core(
            ValueTask<Result> resultTask,
            Func<ValueTask<T>> ifSuccessFunction,
            Func<Error, ValueTask<T>> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

            if (result is null)
                throw Returned.NullAwaited(nameof(resultTask));

            return result.IsSuccess()
                ? await ifSuccessFunction().ConfigureAwait(continueOnCapturedContext)
                : await ifErrorFunction(result.Error).ConfigureAwait(continueOnCapturedContext);
        }
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its <see cref="Result{T}" /> output as an asynchronous operation
    ///     if the <paramref name="resultTask" /> is a successful operation,
    ///     or the provided asynchronous action and returns <see cref="Result{T}" /> with the original error as an asynchronous operation.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <typeparam name="T">The type of the <see cref="Result{T}" /> returned by the matching branch.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="resultTask" /> represents a successful operation. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="resultTask" /> represents a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> outcome is passed to this action. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="ValueTask{TResult}" /> that represents the <see cref="Result{T}" /> returned by the <paramref name="ifSuccessFunction" />
    ///     if <paramref name="resultTask" /> is a successful operation, or the <see cref="Result{T}" /> with the original outcome of
    ///     the <paramref name="resultTask" /> operation after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="ifSuccessFunction" />, or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     or <paramref name="ifSuccessFunction" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    public static ValueTask<Result<T>> MatchAsync<T>(
        this ValueTask<Result> resultTask, Func<ValueTask<Result<T>>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result<T>> Core(
            ValueTask<Result> resultTask,
            Func<ValueTask<Result<T>>> ifSuccessFunction,
            Func<Error, ValueTask> ifErrorAction,
            bool continueOnCapturedContext)
        {
            var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

            if (result is null)
                throw Returned.NullAwaited(nameof(resultTask));

            if (result.IsSuccess())
                return await Returned.NotNullResult(ifSuccessFunction(), nameof(ifSuccessFunction), continueOnCapturedContext);

            await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
            return result.Error;
        }
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="resultTask" /> is a successful operation and returns
    ///     their output as a <see cref="Result{T}" /> instance.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <typeparam name="T">The type of the <see cref="Result{T}" /> returned by the matching branch.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="resultTask" /> represents a successful operation. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function invoked if the <paramref name="resultTask" /> represents a failed operation. The <see cref="Error" /> associated with
    ///     the <paramref name="resultTask" /> operation outcome is passed to this function. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="ValueTask{TResult}" /> that represents the <see cref="Result{T}" /> produced by either <paramref name="ifSuccessFunction" /> or
    ///     <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="ifSuccessFunction" />, or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     <paramref name="ifSuccessFunction" /> returns a value task with a <see langword="null" /> result,
    ///     or <paramref name="ifErrorFunction" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    public static ValueTask<Result<T>> MatchAsync<T>(
        this ValueTask<Result> resultTask, Func<ValueTask<Result<T>>> ifSuccessFunction,
        Func<Error, ValueTask<Result<T>>> ifErrorFunction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result<T>> Core(
            ValueTask<Result> resultTask,
            Func<ValueTask<Result<T>>> ifSuccessFunction,
            Func<Error, ValueTask<Result<T>>> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

            if (result is null)
                throw Returned.NullAwaited(nameof(resultTask));

            return result.IsSuccess()
                ? await Returned.NotNullResult(ifSuccessFunction(), nameof(ifSuccessFunction), continueOnCapturedContext)
                : await Returned.NotNullResult(ifErrorFunction(result.Error), nameof(ifErrorFunction), continueOnCapturedContext);
        }
    }

    #endregion

    #region ValueTask<Result<T>>

    /// <summary>
    ///     Invokes one of the provided asynchronous actions depending on whether the <paramref name="resultTask" /> is a successful operation.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Use this overload when the desired outcome is a side effect,
    ///         and you want to keep the existing <see cref="Result{T}" /> instance for further chaining.
    ///     </para>
    ///     <para>
    ///         The method executes passed <paramref name="resultTask" /> asynchronous operation, does not modify its result.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type wrapped by the <see cref="Result{T}" />.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation, which returns <see cref="Result{T}" />,
    ///     to be executed and which result is to be inspected to select which function to invoke.
    /// </param>
    /// <param name="ifSuccessAction">
    ///     The asynchronous action to execute if the <paramref name="resultTask" /> indicates a successful operation.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action to execute if the <paramref name="resultTask" /> indicates a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> outcome is passed to this action. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="ValueTask{TResult}" /> that represents the outcome of the <paramref name="resultTask" /> for fluent chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="ifSuccessAction" />, or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result<T>> MatchAsync<T>(this ValueTask<Result<T>> resultTask, Func<T, ValueTask> ifSuccessAction,
        Func<Error, ValueTask> ifErrorAction, bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessAction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result<T>> Core(
            ValueTask<Result<T>> resultTask,
            Func<T, ValueTask> ifSuccessAction,
            Func<Error, ValueTask> ifErrorAction,
            bool continueOnCapturedContext)
        {
            var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

            if (result is null)
                throw Returned.NullAwaited(nameof(resultTask));

            if (result.IsSuccess())
                await ifSuccessAction(result.Value).ConfigureAwait(continueOnCapturedContext);
            else
                await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);

            return result;
        }
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its value as a <see cref="Result{TNext}" /> instance if the <paramref name="resultTask" /> is
    ///     a successful operation, or the provided asynchronous action and returns <see cref="Result{TNext}" /> with the original error.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNext">The type produced by either branch.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation, which returns <see cref="Result{T}" />,
    ///     to be executed and which result is to be inspected to select which function to invoke.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="resultTask" /> represents a successful operation.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="resultTask" /> represents a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> outcome is passed to this function. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="ValueTask{TResult}" /> that represents the <see cref="Result{TNext}" /> with the value returned by
    ///     <paramref name="ifSuccessFunction" /> if <paramref name="resultTask" /> is a successful operation, or the <see cref="Result{TNext}" /> with
    ///     the outcome of the original <paramref name="resultTask" /> after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="ifSuccessFunction" />, or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<Result<TNext>> MatchAsync<T, TNext>(
        this ValueTask<Result<T>> resultTask,
        Func<T, ValueTask<TNext>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result<TNext>> Core(
            ValueTask<Result<T>> resultTask,
            Func<T, ValueTask<TNext>> ifSuccessFunction,
            Func<Error, ValueTask> ifErrorAction,
            bool continueOnCapturedContext)
        {
            var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

            if (result is null)
                throw Returned.NullAwaited(nameof(resultTask));

            if (result.IsSuccess())
                return await ifSuccessFunction(result.Value).ConfigureAwait(continueOnCapturedContext);

            await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
            return result.Error;
        }
    }

    /// <summary>
    ///     Invokes the provided asynchronous function if the <paramref name="resultTask" /> is a successful operation and returns a failed
    ///     <see cref="Result" /> with the <see cref="Error" /> it returns; otherwise, invokes the provided asynchronous action and returns
    ///     a failed <see cref="Result" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Use it to turn a successful result into a failure, e.g.,
    ///         <c>resultTask.MatchAsync(async user => Error.Conflict(…), e => LogAsync(e))</c>. The returned <see cref="Result" /> is always failed.
    ///     </para>
    ///     <para>
    ///         Without this overload, such a call would return a <b>successful</b> <c>Result&lt;Error&gt;</c>.
    ///         To get that on purpose, specify the type arguments: <c>MatchAsync&lt;T, Error&gt;(…)</c>.
    ///     </para>
    ///     <para>
    ///         The method executes passed <paramref name="resultTask" /> asynchronous operation.
    ///     </para>
    /// </remarks>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="resultTask" /> represents a successful operation. The value of
    ///     the successful <see cref="Result{T}" /> is passed as a parameter. It returns the <see cref="Error" /> of the failed <see cref="Result" />.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="resultTask" /> represents a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> outcome is passed to this action. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     A <see cref="ValueTask{Result}" /> that represents a failed <see cref="Result" /> with the <see cref="Error" /> returned by
    ///     <paramref name="ifSuccessFunction" /> if the <paramref name="resultTask" /> operation is successful;
    ///     otherwise, a failed <see cref="Result" /> with the original <see cref="Error" /> after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     or <paramref name="ifSuccessFunction" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    public static ValueTask<Result> MatchAsync<T>(
        this ValueTask<Result<T>> resultTask,
        Func<T, ValueTask<Error>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result> Core(
            ValueTask<Result<T>> resultTask,
            Func<T, ValueTask<Error>> ifSuccessFunction,
            Func<Error, ValueTask> ifErrorAction,
            bool continueOnCapturedContext)
        {
            var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

            if (result is null)
                throw Returned.NullAwaited(nameof(resultTask));

            if (result.IsSuccess())
            {
                return Result.FromError(await Returned.NotNullResult(ifSuccessFunction(result.Value), nameof(ifSuccessFunction), continueOnCapturedContext));
            }

            await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
            return result.Error;
        }
    }

    /// <summary>
    ///     Invokes one of the provided asynchronous functions depending on whether the <paramref name="resultTask" /> is a successful operation and returns
    ///     their output as an asynchronous operation that represents a <see cref="Result{TNext}" /> instance.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNext">The type produced by either branch.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation, which returns <see cref="Result{T}" />,
    ///     to be executed and which result is to be inspected to select which function to invoke.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked if the <paramref name="resultTask" /> represents a successful operation.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous function invoked if the <paramref name="resultTask" /> represents a failed operation. The <see cref="Error" /> associated with
    ///     the <paramref name="resultTask" /> operation outcome is passed to this function. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="ValueTask{TNext}" /> that represents the value of the <typeparamref name="TNext" /> type returned by either
    ///     <paramref name="ifSuccessFunction" /> or <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="ifSuccessFunction" />, or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />.
    /// </exception>
    public static ValueTask<TNext> MatchAsync<T, TNext>(
        this ValueTask<Result<T>> resultTask,
        Func<T, ValueTask<TNext>> ifSuccessFunction,
        Func<Error, ValueTask<TNext>> ifErrorFunction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<TNext> Core(
            ValueTask<Result<T>> resultTask,
            Func<T, ValueTask<TNext>> ifSuccessFunction,
            Func<Error, ValueTask<TNext>> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

            if (result is null)
                throw Returned.NullAwaited(nameof(resultTask));

            return result.IsSuccess()
                ? await ifSuccessFunction(result.Value).ConfigureAwait(continueOnCapturedContext)
                : await ifErrorFunction(result.Error).ConfigureAwait(continueOnCapturedContext);
        }
    }

    /// <summary>
    ///     Invokes the provided asynchronous function and returns its value as an asynchronous operation that returns a <see cref="Result{TNext}" /> instance
    ///     if the <paramref name="resultTask" /> is a successful operation,
    ///     or the provided asynchronous action and returns <see cref="Result{TNext}" /> with the original error as an asynchronous operation.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNext">The type of the <see cref="Result{TNext}" /> returned by the invoked function.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="resultTask" /> represents a successful operation.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorAction">
    ///     The asynchronous action invoked when the <paramref name="resultTask" /> represents a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> outcome is passed to this action. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="ValueTask{TResult}" /> that represents the <see cref="Result{TNext}" /> value returned by <paramref name="ifSuccessFunction" />
    ///     if the <paramref name="resultTask" /> is a successful operation, or the <see cref="Result{TNext}" /> with the original outcome of
    ///     the <paramref name="resultTask" /> operation after invoking <paramref name="ifErrorAction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="ifSuccessFunction" />, or <paramref name="ifErrorAction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     or <paramref name="ifSuccessFunction" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    public static ValueTask<Result<TNext>> MatchAsync<T, TNext>(
        this ValueTask<Result<T>> resultTask,
        Func<T, ValueTask<Result<TNext>>> ifSuccessFunction,
        Func<Error, ValueTask> ifErrorAction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        return Core(resultTask, ifSuccessFunction, ifErrorAction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result<TNext>> Core(
            ValueTask<Result<T>> resultTask,
            Func<T, ValueTask<Result<TNext>>> ifSuccessFunction,
            Func<Error, ValueTask> ifErrorAction,
            bool continueOnCapturedContext)
        {
            var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

            if (result is null)
                throw Returned.NullAwaited(nameof(resultTask));

            if (result.IsSuccess())
                return await Returned.NotNullResult(ifSuccessFunction(result.Value), nameof(ifSuccessFunction), continueOnCapturedContext);

            await ifErrorAction(result.Error).ConfigureAwait(continueOnCapturedContext);
            return result.Error;
        }
    }

    /// <summary>
    ///     Executes one of the provided asynchronous functions depending on whether the <paramref name="resultTask" /> is a successful operation and returns
    ///     their output as a <see cref="Result{TNext}" /> instance.
    /// </summary>
    /// <remarks>
    ///     The method executes passed <paramref name="resultTask" /> asynchronous operation.
    /// </remarks>
    /// <typeparam name="T">The type wrapped by the source <see cref="Result{T}" />.</typeparam>
    /// <typeparam name="TNext">The type of the <see cref="Result{TNext}" /> returned by the invoked function.</typeparam>
    /// <param name="resultTask">
    ///     The asynchronous operation to be executed and which result is to be inspected.
    /// </param>
    /// <param name="ifSuccessFunction">
    ///     The asynchronous function invoked when the <paramref name="resultTask" /> represents a successful operation.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter. Must not be <see langword="null" />.
    /// </param>
    /// <param name="ifErrorFunction">
    ///     The asynchronous action invoked when the <paramref name="resultTask" /> represents a failed operation.
    ///     The <see cref="Error" /> associated with the <paramref name="resultTask" /> outcome is passed to this action. Must not be <see langword="null" />.
    /// </param>
    /// <param name="continueOnCapturedContext">
    ///     <see langword="true" /> to run the continuations, including the passed delegates when they run after an <c>await</c>,
    ///     on the captured context (e.g., the UI thread); <see langword="false" /> to run them on any thread;
    ///     <see langword="null" /> (the default) to use <see cref="ResultConfiguration.ContinueOnCapturedContext" />.
    /// </param>
    /// <returns>
    ///     The <see cref="ValueTask{TResult}" /> that represents the <see cref="Result{TNext}" /> returned by either <paramref name="ifSuccessFunction" /> or
    ///     <paramref name="ifErrorFunction" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="ifSuccessFunction" />, or <paramref name="ifErrorFunction" /> parameters are <see langword="null" />.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     If the asynchronous operation represented by <paramref name="resultTask" /> returns <see langword="null" />,
    ///     <paramref name="ifSuccessFunction" /> returns a value task with a <see langword="null" /> result,
    ///     or <paramref name="ifErrorFunction" /> returns a value task with a <see langword="null" /> result.
    /// </exception>
    public static ValueTask<Result<TNext>> MatchAsync<T, TNext>(
        this ValueTask<Result<T>> resultTask,
        Func<T, ValueTask<Result<TNext>>> ifSuccessFunction,
        Func<Error, ValueTask<Result<TNext>>> ifErrorFunction,
        bool? continueOnCapturedContext = null)
    {
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return Core(resultTask, ifSuccessFunction, ifErrorFunction, continueOnCapturedContext ?? ResultConfiguration.ContinueOnCapturedContext);

        static async ValueTask<Result<TNext>> Core(
            ValueTask<Result<T>> resultTask,
            Func<T, ValueTask<Result<TNext>>> ifSuccessFunction,
            Func<Error, ValueTask<Result<TNext>>> ifErrorFunction,
            bool continueOnCapturedContext)
        {
            var result = await resultTask.ConfigureAwait(continueOnCapturedContext);

            if (result is null)
                throw Returned.NullAwaited(nameof(resultTask));

            return result.IsSuccess()
                ? await Returned.NotNullResult(ifSuccessFunction(result.Value), nameof(ifSuccessFunction), continueOnCapturedContext)
                : await Returned.NotNullResult(ifErrorFunction(result.Error), nameof(ifErrorFunction), continueOnCapturedContext);
        }
    }

    #endregion
}
