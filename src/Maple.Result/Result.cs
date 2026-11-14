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
using System.Diagnostics.CodeAnalysis;

namespace Maple.Result;

/// <summary>
///     Defines the contract for an operation result, providing information about success or failure and any associated error details.
/// </summary>
/// <remarks>
///     Use the <see cref="IsSuccess" /> method to determine whether the operation completed successfully,
///     and access the <see cref="Error" /> property for error information if the operation failed.
/// </remarks>
public interface IOperationResult
{
    /// <summary>
    ///     Returns an <see cref="Error" /> object if the operation failed; otherwise, returns <see langword="null" />.
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
/// <remarks>
///     The record is immutable. Create it with <see cref="Success" />, <see cref="FromError" /> or the implicit conversion from an <see cref="Error" />.
/// </remarks>
/// <inheritdoc cref="IOperationResult" />
public sealed record Result : IOperationResult
{
    private static readonly Result SuccessResult = new(null);

    #region constructors

    // The only constructor takes a parameter on purpose: a parameterless one (even a private one) lets
    // Newtonsoft.Json create an instance and silently read every payload, including a failed one, as a success.
    private Result(Error? error)
    {
        Error = error;
    }

    #endregion

    /// <summary>
    ///     Returns an <see cref="Error" /> object if the operation failed; otherwise, returns <see langword="null" />.
    /// </summary>
    public Error? Error { get; }

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
    ///     Returns a successful <see cref="Result" /> instance.
    /// </summary>
    /// <returns>A successful <see cref="Result" /> instance (always the same, shared instance).</returns>
    public static Result Success()
    {
        return SuccessResult;
    }

    #region implicit operators

    /// <summary>
    ///     Converts an <see cref="Error" /> to a failed <see cref="Result" /> with the specified error.
    /// </summary>
    /// <param name="error">The <see cref="Error" /> to convert.</param>
    /// <returns>A failed <see cref="Result" /> instance with the provided error.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="error" /> is <see langword="null" />.</exception>
    public static implicit operator Result(Error error)
    {
        return new Result(error ?? throw new ArgumentNullException(nameof(error), "Error cannot be null!"));
    }

    #endregion

    #region factory methods

    /// <summary>
    ///     Creates a <see cref="Result" /> instance representing a failure with the specified <see cref="Error" />.
    /// </summary>
    /// <exception cref="ArgumentNullException">If the <paramref name="error" /> is <see langword="null" />.</exception>
    public static Result FromError(Error error)
    {
        return new Result(error ?? throw new ArgumentNullException(nameof(error), "Error cannot be null!"));
    }

    /// <summary>
    ///     Creates a successful <see cref="Result{T}" /> instance containing the specified value.
    /// </summary>
    /// <typeparam name="T">The type of the <paramref name="value" />.</typeparam>
    /// <remarks>
    ///     A <see langword="null" /> <paramref name="value" /> is permitted when <typeparamref name="T" /> is nullable
    ///     (a nullable reference type or <see cref="Nullable{T}" />);
    ///     the resulting <see cref="Result{T}" /> is successful and its <see cref="Result{T}.Value" /> is <see langword="null" />.
    /// </remarks>
    public static Result<T> FromValue<T>(T value)
    {
        return Result<T>.FromValue(value);
    }

    #endregion
}

/// <summary>
///     Defines a result of an operation that can either be successful and contain a value of type <typeparamref name="T" />,
///     or contain an <see cref="Error" />.
/// </summary>
/// <typeparam name="T">The type of the successful value.</typeparam>
/// <remarks>
///     The record is immutable. Create it with <see cref="FromValue" />, <see cref="FromError" /> or the implicit conversions from a <typeparamref name="T" />
///     or an <see cref="Error" />. It is successful exactly when it has no <see cref="Error" />.
/// </remarks>
/// <inheritdoc cref="IOperationResult" />
public sealed record Result<T> : IOperationResult
{
    #region constructors

    private Result(T value)
    {
        Value = value;
    }

    private Result(Error error)
    {
        Error = error
                ?? throw new ArgumentNullException(nameof(error), "Error cannot be null!");
    }

    #endregion

    /// <summary>
    ///     Returns the value of type <typeparamref name="T" /> if the operation was successful;
    ///     otherwise, returns the default value of <typeparamref name="T" />.
    /// </summary>
    /// <remarks>
    ///     A successful <see cref="Result{T}" /> carries a value, which may itself be <see langword="null" /> when <typeparamref name="T" /> is nullable
    ///     (a nullable reference type or <see cref="Nullable{T}" />). A <see langword="null" /> (or default) here therefore does not by itself indicate
    ///     failure—use <see cref="IsSuccess" /> to distinguish a successful value from a failure.
    /// </remarks>
    public T? Value { get; }

    /// <summary>
    ///     Returns an <see cref="Error" /> object if the operation failed; otherwise, returns <see langword="null" />.
    /// </summary>
    public Error? Error { get; }

    /// <summary>
    ///     Returns an indicator of whether the operation was successful.
    /// </summary>
    /// <remarks>
    ///     The <see cref="MemberNotNullWhenAttribute" /> on <see cref="Value" /> reflects the common case where a successful result carries a non-null value.
    ///     When <typeparamref name="T" /> is nullable (a nullable reference type or <see cref="Nullable{T}" />) a successful result may legitimately carry
    ///     a <see langword="null" /> value, so the flow-analysis hint is a best effort rather than a guarantee for those types;
    ///     access <see cref="Value" /> accordingly.
    /// </remarks>
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
    /// <remarks>
    ///     A <see langword="null" /> <paramref name="value" /> is permitted when <typeparamref name="T" /> is nullable
    ///     (a nullable reference type or <see cref="Nullable{T}" />);
    ///     the resulting <see cref="Result{T}" /> is successful and its <see cref="Value" /> is <see langword="null" />.
    /// </remarks>
    public static implicit operator Result<T>(T value)
    {
        return new Result<T>(value);
    }

    /// <summary>
    ///     Converts an <see cref="Error" /> to a <see cref="Result{T}" /> representing a failed result.
    /// </summary>
    /// <exception cref="ArgumentNullException">If the <paramref name="error" /> is <see langword="null" />.</exception>
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
    ///     (a nullable reference type or <see cref="Nullable{T}" />);
    ///     the resulting <see cref="Result{T}" /> is successful and its <see cref="Value" /> is <see langword="null" />.
    /// </remarks>
    public static Result<T> FromValue(T value)
    {
        return new Result<T>(value);
    }

    /// <summary>
    ///     Returns a <see cref="Result{T}" /> instance representing a failure with the specified <see cref="Error" />.
    /// </summary>
    /// <exception cref="ArgumentNullException">If the <paramref name="error" /> is <see langword="null" />.</exception>
    public static Result<T> FromError(Error error)
    {
        return new Result<T>(error);
    }

    #endregion
}
