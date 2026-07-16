// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System.Collections.Generic;
using Sut = Maple.Result.Error;

namespace Maple.Result.Tests.Unit;

public class ErrorEqualityUnitTests
{
    private static Sut CreateError()
    {
        return Sut.Validation(
            ErrorUri.Tag("tag:test.com,2024:test"),
            "Test title",
            "Test detail");
    }

    #region value equality

    [Fact]
    public void Equals_TwoIdenticallyBuiltErrorsWithoutDetails_AreEqual()
    {
        // Arrange
        var first = CreateError();
        var second = CreateError();

        // Act & Assert
        first.Equals(second).ShouldBeTrue();
        (first == second).ShouldBeTrue();
        first.GetHashCode().ShouldBe(second.GetHashCode());
    }

    [Fact]
    public void Equals_TwoIdenticallyBuiltErrorsWithDetails_AreEqual()
    {
        // Arrange
        var first = CreateError()
            .AddDetail("#/first", "First detail")
            .AddDetail("#/second", "Second detail");
        var second = CreateError()
            .AddDetail("#/first", "First detail")
            .AddDetail("#/second", "Second detail");

        // Act & Assert
        first.Equals(second).ShouldBeTrue();
        (first == second).ShouldBeTrue();
        first.GetHashCode().ShouldBe(second.GetHashCode());
    }

    [Fact]
    public void Equals_ErrorsWithDifferentDetailContent_AreNotEqual()
    {
        // Arrange
        var first = CreateError().AddDetail("#/property", "First detail");
        var second = CreateError().AddDetail("#/property", "Different detail");

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
    }

    [Fact]
    public void Equals_ErrorsWithDifferentNumberOfDetails_AreNotEqual()
    {
        // Arrange
        var first = CreateError().AddDetail("#/property", "Detail");
        var second = CreateError()
            .AddDetail("#/property", "Detail")
            .AddDetail("#/other", "Detail");

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
    }

    [Fact]
    public void Equals_ErrorsWithDifferentScalarProperties_AreNotEqual()
    {
        // Arrange
        var first = Sut.Validation(ErrorUri.Tag("tag:test.com,2024:test"), "Title");
        var second = Sut.Validation(ErrorUri.Tag("tag:test.com,2024:test"), "Different title");

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
    }

    #endregion

    #region with-expression aliasing

    [Fact]
    public void With_AddDetailOnCopy_DoesNotMutateOriginal()
    {
        // Arrange
        var original = CreateError().AddDetail("#/original", "Original detail");

        // Act
        var copy = original with { Title = "Changed title" };
        copy.AddDetail("#/leaked", "Should not leak into the original");

        // Assert
        original.ErrorDetails.Count.ShouldBe(1);
        copy.ErrorDetails.Count.ShouldBe(2);
    }

    [Fact]
    public void With_AddDetailOnOriginal_DoesNotMutateCopy()
    {
        // Arrange
        var original = CreateError().AddDetail("#/original", "Original detail");

        // Act
        var copy = original with { Title = "Changed title" };
        original.AddDetail("#/leaked", "Should not leak into the copy");

        // Assert
        original.ErrorDetails.Count.ShouldBe(2);
        copy.ErrorDetails.Count.ShouldBe(1);
    }

    [Fact]
    public void With_CopyPreservesExistingDetails_AreEqualUntilMutated()
    {
        // Arrange
        var original = CreateError().AddDetail("#/original", "Original detail");

        // Act
        var copy = original with { };

        // Assert
        copy.ErrorDetails.Count.ShouldBe(1);
        (copy == original).ShouldBeTrue();
    }

    #endregion

    #region read-only collection

    [Fact]
    public void ErrorDetails_ExposedAsReadOnlyList_ReflectsAddDetail()
    {
        // Arrange
        var error = CreateError();

        // Act
        error.AddDetail("#/property", "Detail");

        // Assert
        error.ErrorDetails.ShouldBeAssignableTo<IReadOnlyList<ErrorDetail>>();
        error.ErrorDetails.Count.ShouldBe(1);
    }

    #endregion
}
