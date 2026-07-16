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

namespace Maple.Result.Validators;

internal static class UriLocatorValidator
{
    internal static void Validate(string uriLocator)
    {
        Uri uri;

        try
        {
            uri = new Uri(uriLocator);
        }
        catch (Exception ex) when (ex is not ArgumentNullException)
        {
            throw new UriFormatException(
                "The URI locator is not valid. Check https://datatracker.ietf.org/doc/html/rfc3986#section-3.1 for details.",
                ex);
        }

        if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
        {
            throw new UriFormatException(
                "The URI locator is not valid. Check https://datatracker.ietf.org/doc/html/rfc3986#section-3.1 for details.");
        }
    }
}
