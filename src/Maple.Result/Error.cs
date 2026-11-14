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
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Maple.Result.Helpers;

namespace Maple.Result;

/// <summary>
///     The error with details.
/// </summary>
/// <remarks>
///     <para>
///         Based on the <i>RFC 9457</i>;
///         for more information, see: <seealso href="https://datatracker.ietf.org/doc/html/rfc9457" />.
///     </para>
///     <para>
///         The record is immutable. Create it with one of the factory methods (e.g., <see cref="Validation(ErrorUri, string, string?, ErrorUri?, string?, ReadOnlySpan{ValueTuple{string, object?}})" />)
///         or with an <see cref="ErrorBuilder" />. Methods such as <see cref="WithErrorDetail(ErrorDetail)" /> and
///         <see cref="WithMetadata(string, object?)" /> return a changed copy; to change several members at once, use
///         <see cref="ToBuilder" /> and <see cref="ErrorBuilder.Build" />.
///     </para>
/// </remarks>
public sealed record Error
{
    #region consts

    internal static readonly ReadOnlyCollection<ErrorDetail> EmptyErrorDetails = new([]);

    internal static readonly ReadOnlyDictionary<string, object?> EmptyMetadata =
        new(new Dictionary<string, object?>(StringComparer.Ordinal));

    #endregion

    #region constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="Error" /> record with all its members.
    /// </summary>
    /// <remarks>
    ///     The arguments are trusted: they are validated by the <see cref="ErrorBuilder" />, which is the only caller,
    ///     and the collections are read-only instances that are never modified.
    /// </remarks>
    internal Error(ErrorCategory category, string typeUri, string title, string? detail,
        TemplatedMessage? detailTemplated, string? instanceUri, ReadOnlyCollection<ErrorDetail> errorDetails,
        ReadOnlyDictionary<string, object?> metadata)
    {
        Category = category;
        TypeUri = typeUri;
        Title = title;
        Detail = detail;
        DetailTemplated = detailTemplated;
        InstanceUri = instanceUri;
        ErrorDetailsCollection = errorDetails;
        MetadataDictionary = metadata;
    }

    #endregion

    /// <summary>
    ///     The category of the error
    /// </summary>
    public ErrorCategory Category { get; }

    /// <summary>
    ///     The text containing a <i>URI</i> reference that identifies the problem type.
    /// </summary>
    /// <remarks>
    ///     <para>This is a mandatory property according to the RFC 9457 specification.</para>
    ///     The value of this attribute can be:
    ///     <list type="bullet">
    ///         <item>
    ///             <term>about:blank</term><description>when the type is not specified,</description>
    ///         </item>
    ///         <item>
    ///             <term>URI Locator</term>
    ///             <description>
    ///                 dereferencing it SHOULD provide human-readable documentation for the problem type;
    ///                 however, consumers SHOULD NOT automatically dereference the type <i>URI</i>,
    ///                 unless they do so when providing information to developers (e.g., when a debugging tool is in use);
    ///                 it is recommended to use absolute rather than relative <i>URI</i>s to avoid confusion and malfunctions;
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>URI Tag</term>
    ///             <description>
    ///                 it is not dereferenceable and uniquely represents the different types of problems;
    ///                 for more information about <i>URI Tags</i>, see:
    ///                 <seealso href="https://datatracker.ietf.org/doc/html/rfc4151" />.
    ///             </description>
    ///         </item>
    ///     </list>
    ///     Using a <i>URI</i> to have a unique ID is not recommended especially from the point of view of integration and
    ///     use of tools for which providing further information at the Developer Experience level is a fundamental objective.
    /// </remarks>
    public string TypeUri { get; }

    /// <summary>
    ///     The short, human-readable summary of the problem type (never <see langword="null" />, empty or
    ///     whitespace-only).
    /// </summary>
    /// <remarks>
    ///     It SHOULD NOT change from occurrence to occurrence of the problem, except for localization.
    ///     The “title” string is advisory and is included only for users who are unaware of and
    ///     cannot discover the semantics of the type <i>URI</i> (e.g., during offline log analysis).
    /// </remarks>
    public string Title { get; }

    /// <summary>
    ///     The human-readable explanation specific to this occurrence of the problem.
    /// </summary>
    /// <remarks>
    ///     The “detail” string, if present, ought to focus on helping the client correct the problem, rather than giving
    ///     debugging information.
    ///     Consumers SHOULD NOT parse the “detail” member for information; extensions are more suitable and less error-prone
    ///     ways to obtain such information.
    /// </remarks>
    public string? Detail { get; }

    /// <summary>
    ///     The templated explanation specific to this occurrence of the problem.
    /// </summary>
    /// <remarks>
    ///     This property contains the message template identifier and the optional collection of parameters (names and values)
    ///     that might be required to generate a localized message.
    /// </remarks>
    public TemplatedMessage? DetailTemplated { get; }

    /// <summary>
    ///     The <i>URI</i> reference that identifies the specific occurrence of the problem.
    /// </summary>
    /// <remarks>
    ///     The value of this attribute can be:
    ///     <list type="bullet">
    ///         <item>
    ///             <term>URI Locator</term>
    ///             <description>
    ///                 dereferencing it SHOULD provide the problem details object or
    ///                 information about the problem occurrence in other formats through use of proactive content negotiation;
    ///                 it is recommended to use absolute rather than relative <i>URI</i>s to avoid confusion and malfunctions;
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>URI Tag</term>
    ///             <description>
    ///                 it serves as a unique identifier for the problem occurrence which may be meaningful to the server but
    ///                 is opaque to the client;
    ///                 for more information about <i>URI Tags</i>, see:
    ///                 <seealso href="https://datatracker.ietf.org/doc/html/rfc4151" />.
    ///             </description>
    ///         </item>
    ///     </list>
    /// </remarks>
    public string? InstanceUri { get; }

    /// <summary>
    ///     The collection of individual error occurrences found, with details and a pointer to the location of each
    ///     (if applicable); never <see langword="null" />, empty by default.
    /// </summary>
    /// <remarks>
    ///     The collection is read-only and cannot be modified, also through a cast. Use
    ///     <see cref="WithErrorDetail(ErrorDetail)" /> to obtain a copy with an added occurrence, or
    ///     <see cref="ErrorBuilder.AddErrorDetail(ErrorDetail)" /> to add several.
    /// </remarks>
    public IReadOnlyList<ErrorDetail> ErrorDetails => ErrorDetailsCollection;

    internal ReadOnlyCollection<ErrorDetail> ErrorDetailsCollection { get; }

