// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System;
using System.Collections.Generic;

namespace Maple.Result;

/// <summary>
///     The structure for a templated message.
/// </summary>
/// <remarks>
///     The purpose of this structure is to provide a way to generate client-side localized messages.
/// </remarks>
/// <param name="TemplateId">The identifier of the message template.</param>
/// <param name="Params">
///     The optional collection of parameters (names and values) that might be required
///     to generate a message from the specific template.
/// </param>
public sealed record TemplatedMessage(string TemplateId, IReadOnlyDictionary<string, object>? Params = null)
{
    /// <summary>
    ///     Determines whether the specified <see cref="TemplatedMessage" /> is equal to the current one,
    ///     comparing the <see cref="Params" /> collection by its content rather than by reference.
    /// </summary>
    /// <param name="other">The <see cref="TemplatedMessage" /> to compare with the current instance.</param>
    /// <returns>
    ///     <see langword="true" /> if the specified <see cref="TemplatedMessage" /> is equal to the current one;
    ///     otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals(TemplatedMessage? other)
    {
        return other is not null
               && EqualityContract == other.EqualityContract
               && TemplateId == other.TemplateId
               && ParamsEqual(Params, other.Params);
    }

    /// <summary>
    ///     Returns a hash code that is consistent with <see cref="Equals(TemplatedMessage)" />, incorporating
    ///     the content of the <see cref="Params" /> collection.
    /// </summary>
    /// <returns>A hash code for the current <see cref="TemplatedMessage" />.</returns>
    public override int GetHashCode()
    {
        var hashCode = new HashCode();

        hashCode.Add(EqualityContract);
        hashCode.Add(TemplateId);

        if (Params is not null)
        {
            // Combine the entries in an order-independent way, since a dictionary is unordered.
            var paramsHashCode = 0;
            foreach (var pair in Params)
                paramsHashCode ^= HashCode.Combine(pair.Key, pair.Value);

            hashCode.Add(paramsHashCode);
        }

        return hashCode.ToHashCode();
    }

    private static bool ParamsEqual(IReadOnlyDictionary<string, object>? left,
        IReadOnlyDictionary<string, object>? right)
    {
        if (ReferenceEquals(left, right))
            return true;

        if (left is null || right is null || left.Count != right.Count)
            return false;

        foreach (var pair in left)
        {
            if (!right.TryGetValue(pair.Key, out var value) || !object.Equals(pair.Value, value))
                return false;
        }

        return true;
    }
}
