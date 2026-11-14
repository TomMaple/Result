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
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Maple.Result.Helpers;

namespace Maple.Result;

/// <summary>
///     The structure for a templated message.
/// </summary>
/// <remarks>
///     <para>The purpose of this structure is to provide a way to generate client-side localized messages.</para>
///     <para>
///         The record is immutable: its properties are get-only, and the parameters are copied into a read-only
///         dictionary when the instance is created, so later changes to the source are not reflected.
///     </para>
/// </remarks>
public sealed record TemplatedMessage
{
    private static readonly ReadOnlyDictionary<string, object?> EmptyParams =
        new(new Dictionary<string, object?>(StringComparer.Ordinal));

    #region constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="TemplatedMessage" /> record with the parameters given as
    ///     key-value pairs (e.g., a dictionary).
    /// </summary>
    /// <param name="templateId">The identifier of the message template.</param>
    /// <param name="parameters">
    ///     The optional parameters (names and values) that might be required to generate a message from the
    ///     template; values can be <see langword="null" />. When <see langword="null" />, <see cref="Params" /> is
    ///     <see langword="null" />.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="templateId" /> or a parameter name is <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="templateId" /> is empty or whitespace-only, or a parameter name is repeated.
    /// </exception>
    /// <remarks>
    ///     A <see langword="null" /> or an empty collection expression (<c>[]</c>) as the second argument binds to this
    ///     constructor.
    /// </remarks>
    [OverloadResolutionPriority(1)]
    public TemplatedMessage(string templateId, IEnumerable<KeyValuePair<string, object?>>? parameters = null)
        : this(Validate(templateId, parameters, nameof(templateId), nameof(parameters)))
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="TemplatedMessage" /> record with the parameters given as
    ///     name-value tuples, e.g., <c>new TemplatedMessage("errors.length", ("min", 3), ("max", 10))</c>.
    /// </summary>
    /// <param name="templateId">The identifier of the message template.</param>
    /// <param name="parameters">
    ///     The parameters (names and values) that might be required to generate a message from the template; values
    ///     can be <see langword="null" />. When none are passed, <see cref="Params" /> is an empty dictionary.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="templateId" /> or a parameter name is <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="templateId" /> is empty or whitespace-only, or a parameter name is repeated.
    /// </exception>
    public TemplatedMessage(string templateId, params ReadOnlySpan<(string Key, object? Value)> parameters)
        : this(Validate(templateId, parameters, nameof(templateId), nameof(parameters)))
    {
    }

    private TemplatedMessage(Validated validated)
    {
        TemplateId = validated.TemplateId;
        Params = validated.Params;
    }

    #endregion

    /// <summary>
    ///     The identifier of the message template (never <see langword="null" />, empty or whitespace-only).
    /// </summary>
    public string TemplateId { get; }

    /// <summary>
    ///     The optional collection of parameters (names and values) that might be required
    ///     to generate a message from the specific template.
    /// </summary>
    /// <remarks>
    ///     A read-only dictionary built from the parameters passed to the constructor, with keys compared ordinally
    ///     (case-sensitive); <see langword="null" /> when none were passed as key-value pairs. Values may be
    ///     <see langword="null" />.
    /// </remarks>
    public IReadOnlyDictionary<string, object?>? Params { get; }

    #region internal factory methods

    /// <summary>
    ///     Creates a templated message, reporting invalid arguments with the parameter names of the calling method.
    /// </summary>
    internal static TemplatedMessage Create(string templateId, ReadOnlySpan<(string Key, object? Value)> parameters,
        string templateIdParameterName, string parametersParameterName)
    {
        return new TemplatedMessage(Validate(templateId, parameters, templateIdParameterName, parametersParameterName));
    }

    /// <summary>
    ///     Creates a templated message when <paramref name="templateId" /> is set; otherwise returns
    ///     <see langword="null" />, and throws if parameters were passed without a template identifier.
    /// </summary>
    internal static TemplatedMessage? CreateOptional(string? templateId,
        ReadOnlySpan<(string Key, object? Value)> parameters, string templateIdParameterName,
        string parametersParameterName)
    {
        if (templateId is not null)
            return Create(templateId, parameters, templateIdParameterName, parametersParameterName);

        if (!parameters.IsEmpty)
            throw ParametersWithoutTemplateId(templateIdParameterName, parametersParameterName);

        return null;
    }

    /// <inheritdoc cref="CreateOptional(string?, ReadOnlySpan{ValueTuple{string, object?}}, string, string)" />
    internal static TemplatedMessage? CreateOptional(string? templateId,
        IEnumerable<KeyValuePair<string, object?>>? parameters, string templateIdParameterName,
        string parametersParameterName)
    {
        if (templateId is not null)
            return new TemplatedMessage(Validate(templateId, parameters, templateIdParameterName, parametersParameterName));

        if (parameters is not null && parameters.Any())
            throw ParametersWithoutTemplateId(templateIdParameterName, parametersParameterName);

        return null;
    }

    #endregion

    #region equality

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

    #endregion

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

    #region validation

    private static Validated Validate(string templateId, IEnumerable<KeyValuePair<string, object?>>? parameters,
        string templateIdParameterName, string parametersParameterName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(templateId, templateIdParameterName);

        if (parameters is null)
            return new Validated(templateId, null);

        var dictionary = new Dictionary<string, object?>(StringComparer.Ordinal);
        foreach (var (key, value) in parameters)
            Add(dictionary, key, value, parametersParameterName);

        return new Validated(templateId, ToReadOnly(dictionary));
    }

    private static Validated Validate(string templateId, ReadOnlySpan<(string Key, object? Value)> parameters,
        string templateIdParameterName, string parametersParameterName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(templateId, templateIdParameterName);

        var dictionary = new Dictionary<string, object?>(parameters.Length, StringComparer.Ordinal);
        foreach (var (key, value) in parameters)
            Add(dictionary, key, value, parametersParameterName);

        return new Validated(templateId, ToReadOnly(dictionary));
    }

    private static void Add(Dictionary<string, object?> dictionary, string key, object? value, string parameterName)
    {
        if (key is null)
            throw new ArgumentNullException(parameterName, "A parameter name must not be null.");

        if (!dictionary.TryAdd(key, value))
            throw new ArgumentException($"The parameter ‘{key}’ is repeated.", parameterName);
    }

    private static ReadOnlyDictionary<string, object?> ToReadOnly(Dictionary<string, object?> dictionary)
    {
        return dictionary.Count == 0 ? EmptyParams : new ReadOnlyDictionary<string, object?>(dictionary);
    }

    private static ArgumentException ParametersWithoutTemplateId(string templateIdParameterName,
        string parametersParameterName)
    {
        return new ArgumentException(
            $"Parameters were passed without a template identifier. Set ‘{templateIdParameterName}’ or remove them.",
            parametersParameterName);
    }

    private readonly record struct Validated(string TemplateId, ReadOnlyDictionary<string, object?>? Params);

    #endregion
}