    /// <summary>
    ///     The additional, machine-readable members of the problem (never <see langword="null" />; empty by default).
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         It corresponds to the <i>extension members</i> of <i>RFC 9457</i> (section 3.2): response-level values
    ///         that consumers can read instead of parsing <see cref="Detail" /> (e.g., a retry delay). The library
    ///         does not define any well-known keys.
    ///     </para>
    ///     <para>
    ///         Keys are compared ordinally (case-sensitive). They should start with a letter, consist of letters,
    ///         digits and <c>_</c>, and be at least 3 characters long (<i>RFC 9457</i>, section 4); this is a
    ///         recommendation and is not enforced. Keys must not match the standard members (case-insensitively):
    ///         <c>type</c>, <c>title</c>, <c>status</c>, <c>detail</c>, <c>instance</c>, <c>errors</c> and
    ///         <c>detailTemplated</c>; <see cref="WithMetadata(string, object?)" /> and
    ///         <see cref="ErrorBuilder.WithMetadata(string, object?)" /> reject them.
    ///     </para>
    ///     <para>
    ///         Values may be <see langword="null" />. Use <see cref="TryGetMetadata{T}" /> to read a value as a
    ///         specific type.
    ///     </para>
    ///     <para>
    ///         The dictionary is read-only and cannot be modified, also through a cast. Use
    ///         <see cref="WithMetadata(string, object?)" /> and <see cref="WithoutMetadata(string)" /> to obtain a copy with
    ///         changed metadata, or an <see cref="ErrorBuilder" /> to change several entries. The order of the
    ///         entries is not defined.
    ///     </para>
    /// </remarks>
    public IReadOnlyDictionary<string, object?> Metadata => MetadataDictionary;

    internal ReadOnlyDictionary<string, object?> MetadataDictionary { get; }

    /// <summary>
    ///     Returns a new <see cref="ErrorBuilder" /> initialized with the members of the current error.
    /// </summary>
    /// <returns>A new instance of the <see cref="ErrorBuilder" /> class.</returns>
    /// <remarks>
    ///     Use it to obtain a changed copy of an error, e.g., <c>error.ToBuilder().WithTitle("…").Build()</c>.
    ///     The current instance is never modified.
    /// </remarks>
    public ErrorBuilder ToBuilder()
    {
        return new ErrorBuilder(this);
    }

    #region error details

    /// <summary>
    ///     Returns a copy of the current error with the specified individual error occurrence appended to the
    ///     <see cref="ErrorDetails" />.
    /// </summary>
    /// <param name="errorDetail">The error occurrence to append.</param>
    /// <returns>A new instance of the <see cref="Error" /> object; the current instance is not modified.</returns>
    /// <remarks>
    ///     To add several occurrences, use <see cref="ToBuilder" /> and
    ///     <see cref="ErrorBuilder.AddErrorDetail(ErrorDetail)" />, which does not copy the error for each one.
    /// </remarks>
    /// <exception cref="ArgumentNullException">If the <paramref name="errorDetail" /> is <see langword="null" />.</exception>
    public Error WithErrorDetail(ErrorDetail errorDetail)
    {
        return ToBuilder()
            .AddErrorDetail(errorDetail)
            .Build();
    }

    /// <summary>
    ///     Returns a copy of the current error with the specified individual error occurrence appended to the
    ///     <see cref="ErrorDetails" />.
    /// </summary>
    /// <param name="propertyPointer">The JSON Pointer which identifies the invalid value in the input data.</param>
    /// <param name="detail">The human-readable explanation specific to this individual error occurrence.</param>
    /// <param name="detailTemplateId">
    ///     The identifier of the message template; when <see langword="null" />, no
    ///     <see cref="ErrorDetail.DetailTemplated" /> is set.
    /// </param>
    /// <param name="detailNamedValues">
    ///     The optional parameters (names and values) that might be required to generate a localized message; they
    ///     require <paramref name="detailTemplateId" />.
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object; the current instance is not modified.</returns>
    /// <remarks>
    ///     <para>
    ///         To add several occurrences, use <see cref="ToBuilder" /> and
    ///         <see cref="ErrorBuilder.AddErrorDetail(string?, string, string?, ReadOnlySpan{ValueTuple{string, object?}})" />,
    ///         which does not copy the error for each one.
    ///     </para>
    ///     <para>
    ///         For more details about JSON Pointers, see: <seealso href="https://datatracker.ietf.org/doc/html/rfc6901" />.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="detail" /> or a parameter name in <paramref name="detailNamedValues" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="detailTemplateId" /> is empty or whitespace-only, a parameter name is repeated, or
    ///     <paramref name="detailNamedValues" /> are passed without a <paramref name="detailTemplateId" />.
    /// </exception>
    public Error WithErrorDetail(string? propertyPointer, string detail, string? detailTemplateId = null,
        params ReadOnlySpan<(string Key, object? Value)> detailNamedValues)
    {
        return ToBuilder()
            .AddErrorDetail(propertyPointer, detail, detailTemplateId, detailNamedValues)
            .Build();
    }

    #endregion

    #region metadata

    /// <summary>
    ///     Gets the metadata value of the specified key if it is of the type <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The expected type of the value.</typeparam>
    /// <param name="key">The key of the metadata entry.</param>
    /// <param name="value">
    ///     The value if the entry exists and is a <typeparamref name="T" />; otherwise, the default value of
    ///     <typeparamref name="T" />.
    /// </param>
    /// <returns>
    ///     <see langword="true" /> if the entry exists and its value is a <typeparamref name="T" />; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    /// <remarks>
    ///     <para>This method never throws.</para>
    ///     <para>
    ///         The match is strict: no conversion is performed (e.g., a stored <see cref="int" /> is not read as a
    ///         <see cref="long" />), and a <see langword="null" /> value is never a match.
    ///     </para>
    /// </remarks>
    public bool TryGetMetadata<T>(string key, [MaybeNullWhen(false)] out T value)
    {
        if (key is not null && MetadataDictionary.TryGetValue(key, out var raw) && raw is T typed)
        {
            value = typed;
            return true;
        }

        value = default;
        return false;
    }

    /// <summary>
    ///     Returns a copy of the current error with the specified metadata entry added, or replaced if the key
    ///     already exists.
    /// </summary>
    /// <param name="key">The key of the metadata entry.</param>
    /// <param name="value">The value of the metadata entry; it may be <see langword="null" />.</param>
    /// <returns>A new instance of the <see cref="Error" /> object; the current instance is not modified.</returns>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="key" /> is empty, whitespace-only or matches (case-insensitively) a standard problem
    ///     details member: <c>type</c>, <c>title</c>, <c>status</c>, <c>detail</c>, <c>instance</c>, <c>errors</c> or
    ///     <c>detailTemplated</c>.
    /// </exception>
    /// <exception cref="ArgumentNullException">If the <paramref name="key" /> is <see langword="null" />.</exception>
    public Error WithMetadata(string key, object? value)
    {
        return ToBuilder()
            .WithMetadata(key, value)
            .Build();
    }

