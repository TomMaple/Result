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
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using Maple.Result.Serialization;

namespace Maple.Result;

/// <summary>
///     Defines the contract for an operation result, providing information about success or failure and any associated
///     error details.
/// </summary>
/// <remarks>
///     Use the <see cref="IsSuccess" /> method to determine whether the operation completed successfully, and access
///     the <see cref="Error" /> property for error information if the operation failed.
/// </remarks>
public interface IResult
{
    /// <summary>
    ///     Returns an <see cref="Error" /> object if the operation failed;
    ///     otherwise, returns <see langword="null" />.
    /// </summary>
    public Error? Error { get; }

    /// <summary>
    ///     Returns an indicator of whether the operation was successful.
    /// </summary>
    /// <returns>
    ///     <see langword="true" /> if the operation was successful; otherwise, <see langword="false" />.
    /// </returns>
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess();
}

/// <summary>
///     Defines a result of an operation that can either be successful or contain an error.
/// </summary>
/// <inheritdoc cref="IResult" />
public sealed record Result : IResult
{
    #region constructors

    /// <summary>
    ///     Initializes a new instance of the successful <see cref="Result" /> record.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This constructor is intended only for deserialization purposes,
    ///         and it should not be used directly in code.
    ///     </para>
    ///     <para>
    ///         Use the static <see cref="Success" /> method to create a successful <see cref="Result" /> instance or
    ///         <see cref="FromError" /> method or the implicit operator to create a failed instance.
    ///     </para>
    /// </remarks>
    public Result()
    {
    }

    internal Result(Error error)
    {
        Error = error;
    }

    #endregion

    /// <summary>
    ///     Returns an <see cref="Error" /> object if the operation failed;
    ///     otherwise, returns <see langword="null" />.
    /// </summary>
    public Error? Error { get; init; }

    /// <summary>
    ///     Returns an indicator of whether the operation was successful.
    /// </summary>
    /// <returns>
    ///     <see langword="true" /> if the operation was successful; otherwise, <see langword="false" />.
    /// </returns>
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess()
    {
        return Error is null;
    }

    /// <summary>
    ///     Creates a successful <see cref="Result" /> instance.
    /// </summary>
    /// <returns>A successful <see cref="Result" /> instance.</returns>
    public static Result Success()
    {
        return new Result();
    }

    #region implicit operators

    /// <summary>
    ///     Converts an <see cref="Error" /> to a failed <see cref="Result" /> with the specified error.
    /// </summary>
    /// <param name="error">The <see cref="Error" /> to convert.</param>
    /// <returns>A failed <see cref="Result" /> instance with the provided error.</returns>
    public static implicit operator Result(Error error)
    {
        return new Result(error);
    }

    #endregion

    #region factory methods

    /// <summary>
    ///     Creates a <see cref="Result" /> instance representing a failure with the specified <see cref="Error" />.
    /// </summary>
    public static Result FromError(Error error)
    {
        return new Result(error);
    }

    /// <summary>
    ///     Creates a successful <see cref="Result{T}" /> instance containing the specified value.
    /// </summary>
    /// <typeparam name="T">The type of the <paramref name="value" />.</typeparam>
    /// <remarks>
    ///     A <see langword="null" /> <paramref name="value" /> is permitted when <typeparamref name="T" /> is nullable
    ///     (a nullable reference type or <see cref="Nullable{T}" />); the resulting <see cref="Result{T}" /> is
    ///     successful and its <see cref="Result{T}.Value" /> is <see langword="null" />.
    /// </remarks>
    public static Result<T> FromValue<T>(T value)
    {
        return new Result<T>(value);
    }

    #endregion
}

/// <summary>
///     Defines a result of an operation that can either be successful and contain a value
///     of type <typeparamref name="T" />, or contain an <see cref="Error" />.
/// </summary>
/// <typeparam name="T">The type of the successful value.</typeparam>
/// <inheritdoc cref="IResult" />
[JsonConverter(typeof(ResultJsonConverterFactory))]
public sealed record Result<T> : IResult
{
    #region read-only fields

    private readonly Error? _error;
    private readonly T? _value;

    // Tells an unset value apart from a value that happens to equal default (e.g., 0 for a Result<int>).
    private readonly bool _hasValue;

    #endregion

    #region constructors

    /// <summary>
    ///     Initializes a new instance of the successful <see cref="Result{T}" /> record.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This constructor is intended only for deserialization purposes,
    ///         and it should not be used directly in code.
    ///     </para>
    ///     <para>
    ///         Use the static <see cref="FromValue" /> method to create a successful <see cref="Result" /> instance
    ///         with provided value, or <see cref="FromError" /> method or the implicit operator
    ///         to create a failed instance with provided <see cref="Error" />.
    ///     </para>
    /// </remarks>
    public Result()
    {
    }

