// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Maple.Result.Tests.Unit.TemplatedMessageTests;

public class TemplatedMessageUnitTests
{
    #region read-only fields

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    #endregion

    #region ctor

    [Fact]
    public void Ctor_NullTemplateId_ThrowsArgumentNullException()
    {
        // Act
        var exception = Record.Exception(() => new TemplatedMessage(null!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        ((ArgumentNullException)exception).ParamName.ShouldBe("templateId");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("   ")]
    public void Ctor_BlankTemplateId_ThrowsArgumentException(string templateId)
    {
        // Act
        var exception = Record.Exception(() => new TemplatedMessage(templateId));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentException>();
        ((ArgumentException)exception).ParamName.ShouldBe("templateId");
    }

    [Fact]
    public void Ctor_NullParams_KeepsNullParams()
    {
        // Act
        var sut = new TemplatedMessage("message-id");

        // Assert
        sut.TemplateId.ShouldBe("message-id");
        sut.Params.ShouldBeNull();
    }

    [Fact]
    public void Ctor_SourceDictionaryChangedLater_ParamsAreNotAffected()
    {
        // Arrange
        var source = new Dictionary<string, object?> { ["min"] = 3 };

        // Act
        var sut = new TemplatedMessage("message-id", source);
        source["min"] = 5;
        source["max"] = 10;

        // Assert
        sut.Params.ShouldNotBeNull();
        sut.Params.Count.ShouldBe(1);
        sut.Params["min"].ShouldBe(3);
        sut.Params.ShouldNotBeSameAs(source);
    }

    [Fact]
    public void Ctor_NullParamValue_KeepsNullValue()
    {
        // Act
        var sut = new TemplatedMessage("message-id", new Dictionary<string, object?> { ["name"] = null });

        // Assert
        sut.Params.ShouldNotBeNull();
        sut.Params.ShouldContainKey("name");
        sut.Params["name"].ShouldBeNull();
    }

    [Fact]
    public void Params_CastToMutableDictionary_CannotBeModified()
    {
        // Arrange
        var sut = new TemplatedMessage("message-id", new Dictionary<string, object?> { ["min"] = 3 });

        // Act & Assert
        var dictionary = sut.Params.ShouldBeAssignableTo<IDictionary<string, object?>>();
        dictionary.ShouldNotBeNull();
        dictionary.IsReadOnly.ShouldBeTrue();
        Should.Throw<NotSupportedException>(() => dictionary["min"] = 5);
        sut.Params!["min"].ShouldBe(3);
    }

    [Fact]
    public void Params_Keys_AreCaseSensitive()
    {
        // Arrange
        var source = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase) { ["min"] = 3 };

        // Act
        var sut = new TemplatedMessage("message-id", source);

        // Assert
        sut.Params.ShouldNotBeNull();
        sut.Params.ContainsKey("min").ShouldBeTrue();
        sut.Params.ContainsKey("MIN").ShouldBeFalse();
    }

    #endregion

    #region parameters

    [Fact]
    public void Ctor_InlineTuples_BuildsParams()
    {
        // Act
        var sut = new TemplatedMessage("errors.length", ("min", 3), ("max", null));

        // Assert
        sut.Params.ShouldNotBeNull();
        sut.Params.Count.ShouldBe(2);
        sut.Params["min"].ShouldBe(3);
        sut.Params["max"].ShouldBeNull();
    }

    [Fact]
    public void Ctor_TupleArray_BuildsParams()
    {
        // Arrange
        (string Key, object? Value)[] parameters = [("min", 3), ("max", 10)];

        // Act
        var sut = new TemplatedMessage("errors.length", parameters);

        // Assert
        sut.Params.ShouldNotBeNull();
        sut.Params.Count.ShouldBe(2);
        sut.Params["max"].ShouldBe(10);
    }

    [Fact]
    public void Ctor_ListOfPairs_BuildsParams()
    {
        // Arrange
        var parameters = new List<KeyValuePair<string, object?>> { new("min", 3) };

        // Act
        var sut = new TemplatedMessage("errors.length", parameters);

        // Assert
        sut.Params.ShouldNotBeNull();
        sut.Params["min"].ShouldBe(3);
    }

    [Fact]
    public void Ctor_NullSecondArgument_KeepsNullParams()
    {
        // Act
        var sut = new TemplatedMessage("errors.length", null);

        // Assert
        sut.Params.ShouldBeNull();
    }

    [Fact]
    public void Ctor_EmptyCollectionExpression_BuildsEmptyParams()
    {
        // Act
        var sut = new TemplatedMessage("errors.length", []);

        // Assert
        sut.Params.ShouldNotBeNull();
        sut.Params.ShouldBeEmpty();
    }

    [Fact]
    public void Ctor_EmptyTupleArray_BuildsEmptyParams()
    {
        // Act
        var sut = new TemplatedMessage("errors.length", Array.Empty<(string Key, object? Value)>());

        // Assert
        sut.Params.ShouldNotBeNull();
        sut.Params.ShouldBeEmpty();
    }

    [Fact]
    public void Ctor_RepeatedTupleName_ThrowsArgumentException()
    {
        // Act
        var exception = Record.Exception(() => new TemplatedMessage("errors.length", ("min", 3), ("min", 4)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentException>();
        exception.Message.ShouldContain("‘min’ is repeated");
        ((ArgumentException)exception).ParamName.ShouldBe("parameters");
    }

    [Fact]
    public void Ctor_RepeatedPairName_ThrowsArgumentException()
    {
        // Arrange
        var parameters = new List<KeyValuePair<string, object?>> { new("min", 3), new("min", 4) };

        // Act
        var exception = Record.Exception(() => new TemplatedMessage("errors.length", parameters));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentException>();
        ((ArgumentException)exception).ParamName.ShouldBe("parameters");
    }

    [Fact]
    public void Ctor_NullTupleName_ThrowsArgumentNullException()
    {
        // Act
        var exception = Record.Exception(() => new TemplatedMessage("errors.length", (null!, 3)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        ((ArgumentNullException)exception).ParamName.ShouldBe("parameters");
    }

    [Fact]
    public void Ctor_NullPairName_ThrowsArgumentNullException()
    {
        // Arrange
        var parameters = new List<KeyValuePair<string, object?>> { new(null!, 3) };

        // Act
        var exception = Record.Exception(() => new TemplatedMessage("errors.length", parameters));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        ((ArgumentNullException)exception).ParamName.ShouldBe("parameters");
    }

    [Fact]
    public void Equals_SameParamsFromTuplesAndDictionary_AreEqual()
    {
        // Arrange
        var fromTuples = new TemplatedMessage("errors.length", ("min", 3), ("max", 10));
        var fromDictionary = new TemplatedMessage("errors.length",
            new Dictionary<string, object?> { ["max"] = 10, ["min"] = 3 });

        // Act & Assert
        fromTuples.ShouldBe(fromDictionary);
        fromTuples.GetHashCode().ShouldBe(fromDictionary.GetHashCode());
    }

    #endregion

    #region ToString

    [Fact]
    public void ToString_WithParams_WritesTheirContent()
    {
        // Arrange
        var sut = new TemplatedMessage("message-id", new Dictionary<string, object?> { ["min"] = 3 });

        // Act
        var result = sut.ToString();

        // Assert
        result.ShouldBe("TemplatedMessage { TemplateId = message-id, Params = { min = 3 } }");
    }

    [Fact]
    public void ToString_EmptyParams_WritesEmptyDictionary()
    {
        // Arrange
        var sut = new TemplatedMessage("message-id", new Dictionary<string, object?>());

        // Act
        var result = sut.ToString();

        // Assert
        result.ShouldBe("TemplatedMessage { TemplateId = message-id, Params = {} }");
    }

    [Fact]
    public void ToString_NullParams_WritesNothingForParams()
    {
        // Arrange
        var sut = new TemplatedMessage("message-id");

        // Act
        var result = sut.ToString();

        // Assert
        result.ShouldBe("TemplatedMessage { TemplateId = message-id, Params =  }");
    }

    #endregion

    #region Serialize

    [Fact]
    public void Serialize_OnlyTemplateId_ReturnsSerializedValueWithoutParams()
    {
        // Arrange
        const string ExpectedText = "{\"templateId\":\"user.account.activated\"}";

        const string TemplateId = "user.account.activated";
        var sut = new TemplatedMessage(TemplateId);

        // Act
        var result = Serialize(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    [Fact]
    public void Serialize_EmptyParams_ReturnsSerializedValueWithEmptyParams()
    {
        // Arrange
        const string ExpectedText = "{\"templateId\":\"user.account.activated\",\"params\":{}}";

        const string TemplateId = "user.account.activated";
        var sut = new TemplatedMessage(TemplateId, new Dictionary<string, object?>());

        // Act
        var result = Serialize(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    [Fact]
    public void Serialize_NotEmptyParams_ReturnsSerializedValueWithParams()
    {
        // Arrange
        const string ExpectedText =
            "{\"templateId\":\"user.account.activated\",\"params\":{\"userName\":\"johnny\",\"isSuccessful\":true,\"bonusPoints\":100,\"weight\":78.3,\"dob\":\"1980-04-19\",\"registered\":\"2022-04-15T12:17:58-05:00\"}}";

        const string TemplateId = "user.account.activated";
        var parameters = new Dictionary<string, object?>
        {
            ["userName"] = "johnny",
            ["isSuccessful"] = true,
            ["bonusPoints"] = 100,
            ["weight"] = 78.3m,
            ["dob"] = new DateOnly(1980, 4, 19),
            ["registered"] = new DateTimeOffset(2022, 4, 15, 12, 17, 58, TimeSpan.FromHours(-5))
        };

        var sut = new TemplatedMessage(TemplateId, parameters);

        // Act
        var result = Serialize(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    #endregion

    #region helper methods

    private string Serialize(TemplatedMessage templatedMessage)
    {
        var result = JsonSerializer.Serialize(templatedMessage, _jsonSerializerOptions);
        return result;
    }

    #endregion
}
