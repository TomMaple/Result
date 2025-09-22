using Maple.Result;
using System;

namespace Result.Tests.Unit;

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
        // Act
        var exception = Record.Exception(() => ErrorUri.Locator(null));

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

    #endregion

    #region Tag

    [Theory]
    [InlineData("tag:example@example.org,2021-09-17:OutOfLuck")]
    [InlineData("tag:example.com,2004:1234")]
    [InlineData("tag:example.com,2004-01:1234")]
    [InlineData("tag:example.com,2004-01-01:1234")]
    [InlineData("tag:timothy@hpl.hp.com,2001:web/externalHome")]
    [InlineData("tag:sandro@w3.org,2004-05:Sandro")]
    [InlineData("tag:my-ids.com,2001-09-15:TimKindberg:presentations:UBath2004-05-19")]
    [InlineData("tag:blogger.com,1999:blog-555")]
    [InlineData("tag:yaml.org,2002:int#section1")]
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
        // Act
        var exception = Record.Exception(() => ErrorUri.Tag(null));

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
    public void Tag_InvalidUriTag_ThrowsException(string value)
    {
        // Act
        var exception = Record.Exception(() => ErrorUri.Tag(value));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<UriFormatException>();
        exception.Message.ShouldContain("The URI tag is not valid. Check https://datatracker.ietf.org/doc/html/rfc4151#section-2.1 for details.");
    }

    #endregion
}