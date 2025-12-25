using Maple.Result.Tests.Unit.Helpers;
using System.Collections.Generic;
using Sut = Maple.Result.Error;

namespace Maple.Result.Tests.Unit;

public class ErrorUnitTests
{
    #region ctor

    [Fact]
    public void Ctor_OnlyRequiredDetails_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.Timeout;
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = new Sut(
            ErrorCategory.Timeout,
            "tag:test.com,2024:Test",
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
    public void Ctor_WithOptionalDetails_ReturnsError()
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
        const int ExpectedErrorDetailsCount = 1;
        const string ExpectedErrorDetail1PropertyPointer = "#/property1";
        const string ExpectedErrorDetail1Detail = "Property 1 test detail";
        const string ExpectedErrorDetail1DetailTemplateId = "message-property-id";
        const int ExpectedErrorDetail1DetailParamsCount = 2;
        const string ExpectedErrorDetail1DetailParam1Key = "pk1";
        const string ExpectedErrorDetail1DetailParam1Value = "pv1";
        const string ExpectedErrorDetail1DetailParam2Key = "pk2";
        const string ExpectedErrorDetail1DetailParam2Value = "pv2";
        const string ExpectedInstance = "http://test.com/instance/1013";
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = new Sut(
            ErrorCategory.Unauthenticated,
            "tag:test.com,2024:Test",
            "Test title",
            "Test description.",
            new TemplatedMessage(
                "message-id",
                new Dictionary<string, object>
                {
                    ["key1"] = "value1",
                    ["key2"] = "value2"
                }),
            "http://test.com/instance/1013",
            [
                new ErrorDetail(
                    "#/property1",
                    "Property 1 test detail",
                    new TemplatedMessage(
                        "message-property-id",
                        new Dictionary<string, object>
                        {
                            ["pk1"] = "pv1",
                            ["pk2"] = "pv2"
                        }))
            ]);

        // Assert
        sut.ShouldNotBeNull();
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBe(ExpectedDetail);
        sut.DetailTemplated.ShouldNotBeNull();
        sut.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        sut.DetailTemplated.Params.ShouldNotBeNull();
        sut.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.ErrorDetails[0].PropertyPointer.ShouldBe(ExpectedErrorDetail1PropertyPointer);
        sut.ErrorDetails[0].Detail.ShouldBe(ExpectedErrorDetail1Detail);
        sut.ErrorDetails[0].DetailTemplated.ShouldNotBeNull();
        sut.ErrorDetails[0].DetailTemplated.TemplateId.ShouldBe(ExpectedErrorDetail1DetailTemplateId);
        sut.ErrorDetails[0].DetailTemplated.Params.ShouldNotBeNull();
        sut.ErrorDetails[0].DetailTemplated.Params.Count.ShouldBe(ExpectedErrorDetail1DetailParamsCount);
        sut.ErrorDetails[0].DetailTemplated.Params.Keys.ShouldContain(ExpectedErrorDetail1DetailParam1Key);
        sut.ErrorDetails[0].DetailTemplated.Params[ExpectedErrorDetail1DetailParam1Key].ToString().ShouldBe(ExpectedErrorDetail1DetailParam1Value);
        sut.ErrorDetails[0].DetailTemplated.Params.Keys.ShouldContain(ExpectedErrorDetail1DetailParam2Key);
        sut.ErrorDetails[0].DetailTemplated.Params[ExpectedErrorDetail1DetailParam2Key].ToString().ShouldBe(ExpectedErrorDetail1DetailParam2Value);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    #endregion

    #region AddDetail

    [Fact]
    public void AddDetail_DetailTextOnly_AddsDetail()
    {
        // Arrange
        const int ExpectedErrorDetailsCount = 1;
        const string ExpectedErrorDetail1Detail = "Property 1 test detail";

        var sut = new Sut(
            ErrorCategory.Timeout,
            "tag:test.com,2024:Test",
            "Test title");

        // Act
        sut.AddDetail(null, "Property 1 test detail");

        // Assert
        sut.ShouldNotBeNull();
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.ErrorDetails[0].PropertyPointer.ShouldBeNull();
        sut.ErrorDetails[0].Detail.ShouldBe(ExpectedErrorDetail1Detail);
        sut.ErrorDetails[0].DetailTemplated.ShouldBeNull();
    }

    [Fact]
    public void AddDetail_AllDetails_AddsDetail()
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

        var sut = new Sut(
            ErrorCategory.Timeout,
            "tag:test.com,2024:Test",
            "Test title");

        // Act
        sut.AddDetail("#/property1", "Property 1 test detail", "message-property-id",
            ("pk1", "pv1"), ("pk2", "pv2"));

        // Assert
        sut.ShouldNotBeNull();
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.ErrorDetails[0].PropertyPointer.ShouldBe(ExpectedErrorDetail1PropertyPointer);
        sut.ErrorDetails[0].Detail.ShouldBe(ExpectedErrorDetail1Detail);
        sut.ErrorDetails[0].DetailTemplated.ShouldNotBeNull();
        sut.ErrorDetails[0].DetailTemplated.TemplateId.ShouldBe(ExpectedErrorDetail1DetailTemplateId);
        sut.ErrorDetails[0].DetailTemplated.Params.ShouldNotBeNull();
        sut.ErrorDetails[0].DetailTemplated.Params.Count.ShouldBe(ExpectedErrorDetail1DetailParamsCount);
        sut.ErrorDetails[0].DetailTemplated.Params.Keys.ShouldContain(ExpectedErrorDetail1DetailParam1Key);
        sut.ErrorDetails[0].DetailTemplated.Params[ExpectedErrorDetail1DetailParam1Key].ToString().ShouldBe(ExpectedErrorDetail1DetailParam1Value);
        sut.ErrorDetails[0].DetailTemplated.Params.Keys.ShouldContain(ExpectedErrorDetail1DetailParam2Key);
        sut.ErrorDetails[0].DetailTemplated.Params[ExpectedErrorDetail1DetailParam2Key].ToString().ShouldBe(ExpectedErrorDetail1DetailParam2Value);
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
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
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
            new Dictionary<string, object>
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
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
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
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
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
            new Dictionary<string, object>
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
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    #endregion

    #region Forbidden

    [Fact]
    public void Forbidden_OnlyRequiredDetails_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.Forbidden;
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.Forbidden(
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
    public void Forbidden_WithDetailsCollection_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.Forbidden;
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
        var sut = Sut.Forbidden(
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
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void Forbidden_WithDetailsDictionary_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.Forbidden;
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
        var sut = Sut.Forbidden(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            new Dictionary<string, object>
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
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
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
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
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
            new Dictionary<string, object>
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
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
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
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
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
            new Dictionary<string, object>
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
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
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
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
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
            new Dictionary<string, object>
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
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
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
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
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
            new Dictionary<string, object>
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
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    #endregion

    #region CriticalError

    [Fact]
    public void CriticalError_OnlyRequiredDetails_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.CriticalError;
        const int ExpectedErrorDetailsCount = 0;
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        // Act
        var sut = Sut.CriticalError(
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
    public void CriticalError_WithDetailsCollection_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.CriticalError;
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
        var sut = Sut.CriticalError(
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
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void CriticalError_WithDetailsDictionary_ReturnsError()
    {
        // Arrange
        const ErrorCategory ExpectedCategory = ErrorCategory.CriticalError;
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
        var sut = Sut.CriticalError(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            new Dictionary<string, object>
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
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
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
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
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
            new Dictionary<string, object>
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
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
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
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
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
            new Dictionary<string, object>
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
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    #endregion

    #endregion

    #region Deserialize

    [Fact]
    public void Deserialize_AllPropertiesAndMicrosoftSerializer_ReturnsErrorWithAllProperties()
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
        const int ExpectedErrorDetailsCount = 1;
        const string ExpectedErrorDetail1PropertyPointer = "#/property1";
        const string ExpectedErrorDetail1Detail = "Property 1 test detail";
        const string ExpectedErrorDetail1DetailTemplateId = "message-property-id";
        const int ExpectedErrorDetail1DetailParamsCount = 2;
        const string ExpectedErrorDetail1DetailParam1Key = "pk1";
        const string ExpectedErrorDetail1DetailParam1Value = "pv1";
        const string ExpectedErrorDetail1DetailParam2Key = "pk2";
        const string ExpectedErrorDetail1DetailParam2Value = "pv2";
        const string ExpectedInstance = "http://test.com/instance/1013";
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        const string Json = """{"Category":3,"TypeUri":"tag:test.com,2024:Test","Title":"Test title","Detail":"Test description.","DetailTemplated":{"TemplateId":"message-id","Params":{"key1":"value1","key2":"value2"}},"InstanceUri":"http://test.com/instance/1013","ErrorDetails":[{"PropertyPointer":"#/property1","Detail":"Property 1 test detail","DetailTemplated":{"TemplateId":"message-property-id","Params":{"pk1":"pv1","pk2":"pv2"}}}]}""";

        // Act
        var sut = SerializationHelper.DeserializeWithMicrosoft<Sut>(Json);

        // Assert
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBe(ExpectedDetail);
        sut.DetailTemplated.ShouldNotBeNull();
        sut.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        sut.DetailTemplated.Params.ShouldNotBeNull();
        sut.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.ErrorDetails[0].PropertyPointer.ShouldBe(ExpectedErrorDetail1PropertyPointer);
        sut.ErrorDetails[0].Detail.ShouldBe(ExpectedErrorDetail1Detail);
        sut.ErrorDetails[0].DetailTemplated.ShouldNotBeNull();
        sut.ErrorDetails[0].DetailTemplated.TemplateId.ShouldBe(ExpectedErrorDetail1DetailTemplateId);
        sut.ErrorDetails[0].DetailTemplated.Params.ShouldNotBeNull();
        sut.ErrorDetails[0].DetailTemplated.Params.Count.ShouldBe(ExpectedErrorDetail1DetailParamsCount);
        sut.ErrorDetails[0].DetailTemplated.Params.Keys.ShouldContain(ExpectedErrorDetail1DetailParam1Key);
        sut.ErrorDetails[0].DetailTemplated.Params[ExpectedErrorDetail1DetailParam1Key].ToString().ShouldBe(ExpectedErrorDetail1DetailParam1Value);
        sut.ErrorDetails[0].DetailTemplated.Params.Keys.ShouldContain(ExpectedErrorDetail1DetailParam2Key);
        sut.ErrorDetails[0].DetailTemplated.Params[ExpectedErrorDetail1DetailParam2Key].ToString().ShouldBe(ExpectedErrorDetail1DetailParam2Value);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void Deserialize_AllPropertiesAndNewtonsoftSerializer_ReturnsErrorWithAllProperties()
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
        const int ExpectedErrorDetailsCount = 1;
        const string ExpectedErrorDetail1PropertyPointer = "#/property1";
        const string ExpectedErrorDetail1Detail = "Property 1 test detail";
        const string ExpectedErrorDetail1DetailTemplateId = "message-property-id";
        const int ExpectedErrorDetail1DetailParamsCount = 2;
        const string ExpectedErrorDetail1DetailParam1Key = "pk1";
        const string ExpectedErrorDetail1DetailParam1Value = "pv1";
        const string ExpectedErrorDetail1DetailParam2Key = "pk2";
        const string ExpectedErrorDetail1DetailParam2Value = "pv2";
        const string ExpectedInstance = "http://test.com/instance/1013";
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        const string Json = """{"Category":3,"TypeUri":"tag:test.com,2024:Test","Title":"Test title","Detail":"Test description.","DetailTemplated":{"TemplateId":"message-id","Params":{"key1":"value1","key2":"value2"}},"InstanceUri":"http://test.com/instance/1013","ErrorDetails":[{"PropertyPointer":"#/property1","Detail":"Property 1 test detail","DetailTemplated":{"TemplateId":"message-property-id","Params":{"pk1":"pv1","pk2":"pv2"}}}]}""";

        // Act
        var sut = SerializationHelper.DeserializeWithNewtonsoft<Sut>(Json);

        // Assert
        sut.Category.ShouldBe(ExpectedCategory);
        sut.Detail.ShouldBe(ExpectedDetail);
        sut.DetailTemplated.ShouldNotBeNull();
        sut.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        sut.DetailTemplated.Params.ShouldNotBeNull();
        sut.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        sut.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        sut.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        sut.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
        sut.ErrorDetails.ShouldNotBeNull();
        sut.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        sut.ErrorDetails[0].PropertyPointer.ShouldBe(ExpectedErrorDetail1PropertyPointer);
        sut.ErrorDetails[0].Detail.ShouldBe(ExpectedErrorDetail1Detail);
        sut.ErrorDetails[0].DetailTemplated.ShouldNotBeNull();
        sut.ErrorDetails[0].DetailTemplated.TemplateId.ShouldBe(ExpectedErrorDetail1DetailTemplateId);
        sut.ErrorDetails[0].DetailTemplated.Params.ShouldNotBeNull();
        sut.ErrorDetails[0].DetailTemplated.Params.Count.ShouldBe(ExpectedErrorDetail1DetailParamsCount);
        sut.ErrorDetails[0].DetailTemplated.Params.Keys.ShouldContain(ExpectedErrorDetail1DetailParam1Key);
        sut.ErrorDetails[0].DetailTemplated.Params[ExpectedErrorDetail1DetailParam1Key].ToString().ShouldBe(ExpectedErrorDetail1DetailParam1Value);
        sut.ErrorDetails[0].DetailTemplated.Params.Keys.ShouldContain(ExpectedErrorDetail1DetailParam2Key);
        sut.ErrorDetails[0].DetailTemplated.Params[ExpectedErrorDetail1DetailParam2Key].ToString().ShouldBe(ExpectedErrorDetail1DetailParam2Value);
        sut.InstanceUri.ShouldBe(ExpectedInstance);
        sut.Title.ShouldBe(ExpectedTitle);
        sut.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    #endregion

    #region Serialize

    [Fact]
    public void Serialize_AllPropertiesAndMicrosoftSerializer_ReturnsSerializedValueWithAllProperties()
    {
        // Arrange
        const string ExpectedText = """{"Category":3,"TypeUri":"tag:test.com,2024:Test","Title":"Test title","Detail":"Test description.","DetailTemplated":{"TemplateId":"messageId","Params":{"key1":"value1","key2":"value2"}},"InstanceUri":"http://test.com/instance/1013","ErrorDetails":[{"PropertyPointer":"#/property1","Detail":"Property 1 test detail","DetailTemplated":{"TemplateId":"message-property-id","Params":{"pk1":"pv1","pk2":"pv2"}}}]}""";

        var sut = Sut.NotFound(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "messageId",
            ("key1", "value1"), ("key2", "value2"));

        sut.AddDetail("#/property1", "Property 1 test detail", "message-property-id", ("pk1", "pv1"), ("pk2", "pv2"));

        // Act
        var result = SerializationHelper.SerializeWithMicrosoft(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    [Fact]
    public void Serialize_AllPropertiesAndNewtonsoftSerializer_ReturnsSerializedValueWithAllProperties()
    {
        // Arrange
        const string ExpectedText = """{"Category":3,"TypeUri":"tag:test.com,2024:Test","Title":"Test title","Detail":"Test description.","DetailTemplated":{"TemplateId":"messageId","Params":{"key1":"value1","key2":"value2"}},"InstanceUri":"http://test.com/instance/1013","ErrorDetails":[{"PropertyPointer":"#/property1","Detail":"Property 1 test detail","DetailTemplated":{"TemplateId":"message-property-id","Params":{"pk1":"pv1","pk2":"pv2"}}}]}""";

        var sut = Sut.NotFound(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "messageId",
            ("key1", "value1"), ("key2", "value2"));

        sut.AddDetail("#/property1", "Property 1 test detail", "message-property-id", ("pk1", "pv1"), ("pk2", "pv2"));

        // Act
        var result = SerializationHelper.SerializeWithNewtonsoft(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    #endregion
}
