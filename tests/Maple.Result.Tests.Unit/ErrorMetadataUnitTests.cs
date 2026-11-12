// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Json.ObjectAsPrimitiveConverter;
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
    public void Metadata_WithExpression_SharesReadOnlyDictionary()
    {
        // Arrange
        var original = CreateError().WithMetadata("limit", 100);

        // Act
        var copy = original with { Title = "Changed title" };

        // Assert
        copy.Metadata.ShouldBeSameAs(original.Metadata);
        copy.Metadata["limit"].ShouldBe(100);
    }

    [Fact]
    public void Metadata_CastToMutableDictionary_CannotBeModified()
    {
        // Arrange
        var sut = CreateError().WithMetadata("limit", 100);

        // Act & Assert
        sut.Metadata.ShouldNotBeAssignableTo<Dictionary<string, object>>();
        var dictionary = sut.Metadata.ShouldBeAssignableTo<IDictionary<string, object>>();
        dictionary.ShouldNotBeNull();
        dictionary.IsReadOnly.ShouldBeTrue();
        Should.Throw<NotSupportedException>(() => dictionary["limit"] = 200);
        sut.Metadata["limit"].ShouldBe(100);
    }

    [Fact]
    public void Metadata_Entries_KeepInsertionOrder()
    {
        // Act
        var sut = CreateError()
            .WithMetadata("zeta", 1)
            .WithMetadata("alpha", 2)
            .WithMetadata("middle", 3)
            .WithMetadata("zeta", 4);

        // Assert
        sut.Metadata.Keys.ShouldBe(["zeta", "alpha", "middle"]);
        sut.Metadata["zeta"].ShouldBe(4);
    }

    [Fact]
    public void Metadata_Serialized_KeepsInsertionOrder()
    {
        // Arrange
        var sut = CreateError().WithMetadata("zeta", 1).WithMetadata("alpha", 2);

        // Act
        var microsoft = SerializationHelper.SerializeWithMicrosoft(sut);
        var newtonsoft = SerializationHelper.SerializeWithNewtonsoft(sut);

        // Assert
        microsoft.ShouldEndWith("""
                                "Metadata":{"zeta":1,"alpha":2}}
                                """);
        newtonsoft.ShouldEndWith("""
                                 "Metadata":{"zeta":1,"alpha":2}}
                                 """);
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

    [Theory]
    [InlineData("title")]
    [InlineData("Status")]
    [InlineData(" ")]
    public void Metadata_InitWithInvalidKey_ThrowsArgumentException(string key)
    {
        // Arrange
        var sut = CreateError();

        // Act & Assert
        Should.Throw<ArgumentException>(() =>
            sut with { Metadata = new Dictionary<string, object> { ["limit"] = 100, [key] = "foreign" } });
    }

    [Fact]
    public void Metadata_InitWithNullValue_ThrowsArgumentNullException()
    {
        // Arrange
        var sut = CreateError();

        // Act
        var exception = Should.Throw<ArgumentNullException>(() =>
            sut with { Metadata = new Dictionary<string, object> { ["limit"] = null! } });

        // Assert
        exception.Message.ShouldContain("‘limit’");
    }

    [Fact]
    public void Deserialize_MicrosoftSerializerWithReservedKey_ThrowsArgumentException()
    {
        // Arrange
        const string Json = """{"Category":10,"TypeUri":"about:blank","Title":"Test title","Metadata":{"status":429}}""";

        // Act & Assert
        Should.Throw<ArgumentException>(() => SerializationHelper.DeserializeWithMicrosoft<Sut>(Json));
    }

    [Fact]
    public void Deserialize_NewtonsoftSerializerWithReservedKey_ThrowsWrappedArgumentException()
    {
        // Arrange
        const string Json = """{"Category":10,"TypeUri":"about:blank","Title":"Test title","Metadata":{"status":429}}""";

        // Act & Assert
        Should.Throw<Newtonsoft.Json.JsonSerializationException>(() => SerializationHelper.DeserializeWithNewtonsoft<Sut>(Json))
            .GetBaseException().ShouldBeOfType<ArgumentException>();
    }

    [Fact]
    public void Deserialize_MicrosoftSerializerWithNullValue_ThrowsArgumentNullException()
    {
        // Arrange
        const string Json = """{"Category":10,"TypeUri":"about:blank","Title":"Test title","Metadata":{"limit":null}}""";

        // Act & Assert
        Should.Throw<ArgumentNullException>(() => SerializationHelper.DeserializeWithMicrosoft<Sut>(Json));
    }

    [Fact]
    public void Deserialize_NewtonsoftSerializerWithNullValue_ThrowsWrappedArgumentNullException()
    {
        // Arrange
        const string Json = """{"Category":10,"TypeUri":"about:blank","Title":"Test title","Metadata":{"limit":null}}""";

        // Act & Assert
        Should.Throw<Newtonsoft.Json.JsonSerializationException>(() => SerializationHelper.DeserializeWithNewtonsoft<Sut>(Json))
            .GetBaseException().ShouldBeOfType<ArgumentNullException>();
    }

    [Fact]
    public void WithMetadata_MetadataOfDeserializedError_CopiesEntries()
    {
        // Arrange
        const string Json = """{"Category":10,"TypeUri":"about:blank","Title":"Test title","Metadata":{"limit":100}}""";
        var source = SerializationHelper.DeserializeWithMicrosoft<Sut>(Json);

        // Act
        var sut = CreateError().WithMetadata(source.Metadata);

        // Assert
        sut.TryGetMetadata<int>("limit", out var limit).ShouldBeTrue();
        limit.ShouldBe(100);
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

    [Fact]
    public void TryGetMetadata_EnumValueAsInteger_ReturnsNumber()
    {
        // Arrange
        var sut = CreateError().WithMetadata("level", TestLevel.High);

        // Act
        var found = sut.TryGetMetadata<int>("level", out var value);

        // Assert
        found.ShouldBeTrue();
        value.ShouldBe(2);
    }

    [Fact]
    public void TryGetMetadata_IntegerOutOfEnumUnderlyingRange_ReturnsFalse()
    {
        // Arrange
        var sut = CreateError().WithMetadata("level", 300);

        // Act
        var found = sut.TryGetMetadata<TestByteLevel>("level", out _);

        // Assert
        found.ShouldBeFalse();
    }

    [Fact]
    public void TryGetMetadata_NotFiniteDouble_ReturnsFalse()
    {
        // Arrange
        var sut = CreateError().WithMetadata("ratio", double.NaN);

        // Act
        var found = sut.TryGetMetadata<decimal>("ratio", out _);

        // Assert
        found.ShouldBeFalse();
    }

    #endregion

    #region TryGetMetadata consistency across transports

    [Fact]
    public void TryGetMetadata_DateStringAsDateTimeOffset_ReturnsSameValueForAllTransports()
    {
        AssertSameResultForAllTransports("2026-12-01T00:00:00Z", true,
            new DateTimeOffset(2026, 12, 1, 0, 0, 0, TimeSpan.Zero));
    }

    [Fact]
    public void TryGetMetadata_DateStringAsDateTime_ReturnsSameValueForAllTransports()
    {
        AssertSameResultForAllTransports("2026-12-01T00:00:00Z", true,
            new DateTime(2026, 12, 1, 0, 0, 0, DateTimeKind.Utc));
    }

    [Fact]
    public void TryGetMetadata_DateStringAsString_ReturnsOriginalStringForAllTransports()
    {
        AssertSameResultForAllTransports("2026-12-01T00:00:00Z", true, "2026-12-01T00:00:00Z");
    }

    [Fact]
    public void TryGetMetadata_DateTimeOffsetValueAsDateTimeOffset_ReturnsSameInstantForAllTransports()
    {
        var value = new DateTimeOffset(2026, 12, 1, 10, 30, 0, TimeSpan.FromHours(2));

        AssertSameResultForAllTransports(value, true, value);
    }

    [Fact]
    public void TryGetMetadata_GuidStringAsGuid_ReturnsSameValueForAllTransports()
    {
        AssertSameResultForAllTransports("8f0c6f7e-2d3b-4b8a-9a35-0d4bb1d6f1a2", true,
            Guid.Parse("8f0c6f7e-2d3b-4b8a-9a35-0d4bb1d6f1a2"));
    }

    [Fact]
    public void TryGetMetadata_GuidValueAsString_ReturnsSameValueForAllTransports()
    {
        AssertSameResultForAllTransports(Guid.Parse("8f0c6f7e-2d3b-4b8a-9a35-0d4bb1d6f1a2"), true,
            "8f0c6f7e-2d3b-4b8a-9a35-0d4bb1d6f1a2");
    }

    [Fact]
    public void TryGetMetadata_FractionalNumberAsInteger_ReturnsFalseForAllTransports()
    {
        AssertSameResultForAllTransports(1.5, false, 0);
    }

    [Fact]
    public void TryGetMetadata_FractionalNumberAsDecimal_ReturnsSameValueForAllTransports()
    {
        AssertSameResultForAllTransports(1.5, true, 1.5m);
    }

    [Fact]
    public void TryGetMetadata_NumberAsString_ReturnsFalseForAllTransports()
    {
        AssertSameResultForAllTransports(100L, false, (string?)null);
    }

    [Fact]
    public void TryGetMetadata_BooleanStringAsBoolean_ReturnsFalseForAllTransports()
    {
        AssertSameResultForAllTransports("true", false, false);
    }

    [Fact]
    public void TryGetMetadata_NumericStringAsInteger_ReturnsFalseForAllTransports()
    {
        AssertSameResultForAllTransports("100", false, 0);
    }

    [Fact]
    public void TryGetMetadata_EnumValueAsEnum_ReturnsSameValueForAllTransports()
    {
        AssertSameResultForAllTransports(TestLevel.High, true, TestLevel.High);
    }

    [Fact]
    public void TryGetMetadata_IntegerAsEnum_ReturnsSameValueForAllTransports()
    {
        AssertSameResultForAllTransports(2L, true, TestLevel.High);
    }

    [Fact]
    public void TryGetMetadata_IntegerAsNullableInteger_ReturnsSameValueForAllTransports()
    {
        AssertSameResultForAllTransports(100L, true, (int?)100);
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

    [Fact]
    public void RoundTrip_MicrosoftSerializerWithObjectAsPrimitiveConverter_ReturnsPrimitivesAndEqualError()
    {
        // Arrange
        var options = new JsonSerializerOptions();
        options.Converters.Add(new ObjectAsPrimitiveConverter());

        var original = CreateError()
            .WithMetadata("limit", 100)
            .WithMetadata("bigNumber", 5_000_000_000L)
            .WithMetadata("policy", "monthly")
            .WithMetadata("upgradable", true);

        // Act
        var json = JsonSerializer.Serialize(original, options);
        var sut = JsonSerializer.Deserialize<Sut>(json, options)!;

        // Assert
        sut.Metadata["limit"].ShouldBeOfType<int>();
        sut.Metadata["bigNumber"].ShouldBeOfType<long>();
        sut.Metadata["policy"].ShouldBeOfType<string>();
        sut.Metadata["upgradable"].ShouldBeOfType<bool>();
        sut.ShouldBe(original);
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

    private enum TestLevel
    {
        Low = 1,
        High = 2
    }

    private enum TestByteLevel : byte
    {
        Low = 1
    }

    /// <summary>
    ///     Asserts that reading the stored value gives the same result in memory and after a round-trip through
    ///     System.Text.Json and Newtonsoft.Json.
    /// </summary>
    private static void AssertSameResultForAllTransports<T>(object stored, bool expectedFound, T expectedValue)
    {
        var original = CreateError().WithMetadata("value", stored);
        var microsoft = SerializationHelper.DeserializeWithMicrosoft<Sut>(
            SerializationHelper.SerializeWithMicrosoft(original));
        var newtonsoft = SerializationHelper.DeserializeWithNewtonsoft<Sut>(
            SerializationHelper.SerializeWithNewtonsoft(original));

        foreach (var (transport, sut) in new[]
                 {
                     ("in memory", original), ("System.Text.Json", microsoft), ("Newtonsoft.Json", newtonsoft)
                 })
        {
            var found = sut.TryGetMetadata<T>("value", out var value);

            found.ShouldBe(expectedFound, transport);
            if (expectedFound)
                value.ShouldBe(expectedValue, transport);
        }
    }

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
