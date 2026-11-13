// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Result.Tests.Unit.Helpers;
using System;
using System.Collections.Generic;
using Sut = Maple.Result.Error;

namespace Maple.Result.Tests.Unit;

public class ErrorUnitTests
{
    #region construction checks

    [Fact]
    public void Validation_DefaultErrorUri_ReturnsErrorWithAboutBlankTypeUri()
    {
        // Arrange
        const string ExpectedTypeUri = "about:blank";

        // Act
        var sut = Sut.Validation(default, "Test title");

        // Assert
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void Validation_NullTitle_ThrowsArgumentNullException()
    {
        // Act
        var exception = Record.Exception(() => Sut.Validation(ErrorUri.None(), null!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Validation_BlankTitle_ThrowsArgumentException(string title)
    {
        // Act
        var exception = Record.Exception(() => Sut.Validation(ErrorUri.None(), title));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Validation_BlankDetailTemplateId_ThrowsArgumentException(string detailTemplateId)
    {
        // Act
        var exception = Record.Exception(() => Sut.Validation(ErrorUri.None(), "Test title",
            detailTemplateId: detailTemplateId));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentException>();
    }

    [Fact]
    public void Validation_NullNamedValue_KeepsNullValue()
    {
        // Act
        var sut = Sut.Validation(ErrorUri.None(), "Test title", detailTemplateId: "message-id",
            detailNamedValues: ("key", null));

        // Assert
        sut.DetailTemplated.ShouldNotBeNull();
        sut.DetailTemplated.Params.ShouldNotBeNull();
        sut.DetailTemplated.Params.ShouldContainKey("key");
        sut.DetailTemplated.Params["key"].ShouldBeNull();
    }

    #endregion

    #region WithErrorDetail

    [Fact]
    public void WithErrorDetail_DetailTextOnly_ReturnsCopyWithDetail()
    {
        // Arrange
        const int ExpectedErrorDetailsCount = 1;
        const string ExpectedErrorDetail1Detail = "Property 1 test detail";

        var sut = Sut.Timeout(ErrorUri.Tag("tag:test.com,2024:Test"), "Test title");

        // Act
        var result = sut.WithErrorDetail(null, "Property 1 test detail");

        // Assert
        result.ShouldNotBeNull();
        result.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        result.ErrorDetails[0].PropertyPointer.ShouldBeNull();
        result.ErrorDetails[0].Detail.ShouldBe(ExpectedErrorDetail1Detail);
        result.ErrorDetails[0].DetailTemplated.ShouldBeNull();
    }

    [Fact]
    public void WithErrorDetail_AllDetails_ReturnsCopyWithDetail()
    {
        // Arrange
        const int ExpectedErrorDetailsCount = 1;
        const string ExpectedErrorDetail1PropertyPointer = "#/property1";
        const string ExpectedErrorDetail1Detail = "Property 1 test detail";
        const string ExpectedErrorDetail1DetailTemplateId = "message-property-id";
        const int ExpectedErrorDetail1DetailParamsCount = 2;
        const string ExpectedErrorDetail1DetailParam1Key = "pk1";
        const string ExpectedErrorDetail1DetailParam1Value = "pv1";
        const string ExpectedErrorDetail1DetailParam2Key = "pk2";
        const string ExpectedErrorDetail1DetailParam2Value = "pv2";

        var sut = Sut.Timeout(ErrorUri.Tag("tag:test.com,2024:Test"), "Test title");

        // Act
        var result = sut.WithErrorDetail("#/property1", "Property 1 test detail", "message-property-id",
            ("pk1", "pv1"), ("pk2", "pv2"));

        // Assert
        result.ShouldNotBeNull();
        result.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        result.ErrorDetails[0].PropertyPointer.ShouldBe(ExpectedErrorDetail1PropertyPointer);
        result.ErrorDetails[0].Detail.ShouldBe(ExpectedErrorDetail1Detail);
        result.ErrorDetails[0].DetailTemplated.ShouldNotBeNull();
        result.ErrorDetails[0].DetailTemplated!.TemplateId.ShouldBe(ExpectedErrorDetail1DetailTemplateId);
        result.ErrorDetails[0].DetailTemplated!.Params.ShouldNotBeNull();
        result.ErrorDetails[0].DetailTemplated!.Params!.Count.ShouldBe(ExpectedErrorDetail1DetailParamsCount);
        result.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam1Key].ShouldBe(ExpectedErrorDetail1DetailParam1Value);
        result.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam2Key].ShouldBe(ExpectedErrorDetail1DetailParam2Value);
    }

    [Fact]
    public void WithErrorDetail_ErrorDetailInstance_AppendsItAfterExistingDetails()
    {
        // Arrange
        var detail = new ErrorDetail("#/second", "Second detail");
        var sut = Sut.Validation(ErrorUri.None(), "Test title").WithErrorDetail("#/first", "First detail");

        // Act
        var result = sut.WithErrorDetail(detail);

        // Assert
        result.ErrorDetails.Count.ShouldBe(2);
        result.ErrorDetails[0].PropertyPointer.ShouldBe("#/first");
        result.ErrorDetails[1].ShouldBeSameAs(detail);
    }

    [Fact]
    public void WithErrorDetail_Always_LeavesOriginalUnchanged()
    {
        // Arrange
        var sut = Sut.Validation(ErrorUri.None(), "Test title").WithErrorDetail("#/first", "First detail");

        // Act
        var result = sut.WithErrorDetail("#/second", "Second detail");

        // Assert
        result.ShouldNotBeSameAs(sut);
        sut.ErrorDetails.Count.ShouldBe(1);
        result.ErrorDetails.Count.ShouldBe(2);
        result.Title.ShouldBe(sut.Title);
        result.Category.ShouldBe(sut.Category);
    }

    [Fact]
    public void WithErrorDetail_NullErrorDetail_ThrowsArgumentNullException()
    {
        // Arrange
        var sut = Sut.Validation(ErrorUri.None(), "Test title");

        // Act
        var exception = Record.Exception(() => sut.WithErrorDetail(null!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
    }

    [Fact]
    public void WithErrorDetail_NullDetailText_ThrowsArgumentNullException()
    {
        // Arrange
        var sut = Sut.Validation(ErrorUri.None(), "Test title");

        // Act
        var exception = Record.Exception(() => sut.WithErrorDetail("#/property", null!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
    }

    [Fact]
    public void ErrorDetails_CastToMutableList_CannotBeModified()
    {
        // Arrange
        var sut = Sut.Validation(ErrorUri.None(), "Test title").WithErrorDetail("#/property", "Detail");

        // Act
        var exception = Record.Exception(() => ((IList<ErrorDetail>)sut.ErrorDetails).Add(new ErrorDetail(null, "x")));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<NotSupportedException>();
        sut.ErrorDetails.Count.ShouldBe(1);
    }

    #endregion

    #region ToBuilder

    [Fact]
    public void ToBuilder_BuildWithoutChanges_ReturnsEqualError()
    {
        // Arrange
        var sut = Sut.Conflict(ErrorUri.Tag("tag:test.com,2024:Test"), "Test title", "Test description.",
                ErrorUri.Locator("http://test.com/instance/1013"), "message-id", ("key", "value"))
            .WithErrorDetail("#/property", "Detail")
            .WithMetadata("retryAfter", 30);

        // Act
        var result = sut.ToBuilder().Build();

        // Assert
        result.ShouldBe(sut);
        result.ShouldNotBeSameAs(sut);
    }

    [Fact]
    public void ToBuilder_ChangedTitle_ReturnsCopyAndLeavesOriginalUnchanged()
    {
        // Arrange
        var sut = Sut.Conflict(ErrorUri.None(), "Original title").WithErrorDetail("#/property", "Detail");

        // Act
        var result = sut.ToBuilder().WithTitle("Changed title").Build();

        // Assert
        result.Title.ShouldBe("Changed title");
        result.ErrorDetails.ShouldBe(sut.ErrorDetails);
        sut.Title.ShouldBe("Original title");
    }

    #endregion

    #region ToString

    [Fact]
    public void ToString_DetailsAndMetadata_WritesTheirContent()
    {
        // Arrange
        const string Expected = "Error { Category = Validation, TypeUri = https://example.org/e, Title = Bad, Detail = , "
            + "DetailTemplated = TemplatedMessage { TemplateId = t, Params = { a = 1 } }, InstanceUri = , "
            + "ErrorDetails = [ ErrorDetail { PropertyPointer = /x, Detail = bad x, DetailTemplated =  } ], "
            + "Metadata = { n =  } }";

        var sut = Sut.Validation(ErrorUri.Locator("https://example.org/e"), "Bad", detailTemplateId: "t",
                detailNamedValues: ("a", 1))
            .WithErrorDetail("/x", "bad x")
            .WithMetadata("n", null);

        // Act
        var result = sut.ToString();

        // Assert
        result.ShouldBe(Expected);
    }

    [Fact]
    public void ToString_NoDetailsOrMetadata_WritesEmptyCollections()
    {
        // Arrange
        const string Expected = "Error { Category = NotFound, TypeUri = about:blank, Title = Missing, Detail = , "
            + "DetailTemplated = , InstanceUri = , ErrorDetails = [], Metadata = {} }";

        var sut = Sut.NotFound(ErrorUri.None(), "Missing");

        // Act
        var result = sut.ToString();

        // Assert
        result.ShouldBe(Expected);
    }

    #endregion

    #region factory methods

    #region Validation

    [Fact]
    public void Validation_OnlyRequiredDetails_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.Validation;
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.Validation(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title");

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBeNull();
        sut.DetailTemplated.ShouldBeNull();
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBeNull();
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void Validation_WithDetailsCollection_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.Validation;
        const string ExpectedDetail = "Test description.";
        const string ExpectedDetailTemplateId = "message-id";
        const int ExpectedDetailParamsCount = 2;
        const string ExpectedDetailParam1Key = "key1";
        const string ExpectedDetailParam1Value = "value1";
        const string ExpectedDetailParam2Key = "key2";
        const string ExpectedDetailParam2Value = "value2";
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedInstance = "http://test.com/instance/1013";
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.Validation(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            ("key1", "value1"), ("key2", "value2"));

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBe(ExpectedDetail);
        sut.DetailTemplated.ShouldNotBeNull();
        sut.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        sut.DetailTemplated.Params.ShouldNotBeNull();
        sut.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void Validation_WithDetailsDictionary_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.Validation;
        const string ExpectedDetail = "Test description.";
        const string ExpectedDetailTemplateId = "message-id";
        const int ExpectedDetailParamsCount = 2;
        const string ExpectedDetailParam1Key = "key1";
        const string ExpectedDetailParam1Value = "value1";
        const string ExpectedDetailParam2Key = "key2";
        const string ExpectedDetailParam2Value = "value2";
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedInstance = "http://test.com/instance/1013";
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.Validation(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            new Dictionary<string, object?>
            {
                ["key1"] = "value1",
                ["key2"] = "value2"
            });

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBe(ExpectedDetail);
        sut.DetailTemplated.ShouldNotBeNull();
        sut.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        sut.DetailTemplated.Params.ShouldNotBeNull();
        sut.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    #endregion

    #region Unauthenticated

    [Fact]
    public void Unauthenticated_OnlyRequiredDetails_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.Unauthenticated;
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.Unauthenticated(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title");

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBeNull();
        sut.DetailTemplated.ShouldBeNull();
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBeNull();
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void Unauthenticated_WithDetailsCollection_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.Unauthenticated;
        const string ExpectedDetail = "Test description.";
        const string ExpectedDetailTemplateId = "message-id";
        const int ExpectedDetailParamsCount = 2;
        const string ExpectedDetailParam1Key = "key1";
        const string ExpectedDetailParam1Value = "value1";
        const string ExpectedDetailParam2Key = "key2";
        const string ExpectedDetailParam2Value = "value2";
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedInstance = "http://test.com/instance/1013";
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.Unauthenticated(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            ("key1", "value1"), ("key2", "value2"));

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBe(ExpectedDetail);
        sut.DetailTemplated.ShouldNotBeNull();
        sut.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        sut.DetailTemplated.Params.ShouldNotBeNull();
        sut.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void Unauthenticated_WithDetailsDictionary_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.Unauthenticated;
        const string ExpectedDetail = "Test description.";
        const string ExpectedDetailTemplateId = "message-id";
        const int ExpectedDetailParamsCount = 2;
        const string ExpectedDetailParam1Key = "key1";
        const string ExpectedDetailParam1Value = "value1";
        const string ExpectedDetailParam2Key = "key2";
        const string ExpectedDetailParam2Value = "value2";
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedInstance = "http://test.com/instance/1013";
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.Unauthenticated(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            new Dictionary<string, object?>
            {
                ["key1"] = "value1",
                ["key2"] = "value2"
            });

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBe(ExpectedDetail);
        sut.DetailTemplated.ShouldNotBeNull();
        sut.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        sut.DetailTemplated.Params.ShouldNotBeNull();
        sut.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    #endregion

    #region Unauthorized

    [Fact]
    public void Unauthorized_OnlyRequiredDetails_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.Unauthorized;
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.Unauthorized(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title");

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBeNull();
        sut.DetailTemplated.ShouldBeNull();
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBeNull();
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void Unauthorized_WithDetailsCollection_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.Unauthorized;
        const string ExpectedDetail = "Test description.";
        const string ExpectedDetailTemplateId = "message-id";
        const int ExpectedDetailParamsCount = 2;
        const string ExpectedDetailParam1Key = "key1";
        const string ExpectedDetailParam1Value = "value1";
        const string ExpectedDetailParam2Key = "key2";
        const string ExpectedDetailParam2Value = "value2";
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedInstance = "http://test.com/instance/1013";
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.Unauthorized(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            ("key1", "value1"), ("key2", "value2"));

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBe(ExpectedDetail);
        sut.DetailTemplated.ShouldNotBeNull();
        sut.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        sut.DetailTemplated.Params.ShouldNotBeNull();
        sut.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void Unauthorized_WithDetailsDictionary_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.Unauthorized;
        const string ExpectedDetail = "Test description.";
        const string ExpectedDetailTemplateId = "message-id";
        const int ExpectedDetailParamsCount = 2;
        const string ExpectedDetailParam1Key = "key1";
        const string ExpectedDetailParam1Value = "value1";
        const string ExpectedDetailParam2Key = "key2";
        const string ExpectedDetailParam2Value = "value2";
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedInstance = "http://test.com/instance/1013";
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.Unauthorized(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            new Dictionary<string, object?>
            {
                ["key1"] = "value1",
                ["key2"] = "value2"
            });

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBe(ExpectedDetail);
        sut.DetailTemplated.ShouldNotBeNull();
        sut.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        sut.DetailTemplated.Params.ShouldNotBeNull();
        sut.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    #endregion

    #region NotFound

    [Fact]
    public void NotFound_OnlyRequiredDetails_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.NotFound;
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.NotFound(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title");

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBeNull();
        sut.DetailTemplated.ShouldBeNull();
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBeNull();
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void NotFound_WithDetailsCollection_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.NotFound;
        const string ExpectedDetail = "Test description.";
        const string ExpectedDetailTemplateId = "message-id";
        const int ExpectedDetailParamsCount = 2;
        const string ExpectedDetailParam1Key = "key1";
        const string ExpectedDetailParam1Value = "value1";
        const string ExpectedDetailParam2Key = "key2";
        const string ExpectedDetailParam2Value = "value2";
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedInstance = "http://test.com/instance/1013";
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.NotFound(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            ("key1", "value1"), ("key2", "value2"));

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBe(ExpectedDetail);
        sut.DetailTemplated.ShouldNotBeNull();
        sut.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        sut.DetailTemplated.Params.ShouldNotBeNull();
        sut.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void NotFound_WithDetailsDictionary_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.NotFound;
        const string ExpectedDetail = "Test description.";
        const string ExpectedDetailTemplateId = "message-id";
        const int ExpectedDetailParamsCount = 2;
        const string ExpectedDetailParam1Key = "key1";
        const string ExpectedDetailParam1Value = "value1";
        const string ExpectedDetailParam2Key = "key2";
        const string ExpectedDetailParam2Value = "value2";
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedInstance = "http://test.com/instance/1013";
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.NotFound(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            new Dictionary<string, object?>
            {
                ["key1"] = "value1",
                ["key2"] = "value2"
            });

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBe(ExpectedDetail);
        sut.DetailTemplated.ShouldNotBeNull();
        sut.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        sut.DetailTemplated.Params.ShouldNotBeNull();
        sut.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    #endregion

    #region Timeout

    [Fact]
    public void Timeout_OnlyRequiredDetails_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.Timeout;
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.Timeout(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title");

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBeNull();
        sut.DetailTemplated.ShouldBeNull();
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBeNull();
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void Timeout_WithDetailsCollection_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.Timeout;
        const string ExpectedDetail = "Test description.";
        const string ExpectedDetailTemplateId = "message-id";
        const int ExpectedDetailParamsCount = 2;
        const string ExpectedDetailParam1Key = "key1";
        const string ExpectedDetailParam1Value = "value1";
        const string ExpectedDetailParam2Key = "key2";
        const string ExpectedDetailParam2Value = "value2";
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedInstance = "http://test.com/instance/1013";
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.Timeout(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            ("key1", "value1"), ("key2", "value2"));

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBe(ExpectedDetail);
        sut.DetailTemplated.ShouldNotBeNull();
        sut.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        sut.DetailTemplated.Params.ShouldNotBeNull();
        sut.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void Timeout_WithDetailsDictionary_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.Timeout;
        const string ExpectedDetail = "Test description.";
        const string ExpectedDetailTemplateId = "message-id";
        const int ExpectedDetailParamsCount = 2;
        const string ExpectedDetailParam1Key = "key1";
        const string ExpectedDetailParam1Value = "value1";
        const string ExpectedDetailParam2Key = "key2";
        const string ExpectedDetailParam2Value = "value2";
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedInstance = "http://test.com/instance/1013";
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.Timeout(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            new Dictionary<string, object?>
            {
                ["key1"] = "value1",
                ["key2"] = "value2"
            });

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBe(ExpectedDetail);
        sut.DetailTemplated.ShouldNotBeNull();
        sut.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        sut.DetailTemplated.Params.ShouldNotBeNull();
        sut.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    #endregion

    #region Conflict

    [Fact]
    public void Conflict_OnlyRequiredDetails_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.Conflict;
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.Conflict(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title");

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBeNull();
        sut.DetailTemplated.ShouldBeNull();
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBeNull();
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void Conflict_WithDetailsCollection_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.Conflict;
        const string ExpectedDetail = "Test description.";
        const string ExpectedDetailTemplateId = "message-id";
        const int ExpectedDetailParamsCount = 2;
        const string ExpectedDetailParam1Key = "key1";
        const string ExpectedDetailParam1Value = "value1";
        const string ExpectedDetailParam2Key = "key2";
        const string ExpectedDetailParam2Value = "value2";
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedInstance = "http://test.com/instance/1013";
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.Conflict(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            ("key1", "value1"), ("key2", "value2"));

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBe(ExpectedDetail);
        sut.DetailTemplated.ShouldNotBeNull();
        sut.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        sut.DetailTemplated.Params.ShouldNotBeNull();
        sut.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void Conflict_WithDetailsDictionary_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.Conflict;
        const string ExpectedDetail = "Test description.";
        const string ExpectedDetailTemplateId = "message-id";
        const int ExpectedDetailParamsCount = 2;
        const string ExpectedDetailParam1Key = "key1";
        const string ExpectedDetailParam1Value = "value1";
        const string ExpectedDetailParam2Key = "key2";
        const string ExpectedDetailParam2Value = "value2";
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedInstance = "http://test.com/instance/1013";
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.Conflict(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            new Dictionary<string, object?>
            {
                ["key1"] = "value1",
                ["key2"] = "value2"
            });

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBe(ExpectedDetail);
        sut.DetailTemplated.ShouldNotBeNull();
        sut.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        sut.DetailTemplated.Params.ShouldNotBeNull();
        sut.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    #endregion

    #region Failure

    [Fact]
    public void Failure_OnlyRequiredDetails_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.Failure;
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.Failure(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title");

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBeNull();
        sut.DetailTemplated.ShouldBeNull();
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBeNull();
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void Failure_WithDetailsCollection_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.Failure;
        const string ExpectedDetail = "Test description.";
        const string ExpectedDetailTemplateId = "message-id";
        const int ExpectedDetailParamsCount = 2;
        const string ExpectedDetailParam1Key = "key1";
        const string ExpectedDetailParam1Value = "value1";
        const string ExpectedDetailParam2Key = "key2";
        const string ExpectedDetailParam2Value = "value2";
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedInstance = "http://test.com/instance/1013";
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.Failure(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            ("key1", "value1"), ("key2", "value2"));

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBe(ExpectedDetail);
        sut.DetailTemplated.ShouldNotBeNull();
        sut.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        sut.DetailTemplated.Params.ShouldNotBeNull();
        sut.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void Failure_WithDetailsDictionary_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.Failure;
        const string ExpectedDetail = "Test description.";
        const string ExpectedDetailTemplateId = "message-id";
        const int ExpectedDetailParamsCount = 2;
        const string ExpectedDetailParam1Key = "key1";
        const string ExpectedDetailParam1Value = "value1";
        const string ExpectedDetailParam2Key = "key2";
        const string ExpectedDetailParam2Value = "value2";
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedInstance = "http://test.com/instance/1013";
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.Failure(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            new Dictionary<string, object?>
            {
                ["key1"] = "value1",
                ["key2"] = "value2"
            });

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBe(ExpectedDetail);
        sut.DetailTemplated.ShouldNotBeNull();
        sut.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        sut.DetailTemplated.Params.ShouldNotBeNull();
        sut.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    #endregion

    #region Critical

    [Fact]
    public void Critical_OnlyRequiredDetails_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.Critical;
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.Critical(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title");

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBeNull();
        sut.DetailTemplated.ShouldBeNull();
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBeNull();
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void Critical_WithDetailsCollection_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.Critical;
        const string ExpectedDetail = "Test description.";
        const string ExpectedDetailTemplateId = "message-id";
        const int ExpectedDetailParamsCount = 2;
        const string ExpectedDetailParam1Key = "key1";
        const string ExpectedDetailParam1Value = "value1";
        const string ExpectedDetailParam2Key = "key2";
        const string ExpectedDetailParam2Value = "value2";
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedInstance = "http://test.com/instance/1013";
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.Critical(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            ("key1", "value1"), ("key2", "value2"));

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBe(ExpectedDetail);
        sut.DetailTemplated.ShouldNotBeNull();
        sut.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        sut.DetailTemplated.Params.ShouldNotBeNull();
        sut.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void Critical_WithDetailsDictionary_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.Critical;
        const string ExpectedDetail = "Test description.";
        const string ExpectedDetailTemplateId = "message-id";
        const int ExpectedDetailParamsCount = 2;
        const string ExpectedDetailParam1Key = "key1";
        const string ExpectedDetailParam1Value = "value1";
        const string ExpectedDetailParam2Key = "key2";
        const string ExpectedDetailParam2Value = "value2";
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedInstance = "http://test.com/instance/1013";
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.Critical(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            new Dictionary<string, object?>
            {
                ["key1"] = "value1",
                ["key2"] = "value2"
            });

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBe(ExpectedDetail);
        sut.DetailTemplated.ShouldNotBeNull();
        sut.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        sut.DetailTemplated.Params.ShouldNotBeNull();
        sut.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    #endregion

    #region NotImplemented

    [Fact]
    public void NotImplemented_OnlyRequiredDetails_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.NotImplemented;
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.NotImplemented(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title");

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBeNull();
        sut.DetailTemplated.ShouldBeNull();
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBeNull();
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void NotImplemented_WithDetailsCollection_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.NotImplemented;
        const string ExpectedDetail = "Test description.";
        const string ExpectedDetailTemplateId = "message-id";
        const int ExpectedDetailParamsCount = 2;
        const string ExpectedDetailParam1Key = "key1";
        const string ExpectedDetailParam1Value = "value1";
        const string ExpectedDetailParam2Key = "key2";
        const string ExpectedDetailParam2Value = "value2";
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedInstance = "http://test.com/instance/1013";
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.NotImplemented(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            ("key1", "value1"), ("key2", "value2"));

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBe(ExpectedDetail);
        sut.DetailTemplated.ShouldNotBeNull();
        sut.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        sut.DetailTemplated.Params.ShouldNotBeNull();
        sut.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void NotImplemented_WithDetailsDictionary_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.NotImplemented;
        const string ExpectedDetail = "Test description.";
        const string ExpectedDetailTemplateId = "message-id";
        const int ExpectedDetailParamsCount = 2;
        const string ExpectedDetailParam1Key = "key1";
        const string ExpectedDetailParam1Value = "value1";
        const string ExpectedDetailParam2Key = "key2";
        const string ExpectedDetailParam2Value = "value2";
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedInstance = "http://test.com/instance/1013";
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.NotImplemented(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            new Dictionary<string, object?>
            {
                ["key1"] = "value1",
                ["key2"] = "value2"
            });

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBe(ExpectedDetail);
        sut.DetailTemplated.ShouldNotBeNull();
        sut.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        sut.DetailTemplated.Params.ShouldNotBeNull();
        sut.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    #endregion

    #region Unavailable

    [Fact]
    public void Unavailable_OnlyRequiredDetails_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.Unavailable;
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.Unavailable(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title");

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBeNull();
        sut.DetailTemplated.ShouldBeNull();
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBeNull();
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void Unavailable_WithDetailsCollection_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.Unavailable;
        const string ExpectedDetail = "Test description.";
        const string ExpectedDetailTemplateId = "message-id";
        const int ExpectedDetailParamsCount = 2;
        const string ExpectedDetailParam1Key = "key1";
        const string ExpectedDetailParam1Value = "value1";
        const string ExpectedDetailParam2Key = "key2";
        const string ExpectedDetailParam2Value = "value2";
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedInstance = "http://test.com/instance/1013";
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.Unavailable(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            ("key1", "value1"), ("key2", "value2"));

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBe(ExpectedDetail);
        sut.DetailTemplated.ShouldNotBeNull();
        sut.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        sut.DetailTemplated.Params.ShouldNotBeNull();
        sut.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void Unavailable_WithDetailsDictionary_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.Unavailable;
        const string ExpectedDetail = "Test description.";
        const string ExpectedDetailTemplateId = "message-id";
        const int ExpectedDetailParamsCount = 2;
        const string ExpectedDetailParam1Key = "key1";
        const string ExpectedDetailParam1Value = "value1";
        const string ExpectedDetailParam2Key = "key2";
        const string ExpectedDetailParam2Value = "value2";
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedInstance = "http://test.com/instance/1013";
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.Unavailable(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            new Dictionary<string, object?>
            {
                ["key1"] = "value1",
                ["key2"] = "value2"
            });

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBe(ExpectedDetail);
        sut.DetailTemplated.ShouldNotBeNull();
        sut.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        sut.DetailTemplated.Params.ShouldNotBeNull();
        sut.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    #endregion

    #region LimitExceeded

    [Fact]
    public void LimitExceeded_OnlyRequiredDetails_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.LimitExceeded;
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.LimitExceeded(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title");

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBeNull();
        sut.DetailTemplated.ShouldBeNull();
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBeNull();
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void LimitExceeded_WithDetailsCollection_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.LimitExceeded;
        const string ExpectedDetail = "Test description.";
        const string ExpectedDetailTemplateId = "message-id";
        const int ExpectedDetailParamsCount = 2;
        const string ExpectedDetailParam1Key = "key1";
        const string ExpectedDetailParam1Value = "value1";
        const string ExpectedDetailParam2Key = "key2";
        const string ExpectedDetailParam2Value = "value2";
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedInstance = "http://test.com/instance/1013";
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.LimitExceeded(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            ("key1", "value1"), ("key2", "value2"));

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBe(ExpectedDetail);
        sut.DetailTemplated.ShouldNotBeNull();
        sut.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        sut.DetailTemplated.Params.ShouldNotBeNull();
        sut.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void LimitExceeded_WithDetailsDictionary_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.LimitExceeded;
        const string ExpectedDetail = "Test description.";
        const string ExpectedDetailTemplateId = "message-id";
        const int ExpectedDetailParamsCount = 2;
        const string ExpectedDetailParam1Key = "key1";
        const string ExpectedDetailParam1Value = "value1";
        const string ExpectedDetailParam2Key = "key2";
        const string ExpectedDetailParam2Value = "value2";
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedInstance = "http://test.com/instance/1013";
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.LimitExceeded(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            new Dictionary<string, object?>
            {
                ["key1"] = "value1",
                ["key2"] = "value2"
            });

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBe(ExpectedDetail);
        sut.DetailTemplated.ShouldNotBeNull();
        sut.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        sut.DetailTemplated.Params.ShouldNotBeNull();
        sut.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    #endregion

    #region ErrorCategory values

    [Fact]
    public void ErrorCategory_Unavailable_HasUnchangedNumericValue()
    {
        // Arrange
        const int ExpectedValue = 9;

        // Act
        var value = (int)ErrorCategory.Unavailable;

        // Assert
        value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public void ErrorCategory_LimitExceeded_IsAppendedAfterUnavailable()
    {
        // Arrange
        const int ExpectedValue = 10;

        // Act
        var value = (int)ErrorCategory.LimitExceeded;

        // Assert
        value.ShouldBe(ExpectedValue);
    }

    #endregion

    #endregion

    #region Serialize

    [Fact]
    public void Serialize_AllPropertiesAndMicrosoftSerializer_ReturnsSerializedValueWithAllProperties()
    {
        // Arrange
        const string ExpectedText = """{"Category":3,"TypeUri":"tag:test.com,2024:Test","Title":"Test title","Detail":"Test description.","DetailTemplated":{"TemplateId":"messageId","Params":{"key1":"value1","key2":"value2"}},"InstanceUri":"http://test.com/instance/1013","ErrorDetails":[{"PropertyPointer":"#/property1","Detail":"Property 1 test detail","DetailTemplated":{"TemplateId":"message-property-id","Params":{"pk1":"pv1","pk2":"pv2"}}}],"Metadata":{}}""";

        var sut = Sut.NotFound(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "messageId",
            ("key1", "value1"), ("key2", "value2"));

        sut = sut.WithErrorDetail("#/property1", "Property 1 test detail", "message-property-id", ("pk1", "pv1"), ("pk2", "pv2"));

        // Act
        var result = SerializationHelper.SerializeWithMicrosoft(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    [Fact]
    public void Serialize_AllPropertiesAndNewtonsoftSerializer_ReturnsSerializedValueWithAllProperties()
    {
        // Arrange
        const string ExpectedText = """{"Category":3,"TypeUri":"tag:test.com,2024:Test","Title":"Test title","Detail":"Test description.","DetailTemplated":{"TemplateId":"messageId","Params":{"key1":"value1","key2":"value2"}},"InstanceUri":"http://test.com/instance/1013","ErrorDetails":[{"PropertyPointer":"#/property1","Detail":"Property 1 test detail","DetailTemplated":{"TemplateId":"message-property-id","Params":{"pk1":"pv1","pk2":"pv2"}}}],"Metadata":{}}""";

        var sut = Sut.NotFound(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "messageId",
            ("key1", "value1"), ("key2", "value2"));

        sut = sut.WithErrorDetail("#/property1", "Property 1 test detail", "message-property-id", ("pk1", "pv1"), ("pk2", "pv2"));

        // Act
        var result = SerializationHelper.SerializeWithNewtonsoft(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    #endregion
}
