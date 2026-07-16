// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

namespace Maple.Result.Tests.Unit;

public class ResultEqualityUnitTests
{
    #region Result equality

    [Fact]
    public void Equals_TwoSuccessfulResults_AreEqual()
    {
        // Arrange
        var first = Result.Success();
        var second = Result.Success();

        // Act & Assert
        first.Equals(second).ShouldBeTrue();
        (first == second).ShouldBeTrue();
        first.GetHashCode().ShouldBe(second.GetHashCode());
    }

    [Fact]
    public void Equals_TwoFailedResultsWithEqualErrors_AreEqual()
    {
        // Arrange
        var first = Result.FromError(CreateError());
        var second = Result.FromError(CreateError());

        // Act & Assert
        first.Equals(second).ShouldBeTrue();
        (first == second).ShouldBeTrue();
        first.GetHashCode().ShouldBe(second.GetHashCode());
    }

    [Fact]
    public void Equals_TwoFailedResultsWithEqualErrorsHavingTemplatedDetails_AreEqual()
    {
        // Arrange
        var first = Result.FromError(
            CreateError().AddDetail("#/email", "The email is required.", "errors.email.required", ("minLength", 3)));
        var second = Result.FromError(
            CreateError().AddDetail("#/email", "The email is required.", "errors.email.required", ("minLength", 3)));

        // Act & Assert
        first.Equals(second).ShouldBeTrue();
        (first == second).ShouldBeTrue();
        first.GetHashCode().ShouldBe(second.GetHashCode());
    }

    [Fact]
    public void Equals_FailedResultsWithErrorsHavingDifferentTemplatedDetailParams_AreNotEqual()
    {
        // Arrange
        var first = Result.FromError(
            CreateError().AddDetail("#/email", "The email is required.", "errors.email.required", ("minLength", 3)));
        var second = Result.FromError(
            CreateError().AddDetail("#/email", "The email is required.", "errors.email.required", ("minLength", 5)));

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
    }

    [Fact]
    public void Equals_FailedResultsWithDifferentErrors_AreNotEqual()
    {
        // Arrange
        var first = Result.FromError(CreateError("First title"));
        var second = Result.FromError(CreateError("Second title"));

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
    }

    [Fact]
    public void Equals_SuccessfulAndFailedResult_AreNotEqual()
    {
        // Arrange
        var success = Result.Success();
        var failure = Result.FromError(CreateError());

        // Act & Assert
        success.Equals(failure).ShouldBeFalse();
        (success == failure).ShouldBeFalse();
    }

    #endregion

    #region Result<T> equality

    [Fact]
    public void Equals_TwoSuccessfulResultsWithEqualValues_AreEqual()
    {
        // Arrange
        var first = Result.FromValue(42);
        var second = Result.FromValue(42);

        // Act & Assert
        first.Equals(second).ShouldBeTrue();
        (first == second).ShouldBeTrue();
        first.GetHashCode().ShouldBe(second.GetHashCode());
    }

    [Fact]
    public void Equals_SuccessfulResultsWithDifferentValues_AreNotEqual()
    {
        // Arrange
        var first = Result.FromValue(42);
        var second = Result.FromValue(43);

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
    }

    [Fact]
    public void Equals_TwoFailedGenericResultsWithEqualErrors_AreEqual()
    {
        // Arrange
        var first = Result<int>.FromError(CreateError());
        var second = Result<int>.FromError(CreateError());

        // Act & Assert
        first.Equals(second).ShouldBeTrue();
        (first == second).ShouldBeTrue();
        first.GetHashCode().ShouldBe(second.GetHashCode());
    }

    [Fact]
    public void Equals_TwoFailedGenericResultsWithEqualErrorsHavingTemplatedDetails_AreEqual()
    {
        // Arrange
        var first = Result<int>.FromError(
            CreateError().AddDetail("#/email", "The email is required.", "errors.email.required", ("minLength", 3)));
        var second = Result<int>.FromError(
            CreateError().AddDetail("#/email", "The email is required.", "errors.email.required", ("minLength", 3)));

        // Act & Assert
        first.Equals(second).ShouldBeTrue();
        (first == second).ShouldBeTrue();
        first.GetHashCode().ShouldBe(second.GetHashCode());
    }

    [Fact]
    public void Equals_FailedGenericResultsWithErrorsHavingDifferentTemplatedDetailParams_AreNotEqual()
    {
        // Arrange
        var first = Result<int>.FromError(
            CreateError().AddDetail("#/email", "The email is required.", "errors.email.required", ("minLength", 3)));
        var second = Result<int>.FromError(
            CreateError().AddDetail("#/email", "The email is required.", "errors.email.required", ("minLength", 5)));

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
    }

    [Fact]
    public void Equals_FailedGenericResultsWithDifferentErrors_AreNotEqual()
    {
        // Arrange
        var first = Result<int>.FromError(CreateError("First title"));
        var second = Result<int>.FromError(CreateError("Second title"));

        // Act & Assert
        first.Equals(second).ShouldBeFalse();
        (first == second).ShouldBeFalse();
    }

    [Fact]
    public void Equals_SuccessfulAndFailedGenericResult_AreNotEqual()
    {
        // Arrange
        var success = Result.FromValue(42);
        var failure = Result<int>.FromError(CreateError());

        // Act & Assert
        success.Equals(failure).ShouldBeFalse();
        (success == failure).ShouldBeFalse();
    }

    #endregion

    #region helper methods

    private static Error CreateError(string title = "Test title")
    {
        return Error.Validation(ErrorUri.Tag("tag:test.com,2024:test"), title, "Test detail");
    }

    #endregion
}
