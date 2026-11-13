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
using Maple.Result.Validators;

namespace Maple.Result;

/// <summary>
///     A mutable builder of an immutable <see cref="Error" />.
/// </summary>
/// <remarks>
///     <para>
///         Use it to assemble an error step by step (e.g., adding one <see cref="ErrorDetail" /> per invalid field in a
///         loop) and call <see cref="Build" /> to obtain the <see cref="Error" />. Create it with the constructor, with
///         one of the static methods named after the categories (e.g., <see cref="Validation" />), or from an existing
///         error with <see cref="Error.ToBuilder" />.
///     </para>
///     <para>
///         Every argument is validated when it is passed, so <see cref="Build" /> never throws. Every method returns
///         the same builder instance to allow chaining.
///     </para>
///     <para>
///         <see cref="Build" /> takes a snapshot: later changes to the builder do not affect an <see cref="Error" />
///         already built, and the builder stays usable after <see cref="Build" />.
///     </para>
///     <para>The builder is not thread-safe.</para>
/// </remarks>
public sealed class ErrorBuilder
{
    private ErrorCategory _category;
    private ErrorUri _typeUri;
    private string _title;
    private string? _detail;
    private TemplatedMessage? _detailTemplated;
    private ErrorUri? _instanceUri;

    // Copy-on-write: the last immutable collection (shared with an Error built from or by this builder) is reused
    // until the first change, which copies it into the pending, mutable collection.
    private ReadOnlyCollection<ErrorDetail> _builtErrorDetails;
    private List<ErrorDetail>? _pendingErrorDetails;
    private ReadOnlyDictionary<string, object?> _builtMetadata;
    private Dictionary<string, object?>? _pendingMetadata;

