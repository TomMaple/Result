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
using System.Threading.Tasks;
using Maple.Result.Extensions;

namespace Maple.Result.Tests.Unit.Extensions;

public class ResultTaskAsyncExtensionsUnitTests
{
    [Fact]
    public async Task ToResult_NoGenericResultTask_ThrowsException()
    {
        // Arrange
        const Task<Result<int>>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.ToResultAsync());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task ToResult_NullGenericResultTask_ThrowsException()
    {
        // Arrange
        var sut = Task.FromResult<Result<int>?>(null);

        // Act
        var exception = await Record.ExceptionAsync(() => sut!.ToResultAsync());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidOperationException>();
        exception.Message.ShouldStartWith("The asynchronous operation returned null.");
    }

    [Fact]
    public async Task ToResult_SuccessfulGenericResultTask_ReturnsSuccessfulResult()
    {
        // Arrange
        const int InitialValue = 362;

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        var result = await sut.ToResultAsync();

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.ShouldBeOfType<Result>();
    }

    [Fact]
    public async Task ToResult_ErrorGenericResultTask_ReturnsErrorResultWithOriginalError()
    {
        // Arrange
        var error = GetError();

        var sut = Task.FromResult(Result<int>.FromError(error));

        // Act
        var result = await sut.ToResultAsync();

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(error);
    }

    #region helper methods

    private static Error GetError()
    {
        return Error.Failure(ErrorUri.None(), "4b3612e7-d22a-45f2-9a5c-4a8c14d39141", "Error title 1");
    }

    #endregion
}