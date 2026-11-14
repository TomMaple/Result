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
///                 (e.g., <example><c>tag:example@example.org,2021-09-17:OutOfLuck</c></example>),
///             </description>
///         </item>
///         <item>
///             <term>URN</term>
///             <description>
///                 the non-dereferenceable, persistent name of the specific type of problem
///                 (e.g., <example><c>urn:example:errors:out-of-credit</c></example>),
///             </description>
///         </item>
///         <item>
///             <term>relative URI reference</term>
///             <description>
///                 the full path resolved against the base <i>URI</i> of the response (e.g., <example><c>/account/12345/msgs/abc</c></example>);
///                 <i>RFC 9457</i> recommends absolute <i>URI</i>s when possible.
///             </description>
///         </item>
///     </list>
///     For <i>URI Locator</i> and relative reference specification, see also: <seealso href="https://datatracker.ietf.org/doc/html/rfc3986" />.<br />
///     For <i>URI Tag</i> specification, see also: <seealso href="https://datatracker.ietf.org/doc/html/rfc4151" />.<br />
///     For <i>URN</i> specification, see also: <seealso href="https://datatracker.ietf.org/doc/html/rfc8141" />.
/// </remarks>
public readonly record struct ErrorUri
{
    private const string NoneValue = "about:blank";

    private readonly string? _value;

    #region constructors

    /// <summary>
    ///     Initializes a new instance of the <see cref="ErrorUri" /> record with the provided value, without validating it.
    /// </summary>
    /// <remarks>
    ///     Used by the factory methods (<see cref="None" />, <see cref="Locator" />, <see cref="Tag" />, <see cref="Urn" />, <see cref="Relative" />)
    ///     after validation, and for values taken from an existing <see cref="Error" />, which were validated when it was created.
    /// </remarks>
    internal ErrorUri(string value)
    {
        _value = value;
    }

    #endregion

    /// <summary>
    ///     The <i>URI</i> value representing the error type or category.
    /// </summary>
    /// <remarks>
    ///     A default <see cref="ErrorUri" /> instance—which bypasses the constructor and therefore has no underlying value—reports <c>about:blank</c>,
    ///     the value that <i>RFC 9457</i> defines for an unspecified type.
    ///     This keeps the mandatory <see cref="Error.TypeUri" /> from ever being <see langword="null" />.
    /// </remarks>
    public string Value => _value ?? NoneValue;

    /// <summary>
    ///     Returns a new instance of the no-value <see cref="ErrorUri" />: <c>about:blank</c>.
    /// </summary>
    public static ErrorUri None()
    {
        return new ErrorUri(NoneValue);
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

    /// <summary>
    ///     Returns a new instance of the <see cref="ErrorUri" /> with a valid <i>URN</i> (<i>RFC 8141</i>).
    /// </summary>
    /// <param name="urn">
    ///     The <i>URN</i>, e.g., <c>urn:example:errors:out-of-credit</c>; non-ASCII characters must be percent-encoded.
    /// </param>
    /// <returns>A new instance of the <see cref="ErrorUri" /> with the specified <i>URN</i>.</returns>
    /// <exception cref="ArgumentNullException">If the <paramref name="urn" /> is <see langword="null" />.</exception>
    /// <exception cref="UriFormatException">If the <paramref name="urn" /> is not a valid <i>URN</i>.</exception>
    public static ErrorUri Urn(string urn)
    {
        ArgumentNullException.ThrowIfNull(urn);
        UriUrnValidator.Validate(urn);

        return new ErrorUri(urn);
    }

    /// <summary>
    ///     Returns a new instance of the <see cref="ErrorUri" /> with a valid relative <i>URI</i> reference with the full path (<i>RFC 3986</i>, section 4.2).
    /// </summary>
    /// <param name="relativeReference">
    ///     The relative reference with the full path (e.g., <c>/account/12345/msgs/abc</c>), optionally followed by a query and a fragment;
    ///     non-ASCII characters must be percent-encoded.
    /// </param>
    /// <returns>A new instance of the <see cref="ErrorUri" /> with the specified relative reference.</returns>
    /// <remarks>
    ///     <para>
    ///         A relative reference is resolved against the base <i>URI</i> of the response.
    ///         <i>RFC 9457</i> (sections 3.1.1 and 3.1.5) allows it in both <see cref="Error.TypeUri" /> and <see cref="Error.InstanceUri" />,
    ///         but recommends absolute <i>URI</i>s when possible, because relative ones can cause confusion and might not be handled correctly by all
    ///         implementations. When a relative reference is used, it should include the full path.
    ///     </para>
    ///     <para>
    ///         Therefore, only a reference that starts with a single <c>/</c> is accepted: a relative path (<c>msgs/abc</c>),
    ///         a network-path reference (<c>//host/path</c>) and an empty reference are rejected.
    ///     </para>
    /// </remarks>
    /// <exception cref="ArgumentNullException">If the <paramref name="relativeReference" /> is <see langword="null" />.</exception>
    /// <exception cref="UriFormatException">If the <paramref name="relativeReference" /> is not a valid relative reference.</exception>
    public static ErrorUri Relative(string relativeReference)
    {
        ArgumentNullException.ThrowIfNull(relativeReference);
        UriRelativeValidator.Validate(relativeReference);

        return new ErrorUri(relativeReference);
    }

    /// <summary>
    ///     Returns the <i>URI</i> <see cref="Value" />.
    /// </summary>
    /// <returns>The <i>URI</i> value, e.g., <c>about:blank</c>.</returns>
    public override string ToString()
    {
        return Value;
    }

    #region equality

    /// <summary>
    ///     Determines whether the specified <see cref="ErrorUri" /> is equal to the current one,
    ///     comparing the <see cref="Value" /> rather than the underlying field, so that a default instance is equal to the one returned by <see cref="None" />.
    /// </summary>
    /// <param name="other">The <see cref="ErrorUri" /> to compare with the current instance.</param>
    /// <returns>
    ///     <see langword="true" /> if the specified <see cref="ErrorUri" /> is equal to the current one; otherwise, <see langword="false" />.
    /// </returns>
    public bool Equals(ErrorUri other)
    {
        return Value == other.Value;
    }

    /// <summary>
    ///     Returns a hash code that is consistent with <see cref="Equals(ErrorUri)" />.
    /// </summary>
    /// <returns>A hash code for the current <see cref="ErrorUri" />.</returns>
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    #endregion
}
