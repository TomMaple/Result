// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Result.Validators;

namespace Maple.Result;

/// <summary>
///     Represents a <i>URI</i> that identifies or describes the type or category of an error in a standardized format.
/// </summary>
/// <remarks>
///     It can be
///     <list type="bullet">
///         <item>
///             <term>no value</term>
///             <description><c>about:blank</c></description>
///         </item>
///         <item>
///             <term>URI locator</term>
///             <description>the <i>URI</i> of the human-readable documentation for the type of problem,</description>
///         </item>
///         <item>
///             <term>URI tag</term>
///             <description>
///                 the non-dereferenceable, unique representation of the specific type of problem
///                 (e.g., <example><c>tag:example@example.org,2021-09-17:OutOfLuck</c></example>).
///             </description>
///         </item>
///     </list>
///     For <i>URI Locator</i> specification, see also: <seealso href="https://datatracker.ietf.org/doc/html/rfc3986" />.<br />
///     For <i>URI Tag</i> specification, see also: <seealso href="https://datatracker.ietf.org/doc/html/rfc4151" />
/// </remarks>
public readonly record struct ErrorUri
{
    #region constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="ErrorUri" /> record with the provided value.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This constructor is intended only for deserialization purposes,
    ///         and it should not be used directly in code.
    ///     </para>
    ///     <para>
    ///         To create a new instance of the <see cref="ErrorUri" /> object, use one of the factory methods provided
    ///         (<see cref="None"/>, <see cref="Locator"/> or <see cref="Tag"/>).
    ///     </para>
    /// </remarks>
    public ErrorUri(string value)
    {
        Value = value;
    }

    #endregion

    /// <summary>
    ///     The <i>URI</i> value representing the error type or category.
    /// </summary>
    public string Value { get; }

    /// <summary>
    ///     Returns a new instance of the no-value <see cref="ErrorUri" />: <c>about:blank</c>.
    /// </summary>
    public static ErrorUri None()
    {
        return new ErrorUri("about:blank");
    }

    /// <summary>
    ///     Returns a new instance of the <see cref="ErrorUri" /> with a valid <i>URI Locator</i>.
    /// </summary>
    public static ErrorUri Locator(string uriLocator)
    {
        UriLocatorValidator.Validate(uriLocator);

        return new ErrorUri(uriLocator);
    }

    /// <summary>
    ///     Returns a new instance of the <see cref="ErrorUri" /> with a valid <i>URI Tag</i>.
    /// </summary>
    public static ErrorUri Tag(string uriTag)
    {
        UriTagValidator.Validate(uriTag);

        return new ErrorUri(uriTag);
    }
}
