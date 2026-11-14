// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Result.Validators;
using System;

namespace Maple.Result.Tests.Unit;

public class MetadataKeyValidatorUnitTests
{
    [Theory]
    [InlineData("type")]
    [InlineData("title")]
    [InlineData("status")]
    [InlineData("detail")]
    [InlineData("instance")]
    [InlineData("errors")]
    [InlineData("detailTemplated")]
    [InlineData("TYPE")]
    [InlineData("Instance")]
    [InlineData("detailtemplated")]
    public void IsReserved_StandardMemberName_ReturnsTrue(string key)
    {
        // Act
        var result = MetadataKeyValidator.IsReserved(key);

        // Assert
        result.ShouldBeTrue();
    }

    [Theory]
    [InlineData("metadata")]
    [InlineData("traceId")]
    [InlineData("limit")]
    [InlineData("detailTemplatedX")]
    [InlineData("typ")]
    public void IsReserved_OtherName_ReturnsFalse(string key)
    {
        // Act
        var result = MetadataKeyValidator.IsReserved(key);

        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public void Validate_ValidKey_DoesNotThrow()
    {
        // Act & Assert
        Should.NotThrow(() => MetadataKeyValidator.Validate("retryAfterSeconds"));
    }

    [Fact]
    public void Validate_ReservedKey_ThrowsArgumentExceptionWithQuotedKey()
    {
        // Act
        var exception = Should.Throw<ArgumentException>(() => MetadataKeyValidator.Validate("Title"));

        // Assert
        exception.Message.ShouldStartWith("The metadata key ‘Title’ is reserved.");
        exception.Message.ShouldContain("‘detailTemplated’");
    }

    [Fact]
    public void Validate_NullKey_ThrowsArgumentNullException()
    {
        // Act & Assert
        Should.Throw<ArgumentNullException>(() => MetadataKeyValidator.Validate(null!));
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData("errors")]
    [InlineData("Errors")]
    public void Validate_InvalidKey_ThrowsArgumentException(string key)
    {
        // Act & Assert
        Should.Throw<ArgumentException>(() => MetadataKeyValidator.Validate(key));
    }
}
