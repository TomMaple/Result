// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Result.Extensions.ValueTasks;
using Maple.Result.Tests.Unit.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Maple.Result.Tests.Unit.Extensions.ValueTasks;

/// <summary>
///     Verifies on which context the delegates passed to the <see cref="ValueTask" />-based extension methods run.
/// </summary>
/// <remarks>
///     The awaited task is completed by a thread-pool thread only after the extension method has started awaiting it,
///     so the continuation is never inlined and the result depends only on the effective
///     <c>continueOnCapturedContext</c> value.
/// </remarks>
[Collection(ResultConfigurationCollection.Name)]
public sealed class SynchronizationContextValueTaskUnitTests : IDisposable
{
    public void Dispose()
    {
        ResultConfiguration.ContinueOnCapturedContext = false;
    }

    #region IfSuccessAsync

    [Fact]
    public void IfSuccessAsync_DefaultSettings_RunsDelegateOffCapturedContext()
    {
        // Act
        var (context, callbackContext) = RunIfSuccessAsync(continueOnCapturedContext: null);

        // Assert
        callbackContext.ShouldNotBeSameAs(context);
    }

    [Fact]
    public void IfSuccessAsync_GlobalSettingTrue_RunsDelegateOnCapturedContext()
    {
        // Arrange
        ResultConfiguration.ContinueOnCapturedContext = true;

        // Act
        var (context, callbackContext) = RunIfSuccessAsync(continueOnCapturedContext: null);

        // Assert
        callbackContext.ShouldBeSameAs(context);
    }

    [Fact]
    public void IfSuccessAsync_GlobalSettingTrueAndArgumentFalse_RunsDelegateOffCapturedContext()
    {
        // Arrange
        ResultConfiguration.ContinueOnCapturedContext = true;

        // Act
        var (context, callbackContext) = RunIfSuccessAsync(continueOnCapturedContext: false);

        // Assert
        callbackContext.ShouldNotBeSameAs(context);
    }

    [Fact]
    public void IfSuccessAsync_GlobalSettingFalseAndArgumentTrue_RunsDelegateOnCapturedContext()
    {
        // Act
        var (context, callbackContext) = RunIfSuccessAsync(continueOnCapturedContext: true);

        // Assert
        callbackContext.ShouldBeSameAs(context);
    }

    #endregion

    #region LINQ

    [Fact]
    public void Select_DefaultSettings_RunsSelectorOffCapturedContext()
    {
        // Act
        var (context, callbackContext) = RunSelect();

        // Assert
        callbackContext.ShouldNotBeSameAs(context);
    }

    [Fact]
    public void Select_GlobalSettingTrue_RunsSelectorOnCapturedContext()
    {
        // Arrange
        ResultConfiguration.ContinueOnCapturedContext = true;

        // Act
        var (context, callbackContext) = RunSelect();

        // Assert
        callbackContext.ShouldBeSameAs(context);
    }

    #endregion

    #region helper methods

    private static (SynchronizationContext Context, SynchronizationContext? CallbackContext) RunIfSuccessAsync(
        bool? continueOnCapturedContext)
    {
        return SingleThreadSynchronizationContext.Run(async context =>
        {
            var source = new TaskCompletionSource<Result<int>>(TaskCreationOptions.RunContinuationsAsynchronously);
            var holder = new ContextHolder();
            Func<int, ValueTask> action = _ =>
            {
                holder.Value = SynchronizationContext.Current;
                return ValueTask.CompletedTask;
            };

            var chain = new ValueTask<Result<int>>(source.Task).IfSuccessAsync(action, continueOnCapturedContext);
            CompleteOnThreadPool(source, 1);
            await chain;

            return (context, holder.Value);
        });
    }

    private static (SynchronizationContext Context, SynchronizationContext? CallbackContext) RunSelect()
    {
        return SingleThreadSynchronizationContext.Run(async context =>
        {
            var source = new TaskCompletionSource<Result<int>>(TaskCreationOptions.RunContinuationsAsynchronously);
            var holder = new ContextHolder();

            var query = from x in new ValueTask<Result<int>>(source.Task) select Capture(x, holder);

            CompleteOnThreadPool(source, 1);
            await query;

            return (context, holder.Value);
        });
    }

    private static int Capture(int value, ContextHolder holder)
    {
        holder.Value = SynchronizationContext.Current;
        return value;
    }

    private static void CompleteOnThreadPool(TaskCompletionSource<Result<int>> source, int value)
    {
        ThreadPool.QueueUserWorkItem(_ => source.SetResult(Result<int>.FromValue(value)));
    }

    private sealed class ContextHolder
    {
        public SynchronizationContext? Value { get; set; }
    }

    #endregion
}
