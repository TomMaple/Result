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

namespace Maple.Result.Tests.Unit;

public class ErrorDetailUnitTests
{
    #region ctor

    [Fact]
    public void Ctor_AllMembers_SetsThem()
    {
        // Arrange
        var detailTemplated = new TemplatedMessage("message-id");

        // Act
        var sut = new ErrorDetail("#/name", "The name is required.", detailTemplated);

        // Assert
        sut.PropertyPointer.ShouldBe("#/name");
        sut.Detail.ShouldBe("The name is required.");
        sut.DetailTemplated.ShouldBeSameAs(detailTemplated);
    }

    [Fact]
    public void Ctor_NullDetail_ThrowsArgumentNullException()
    {
        // Act
        var exception = Record.Exception(() => new ErrorDetail("#/name", null!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        ((ArgumentNullException)exception).ParamName.ShouldBe("Detail");
    }

    [Fact]
    public void Ctor_NullPropertyPointerAndTemplate_IsAllowed()
    {
        // Act
        var sut = new ErrorDetail(null, "The request is invalid.");

        // Assert
        sut.PropertyPointer.ShouldBeNull();
        sut.DetailTemplated.ShouldBeNull();
    }

    [Fact]
    public void Deconstruct_Always_ReturnsAllMembers()
    {
        // Arrange
        var sut = new ErrorDetail("#/name", "The name is required.");

        // Act
        var (propertyPointer, detail, detailTemplated) = sut;

        // Assert
        propertyPointer.ShouldBe("#/name");
        detail.ShouldBe("The name is required.");
        detailTemplated.ShouldBeNull();
    }

    #endregion
}
