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
using System.Text.Json;
using Sut = Maple.Result.Error;

namespace Maple.Result.Tests.Unit;

public class ErrorMetadataUnitTests
{
    #region Metadata

    [Fact]
    public void Metadata_FactoryMethod_IsEmpty()
    {
        // Act
        var sut = CreateError();

        // Assert
        sut.Metadata.ShouldNotBeNull();
        sut.Metadata.ShouldBeEmpty();
    }

    [Fact]
    public void Metadata_ParameterlessConstructor_IsEmpty()
    {
        // Act
        var sut = new Sut();

        // Assert
        sut.Metadata.ShouldNotBeNull();
        sut.Metadata.ShouldBeEmpty();
    }

    [Fact]
    public void Metadata_InitWithNull_IsEmpty()
    {
        // Act
        var sut = CreateError() with { Metadata = null! };

        // Assert
        sut.Metadata.ShouldNotBeNull();
        sut.Metadata.ShouldBeEmpty();
    }

    [Fact]
    public void Metadata_InitWithDictionary_CopiesEntries()
    {
        // Arrange
        var source = new Dictionary<string, object> { ["limit"] = 100 };

        // Act
        var sut = CreateError() with { Metadata = source };
        source["limit"] = 200;
        source["added"] = true;

        // Assert
        sut.Metadata.Count.ShouldBe(1);
        sut.Metadata["limit"].ShouldBe(100);
    }

    [Fact]
    public void Metadata_Keys_AreCaseSensitive()
    {
        // Act
        var sut = CreateError().WithMetadata("limit", 100).WithMetadata("Limit", 200);

        // Assert
        sut.Metadata.Count.ShouldBe(2);
    }

    [Fact]
    public void Metadata_WithExpression_DoesNotShareDictionary()
    {
        // Arrange
        var original = CreateError().WithMetadata("limit", 100);

        // Act
        var copy = original with { Title = "Changed title" };

        // Assert
        copy.Metadata.ShouldNotBeSameAs(original.Metadata);
        copy.Metadata["limit"].ShouldBe(100);
    }

    #endregion

    #region WithMetadata

    [Fact]
    public void WithMetadata_NewKey_ReturnsCopyWithEntryAndLeavesOriginalUntouched()
    {
        // Arrange
        var original = CreateError();

        // Act
        var sut = original.WithMetadata("limit", 100);

        // Assert
        sut.ShouldNotBeSameAs(original);
        sut.Metadata.Count.ShouldBe(1);
        sut.Metadata["limit"].ShouldBe(100);
        original.Metadata.ShouldBeEmpty();
    }

    [Fact]
    public void WithMetadata_ExistingKey_ReplacesValue()
    {
        // Arrange
        var original = CreateError().WithMetadata("limit", 100);

        // Act
        var sut = original.WithMetadata("limit", 200);

        // Assert
        sut.Metadata.Count.ShouldBe(1);
        sut.Metadata["limit"].ShouldBe(200);
        original.Metadata["limit"].ShouldBe(100);
    }

