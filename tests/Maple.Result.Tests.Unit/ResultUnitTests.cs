// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Result.Tests.Unit.Helpers;
using System;
using Sut = Maple.Result.Result;

namespace Maple.Result.Tests.Unit;

public class ResultUnitTests
{
    #region Result, ctor

    [Fact]
    public void Ctor_Success_ReturnsSuccessResult()
    {
        // Act
        var result = new Sut();

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
    }

    [Fact]
    public void Ctor_Error_ReturnsErrorResult()
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

        var error = Error.NotFound(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            ("key1", "value1"), ("key2", "value2"));

        error.AddDetail("#/property1", "Property 1 test detail", "message-property-id", ("pk1", "pv1"), ("pk2", "pv2"));

        // Act
        var result = new Sut(error);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldNotBeNull();

        result.Error.Category.ShouldBe(ExpectedCategory);
        result.Error.Detail.ShouldBe(ExpectedDetail);
        result.Error.DetailTemplated.ShouldNotBeNull();
        result.Error.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        result.Error.DetailTemplated.Params.ShouldNotBeNull();
        result.Error.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        result.Error.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        result.Error.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        result.Error.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        result.Error.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
        result.Error.ErrorDetails.ShouldNotBeNull();
        result.Error.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        result.Error.ErrorDetails[0].PropertyPointer.ShouldBe(ExpectedErrorDetail1PropertyPointer);
        result.Error.ErrorDetails[0].Detail.ShouldBe(ExpectedErrorDetail1Detail);
        result.Error.ErrorDetails[0].DetailTemplated.ShouldNotBeNull();
        result.Error.ErrorDetails[0].DetailTemplated!.TemplateId.ShouldBe(ExpectedErrorDetail1DetailTemplateId);
        result.Error.ErrorDetails[0].DetailTemplated!.Params.ShouldNotBeNull();
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Count.ShouldBe(ExpectedErrorDetail1DetailParamsCount);
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Keys.ShouldContain(ExpectedErrorDetail1DetailParam1Key);
        result.Error.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam1Key].ToString().ShouldBe(ExpectedErrorDetail1DetailParam1Value);
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Keys.ShouldContain(ExpectedErrorDetail1DetailParam2Key);
        result.Error.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam2Key].ToString().ShouldBe(ExpectedErrorDetail1DetailParam2Value);
        result.Error.InstanceUri.ShouldBe(ExpectedInstance);
        result.Error.Title.ShouldBe(ExpectedTitle);
        result.Error.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    #endregion

    #region Result, implicit operator

    [Fact]
    public void ImplicitOperator_Error_ReturnsErrorResult()
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
        
        var error = Error.NotFound(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            ("key1", "value1"), ("key2", "value2"));

        error.AddDetail("#/property1", "Property 1 test detail", "message-property-id", ("pk1", "pv1"), ("pk2", "pv2"));

        // Act
        Sut result = error;

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldNotBeNull();

        result.Error.Category.ShouldBe(ExpectedCategory);
        result.Error.Detail.ShouldBe(ExpectedDetail);
        result.Error.DetailTemplated.ShouldNotBeNull();
        result.Error.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        result.Error.DetailTemplated.Params.ShouldNotBeNull();
        result.Error.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        result.Error.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        result.Error.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        result.Error.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        result.Error.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
        result.Error.ErrorDetails.ShouldNotBeNull();
        result.Error.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        result.Error.ErrorDetails[0].PropertyPointer.ShouldBe(ExpectedErrorDetail1PropertyPointer);
        result.Error.ErrorDetails[0].Detail.ShouldBe(ExpectedErrorDetail1Detail);
        result.Error.ErrorDetails[0].DetailTemplated.ShouldNotBeNull();
        result.Error.ErrorDetails[0].DetailTemplated!.TemplateId.ShouldBe(ExpectedErrorDetail1DetailTemplateId);
        result.Error.ErrorDetails[0].DetailTemplated!.Params.ShouldNotBeNull();
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Count.ShouldBe(ExpectedErrorDetail1DetailParamsCount);
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Keys.ShouldContain(ExpectedErrorDetail1DetailParam1Key);
        result.Error.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam1Key].ToString().ShouldBe(ExpectedErrorDetail1DetailParam1Value);
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Keys.ShouldContain(ExpectedErrorDetail1DetailParam2Key);
        result.Error.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam2Key].ToString().ShouldBe(ExpectedErrorDetail1DetailParam2Value);
        result.Error.InstanceUri.ShouldBe(ExpectedInstance);
        result.Error.Title.ShouldBe(ExpectedTitle);
        result.Error.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    #endregion

    #region Result<T>, FromValue()

    [Fact]
    public void FromValue_ResultWithValidValue_ReturnsSuccessfulResult()
    {
        // Arrange
        const string ExpectedText = "Test text";
        const int ExpectedNumber = 43;

        var value = new TestClass
        {
            Number = 43,
            Text = "Test text"
        };

        // Act
        var result = Sut.FromValue(value);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Text.ShouldBe(ExpectedText);
        result.Value.Number.ShouldBe(ExpectedNumber);
    }

    #endregion

    #region Result, FromError()

    [Fact]
    public void FromError_Error_ReturnsErrorResult()
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
        
        var error = Error.NotFound(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            ("key1", "value1"), ("key2", "value2"));

        error.AddDetail("#/property1", "Property 1 test detail", "message-property-id", ("pk1", "pv1"), ("pk2", "pv2"));

        // Act
        var result = Sut.FromError(error);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldNotBeNull();

        result.Error.Category.ShouldBe(ExpectedCategory);
        result.Error.Detail.ShouldBe(ExpectedDetail);
        result.Error.DetailTemplated.ShouldNotBeNull();
        result.Error.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        result.Error.DetailTemplated.Params.ShouldNotBeNull();
        result.Error.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        result.Error.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        result.Error.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        result.Error.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        result.Error.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
        result.Error.ErrorDetails.ShouldNotBeNull();
        result.Error.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        result.Error.ErrorDetails[0].PropertyPointer.ShouldBe(ExpectedErrorDetail1PropertyPointer);
        result.Error.ErrorDetails[0].Detail.ShouldBe(ExpectedErrorDetail1Detail);
        result.Error.ErrorDetails[0].DetailTemplated.ShouldNotBeNull();
        result.Error.ErrorDetails[0].DetailTemplated!.TemplateId.ShouldBe(ExpectedErrorDetail1DetailTemplateId);
        result.Error.ErrorDetails[0].DetailTemplated!.Params.ShouldNotBeNull();
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Count.ShouldBe(ExpectedErrorDetail1DetailParamsCount);
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Keys.ShouldContain(ExpectedErrorDetail1DetailParam1Key);
        result.Error.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam1Key].ToString().ShouldBe(ExpectedErrorDetail1DetailParam1Value);
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Keys.ShouldContain(ExpectedErrorDetail1DetailParam2Key);
        result.Error.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam2Key].ToString().ShouldBe(ExpectedErrorDetail1DetailParam2Value);
        result.Error.InstanceUri.ShouldBe(ExpectedInstance);
        result.Error.Title.ShouldBe(ExpectedTitle);
        result.Error.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    #endregion

    #region Result, Success

    [Fact]
    public void Success_Always_ReturnsSuccessResult()
    {
        // Act
        var result = Sut.Success();

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Error.ShouldBeNull();
    }

    #endregion

    #region Result, Deserialize

    [Fact]
    public void Deserialize_SuccessAndMicrosoftSerializer_ReturnsValueWithAllProperties()
    {
        // Arrange
        const string Json = "{}";

        // Act
        var sut = SerializationHelper.DeserializeWithMicrosoft<Sut>(Json);

        // Assert
        sut.ShouldNotBeNull();
        sut.IsSuccess().ShouldBeTrue();
    }

    [Fact]
    public void Deserialize_SuccessAndNewtonsoftSerializer_ReturnsValueWithAllProperties()
    {
        // Arrange
        const string Json = "{}";

        // Act
        var sut = SerializationHelper.DeserializeWithNewtonsoft<Sut>(Json);

        // Assert
        sut.ShouldNotBeNull();
        sut.IsSuccess().ShouldBeTrue();
    }

    [Fact]
    public void Deserialize_ErrorAndMicrosoftSerializer_ReturnsErrorWithAllProperties()
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

        const string Json = """{"Error":{"Category":3,"TypeUri":"tag:test.com,2024:Test","Title":"Test title","Detail":"Test description.","DetailTemplated":{"TemplateId":"message-id","Params":{"key1":"value1","key2":"value2"}},"InstanceUri":"http://test.com/instance/1013","ErrorDetails":[{"PropertyPointer":"#/property1","Detail":"Property 1 test detail","DetailTemplated":{"TemplateId":"message-property-id","Params":{"pk1":"pv1","pk2":"pv2"}}}]}}""";

        // Act
        var result = SerializationHelper.DeserializeWithMicrosoft<Sut>(Json);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldNotBeNull();
        result.Error.Category.ShouldBe(ExpectedCategory);
        result.Error.Detail.ShouldBe(ExpectedDetail);
        result.Error.DetailTemplated.ShouldNotBeNull();
        result.Error.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        result.Error.DetailTemplated.Params.ShouldNotBeNull();
        result.Error.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        result.Error.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        result.Error.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        result.Error.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        result.Error.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
        result.Error.ErrorDetails.ShouldNotBeNull();
        result.Error.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        result.Error.ErrorDetails[0].PropertyPointer.ShouldBe(ExpectedErrorDetail1PropertyPointer);
        result.Error.ErrorDetails[0].Detail.ShouldBe(ExpectedErrorDetail1Detail);
        result.Error.ErrorDetails[0].DetailTemplated.ShouldNotBeNull();
        result.Error.ErrorDetails[0].DetailTemplated!.TemplateId.ShouldBe(ExpectedErrorDetail1DetailTemplateId);
        result.Error.ErrorDetails[0].DetailTemplated!.Params.ShouldNotBeNull();
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Count.ShouldBe(ExpectedErrorDetail1DetailParamsCount);
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Keys.ShouldContain(ExpectedErrorDetail1DetailParam1Key);
        result.Error.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam1Key].ToString().ShouldBe(ExpectedErrorDetail1DetailParam1Value);
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Keys.ShouldContain(ExpectedErrorDetail1DetailParam2Key);
        result.Error.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam2Key].ToString().ShouldBe(ExpectedErrorDetail1DetailParam2Value);
        result.Error.InstanceUri.ShouldBe(ExpectedInstance);
        result.Error.Title.ShouldBe(ExpectedTitle);
        result.Error.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void Deserialize_ErrorAndNewtonsoftSerializer_ReturnsErrorWithAllProperties()
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

        const string Json = """{"Error":{"Category":3,"TypeUri":"tag:test.com,2024:Test","Title":"Test title","Detail":"Test description.","DetailTemplated":{"TemplateId":"message-id","Params":{"key1":"value1","key2":"value2"}},"InstanceUri":"http://test.com/instance/1013","ErrorDetails":[{"PropertyPointer":"#/property1","Detail":"Property 1 test detail","DetailTemplated":{"TemplateId":"message-property-id","Params":{"pk1":"pv1","pk2":"pv2"}}}]}}""";

        // Act
        var result = SerializationHelper.DeserializeWithNewtonsoft<Sut>(Json);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldNotBeNull();
        result.Error.Category.ShouldBe(ExpectedCategory);
        result.Error.Detail.ShouldBe(ExpectedDetail);
        result.Error.DetailTemplated.ShouldNotBeNull();
        result.Error.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        result.Error.DetailTemplated.Params.ShouldNotBeNull();
        result.Error.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        result.Error.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        result.Error.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        result.Error.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        result.Error.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
        result.Error.ErrorDetails.ShouldNotBeNull();
        result.Error.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        result.Error.ErrorDetails[0].PropertyPointer.ShouldBe(ExpectedErrorDetail1PropertyPointer);
        result.Error.ErrorDetails[0].Detail.ShouldBe(ExpectedErrorDetail1Detail);
        result.Error.ErrorDetails[0].DetailTemplated.ShouldNotBeNull();
        result.Error.ErrorDetails[0].DetailTemplated!.TemplateId.ShouldBe(ExpectedErrorDetail1DetailTemplateId);
        result.Error.ErrorDetails[0].DetailTemplated!.Params.ShouldNotBeNull();
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Count.ShouldBe(ExpectedErrorDetail1DetailParamsCount);
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Keys.ShouldContain(ExpectedErrorDetail1DetailParam1Key);
        result.Error.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam1Key].ToString().ShouldBe(ExpectedErrorDetail1DetailParam1Value);
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Keys.ShouldContain(ExpectedErrorDetail1DetailParam2Key);
        result.Error.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam2Key].ToString().ShouldBe(ExpectedErrorDetail1DetailParam2Value);
        result.Error.InstanceUri.ShouldBe(ExpectedInstance);
        result.Error.Title.ShouldBe(ExpectedTitle);
        result.Error.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    #endregion

    #region Result, Serialize

    [Fact]
    public void Serialize_AllPropertiesAndMicrosoftSerializer_ReturnsSerializedValueWithAllProperties()
    {
        // Arrange
        const string ExpectedText = """{"Error":{"Category":3,"TypeUri":"tag:test.com,2024:Test","Title":"Test title","Detail":"Test description.","DetailTemplated":{"TemplateId":"messageId","Params":{"key1":"value1","key2":"value2"}},"InstanceUri":"http://test.com/instance/1013","ErrorDetails":[{"PropertyPointer":"#/property1","Detail":"Property 1 test detail","DetailTemplated":{"TemplateId":"message-property-id","Params":{"pk1":"pv1","pk2":"pv2"}}}]}}""";

        var error = Error.NotFound(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "messageId",
            ("key1", "value1"), ("key2", "value2"));

        error.AddDetail("#/property1", "Property 1 test detail", "message-property-id", ("pk1", "pv1"), ("pk2", "pv2"));

        var sut = new Sut(error);

        // Act
        var result = SerializationHelper.SerializeWithMicrosoft(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    [Fact]
    public void Serialize_AllPropertiesAndNewtonsoftSerializer_ReturnsSerializedValueWithAllProperties()
    {
        // Arrange
        const string ExpectedText = """{"Error":{"Category":3,"TypeUri":"tag:test.com,2024:Test","Title":"Test title","Detail":"Test description.","DetailTemplated":{"TemplateId":"messageId","Params":{"key1":"value1","key2":"value2"}},"InstanceUri":"http://test.com/instance/1013","ErrorDetails":[{"PropertyPointer":"#/property1","Detail":"Property 1 test detail","DetailTemplated":{"TemplateId":"message-property-id","Params":{"pk1":"pv1","pk2":"pv2"}}}]}}""";

        var error = Error.NotFound(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "messageId",
            ("key1", "value1"), ("key2", "value2"));

        error.AddDetail("#/property1", "Property 1 test detail", "message-property-id", ("pk1", "pv1"), ("pk2", "pv2"));

        var sut = new Sut(error);

        // Act
        var result = SerializationHelper.SerializeWithNewtonsoft(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    #endregion

    #region Result<T>, ctor

    [Fact]
    public void IsSuccess_ParameterlessCtorAndReferenceTypeValue_ThrowsException()
    {
        // Arrange
        var sut = new Result<TestClass>();

        // Act
        var exception = Record.Exception(() => sut.IsSuccess());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidOperationException>();
        exception.Message.ShouldContain("has neither a value nor an error");
    }

    [Fact]
    public void IsSuccess_ParameterlessCtorAndValueTypeValue_ThrowsException()
    {
        // Arrange
        var sut = new Result<int>();

        // Act
        var exception = Record.Exception(() => sut.IsSuccess());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidOperationException>();
        exception.Message.ShouldContain("has neither a value nor an error");
    }

    [Fact]
    public void IsSuccess_ValueTypeValueEqualToDefault_ReturnsTrue()
    {
        // Arrange
        var sut = Result.FromValue(0);

        // Act
        var result = sut.IsSuccess();

        // Assert
        result.ShouldBeTrue();
        sut.Value.ShouldBe(0);
    }

    [Fact]
    public void IsSuccess_ParameterlessCtorWithValueAssigned_ReturnsTrue()
    {
        // Arrange
        var sut = new Result<int> { Value = 0 };

        // Act
        var result = sut.IsSuccess();

        // Assert
        result.ShouldBeTrue();
        sut.Value.ShouldBe(0);
    }

    [Fact]
    public void IsSuccess_ParameterlessCtorWithErrorAssigned_ReturnsFalse()
    {
        // Arrange
        var error = Error.NotFound(ErrorUri.Tag("tag:test.com,2024:Test"), "Test title");
        var sut = new Result<TestClass> { Error = error };

        // Act
        var result = sut.IsSuccess();

        // Assert
        result.ShouldBeFalse();
        sut.Error.ShouldBe(error);
    }

    [Fact]
    public void Ctor_SuccessWithValue_ReturnsSuccessResultWithValue()
    {
        // Arrange
        const string ExpectedText = "Test text";
        const int ExpectedNumber = 43;

        var value = new TestClass
        {
            Number = 43,
            Text = "Test text"
        };

        // Act
        var result = new Result<TestClass>(value);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Text.ShouldBe(ExpectedText);
        result.Value.Number.ShouldBe(ExpectedNumber);
    }

    [Fact]
    public void Ctor_SuccessWithValueAndError_ThrowsException()
    {
        // Arrange
        var value = new TestClass
        {
            Number = 43,
            Text = "Test text"
        };

        var error = Error.NotFound(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            ("key1", "value1"), ("key2", "value2"));

        error.AddDetail("#/property1", "Property 1 test detail", "message-property-id", ("pk1", "pv1"), ("pk2", "pv2"));

        // Act
        var exception = Record.Exception(() => new Result<TestClass>(value) { Error = error });

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidOperationException>();
        exception.Message.ShouldBe("Cannot set both Value and Error of the Result!");
    }

    [Fact]
    public void Ctor_ErrorWithoutValue_ReturnsErrorResult()
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

        var error = Error.NotFound(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            ("key1", "value1"), ("key2", "value2"));

        error.AddDetail("#/property1", "Property 1 test detail", "message-property-id", ("pk1", "pv1"), ("pk2", "pv2"));

        // Act
        var result = new Result<TestClass>(error);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldNotBeNull();

        result.Error.Category.ShouldBe(ExpectedCategory);
        result.Error.Detail.ShouldBe(ExpectedDetail);
        result.Error.DetailTemplated.ShouldNotBeNull();
        result.Error.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        result.Error.DetailTemplated.Params.ShouldNotBeNull();
        result.Error.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        result.Error.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        result.Error.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        result.Error.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        result.Error.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
        result.Error.ErrorDetails.ShouldNotBeNull();
        result.Error.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        result.Error.ErrorDetails[0].PropertyPointer.ShouldBe(ExpectedErrorDetail1PropertyPointer);
        result.Error.ErrorDetails[0].Detail.ShouldBe(ExpectedErrorDetail1Detail);
        result.Error.ErrorDetails[0].DetailTemplated.ShouldNotBeNull();
        result.Error.ErrorDetails[0].DetailTemplated!.TemplateId.ShouldBe(ExpectedErrorDetail1DetailTemplateId);
        result.Error.ErrorDetails[0].DetailTemplated!.Params.ShouldNotBeNull();
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Count.ShouldBe(ExpectedErrorDetail1DetailParamsCount);
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Keys.ShouldContain(ExpectedErrorDetail1DetailParam1Key);
        result.Error.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam1Key].ToString().ShouldBe(ExpectedErrorDetail1DetailParam1Value);
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Keys.ShouldContain(ExpectedErrorDetail1DetailParam2Key);
        result.Error.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam2Key].ToString().ShouldBe(ExpectedErrorDetail1DetailParam2Value);
        result.Error.InstanceUri.ShouldBe(ExpectedInstance);
        result.Error.Title.ShouldBe(ExpectedTitle);
        result.Error.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void Ctor_ErrorWithValue_ThrowsException()
    {
        // Arrange
        var value = new TestClass
        {
            Number = 43,
            Text = "Test text"
        };

        var error = Error.NotFound(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            ("key1", "value1"), ("key2", "value2"));

        error.AddDetail("#/property1", "Property 1 test detail", "message-property-id", ("pk1", "pv1"), ("pk2", "pv2"));

        // Act
        var exception = Record.Exception(() => new Result<TestClass>(error) { Value = value });

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidOperationException>();
        exception.Message.ShouldBe("Cannot set both Value and Error of the Result!");
    }

    #endregion

    #region Result<T>, implicit operator

    [Fact]
    public void ImplicitOperator_Value_ReturnsSuccessResultWithValue()
    {
        // Arrange
        const string ExpectedText = "Test text";
        const int ExpectedNumber = 43;

        var value = new TestClass
        {
            Number = 43,
            Text = "Test text"
        };

        // Act
        Result<TestClass> result = value;

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Text.ShouldBe(ExpectedText);
        result.Value.Number.ShouldBe(ExpectedNumber);
    }

    [Fact]
    public void ImplicitOperator_Error_ReturnsErrorResultWithError()
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

        var error = Error.NotFound(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            ("key1", "value1"), ("key2", "value2"));

        error.AddDetail("#/property1", "Property 1 test detail", "message-property-id", ("pk1", "pv1"), ("pk2", "pv2"));

        // Act
        Result<TestClass> result = error;

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldNotBeNull();

        result.Error.Category.ShouldBe(ExpectedCategory);
        result.Error.Detail.ShouldBe(ExpectedDetail);
        result.Error.DetailTemplated.ShouldNotBeNull();
        result.Error.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        result.Error.DetailTemplated.Params.ShouldNotBeNull();
        result.Error.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        result.Error.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        result.Error.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        result.Error.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        result.Error.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
        result.Error.ErrorDetails.ShouldNotBeNull();
        result.Error.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        result.Error.ErrorDetails[0].PropertyPointer.ShouldBe(ExpectedErrorDetail1PropertyPointer);
        result.Error.ErrorDetails[0].Detail.ShouldBe(ExpectedErrorDetail1Detail);
        result.Error.ErrorDetails[0].DetailTemplated.ShouldNotBeNull();
        result.Error.ErrorDetails[0].DetailTemplated!.TemplateId.ShouldBe(ExpectedErrorDetail1DetailTemplateId);
        result.Error.ErrorDetails[0].DetailTemplated!.Params.ShouldNotBeNull();
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Count.ShouldBe(ExpectedErrorDetail1DetailParamsCount);
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Keys.ShouldContain(ExpectedErrorDetail1DetailParam1Key);
        result.Error.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam1Key].ToString().ShouldBe(ExpectedErrorDetail1DetailParam1Value);
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Keys.ShouldContain(ExpectedErrorDetail1DetailParam2Key);
        result.Error.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam2Key].ToString().ShouldBe(ExpectedErrorDetail1DetailParam2Value);
        result.Error.InstanceUri.ShouldBe(ExpectedInstance);
        result.Error.Title.ShouldBe(ExpectedTitle);
        result.Error.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    #endregion

    #region Result<T>, FromValue()

    [Fact]
    public void FromValue_Always_ReturnsSuccessfulResult()
    {
        // Arrange
        const string ExpectedText = "Test text";
        const int ExpectedNumber = 43;

        var value = new TestClass
        {
            Number = 43,
            Text = "Test text"
        };

        // Act
        var result = Result<TestClass>.FromValue(value);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Text.ShouldBe(ExpectedText);
        result.Value.Number.ShouldBe(ExpectedNumber);
    }

    #endregion

    #region Result, FromError()

    [Fact]
    public void FromError_ValueWithError_ReturnsErrorResult()
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

        var error = Error.NotFound(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "message-id",
            ("key1", "value1"), ("key2", "value2"));

        error.AddDetail("#/property1", "Property 1 test detail", "message-property-id", ("pk1", "pv1"), ("pk2", "pv2"));

        // Act
        var result = Result<TestClass>.FromError(error);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldNotBeNull();

        result.Error.Category.ShouldBe(ExpectedCategory);
        result.Error.Detail.ShouldBe(ExpectedDetail);
        result.Error.DetailTemplated.ShouldNotBeNull();
        result.Error.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        result.Error.DetailTemplated.Params.ShouldNotBeNull();
        result.Error.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        result.Error.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        result.Error.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        result.Error.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        result.Error.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
        result.Error.ErrorDetails.ShouldNotBeNull();
        result.Error.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        result.Error.ErrorDetails[0].PropertyPointer.ShouldBe(ExpectedErrorDetail1PropertyPointer);
        result.Error.ErrorDetails[0].Detail.ShouldBe(ExpectedErrorDetail1Detail);
        result.Error.ErrorDetails[0].DetailTemplated.ShouldNotBeNull();
        result.Error.ErrorDetails[0].DetailTemplated!.TemplateId.ShouldBe(ExpectedErrorDetail1DetailTemplateId);
        result.Error.ErrorDetails[0].DetailTemplated!.Params.ShouldNotBeNull();
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Count.ShouldBe(ExpectedErrorDetail1DetailParamsCount);
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Keys.ShouldContain(ExpectedErrorDetail1DetailParam1Key);
        result.Error.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam1Key].ToString().ShouldBe(ExpectedErrorDetail1DetailParam1Value);
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Keys.ShouldContain(ExpectedErrorDetail1DetailParam2Key);
        result.Error.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam2Key].ToString().ShouldBe(ExpectedErrorDetail1DetailParam2Value);
        result.Error.InstanceUri.ShouldBe(ExpectedInstance);
        result.Error.Title.ShouldBe(ExpectedTitle);
        result.Error.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    #endregion

    #region Result<T>, Deserialize

    [Fact]
    public void Deserialize_SuccessWithValueAndMicrosoftSerializer_ReturnsValueWithAllProperties()
    {
        // Arrange
        const string ExpectedText = "Test text";
        const int ExpectedNumber = 38;

        const string Json = """{"Value":{"Text":"Test text","Number":38}}""";

        // Act
        var result = SerializationHelper.DeserializeWithMicrosoft<Result<TestClass>>(Json);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Text.ShouldBe(ExpectedText);
        result.Value.Number.ShouldBe(ExpectedNumber);
    }

    [Fact]
    public void Deserialize_SuccessWithValueAndNewtonsoftSerializer_ReturnsValueWithAllProperties()
    {
        // Arrange
        const string ExpectedText = "Test text";
        const int ExpectedNumber = 38;

        const string Json = """{"Value":{"Text":"Test text","Number":38}}""";

        // Act
        var result = SerializationHelper.DeserializeWithNewtonsoft<Result<TestClass>>(Json);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Text.ShouldBe(ExpectedText);
        result.Value.Number.ShouldBe(ExpectedNumber);
    }

    [Fact]
    public void Deserialize_ErrorWithValueAndMicrosoftSerializer_ReturnsErrorWithAllProperties()
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

        const string Json = """{"Error":{"Category":3,"TypeUri":"tag:test.com,2024:Test","Title":"Test title","Detail":"Test description.","DetailTemplated":{"TemplateId":"message-id","Params":{"key1":"value1","key2":"value2"}},"InstanceUri":"http://test.com/instance/1013","ErrorDetails":[{"PropertyPointer":"#/property1","Detail":"Property 1 test detail","DetailTemplated":{"TemplateId":"message-property-id","Params":{"pk1":"pv1","pk2":"pv2"}}}]}}""";

        // Act
        var result = SerializationHelper.DeserializeWithMicrosoft<Result<TestClass>>(Json);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldNotBeNull();
        result.Error.Category.ShouldBe(ExpectedCategory);
        result.Error.Detail.ShouldBe(ExpectedDetail);
        result.Error.DetailTemplated.ShouldNotBeNull();
        result.Error.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        result.Error.DetailTemplated.Params.ShouldNotBeNull();
        result.Error.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        result.Error.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        result.Error.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        result.Error.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        result.Error.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
        result.Error.ErrorDetails.ShouldNotBeNull();
        result.Error.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        result.Error.ErrorDetails[0].PropertyPointer.ShouldBe(ExpectedErrorDetail1PropertyPointer);
        result.Error.ErrorDetails[0].Detail.ShouldBe(ExpectedErrorDetail1Detail);
        result.Error.ErrorDetails[0].DetailTemplated.ShouldNotBeNull();
        result.Error.ErrorDetails[0].DetailTemplated!.TemplateId.ShouldBe(ExpectedErrorDetail1DetailTemplateId);
        result.Error.ErrorDetails[0].DetailTemplated!.Params.ShouldNotBeNull();
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Count.ShouldBe(ExpectedErrorDetail1DetailParamsCount);
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Keys.ShouldContain(ExpectedErrorDetail1DetailParam1Key);
        result.Error.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam1Key].ToString().ShouldBe(ExpectedErrorDetail1DetailParam1Value);
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Keys.ShouldContain(ExpectedErrorDetail1DetailParam2Key);
        result.Error.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam2Key].ToString().ShouldBe(ExpectedErrorDetail1DetailParam2Value);
        result.Error.InstanceUri.ShouldBe(ExpectedInstance);
        result.Error.Title.ShouldBe(ExpectedTitle);
        result.Error.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void Deserialize_ErrorWithValueAndNewtonsoftSerializer_ReturnsErrorWithAllProperties()
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

        const string Json = """{"Error":{"Category":3,"TypeUri":"tag:test.com,2024:Test","Title":"Test title","Detail":"Test description.","DetailTemplated":{"TemplateId":"message-id","Params":{"key1":"value1","key2":"value2"}},"InstanceUri":"http://test.com/instance/1013","ErrorDetails":[{"PropertyPointer":"#/property1","Detail":"Property 1 test detail","DetailTemplated":{"TemplateId":"message-property-id","Params":{"pk1":"pv1","pk2":"pv2"}}}]}}""";

        // Act
        var result = SerializationHelper.DeserializeWithNewtonsoft<Result<TestClass>>(Json);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldNotBeNull();
        result.Error.Category.ShouldBe(ExpectedCategory);
        result.Error.Detail.ShouldBe(ExpectedDetail);
        result.Error.DetailTemplated.ShouldNotBeNull();
        result.Error.DetailTemplated.TemplateId.ShouldBe(ExpectedDetailTemplateId);
        result.Error.DetailTemplated.Params.ShouldNotBeNull();
        result.Error.DetailTemplated.Params.Count.ShouldBe(ExpectedDetailParamsCount);
        result.Error.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam1Key);
        result.Error.DetailTemplated.Params[ExpectedDetailParam1Key].ToString().ShouldBe(ExpectedDetailParam1Value);
        result.Error.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        result.Error.DetailTemplated.Params[ExpectedDetailParam2Key].ToString().ShouldBe(ExpectedDetailParam2Value);
        result.Error.ErrorDetails.ShouldNotBeNull();
        result.Error.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        result.Error.ErrorDetails[0].PropertyPointer.ShouldBe(ExpectedErrorDetail1PropertyPointer);
        result.Error.ErrorDetails[0].Detail.ShouldBe(ExpectedErrorDetail1Detail);
        result.Error.ErrorDetails[0].DetailTemplated.ShouldNotBeNull();
        result.Error.ErrorDetails[0].DetailTemplated!.TemplateId.ShouldBe(ExpectedErrorDetail1DetailTemplateId);
        result.Error.ErrorDetails[0].DetailTemplated!.Params.ShouldNotBeNull();
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Count.ShouldBe(ExpectedErrorDetail1DetailParamsCount);
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Keys.ShouldContain(ExpectedErrorDetail1DetailParam1Key);
        result.Error.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam1Key].ToString().ShouldBe(ExpectedErrorDetail1DetailParam1Value);
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Keys.ShouldContain(ExpectedErrorDetail1DetailParam2Key);
        result.Error.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam2Key].ToString().ShouldBe(ExpectedErrorDetail1DetailParam2Value);
        result.Error.InstanceUri.ShouldBe(ExpectedInstance);
        result.Error.Title.ShouldBe(ExpectedTitle);
        result.Error.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    #endregion

    #region Result<T>, Serialize

    [Fact]
    public void Serialize_SuccessWithReferenceTypeValueAndMicrosoftSerializer_ReturnsSerializedValueWithAllProperties()
    {
        // Arrange
        const string ExpectedText = """{"Value":{"Text":"Test text","Number":137},"Error":null}""";

        var value = new TestClass
        {
            Number = 137,
            Text = "Test text"
        };

        var sut = Sut.FromValue(value);

        // Act
        var result = SerializationHelper.SerializeWithMicrosoft(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    [Fact]
    public void Serialize_SuccessWithReferenceTypeValueAndNewtonsoftSerializer_ReturnsSerializedValueWithAllProperties()
    {
        // Arrange
        const string ExpectedText = """{"Value":{"Text":"Test text","Number":137},"Error":null}""";

        var value = new TestClass
        {
            Number = 137,
            Text = "Test text"
        };

        var sut = Sut.FromValue(value);

        // Act
        var result = SerializationHelper.SerializeWithNewtonsoft(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    [Fact]
    public void Serialize_SuccessWithValueTypeValueAndMicrosoftSerializer_ReturnsSerializedValueWithAllProperties()
    {
        // Arrange
        const string ExpectedText = """{"Value":128,"Error":null}""";

        const int Value = 128;

        var sut = Sut.FromValue(Value);

        // Act
        var result = SerializationHelper.SerializeWithMicrosoft(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    [Fact]
    public void Serialize_SuccessWithValueTypeValueAndNewtonsoftSerializer_ReturnsSerializedValueWithAllProperties()
    {
        // Arrange
        const string ExpectedText = """{"Value":128,"Error":null}""";

        const int Value = 128;

        var sut = Sut.FromValue(Value);

        // Act
        var result = SerializationHelper.SerializeWithNewtonsoft(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    [Fact]
    public void Serialize_ReferenceTypeAndErrorAllPropertiesAndMicrosoftSerializer_ReturnsSerializedValueWithAllProperties()
    {
        // Arrange
        const string ExpectedText = """{"Value":null,"Error":{"Category":3,"TypeUri":"tag:test.com,2024:Test","Title":"Test title","Detail":"Test description.","DetailTemplated":{"TemplateId":"messageId","Params":{"key1":"value1","key2":"value2"}},"InstanceUri":"http://test.com/instance/1013","ErrorDetails":[{"PropertyPointer":"#/property1","Detail":"Property 1 test detail","DetailTemplated":{"TemplateId":"message-property-id","Params":{"pk1":"pv1","pk2":"pv2"}}}]}}""";

        var error = Error.NotFound(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "messageId",
            ("key1", "value1"), ("key2", "value2"));

        error.AddDetail("#/property1", "Property 1 test detail", "message-property-id", ("pk1", "pv1"), ("pk2", "pv2"));

        var sut = new Result<TestClass>(error);

        // Act
        var result = SerializationHelper.SerializeWithMicrosoft(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    [Fact]
    public void Serialize_ReferenceTypeAndErrorAllPropertiesAndNewtonsoftSerializer_ReturnsSerializedValueWithAllProperties()
    {
        // Arrange
        const string ExpectedText = """{"Value":null,"Error":{"Category":3,"TypeUri":"tag:test.com,2024:Test","Title":"Test title","Detail":"Test description.","DetailTemplated":{"TemplateId":"messageId","Params":{"key1":"value1","key2":"value2"}},"InstanceUri":"http://test.com/instance/1013","ErrorDetails":[{"PropertyPointer":"#/property1","Detail":"Property 1 test detail","DetailTemplated":{"TemplateId":"message-property-id","Params":{"pk1":"pv1","pk2":"pv2"}}}]}}""";

        var error = Error.NotFound(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "messageId",
            ("key1", "value1"), ("key2", "value2"));

        error.AddDetail("#/property1", "Property 1 test detail", "message-property-id", ("pk1", "pv1"), ("pk2", "pv2"));

        var sut = new Result<TestClass>(error);

        // Act
        var result = SerializationHelper.SerializeWithNewtonsoft(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    [Fact]
    public void Serialize_ValueTypeAndErrorAllPropertiesAndMicrosoftSerializer_ReturnsSerializedValueWithAllProperties()
    {
        // Arrange
        const string ExpectedText = """{"Error":{"Category":3,"TypeUri":"tag:test.com,2024:Test","Title":"Test title","Detail":"Test description.","DetailTemplated":{"TemplateId":"messageId","Params":{"key1":"value1","key2":"value2"}},"InstanceUri":"http://test.com/instance/1013","ErrorDetails":[{"PropertyPointer":"#/property1","Detail":"Property 1 test detail","DetailTemplated":{"TemplateId":"message-property-id","Params":{"pk1":"pv1","pk2":"pv2"}}}]}}""";

        var error = Error.NotFound(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "messageId",
            ("key1", "value1"), ("key2", "value2"));

        error.AddDetail("#/property1", "Property 1 test detail", "message-property-id", ("pk1", "pv1"), ("pk2", "pv2"));

        var sut = new Result<int>(error);

        // Act
        var result = SerializationHelper.SerializeWithMicrosoft(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    [Fact]
    public void Serialize_ValueTypeAndErrorAllPropertiesAndNewtonsoftSerializer_ReturnsSerializedValueWithAllProperties()
    {
        // Arrange
        const string ExpectedText = """{"Error":{"Category":3,"TypeUri":"tag:test.com,2024:Test","Title":"Test title","Detail":"Test description.","DetailTemplated":{"TemplateId":"messageId","Params":{"key1":"value1","key2":"value2"}},"InstanceUri":"http://test.com/instance/1013","ErrorDetails":[{"PropertyPointer":"#/property1","Detail":"Property 1 test detail","DetailTemplated":{"TemplateId":"message-property-id","Params":{"pk1":"pv1","pk2":"pv2"}}}]}}""";

        var error = Error.NotFound(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "messageId",
            ("key1", "value1"), ("key2", "value2"));

        error.AddDetail("#/property1", "Property 1 test detail", "message-property-id", ("pk1", "pv1"), ("pk2", "pv2"));

        var sut = new Result<int>(error);

        // Act
        var result = SerializationHelper.SerializeWithNewtonsoft(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    #endregion

    #region Result<T>, value type initialization

    [Fact]
    public void Ctor_ValueTypeErrorWithoutValue_ReturnsErrorResult()
    {
        // Arrange
        var error = Error.NotFound(ErrorUri.Tag("tag:test.com,2024:Test"), "Test title");

        // Act
        var result = new Result<int> { Error = error };

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(error);
    }

    [Fact]
    public void Ctor_StructValueTypeErrorWithoutValue_ReturnsErrorResult()
    {
        // Arrange
        var error = Error.NotFound(ErrorUri.Tag("tag:test.com,2024:Test"), "Test title");

        // Act
        var result = new Result<Guid> { Error = error };

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(error);
    }

    [Fact]
    public void Ctor_ValueTypeValueAndError_ThrowsException()
    {
        // Arrange
        var error = Error.NotFound(ErrorUri.Tag("tag:test.com,2024:Test"), "Test title");

        // Act
        var exception = Record.Exception(() => new Result<int> { Value = 43, Error = error });

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidOperationException>();
        exception.Message.ShouldBe("Cannot set both Value and Error of the Result!");
    }

    [Fact]
    public void Ctor_ValueTypeErrorAndValue_ThrowsException()
    {
        // Arrange
        var error = Error.NotFound(ErrorUri.Tag("tag:test.com,2024:Test"), "Test title");

        // Act
        var exception = Record.Exception(() => new Result<int>(error) { Value = 43 });

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidOperationException>();
        exception.Message.ShouldBe("Cannot set both Value and Error of the Result!");
    }

    #endregion

    #region Result<T>, value type round-trip

    [Fact]
    public void RoundTrip_ErrorWithValueTypeAndMicrosoftSerializer_ReturnsErrorResult()
    {
        // Arrange
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        var sut = new Result<int>(Error.NotFound(ErrorUri.Tag(ExpectedTypeUri), ExpectedTitle));
        var json = SerializationHelper.SerializeWithMicrosoft(sut);

        // Act
        var result = SerializationHelper.DeserializeWithMicrosoft<Result<int>>(json);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldNotBeNull();
        result.Error.Title.ShouldBe(ExpectedTitle);
        result.Error.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void RoundTrip_ErrorWithValueTypeAndNewtonsoftSerializer_ReturnsErrorResult()
    {
        // Arrange
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        var sut = new Result<int>(Error.NotFound(ErrorUri.Tag(ExpectedTypeUri), ExpectedTitle));
        var json = SerializationHelper.SerializeWithNewtonsoft(sut);

        // Act
        var result = SerializationHelper.DeserializeWithNewtonsoft<Result<int>>(json);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldNotBeNull();
        result.Error.Title.ShouldBe(ExpectedTitle);
        result.Error.TypeUri.ShouldBe(ExpectedTypeUri);
    }

    [Fact]
    public void RoundTrip_ErrorWithStructValueTypeAndMicrosoftSerializer_ReturnsErrorResult()
    {
        // Arrange
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        var sut = new Result<Guid>(Error.NotFound(ErrorUri.Tag(ExpectedTypeUri), ExpectedTitle));
        var json = SerializationHelper.SerializeWithMicrosoft(sut);

        // Act
        var result = SerializationHelper.DeserializeWithMicrosoft<Result<Guid>>(json);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldNotBeNull();
        result.Error.Title.ShouldBe(ExpectedTitle);
    }

    [Fact]
    public void RoundTrip_ErrorWithStructValueTypeAndNewtonsoftSerializer_ReturnsErrorResult()
    {
        // Arrange
        const string ExpectedTitle = "Test title";
        const string ExpectedTypeUri = "tag:test.com,2024:Test";

        var sut = new Result<Guid>(Error.NotFound(ErrorUri.Tag(ExpectedTypeUri), ExpectedTitle));
        var json = SerializationHelper.SerializeWithNewtonsoft(sut);

        // Act
        var result = SerializationHelper.DeserializeWithNewtonsoft<Result<Guid>>(json);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldNotBeNull();
        result.Error.Title.ShouldBe(ExpectedTitle);
    }

    [Fact]
    public void RoundTrip_SuccessWithDefaultValueTypeAndMicrosoftSerializer_ReturnsSuccessResult()
    {
        // Arrange
        const int ExpectedValue = 0;

        var sut = Result.FromValue(0);
        var json = SerializationHelper.SerializeWithMicrosoft(sut);

        // Act
        var result = SerializationHelper.DeserializeWithMicrosoft<Result<int>>(json);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public void RoundTrip_SuccessWithDefaultValueTypeAndNewtonsoftSerializer_ReturnsSuccessResult()
    {
        // Arrange
        const int ExpectedValue = 0;

        var sut = Result.FromValue(0);
        var json = SerializationHelper.SerializeWithNewtonsoft(sut);

        // Act
        var result = SerializationHelper.DeserializeWithNewtonsoft<Result<int>>(json);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public void Serialize_ErrorWithValueTypeAndMicrosoftSerializer_DoesNotWritePhantomValue()
    {
        // Arrange
        var sut = new Result<int>(Error.NotFound(ErrorUri.Tag("tag:test.com,2024:Test"), "Test title"));

        // Act
        var result = SerializationHelper.SerializeWithMicrosoft(sut);

        // Assert
        result.ShouldNotContain("\"Value\":0");
    }

    [Fact]
    public void Serialize_ErrorWithValueTypeAndNewtonsoftSerializer_DoesNotWritePhantomValue()
    {
        // Arrange
        var sut = new Result<int>(Error.NotFound(ErrorUri.Tag("tag:test.com,2024:Test"), "Test title"));

        // Act
        var result = SerializationHelper.SerializeWithNewtonsoft(sut);

        // Assert
        result.ShouldNotContain("\"Value\":0");
    }

    #endregion

    #region helper classes

    private class TestClass
    {
        public string? Text { get; init; }
        public int Number { get; init; }
    }

    #endregion
}
