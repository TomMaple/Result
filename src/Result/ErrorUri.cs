using Maple.Result.Validators;

namespace Maple.Result;

/// <summary>
///     Represents a URI that identifies or describes the type or category of an error in a standardized format.
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
///             <description>the URI of the human-readable documentation for the type of problem,</description>
///         </item>
///         <item>
///             <term>URI tag</term>
///             <description>
///                 the non-dereferenceable, unique representation of the specific type of problem
///                 (e.g., <example><c>tag:example@example.org,2021-09-17:OutOfLuck</c></example>).
///             </description>
///         </item>
///     </list>
///     For URI locator specification, see also: <seealso href="https://datatracker.ietf.org/doc/html/rfc3986" />.<br />
///     For URI tag specification, see also: <seealso href="https://datatracker.ietf.org/doc/html/rfc4151" />
/// </remarks>
public readonly record struct ErrorUri
{
    #region constructors

    public ErrorUri(string value)
    {
        Value = value;
    }

    #endregion

    /// <summary>
    ///     The URI value representing the error type or category.
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
    ///     Returns a new instance of the <see cref="ErrorUri" /> with a valid URI locator.
    /// </summary>
    public static ErrorUri Locator(string uriLocator)
    {
        UriLocatorValidator.Validate(uriLocator);

        return new ErrorUri(uriLocator);
    }

    /// <summary>
    ///     Returns a new instance of the <see cref="ErrorUri" /> with a valid URI tag.
    /// </summary>
    public static ErrorUri Tag(string uriTag)
    {
        UriTagValidator.Validate(uriTag);

        return new ErrorUri(uriTag);
    }
}
