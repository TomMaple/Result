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
using System.Collections.ObjectModel;
using System.Text;

namespace Maple.Result.Helpers;

/// <summary>
///     Content-based (order-independent) equality and hashing for string-keyed dictionaries, read-only copies, and
///     their text representation.
/// </summary>
internal static class DictionaryHelper
{
    /// <summary>
    ///     Appends the entries of the dictionary to the <paramref name="builder" /> as <c>{ key = value, … }</c>
    ///     (<c>{}</c> when empty), in the style of the compiler-generated <c>ToString</c> of a record.
    /// </summary>
    internal static void Append(StringBuilder builder, IReadOnlyDictionary<string, object?> dictionary)
    {
        if (dictionary.Count == 0)
        {
            builder.Append("{}");
            return;
        }

        builder.Append("{ ");

        var first = true;
        foreach (var pair in dictionary)
        {
            if (!first)
                builder.Append(", ");

            builder.Append(pair.Key).Append(" = ").Append(pair.Value);
            first = false;
        }

        builder.Append(" }");
    }

    /// <summary>
    ///     Determines whether two dictionaries contain the same keys with equal values, regardless of order.
    /// </summary>
    internal static bool Equal(IReadOnlyDictionary<string, object?>? left, IReadOnlyDictionary<string, object?>? right)
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
    internal static int GetHashCode(IReadOnlyDictionary<string, object?> dictionary)
    {
        // Combine the entries in an order-independent way, since a dictionary is unordered.
        var hashCode = 0;
        foreach (var pair in dictionary)
            hashCode ^= HashCode.Combine(pair.Key, pair.Value);

        return hashCode;
    }

    /// <summary>
    ///     Returns a read-only copy of the entries, keyed with the ordinal comparer, or <see langword="null" /> when
    ///     <paramref name="source" /> is <see langword="null" />.
    /// </summary>
    /// <remarks>
    ///     The copy is independent of <paramref name="source" />: later changes to the source do not affect it, and
    ///     it cannot be modified through a cast. A repeated key keeps the last value.
    /// </remarks>
    internal static ReadOnlyDictionary<string, object?>? Copy(IEnumerable<KeyValuePair<string, object?>>? source)
    {
        if (source is null)
            return null;

        var copy = new Dictionary<string, object?>(StringComparer.Ordinal);
        foreach (var pair in source)
            copy[pair.Key] = pair.Value;

        return new ReadOnlyDictionary<string, object?>(copy);
    }
}
