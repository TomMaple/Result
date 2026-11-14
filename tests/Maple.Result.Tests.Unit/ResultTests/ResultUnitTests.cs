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
using Sut = Maple.Result.Result;

namespace Maple.Result.Tests.Unit.ResultTests;

public class ResultUnitTests
{
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

        error = error.WithErrorDetail("#/property1", "Property 1 test detail", "message-property-id", ("pk1", "pv1"), ("pk2", "pv2"));

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
        result.Error.DetailTemplated.Params[ExpectedDetailParam1Key].ShouldBe(ExpectedDetailParam1Value);
        result.Error.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        result.Error.DetailTemplated.Params[ExpectedDetailParam2Key].ShouldBe(ExpectedDetailParam2Value);
        result.Error.ErrorDetails.ShouldNotBeNull();
        result.Error.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        result.Error.ErrorDetails[0].PropertyPointer.ShouldBe(ExpectedErrorDetail1PropertyPointer);
        result.Error.ErrorDetails[0].Detail.ShouldBe(ExpectedErrorDetail1Detail);
        result.Error.ErrorDetails[0].DetailTemplated.ShouldNotBeNull();
        result.Error.ErrorDetails[0].DetailTemplated!.TemplateId.ShouldBe(ExpectedErrorDetail1DetailTemplateId);
        result.Error.ErrorDetails[0].DetailTemplated!.Params.ShouldNotBeNull();
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Count.ShouldBe(ExpectedErrorDetail1DetailParamsCount);
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Keys.ShouldContain(ExpectedErrorDetail1DetailParam1Key);
        result.Error.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam1Key].ShouldBe(ExpectedErrorDetail1DetailParam1Value);
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Keys.ShouldContain(ExpectedErrorDetail1DetailParam2Key);
        result.Error.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam2Key].ShouldBe(ExpectedErrorDetail1DetailParam2Value);
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
    public void FromError_NullError_ThrowsException()
    {
        // Act
        var exception = Record.Exception(() => Sut.FromError(null!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
    }

    [Fact]
    public void ImplicitOperator_NullError_ThrowsException()
    {
        // Act
        var exception = Record.Exception(() =>
        {
            Sut _ = (Error)null!;
        });

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
    }

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

        error = error.WithErrorDetail("#/property1", "Property 1 test detail", "message-property-id", ("pk1", "pv1"), ("pk2", "pv2"));

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
        result.Error.DetailTemplated.Params[ExpectedDetailParam1Key].ShouldBe(ExpectedDetailParam1Value);
        result.Error.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        result.Error.DetailTemplated.Params[ExpectedDetailParam2Key].ShouldBe(ExpectedDetailParam2Value);
        result.Error.ErrorDetails.ShouldNotBeNull();
        result.Error.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        result.Error.ErrorDetails[0].PropertyPointer.ShouldBe(ExpectedErrorDetail1PropertyPointer);
        result.Error.ErrorDetails[0].Detail.ShouldBe(ExpectedErrorDetail1Detail);
        result.Error.ErrorDetails[0].DetailTemplated.ShouldNotBeNull();
        result.Error.ErrorDetails[0].DetailTemplated!.TemplateId.ShouldBe(ExpectedErrorDetail1DetailTemplateId);
        result.Error.ErrorDetails[0].DetailTemplated!.Params.ShouldNotBeNull();
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Count.ShouldBe(ExpectedErrorDetail1DetailParamsCount);
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Keys.ShouldContain(ExpectedErrorDetail1DetailParam1Key);
        result.Error.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam1Key].ShouldBe(ExpectedErrorDetail1DetailParam1Value);
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Keys.ShouldContain(ExpectedErrorDetail1DetailParam2Key);
        result.Error.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam2Key].ShouldBe(ExpectedErrorDetail1DetailParam2Value);
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

    [Fact]
    public void Success_CalledTwice_ReturnsSameInstance()
    {
        // Act
        var first = Sut.Success();
        var second = Sut.Success();

        // Assert
        second.ShouldBeSameAs(first);
    }

    #endregion

    #region Result, Serialize

    [Fact]
    public void Serialize_AllPropertiesAndMicrosoftSerializer_ReturnsSerializedValueWithAllProperties()
    {
        // Arrange
        const string ExpectedText = """{"Error":{"Category":3,"TypeUri":"tag:test.com,2024:Test","Title":"Test title","Detail":"Test description.","DetailTemplated":{"TemplateId":"messageId","Params":{"key1":"value1","key2":"value2"}},"InstanceUri":"http://test.com/instance/1013","ErrorDetails":[{"PropertyPointer":"#/property1","Detail":"Property 1 test detail","DetailTemplated":{"TemplateId":"message-property-id","Params":{"pk1":"pv1","pk2":"pv2"}}}],"Metadata":{}}}""";

        var error = Error.NotFound(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "messageId",
            ("key1", "value1"), ("key2", "value2"));

        error = error.WithErrorDetail("#/property1", "Property 1 test detail", "message-property-id", ("pk1", "pv1"), ("pk2", "pv2"));

        var sut = Sut.FromError(error);

        // Act
        var result = SerializationHelper.SerializeWithMicrosoft(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    [Fact]
    public void Serialize_AllPropertiesAndNewtonsoftSerializer_ReturnsSerializedValueWithAllProperties()
    {
        // Arrange
        const string ExpectedText = """{"Error":{"Category":3,"TypeUri":"tag:test.com,2024:Test","Title":"Test title","Detail":"Test description.","DetailTemplated":{"TemplateId":"messageId","Params":{"key1":"value1","key2":"value2"}},"InstanceUri":"http://test.com/instance/1013","ErrorDetails":[{"PropertyPointer":"#/property1","Detail":"Property 1 test detail","DetailTemplated":{"TemplateId":"message-property-id","Params":{"pk1":"pv1","pk2":"pv2"}}}],"Metadata":{}}}""";

        var error = Error.NotFound(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "messageId",
            ("key1", "value1"), ("key2", "value2"));

        error = error.WithErrorDetail("#/property1", "Property 1 test detail", "message-property-id", ("pk1", "pv1"), ("pk2", "pv2"));

        var sut = Sut.FromError(error);

        // Act
        var result = SerializationHelper.SerializeWithNewtonsoft(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    #endregion

    #region Result<T>, IsSuccess()

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

        error = error.WithErrorDetail("#/property1", "Property 1 test detail", "message-property-id", ("pk1", "pv1"), ("pk2", "pv2"));

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
        result.Error.DetailTemplated.Params[ExpectedDetailParam1Key].ShouldBe(ExpectedDetailParam1Value);
        result.Error.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        result.Error.DetailTemplated.Params[ExpectedDetailParam2Key].ShouldBe(ExpectedDetailParam2Value);
        result.Error.ErrorDetails.ShouldNotBeNull();
        result.Error.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        result.Error.ErrorDetails[0].PropertyPointer.ShouldBe(ExpectedErrorDetail1PropertyPointer);
        result.Error.ErrorDetails[0].Detail.ShouldBe(ExpectedErrorDetail1Detail);
        result.Error.ErrorDetails[0].DetailTemplated.ShouldNotBeNull();
        result.Error.ErrorDetails[0].DetailTemplated!.TemplateId.ShouldBe(ExpectedErrorDetail1DetailTemplateId);
        result.Error.ErrorDetails[0].DetailTemplated!.Params.ShouldNotBeNull();
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Count.ShouldBe(ExpectedErrorDetail1DetailParamsCount);
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Keys.ShouldContain(ExpectedErrorDetail1DetailParam1Key);
        result.Error.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam1Key].ShouldBe(ExpectedErrorDetail1DetailParam1Value);
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Keys.ShouldContain(ExpectedErrorDetail1DetailParam2Key);
        result.Error.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam2Key].ShouldBe(ExpectedErrorDetail1DetailParam2Value);
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
    public void FromError_ValueNullError_ThrowsException()
    {
        // Act
        var exception = Record.Exception(() => Result<TestClass>.FromError(null!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
    }

    [Fact]
    public void ImplicitOperator_ValueNullError_ThrowsException()
    {
        // Act
        var exception = Record.Exception(() =>
        {
            Result<TestClass> _ = (Error)null!;
        });

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
    }

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

        error = error.WithErrorDetail("#/property1", "Property 1 test detail", "message-property-id", ("pk1", "pv1"), ("pk2", "pv2"));

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
        result.Error.DetailTemplated.Params[ExpectedDetailParam1Key].ShouldBe(ExpectedDetailParam1Value);
        result.Error.DetailTemplated.Params.Keys.ShouldContain(ExpectedDetailParam2Key);
        result.Error.DetailTemplated.Params[ExpectedDetailParam2Key].ShouldBe(ExpectedDetailParam2Value);
        result.Error.ErrorDetails.ShouldNotBeNull();
        result.Error.ErrorDetails.Count.ShouldBe(ExpectedErrorDetailsCount);
        result.Error.ErrorDetails[0].PropertyPointer.ShouldBe(ExpectedErrorDetail1PropertyPointer);
        result.Error.ErrorDetails[0].Detail.ShouldBe(ExpectedErrorDetail1Detail);
        result.Error.ErrorDetails[0].DetailTemplated.ShouldNotBeNull();
        result.Error.ErrorDetails[0].DetailTemplated!.TemplateId.ShouldBe(ExpectedErrorDetail1DetailTemplateId);
        result.Error.ErrorDetails[0].DetailTemplated!.Params.ShouldNotBeNull();
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Count.ShouldBe(ExpectedErrorDetail1DetailParamsCount);
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Keys.ShouldContain(ExpectedErrorDetail1DetailParam1Key);
        result.Error.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam1Key].ShouldBe(ExpectedErrorDetail1DetailParam1Value);
        result.Error.ErrorDetails[0].DetailTemplated!.Params!.Keys.ShouldContain(ExpectedErrorDetail1DetailParam2Key);
        result.Error.ErrorDetails[0].DetailTemplated!.Params![ExpectedErrorDetail1DetailParam2Key].ShouldBe(ExpectedErrorDetail1DetailParam2Value);
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
        const string ExpectedText = """{"Value":null,"Error":{"Category":3,"TypeUri":"tag:test.com,2024:Test","Title":"Test title","Detail":"Test description.","DetailTemplated":{"TemplateId":"messageId","Params":{"key1":"value1","key2":"value2"}},"InstanceUri":"http://test.com/instance/1013","ErrorDetails":[{"PropertyPointer":"#/property1","Detail":"Property 1 test detail","DetailTemplated":{"TemplateId":"message-property-id","Params":{"pk1":"pv1","pk2":"pv2"}}}],"Metadata":{}}}""";

        var error = Error.NotFound(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "messageId",
            ("key1", "value1"), ("key2", "value2"));

        error = error.WithErrorDetail("#/property1", "Property 1 test detail", "message-property-id", ("pk1", "pv1"), ("pk2", "pv2"));

        var sut = Result<TestClass>.FromError(error);

        // Act
        var result = SerializationHelper.SerializeWithMicrosoft(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    [Fact]
    public void Serialize_ReferenceTypeAndErrorAllPropertiesAndNewtonsoftSerializer_ReturnsSerializedValueWithAllProperties()
    {
        // Arrange
        const string ExpectedText = """{"Value":null,"Error":{"Category":3,"TypeUri":"tag:test.com,2024:Test","Title":"Test title","Detail":"Test description.","DetailTemplated":{"TemplateId":"messageId","Params":{"key1":"value1","key2":"value2"}},"InstanceUri":"http://test.com/instance/1013","ErrorDetails":[{"PropertyPointer":"#/property1","Detail":"Property 1 test detail","DetailTemplated":{"TemplateId":"message-property-id","Params":{"pk1":"pv1","pk2":"pv2"}}}],"Metadata":{}}}""";

        var error = Error.NotFound(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "messageId",
            ("key1", "value1"), ("key2", "value2"));

        error = error.WithErrorDetail("#/property1", "Property 1 test detail", "message-property-id", ("pk1", "pv1"), ("pk2", "pv2"));

        var sut = Result<TestClass>.FromError(error);

        // Act
        var result = SerializationHelper.SerializeWithNewtonsoft(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    [Fact]
    public void Serialize_ValueTypeAndErrorAllPropertiesAndMicrosoftSerializer_ReturnsSerializedValueWithAllProperties()
    {
        // Arrange
        const string ExpectedText = """{"Value":0,"Error":{"Category":3,"TypeUri":"tag:test.com,2024:Test","Title":"Test title","Detail":"Test description.","DetailTemplated":{"TemplateId":"messageId","Params":{"key1":"value1","key2":"value2"}},"InstanceUri":"http://test.com/instance/1013","ErrorDetails":[{"PropertyPointer":"#/property1","Detail":"Property 1 test detail","DetailTemplated":{"TemplateId":"message-property-id","Params":{"pk1":"pv1","pk2":"pv2"}}}],"Metadata":{}}}""";

        var error = Error.NotFound(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "messageId",
            ("key1", "value1"), ("key2", "value2"));

        error = error.WithErrorDetail("#/property1", "Property 1 test detail", "message-property-id", ("pk1", "pv1"), ("pk2", "pv2"));

        var sut = Result<int>.FromError(error);

        // Act
        var result = SerializationHelper.SerializeWithMicrosoft(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    [Fact]
    public void Serialize_ValueTypeAndErrorAllPropertiesAndNewtonsoftSerializer_ReturnsSerializedValueWithAllProperties()
    {
        // Arrange
        const string ExpectedText = """{"Value":0,"Error":{"Category":3,"TypeUri":"tag:test.com,2024:Test","Title":"Test title","Detail":"Test description.","DetailTemplated":{"TemplateId":"messageId","Params":{"key1":"value1","key2":"value2"}},"InstanceUri":"http://test.com/instance/1013","ErrorDetails":[{"PropertyPointer":"#/property1","Detail":"Property 1 test detail","DetailTemplated":{"TemplateId":"message-property-id","Params":{"pk1":"pv1","pk2":"pv2"}}}],"Metadata":{}}}""";

        var error = Error.NotFound(
            ErrorUri.Tag("tag:test.com,2024:Test"),
            "Test title",
            "Test description.",
            ErrorUri.Locator("http://test.com/instance/1013"),
            "messageId",
            ("key1", "value1"), ("key2", "value2"));

        error = error.WithErrorDetail("#/property1", "Property 1 test detail", "message-property-id", ("pk1", "pv1"), ("pk2", "pv2"));

        var sut = Result<int>.FromError(error);

        // Act
        var result = SerializationHelper.SerializeWithNewtonsoft(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    #endregion

    #region Result<T>, value type failure

    [Fact]
    public void FromError_ValueTypeError_ReturnsErrorResult()
    {
        // Arrange
        var error = Error.NotFound(ErrorUri.Tag("tag:test.com,2024:Test"), "Test title");

        // Act
        var result = Result<int>.FromError(error);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(error);
        result.Value.ShouldBe(0);
    }

    [Fact]
    public void FromError_StructValueTypeError_ReturnsErrorResult()
    {
        // Arrange
        var error = Error.NotFound(ErrorUri.Tag("tag:test.com,2024:Test"), "Test title");

        // Act
        var result = Result<Guid>.FromError(error);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(error);
        result.Value.ShouldBe(Guid.Empty);
    }

    #endregion

    #region Result<T>, value type serialization

    [Fact]
    public void Serialize_ErrorWithValueTypeAndMicrosoftSerializer_WritesDefaultValue()
    {
        // Arrange
        var sut = Result<int>.FromError(Error.NotFound(ErrorUri.Tag("tag:test.com,2024:Test"), "Test title"));

        // Act
        var result = SerializationHelper.SerializeWithMicrosoft(sut);

        // Assert
        result.ShouldContain("\"Value\":0");
    }

    [Fact]
    public void Serialize_ErrorWithValueTypeAndNewtonsoftSerializer_WritesDefaultValue()
    {
        // Arrange
        var sut = Result<int>.FromError(Error.NotFound(ErrorUri.Tag("tag:test.com,2024:Test"), "Test title"));

        // Act
        var result = SerializationHelper.SerializeWithNewtonsoft(sut);

        // Assert
        result.ShouldContain("\"Value\":0");
    }

    #endregion

    #region Result<T>, nullable value

    [Fact]
    public void FromValue_NullNullableReferenceType_ReturnsSuccessResultWithNullValue()
    {
        // Act
        var result = Result.FromValue<string?>(null);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBeNull();
        result.Error.ShouldBeNull();
    }

    [Fact]
    public void FromValue_NullReferenceType_ReturnsSuccessResultWithNullValue()
    {
        // Act
        var result = Result.FromValue<string>(null!);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBeNull();
        result.Error.ShouldBeNull();
    }

    [Fact]
    public void FromValue_NullNullableValueType_ReturnsSuccessResultWithNullValue()
    {
        // Act
        var result = Result.FromValue<int?>(null);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBeNull();
        result.Error.ShouldBeNull();
    }

    [Fact]
    public void ImplicitOperator_NullReferenceType_ReturnsSuccessResultWithNullValue()
    {
        // Arrange
        string? value = null;

        // Act
        Result<string?> result = value;

        // Assert
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBeNull();
    }

    [Fact]
    public void NullValueSuccess_DistinctFromFailure()
    {
        // Arrange
        var success = Result.FromValue<string?>(null);
        var failure = Result<string?>.FromError(Error.NotFound(ErrorUri.Tag("tag:test.com,2024:Test"), "Test title"));

        // Assert
        success.IsSuccess().ShouldBeTrue();
        failure.IsSuccess().ShouldBeFalse();
        success.ShouldNotBe(failure);
    }

    [Fact]
    public void Serialize_NullReferenceTypeValueAndMicrosoftSerializer_WritesNullValue()
    {
        // Arrange
        const string ExpectedText = """{"Value":null,"Error":null}""";
        var sut = Result.FromValue<string?>(null);

        // Act
        var result = SerializationHelper.SerializeWithMicrosoft(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    [Fact]
    public void Serialize_NullReferenceTypeValueAndNewtonsoftSerializer_WritesNullValue()
    {
        // Arrange
        const string ExpectedText = """{"Value":null,"Error":null}""";
        var sut = Result.FromValue<string?>(null);

        // Act
        var result = SerializationHelper.SerializeWithNewtonsoft(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    [Fact]
    public void Serialize_NullNullableValueTypeValueAndMicrosoftSerializer_WritesNullValue()
    {
        // Arrange
        const string ExpectedText = """{"Value":null,"Error":null}""";
        var sut = Result.FromValue<int?>(null);

        // Act
        var result = SerializationHelper.SerializeWithMicrosoft(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    [Fact]
    public void Serialize_NullNullableValueTypeValueAndNewtonsoftSerializer_WritesNullValue()
    {
        // Arrange
        const string ExpectedText = """{"Value":null,"Error":null}""";
        var sut = Result.FromValue<int?>(null);

        // Act
        var result = SerializationHelper.SerializeWithNewtonsoft(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    #endregion

    #region deserialization is not supported

    [Fact]
    public void Deserialize_FailedResultAndMicrosoftSerializer_ThrowsNotSupportedException()
    {
        // Arrange
        var json = SerializationHelper.SerializeWithMicrosoft(
            Sut.FromError(Error.NotFound(ErrorUri.Tag("tag:test.com,2024:Test"), "Test title")));

        // Act
        var exception = Record.Exception(() => System.Text.Json.JsonSerializer.Deserialize<Sut>(json));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<NotSupportedException>();
    }

    [Fact]
    public void Deserialize_FailedResultAndNewtonsoftSerializer_ThrowsJsonSerializationException()
    {
        // Arrange
        var json = SerializationHelper.SerializeWithNewtonsoft(
            Sut.FromError(Error.NotFound(ErrorUri.Tag("tag:test.com,2024:Test"), "Test title")));

        // Act
        var exception = Record.Exception(() => Newtonsoft.Json.JsonConvert.DeserializeObject<Sut>(json));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<Newtonsoft.Json.JsonSerializationException>();
    }

    [Fact]
    public void Deserialize_SuccessfulResultAndNewtonsoftSerializer_ThrowsJsonSerializationException()
    {
        // Arrange
        var json = SerializationHelper.SerializeWithNewtonsoft(Sut.Success());

        // Act
        var exception = Record.Exception(() => Newtonsoft.Json.JsonConvert.DeserializeObject<Sut>(json));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<Newtonsoft.Json.JsonSerializationException>();
    }

    [Fact]
    public void Deserialize_FailedResultOfTAndMicrosoftSerializer_ThrowsNotSupportedException()
    {
        // Arrange
        var json = SerializationHelper.SerializeWithMicrosoft(
            Result<int>.FromError(Error.NotFound(ErrorUri.Tag("tag:test.com,2024:Test"), "Test title")));

        // Act
        var exception = Record.Exception(() => System.Text.Json.JsonSerializer.Deserialize<Result<int>>(json));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<NotSupportedException>();
    }

    [Fact]
    public void Deserialize_FailedResultOfTAndNewtonsoftSerializer_ThrowsJsonSerializationException()
    {
        // Arrange
        var json = SerializationHelper.SerializeWithNewtonsoft(
            Result<int>.FromError(Error.NotFound(ErrorUri.Tag("tag:test.com,2024:Test"), "Test title")));

        // Act
        var exception = Record.Exception(() => Newtonsoft.Json.JsonConvert.DeserializeObject<Result<int>>(json));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<Newtonsoft.Json.JsonSerializationException>();
    }

    [Fact]
    public void Deserialize_SuccessfulResultOfTAndNewtonsoftSerializer_ThrowsJsonSerializationException()
    {
        // Arrange
        var json = SerializationHelper.SerializeWithNewtonsoft(Result<int>.FromValue(1));

        // Act
        var exception = Record.Exception(() => Newtonsoft.Json.JsonConvert.DeserializeObject<Result<int>>(json));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<Newtonsoft.Json.JsonSerializationException>();
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
