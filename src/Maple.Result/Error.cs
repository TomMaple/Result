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
using System.Buffers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using Maple.Result.Validators;

namespace Maple.Result;

/// <summary>
///     The error with details.
/// </summary>
/// <remarks>
///     Based on the <i>RFC 9457</i>;
///     for more information, see: <seealso href="https://datatracker.ietf.org/doc/html/rfc9457" />.
/// </remarks>
public sealed record Error
{
    #region consts

    private static readonly ReadOnlyDictionary<string, object> EmptyMetadata =
        new(new Dictionary<string, object>(StringComparer.Ordinal));

    #endregion

    private readonly List<ErrorDetail> _errorDetails = [];

    // Never modified after construction, so instances (e.g., `with` copies) can share it safely.
    private readonly ReadOnlyDictionary<string, object> _metadata = EmptyMetadata;

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
        _metadata = EmptyMetadata;
    }

    internal Error(ErrorCategory category, string typeUri, string title, string? detail = null,
        TemplatedMessage? detailTemplated = null, string? instanceUri = null,
        IReadOnlyList<ErrorDetail>? errorDetails = null,
        IEnumerable<KeyValuePair<string, object>>? metadata = null)
    {
        if (string.IsNullOrWhiteSpace(typeUri))
        {
            throw new ArgumentException(
                "The type URI is required. Use ErrorUri.None(), ErrorUri.Locator() or ErrorUri.Tag() to create it.",
                nameof(typeUri));
        }

        Category = category;
        TypeUri = typeUri;
        Title = title;
        Detail = detail;
        InstanceUri = instanceUri;
        DetailTemplated = detailTemplated;
        _errorDetails = [];
        _metadata = CreateMetadata(metadata);

        if (errorDetails is not null)
            _errorDetails.AddRange(errorDetails);
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Error" /> record by copying the state of an existing instance.
    /// </summary>
    /// <remarks>
    ///     This copy constructor is used by <c>with</c> expressions. It deep-copies the mutable
    ///     <see cref="ErrorDetails" /> collection so that a copy and its original do not share the same underlying
    ///     list; otherwise mutating one (e.g., via <see cref="AddDetail(string?, string, string?, ValueTuple{string, object}[])" />)
    ///     would also mutate the other. The <see cref="Metadata" /> dictionary is read-only and never modified, so it is
    ///     shared rather than copied.
    /// </remarks>
    /// <param name="original">The <see cref="Error" /> instance to copy.</param>
    private Error(Error original)
    {
        Category = original.Category;
        TypeUri = original.TypeUri;
        Title = original.Title;
        Detail = original.Detail;
        DetailTemplated = original.DetailTemplated;
        InstanceUri = original.InstanceUri;
        _errorDetails = [.. original._errorDetails];
        _metadata = original._metadata;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Error" /> record as a copy of an existing instance with
    ///     the specified metadata, without copying the metadata again.
    /// </summary>
    private Error(Error original, ReadOnlyDictionary<string, object> metadata) : this(original)
    {
        _metadata = metadata;
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
    ///         <c>detailTemplated</c>; <see cref="WithMetadata(string, object)" /> and the <c>init</c> setter (also during
    ///         deserialization) reject them.
    ///     </para>
    ///     <para>
    ///         Values must not be <see langword="null" />; <see cref="WithMetadata(string, object)" /> and the
    ///         <c>init</c> setter (also during deserialization, e.g., <c>{"key": null}</c>) reject them.
    ///         Values should be JSON primitives (strings, numbers, booleans). After deserialization, values are
    ///         returned as <see cref="JsonElement" /> (System.Text.Json) or as <see cref="long" />, <see cref="double" />,
    ///         <see cref="string" />, <see cref="bool" />, <see cref="DateTime" /> (for ISO 8601 date strings, by default)
    ///         or a <c>JToken</c> (Newtonsoft.Json); use <see cref="TryGetMetadata{T}" /> to read them as a specific type
    ///         with the same result regardless of the serializer. <see cref="JsonElement" /> values have no
    ///         value equality, so an error can be not equal to its original after a JSON round-trip (the same limitation
    ///         applies to <see cref="TemplatedMessage.Params" />).
    ///     </para>
    ///     <para>
    ///         Use <see cref="WithMetadata(string, object)" /> and <see cref="WithoutMetadata" /> to obtain a copy with
    ///         changed metadata. The setter is <c>init</c>-only to allow deserialization; it copies the provided entries
    ///         (so later changes to the source are not reflected), validates them as
    ///         <see cref="WithMetadata(string, object)" /> does, and treats <see langword="null" /> as empty.
    ///     </para>
    ///     <para>
    ///         The returned dictionary is a read-only wrapper that is never modified, so copies of an error
    ///         (e.g., created by a <c>with</c> expression) share it safely. Entries are enumerated (and serialized) in
    ///         the order they were added.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     When set with a key that is empty, whitespace-only or matches (case-insensitively) a standard problem
    ///     details member: <c>type</c>, <c>title</c>, <c>status</c>, <c>detail</c>, <c>instance</c>, <c>errors</c> or
    ///     <c>detailTemplated</c>.
    /// </exception>
    /// <exception cref="ArgumentNullException">When set with a <see langword="null" /> key or value.</exception>
    public IReadOnlyDictionary<string, object> Metadata
    {
        get => _metadata;
        init => _metadata = CreateMetadata(value);
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
    ///     <para>
    ///         This method mutates the current instance in place and returns that same instance; it does not create
    ///         a copy. Every holder of the instance observes the added detail, so do not add details to an
    ///         <see cref="Error" /> that is shared or cached (for example, in a static field). Use a <c>with</c>
    ///         expression first to obtain an independent copy.
    ///     </para>
    ///     <para>
    ///         Because <see cref="GetHashCode" /> incorporates the <see cref="ErrorDetails" /> collection, adding
    ///         a detail changes the hash code of the instance. An <see cref="Error" /> must therefore be fully built
    ///         before it is used as a key in a hash-based collection such as a dictionary or a set; adding a detail
    ///         afterwards leaves the entry stored under its previous hash code and unreachable by lookup.
    ///     </para>
    ///     <para>
    ///         For more details about JSON Pointers, see: <seealso href="https://datatracker.ietf.org/doc/html/rfc6901" />.
    ///     </para>
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

    #region metadata

    /// <summary>
    ///     Returns a copy of the current error with the specified metadata entry added, or replaced if the key
    ///     already exists.
    /// </summary>
    /// <param name="key">The key of the metadata entry.</param>
    /// <param name="value">The value of the metadata entry; it should be a JSON primitive.</param>
    /// <returns>A new instance of the <see cref="Error" /> object; the current instance is not modified.</returns>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="key" /> is empty, whitespace-only or matches (case-insensitively) a standard problem
    ///     details member: <c>type</c>, <c>title</c>, <c>status</c>, <c>detail</c>, <c>instance</c>, <c>errors</c> or
    ///     <c>detailTemplated</c>.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="key" /> or the <paramref name="value" /> is <see langword="null" />.
    /// </exception>
    public Error WithMetadata(string key, object value)
    {
        ValidateMetadataEntry(key, value);

        var metadata = new Dictionary<string, object>(_metadata, StringComparer.Ordinal)
        {
            [key] = value
        };

        return new Error(this, new ReadOnlyDictionary<string, object>(metadata));
    }

    /// <summary>
    ///     Returns a copy of the current error with the specified metadata entries added; entries with existing keys
    ///     are replaced and the other existing entries are kept.
    /// </summary>
    /// <param name="metadata">The metadata entries to add or replace.</param>
    /// <returns>A new instance of the <see cref="Error" /> object; the current instance is not modified.</returns>
    /// <exception cref="ArgumentException">
    ///     If any key is empty, whitespace-only or matches (case-insensitively) a standard problem details member:
    ///     <c>type</c>, <c>title</c>, <c>status</c>, <c>detail</c>, <c>instance</c>, <c>errors</c> or
    ///     <c>detailTemplated</c>.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="metadata" />, any key or any value is <see langword="null" />.
    /// </exception>
    public Error WithMetadata(IEnumerable<KeyValuePair<string, object>> metadata)
    {
        ArgumentNullException.ThrowIfNull(metadata);

        var result = new Dictionary<string, object>(_metadata, StringComparer.Ordinal);
        foreach (var pair in metadata)
        {
            ValidateMetadataEntry(pair.Key, pair.Value);
            result[pair.Key] = pair.Value;
        }

        return new Error(this, new ReadOnlyDictionary<string, object>(result));
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
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        if (!_metadata.ContainsKey(key))
            return new Error(this);

        var metadata = new Dictionary<string, object>(_metadata, StringComparer.Ordinal);
        metadata.Remove(key);

        return new Error(this, metadata.Count == 0 ? EmptyMetadata : new ReadOnlyDictionary<string, object>(metadata));
    }

    /// <summary>
    ///     Gets the metadata value of the specified key, converted to the type <typeparamref name="T" />.
    /// </summary>
    /// <typeparam name="T">The expected type of the value.</typeparam>
    /// <param name="key">The key of the metadata entry.</param>
    /// <param name="value">
    ///     The value if the entry exists and can be converted; otherwise, the default value of <typeparamref name="T" />.
    /// </param>
    /// <returns>
    ///     <see langword="true" /> if the entry exists and its value is, or can be converted to,
    ///     <typeparamref name="T" />; otherwise, <see langword="false" />.
    /// </returns>
    /// <remarks>
    ///     <para>This method never throws.</para>
    ///     <para>
    ///         A value that already is a <typeparamref name="T" /> is returned as is. Any other value is converted
    ///         through a single path, so the result depends only on the value, not on whether the error was created in
    ///         memory or deserialized with System.Text.Json or Newtonsoft.Json: a value that is not a
    ///         <see cref="JsonElement" /> is first written as a JSON primitive (a string, a number or a boolean, exactly
    ///         as System.Text.Json would serialize it), and the resulting <see cref="JsonElement" /> is converted to
    ///         <typeparamref name="T" />.
    ///     </para>
    ///     <para>
    ///         The conversion is strict: <see cref="string" />, <see cref="DateTime" />, <see cref="DateTimeOffset" />
    ///         and <see cref="Guid" /> are read from JSON strings (dates in the ISO 8601-1 extended format),
    ///         <see cref="bool" /> from JSON booleans, and integer, floating-point and enum types from JSON numbers
    ///         that fit the target type exactly. Other types are deserialized with
    ///         <see cref="JsonSerializer.Deserialize{TValue}(JsonElement, JsonSerializerOptions)" />.
    ///     </para>
    ///     <para>
    ///         Deserializing to other types (e.g., arrays or objects) uses reflection-based serialization, which is not
    ///         available in Native AOT and trimmed applications (when <c>JsonSerializerIsReflectionEnabledByDefault</c>
    ///         is <see langword="false" />); in that case this method returns <see langword="false" /> for such types.
    ///         The conversions listed above do not use reflection.
    ///     </para>
    /// </remarks>
    public bool TryGetMetadata<T>(string key, [MaybeNullWhen(false)] out T value)
    {
        value = default;

        if (key is null || !_metadata.TryGetValue(key, out var raw) || raw is null)
            return false;

        if (raw is T typed)
        {
            value = typed;
            return true;
        }

        try
        {
            var converted = raw is JsonElement element
                ? ConvertJsonElement<T>(element)
                : ConvertPrimitive<T>(raw);

            if (converted is not T result)
                return false;

            value = result;
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    ///     Converts a non-<see cref="JsonElement" /> value by writing it as a JSON primitive first, so that it follows
    ///     the same conversion rules as a deserialized value.
    /// </summary>
    private static object? ConvertPrimitive<T>(object raw)
    {
        var buffer = new ArrayBufferWriter<byte>();
        using (var writer = new Utf8JsonWriter(buffer))
        {
            if (!TryWritePrimitive(writer, raw))
                return null;
        }

        using var document = JsonDocument.Parse(buffer.WrittenMemory);
        return ConvertJsonElement<T>(document.RootElement);
    }

    /// <summary>
    ///     Writes a primitive value as System.Text.Json serializes it. <see cref="IConvertible" /> values are
    ///     recognized by their <see cref="TypeCode" />, which also covers enums (as numbers) and Newtonsoft.Json
    ///     <c>JValue</c> instances, without referencing Newtonsoft.Json.
    /// </summary>
    private static bool TryWritePrimitive(Utf8JsonWriter writer, object raw)
    {
        switch (raw)
        {
            case DateTimeOffset dateTimeOffset:
                writer.WriteStringValue(dateTimeOffset);
                return true;
            case Guid guid:
                writer.WriteStringValue(guid);
                return true;
            case IConvertible convertible:
                var provider = CultureInfo.InvariantCulture;
                switch (convertible.GetTypeCode())
                {
                    case TypeCode.Boolean:
                        writer.WriteBooleanValue(convertible.ToBoolean(provider));
                        return true;
                    case TypeCode.Char or TypeCode.String:
                        writer.WriteStringValue(convertible.ToString(provider));
                        return true;
                    case TypeCode.DateTime:
                        writer.WriteStringValue(convertible.ToDateTime(provider));
                        return true;
                    case TypeCode.SByte or TypeCode.Int16 or TypeCode.Int32 or TypeCode.Int64:
                        writer.WriteNumberValue(convertible.ToInt64(provider));
                        return true;
                    case TypeCode.Byte or TypeCode.UInt16 or TypeCode.UInt32 or TypeCode.UInt64:
                        writer.WriteNumberValue(convertible.ToUInt64(provider));
                        return true;
                    case TypeCode.Single:
                        writer.WriteNumberValue(convertible.ToSingle(provider));
                        return true;
                    case TypeCode.Double:
                        writer.WriteNumberValue(convertible.ToDouble(provider));
                        return true;
                    case TypeCode.Decimal:
                        writer.WriteNumberValue(convertible.ToDecimal(provider));
                        return true;
                    default:
                        return false;
                }
            default:
                return false;
        }
    }

    private static object? ConvertJsonElement<T>(JsonElement element)
    {
        var type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);

        if (type == typeof(string))
            return element.ValueKind == JsonValueKind.String ? element.GetString() : null;

        if (type == typeof(bool))
            return element.ValueKind is JsonValueKind.True or JsonValueKind.False ? element.GetBoolean() : null;

        if (type == typeof(DateTime))
            return element.ValueKind == JsonValueKind.String && element.TryGetDateTime(out var dateTime)
                ? dateTime
                : null;

        if (type == typeof(DateTimeOffset))
            return element.ValueKind == JsonValueKind.String && element.TryGetDateTimeOffset(out var dateTimeOffset)
                ? dateTimeOffset
                : null;

        if (type == typeof(Guid))
            return element.ValueKind == JsonValueKind.String && element.TryGetGuid(out var guid) ? guid : null;

        if (type.IsEnum)
        {
            var number = ConvertJsonNumber(element, Type.GetTypeCode(type));
            return number is null ? null : Enum.ToObject(type, number);
        }

        if (Type.GetTypeCode(type) is >= TypeCode.SByte and <= TypeCode.Decimal)
            return ConvertJsonNumber(element, Type.GetTypeCode(type));

        return element.Deserialize<T>();
    }

    private static object? ConvertJsonNumber(JsonElement element, TypeCode typeCode)
    {
        if (element.ValueKind != JsonValueKind.Number)
            return null;

        return typeCode switch
        {
            TypeCode.SByte => element.TryGetSByte(out var number) ? number : null,
            TypeCode.Byte => element.TryGetByte(out var number) ? number : null,
            TypeCode.Int16 => element.TryGetInt16(out var number) ? number : null,
            TypeCode.UInt16 => element.TryGetUInt16(out var number) ? number : null,
            TypeCode.Int32 => element.TryGetInt32(out var number) ? number : null,
            TypeCode.UInt32 => element.TryGetUInt32(out var number) ? number : null,
            TypeCode.Int64 => element.TryGetInt64(out var number) ? number : null,
            TypeCode.UInt64 => element.TryGetUInt64(out var number) ? number : null,
            TypeCode.Single => element.TryGetSingle(out var number) ? number : null,
            TypeCode.Double => element.TryGetDouble(out var number) ? number : null,
            TypeCode.Decimal => element.TryGetDecimal(out var number) ? number : null,
            _ => null
        };
    }

    private static ReadOnlyDictionary<string, object> CreateMetadata(
        IEnumerable<KeyValuePair<string, object>>? metadata)
    {
        if (metadata is null)
            return EmptyMetadata;

        var copy = new Dictionary<string, object>(StringComparer.Ordinal);
        foreach (var pair in metadata)
        {
            ValidateMetadataEntry(pair.Key, pair.Value);
            copy[pair.Key] = pair.Value;
        }

        return copy.Count == 0 ? EmptyMetadata : new ReadOnlyDictionary<string, object>(copy);
    }

    private static void ValidateMetadataEntry(string key, object value)
    {
        MetadataKeyValidator.Validate(key);

        if (value is null)
            throw new ArgumentNullException(nameof(value), $"The value of the metadata key ‘{key}’ must not be null.");
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
               && _errorDetails.SequenceEqual(other._errorDetails)
               && DictionaryContent.Equal(_metadata, other._metadata);
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

        foreach (var errorDetail in _errorDetails)
            hashCode.Add(errorDetail);

        hashCode.Add(DictionaryContent.GetHashCode(_metadata));

        return hashCode.ToHashCode();
    }

    #endregion

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
    ///     If the <paramref name="typeUri" /> resolves to an empty or whitespace-only value.
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
    ///     If the <paramref name="typeUri" /> resolves to an empty or whitespace-only value.
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
    ///     Indicates that the operation has failed because the authentication is missing or invalid.<br />
    ///     It can be mapped to the 401 HTTP status code (Unauthorized) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> resolves to an empty or whitespace-only value.
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
    ///     Indicates that the operation has failed because the authentication is missing or invalid.<br />
    ///     It can be mapped to the 401 HTTP status code (Unauthorized) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> resolves to an empty or whitespace-only value.
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
    ///     If the <paramref name="typeUri" /> resolves to an empty or whitespace-only value.
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
    ///     Indicates that the operation has failed because the identity does not have permission to perform it.<br />
    ///     It can be mapped to the 403 HTTP status code (Forbidden) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> resolves to an empty or whitespace-only value.
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
    ///     If the <paramref name="typeUri" /> resolves to an empty or whitespace-only value.
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
    ///     Indicates that the requested resource has not been found.<br />
    ///     It can be mapped to the 404 HTTP status code (Not Found) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> resolves to an empty or whitespace-only value.
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
    ///     If the <paramref name="typeUri" /> resolves to an empty or whitespace-only value.
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
    ///     Indicates that the operation failed because it was not completed within the allotted time.<br />
    ///     It can be mapped to the 408 HTTP status code (Request Timeout) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> resolves to an empty or whitespace-only value.
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
    ///     Indicates that the operation failed because of a data or state conflict (such as a unique constraint violation).<br />
    ///     It can be mapped to the 409 HTTP status code (Conflict) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> resolves to an empty or whitespace-only value.
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
    ///     Indicates that the operation failed because of a data or state conflict (such as a unique constraint violation).<br />
    ///     It can be mapped to the 409 HTTP status code (Conflict) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> resolves to an empty or whitespace-only value.
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
    ///     Indicates an expected error that is not critical to the operation.<br />
    ///     It can be mapped to the 422 HTTP status code (Unprocessable Content) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> resolves to an empty or whitespace-only value.
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
    ///     Indicates an expected error that is not critical to the operation.<br />
    ///     It can be mapped to the 422 HTTP status code (Unprocessable Content) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> resolves to an empty or whitespace-only value.
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
    ///     Indicates an unexpected, critical error.<br />
    ///     It can be mapped to the 500 HTTP status code (Internal Server Error) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> resolves to an empty or whitespace-only value.
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
    ///     Indicates an unexpected, critical error.<br />
    ///     It can be mapped to the 500 HTTP status code (Internal Server Error) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> resolves to an empty or whitespace-only value.
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
    ///     Indicates that the operation, feature or case is not implemented.<br />
    ///     It can be mapped to the 501 HTTP status code (Not Implemented) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> resolves to an empty or whitespace-only value.
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
    ///     Indicates that the operation, feature or case is not implemented.<br />
    ///     It can be mapped to the 501 HTTP status code (Not Implemented) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> resolves to an empty or whitespace-only value.
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
    ///     Indicates that the service is not available and the operation can be retried later.<br />
    ///     It can be mapped to the 503 HTTP status code (Service Unavailable) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> resolves to an empty or whitespace-only value.
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
    ///     Indicates that the service is not available and the operation can be retried later.<br />
    ///     It can be mapped to the 503 HTTP status code (Service Unavailable) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> resolves to an empty or whitespace-only value.
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
    ///     message (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates that the request is valid and permitted, but a usage limit, quota or rate has been exhausted.
    ///     The operation may succeed later or once the limit is raised.<br />
    ///     It can be mapped to the 429 HTTP status code (Too Many Requests) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> resolves to an empty or whitespace-only value.
    /// </exception>
    public static Error LimitExceeded(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        params (string Key, object Value)[] detailNamedValues)
    {
        return new Error(
            ErrorCategory.LimitExceeded,
            typeUri.Value,
            title,
            detail,
            detailTemplateId is null
                ? null
                : new TemplatedMessage(detailTemplateId, detailNamedValues.ToDictionary(x => x.Key, x => x.Value)),
            instanceUri?.Value);
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
    ///     message (the human-readable explanation specific to this occurrence of the problem).
    /// </param>
    /// <returns>A new instance of the <see cref="Error" /> object.</returns>
    /// <remarks>
    ///     Indicates that the request is valid and permitted, but a usage limit, quota or rate has been exhausted.
    ///     The operation may succeed later or once the limit is raised.<br />
    ///     It can be mapped to the 429 HTTP status code (Too Many Requests) response.
    /// </remarks>
    /// <exception cref="ArgumentException">
    ///     If the <paramref name="typeUri" /> resolves to an empty or whitespace-only value.
    /// </exception>
    public static Error LimitExceeded(ErrorUri typeUri, string title, string? detail = null,
        ErrorUri? instanceUri = null, string? detailTemplateId = null,
        IEnumerable<KeyValuePair<string, object>>? detailNamedValues = null)
    {
        return new Error(
            ErrorCategory.LimitExceeded,
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