    /// <summary>
    ///     Returns a copy of the current error with the specified metadata entries added; entries with existing keys
    ///     are replaced and the other existing entries are kept.
    /// </summary>
    /// <param name="metadata">The metadata entries to add or replace; values may be <see langword="null" />.</param>
    /// <returns>A new instance of the <see cref="Error" /> object; the current instance is not modified.</returns>
    /// <exception cref="ArgumentException">
    ///     If any key is empty, whitespace-only or matches (case-insensitively) a standard problem details member:
    ///     <c>type</c>, <c>title</c>, <c>status</c>, <c>detail</c>, <c>instance</c>, <c>errors</c> or
    ///     <c>detailTemplated</c>.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="metadata" /> or any key is <see langword="null" />.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public Error WithMetadata(IEnumerable<KeyValuePair<string, object?>> metadata)
    {
        return ToBuilder()
            .WithMetadata(metadata)
            .Build();
    }

    /// <summary>
    ///     Returns a copy of the current error with the specified metadata entries, given as key-value tuples, added;
    ///     entries with existing keys are replaced and the other existing entries are kept.
    /// </summary>
    /// <param name="metadata">
    ///     The metadata entries to add or replace, e.g., <c>WithMetadata(("limit", 100), ("remaining", 0))</c>; values
    ///     may be <see langword="null" />. A key repeated in the same call takes the last value.
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object; the current instance is not modified.</returns>
    /// <exception cref="ArgumentException">
    ///     If any key is empty, whitespace-only or matches (case-insensitively) a standard problem details member:
    ///     <c>type</c>, <c>title</c>, <c>status</c>, <c>detail</c>, <c>instance</c>, <c>errors</c> or
    ///     <c>detailTemplated</c>.
    /// </exception>
    /// <exception cref="ArgumentNullException">If any key is <see langword="null" />.</exception>
    public Error WithMetadata(params ReadOnlySpan<(string Key, object? Value)> metadata)
    {
        return ToBuilder()
            .WithMetadata(metadata)
            .Build();
    }

    /// <summary>
    ///     Returns a copy of the current error without any metadata.
    /// </summary>
    /// <returns>
    ///     A new instance of the <see cref="Error" /> object with an empty <see cref="Metadata" /> dictionary;
    ///     the current instance is not modified.
    /// </returns>
    public Error WithoutMetadata()
    {
        return ToBuilder()
            .WithoutMetadata()
            .Build();
    }

    /// <summary>
    ///     Returns a copy of the current error without the metadata entry of the specified key.
    /// </summary>
    /// <param name="key">The key of the metadata entry to remove.</param>
    /// <returns>
    ///     A new instance of the <see cref="Error" /> object (equal to the current one if the key is absent);
    ///     the current instance is not modified.
    /// </returns>
    /// <exception cref="ArgumentException">If the <paramref name="key" /> is empty or whitespace-only.</exception>
    /// <exception cref="ArgumentNullException">If the <paramref name="key" /> is <see langword="null" />.</exception>
    public Error WithoutMetadata(string key)
    {
        return ToBuilder()
            .WithoutMetadata(key)
            .Build();
    }

    #endregion

    #region equality

    /// <summary>
    ///     Determines whether the specified <see cref="Error" /> is equal to the current <see cref="Error" />,
    ///     comparing the <see cref="ErrorDetails" /> collection and the <see cref="Metadata" /> dictionary by their
    ///     content rather than by reference.
    /// </summary>
    /// <param name="other">The <see cref="Error" /> to compare with the current instance.</param>
    /// <returns>
    ///     <see langword="true" /> if the specified <see cref="Error" /> is equal to the current one;
    ///     otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals(Error? other)
    {
        return other is not null
               && EqualityContract == other.EqualityContract
               && Category == other.Category
               && TypeUri == other.TypeUri
               && Title == other.Title
               && Detail == other.Detail
               && DetailTemplated == other.DetailTemplated
               && InstanceUri == other.InstanceUri
               && ErrorDetailsCollection.SequenceEqual(other.ErrorDetailsCollection)
               && DictionaryHelper.Equal(MetadataDictionary, other.MetadataDictionary);
    }

    /// <summary>
    ///     Returns a hash code that is consistent with <see cref="Equals(Error)" />, incorporating the content
    ///     of the <see cref="ErrorDetails" /> collection and of the <see cref="Metadata" /> dictionary (independent of
    ///     the order of its entries).
    /// </summary>
    /// <returns>A hash code for the current <see cref="Error" />.</returns>
    public override int GetHashCode()
    {
        var hashCode = new HashCode();

        hashCode.Add(EqualityContract);
        hashCode.Add(Category);
        hashCode.Add(TypeUri);
        hashCode.Add(Title);
        hashCode.Add(Detail);
        hashCode.Add(DetailTemplated);
        hashCode.Add(InstanceUri);

        foreach (var errorDetail in ErrorDetailsCollection)
            hashCode.Add(errorDetail);

        hashCode.Add(DictionaryHelper.GetHashCode(MetadataDictionary));

        return hashCode.ToHashCode();
    }

    #endregion

    #region string representation

    /// <summary>
    ///     Appends the members of the error to the <see cref="ToString" /> output, writing the content of the
    ///     <see cref="ErrorDetails" /> collection and of the <see cref="Metadata" /> dictionary instead of their
    ///     type names.
    /// </summary>
    private bool PrintMembers(StringBuilder builder)
    {
        builder.Append("Category = ").Append(Category);
        builder.Append(", TypeUri = ").Append(TypeUri);
        builder.Append(", Title = ").Append(Title);
        builder.Append(", Detail = ").Append(Detail);
        builder.Append(", DetailTemplated = ").Append(DetailTemplated);
        builder.Append(", InstanceUri = ").Append(InstanceUri);

        builder.Append(", ErrorDetails = ");
        if (ErrorDetailsCollection.Count == 0)
        {
            builder.Append("[]");
        }
        else
        {
            builder.Append("[ ").AppendJoin(", ", ErrorDetailsCollection).Append(" ]");
        }

        builder.Append(", Metadata = ");
        DictionaryHelper.Append(builder, MetadataDictionary);

        return true;
    }

