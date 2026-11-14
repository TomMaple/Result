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
using System.Collections.Frozen;

namespace Maple.Result.Validators;

/// <summary>
///     Validates the keys of the <see cref="Error.Metadata" /> entries.
/// </summary>
/// <remarks>
///     <para>
///         Metadata entries correspond to the <i>RFC 9457</i> extension members, which share the top-level object with the standard members.
///         A key that matches a standard member name would produce a duplicate or ambiguous member once the metadata is flattened into
///         a problem details response, so such keys are rejected.
///     </para>
///     <para>
///         The comparison ignores case: clients often deserialize JSON case-insensitively,
///         and naming policies can change the case of a key during serialization.
///     </para>
///     <para>
///         The naming recommendations of <i>RFC 9457</i> (section 4: letters, digits and <c>_</c>, starting with a letter, at least 3 characters)
///         are not enforced, because they are not mandatory and problem details received from other services must be accepted as they are.
///     </para>
/// </remarks>
internal static class MetadataKeyValidator
{
    private static readonly FrozenSet<string> ReservedKeys = new[]
    {
        "type", "title", "status", "detail", "instance", "errors", "detailTemplated"
    }.ToFrozenSet(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    ///     Determines whether the key matches a standard problem details member name (case-insensitive).
    /// </summary>
    internal static bool IsReserved(string key)
    {
        return ReservedKeys.Contains(key);
    }

    /// <summary>
    ///     Throws if the key is null, empty, whitespace-only or reserved.
    /// </summary>
    /// <exception cref="ArgumentNullException">If the <paramref name="key" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="key" /> is empty, whitespace-only or matches a standard problem details member name.
    /// </exception>
    internal static void Validate(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        if (IsReserved(key))
        {
            throw new ArgumentException(
                $"The metadata key ‘{key}’ is reserved. Keys must not match (case-insensitively) the standard " +
                "problem details members: ‘type’, ‘title’, ‘status’, ‘detail’, ‘instance’, ‘errors’, ‘detailTemplated’.",
                nameof(key));
        }
    }
}
