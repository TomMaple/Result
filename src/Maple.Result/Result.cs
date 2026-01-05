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
    ///     Returns an <see cref="Error" /> object if the operation failed; otherwise, returns <see langword="null" />.
    /// </summary>
    public Error? Error { get; }

    /// <summary>
    ///     Returns an indication of whether the operation was successful.
    /// </summary>
    public bool IsSuccess();
}

/// <summary>
///     Defines a result of an operation that can either be successful or contain an error.
/// </summary>
/// <inheritdoc cref="IResult" />
public sealed record Result : IResult
{
    #region constructors

    public Result()
    {
    }

    internal Result(Error error)
    {
        Error = error;
    }

    #endregion

    /// <summary>
    /// </summary>
    public Error? Error { get; init; }

    public bool IsSuccess()
    {
        return Error == null;
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
///     of type <typeparamref name="T" />, or contain an error.
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
        if (error is null)
            throw new ArgumentNullException(nameof(error), "Error cannot be null!");

        _error = error;
    }

    #endregion

    /// <summary>
    ///     Returns the value of type <typeparamref name="T" /> if the operation was successful;
    ///     otherwise, returns <see langword="null" />.
    /// </summary>
    public T? Value
    {
        get => _value;
        init
        {
            if (value is null)
                return;

            if (_error is not null)
                throw new InvalidOperationException("Cannot set both Value and Error of the Result!");

            _value = value;
        }
    }

    public Error? Error
    {
        get => _error;
        init
        {
            if (value is null)
                return;

            if (_value is not null)
                throw new InvalidOperationException("Cannot set both Value and Error of the Result!");

            _error = value;
        }
    }

    public bool IsSuccess()
    {
        return Error == null;
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