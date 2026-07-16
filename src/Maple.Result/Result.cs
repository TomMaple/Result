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
using System.Diagnostics.CodeAnalysis;

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
public sealed record Result<T> : IResult
{
    #region read-only fields

    private readonly Error? _error;
    private readonly T? _value;

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
        if (value is null)
            throw new ArgumentNullException(nameof(value), "Value cannot be null!");

        _value = value;
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
    ///     The setter is <see langword="init" />-only and intended for deserialization. A <see langword="null" />
    ///     assignment is ignored rather than throwing, so that deserializing a failed <see cref="Result{T}" />
    ///     (where the value is absent) leaves the value unset instead of overwriting an already-populated member.
    ///     Assigning a non-null value while an <see cref="Error" /> is already set throws, because a
    ///     <see cref="Result{T}" /> can never hold both a value and an error.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    ///     If a non-null value is assigned while an <see cref="Error" /> is already set.
    /// </exception>
    public T? Value
    {
        get => _value;
        init
        {
            // Ignore a null assignment (e.g., the absent value when deserializing a failed result) instead of
            // throwing, so object/deserialization initializers can set only the relevant member.
            if (value is null)
                return;

            // A result is either successful (a value) or failed (an error), never both.
            if (_error is not null)
                throw new InvalidOperationException("Cannot set both Value and Error of the Result!");

            _value = value;
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

            // A result is either successful (a value) or failed (an error), never both.
            if (_value is not null)
                throw new InvalidOperationException("Cannot set both Value and Error of the Result!");

            _error = value;
        }
    }

    /// <summary>
    ///     Returns an indicator of whether the operation was successful.
    /// </summary>
    /// <returns>
    ///     <see langword="true" /> if the operation was successful; otherwise, <see langword="false" />.
    /// </returns>
    [MemberNotNullWhen(false, nameof(Error))]
    [MemberNotNullWhen(true, nameof(Value))]
    public bool IsSuccess()
    {
        return Error is null;
    }

    #region implicit operators

    /// <summary>
    ///     Converts a value of type <typeparamref name="T" /> to a <see cref="Result{T}" /> representing a successful result.
    /// </summary>
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
