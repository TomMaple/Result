// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System;
using Maple.Result.Extensions;

namespace Maple.Result.Tests.Unit.Extensions;

public class ResultExtensionsUnitTests
{
    [Fact]
    public void ToResult_NoGenericResult_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.ToResult());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void ToResult_SuccessfulGenericResult_ReturnsSuccessfulResult()
    {
        // Arrange
        const int InitialValue = 362;

        Result<int> sut = InitialValue;

        // Act
        var result = sut.ToResult();

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.ShouldBeOfType<Result>();
    }

    [Fact]
    public void ToResult_SuccessfulGenericResult_DoesNotModifyGenericResult()
    {
        // Arrange
        const int InitialValue = 362;

        Result<int> sut = InitialValue;

        // Act
        _ = sut.ToResult();

        // Assert
        sut.ShouldNotBeNull();
        sut.IsSuccess().ShouldBeTrue();
        sut.Value.ShouldBe(InitialValue);
    }

    [Fact]
    public void ToResult_ErrorGenericResult_ReturnsErrorResultWithOriginalError()
    {
        // Arrange
        var error = GetError();

        Result<int> sut = error;

        // Act
        var result = sut.ToResult();

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(error);
    }

    [Fact]
    public void ToResult_ErrorGenericResult_DoesNotModifyGenericResult()
    {
        // Arrange
        var error = GetError();

        Result<int> sut = error;

        // Act
        _ = sut.ToResult();

        // Assert
        sut.ShouldNotBeNull();
        sut.IsSuccess().ShouldBeFalse();
        sut.Error.ShouldBe(error);
    }

    #region helper methods

    private static Error GetError()
    {
        return Error.Failure(ErrorUri.None(), "4b3612e7-d22a-45f2-9a5c-4a8c14d39141", "Error title 1");
    }

    #endregion
}
