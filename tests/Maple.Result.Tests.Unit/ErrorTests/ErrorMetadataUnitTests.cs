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

namespace Maple.Result.Tests.Unit.ErrorTests;

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
    public void Metadata_SequenceSourceChangedLater_IsNotAffected()
    {
        // Arrange
        var source = new Dictionary<string, object?> { ["limit"] = 100 };

        // Act
        var sut = CreateError().WithMetadata(source);
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
    public void Metadata_ToBuilderWithoutMetadataChange_SharesReadOnlyDictionary()
    {
        // Arrange
        var original = CreateError().WithMetadata("limit", 100);

        // Act
        var copy = original.ToBuilder().WithTitle("Changed title").Build();

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
        sut.Metadata.ShouldNotBeAssignableTo<Dictionary<string, object?>>();
        var dictionary = sut.Metadata.ShouldBeAssignableTo<IDictionary<string, object?>>();
        dictionary.ShouldNotBeNull();
        dictionary.IsReadOnly.ShouldBeTrue();
        Should.Throw<NotSupportedException>(() => dictionary["limit"] = 200);
        sut.Metadata["limit"].ShouldBe(100);
    }

    [Fact]
    public void Metadata_SerializedWithMicrosoftSerializer_WritesAllEntries()
    {
        // Arrange
        var sut = CreateError().WithMetadata("zeta", 1).WithMetadata("alpha", 2);

        // Act
        var result = SerializationHelper.SerializeWithMicrosoft(sut);

        // Assert
        result.ShouldContain("\"Metadata\":{");
        result.ShouldContain("\"zeta\":1");
        result.ShouldContain("\"alpha\":2");
    }

    [Fact]
    public void Metadata_SerializedWithNewtonsoftSerializer_WritesAllEntries()
    {
        // Arrange
        var sut = CreateError().WithMetadata("zeta", 1).WithMetadata("alpha", 2);

        // Act
        var result = SerializationHelper.SerializeWithNewtonsoft(sut);

        // Assert
        result.ShouldContain("\"Metadata\":{");
        result.ShouldContain("\"zeta\":1");
        result.ShouldContain("\"alpha\":2");
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
    public void WithMetadata_NullValue_AddsEntryWithNullValue()
    {
        // Act
        var sut = CreateError().WithMetadata("traceId", null);

        // Assert
        sut.Metadata.Count.ShouldBe(1);
        sut.Metadata.ShouldContainKey("traceId");
        sut.Metadata["traceId"].ShouldBeNull();
    }

    [Fact]
    public void WithMetadata_Sequence_AddsAndReplacesEntriesAndKeepsOthers()
    {
        // Arrange
        var original = CreateError().WithMetadata("limit", 100).WithMetadata("remaining", 5);

        // Act
        var sut = original.WithMetadata(new Dictionary<string, object?> { ["remaining"] = 0, ["resetAt"] = "tomorrow" });

        // Assert
        sut.Metadata.Count.ShouldBe(3);
        sut.Metadata["limit"].ShouldBe(100);
        sut.Metadata["remaining"].ShouldBe(0);
        sut.Metadata["resetAt"].ShouldBe("tomorrow");
        original.Metadata.Count.ShouldBe(2);
        original.Metadata["remaining"].ShouldBe(5);
    }

    [Fact]
    public void WithMetadata_SequenceWithNullValue_AddsEntryWithNullValue()
    {
        // Act
        var sut = CreateError().WithMetadata([new KeyValuePair<string, object?>("limit", null)]);

        // Assert
        sut.Metadata.ShouldContainKey("limit");
        sut.Metadata["limit"].ShouldBeNull();
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
    public void WithMetadata_NullKey_ThrowsArgumentNullException()
    {
        // Arrange
        var sut = CreateError();

        // Act & Assert
        Should.Throw<ArgumentNullException>(() => sut.WithMetadata(null!, 1));
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
            sut.WithMetadata([new KeyValuePair<string, object?>("limit", 1), new KeyValuePair<string, object?>("Type", 1)]));
    }

    [Fact]
    public void WithMetadata_NullSequence_ThrowsArgumentNullException()
    {
        // Arrange
        var sut = CreateError();

        // Act & Assert
        Should.Throw<ArgumentNullException>(() => sut.WithMetadata((IEnumerable<KeyValuePair<string, object?>>)null!));
    }

    [Fact]
    public void WithMetadata_SequenceWithWhitespaceKey_ThrowsArgumentException()
    {
        // Arrange
        var sut = CreateError();

        // Act & Assert
        Should.Throw<ArgumentException>(() =>
            sut.WithMetadata([new KeyValuePair<string, object?>(" ", 1)]));
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

    [Fact]
    public void WithoutMetadata_NoKey_ReturnsCopyWithoutEntriesAndLeavesOriginalUntouched()
    {
        // Arrange
        var original = CreateError().WithMetadata("limit", 100).WithMetadata("remaining", 0);

        // Act
        var sut = original.WithoutMetadata();

        // Assert
        sut.Metadata.ShouldBeEmpty();
        sut.Title.ShouldBe(original.Title);
        original.Metadata.Count.ShouldBe(2);
    }

    [Fact]
    public void WithoutMetadata_NoKeyAndNoEntries_ReturnsEqualCopy()
    {
        // Arrange
        var original = CreateError();

        // Act
        var sut = original.WithoutMetadata();

        // Assert
        sut.ShouldBe(original);
        sut.Metadata.ShouldBeEmpty();
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
    public void TryGetMetadata_NullableTarget_ReturnsValue()
    {
        // Arrange
        var sut = CreateError().WithMetadata("limit", 100);

        // Act
        var found = sut.TryGetMetadata<int?>("limit", out var value);

        // Assert
        found.ShouldBeTrue();
        value.ShouldBe(100);
    }

    [Fact]
    public void TryGetMetadata_BaseTypeOrInterface_ReturnsValue()
    {
        // Arrange
        var sut = CreateError().WithMetadata("policy", "monthly");

        // Act
        var objectFound = sut.TryGetMetadata<object>("policy", out var asObject);
        var interfaceFound = sut.TryGetMetadata<IComparable>("policy", out var asComparable);

        // Assert
        objectFound.ShouldBeTrue();
        asObject.ShouldBe("monthly");
        interfaceFound.ShouldBeTrue();
        asComparable.ShouldBe("monthly");
    }

    [Fact]
    public void TryGetMetadata_IntegerReadAsLong_ReturnsFalse()
    {
        // Arrange
        var sut = CreateError().WithMetadata("limit", 100);

        // Act
        var found = sut.TryGetMetadata<long>("limit", out var value);

        // Assert
        found.ShouldBeFalse();
        value.ShouldBe(0L);
    }

    [Fact]
    public void TryGetMetadata_LongReadAsInteger_ReturnsFalse()
    {
        // Arrange
        var sut = CreateError().WithMetadata("limit", 100L);

        // Act
        var found = sut.TryGetMetadata<int>("limit", out _);

        // Assert
        found.ShouldBeFalse();
    }

    [Fact]
    public void TryGetMetadata_StringReadAsInteger_ReturnsFalse()
    {
        // Arrange
        var sut = CreateError().WithMetadata("limit", "100");

        // Act
        var found = sut.TryGetMetadata<int>("limit", out var value);

        // Assert
        found.ShouldBeFalse();
        value.ShouldBe(0);
    }

    [Fact]
    public void TryGetMetadata_EnumReadAsInteger_ReturnsFalse()
    {
        // Arrange
        var sut = CreateError().WithMetadata("level", TestLevel.High);

        // Act
        var found = sut.TryGetMetadata<int>("level", out _);

        // Assert
        found.ShouldBeFalse();
    }

    [Fact]
    public void TryGetMetadata_IntegerReadAsEnum_ReturnsFalse()
    {
        // Arrange
        var sut = CreateError().WithMetadata("level", 2);

        // Act
        var found = sut.TryGetMetadata<TestLevel>("level", out _);

        // Assert
        found.ShouldBeFalse();
    }

    [Fact]
    public void TryGetMetadata_EnumReadAsEnum_ReturnsValue()
    {
        // Arrange
        var sut = CreateError().WithMetadata("level", TestLevel.High);

        // Act
        var found = sut.TryGetMetadata<TestLevel>("level", out var value);

        // Assert
        found.ShouldBeTrue();
        value.ShouldBe(TestLevel.High);
    }

    [Fact]
    public void TryGetMetadata_NullValue_ReturnsFalse()
    {
        // Arrange
        var sut = CreateError().WithMetadata("traceId", null);

        // Act
        var stringFound = sut.TryGetMetadata<string?>("traceId", out var asString);
        var objectFound = sut.TryGetMetadata<object>("traceId", out var asObject);
        var nullableFound = sut.TryGetMetadata<int?>("traceId", out var asNullable);

        // Assert
        stringFound.ShouldBeFalse();
        asString.ShouldBeNull();
        objectFound.ShouldBeFalse();
        asObject.ShouldBeNull();
        nullableFound.ShouldBeFalse();
        asNullable.ShouldBeNull();
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
    public void TryGetMetadata_ObjectOfOtherType_ReturnsFalse()
    {
        // Arrange
        var sut = CreateError().WithMetadata("limit", new object());

        // Act
        var found = sut.TryGetMetadata<int>("limit", out _);

        // Assert
        found.ShouldBeFalse();
    }

    #endregion

    #region helper methods

    private enum TestLevel
    {
        Low = 1,
        High = 2
    }

    private static Sut CreateError()
    {
        return Sut.LimitExceeded(
            ErrorUri.Tag("tag:test.com,2024:test"),
            "Test title",
            "Test detail");
    }

    #endregion
}
