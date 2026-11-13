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
using System.Text;
using Maple.Result.Helpers;

namespace Maple.Result;

/// <summary>
///     The structure for a templated message.
/// </summary>
/// <remarks>
///     <para>The purpose of this structure is to provide a way to generate client-side localized messages.</para>
///     <para>
///         The record is immutable: its properties are get-only, and the <see cref="Params" /> are copied when the
///         instance is created, so later changes to the source dictionary are not reflected.
///     </para>
/// </remarks>
/// <param name="TemplateId">The identifier of the message template.</param>
/// <param name="Params">
///     The optional collection of parameters (names and values) that might be required
///     to generate a message from the specific template.
/// </param>
/// <exception cref="ArgumentNullException">If the <paramref name="TemplateId" /> is <see langword="null" />.</exception>
/// <exception cref="ArgumentException">If the <paramref name="TemplateId" /> is empty or whitespace-only.</exception>
public sealed record TemplatedMessage(string TemplateId, IReadOnlyDictionary<string, object?>? Params = null)
{
    /// <summary>
    ///     The identifier of the message template (never <see langword="null" />, empty or whitespace-only).
    /// </summary>
    public string TemplateId { get; } = ValidateTemplateId(TemplateId);

    /// <summary>
    ///     The optional collection of parameters (names and values) that might be required
    ///     to generate a message from the specific template.
    /// </summary>
    /// <remarks>
    ///     A read-only copy of the dictionary passed to the constructor, with keys compared ordinally
    ///     (case-sensitive); <see langword="null" /> when none was passed. Values may be <see langword="null" />.
    /// </remarks>
    public IReadOnlyDictionary<string, object?>? Params { get; } = DictionaryHelper.Copy(Params);

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
               && DictionaryHelper.Equal(Params, other.Params);
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
            hashCode.Add(DictionaryHelper.GetHashCode(Params));

        return hashCode.ToHashCode();
    }

    /// <summary>
    ///     Appends the members of the message to the <see cref="ToString" /> output, writing the content of the
    ///     <see cref="Params" /> dictionary instead of its type name.
    /// </summary>
    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append("TemplateId = ").Append(TemplateId);
        builder.Append(", Params = ");

        if (Params is not null)
            DictionaryHelper.Append(builder, Params);

        return true;
    }

    private static string ValidateTemplateId(string templateId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(templateId, nameof(TemplateId));

        return templateId;
    }
}