    #endregion

    #region factory methods

    private static Error Create(ErrorCategory category, ErrorUri typeUri, string title, string? detail,
        ErrorUri? instanceUri, string? detailTemplateId, ReadOnlySpan<(string Key, object? Value)> detailNamedValues)
    {
        var detailTemplated = TemplatedMessage.CreateOptional(detailTemplateId, detailNamedValues,
            nameof(detailTemplateId), nameof(detailNamedValues));

        return Create(category, typeUri, title, detail, instanceUri, detailTemplated);
    }

    private static Error Create(ErrorCategory category, ErrorUri typeUri, string title, string? detail,
        ErrorUri? instanceUri, string? detailTemplateId, IEnumerable<KeyValuePair<string, object?>>? detailNamedValues)
    {
        var detailTemplated = TemplatedMessage.CreateOptional(detailTemplateId, detailNamedValues,
            nameof(detailTemplateId), nameof(detailNamedValues));

        return Create(category, typeUri, title, detail, instanceUri, detailTemplated);
    }

    private static Error Create(ErrorCategory category, ErrorUri typeUri, string title, string? detail,
        ErrorUri? instanceUri, TemplatedMessage? detailTemplated)
    {
        return new ErrorBuilder(category, typeUri, title)
            .WithDetail(detail)
            .WithDetailTemplated(detailTemplated)
            .WithInstanceUri(instanceUri)
            .Build();
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Error" /> object of the <see cref="ErrorCategory.Conflict" /> category.
    /// </summary>
    /// <param name="typeUri">A record containing a <i>URI</i> reference that identifies the problem type.</param>
    /// <param name="title">A short, human-readable summary of the problem.</param>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    /// <param name="instanceUri">A <i>URI</i> reference that identifies the specific occurrence of the problem.</param>
    /// <param name="detailTemplateId">
    ///     A message template identifier to generate a localized detail message
    ///     (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <param name="detailNamedValues">
    ///     An optional collection of parameters (names and values) to generate a localized detail
    ///     message (the human-readable explanation specific to this occurrence of the problem); they require
    ///     <paramref name="detailTemplateId" />.
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates that the operation failed because of a data or state conflict (such as a unique constraint violation).<br />
    ///     It can be mapped to the 409 HTTP status code (Conflict) response.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="title" /> or a parameter name in <paramref name="detailNamedValues" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="title" /> or the <paramref name="detailTemplateId" /> is empty or whitespace-only, a
    ///     parameter name is repeated, or <paramref name="detailNamedValues" /> are passed without a
    ///     <paramref name="detailTemplateId" />.
    /// </exception>
    public static Error Conflict(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        params ReadOnlySpan<(string Key, object? Value)> detailNamedValues)
    {
        return Create(ErrorCategory.Conflict, typeUri, title, detail, instanceUri, detailTemplateId,
            detailNamedValues);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Error" /> object of the <see cref="ErrorCategory.Conflict" /> category.
    /// </summary>
    /// <param name="typeUri">A record containing a <i>URI</i> reference that identifies the problem type.</param>
    /// <param name="title">A short, human-readable summary of the problem.</param>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    /// <param name="instanceUri">A <i>URI</i> reference that identifies the specific occurrence of the problem.</param>
    /// <param name="detailTemplateId">
    ///     A message template identifier to generate a localized detail message
    ///     (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <param name="detailNamedValues">
    ///     An optional sequence of parameters (names and values) to generate a localized detail
    ///     message (the human-readable explanation specific to this occurrence of the problem); they require
    ///     <paramref name="detailTemplateId" />.
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates that the operation failed because of a data or state conflict (such as a unique constraint violation).<br />
    ///     It can be mapped to the 409 HTTP status code (Conflict) response.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="title" /> or a parameter name in <paramref name="detailNamedValues" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="title" /> or the <paramref name="detailTemplateId" /> is empty or whitespace-only, a
    ///     parameter name is repeated, or <paramref name="detailNamedValues" /> are passed without a
    ///     <paramref name="detailTemplateId" />.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Error Conflict(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        IEnumerable<KeyValuePair<string, object?>>? detailNamedValues = null)
    {
        return Create(ErrorCategory.Conflict, typeUri, title, detail, instanceUri, detailTemplateId,
            detailNamedValues);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Error" /> object of the <see cref="ErrorCategory.Critical" /> category.
    /// </summary>
    /// <param name="typeUri">A record containing a <i>URI</i> reference that identifies the problem type.</param>
    /// <param name="title">A short, human-readable summary of the problem.</param>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    /// <param name="instanceUri">A <i>URI</i> reference that identifies the specific occurrence of the problem.</param>
    /// <param name="detailTemplateId">
    ///     A message template identifier to generate a localized detail message
    ///     (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <param name="detailNamedValues">
    ///     An optional collection of parameters (names and values) to generate a localized detail
    ///     message (the human-readable explanation specific to this occurrence of the problem); they require
    ///     <paramref name="detailTemplateId" />.
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates an unexpected, critical error.<br />
    ///     It can be mapped to the 500 HTTP status code (Internal Server Error) response.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="title" /> or a parameter name in <paramref name="detailNamedValues" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="title" /> or the <paramref name="detailTemplateId" /> is empty or whitespace-only, a
    ///     parameter name is repeated, or <paramref name="detailNamedValues" /> are passed without a
    ///     <paramref name="detailTemplateId" />.
    /// </exception>
    public static Error Critical(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        params ReadOnlySpan<(string Key, object? Value)> detailNamedValues)
    {
        return Create(ErrorCategory.Critical, typeUri, title, detail, instanceUri, detailTemplateId,
            detailNamedValues);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Error" /> object of the <see cref="ErrorCategory.Critical" /> category.
    /// </summary>
    /// <param name="typeUri">A record containing a <i>URI</i> reference that identifies the problem type.</param>
    /// <param name="title">A short, human-readable summary of the problem.</param>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    /// <param name="instanceUri">A <i>URI</i> reference that identifies the specific occurrence of the problem.</param>
    /// <param name="detailTemplateId">
    ///     A message template identifier to generate a localized detail message
    ///     (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <param name="detailNamedValues">
    ///     An optional sequence of parameters (names and values) to generate a localized detail
    ///     message (the human-readable explanation specific to this occurrence of the problem); they require
    ///     <paramref name="detailTemplateId" />.
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates an unexpected, critical error.<br />
    ///     It can be mapped to the 500 HTTP status code (Internal Server Error) response.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="title" /> or a parameter name in <paramref name="detailNamedValues" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="title" /> or the <paramref name="detailTemplateId" /> is empty or whitespace-only, a
    ///     parameter name is repeated, or <paramref name="detailNamedValues" /> are passed without a
    ///     <paramref name="detailTemplateId" />.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Error Critical(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        IEnumerable<KeyValuePair<string, object?>>? detailNamedValues = null)
    {
        return Create(ErrorCategory.Critical, typeUri, title, detail, instanceUri, detailTemplateId,
            detailNamedValues);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Error" /> object of the <see cref="ErrorCategory.Failure" /> category.
    /// </summary>
    /// <param name="typeUri">A record containing a <i>URI</i> reference that identifies the problem type.</param>
    /// <param name="title">A short, human-readable summary of the problem.</param>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    /// <param name="instanceUri">A <i>URI</i> reference that identifies the specific occurrence of the problem.</param>
    /// <param name="detailTemplateId">
    ///     A message template identifier to generate a localized detail message
    ///     (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <param name="detailNamedValues">
    ///     An optional collection of parameters (names and values) to generate a localized detail
    ///     message (the human-readable explanation specific to this occurrence of the problem); they require
    ///     <paramref name="detailTemplateId" />.
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates an expected error that is not critical to the operation.<br />
    ///     It can be mapped to the 422 HTTP status code (Unprocessable Content) response.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="title" /> or a parameter name in <paramref name="detailNamedValues" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="title" /> or the <paramref name="detailTemplateId" /> is empty or whitespace-only, a
    ///     parameter name is repeated, or <paramref name="detailNamedValues" /> are passed without a
    ///     <paramref name="detailTemplateId" />.
    /// </exception>
    public static Error Failure(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        params ReadOnlySpan<(string Key, object? Value)> detailNamedValues)
    {
        return Create(ErrorCategory.Failure, typeUri, title, detail, instanceUri, detailTemplateId,
            detailNamedValues);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Error" /> object of the <see cref="ErrorCategory.Failure" /> category.
    /// </summary>
    /// <param name="typeUri">A record containing a <i>URI</i> reference that identifies the problem type.</param>
    /// <param name="title">A short, human-readable summary of the problem.</param>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    /// <param name="instanceUri">A <i>URI</i> reference that identifies the specific occurrence of the problem.</param>
    /// <param name="detailTemplateId">
    ///     A message template identifier to generate a localized detail message
    ///     (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <param name="detailNamedValues">
    ///     An optional sequence of parameters (names and values) to generate a localized detail
    ///     message (the human-readable explanation specific to this occurrence of the problem); they require
    ///     <paramref name="detailTemplateId" />.
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates an expected error that is not critical to the operation.<br />
    ///     It can be mapped to the 422 HTTP status code (Unprocessable Content) response.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="title" /> or a parameter name in <paramref name="detailNamedValues" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="title" /> or the <paramref name="detailTemplateId" /> is empty or whitespace-only, a
    ///     parameter name is repeated, or <paramref name="detailNamedValues" /> are passed without a
    ///     <paramref name="detailTemplateId" />.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Error Failure(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        IEnumerable<KeyValuePair<string, object?>>? detailNamedValues = null)
    {
        return Create(ErrorCategory.Failure, typeUri, title, detail, instanceUri, detailTemplateId,
            detailNamedValues);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Error" /> object of the <see cref="ErrorCategory.LimitExceeded" /> category.
    /// </summary>
    /// <param name="typeUri">A record containing a <i>URI</i> reference that identifies the problem type.</param>
    /// <param name="title">A short, human-readable summary of the problem.</param>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    /// <param name="instanceUri">A <i>URI</i> reference that identifies the specific occurrence of the problem.</param>
    /// <param name="detailTemplateId">
    ///     A message template identifier to generate a localized detail message
    ///     (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <param name="detailNamedValues">
    ///     An optional collection of parameters (names and values) to generate a localized detail
    ///     message (the human-readable explanation specific to this occurrence of the problem); they require
    ///     <paramref name="detailTemplateId" />.
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates that the request is valid and permitted, but a usage limit, quota or rate has been exhausted.
    ///     The operation may succeed later or once the limit is raised.<br />
    ///     It can be mapped to the 429 HTTP status code (Too Many Requests) response.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="title" /> or a parameter name in <paramref name="detailNamedValues" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="title" /> or the <paramref name="detailTemplateId" /> is empty or whitespace-only, a
    ///     parameter name is repeated, or <paramref name="detailNamedValues" /> are passed without a
    ///     <paramref name="detailTemplateId" />.
    /// </exception>
    public static Error LimitExceeded(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        params ReadOnlySpan<(string Key, object? Value)> detailNamedValues)
    {
        return Create(ErrorCategory.LimitExceeded, typeUri, title, detail, instanceUri, detailTemplateId,
            detailNamedValues);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Error" /> object of the <see cref="ErrorCategory.LimitExceeded" /> category.
    /// </summary>
    /// <param name="typeUri">A record containing a <i>URI</i> reference that identifies the problem type.</param>
    /// <param name="title">A short, human-readable summary of the problem.</param>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    /// <param name="instanceUri">A <i>URI</i> reference that identifies the specific occurrence of the problem.</param>
    /// <param name="detailTemplateId">
    ///     A message template identifier to generate a localized detail message
    ///     (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <param name="detailNamedValues">
    ///     An optional sequence of parameters (names and values) to generate a localized detail
    ///     message (the human-readable explanation specific to this occurrence of the problem); they require
    ///     <paramref name="detailTemplateId" />.
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates that the request is valid and permitted, but a usage limit, quota or rate has been exhausted.
    ///     The operation may succeed later or once the limit is raised.<br />
    ///     It can be mapped to the 429 HTTP status code (Too Many Requests) response.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="title" /> or a parameter name in <paramref name="detailNamedValues" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="title" /> or the <paramref name="detailTemplateId" /> is empty or whitespace-only, a
    ///     parameter name is repeated, or <paramref name="detailNamedValues" /> are passed without a
    ///     <paramref name="detailTemplateId" />.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Error LimitExceeded(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        IEnumerable<KeyValuePair<string, object?>>? detailNamedValues = null)
    {
        return Create(ErrorCategory.LimitExceeded, typeUri, title, detail, instanceUri, detailTemplateId,
            detailNamedValues);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Error" /> object of the <see cref="ErrorCategory.NotFound" /> category.
    /// </summary>
    /// <param name="typeUri">A record containing a <i>URI</i> reference that identifies the problem type.</param>
    /// <param name="title">A short, human-readable summary of the problem.</param>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    /// <param name="instanceUri">A <i>URI</i> reference that identifies the specific occurrence of the problem.</param>
    /// <param name="detailTemplateId">
    ///     A message template identifier to generate a localized detail message
    ///     (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <param name="detailNamedValues">
    ///     An optional collection of parameters (names and values) to generate a localized detail
    ///     message (the human-readable explanation specific to this occurrence of the problem); they require
    ///     <paramref name="detailTemplateId" />.
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates that the requested resource has not been found.<br />
    ///     It can be mapped to the 404 HTTP status code (Not Found) response.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="title" /> or a parameter name in <paramref name="detailNamedValues" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="title" /> or the <paramref name="detailTemplateId" /> is empty or whitespace-only, a
    ///     parameter name is repeated, or <paramref name="detailNamedValues" /> are passed without a
    ///     <paramref name="detailTemplateId" />.
    /// </exception>
    public static Error NotFound(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        params ReadOnlySpan<(string Key, object? Value)> detailNamedValues)
    {
        return Create(ErrorCategory.NotFound, typeUri, title, detail, instanceUri, detailTemplateId,
            detailNamedValues);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Error" /> object of the <see cref="ErrorCategory.NotFound" /> category.
    /// </summary>
    /// <param name="typeUri">A record containing a <i>URI</i> reference that identifies the problem type.</param>
    /// <param name="title">A short, human-readable summary of the problem.</param>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    /// <param name="instanceUri">A <i>URI</i> reference that identifies the specific occurrence of the problem.</param>
    /// <param name="detailTemplateId">
    ///     A message template identifier to generate a localized detail message
    ///     (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <param name="detailNamedValues">
    ///     An optional sequence of parameters (names and values) to generate a localized detail
    ///     message (the human-readable explanation specific to this occurrence of the problem); they require
    ///     <paramref name="detailTemplateId" />.
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates that the requested resource has not been found.<br />
    ///     It can be mapped to the 404 HTTP status code (Not Found) response.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="title" /> or a parameter name in <paramref name="detailNamedValues" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="title" /> or the <paramref name="detailTemplateId" /> is empty or whitespace-only, a
    ///     parameter name is repeated, or <paramref name="detailNamedValues" /> are passed without a
    ///     <paramref name="detailTemplateId" />.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Error NotFound(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        IEnumerable<KeyValuePair<string, object?>>? detailNamedValues = null)
    {
        return Create(ErrorCategory.NotFound, typeUri, title, detail, instanceUri, detailTemplateId,
            detailNamedValues);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Error" /> object of the <see cref="ErrorCategory.NotImplemented" /> category.
    /// </summary>
    /// <param name="typeUri">A record containing a <i>URI</i> reference that identifies the problem type.</param>
    /// <param name="title">A short, human-readable summary of the problem.</param>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    /// <param name="instanceUri">A <i>URI</i> reference that identifies the specific occurrence of the problem.</param>
    /// <param name="detailTemplateId">
    ///     A message template identifier to generate a localized detail message
    ///     (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <param name="detailNamedValues">
    ///     An optional collection of parameters (names and values) to generate a localized detail
    ///     message (the human-readable explanation specific to this occurrence of the problem); they require
    ///     <paramref name="detailTemplateId" />.
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates that the operation, feature or case is not implemented.<br />
    ///     It can be mapped to the 501 HTTP status code (Not Implemented) response.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="title" /> or a parameter name in <paramref name="detailNamedValues" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="title" /> or the <paramref name="detailTemplateId" /> is empty or whitespace-only, a
    ///     parameter name is repeated, or <paramref name="detailNamedValues" /> are passed without a
    ///     <paramref name="detailTemplateId" />.
    /// </exception>
    public static Error NotImplemented(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        params ReadOnlySpan<(string Key, object? Value)> detailNamedValues)
    {
        return Create(ErrorCategory.NotImplemented, typeUri, title, detail, instanceUri, detailTemplateId,
            detailNamedValues);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Error" /> object of the <see cref="ErrorCategory.NotImplemented" /> category.
    /// </summary>
    /// <param name="typeUri">A record containing a <i>URI</i> reference that identifies the problem type.</param>
    /// <param name="title">A short, human-readable summary of the problem.</param>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    /// <param name="instanceUri">A <i>URI</i> reference that identifies the specific occurrence of the problem.</param>
    /// <param name="detailTemplateId">
    ///     A message template identifier to generate a localized detail message
    ///     (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <param name="detailNamedValues">
    ///     An optional sequence of parameters (names and values) to generate a localized detail
    ///     message (the human-readable explanation specific to this occurrence of the problem); they require
    ///     <paramref name="detailTemplateId" />.
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates that the operation, feature or case is not implemented.<br />
    ///     It can be mapped to the 501 HTTP status code (Not Implemented) response.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="title" /> or a parameter name in <paramref name="detailNamedValues" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="title" /> or the <paramref name="detailTemplateId" /> is empty or whitespace-only, a
    ///     parameter name is repeated, or <paramref name="detailNamedValues" /> are passed without a
    ///     <paramref name="detailTemplateId" />.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Error NotImplemented(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        IEnumerable<KeyValuePair<string, object?>>? detailNamedValues = null)
    {
        return Create(ErrorCategory.NotImplemented, typeUri, title, detail, instanceUri, detailTemplateId,
            detailNamedValues);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Error" /> object of the <see cref="ErrorCategory.Timeout" /> category.
    /// </summary>
    /// <param name="typeUri">A record containing a <i>URI</i> reference that identifies the problem type.</param>
    /// <param name="title">A short, human-readable summary of the problem.</param>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    /// <param name="instanceUri">A <i>URI</i> reference that identifies the specific occurrence of the problem.</param>
    /// <param name="detailTemplateId">
    ///     A message template identifier to generate a localized detail message
    ///     (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <param name="detailNamedValues">
    ///     An optional collection of parameters (names and values) to generate a localized detail
    ///     message (the human-readable explanation specific to this occurrence of the problem); they require
    ///     <paramref name="detailTemplateId" />.
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates that the operation failed because it was not completed within the allotted time.<br />
    ///     It can be mapped to the 408 HTTP status code (Request Timeout) response.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="title" /> or a parameter name in <paramref name="detailNamedValues" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="title" /> or the <paramref name="detailTemplateId" /> is empty or whitespace-only, a
    ///     parameter name is repeated, or <paramref name="detailNamedValues" /> are passed without a
    ///     <paramref name="detailTemplateId" />.
    /// </exception>
    public static Error Timeout(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        params ReadOnlySpan<(string Key, object? Value)> detailNamedValues)
    {
        return Create(ErrorCategory.Timeout, typeUri, title, detail, instanceUri, detailTemplateId,
            detailNamedValues);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Error" /> object of the <see cref="ErrorCategory.Timeout" /> category.
    /// </summary>
    /// <param name="typeUri">A record containing a <i>URI</i> reference that identifies the problem type.</param>
    /// <param name="title">A short, human-readable summary of the problem.</param>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    /// <param name="instanceUri">A <i>URI</i> reference that identifies the specific occurrence of the problem.</param>
    /// <param name="detailTemplateId">
    ///     A message template identifier to generate a localized detail message
    ///     (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <param name="detailNamedValues">
    ///     An optional sequence of parameters (names and values) to generate a localized detail
    ///     message (the human-readable explanation specific to this occurrence of the problem); they require
    ///     <paramref name="detailTemplateId" />.
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates that the operation failed because it was not completed within the allotted time.<br />
    ///     It can be mapped to the 408 HTTP status code (Request Timeout) response.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="title" /> or a parameter name in <paramref name="detailNamedValues" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="title" /> or the <paramref name="detailTemplateId" /> is empty or whitespace-only, a
    ///     parameter name is repeated, or <paramref name="detailNamedValues" /> are passed without a
    ///     <paramref name="detailTemplateId" />.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Error Timeout(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        IEnumerable<KeyValuePair<string, object?>>? detailNamedValues = null)
    {
        return Create(ErrorCategory.Timeout, typeUri, title, detail, instanceUri, detailTemplateId,
            detailNamedValues);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Error" /> object of the <see cref="ErrorCategory.Unauthenticated" />
    ///     category.
    /// </summary>
    /// <param name="typeUri">A record containing a <i>URI</i> reference that identifies the problem type.</param>
    /// <param name="title">A short, human-readable summary of the problem.</param>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    /// <param name="instanceUri">A <i>URI</i> reference that identifies the specific occurrence of the problem.</param>
    /// <param name="detailTemplateId">
    ///     A message template identifier to generate a localized detail message
    ///     (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <param name="detailNamedValues">
    ///     An optional collection of parameters (names and values) to generate a localized detail
    ///     message (the human-readable explanation specific to this occurrence of the problem); they require
    ///     <paramref name="detailTemplateId" />.
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates that the operation has failed because the authentication is missing or invalid.<br />
    ///     It can be mapped to the 401 HTTP status code (Unauthorized) response.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="title" /> or a parameter name in <paramref name="detailNamedValues" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="title" /> or the <paramref name="detailTemplateId" /> is empty or whitespace-only, a
    ///     parameter name is repeated, or <paramref name="detailNamedValues" /> are passed without a
    ///     <paramref name="detailTemplateId" />.
    /// </exception>
    public static Error Unauthenticated(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        params ReadOnlySpan<(string Key, object? Value)> detailNamedValues)
    {
        return Create(ErrorCategory.Unauthenticated, typeUri, title, detail, instanceUri, detailTemplateId,
            detailNamedValues);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Error" /> object of the <see cref="ErrorCategory.Unauthenticated" /> category.
    /// </summary>
    /// <param name="typeUri">A record containing a <i>URI</i> reference that identifies the problem type.</param>
    /// <param name="title">A short, human-readable summary of the problem.</param>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    /// <param name="instanceUri">A <i>URI</i> reference that identifies the specific occurrence of the problem.</param>
    /// <param name="detailTemplateId">
    ///     A message template identifier to generate a localized detail message
    ///     (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <param name="detailNamedValues">
    ///     An optional sequence of parameters (names and values) to generate a localized detail
    ///     message (the human-readable explanation specific to this occurrence of the problem); they require
    ///     <paramref name="detailTemplateId" />.
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates that the operation has failed because the authentication is missing or invalid.<br />
    ///     It can be mapped to the 401 HTTP status code (Unauthorized) response.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="title" /> or a parameter name in <paramref name="detailNamedValues" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="title" /> or the <paramref name="detailTemplateId" /> is empty or whitespace-only, a
    ///     parameter name is repeated, or <paramref name="detailNamedValues" /> are passed without a
    ///     <paramref name="detailTemplateId" />.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Error Unauthenticated(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        IEnumerable<KeyValuePair<string, object?>>? detailNamedValues = null)
    {
        return Create(ErrorCategory.Unauthenticated, typeUri, title, detail, instanceUri, detailTemplateId,
            detailNamedValues);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Error" /> object of the <see cref="ErrorCategory.Unauthorized" /> category.
    /// </summary>
    /// <param name="typeUri">A record containing a <i>URI</i> reference that identifies the problem type.</param>
    /// <param name="title">A short, human-readable summary of the problem.</param>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    /// <param name="instanceUri">A <i>URI</i> reference that identifies the specific occurrence of the problem.</param>
    /// <param name="detailTemplateId">
    ///     A message template identifier to generate a localized detail message
    ///     (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <param name="detailNamedValues">
    ///     An optional collection of parameters (names and values) to generate a localized detail
    ///     message (the human-readable explanation specific to this occurrence of the problem); they require
    ///     <paramref name="detailTemplateId" />.
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates that the operation has failed because the identity does not have permission to perform it.<br />
    ///     It can be mapped to the 403 HTTP status code (Forbidden) response.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="title" /> or a parameter name in <paramref name="detailNamedValues" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="title" /> or the <paramref name="detailTemplateId" /> is empty or whitespace-only, a
    ///     parameter name is repeated, or <paramref name="detailNamedValues" /> are passed without a
    ///     <paramref name="detailTemplateId" />.
    /// </exception>
    public static Error Unauthorized(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        params ReadOnlySpan<(string Key, object? Value)> detailNamedValues)
    {
        return Create(ErrorCategory.Unauthorized, typeUri, title, detail, instanceUri, detailTemplateId,
            detailNamedValues);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Error" /> object of the <see cref="ErrorCategory.Unauthorized" /> category.
    /// </summary>
    /// <param name="typeUri">A record containing a <i>URI</i> reference that identifies the problem type.</param>
    /// <param name="title">A short, human-readable summary of the problem.</param>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    /// <param name="instanceUri">A <i>URI</i> reference that identifies the specific occurrence of the problem.</param>
    /// <param name="detailTemplateId">
    ///     A message template identifier to generate a localized detail message
    ///     (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <param name="detailNamedValues">
    ///     An optional sequence of parameters (names and values) to generate a localized detail
    ///     message (the human-readable explanation specific to this occurrence of the problem); they require
    ///     <paramref name="detailTemplateId" />.
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates that the operation has failed because the identity does not have permission to perform it.<br />
    ///     It can be mapped to the 403 HTTP status code (Forbidden) response.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="title" /> or a parameter name in <paramref name="detailNamedValues" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="title" /> or the <paramref name="detailTemplateId" /> is empty or whitespace-only, a
    ///     parameter name is repeated, or <paramref name="detailNamedValues" /> are passed without a
    ///     <paramref name="detailTemplateId" />.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Error Unauthorized(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        IEnumerable<KeyValuePair<string, object?>>? detailNamedValues = null)
    {
        return Create(ErrorCategory.Unauthorized, typeUri, title, detail, instanceUri, detailTemplateId,
            detailNamedValues);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Error" /> object of the <see cref="ErrorCategory.Unavailable" /> category.
    /// </summary>
    /// <param name="typeUri">A record containing a <i>URI</i> reference that identifies the problem type.</param>
    /// <param name="title">A short, human-readable summary of the problem.</param>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    /// <param name="instanceUri">A <i>URI</i> reference that identifies the specific occurrence of the problem.</param>
    /// <param name="detailTemplateId">
    ///     A message template identifier to generate a localized detail message
    ///     (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <param name="detailNamedValues">
    ///     An optional collection of parameters (names and values) to generate a localized detail
    ///     message (the human-readable explanation specific to this occurrence of the problem); they require
    ///     <paramref name="detailTemplateId" />.
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates that the service is not available and the operation can be retried later.<br />
    ///     It can be mapped to the 503 HTTP status code (Service Unavailable) response.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="title" /> or a parameter name in <paramref name="detailNamedValues" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="title" /> or the <paramref name="detailTemplateId" /> is empty or whitespace-only, a
    ///     parameter name is repeated, or <paramref name="detailNamedValues" /> are passed without a
    ///     <paramref name="detailTemplateId" />.
    /// </exception>
    public static Error Unavailable(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        params ReadOnlySpan<(string Key, object? Value)> detailNamedValues)
    {
        return Create(ErrorCategory.Unavailable, typeUri, title, detail, instanceUri, detailTemplateId,
            detailNamedValues);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Error" /> object of the <see cref="ErrorCategory.Unavailable" /> category.
    /// </summary>
    /// <param name="typeUri">A record containing a <i>URI</i> reference that identifies the problem type.</param>
    /// <param name="title">A short, human-readable summary of the problem.</param>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    /// <param name="instanceUri">A <i>URI</i> reference that identifies the specific occurrence of the problem.</param>
    /// <param name="detailTemplateId">
    ///     A message template identifier to generate a localized detail message
    ///     (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <param name="detailNamedValues">
    ///     An optional sequence of parameters (names and values) to generate a localized detail
    ///     message (the human-readable explanation specific to this occurrence of the problem); they require
    ///     <paramref name="detailTemplateId" />.
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates that the service is not available and the operation can be retried later.<br />
    ///     It can be mapped to the 503 HTTP status code (Service Unavailable) response.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="title" /> or a parameter name in <paramref name="detailNamedValues" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="title" /> or the <paramref name="detailTemplateId" /> is empty or whitespace-only, a
    ///     parameter name is repeated, or <paramref name="detailNamedValues" /> are passed without a
    ///     <paramref name="detailTemplateId" />.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Error Unavailable(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        IEnumerable<KeyValuePair<string, object?>>? detailNamedValues = null)
    {
        return Create(ErrorCategory.Unavailable, typeUri, title, detail, instanceUri, detailTemplateId,
            detailNamedValues);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Error" /> object of the <see cref="ErrorCategory.Validation" /> category.
    /// </summary>
    /// <param name="typeUri">A record containing a <i>URI</i> reference that identifies the problem type.</param>
    /// <param name="title">A short, human-readable summary of the problem.</param>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    /// <param name="instanceUri">A <i>URI</i> reference that identifies the specific occurrence of the problem.</param>
    /// <param name="detailTemplateId">
    ///     A message template identifier to generate a localized detail message
    ///     (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <param name="detailNamedValues">
    ///     An optional collection of parameters (names and values) to generate a localized detail
    ///     message (the human-readable explanation specific to this occurrence of the problem); they require
    ///     <paramref name="detailTemplateId" />.
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Represents validation errors that prevent the underlying service from completing.<br />
    ///     It can be mapped to the 400 HTTP status code (Bad Request) response.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="title" /> or a parameter name in <paramref name="detailNamedValues" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="title" /> or the <paramref name="detailTemplateId" /> is empty or whitespace-only, a
    ///     parameter name is repeated, or <paramref name="detailNamedValues" /> are passed without a
    ///     <paramref name="detailTemplateId" />.
    /// </exception>
    public static Error Validation(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        params ReadOnlySpan<(string Key, object? Value)> detailNamedValues)
    {
        return Create(ErrorCategory.Validation, typeUri, title, detail, instanceUri, detailTemplateId,
            detailNamedValues);
    }

    /// <summary>
    ///     Creates a new instance of the <see cref="Error" /> object of the <see cref="ErrorCategory.Validation" /> category.
    /// </summary>
    /// <param name="typeUri">A record containing a <i>URI</i> reference that identifies the problem type.</param>
    /// <param name="title">A short, human-readable summary of the problem.</param>
    /// <param name="detail">A human-readable explanation specific to this occurrence of the problem.</param>
    /// <param name="instanceUri">A <i>URI</i> reference that identifies the specific occurrence of the problem.</param>
    /// <param name="detailTemplateId">
    ///     A message template identifier to generate a localized detail message
    ///     (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <param name="detailNamedValues">
    ///     An optional collection of parameters (names and values) to generate a localized detail
    ///     message (the human-readable explanation specific to this occurrence of the problem); they require
    ///     <paramref name="detailTemplateId" />.
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Represents validation errors that prevent the underlying service from completing.<br />
    ///     It can be mapped to the 400 HTTP status code (Bad Request) response.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="title" /> or a parameter name in <paramref name="detailNamedValues" /> is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="title" /> or the <paramref name="detailTemplateId" /> is empty or whitespace-only, a
    ///     parameter name is repeated, or <paramref name="detailNamedValues" /> are passed without a
    ///     <paramref name="detailTemplateId" />.
    /// </exception>
    [OverloadResolutionPriority(1)]
    public static Error Validation(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        IEnumerable<KeyValuePair<string, object?>>? detailNamedValues = null)
    {
        return Create(ErrorCategory.Validation, typeUri, title, detail, instanceUri, detailTemplateId,
            detailNamedValues);
    }

    #endregion
}
