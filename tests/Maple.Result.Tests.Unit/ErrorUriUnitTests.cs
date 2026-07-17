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

namespace Maple.Result.Tests.Unit;

public class ErrorUriUnitTests
{
    #region None

    [Fact]
    public void None_Always_ReturnsErrorUriWithAboutBlank()
    {
        // Act
        var result = ErrorUri.None();

        // Assert
        result.Value.ShouldBe("about:blank");
    }

    #endregion

    #region default instance

    [Fact]
    public void Value_DefaultInstance_ReturnsAboutBlank()
    {
        // Arrange
        const string ExpectedValue = "about:blank";

        // Act
        var result = default(ErrorUri);

        // Assert
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public void Value_ConstructedWithNull_ReturnsAboutBlank()
    {
        // Arrange
        const string ExpectedValue = "about:blank";

        // Act
        var result = new ErrorUri(null!);

        // Assert
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public void Equals_DefaultInstanceAndNone_AreEqual()
    {
        // Arrange
        var first = default(ErrorUri);
        var second = ErrorUri.None();

        // Act & Assert
        first.Equals(second).ShouldBeTrue();
        (first == second).ShouldBeTrue();
    }

    [Fact]
    public void GetHashCode_DefaultInstanceAndNone_AreEqual()
    {
        // Arrange
        var first = default(ErrorUri);
        var second = ErrorUri.None();

        // Act & Assert
        first.GetHashCode().ShouldBe(second.GetHashCode());
    }

    [Fact]
    public void Equals_ErrorUrisWithDifferentValues_AreNotEqual()
    {
        // Arrange
        var first = ErrorUri.None();
        var second = ErrorUri.Locator("https://example.com");

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
    }

    #endregion

    #region Locator

    [Theory]
    [InlineData("http://example.com")]
    [InlineData("https://example.com/probs/out-of-credit")]
    public void Locator_ValidUriLocator_ReturnsErrorUriWithUriLocator(string uriValue)
    {
        // Act
        var result = ErrorUri.Locator(uriValue);

        // Assert
        result.Value.ShouldBe(uriValue);
    }

    [Fact]
    public void Locator_NullLocator_ThrowsException()
    {
        // Arrange
        const string? Locator = null;

        // Act
        var exception = Record.Exception(() => ErrorUri.Locator(Locator!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldContain("Value cannot be null");
    }

    [Fact]
    public void Locator_EmptyValue_ThrowsException()
    {
        // Act
        var exception = Record.Exception(() => ErrorUri.Locator(string.Empty));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<UriFormatException>();
        exception.Message.ShouldContain("The URI locator is not valid. Check https://datatracker.ietf.org/doc/html/rfc3986#section-3.1 for details.");
    }

    [Theory]
    [InlineData("error")]
    [InlineData("mailto:email@company.com")]
    [InlineData("tag:example.com,2004:1234")]
    [InlineData("tag:timothy@hpl.hp.com,2001:web/externalHome")]
    public void Locator_InvalidUriLocator_ThrowsException(string value)
    {
        // Act
        var exception = Record.Exception(() => ErrorUri.Locator(value));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<UriFormatException>();
        exception.Message.ShouldContain("The URI locator is not valid. Check https://datatracker.ietf.org/doc/html/rfc3986#section-3.1 for details.");
    }

    [Theory]
    [InlineData("mailto:email@company.com")]
    [InlineData("tag:example.com,2004:1234")]
    public void Locator_WellFormedUriWithNonHttpScheme_ThrowsWithoutWrappingAnotherException(string value)
    {
        // Act
        var exception = Record.Exception(() => ErrorUri.Locator(value));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<UriFormatException>();
        exception.InnerException.ShouldBeNull();
    }

    [Fact]
    public void Locator_MalformedUri_ThrowsWithPreservedInnerException()
    {
        // Act
        var exception = Record.Exception(() => ErrorUri.Locator("error"));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<UriFormatException>();
        exception.InnerException.ShouldNotBeNull();
    }

    #endregion

    #region Tag

    [Theory]
    [InlineData("tag:example@example.org,2021-09-17:OutOfLuck")]
    [InlineData("tag:example.com,2004:1234")]
    [InlineData("tag:example.com,2004-01:1234")]
    [InlineData("tag:example.com,2004-01-01:1234")]
    // Examples from RFC 4151 (https://datatracker.ietf.org/doc/html/rfc4151#section-2.1) and its Wikipedia article.
    [InlineData("tag:timothy@hpl.hp.com,2001:web/externalHome")]
    [InlineData("tag:sandro@w3.org,2004-05:Sandro")]
    [InlineData("tag:my-ids.com,2001-09-15:TimKindberg:presentations:UBath2004-05-19")]
    [InlineData("tag:blogger.com,1999:blog-555")]
    [InlineData("tag:yaml.org,2002:int")]
    [InlineData("tag:yaml.org,2002:int#section1")]
    // RFC 4151 section 2.4: the specific part may be empty (specific = *( pchar / "/" / "?" )).
    [InlineData("tag:example.com,2000:")]
    [InlineData("tag:example.com,2000-01-01:")]
    // RFC 4151 section 2.4: the authority name is case-sensitive but upper-case letters are syntactically allowed.
    [InlineData("tag:EXAMPLE.com,2000:")]
    // RFC 4151 section 2.4: a future date is syntactically well-formed; the "date held" rule is a semantic
    // constraint this syntactic validator does not (and cannot) enforce.
    [InlineData("tag:hp.com,2999:")]
    // RFC 3986 pchar: non-ASCII must be percent-encoded UTF-8, and an internationalized domain name must use its
    // punycode (xn--) form. Both are pure ASCII and therefore valid.
    [InlineData("tag:example.com,2004:caf%C3%A9")]        // "café" with the "é" percent-encoded
    [InlineData("tag:example.com,2004:%F0%9F%98%80")]     // an emoji percent-encoded
    [InlineData("tag:xn--mnchen-3ya.de,2004:x")]          // "münchen.de" in punycode
    [InlineData("tag:example.com,2004:a?q/b")]            // "?" and "/" are allowed in the specific part
    [InlineData("tag:example.com,2004:a=b&c;d")]          // sub-delims are allowed in the specific part
    [InlineData("tag:example.com,2004:~a.b_c-d")]         // unreserved marks are allowed in the specific part
    public void Tag_ValidUriTag_ReturnsErrorUriWithUriTag(string tagValue)
    {
        // Act
        var result = ErrorUri.Tag(tagValue);

        // Assert
        result.Value.ShouldBe(tagValue);
    }

    [Fact]
    public void Tag_NullLocator_ThrowsException()
    {
        // Arrange
        const string? Tag = null;

        // Act
        var exception = Record.Exception(() => ErrorUri.Tag(Tag!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldContain("Value cannot be null");
    }

    [Fact]
    public void Tag_EmptyValue_ThrowsException()
    {
        // Act
        var exception = Record.Exception(() => ErrorUri.Tag(string.Empty));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<UriFormatException>();
        exception.Message.ShouldContain("The URI tag is not valid. Check https://datatracker.ietf.org/doc/html/rfc4151#section-2.1 for details.");
    }

    [Theory]
    [InlineData("error")]
    [InlineData("mailto:email@company.com")]
    [InlineData("https://www.company.com")]
    // Well-formed "tag:" scheme but structurally invalid taggingEntity.
    [InlineData("tag:")]
    [InlineData("tag:@@@")]
    [InlineData("tag:no-date-here")]
    [InlineData("tag:example.com:1234")]
    [InlineData("tag:,2004:1234")]
    [InlineData("tag:example.com,2004")]
    // Invalid date components: year not starting with 1 or 2, wrong digit count, or out-of-range month/day.
    [InlineData("tag:example.com,204:1234")]
    [InlineData("tag:example.com,0204:1234")]
    [InlineData("tag:example.com,3004:1234")]
    [InlineData("tag:example.com,2004-13:1234")]
    [InlineData("tag:example.com,2004-00:1234")]
    [InlineData("tag:example.com,2004-1:1234")]
    [InlineData("tag:example.com,2004-01-32:1234")]
    [InlineData("tag:example.com,2004-01-00:1234")]
    [InlineData("tag:example.com,2004-01-1:1234")]
    // Raw (non-percent-encoded) Unicode is outside the ASCII-only RFC 4151 / RFC 3986 grammar, whether in the
    // specific part or the authority (which would need its punycode form). The emoji uses a \U escape as it is
    // outside the Basic Multilingual Plane; the file is saved as UTF-8, so the other literals are safe.
    [InlineData("tag:example.com,2004:café")]        // "café" with a literal "é"
    [InlineData("tag:example.com,2004:日本")]         // literal CJK ("日本") in the specific part
    [InlineData("tag:example.com,2004:\U0001F600")]  // a literal emoji in the specific part
    [InlineData("tag:münchen.de,2004:x")]            // literal "ü" in the authority (needs punycode instead)
    [InlineData("tag:例.com,2004:x")]                // literal CJK ("例") in the authority
    public void Tag_InvalidUriTag_ThrowsException(string value)
    {
        // Act
        var exception = Record.Exception(() => ErrorUri.Tag(value));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<UriFormatException>();
        exception.Message.ShouldContain("The URI tag is not valid. Check https://datatracker.ietf.org/doc/html/rfc4151#section-2.1 for details.");
    }

    [Theory]
    [InlineData("mailto:email@company.com")]
    [InlineData("https://www.company.com")]
    public void Tag_WellFormedUriWithNonTagScheme_ThrowsWithoutWrappingAnotherException(string value)
    {
        // Act
        var exception = Record.Exception(() => ErrorUri.Tag(value));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<UriFormatException>();
        exception.InnerException.ShouldBeNull();
    }

    [Fact]
    public void Tag_MalformedUri_ThrowsWithPreservedInnerException()
    {
        // Act
        var exception = Record.Exception(() => ErrorUri.Tag("error"));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<UriFormatException>();
        exception.InnerException.ShouldNotBeNull();
    }

    #endregion
}
