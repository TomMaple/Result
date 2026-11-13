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
using System.Collections.Generic;

namespace Maple.Result;

/// <summary>
///     Content-based (order-independent) equality and hashing for string-keyed dictionaries.
/// </summary>
internal static class DictionaryContent
{
    /// <summary>
    ///     Determines whether two dictionaries contain the same keys with equal values, regardless of order.
    /// </summary>
    internal static bool Equal(IReadOnlyDictionary<string, object>? left, IReadOnlyDictionary<string, object>? right)
    {
        if (ReferenceEquals(left, right))
            return true;

        if (left is null || right is null || left.Count != right.Count)
            return false;

        foreach (var pair in left)
        {
            if (!right.TryGetValue(pair.Key, out var value) || !Equals(pair.Value, value))
                return false;
        }

        return true;
    }

    /// <summary>
    ///     Returns an order-independent hash code of the dictionary entries, consistent with
    ///     <see cref="Equal" />.
    /// </summary>
    internal static int GetHashCode(IReadOnlyDictionary<string, object> dictionary)
    {
        // Combine the entries in an order-independent way, since a dictionary is unordered.
        var hashCode = 0;
        foreach (var pair in dictionary)
            hashCode ^= HashCode.Combine(pair.Key, pair.Value);

        return hashCode;
    }
}