    internal Result(T value)
    {
        // A successful result carries a value, and that value may itself be null when T is nullable
        // (a nullable reference type or Nullable<U>). A non-nullable value type has no null to pass here.
        // Record that a value is present so it is told apart from an absent one (a failed or unpopulated result).
        _value = value;
        _hasValue = true;
    }

    internal Result(Error error)
    {
        _error = error
                 ?? throw new ArgumentNullException(nameof(error), "Error cannot be null!");
    }

    #endregion

    /// <summary>
    ///     Returns the value of type <typeparamref name="T" /> if the operation was successful;
    ///     otherwise, returns <see langword="null" />.
    /// </summary>
    /// <remarks>
    ///     A successful <see cref="Result{T}" /> carries a value, which may itself be <see langword="null" /> when
    ///     <typeparamref name="T" /> is nullable (a nullable reference type or <see cref="Nullable{T}" />). A
    ///     <see langword="null" /> here therefore does not by itself indicate failure—use <see cref="IsSuccess" />
    ///     to distinguish a successful null value from a failure.
    ///     The setter is <see langword="init" />-only and intended for deserialization. Assigning a value while an
    ///     <see cref="Error" /> is already set throws, because a <see cref="Result{T}" /> can never hold both.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    ///     If a value is assigned while an <see cref="Error" /> is already set.
    /// </exception>
    public T? Value
    {
        get => _value;
        init
        {
            // A result is either successful (a value) or failed (an error), never both.
            if (_error is not null)
                throw new InvalidOperationException("Cannot set both Value and Error of the Result!");

            // A null is a valid successful value when T is nullable; mark the value present either way so that
            // it is told apart from an absent one (a failed or not-yet-populated result).
            _value = value;
            _hasValue = true;
        }
    }

    /// <summary>
    ///     Returns an <see cref="Error" /> object if the operation failed;
    ///     otherwise, returns <see langword="null" />.
    /// </summary>
    /// <remarks>
    ///     The setter is <see langword="init" />-only and intended for deserialization. A <see langword="null" />
    ///     assignment is ignored rather than throwing, so that deserializing a successful <see cref="Result{T}" />
    ///     (where the error is absent) leaves the error unset instead of overwriting an already-populated member.
    ///     Assigning a non-null error while a value is already set throws, because a <see cref="Result{T}" />
    ///     can never hold both a value and an error.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    ///     If a non-null error is assigned while a value is already set.
    /// </exception>
    public Error? Error
    {
        get => _error;
        init
        {
            // Ignore a null assignment (e.g., the absent error when deserializing a successful result) instead of
            // throwing, so object/deserialization initializers can set only the relevant member.
            if (value is null)
                return;

            // A result is either successful (a value) or failed (an error), never both. Test _hasValue rather
            // than the value itself: for a non-nullable value type `_value is not null` is always true, which
            // would reject every error assigned to, say, a Result<int>.
            if (_hasValue)
                throw new InvalidOperationException("Cannot set both Value and Error of the Result!");

            _error = value;
        }
    }

    /// <summary>
    ///     Returns an indicator of whether a <see cref="Value" /> is present, telling an unset value apart from
    ///     one that happens to equal the default of <typeparamref name="T" />.
    /// </summary>
    internal bool HasValue => _hasValue;

    /// <summary>
    ///     Returns an indicator of whether <typeparamref name="T" /> can hold <see langword="null" />—that is,
    ///     whether it is a reference type or a <see cref="Nullable{T}" />.
    /// </summary>
    /// <remarks>
    ///     Used when reading JSON to tell a present <see langword="null"/> value (a successful result of a nullable
    ///     <typeparamref name="T" />) apart from an absent one (a non-nullable value type has no null to hold,
    ///     so a null token there denotes absence). Shared with <see cref="ResultJsonConverter{T}" /> so that both
    ///     serializers apply the same rule.
    /// </remarks>
    internal static bool CanHoldNull { get; } =
        !typeof(T).IsValueType || Nullable.GetUnderlyingType(typeof(T)) is not null;

