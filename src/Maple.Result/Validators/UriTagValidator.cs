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
using System.Text.RegularExpressions;

namespace Maple.Result.Validators;

internal static partial class UriTagValidator
{
    private const string Message =
        "The URI tag is not valid. Check https://datatracker.ietf.org/doc/html/rfc4151#section-2.1 for details.";

    // RFC 4151: tag = "tag:" taggingEntity ":" specific [ "#" fragment ]
    //   taggingEntity = authorityName "," date
    //   authorityName = DNSname / emailAddress
    //   date          = year ["-" month ["-" day]]
    // The date is validated partially: the year is 1xxx or 2xxx, the month is 01-12 and the day is 01-31, each as
    // two digits per the RFC's 2DIGIT grammar. This rejects impossible values such as month 13 or day 32 without
    // asserting a real calendar date (for example, it still accepts 02-30).
    [GeneratedRegex(
        "^tag:" +
        // authorityName = DNSname / emailAddress
        @"(?:[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?(?:\.[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?)*" +
        @"|[A-Za-z0-9._-]+@[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?(?:\.[A-Za-z0-9](?:[A-Za-z0-9-]*[A-Za-z0-9])?)*)" +
        // "," date
        @",[12][0-9]{3}(?:-(?:0[1-9]|1[0-2])(?:-(?:0[1-9]|[12][0-9]|3[01]))?)?" +
        // ":" specific [ "#" fragment ]
        @":[A-Za-z0-9\-._~%!$&'()*+,;=:@/?]*(?:#[A-Za-z0-9\-._~%!$&'()*+,;=:@/?]*)?$",
        RegexOptions.CultureInvariant)]
    private static partial Regex TagPattern();

    internal static void Validate(string uriTag)
    {
        Uri uri;

        try
        {
            uri = new Uri(uriTag);
        }
        catch (Exception ex) when (ex is not ArgumentNullException)
        {
            throw new UriFormatException(Message, ex);
        }

        if (uri.Scheme != "tag" || !TagPattern().IsMatch(uriTag))
            throw new UriFormatException(Message);
    }
}