    [Fact]
    public void WithMetadata_Sequence_AddsAndReplacesEntriesAndKeepsOthers()
    {
        // Arrange
        var original = CreateError().WithMetadata("limit", 100).WithMetadata("remaining", 5);

        // Act
        var sut = original.WithMetadata(new Dictionary<string, object> { ["remaining"] = 0, ["resetAt"] = "tomorrow" });

        // Assert
        sut.Metadata.Count.ShouldBe(3);
        sut.Metadata["limit"].ShouldBe(100);
        sut.Metadata["remaining"].ShouldBe(0);
        sut.Metadata["resetAt"].ShouldBe("tomorrow");
        original.Metadata.Count.ShouldBe(2);
        original.Metadata["remaining"].ShouldBe(5);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void WithMetadata_EmptyOrWhitespaceKey_ThrowsArgumentException(string key)
    {
        // Arrange
        var sut = CreateError();

        // Act & Assert
        Should.Throw<ArgumentException>(() => sut.WithMetadata(key, 1));
    }

    [Fact]
    public void WithMetadata_NullKey_ThrowsArgumentException()
    {
        // Arrange
        var sut = CreateError();

        // Act & Assert
        Should.Throw<ArgumentException>(() => sut.WithMetadata(null!, 1));
    }

    [Theory]
    [InlineData("type")]
    [InlineData("title")]
    [InlineData("status")]
    [InlineData("detail")]
    [InlineData("instance")]
    [InlineData("errors")]
    [InlineData("detailTemplated")]
    [InlineData("Title")]
    [InlineData("STATUS")]
    [InlineData("DetailTemplated")]
    public void WithMetadata_ReservedKey_ThrowsArgumentException(string key)
    {
        // Arrange
        var sut = CreateError();

        // Act & Assert
        var exception = Should.Throw<ArgumentException>(() => sut.WithMetadata(key, 1));
        exception.ShouldNotBeOfType<ArgumentNullException>();
        exception.ParamName.ShouldBe("key");
    }

    [Theory]
    [InlineData("metadata")]
    [InlineData("traceId")]
    [InlineData("id")]
    [InlineData("retry-after")]
    [InlineData("titles")]
    public void WithMetadata_NotReservedKey_AddsEntry(string key)
    {
        // Act
        var sut = CreateError().WithMetadata(key, 1);

        // Assert
        sut.Metadata[key].ShouldBe(1);
    }

    [Fact]
    public void WithMetadata_SequenceWithReservedKey_ThrowsArgumentException()
    {
        // Arrange
        var sut = CreateError();

        // Act & Assert
        Should.Throw<ArgumentException>(() =>
            sut.WithMetadata([new KeyValuePair<string, object>("limit", 1), new KeyValuePair<string, object>("Type", 1)]));
    }

    [Fact]
    public void Metadata_InitWithReservedKey_IsAccepted()
    {
        // Act
        var sut = CreateError() with { Metadata = new Dictionary<string, object> { ["title"] = "foreign" } };

        // Assert
        sut.Metadata["title"].ShouldBe("foreign");
    }

    [Fact]
    public void Deserialize_ReservedKey_IsAccepted()
    {
        // Arrange
        const string Json = """{"Category":10,"TypeUri":"about:blank","Title":"Test title","Metadata":{"status":429}}""";

        // Act
        var microsoft = SerializationHelper.DeserializeWithMicrosoft<Sut>(Json);
        var newtonsoft = SerializationHelper.DeserializeWithNewtonsoft<Sut>(Json);

        // Assert
        microsoft.TryGetMetadata<int>("status", out var microsoftStatus).ShouldBeTrue();
        microsoftStatus.ShouldBe(429);
        newtonsoft.TryGetMetadata<int>("status", out var newtonsoftStatus).ShouldBeTrue();
        newtonsoftStatus.ShouldBe(429);
    }

    [Fact]
    public void WithMetadata_NullValue_ThrowsArgumentNullException()
    {
        // Arrange
        var sut = CreateError();

        // Act & Assert
        Should.Throw<ArgumentNullException>(() => sut.WithMetadata("limit", null!));
    }

    [Fact]
    public void WithMetadata_NullSequence_ThrowsArgumentNullException()
    {
        // Arrange
        var sut = CreateError();

        // Act & Assert
        Should.Throw<ArgumentNullException>(() => sut.WithMetadata((IEnumerable<KeyValuePair<string, object>>)null!));
    }

    [Fact]
    public void WithMetadata_SequenceWithWhitespaceKey_ThrowsArgumentException()
    {
        // Arrange
        var sut = CreateError();

        // Act & Assert
        Should.Throw<ArgumentException>(() =>
            sut.WithMetadata([new KeyValuePair<string, object>(" ", 1)]));
    }

    [Fact]
    public void WithMetadata_SequenceWithNullValue_ThrowsArgumentNullException()
    {
        // Arrange
        var sut = CreateError();

        // Act & Assert
        Should.Throw<ArgumentNullException>(() =>
            sut.WithMetadata([new KeyValuePair<string, object>("limit", null!)]));
    }

    #endregion

    #region WithoutMetadata

    [Fact]
    public void WithoutMetadata_ExistingKey_ReturnsCopyWithoutEntryAndLeavesOriginalUntouched()
    {
        // Arrange
        var original = CreateError().WithMetadata("limit", 100).WithMetadata("remaining", 0);

        // Act
        var sut = original.WithoutMetadata("limit");

        // Assert
        sut.Metadata.Count.ShouldBe(1);
        sut.Metadata.ContainsKey("limit").ShouldBeFalse();
        original.Metadata.Count.ShouldBe(2);
    }

    [Fact]
    public void WithoutMetadata_AbsentKey_ReturnsEqualCopy()
    {
        // Arrange
        var original = CreateError().WithMetadata("limit", 100);

        // Act
        var sut = original.WithoutMetadata("missing");

        // Assert
        sut.ShouldBe(original);
        sut.Metadata.Count.ShouldBe(1);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void WithoutMetadata_InvalidKey_ThrowsArgumentException(string? key)
    {
        // Arrange
        var sut = CreateError();

        // Act & Assert
        Should.Throw<ArgumentException>(() => sut.WithoutMetadata(key!));
    }

    #endregion

    #region TryGetMetadata

    [Fact]
    public void TryGetMetadata_ValueOfRequestedType_ReturnsValue()
    {
        // Arrange
        var sut = CreateError().WithMetadata("limit", 100);

        // Act
        var found = sut.TryGetMetadata<int>("limit", out var value);

        // Assert
        found.ShouldBeTrue();
        value.ShouldBe(100);
    }

    [Fact]
    public void TryGetMetadata_ConvertibleValue_ReturnsConvertedValue()
    {
        // Arrange
        var sut = CreateError().WithMetadata("limit", 100L);

        // Act
        var found = sut.TryGetMetadata<int>("limit", out var value);

        // Assert
        found.ShouldBeTrue();
        value.ShouldBe(100);
    }

    [Fact]
    public void TryGetMetadata_NullableTarget_ReturnsConvertedValue()
    {
        // Arrange
        var sut = CreateError().WithMetadata("limit", 100L);

        // Act
        var found = sut.TryGetMetadata<int?>("limit", out var value);

        // Assert
        found.ShouldBeTrue();
        value.ShouldBe(100);
    }

    [Fact]
    public void TryGetMetadata_MissingKey_ReturnsFalse()
    {
        // Arrange
        var sut = CreateError();

        // Act
        var found = sut.TryGetMetadata<int>("limit", out var value);

        // Assert
        found.ShouldBeFalse();
        value.ShouldBe(0);
    }

    [Fact]
    public void TryGetMetadata_NullKey_ReturnsFalse()
    {
        // Arrange
        var sut = CreateError().WithMetadata("limit", 100);

        // Act
        var found = sut.TryGetMetadata<int>(null!, out _);

        // Assert
        found.ShouldBeFalse();
    }

    [Fact]
    public void TryGetMetadata_NotConvertibleValue_ReturnsFalse()
    {
        // Arrange
        var sut = CreateError().WithMetadata("limit", "not a number");

        // Act
        var found = sut.TryGetMetadata<int>("limit", out var value);

        // Assert
        found.ShouldBeFalse();
        value.ShouldBe(0);
    }

    [Fact]
    public void TryGetMetadata_NonConvertibleObject_ReturnsFalse()
    {
        // Arrange
        var sut = CreateError().WithMetadata("limit", new object());

        // Act
        var found = sut.TryGetMetadata<int>("limit", out _);

        // Assert
        found.ShouldBeFalse();
    }

    [Fact]
    public void TryGetMetadata_JsonElementOfWrongKind_ReturnsFalse()
    {
        // Arrange
        var sut = CreateError().WithMetadata("limit", JsonDocument.Parse("\"abc\"").RootElement);

        // Act
        var found = sut.TryGetMetadata<int>("limit", out _);

        // Assert
        found.ShouldBeFalse();
    }

    [Fact]
    public void TryGetMetadata_JsonElementOutOfRange_ReturnsFalse()
    {
        // Arrange
        var sut = CreateError().WithMetadata("limit", JsonDocument.Parse("3000000000").RootElement);

        // Act
        var found = sut.TryGetMetadata<int>("limit", out _);

        // Assert
        found.ShouldBeFalse();
    }

    [Fact]
    public void TryGetMetadata_JsonElementDateTimeOffsetAndGuid_ReturnsConvertedValues()
    {
        // Arrange
        var expectedDate = new DateTimeOffset(2026, 11, 12, 10, 30, 0, TimeSpan.Zero);
        var expectedGuid = Guid.Parse("8f0c6f7e-2d3b-4b8a-9a35-0d4bb1d6f1a2");
        var sut = CreateError()
            .WithMetadata("resetAt", JsonDocument.Parse("\"2026-11-12T10:30:00+00:00\"").RootElement)
            .WithMetadata("requestId", JsonDocument.Parse("\"8f0c6f7e-2d3b-4b8a-9a35-0d4bb1d6f1a2\"").RootElement);

        // Act
        var dateFound = sut.TryGetMetadata<DateTimeOffset>("resetAt", out var date);
        var guidFound = sut.TryGetMetadata<Guid>("requestId", out var guid);

        // Assert
        dateFound.ShouldBeTrue();
        date.ShouldBe(expectedDate);
        guidFound.ShouldBeTrue();
        guid.ShouldBe(expectedGuid);
    }

    [Fact]
    public void TryGetMetadata_JsonElementOfComplexType_ReturnsDeserializedValue()
    {
        // Arrange
        var sut = CreateError().WithMetadata("limits", JsonDocument.Parse("[1,2,3]").RootElement);

        // Act
        var found = sut.TryGetMetadata<int[]>("limits", out var value);

        // Assert
        found.ShouldBeTrue();
        value.ShouldBe([1, 2, 3]);
    }

    #endregion

    #region serialization round-trip

    [Fact]
    public void RoundTrip_MicrosoftSerializer_TryGetMetadataReturnsValues()
    {
        // Arrange
        var original = CreateMetadataError();

        // Act
        var json = SerializationHelper.SerializeWithMicrosoft(original);
        var sut = SerializationHelper.DeserializeWithMicrosoft<Sut>(json);

        // Assert
        AssertMetadata(sut);
    }

    [Fact]
    public void RoundTrip_NewtonsoftSerializer_TryGetMetadataReturnsValues()
    {
        // Arrange
        var original = CreateMetadataError();

        // Act
        var json = SerializationHelper.SerializeWithNewtonsoft(original);
        var sut = SerializationHelper.DeserializeWithNewtonsoft<Sut>(json);

        // Assert
        AssertMetadata(sut);
    }

    [Theory]
    [InlineData("""{"Category":10,"TypeUri":"about:blank","Title":"Test title","Metadata":null}""")]
    [InlineData("""{"Category":10,"TypeUri":"about:blank","Title":"Test title"}""")]
    public void Deserialize_MicrosoftSerializerWithNullOrMissingMetadata_ReturnsEmptyMetadata(string json)
    {
        // Act
        var sut = SerializationHelper.DeserializeWithMicrosoft<Sut>(json);

        // Assert
        sut.Category.ShouldBe(ErrorCategory.LimitExceeded);
        sut.Metadata.ShouldNotBeNull();
        sut.Metadata.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("""{"Category":10,"TypeUri":"about:blank","Title":"Test title","Metadata":null}""")]
    [InlineData("""{"Category":10,"TypeUri":"about:blank","Title":"Test title"}""")]
    public void Deserialize_NewtonsoftSerializerWithNullOrMissingMetadata_ReturnsEmptyMetadata(string json)
    {
        // Act
        var sut = SerializationHelper.DeserializeWithNewtonsoft<Sut>(json);

        // Assert
        sut.Category.ShouldBe(ErrorCategory.LimitExceeded);
        sut.Metadata.ShouldNotBeNull();
        sut.Metadata.ShouldBeEmpty();
    }

    #endregion

    #region helper methods

    private static Sut CreateError()
    {
        return Sut.LimitExceeded(
            ErrorUri.Tag("tag:test.com,2024:test"),
            "Test title",
            "Test detail");
    }

    private static Sut CreateMetadataError()
    {
        return CreateError()
            .WithMetadata("limit", 100)
            .WithMetadata("bigNumber", 5_000_000_000L)
            .WithMetadata("policy", "monthly")
            .WithMetadata("upgradable", true);
    }

    private static void AssertMetadata(Sut sut)
    {
        sut.Metadata.Count.ShouldBe(4);

        sut.TryGetMetadata<int>("limit", out var limit).ShouldBeTrue();
        limit.ShouldBe(100);

        sut.TryGetMetadata<long>("bigNumber", out var bigNumber).ShouldBeTrue();
        bigNumber.ShouldBe(5_000_000_000L);

        sut.TryGetMetadata<string>("policy", out var policy).ShouldBeTrue();
        policy.ShouldBe("monthly");

        sut.TryGetMetadata<bool>("upgradable", out var upgradable).ShouldBeTrue();
        upgradable.ShouldBeTrue();
    }

    #endregion
}