    /// <summary>
    ///     Returns an indicator of whether <i>Newtonsoft.Json</i> should write the <see cref="Value" /> member.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         <i>Newtonsoft.Json</i> discovers this method by convention and does not honour the
    ///         <see cref="JsonConverterAttribute" /> that governs <i>System.Text.Json</i>. The member is written only
    ///         when a value is present (<see cref="HasValue" />) and no <see cref="Error" /> is set—including a
    ///         successful <see langword="null"/> value, which is written as <see langword="null" />. A failed result
    ///         has no value, so the member is omitted rather than written as a default (e.g., <c>0</c>) or a
    ///         <see langword="null"/> that would be mistaken for a successful null value when read back; the
    ///         <see cref="Error" /> guard also ensures a failed result never emits a value.
    ///     </para>
    ///     <para>
    ///         This method mirrors the rule applied by <i>System.Text.Json</i>, so both serializers produce the same
    ///         payload. It is public only because <i>Newtonsoft.Json</i> requires it to be; it is not intended to be
    ///         called directly.
    ///     </para>
    /// </remarks>
    /// <returns>
    ///     <see langword="true" /> if a value is present (even if that value is <see langword="null" />)
    ///     and no <see cref="Error" /> is set; otherwise, <see langword="false" />.
    /// </returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool ShouldSerializeValue()
    {
        return _hasValue && _error is null;
    }

    /// <summary>
    ///     Returns an indicator of whether the operation was successful.
    /// </summary>
    /// <remarks>
    ///     The <see cref="MemberNotNullWhenAttribute" /> on <see cref="Value" /> reflects the common case where a
    ///     successful result carries a non-null value. When <typeparamref name="T" /> is nullable (a nullable
    ///     reference type or <see cref="Nullable{T}" />) a successful result may legitimately carry a
    ///     <see langword="null" /> value, so the flow-analysis hint is a best effort rather than a guarantee for
    ///     those types; access <see cref="Value" /> accordingly.
    /// </remarks>
    /// <returns>
    ///     <see langword="true" /> if the operation was successful; otherwise, <see langword="false" />.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     If the <see cref="Result{T}" /> holds neither a <see cref="Value" /> nor an <see cref="Error" />.
    ///     Such an instance can only be produced by the parameterless constructor (which is intended for
    ///     deserialization) when neither member is subsequently populated.
    /// </exception>
    [MemberNotNullWhen(false, nameof(Error))]
    [MemberNotNullWhen(true, nameof(Value))]
    public bool IsSuccess()
    {
        if (_error is not null && _hasValue)
            throw new InvalidOperationException("A Result<T> cannot have both a value and an error.");

        // A Result<T> is either successful (a value) or failed (an error). An instance holding neither cannot honour
        // the MemberNotNullWhen contract below, so report the broken state instead of returning a misleading `true`.
        if (_error is null && !_hasValue)
        {
            throw new InvalidOperationException(
                "The Result<T> has neither a value nor an error. It was created with the parameterless constructor "
                + "and never populated. Use Result.FromValue(), Result<T>.FromError() or an implicit conversion.");
        }

        return _error is null;
    }

    #region implicit operators

    /// <summary>
    ///     Converts a value of type <typeparamref name="T" /> to a <see cref="Result{T}" /> representing a successful result.
    /// </summary>
    /// <remarks>
    ///     A <see langword="null" /> <paramref name="value" /> is permitted when <typeparamref name="T" /> is nullable
    ///     (a nullable reference type or <see cref="Nullable{T}" />); the resulting <see cref="Result{T}" /> is
    ///     successful and its <see cref="Value" /> is <see langword="null" />.
    /// </remarks>
    public static implicit operator Result<T>(T value)
    {
        return new Result<T>(value);
    }

    /// <summary>
    ///     Converts an <see cref="Error" /> to a <see cref="Result{T}" /> representing a failed result.
    /// </summary>
    public static implicit operator Result<T>(Error error)
    {
        return new Result<T>(error);
    }

    #endregion

    #region factory methods

    /// <summary>
    ///     Returns a successful <see cref="Result{T}" /> instance containing the specified value.
    /// </summary>
    /// <remarks>
    ///     A <see langword="null" /> <paramref name="value" /> is permitted when <typeparamref name="T" /> is nullable
    ///     (a nullable reference type or <see cref="Nullable{T}" />); the resulting <see cref="Result{T}" /> is
    ///     successful and its <see cref="Value" /> is <see langword="null" />.
    /// </remarks>
    public static Result<T> FromValue(T value)
    {
        return new Result<T>(value);
    }

    /// <summary>
    ///     Returns a <see cref="Result{T}" /> instance representing a failure with the specified <see cref="Error" />.
    /// </summary>
    public static Result<T> FromError(Error error)
    {
        return new Result<T>(error);
    }

    #endregion
}
