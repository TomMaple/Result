// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

namespace Maple.Result;

/// <summary>
///     Global settings of the library.
/// </summary>
/// <remarks>
///     Set them once at application startup, before any other code of the library runs. They apply to the whole
///     process.
/// </remarks>
public static class ResultConfiguration
{
    private static volatile bool s_continueOnCapturedContext;

    /// <summary>
    ///     The default value used by the asynchronous extension methods (e.g., <c>IfSuccessAsync</c>,
    ///     <c>MatchAsync</c> and the LINQ query operators) when their <c>continueOnCapturedContext</c> argument is not
    ///     passed. The default is <see langword="false" />.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         When it is <see langword="false" />, the methods that take a <c>Task</c> or <c>ValueTask</c> of a result
    ///         await it without capturing the current <see cref="System.Threading.SynchronizationContext" />, so the
    ///         passed delegates run on a thread-pool thread. This is the common choice for libraries and is the right
    ///         one for ASP.NET Core, console applications, Azure Functions and background services, which have no
    ///         synchronization context.
    ///     </para>
    ///     <para>
    ///         Set it to <see langword="true" /> in applications with a synchronization context (WPF, Windows Forms,
    ///         .NET MAUI, Blazor Server) when the delegates access UI elements or other context-bound state: the
    ///         delegates then run on the captured context (e.g., the UI thread). An explicit
    ///         <c>continueOnCapturedContext</c> argument always takes precedence over this setting.
    ///     </para>
    /// </remarks>
    /// <example>
    ///     <code>
    ///         // e.g., in App.OnStartup (WPF) or MauiProgram.CreateMauiApp (.NET MAUI)
    ///         ResultConfiguration.ContinueOnCapturedContext = true;
    ///     </code>
    /// </example>
    public static bool ContinueOnCapturedContext
    {
        get => s_continueOnCapturedContext;
        set => s_continueOnCapturedContext = value;
    }
}
