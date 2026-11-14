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
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Maple.Result.Tests.Unit.Helpers;

/// <summary>
///     A synchronization context that runs every posted callback on the thread that called <see cref="Run" />, like the
///     UI thread of a WPF or Windows Forms application.
/// </summary>
internal sealed class SingleThreadSynchronizationContext : SynchronizationContext
{
    private readonly BlockingCollection<(SendOrPostCallback Callback, object? State)> _queue = new();

    public override void Post(SendOrPostCallback d, object? state)
    {
        _queue.Add((d, state));
    }

    public override void Send(SendOrPostCallback d, object? state)
    {
        throw new NotSupportedException("Synchronous dispatch is not supported.");
    }

    /// <summary>
    ///     Runs the asynchronous function with this context installed as the current one, processes the posted
    ///     callbacks until the function completes, and returns its result.
    /// </summary>
    internal static T Run<T>(Func<SingleThreadSynchronizationContext, Task<T>> function)
    {
        var previous = Current;
        var context = new SingleThreadSynchronizationContext();
        SetSynchronizationContext(context);

        try
        {
            var task = function(context);
            task.ContinueWith(_ => context._queue.CompleteAdding(), TaskScheduler.Default);

            foreach (var (callback, state) in context._queue.GetConsumingEnumerable())
                callback(state);

            return task.GetAwaiter().GetResult();
        }
        finally
        {
            SetSynchronizationContext(previous);
        }
    }
}