    #region constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="ErrorBuilder" /> class with the mandatory members of an
    ///     <see cref="Error" />.
    /// </summary>
    /// <param name="category">The category of the error.</param>
    /// <param name="typeUri">A record containing a <i>URI</i> reference that identifies the problem type.</param>
    /// <param name="title">A short, human-readable summary of the problem.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     If the <paramref name="category" /> is not a defined <see cref="ErrorCategory" /> value.
    /// </exception>
    /// <exception cref="ArgumentNullException">If the <paramref name="title" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">If the <paramref name="title" /> is empty or whitespace-only.</exception>
    public ErrorBuilder(ErrorCategory category, ErrorUri typeUri, string title)
    {
        ValidateCategory(category);
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        _category = category;
        _typeUri = typeUri;
        _title = title;
        _builtErrorDetails = Error.EmptyErrorDetails;
        _builtMetadata = Error.EmptyMetadata;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="ErrorBuilder" /> class with the members of an existing
    ///     <see cref="Error" />, which were validated when it was created.
    /// </summary>
    internal ErrorBuilder(Error error)
    {
        _category = error.Category;
        _typeUri = new ErrorUri(error.TypeUri);
        _title = error.Title;
        _detail = error.Detail;
        _detailTemplated = error.DetailTemplated;
        _instanceUri = error.InstanceUri is null ? null : new ErrorUri(error.InstanceUri);
        _builtErrorDetails = error.ErrorDetailsCollection;
        _builtMetadata = error.MetadataDictionary;
    }

    #endregion

    #region factory methods

    /// <summary>
    ///     Returns a new <see cref="ErrorBuilder" /> of the <see cref="ErrorCategory.Validation" /> category.
    /// </summary>
    /// <inheritdoc cref="ErrorBuilder(ErrorCategory, ErrorUri, string)" />
    public static ErrorBuilder Validation(ErrorUri typeUri, string title)
    {
        return new ErrorBuilder(ErrorCategory.Validation, typeUri, title);
    }

    /// <summary>
    ///     Returns a new <see cref="ErrorBuilder" /> of the <see cref="ErrorCategory.Unauthenticated" /> category.
    /// </summary>
    /// <inheritdoc cref="ErrorBuilder(ErrorCategory, ErrorUri, string)" />
    public static ErrorBuilder Unauthenticated(ErrorUri typeUri, string title)
    {
        return new ErrorBuilder(ErrorCategory.Unauthenticated, typeUri, title);
    }

    /// <summary>
    ///     Returns a new <see cref="ErrorBuilder" /> of the <see cref="ErrorCategory.Unauthorized" /> category.
    /// </summary>
    /// <inheritdoc cref="ErrorBuilder(ErrorCategory, ErrorUri, string)" />
    public static ErrorBuilder Unauthorized(ErrorUri typeUri, string title)
    {
        return new ErrorBuilder(ErrorCategory.Unauthorized, typeUri, title);
    }

    /// <summary>
    ///     Returns a new <see cref="ErrorBuilder" /> of the <see cref="ErrorCategory.NotFound" /> category.
    /// </summary>
    /// <inheritdoc cref="ErrorBuilder(ErrorCategory, ErrorUri, string)" />
    public static ErrorBuilder NotFound(ErrorUri typeUri, string title)
    {
        return new ErrorBuilder(ErrorCategory.NotFound, typeUri, title);
    }

    /// <summary>
    ///     Returns a new <see cref="ErrorBuilder" /> of the <see cref="ErrorCategory.Timeout" /> category.
    /// </summary>
    /// <inheritdoc cref="ErrorBuilder(ErrorCategory, ErrorUri, string)" />
    public static ErrorBuilder Timeout(ErrorUri typeUri, string title)
    {
        return new ErrorBuilder(ErrorCategory.Timeout, typeUri, title);
    }

    /// <summary>
    ///     Returns a new <see cref="ErrorBuilder" /> of the <see cref="ErrorCategory.Conflict" /> category.
    /// </summary>
    /// <inheritdoc cref="ErrorBuilder(ErrorCategory, ErrorUri, string)" />
    public static ErrorBuilder Conflict(ErrorUri typeUri, string title)
    {
        return new ErrorBuilder(ErrorCategory.Conflict, typeUri, title);
    }

    /// <summary>
    ///     Returns a new <see cref="ErrorBuilder" /> of the <see cref="ErrorCategory.Failure" /> category.
    /// </summary>
    /// <inheritdoc cref="ErrorBuilder(ErrorCategory, ErrorUri, string)" />
    public static ErrorBuilder Failure(ErrorUri typeUri, string title)
    {
        return new ErrorBuilder(ErrorCategory.Failure, typeUri, title);
    }

    /// <summary>
    ///     Returns a new <see cref="ErrorBuilder" /> of the <see cref="ErrorCategory.Critical" /> category.
    /// </summary>
    /// <inheritdoc cref="ErrorBuilder(ErrorCategory, ErrorUri, string)" />
    public static ErrorBuilder Critical(ErrorUri typeUri, string title)
    {
        return new ErrorBuilder(ErrorCategory.Critical, typeUri, title);
    }

    /// <summary>
    ///     Returns a new <see cref="ErrorBuilder" /> of the <see cref="ErrorCategory.NotImplemented" /> category.
    /// </summary>
    /// <inheritdoc cref="ErrorBuilder(ErrorCategory, ErrorUri, string)" />
    public static ErrorBuilder NotImplemented(ErrorUri typeUri, string title)
    {
        return new ErrorBuilder(ErrorCategory.NotImplemented, typeUri, title);
    }

    /// <summary>
    ///     Returns a new <see cref="ErrorBuilder" /> of the <see cref="ErrorCategory.Unavailable" /> category.
    /// </summary>
    /// <inheritdoc cref="ErrorBuilder(ErrorCategory, ErrorUri, string)" />
    public static ErrorBuilder Unavailable(ErrorUri typeUri, string title)
    {
        return new ErrorBuilder(ErrorCategory.Unavailable, typeUri, title);
    }

    /// <summary>
    ///     Returns a new <see cref="ErrorBuilder" /> of the <see cref="ErrorCategory.LimitExceeded" /> category.
    /// </summary>
    /// <inheritdoc cref="ErrorBuilder(ErrorCategory, ErrorUri, string)" />
    public static ErrorBuilder LimitExceeded(ErrorUri typeUri, string title)
    {
        return new ErrorBuilder(ErrorCategory.LimitExceeded, typeUri, title);
    }

    #endregion

    #region setters

    /// <summary>
    ///     Sets the <see cref="Error.Category" />.
    /// </summary>
    /// <param name="category">The category of the error.</param>
    /// <returns>The current <see cref="ErrorBuilder" /> instance.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     If the <paramref name="category" /> is not a defined <see cref="ErrorCategory" /> value.
    /// </exception>
    public ErrorBuilder WithCategory(ErrorCategory category)
    {
        ValidateCategory(category);

        _category = category;
        return this;
    }

    /// <summary>
    ///     Sets the <see cref="Error.TypeUri" />.
    /// </summary>
    /// <param name="typeUri">A record containing a <i>URI</i> reference that identifies the problem type.</param>
    /// <returns>The current <see cref="ErrorBuilder" /> instance.</returns>
    public ErrorBuilder WithTypeUri(ErrorUri typeUri)
    {
        _typeUri = typeUri;
        return this;
    }

    /// <summary>
    ///     Sets the <see cref="Error.Title" />.
    /// </summary>
    /// <param name="title">A short, human-readable summary of the problem.</param>
    /// <returns>The current <see cref="ErrorBuilder" /> instance.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="title" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">If the <paramref name="title" /> is empty or whitespace-only.</exception>
    public ErrorBuilder WithTitle(string title)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);

