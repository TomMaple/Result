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
using System.Linq;

namespace Maple.Result;

/// <summary>
///     The error with details.
/// </summary>
/// <remarks>
///     Based on the <i>RFC 9457</i>;
///     for more information, see: <seealso href="https://datatracker.ietf.org/doc/html/rfc9457" />.
/// </remarks>
public record Error
{
    private readonly List<ErrorDetail> _errorDetails = [];

    #region constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="Error" /> record with default values.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This constructor is intended only for deserialization purposes,
    ///         and it should not be used directly in code.
    ///     </para>
    ///     <para>
    ///         To create a new instance of the <see cref="Error" /> object, use one of the factory methods provided
    ///         (e.g., <c>Validation</c>, <c>Unauthenticated</c>, <c>NotFound</c>).
    ///     </para>
    /// </remarks>
    public Error()
    {
        Category = ErrorCategory.Critical;
        TypeUri = "about:blank";
        Title = "Unknown error";
        Detail = null;
        InstanceUri = null;
        DetailTemplated = null;
        _errorDetails = [];
    }

    internal Error(ErrorCategory category, string typeUri, string title, string? detail = null,
        TemplatedMessage? detailTemplated = null, string? instanceUri = null,
        IReadOnlyList<ErrorDetail>? errorDetails = null)
    {
        Category = category;
        TypeUri = typeUri;
        Title = title;
        Detail = detail;
        InstanceUri = instanceUri;
        DetailTemplated = detailTemplated;
        _errorDetails = [];

        if (errorDetails is not null)
            _errorDetails.AddRange(errorDetails);
    }

    #endregion

    /// <summary>
    ///     The category of the error
    /// </summary>
    public ErrorCategory Category { get; init; }

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
    public string TypeUri { get; init; }

    /// <summary>
    ///     The short, human-readable summary of the problem type.
    /// </summary>
    /// <remarks>
    ///     It SHOULD NOT change from occurrence to occurrence of the problem, except for localization.
    ///     The “title” string is advisory and is included only for users who are unaware of and
    ///     cannot discover the semantics of the type <i>URI</i> (e.g., during offline log analysis).
    /// </remarks>
    public string Title { get; init; }

    /// <summary>
    ///     The human-readable explanation specific to this occurrence of the problem.
    /// </summary>
    /// <remarks>
    ///     The “detail” string, if present, ought to focus on helping the client correct the problem, rather than giving
    ///     debugging information.
    ///     Consumers SHOULD NOT parse the “detail” member for information; extensions are more suitable and less error-prone
    ///     ways to obtain such information.
    /// </remarks>
    public string? Detail { get; init; }

    /// <summary>
    ///     The templated explanation specific to this occurrence of the problem.
    /// </summary>
    /// <remarks>
    ///     This property contains the message template identifier and the optional collection of parameters (names and values)
    ///     that might be required to generate a localized message.
    /// </remarks>
    public TemplatedMessage? DetailTemplated { get; init; }

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
    public string? InstanceUri { get; init; }

    /// <summary>
    ///     The collection of individual error occurrences found, with details and a pointer to the location of each
    ///     (if applicable).
    /// </summary>
    public IReadOnlyList<ErrorDetail> ErrorDetails
    {
        get => _errorDetails;
        init => _errorDetails = [.. value];
    }

    /// <summary>
    ///     Adds details of an individual error occurrence found.
    /// </summary>
    /// <param name="propertyPointer">The JSON Pointer which identifies the invalid value in the input data.</param>
    /// <param name="detail">The human-readable explanation specific to this individual error occurrence.</param>
    /// <param name="messageId">The identifier of the message template.</param>
    /// <param name="namedValues">
    ///     The optional collection of parameters (names and values) that might be required to generate
    ///     a localized message.
    /// </param>
    /// <remarks>
    ///     For more details about JSON Pointers, see: <seealso href="https://datatracker.ietf.org/doc/html/rfc6901" />.
    /// </remarks>
    /// <returns>
    ///     The current instance of the <see cref="Error" /> object.
    /// </returns>
    public Error AddDetail(string? propertyPointer, string detail, string? messageId = null,
        params (string key, object value)[] namedValues)
    {
        var templatedMessage = messageId is null
            ? null
            : new TemplatedMessage(messageId, namedValues.ToDictionary(x => x.key, x => x.value));

        var errorDetail = new ErrorDetail(propertyPointer, detail, templatedMessage);
        _errorDetails.Add(errorDetail);

        return this;
    }

