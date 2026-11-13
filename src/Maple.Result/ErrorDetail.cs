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

namespace Maple.Result;

/// <summary>
///     The structure for an individual error occurrence that contains a problem detail.
/// </summary>
/// <remarks>
///     <para>The record is immutable: its properties are get-only.</para>
///     <para>
///         For more details about JSON Pointers, see: <seealso href="https://datatracker.ietf.org/doc/html/rfc6901"/>.
///     </para>
/// </remarks>
/// <param name="PropertyPointer">The JSON Pointer which identifies the invalid value in the input data.</param>
/// <param name="Detail">The human-readable explanation specific to this individual error occurrence.</param>
/// <param name="DetailTemplated">The message template with the human-readable explanation.</param>
/// <exception cref="ArgumentNullException">If the <paramref name="Detail" /> is <see langword="null" />.</exception>
public sealed record ErrorDetail(string? PropertyPointer, string Detail, TemplatedMessage? DetailTemplated = null)
{
    /// <summary>
    ///     The JSON Pointer which identifies the invalid value in the input data.
    /// </summary>
    public string? PropertyPointer { get; } = PropertyPointer;

    /// <summary>
    ///     The human-readable explanation specific to this individual error occurrence (never <see langword="null" />).
    /// </summary>
    public string Detail { get; } = Detail ?? throw new ArgumentNullException(nameof(Detail));

    /// <summary>
    ///     The message template with the human-readable explanation.
    /// </summary>
    public TemplatedMessage? DetailTemplated { get; } = DetailTemplated;
}
