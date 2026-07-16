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

internal static class UriTagValidator
{
    internal static void Validate(string uriTag)
    {
        Uri uri;

        try
        {
            uri = new Uri(uriTag);
        }
        catch (Exception ex) when (ex is not ArgumentNullException)
        {
            throw new UriFormatException(
                "The URI tag is not valid. Check https://datatracker.ietf.org/doc/html/rfc4151#section-2.1 for details.",
                ex);
        }

        if (uri.Scheme != "tag")
        {
            throw new UriFormatException(
                "The URI tag is not valid. Check https://datatracker.ietf.org/doc/html/rfc4151#section-2.1 for details.");
        }
    }
}