        _title = title;
        return this;
    }

    /// <summary>
    ///     Sets the <see cref="Error.Detail" />: the human-readable explanation specific to this occurrence of the
    ///     problem (the <i>RFC 9457</i> “detail” member).
    /// </summary>
    /// <param name="detail">The explanation, or <see langword="null" /> to clear it.</param>
    /// <returns>The current <see cref="ErrorBuilder" /> instance.</returns>
    /// <remarks>
    ///     To add an individual error occurrence (an <see cref="ErrorDetail" />), use <see cref="AddErrorDetail(ErrorDetail)" />.
    /// </remarks>
    public ErrorBuilder WithDetail(string? detail)
    {
        _detail = detail;
        return this;
    }

    /// <summary>
    ///     Sets the <see cref="Error.DetailTemplated" />.
    /// </summary>
    /// <param name="detailTemplated">The templated explanation, or <see langword="null" /> to clear it.</param>
    /// <returns>The current <see cref="ErrorBuilder" /> instance.</returns>
    /// <remarks>
    ///     A bare <see langword="null" /> literal is ambiguous between the overloads of this method; to clear the
    ///     value, write <c>WithDetailTemplated((TemplatedMessage?)null)</c>.
    /// </remarks>
    public ErrorBuilder WithDetailTemplated(TemplatedMessage? detailTemplated)
    {
        _detailTemplated = detailTemplated;
        return this;
    }

    /// <summary>
    ///     Sets the <see cref="Error.DetailTemplated" /> to a new <see cref="TemplatedMessage" />.
    /// </summary>
    /// <param name="templateId">The identifier of the message template.</param>
    /// <param name="namedValues">
    ///     The parameters (names and values) that might be required to generate a localized message.
    /// </param>
    /// <returns>The current <see cref="ErrorBuilder" /> instance.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="templateId" />, the <paramref name="namedValues" /> or any name is
    ///     <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="templateId" /> is empty or whitespace-only, or a name is repeated.
    /// </exception>
    public ErrorBuilder WithDetailTemplated(string templateId, params (string Key, object? Value)[] namedValues)
    {
        ArgumentNullException.ThrowIfNull(namedValues);

        _detailTemplated = new TemplatedMessage(templateId, namedValues.ToDictionary(x => x.Key, x => x.Value));
        return this;
    }

    /// <summary>
    ///     Sets the <see cref="Error.InstanceUri" />.
    /// </summary>
    /// <param name="instanceUri">
    ///     A <i>URI</i> reference that identifies the specific occurrence of the problem, or <see langword="null" />
    ///     to clear it.
    /// </param>
    /// <returns>The current <see cref="ErrorBuilder" /> instance.</returns>
    public ErrorBuilder WithInstanceUri(ErrorUri? instanceUri)
    {
        _instanceUri = instanceUri;
        return this;
    }

    #endregion

    #region error details

    /// <summary>
    ///     Appends an individual error occurrence to the <see cref="Error.ErrorDetails" />.
    /// </summary>
    /// <param name="errorDetail">The error occurrence to append.</param>
    /// <returns>The current <see cref="ErrorBuilder" /> instance.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="errorDetail" /> is <see langword="null" />.</exception>
    public ErrorBuilder AddErrorDetail(ErrorDetail errorDetail)
    {
        ArgumentNullException.ThrowIfNull(errorDetail);

        GetPendingErrorDetails().Add(errorDetail);
        return this;
    }

    /// <summary>
    ///     Appends an individual error occurrence to the <see cref="Error.ErrorDetails" />.
    /// </summary>
    /// <param name="propertyPointer">The JSON Pointer which identifies the invalid value in the input data.</param>
    /// <param name="detail">The human-readable explanation specific to this individual error occurrence.</param>
    /// <param name="messageId">
    ///     The identifier of the message template; when <see langword="null" />, no
    ///     <see cref="ErrorDetail.DetailTemplated" /> is set and the <paramref name="namedValues" /> are ignored.
    /// </param>
    /// <param name="namedValues">
    ///     The optional collection of parameters (names and values) that might be required to generate
    ///     a localized message.
    /// </param>
    /// <returns>The current <see cref="ErrorBuilder" /> instance.</returns>
    /// <remarks>
    ///     For more details about JSON Pointers, see: <seealso href="https://datatracker.ietf.org/doc/html/rfc6901" />.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="detail" /> is <see langword="null" />, or the <paramref name="messageId" /> is set
    ///     and a name is <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="messageId" /> is empty or whitespace-only, or a name is repeated.
    /// </exception>
    public ErrorBuilder AddErrorDetail(string? propertyPointer, string detail, string? messageId = null,
        params (string key, object? value)[] namedValues)
    {
        var templatedMessage = messageId is null
            ? null
            : new TemplatedMessage(messageId, namedValues.ToDictionary(x => x.key, x => x.value));

        return AddErrorDetail(new ErrorDetail(propertyPointer, detail, templatedMessage));
    }

    #endregion

    #region metadata

    /// <summary>
    ///     Adds the specified entry to the <see cref="Error.Metadata" />, or replaces its value if the key already
    ///     exists.
    /// </summary>
    /// <param name="key">The key of the metadata entry.</param>
    /// <param name="value">The value of the metadata entry; it may be <see langword="null" />.</param>
    /// <returns>The current <see cref="ErrorBuilder" /> instance.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="key" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="key" /> is empty, whitespace-only or matches (case-insensitively) a standard problem
    ///     details member: <c>type</c>, <c>title</c>, <c>status</c>, <c>detail</c>, <c>instance</c>, <c>errors</c> or
    ///     <c>detailTemplated</c>.
    /// </exception>
    public ErrorBuilder WithMetadata(string key, object? value)
    {
        MetadataKeyValidator.Validate(key);

        GetPendingMetadata()[key] = value;
        return this;
    }

    /// <summary>
    ///     Adds the specified entries to the <see cref="Error.Metadata" />; entries with existing keys are replaced
    ///     and the other existing entries are kept.
    /// </summary>
    /// <param name="metadata">The metadata entries to add or replace; values may be <see langword="null" />.</param>
    /// <returns>The current <see cref="ErrorBuilder" /> instance.</returns>
    /// <remarks>
    ///     All keys are validated before any entry is applied, so the builder is unchanged when this method throws.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="metadata" /> or any key is <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    ///     If any key is empty, whitespace-only or matches (case-insensitively) a standard problem details member:
    ///     <c>type</c>, <c>title</c>, <c>status</c>, <c>detail</c>, <c>instance</c>, <c>errors</c> or
    ///     <c>detailTemplated</c>.
    /// </exception>
    public ErrorBuilder WithMetadata(IEnumerable<KeyValuePair<string, object?>> metadata)
    {
        ArgumentNullException.ThrowIfNull(metadata);

        var entries = metadata.ToArray();
        foreach (var pair in entries)
            MetadataKeyValidator.Validate(pair.Key);

        if (entries.Length == 0)
            return this;

        var pending = GetPendingMetadata();
        foreach (var pair in entries)
            pending[pair.Key] = pair.Value;

        return this;
    }

    /// <summary>
    ///     Removes the entry of the specified key from the <see cref="Error.Metadata" />.
    /// </summary>
    /// <param name="key">The key of the metadata entry to remove; an absent key is not an error.</param>
    /// <returns>The current <see cref="ErrorBuilder" /> instance.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="key" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">If the <paramref name="key" /> is empty or whitespace-only.</exception>
    public ErrorBuilder WithoutMetadata(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        IReadOnlyDictionary<string, object?> current = _pendingMetadata ?? (IReadOnlyDictionary<string, object?>)_builtMetadata;
        if (current.ContainsKey(key))
            GetPendingMetadata().Remove(key);

        return this;
    }

    /// <summary>
    ///     Removes all entries from the <see cref="Error.Metadata" />.
    /// </summary>
    /// <returns>The current <see cref="ErrorBuilder" /> instance.</returns>
    public ErrorBuilder WithoutMetadata()
    {
        _builtMetadata = Error.EmptyMetadata;
        _pendingMetadata = null;

        return this;
    }

    #endregion

    /// <summary>
    ///     Returns a new <see cref="Error" /> with the current members of the builder.
    /// </summary>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     This method never throws. The returned error is a snapshot: later changes to the builder do not affect it,
    ///     and the builder can still be used.
    /// </remarks>
    public Error Build()
    {
        if (_pendingErrorDetails is not null)
        {
            _builtErrorDetails = _pendingErrorDetails.Count == 0
                ? Error.EmptyErrorDetails
                : new ReadOnlyCollection<ErrorDetail>(_pendingErrorDetails.ToArray());
            _pendingErrorDetails = null;
        }

        if (_pendingMetadata is not null)
        {
            _builtMetadata = _pendingMetadata.Count == 0
                ? Error.EmptyMetadata
                : new ReadOnlyDictionary<string, object?>(
                    new Dictionary<string, object?>(_pendingMetadata, StringComparer.Ordinal));
            _pendingMetadata = null;
        }

        return new Error(_category, _typeUri.Value, _title, _detail, _detailTemplated, _instanceUri?.Value,
            _builtErrorDetails, _builtMetadata);
    }

    private List<ErrorDetail> GetPendingErrorDetails()
    {
        return _pendingErrorDetails ??= [.. _builtErrorDetails];
    }

    private Dictionary<string, object?> GetPendingMetadata()
    {
        return _pendingMetadata ??= new Dictionary<string, object?>(_builtMetadata, StringComparer.Ordinal);
    }

    private static void ValidateCategory(ErrorCategory category)
    {
        if (!Enum.IsDefined(category))
        {
            throw new ArgumentOutOfRangeException(nameof(category), category,
                $"The error category ‘{category}’ is not defined.");
        }
    }
}
