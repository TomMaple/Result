using System;

namespace Maple.Result.Validators;

internal static class UriTagValidator
{
    internal static void Validate(string uriTag)
    {
        try
        {
            var uri = new Uri(uriTag);
            if (uri.Scheme != "tag")
            {
                throw new UriFormatException(
                    "The URI tag is not valid. Check https://datatracker.ietf.org/doc/html/rfc4151#section-2.1 for details.");
            }
        }
        catch (Exception ex) when (ex is not ArgumentNullException)
        {
            throw new UriFormatException(
                "The URI tag is not valid. Check https://datatracker.ietf.org/doc/html/rfc4151#section-2.1 for details.",
                ex);
        }
    }
}
