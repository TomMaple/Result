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
using System.Text.RegularExpressions;

namespace Maple.Result.Validators;

internal static partial class UriRelativeValidator
{
    private const string Message =
        "The relative URI reference is not valid. Check https://datatracker.ietf.org/doc/html/rfc3986#section-4.2 for details.";

    // RFC 3986: pchar = unreserved / pct-encoded / sub-delims / ":" / "@"
    private const string PChar = @"(?:[A-Za-z0-9\-._~!$&'()*+,;=:@]|%[0-9A-Fa-f]{2})";

    // RFC 3986: relative-ref  = relative-part [ "?" query ] [ "#" fragment ]
    //           relative-part = "//" authority path-abempty / path-absolute / path-noscheme / path-empty
    //           path-absolute = "/" [ segment-nz *( "/" segment ) ]
    //           query / fragment = *( pchar / "/" / "?" )
    // Only a path-absolute ("/a/b") is accepted, as RFC 9457 (sections 3.1.1 and 3.1.5) asks relative URIs to include the full path.
    // A network-path reference ("//host/a") names another server, and a relative path ("a/b") depends on the path of the base URI.
    [GeneratedRegex(
        // path-absolute
        "^/(?:" + PChar + "+(?:/" + PChar + "*)*)?" +
        // query and fragment
        @"(?:\?(?:" + PChar + @"|[/?])*)?" +
        "(?:#(?:" + PChar + @"|[/?])*)?\z",
        RegexOptions.CultureInvariant)]
    private static partial Regex RelativePattern();

    internal static void Validate(string relativeReference)
    {
        if (!RelativePattern().IsMatch(relativeReference))
            throw new UriFormatException(Message);
    }
}
