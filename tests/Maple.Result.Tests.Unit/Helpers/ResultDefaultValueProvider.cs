// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Moq;
using System;
using System.Threading.Tasks;

namespace Maple.Result.Tests.Unit.Helpers;

/// <summary>
///     Default return values for mocked functions that the extension methods require to be non-null: a successful
///     <see cref="Result" /> or <see cref="Result{T}" />, an <see cref="Error" />, and completed tasks of those.
/// </summary>
/// <remarks>
///     Use it as <c>new Mock&lt;Func&lt;Result&gt;&gt; { DefaultValueProvider = ResultDefaultValueProvider.Instance }</c> when a test only checks
///     that the function was called: the extension methods throw an <see cref="InvalidOperationException" /> when the function returns
///     <see langword="null" /> (issue #121).
/// </remarks>
internal sealed class ResultDefaultValueProvider : LookupOrFallbackDefaultValueProvider
{
    internal static readonly ResultDefaultValueProvider Instance = new();

    private static readonly Error DefaultError = Error.Failure(ErrorUri.None(), "d5c4d8a2-6f0e-4d7b-9a51-3f1c8e2b7a40", "Default error");

    private ResultDefaultValueProvider()
    {
        Register(typeof(Result), (_, _) => Result.Success());
        Register(typeof(Result<>), (type, mock) => CreateSuccessfulResult(type, mock));
        Register(typeof(Error), (_, _) => DefaultError);
        Register(typeof(Task), (_, _) => Task.CompletedTask);
        Register(typeof(Task<>), (type, mock) => CreateTask(type, mock));
        Register(typeof(ValueTask), (_, _) => ValueTask.CompletedTask);
        Register(typeof(ValueTask<>), (type, mock) => Activator.CreateInstance(type, GetDefaultValue(type.GetGenericArguments()[0], mock))!);
    }

    protected override object? GetFallbackDefaultValue(Type type, Mock mock)
    {
        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }

    private object CreateSuccessfulResult(Type type, Mock mock)
    {
        var fromValue = type.GetMethod(nameof(Result<>.FromValue))!;
        return fromValue.Invoke(null, [GetDefaultValue(type.GetGenericArguments()[0], mock)])!;
    }

    private object CreateTask(Type type, Mock mock)
    {
        var valueType = type.GetGenericArguments()[0];
        var fromResult = typeof(Task).GetMethod(nameof(Task.FromResult))!.MakeGenericMethod(valueType);
        return fromResult.Invoke(null, [GetDefaultValue(valueType, mock)])!;
    }
}
