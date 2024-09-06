using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Maple.Result.Converters;
using Sut = Maple.Result.MessageTemplate;

namespace Result.Tests.Unit;

public class MessageTemplateUnitTests
{
    #region read-only fields

    private readonly JsonSerializerOptions _jsonSerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    #endregion

    #region Deserialize

    [Fact]
    public void Deserialize_OnlyTemplateId_ReturnsMessageTemplateWithMessageId()
    {
        // Arrange
        const string ExpectedTemplateId = "user.account.activated";

        const string Json = "{\"templateId\":\"user.account.activated\"}";

        // Act
        var sut = Deserialize(Json);

        // Assert
        sut.TemplateId.ShouldBe(ExpectedTemplateId);
        sut.Params.ShouldBeNull();
    }

    [Fact]
    public void Deserialize_TemplateIdAndEmptyParameters_ReturnsMessageTemplateWithMessageIdAndEmptyParameters()
    {
        // Arrange
        const string ExpectedTemplateId = "user.account.activated";

        const string Json = "{\"templateId\":\"user.account.activated\",\"params\":{}}";

        // Act
        var sut = Deserialize(Json);

        // Assert
        sut.TemplateId.ShouldBe(ExpectedTemplateId);
        sut.Params.ShouldBeEmpty();
    }

    [Fact]
    public void Deserialize_TemplateIdAndParameters_ReturnsMessageTemplateWithMessageIdAndParameters()
    {
        // Arrange
        _jsonSerializerOptions.Converters.Add(new ObjectAsPrimitiveConverter());

        const string ExpectedTemplateId = "user.account.activated";
        const int ExpectedParametersCount = 5;
        const string Parameter1Name = "userName";
        const string Parameter2Name = "isSuccessful";
        const string Parameter3Name = "bonusPoints";
        const string Parameter4Name = "weight";
        const string Parameter5Name = "dob";
        const string ExpectedParameter1Value = "johnny";
        const bool ExpectedParameter2Value = true;
        const int ExpectedParameter3Value = 100;
        const decimal ExpectedParameter4Value = 78.3m;
        const string ExpectedParameter5Value = "1980-04-19";

        const string Json =
            "{\"templateId\":\"user.account.activated\",\"params\":{\"userName\":\"johnny\",\"isSuccessful\":true,\"bonusPoints\":100,\"weight\":78.3,\"dob\":\"1980-04-19\"}}";

        // Act
        var sut = Deserialize(Json);

        // Assert
        sut.TemplateId.ShouldBe(ExpectedTemplateId);
        sut.Params.ShouldNotBeNull();
        sut.Params.Count.ShouldBe(ExpectedParametersCount);
        sut.Params.Keys.ShouldContain(Parameter1Name);
        sut.Params.Keys.ShouldContain(Parameter2Name);
        sut.Params.Keys.ShouldContain(Parameter3Name);
        sut.Params.Keys.ShouldContain(Parameter4Name);
        sut.Params.Keys.ShouldContain(Parameter5Name);
        sut.Params[Parameter1Name].ShouldBe(ExpectedParameter1Value);
        sut.Params[Parameter2Name].ShouldBe(ExpectedParameter2Value);
        sut.Params[Parameter3Name].ShouldBe(ExpectedParameter3Value);
        sut.Params[Parameter4Name].ShouldBe(ExpectedParameter4Value);
        sut.Params[Parameter5Name].ShouldBe(ExpectedParameter5Value);
    }

    #endregion

    #region Serialize

    [Fact]
    public void Serialize_OnlyTemplateId_ReturnsSerializedValueWithoutParams()
    {
        // Arrange
        const string ExpectedText = "{\"templateId\":\"user.account.activated\"}";

        const string TemplateId = "user.account.activated";
        var sut = new Sut(TemplateId);

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
        var sut = new Sut(TemplateId, new Dictionary<string, object>());

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
        var parameters = new Dictionary<string, object>
        {
            ["userName"] = "johnny",
            ["isSuccessful"] = true,
            ["bonusPoints"] = 100,
            ["weight"] = 78.3m,
            ["dob"] = new DateOnly(1980, 4, 19),
            ["registered"] = new DateTimeOffset(2022, 4, 15, 12, 17, 58, TimeSpan.FromHours(-5))
        };

        var sut = new Sut(TemplateId, parameters);

        // Act
        var result = Serialize(sut);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    #endregion

    #region DoubleSerialize

    [Fact]
    public void DoubleSerialize_OnlyTemplateId_ReturnsSerializedValueWithoutParams()
    {
        // Arrange
        const string ExpectedText = "{\"templateId\":\"user.account.activated\"}";

        const string TemplateId = "user.account.activated";
        var sut = new Sut(TemplateId);

        // Act
        var json = Serialize(sut);
        var obj = Deserialize(json);
        var result = Serialize(obj);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    [Fact]
    public void DoubleSerialize_EmptyParams_ReturnsSerializedValueWithEmptyParams()
    {
        // Arrange
        const string ExpectedText = "{\"templateId\":\"user.account.activated\",\"params\":{}}";

        const string TemplateId = "user.account.activated";
        var sut = new Sut(TemplateId, new Dictionary<string, object>());

        // Act
        var json = Serialize(sut);
        var obj = Deserialize(json);
        var result = Serialize(obj);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    [Fact]
    public void DoubleSerialize_NotEmptyParams_ReturnsSerializedValueWithParams()
    {
        // Arrange
        const string ExpectedText =
            "{\"templateId\":\"user.account.activated\",\"params\":{\"userName\":\"johnny\",\"isSuccessful\":true,\"bonusPoints\":100,\"weight\":78.3,\"dob\":\"1980-04-19\"}}";

        const string TemplateId = "user.account.activated";
        var parameters = new Dictionary<string, object>
        {
            ["userName"] = "johnny",
            ["isSuccessful"] = true,
            ["bonusPoints"] = 100,
            ["weight"] = 78.3m,
            ["dob"] = new DateOnly(1980, 4, 19)
        };

        var sut = new Sut(TemplateId, parameters);

        // Act
        var json = Serialize(sut);
        var obj = Deserialize(json);
        var result = Serialize(obj);

        // Assert
        result.ShouldBe(ExpectedText);
    }

    #endregion

    #region helper methods

    private Sut Deserialize(string json)
    {
        var result = JsonSerializer.Deserialize<Sut>(json, _jsonSerializerOptions);
        return result;
    }

    private string Serialize(Sut messageTemplate)
    {
        var result = JsonSerializer.Serialize(messageTemplate, _jsonSerializerOptions);
        return result;
    }

    #endregion
}