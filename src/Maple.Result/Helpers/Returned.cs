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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Maple.Result.Helpers;

/// <summary>
///     Checks what the functions passed to the extension methods return. A <see langword="null" /> where a result, an <see cref="Error" /> or
///     a task is required is a bug in the passed function, not in an argument, so it is reported with an <see cref="InvalidOperationException" />
///     that names the function.
/// </summary>
/// <remarks>
///     A <see langword="null" /> value of a <see cref="Result{T}" /> is not checked: it is a valid value when the type is nullable.
/// </remarks>
internal static class Returned
{
    /// <summary>
    ///     Returns the <paramref name="value" /> returned by the function named <paramref name="functionName" />, or throws if it is
    ///     <see langword="null" />.
    /// </summary>
    /// <exception cref="InvalidOperationException">If the <paramref name="value" /> is <see langword="null" />.</exception>
    internal static T NotNull<T>([NotNull] T? value, string functionName)
    {
        if (value is null)
            throw NullReturned(functionName);

        return value;
    }

    /// <summary>
    ///     Returns the <paramref name="task" /> returned by the function named <paramref name="functionName" />, or throws if it is
    ///     <see langword="null" />. The result of the task is not checked.
    /// </summary>
    /// <exception cref="InvalidOperationException">If the <paramref name="task" /> is <see langword="null" />.</exception>
    internal static TTask NotNullTask<TTask>([NotNull] TTask? task, string functionName)
        where TTask : Task
    {
        return task ?? throw NullTaskReturned(functionName);
    }

    /// <summary>
    ///     Returns an awaitable for the <paramref name="task" /> returned by the function named <paramref name="functionName" /> that throws if
    ///     the task, or its result, is <see langword="null" />.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///     If the <paramref name="task" /> is <see langword="null" />; when awaited, if its result is <see langword="null" />.
    /// </exception>
    internal static NotNullTaskAwaitable<T> NotNullResult<T>(Task<T>? task, string functionName, bool continueOnCapturedContext)
    {
        return new NotNullTaskAwaitable<T>(NotNullTask(task, functionName).ConfigureAwait(continueOnCapturedContext).GetAwaiter(), functionName);
    }

    /// <summary>
    ///     Returns an awaitable for the <paramref name="task" /> returned by the function named <paramref name="functionName" /> that throws if
    ///     its result is <see langword="null" />.
    /// </summary>
    /// <exception cref="InvalidOperationException">When awaited, if the result of the <paramref name="task" /> is <see langword="null" />.</exception>
    internal static NotNullValueTaskAwaitable<T> NotNullResult<T>(ValueTask<T> task, string functionName, bool continueOnCapturedContext)
    {
        return new NotNullValueTaskAwaitable<T>(task.ConfigureAwait(continueOnCapturedContext).GetAwaiter(), functionName);
    }

    /// <summary>
    ///     Creates the exception for an asynchronous operation, passed as the parameter named <paramref name="parameterName" />,
    ///     that returned <see langword="null" />.
    /// </summary>
    internal static InvalidOperationException NullAwaited(string parameterName)
    {
        return new InvalidOperationException($"The asynchronous operation represented by ‘{parameterName}’ returned null.");
    }

    private static InvalidOperationException NullReturned(string functionName)
    {
        return new InvalidOperationException($"The function ‘{functionName}’ returned null.");
    }

    private static InvalidOperationException NullTaskReturned(string functionName)
    {
        return new InvalidOperationException($"The function ‘{functionName}’ returned a null task.");
    }

    /// <summary>
    ///     Awaits a <see cref="Task{TResult}" /> as <see cref="ConfiguredTaskAwaitable{TResult}" /> does and throws if its result is
    ///     <see langword="null" />. It is its own awaiter.
    /// </summary>
    internal readonly struct NotNullTaskAwaitable<T> : ICriticalNotifyCompletion
    {
        private readonly ConfiguredTaskAwaitable<T>.ConfiguredTaskAwaiter _awaiter;
        private readonly string _functionName;

        internal NotNullTaskAwaitable(ConfiguredTaskAwaitable<T>.ConfiguredTaskAwaiter awaiter, string functionName)
        {
            _awaiter = awaiter;
            _functionName = functionName;
        }

        public bool IsCompleted => _awaiter.IsCompleted;

        public NotNullTaskAwaitable<T> GetAwaiter()
        {
            return this;
        }

        public T GetResult()
        {
            var result = _awaiter.GetResult();

            if (result is null)
                throw NullReturned(_functionName);

            return result;
        }

        public void OnCompleted(Action continuation)
        {
            _awaiter.OnCompleted(continuation);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            _awaiter.UnsafeOnCompleted(continuation);
        }
    }

    /// <summary>
    ///     Awaits a <see cref="ValueTask{TResult}" /> as <see cref="ConfiguredValueTaskAwaitable{TResult}" /> does and throws if its result is
    ///     <see langword="null" />. It is its own awaiter.
    /// </summary>
    internal readonly struct NotNullValueTaskAwaitable<T> : ICriticalNotifyCompletion
    {
        private readonly ConfiguredValueTaskAwaitable<T>.ConfiguredValueTaskAwaiter _awaiter;
        private readonly string _functionName;

        internal NotNullValueTaskAwaitable(ConfiguredValueTaskAwaitable<T>.ConfiguredValueTaskAwaiter awaiter, string functionName)
        {
            _awaiter = awaiter;
            _functionName = functionName;
        }

        public bool IsCompleted => _awaiter.IsCompleted;

        public NotNullValueTaskAwaitable<T> GetAwaiter()
        {
            return this;
        }

        public T GetResult()
        {
            var result = _awaiter.GetResult();

            if (result is null)
                throw NullReturned(_functionName);

            return result;
        }

        public void OnCompleted(Action continuation)
        {
            _awaiter.OnCompleted(continuation);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            _awaiter.UnsafeOnCompleted(continuation);
        }
    }
}
