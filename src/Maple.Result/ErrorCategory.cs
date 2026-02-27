// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

namespace Maple.Result;

/// <summary>
///     An enumeration representing the different categories of errors that can be returned by the Result record.
/// </summary>
public enum ErrorCategory
{
    /// <summary>
    ///     A validation error.
    /// </summary>
    /// <remarks>
    ///     The operation failed due to something wrong with the input data.<br />
    ///     Can be mapped to the 400 HTTP status code (Bad Request).
    /// </remarks>
    Validation,

    /// <summary>
    ///     The action is not authenticated.
    /// </summary>
    /// <remarks>
    ///     The authentication is either missing or invalid.<br />
    ///     Can be mapped to the 401 HTTP status code (Unauthorized).
    /// </remarks>
    Unauthenticated,

    /// <summary>
    ///     The action is not authorized.
    /// </summary>
    /// <remarks>
    ///     The authentication might be valid, but the identity does not have the required permissions.<br />
    ///     Can be mapped to the 403 HTTP status code (Forbidden).
    /// </remarks>
    Unauthorized,

    /// <summary>
    ///     The resource was not found error.
    /// </summary>
    /// <remarks>
    ///     Can be mapped to the 404 HTTP status code (Not Found).
    /// </remarks>
    NotFound,

    /// <summary>
    ///     A timeout error.
    /// </summary>
    /// <remarks>
    ///     The operation timed out.<br />
    ///     Can be mapped to the 408 HTTP status code (Request Timeout).
    /// </remarks>
    Timeout,

    /// <summary>
    ///     A conflict error.
    /// </summary>
    /// <remarks>
    ///     The operation failed due to the data or state conflict (such as a unique constraint violation).<br />
    ///     Can be mapped to the 409 HTTP status code (Conflict).
    /// </remarks>
    Conflict,

    /// <summary>
    ///     An expected error that is not critical to the operation.
    /// </summary>
    /// <remarks>
    ///     Do not use for authentication, authorization, validation, data conflict or unexpected errors.<br />
    ///     Can be mapped to the 422 HTTP status code (Unprocessable Content).
    /// </remarks>
    Failure,

    /// <summary>
    ///     An unexpected, critical error.
    /// </summary>
    /// <remarks>
    ///     Can be mapped to the 500 HTTP status code (Internal Server Error).
    /// </remarks>
    Critical,

    /// <summary>
    ///     The not implemented error.
    /// </summary>
    /// <remarks>
    ///     The operation, feature or case is not implemented.<br />
    ///     Can be mapped to the 501 HTTP status code (Not Implemented).
    /// </remarks>
    NotImplemented,

    /// <summary>
    ///     The resource is not available.
    /// </summary>
    /// <remarks>
    ///     Try again later.<br />
    ///     Can be mapped to the 503 HTTP status code (Service Unavailable).
    /// </remarks>
    Unavailable
}
