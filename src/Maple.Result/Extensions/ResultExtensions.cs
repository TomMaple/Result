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

namespace Maple.Result.Extensions;

/// <summary>
///     The collection of extension methods for the <see cref="Result{T}" /> type.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    ///     Converts the generic <see cref="Result{T}" /> to the non-generic <see cref="Result" />.
    /// </summary>
    /// <typeparam name="T">The type of the value contained in the <see cref="Result{T}" />.</typeparam>
    /// <param name="result">The <see cref="Result{T}" /> instance to convert.</param>
    /// <returns>
    ///     A non-generic <see cref="Result" /> representing a success state or
    ///     a failure state (with the original <see cref="Error" />).
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If the <paramref name="result" /> is <see langword="null" />.
    /// </exception>
    public static Result ToResult<T>(this Result<T> result)
    {
        ArgumentNullException.ThrowIfNull(result);

        return result.IsSuccess()
            ? Result.Success()
            : Result.FromError(result.Error!);
    }
}