    internal Error AddDetail(ErrorDetail errorDetail)
    {
        _errorDetails.Add(errorDetail);
        return this;
    }

    #region factory methods

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
    ///     message (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Represents validation errors that prevent the underlying service from completing.<br />
    ///     It can be mapped to the 400 HTTP status code (Bad Request) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> or <paramref name="instanceUri" /> contains a value
    ///     that is not a valid HTTP/HTTPS <i>URI</i> or a valid <i>URI Tag</i>.
    /// </exception>
    public static Error Validation(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        params (string Key, object Value)[] detailNamedValues)
    {
        return new Error(
            ErrorCategory.Validation,
            typeUri.Value,
            title,
            detail,
            detailTemplateId is null
                ? null
                : new TemplatedMessage(detailTemplateId, detailNamedValues.ToDictionary(x => x.Key, x => x.Value)),
            instanceUri?.Value);
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
    ///     message (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Represents validation errors that prevent the underlying service from completing.<br />
    ///     It can be mapped to the 400 HTTP status code (Bad Request) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> or <paramref name="instanceUri" /> contains a value
    ///     that is not a valid HTTP/HTTPS <i>URI</i> or a valid <i>URI Tag</i>.
    /// </exception>
    public static Error Validation(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        IEnumerable<KeyValuePair<string, object>>? detailNamedValues = null)
    {
        return new Error(
            ErrorCategory.Validation,
            typeUri.Value,
            title,
            detail,
            detailTemplateId is null
                ? null
                : new TemplatedMessage(detailTemplateId, detailNamedValues?.ToDictionary(x => x.Key, x => x.Value)),
            instanceUri?.Value);
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
    ///     message (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates that the operation has failed because the lack or failed authentication.<br />
    ///     It can be mapped to the 401 HTTP status code (Unauthorized) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> or <paramref name="instanceUri" /> contains a value
    ///     that is not a valid HTTP/HTTP <i>URI</i> or a valid <i>URI Tag</i>.
    /// </exception>
    public static Error Unauthenticated(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        params (string Key, object Value)[] detailNamedValues)
    {
        return new Error(
            ErrorCategory.Unauthenticated,
            typeUri.Value,
            title,
            detail,
            detailTemplateId is null
                ? null
                : new TemplatedMessage(detailTemplateId, detailNamedValues.ToDictionary(x => x.Key, x => x.Value)),
            instanceUri?.Value);
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
    ///     message (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Represents validation errors that prevent the underlying service from completing.<br />
    ///     It can be mapped to the 401 HTTP status code (Unauthorized) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> or <paramref name="instanceUri" /> contains a value
    ///     that is not a valid HTTP/HTTPS <i>URI</i> or a valid <i>URI Tag</i>.
    /// </exception>
    public static Error Unauthenticated(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        IEnumerable<KeyValuePair<string, object>>? detailNamedValues = null)
    {
        return new Error(
            ErrorCategory.Unauthenticated,
            typeUri.Value,
            title,
            detail,
            detailTemplateId is null
                ? null
                : new TemplatedMessage(detailTemplateId, detailNamedValues?.ToDictionary(x => x.Key, x => x.Value)),
            instanceUri?.Value);
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
    ///     message (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates that the operation has failed because the identity does not have permission to perform it.<br />
    ///     It can be mapped to the 403 HTTP status code (Forbidden) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> or <paramref name="instanceUri" /> contains a value
    ///     that is not a valid HTTP/HTTP <i>URI</i> or a valid <i>URI Tag</i>.
    /// </exception>
    public static Error Unauthorized(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        params (string Key, object Value)[] detailNamedValues)
    {
        return new Error(ErrorCategory.Unauthorized,
            typeUri.Value,
            title,
            detail,
            detailTemplateId is null
                ? null
                : new TemplatedMessage(detailTemplateId, detailNamedValues.ToDictionary(x => x.Key, x => x.Value)),
            instanceUri?.Value);
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
    ///     message (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Represents validation errors that prevent the underlying service from completing.<br />
    ///     It can be mapped to the 403 HTTP status code (Forbidden) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> or <paramref name="instanceUri" /> contains a value
    ///     that is not a valid HTTP/HTTPS <i>URI</i> or a valid <i>URI Tag</i>.
    /// </exception>
    public static Error Unauthorized(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        IEnumerable<KeyValuePair<string, object>>? detailNamedValues = null)
    {
        return new Error(
            ErrorCategory.Unauthorized,
            typeUri.Value,
            title,
            detail,
            detailTemplateId is null
                ? null
                : new TemplatedMessage(detailTemplateId, detailNamedValues?.ToDictionary(x => x.Key, x => x.Value)),
            instanceUri?.Value);
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
    ///     message (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates that the requested resource has not been found.<br />
    ///     It can be mapped to the 404 HTTP status code (Not Found) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> or <paramref name="instanceUri" /> contains a value
    ///     that is not a valid HTTP/HTTP <i>URI</i> or a valid <i>URI Tag</i>.
    /// </exception>
    public static Error NotFound(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        params (string Key, object Value)[] detailNamedValues)
    {
        return new Error(
            ErrorCategory.NotFound,
            typeUri.Value,
            title,
            detail,
            detailTemplateId is null
                ? null
                : new TemplatedMessage(detailTemplateId, detailNamedValues.ToDictionary(x => x.Key, x => x.Value)),
            instanceUri?.Value);
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
    ///     message (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Represents validation errors that prevent the underlying service from completing.<br />
    ///     It can be mapped to the 404 HTTP status code (Not Found) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> or <paramref name="instanceUri" /> contains a value
    ///     that is not a valid HTTP/HTTPS <i>URI</i> or a valid <i>URI Tag</i>.
    /// </exception>
    public static Error NotFound(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        IEnumerable<KeyValuePair<string, object>>? detailNamedValues = null)
    {
        return new Error(
            ErrorCategory.NotFound,
            typeUri.Value,
            title,
            detail,
            detailTemplateId is null
                ? null
                : new TemplatedMessage(detailTemplateId, detailNamedValues?.ToDictionary(x => x.Key, x => x.Value)),
            instanceUri?.Value);
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
    ///     message (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates that the operation failed because it was not completed within the allotted time.<br />
    ///     It can be mapped to the 408 HTTP status code (Request Timeout) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> or <paramref name="instanceUri" /> contains a value
    ///     that is not a valid HTTP/HTTP <i>URI</i> or a valid <i>URI Tag</i>.
    /// </exception>
    public static Error Timeout(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        params (string Key, object Value)[] detailNamedValues)
    {
        return new Error(
            ErrorCategory.Timeout,
            typeUri.Value,
            title,
            detail,
            detailTemplateId is null
                ? null
                : new TemplatedMessage(detailTemplateId, detailNamedValues.ToDictionary(x => x.Key, x => x.Value)),
            instanceUri?.Value);
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
    ///     message (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Represents validation errors that prevent the underlying service from completing.<br />
    ///     It can be mapped to the 408 HTTP status code (Request Timeout) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> or <paramref name="instanceUri" /> contains a value
    ///     that is not a valid HTTP/HTTPS <i>URI</i> or a valid <i>URI Tag</i>.
    /// </exception>
    public static Error Timeout(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        IEnumerable<KeyValuePair<string, object>>? detailNamedValues = null)
    {
        return new Error(
            ErrorCategory.Timeout,
            typeUri.Value,
            title,
            detail,
            detailTemplateId is null
                ? null
                : new TemplatedMessage(detailTemplateId, detailNamedValues?.ToDictionary(x => x.Key, x => x.Value)),
            instanceUri?.Value);
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
    ///     message (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates that the operation failed because it was not completed within the allotted time.<br />
    ///     It can be mapped to the 409 HTTP status code (Conflict) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> or <paramref name="instanceUri" /> contains a value
    ///     that is not a valid HTTP/HTTP <i>URI</i> or a valid <i>URI Tag</i>.
    /// </exception>
    public static Error Conflict(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        params (string Key, object Value)[] detailNamedValues)
    {
        return new Error(
            ErrorCategory.Conflict,
            typeUri.Value,
            title,
            detail,
            detailTemplateId is null
                ? null
                : new TemplatedMessage(detailTemplateId, detailNamedValues.ToDictionary(x => x.Key, x => x.Value)),
            instanceUri?.Value);
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
    ///     message (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Represents validation errors that prevent the underlying service from completing.<br />
    ///     It can be mapped to the 409 HTTP status code (Conflict) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> or <paramref name="instanceUri" /> contains a value
    ///     that is not a valid HTTP/HTTPS <i>URI</i> or a valid <i>URI Tag</i>.
    /// </exception>
    public static Error Conflict(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        IEnumerable<KeyValuePair<string, object>>? detailNamedValues = null)
    {
        return new Error(
            ErrorCategory.Conflict,
            typeUri.Value,
            title,
            detail,
            detailTemplateId is null
                ? null
                : new TemplatedMessage(detailTemplateId, detailNamedValues?.ToDictionary(x => x.Key, x => x.Value)),
            instanceUri?.Value);
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
    ///     message (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates that the operation failed because it was not completed within the allotted time.<br />
    ///     It can be mapped to the 422 HTTP status code (Unprocessable Content) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> or <paramref name="instanceUri" /> contains a value
    ///     that is not a valid HTTP/HTTP <i>URI</i> or a valid <i>URI Tag</i>.
    /// </exception>
    public static Error Failure(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        params (string Key, object Value)[] detailNamedValues)
    {
        return new Error(
            ErrorCategory.Failure,
            typeUri.Value,
            title,
            detail,
            detailTemplateId is null
                ? null
                : new TemplatedMessage(detailTemplateId, detailNamedValues.ToDictionary(x => x.Key, x => x.Value)),
            instanceUri?.Value);
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
    ///     message (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Represents validation errors that prevent the underlying service from completing.<br />
    ///     It can be mapped to the 422 HTTP status code (Unprocessable Content) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> or <paramref name="instanceUri" /> contains a value
    ///     that is not a valid HTTP/HTTPS <i>URI</i> or a valid <i>URI Tag</i>.
    /// </exception>
    public static Error Failure(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        IEnumerable<KeyValuePair<string, object>>? detailNamedValues = null)
    {
        return new Error(
            ErrorCategory.Failure,
            typeUri.Value,
            title,
            detail,
            detailTemplateId is null
                ? null
                : new TemplatedMessage(detailTemplateId, detailNamedValues?.ToDictionary(x => x.Key, x => x.Value)),
            instanceUri?.Value);
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
    ///     message (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates that the operation failed because it was not completed within the allotted time.<br />
    ///     It can be mapped to the 500 HTTP status code (Internal Server Error) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> or <paramref name="instanceUri" /> contains a value
    ///     that is not a valid HTTP/HTTP <i>URI</i> or a valid <i>URI Tag</i>.
    /// </exception>
    public static Error Critical(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        params (string Key, object Value)[] detailNamedValues)
    {
        return new Error(
            ErrorCategory.Critical,
            typeUri.Value,
            title,
            detail,
            detailTemplateId is null
                ? null
                : new TemplatedMessage(detailTemplateId, detailNamedValues.ToDictionary(x => x.Key, x => x.Value)),
            instanceUri?.Value);
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
    ///     message (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Represents validation errors that prevent the underlying service from completing.<br />
    ///     It can be mapped to the 500 HTTP status code (Internal Server Error) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> or <paramref name="instanceUri" /> contains a value
    ///     that is not a valid HTTP/HTTPS <i>URI</i> or a valid <i>URI Tag</i>.
    /// </exception>
    public static Error Critical(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        IEnumerable<KeyValuePair<string, object>>? detailNamedValues = null)
    {
        return new Error(
            ErrorCategory.Critical,
            typeUri.Value,
            title,
            detail,
            detailTemplateId is null
                ? null
                : new TemplatedMessage(detailTemplateId, detailNamedValues?.ToDictionary(x => x.Key, x => x.Value)),
            instanceUri?.Value);
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
    ///     message (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates that the operation failed because it was not completed within the allotted time.<br />
    ///     It can be mapped to the 501 HTTP status code (Not Implemented) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> or <paramref name="instanceUri" /> contains a value
    ///     that is not a valid HTTP/HTTP <i>URI</i> or a valid <i>URI Tag</i>.
    /// </exception>
    public static Error NotImplemented(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        params (string Key, object Value)[] detailNamedValues)
    {
        return new Error(
            ErrorCategory.NotImplemented,
            typeUri.Value,
            title,
            detail,
            detailTemplateId is null
                ? null
                : new TemplatedMessage(detailTemplateId, detailNamedValues.ToDictionary(x => x.Key, x => x.Value)),
            instanceUri?.Value);
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
    ///     message (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Represents validation errors that prevent the underlying service from completing.<br />
    ///     It can be mapped to the 501 HTTP status code (Not Implemented) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> or <paramref name="instanceUri" /> contains a value
    ///     that is not a valid HTTP/HTTPS <i>URI</i> or a valid <i>URI Tag</i>.
    /// </exception>
    public static Error NotImplemented(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        IEnumerable<KeyValuePair<string, object>>? detailNamedValues = null)
    {
        return new Error(
            ErrorCategory.NotImplemented,
            typeUri.Value,
            title,
            detail,
            detailTemplateId is null
                ? null
                : new TemplatedMessage(detailTemplateId, detailNamedValues?.ToDictionary(x => x.Key, x => x.Value)),
            instanceUri?.Value);
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
    ///     message (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates that the operation failed because it was not completed within the allotted time.<br />
    ///     It can be mapped to the 503 HTTP status code (Service Unavailable) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> or <paramref name="instanceUri" /> contains a value
    ///     that is not a valid HTTP/HTTP <i>URI</i> or a valid <i>URI Tag</i>.
    /// </exception>
    public static Error Unavailable(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        params (string Key, object Value)[] detailNamedValues)
    {
        return new Error(
            ErrorCategory.Unavailable,
            typeUri.Value,
            title,
            detail,
            detailTemplateId is null
                ? null
                : new TemplatedMessage(detailTemplateId, detailNamedValues.ToDictionary(x => x.Key, x => x.Value)),
            instanceUri?.Value);
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
    ///     message (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Represents validation errors that prevent the underlying service from completing.<br />
    ///     It can be mapped to the 503 HTTP status code (Service Unavailable) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> or <paramref name="instanceUri" /> contains a value
    ///     that is not a valid HTTP/HTTPS <i>URI</i> or a valid <i>URI Tag</i>.
    /// </exception>
    public static Error Unavailable(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        IEnumerable<KeyValuePair<string, object>>? detailNamedValues = null)
    {
        return new Error(
            ErrorCategory.Unavailable,
            typeUri.Value,
            title,
            detail,
            detailTemplateId is null
                ? null
                : new TemplatedMessage(detailTemplateId, detailNamedValues?.ToDictionary(x => x.Key, x => x.Value)),
            instanceUri?.Value);
    }

    #endregion
}
