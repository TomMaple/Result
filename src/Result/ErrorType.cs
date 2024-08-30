// Maple.Result library
// Under the MIT licence.

namespace Maple.Result;

/// <summary>
///     An enumeration representing the different types of errors that can be returned by the Result class.
/// </summary>
public enum ErrorType
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
    Forbidden,

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
    CriticalError,

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