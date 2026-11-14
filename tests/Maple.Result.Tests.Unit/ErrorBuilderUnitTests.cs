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
using System.Collections.Generic;
using Sut = Maple.Result.ErrorBuilder;

namespace Maple.Result.Tests.Unit;

public class ErrorBuilderUnitTests
{
    #region ctor

    [Fact]
    public void Ctor_RequiredMembers_BuildsErrorWithDefaults()
    {
        // Arrange
        var sut = new Sut(ErrorCategory.Conflict, ErrorUri.Tag("tag:test.com,2024:Test"), "Test title");

        // Act
        var result = sut.Build();

        // Assert
        result.Category.ShouldBe(ErrorCategory.Conflict);
        result.TypeUri.ShouldBe("tag:test.com,2024:Test");
        result.Title.ShouldBe("Test title");
        result.Detail.ShouldBeNull();
        result.DetailTemplated.ShouldBeNull();
        result.InstanceUri.ShouldBeNull();
        result.ErrorDetails.ShouldBeEmpty();
        result.Metadata.ShouldBeEmpty();
    }

    [Fact]
    public void Ctor_DefaultErrorUri_BuildsErrorWithAboutBlankTypeUri()
    {
        // Act
        var result = new Sut(ErrorCategory.Failure, default, "Test title").Build();

        // Assert
        result.TypeUri.ShouldBe("about:blank");
    }

    [Fact]
    public void Ctor_UndefinedCategory_ThrowsArgumentOutOfRangeException()
    {
        // Act
        var exception = Record.Exception(() => new Sut((ErrorCategory)999, ErrorUri.None(), "Test title"));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentOutOfRangeException>();
        ((ArgumentOutOfRangeException)exception).ParamName.ShouldBe("category");
    }

    [Fact]
    public void Ctor_NullTitle_ThrowsArgumentNullException()
    {
        // Act
        var exception = Record.Exception(() => new Sut(ErrorCategory.Failure, ErrorUri.None(), null!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Ctor_BlankTitle_ThrowsArgumentException(string title)
    {
        // Act
        var exception = Record.Exception(() => new Sut(ErrorCategory.Failure, ErrorUri.None(), title));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentException>();
    }

    #endregion

    #region factory methods

    public static TheoryData<Func<ErrorUri, string, Sut>, ErrorCategory> FactoryMethods => new()
    {
        { Sut.Validation, ErrorCategory.Validation },
        { Sut.Unauthenticated, ErrorCategory.Unauthenticated },
        { Sut.Unauthorized, ErrorCategory.Unauthorized },
        { Sut.NotFound, ErrorCategory.NotFound },
        { Sut.Timeout, ErrorCategory.Timeout },
        { Sut.Conflict, ErrorCategory.Conflict },
        { Sut.Failure, ErrorCategory.Failure },
        { Sut.Critical, ErrorCategory.Critical },
        { Sut.NotImplemented, ErrorCategory.NotImplemented },
        { Sut.Unavailable, ErrorCategory.Unavailable },
        { Sut.LimitExceeded, ErrorCategory.LimitExceeded }
    };

    [Theory]
    [MemberData(nameof(FactoryMethods))]
    public void FactoryMethod_Always_BuildsErrorOfItsCategory(Func<ErrorUri, string, Sut> factory,
        ErrorCategory expectedCategory)
    {
        // Act
        var result = factory(ErrorUri.Tag("tag:test.com,2024:Test"), "Test title").Build();

        // Assert
        result.Category.ShouldBe(expectedCategory);
        result.TypeUri.ShouldBe("tag:test.com,2024:Test");
        result.Title.ShouldBe("Test title");
    }

    [Theory]
    [MemberData(nameof(FactoryMethods))]
    public void FactoryMethod_BlankTitle_ThrowsArgumentException(Func<ErrorUri, string, Sut> factory,
        ErrorCategory _)
    {
        // Act
        var exception = Record.Exception(() => factory(ErrorUri.None(), " "));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeAssignableTo<ArgumentException>();
    }

    #endregion

    #region setters

    [Fact]
    public void Setters_AllMembers_BuildErrorWithAllMembers()
    {
        // Arrange
        var detailTemplated = new TemplatedMessage("message-id", new Dictionary<string, object?> { ["key"] = "value" });

        // Act
        var result = Sut.Validation(ErrorUri.None(), "Initial title")
            .WithCategory(ErrorCategory.NotFound)
            .WithTypeUri(ErrorUri.Tag("tag:test.com,2024:Test"))
            .WithTitle("Test title")
            .WithDetail("Test description.")
            .WithDetailTemplated(detailTemplated)
            .WithInstanceUri(ErrorUri.Locator("http://test.com/instance/1013"))
            .Build();

        // Assert
        result.Category.ShouldBe(ErrorCategory.NotFound);
        result.TypeUri.ShouldBe("tag:test.com,2024:Test");
        result.Title.ShouldBe("Test title");
        result.Detail.ShouldBe("Test description.");
        result.DetailTemplated.ShouldBe(detailTemplated);
        result.InstanceUri.ShouldBe("http://test.com/instance/1013");
    }

    [Fact]
    public void Setters_Always_ReturnSameBuilder()
    {
        // Arrange
        var sut = Sut.Validation(ErrorUri.None(), "Test title");

        // Act & Assert
        sut.WithCategory(ErrorCategory.Failure).ShouldBeSameAs(sut);
        sut.WithTypeUri(ErrorUri.None()).ShouldBeSameAs(sut);
        sut.WithTitle("Other title").ShouldBeSameAs(sut);
        sut.WithDetail("Detail").ShouldBeSameAs(sut);
        sut.WithDetailTemplated("message-id").ShouldBeSameAs(sut);
        sut.WithDetailTemplated((TemplatedMessage?)null).ShouldBeSameAs(sut);
        sut.WithInstanceUri(null).ShouldBeSameAs(sut);
        sut.AddErrorDetail(null, "Detail").ShouldBeSameAs(sut);
        sut.AddErrorDetail(new ErrorDetail(null, "Detail")).ShouldBeSameAs(sut);
        sut.WithMetadata("limit", 1).ShouldBeSameAs(sut);
        sut.WithMetadata([new KeyValuePair<string, object?>("other", 2)]).ShouldBeSameAs(sut);
        sut.WithoutMetadata("limit").ShouldBeSameAs(sut);
        sut.WithoutMetadata().ShouldBeSameAs(sut);
    }

    [Fact]
    public void Setters_NullOptionalMembers_ClearThem()
    {
        // Arrange
        var sut = Sut.Validation(ErrorUri.None(), "Test title")
            .WithDetail("Detail")
            .WithDetailTemplated("message-id")
            .WithInstanceUri(ErrorUri.Locator("http://test.com/instance/1013"));

        // Act
        var result = sut
            .WithDetail(null)
            .WithDetailTemplated((TemplatedMessage?)null)
            .WithInstanceUri(null)
            .Build();

        // Assert
        result.Detail.ShouldBeNull();
        result.DetailTemplated.ShouldBeNull();
        result.InstanceUri.ShouldBeNull();
    }

    [Fact]
    public void WithCategory_UndefinedCategory_ThrowsArgumentOutOfRangeExceptionAndKeepsPreviousValue()
    {
        // Arrange
        var sut = Sut.Validation(ErrorUri.None(), "Test title");

        // Act
        var exception = Record.Exception(() => sut.WithCategory((ErrorCategory)999));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentOutOfRangeException>();
        sut.Build().Category.ShouldBe(ErrorCategory.Validation);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void WithTitle_NullOrBlankTitle_ThrowsArgumentExceptionAndKeepsPreviousValue(string? title)
    {
        // Arrange
        var sut = Sut.Validation(ErrorUri.None(), "Test title");

        // Act
        var exception = Record.Exception(() => sut.WithTitle(title!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeAssignableTo<ArgumentException>();
        sut.Build().Title.ShouldBe("Test title");
    }

    [Fact]
    public void WithDetailTemplated_TemplateIdAndNamedValues_BuildsTemplatedMessage()
    {
        // Act
        var result = Sut.Validation(ErrorUri.None(), "Test title")
            .WithDetailTemplated("message-id", ("min", 3), ("name", null))
            .Build();

        // Assert
        result.DetailTemplated.ShouldNotBeNull();
        result.DetailTemplated.TemplateId.ShouldBe("message-id");
        result.DetailTemplated.Params.ShouldNotBeNull();
        result.DetailTemplated.Params.Count.ShouldBe(2);
        result.DetailTemplated.Params["min"].ShouldBe(3);
        result.DetailTemplated.Params["name"].ShouldBeNull();
    }

    [Fact]
    public void WithDetailTemplated_NullLiteral_ClearsTheValue()
    {
        // Act
        var result = Sut.Validation(ErrorUri.None(), "Test title")
            .WithDetailTemplated("message-id")
            .WithDetailTemplated(null)
            .Build();

        // Assert
        result.DetailTemplated.ShouldBeNull();
    }
    [Fact]
    public void WithDetailTemplated_DuplicateName_ThrowsArgumentException()
    {
        // Arrange
        var sut = Sut.Validation(ErrorUri.None(), "Test title");

        // Act
        var exception = Record.Exception(() => sut.WithDetailTemplated("message-id", ("min", 3), ("min", 4)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentException>();
    }

    [Fact]
    public void WithDetailTemplated_BlankTemplateId_ThrowsArgumentException()
    {
        // Arrange
        var sut = Sut.Validation(ErrorUri.None(), "Test title");

        // Act
        var exception = Record.Exception(() => sut.WithDetailTemplated(" "));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentException>();
    }

    #endregion

    #region AddErrorDetail

    [Fact]
    public void AddErrorDetail_SeveralDetails_KeepsTheirOrder()
    {
        // Arrange
        var sut = Sut.Validation(ErrorUri.None(), "Test title");

        // Act
        foreach (var field in new[] { "name", "email", "age" })
            sut.AddErrorDetail($"#/{field}", $"The {field} is invalid.");

        var result = sut.Build();

        // Assert
        result.ErrorDetails.Count.ShouldBe(3);
        result.ErrorDetails[0].PropertyPointer.ShouldBe("#/name");
        result.ErrorDetails[1].PropertyPointer.ShouldBe("#/email");
        result.ErrorDetails[2].PropertyPointer.ShouldBe("#/age");
    }

    [Fact]
    public void AddErrorDetail_ErrorDetailInstance_AddsSameInstance()
    {
        // Arrange
        var detail = new ErrorDetail("#/name", "The name is required.");

        // Act
        var result = Sut.Validation(ErrorUri.None(), "Test title").AddErrorDetail(detail).Build();

        // Assert
        result.ErrorDetails.ShouldHaveSingleItem().ShouldBeSameAs(detail);
    }

    [Fact]
    public void AddErrorDetail_MessageIdAndNamedValues_BuildsTemplatedDetail()
    {
        // Act
        var result = Sut.Validation(ErrorUri.None(), "Test title")
            .AddErrorDetail("#/name", "Too short.", "errors.name.length", ("min", 3), ("max", null))
            .Build();

        // Assert
        var detail = result.ErrorDetails.ShouldHaveSingleItem();
        detail.PropertyPointer.ShouldBe("#/name");
        detail.Detail.ShouldBe("Too short.");
        detail.DetailTemplated.ShouldNotBeNull();
        detail.DetailTemplated.TemplateId.ShouldBe("errors.name.length");
        detail.DetailTemplated.Params.ShouldNotBeNull();
        detail.DetailTemplated.Params["min"].ShouldBe(3);
        detail.DetailTemplated.Params["max"].ShouldBeNull();
    }

    [Fact]
    public void AddErrorDetail_NamedValuesWithoutTemplateId_ThrowsArgumentExceptionAndAddsNothing()
    {
        // Arrange
        var sut = Sut.Validation(ErrorUri.None(), "Test title");

        // Act
        var exception = Record.Exception(() => sut.AddErrorDetail("#/name", "Too short.", null, ("min", 3)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentException>();
        ((ArgumentException)exception).ParamName.ShouldBe("detailNamedValues");
        sut.Build().ErrorDetails.ShouldBeEmpty();
    }

    [Fact]
    public void AddErrorDetail_NoTemplateIdAndNoNamedValues_AddsDetailWithoutTemplate()
    {
        // Act
        var result = Sut.Validation(ErrorUri.None(), "Test title")
            .AddErrorDetail("#/name", "Too short.")
            .Build();

        // Assert
        result.ErrorDetails.ShouldHaveSingleItem().DetailTemplated.ShouldBeNull();
    }

    [Fact]
    public void AddErrorDetail_RepeatedName_ThrowsArgumentExceptionWithClearMessage()
    {
        // Arrange
        var sut = Sut.Validation(ErrorUri.None(), "Test title");

        // Act
        var exception = Record.Exception(() => sut.AddErrorDetail("#/name", "Too short.", "errors.length",
            ("min", 3), ("min", 4)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentException>();
        exception.Message.ShouldContain("‘min’ is repeated");
        ((ArgumentException)exception).ParamName.ShouldBe("detailNamedValues");
    }

    [Fact]
    public void AddErrorDetail_NullErrorDetail_ThrowsArgumentNullException()
    {
        // Arrange
        var sut = Sut.Validation(ErrorUri.None(), "Test title");

        // Act
        var exception = Record.Exception(() => sut.AddErrorDetail(null!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
    }

    [Fact]
    public void AddErrorDetail_NullDetailText_ThrowsArgumentNullExceptionAndAddsNothing()
    {
        // Arrange
        var sut = Sut.Validation(ErrorUri.None(), "Test title");

        // Act
        var exception = Record.Exception(() => sut.AddErrorDetail("#/name", null!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        sut.Build().ErrorDetails.ShouldBeEmpty();
    }

    #endregion

    #region metadata

    [Fact]
    public void WithMetadata_NewAndRepeatedKeys_AddsAndReplacesValues()
    {
        // Act
        var result = Sut.LimitExceeded(ErrorUri.None(), "Test title")
            .WithMetadata("limit", 100)
            .WithMetadata("remaining", 5)
            .WithMetadata("remaining", 0)
            .WithMetadata("traceId", null)
            .Build();

        // Assert
        result.Metadata.Count.ShouldBe(3);
        result.Metadata["limit"].ShouldBe(100);
        result.Metadata["remaining"].ShouldBe(0);
        result.Metadata["traceId"].ShouldBeNull();
    }

    [Fact]
    public void WithMetadata_Sequence_AddsAndReplacesEntries()
    {
        // Act
        var result = Sut.LimitExceeded(ErrorUri.None(), "Test title")
            .WithMetadata("limit", 100)
            .WithMetadata(new Dictionary<string, object?> { ["limit"] = 200, ["policy"] = "monthly" })
            .Build();

        // Assert
        result.Metadata.Count.ShouldBe(2);
        result.Metadata["limit"].ShouldBe(200);
        result.Metadata["policy"].ShouldBe("monthly");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("type")]
    [InlineData("Title")]
    public void WithMetadata_InvalidKey_ThrowsArgumentException(string key)
    {
        // Arrange
        var sut = Sut.LimitExceeded(ErrorUri.None(), "Test title");

        // Act
        var exception = Record.Exception(() => sut.WithMetadata(key, 1));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentException>();
    }

    [Fact]
    public void WithMetadata_NullKey_ThrowsArgumentNullException()
    {
        // Arrange
        var sut = Sut.LimitExceeded(ErrorUri.None(), "Test title");

        // Act
        var exception = Record.Exception(() => sut.WithMetadata(null!, 1));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
    }

    [Fact]
    public void WithMetadata_NullSequence_ThrowsArgumentNullException()
    {
        // Arrange
        var sut = Sut.LimitExceeded(ErrorUri.None(), "Test title");

        // Act
        var exception = Record.Exception(() => sut.WithMetadata((IEnumerable<KeyValuePair<string, object?>>)null!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
    }

    [Fact]
    public void WithMetadata_SequenceWithInvalidKey_ThrowsAndAppliesNoEntry()
    {
        // Arrange
        var sut = Sut.LimitExceeded(ErrorUri.None(), "Test title").WithMetadata("limit", 100);

        // Act
        var exception = Record.Exception(() => sut.WithMetadata([
            new KeyValuePair<string, object?>("limit", 200),
            new KeyValuePair<string, object?>("policy", "monthly"),
            new KeyValuePair<string, object?>("status", 429)
        ]));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentException>();
        var result = sut.Build();
        result.Metadata.Count.ShouldBe(1);
        result.Metadata["limit"].ShouldBe(100);
    }

    [Fact]
    public void WithoutMetadata_ExistingAndAbsentKeys_RemovesOnlyExistingEntry()
    {
        // Act
        var result = Sut.LimitExceeded(ErrorUri.None(), "Test title")
            .WithMetadata("limit", 100)
            .WithMetadata("remaining", 0)
            .WithoutMetadata("limit")
            .WithoutMetadata("missing")
            .Build();

        // Assert
        result.Metadata.Count.ShouldBe(1);
        result.Metadata.ShouldContainKey("remaining");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void WithoutMetadata_NullOrBlankKey_ThrowsArgumentException(string? key)
    {
        // Arrange
        var sut = Sut.LimitExceeded(ErrorUri.None(), "Test title");

        // Act
        var exception = Record.Exception(() => sut.WithoutMetadata(key!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeAssignableTo<ArgumentException>();
    }

    [Fact]
    public void WithoutMetadata_NoKey_RemovesAllEntries()
    {
        // Act
        var result = Sut.LimitExceeded(ErrorUri.None(), "Test title")
            .WithMetadata("limit", 100)
            .WithMetadata("remaining", 0)
            .WithoutMetadata()
            .Build();

        // Assert
        result.Metadata.ShouldBeEmpty();
    }

    [Fact]
    public void WithoutMetadata_NoKeyThenWithMetadata_KeepsOnlyNewEntries()
    {
        // Act
        var result = Sut.LimitExceeded(ErrorUri.None(), "Test title")
            .WithMetadata("limit", 100)
            .WithoutMetadata()
            .WithMetadata("policy", "monthly")
            .Build();

        // Assert
        result.Metadata.Count.ShouldBe(1);
        result.Metadata["policy"].ShouldBe("monthly");
    }

    #endregion

    #region Build

    [Fact]
    public void Build_LaterChangesToBuilder_DoNotAffectBuiltError()
    {
        // Arrange
        var sut = Sut.Validation(ErrorUri.None(), "Test title")
            .AddErrorDetail("#/first", "First detail")
            .WithMetadata("limit", 100);
        var built = sut.Build();

        // Act
        sut.WithTitle("Changed title")
            .AddErrorDetail("#/second", "Second detail")
            .WithMetadata("limit", 200)
            .WithMetadata("remaining", 0);

        // Assert
        built.Title.ShouldBe("Test title");
        built.ErrorDetails.Count.ShouldBe(1);
        built.Metadata.Count.ShouldBe(1);
        built.Metadata["limit"].ShouldBe(100);
    }

    [Fact]
    public void Build_CalledAgainAfterChanges_ReturnsErrorWithAllChanges()
    {
        // Arrange
        var sut = Sut.Validation(ErrorUri.None(), "Test title").AddErrorDetail("#/first", "First detail");
        var first = sut.Build();

        // Act
        var second = sut.AddErrorDetail("#/second", "Second detail").Build();

        // Assert
        first.ErrorDetails.Count.ShouldBe(1);
        second.ErrorDetails.Count.ShouldBe(2);
        second.ErrorDetails[0].PropertyPointer.ShouldBe("#/first");
        second.ErrorDetails[1].PropertyPointer.ShouldBe("#/second");
    }

    [Fact]
    public void Build_CalledTwiceWithoutChanges_ReturnsEqualErrorsSharingCollections()
    {
        // Arrange
        var sut = Sut.Validation(ErrorUri.None(), "Test title")
            .AddErrorDetail("#/first", "First detail")
            .WithMetadata("limit", 100);

        // Act
        var first = sut.Build();
        var second = sut.Build();

        // Assert
        second.ShouldBe(first);
        second.ShouldNotBeSameAs(first);
        second.ErrorDetails.ShouldBeSameAs(first.ErrorDetails);
        second.Metadata.ShouldBeSameAs(first.Metadata);
    }

    [Fact]
    public void Build_ToBuilderWithoutChanges_ReusesSourceCollections()
    {
        // Arrange
        var error = Error.Validation(ErrorUri.None(), "Test title")
            .WithErrorDetail("#/first", "First detail")
            .WithMetadata("limit", 100);

        // Act
        var result = error.ToBuilder().WithDetail("Changed detail").Build();

        // Assert
        result.ErrorDetails.ShouldBeSameAs(error.ErrorDetails);
        result.Metadata.ShouldBeSameAs(error.Metadata);
    }

    [Fact]
    public void Build_ToBuilderAndChanges_LeavesSourceErrorUnchanged()
    {
        // Arrange
        var error = Error.Validation(ErrorUri.None(), "Test title")
            .WithErrorDetail("#/first", "First detail")
            .WithMetadata("limit", 100);

        // Act
        var result = error.ToBuilder()
            .AddErrorDetail("#/second", "Second detail")
            .WithoutMetadata("limit")
            .Build();

        // Assert
        result.ErrorDetails.Count.ShouldBe(2);
        result.Metadata.ShouldBeEmpty();
        error.ErrorDetails.Count.ShouldBe(1);
        error.Metadata.Count.ShouldBe(1);
    }

    [Fact]
    public void Build_ToBuilderOfFullError_ReturnsEqualError()
    {
        // Arrange
        var error = Error.Conflict(ErrorUri.Tag("tag:test.com,2024:Test"), "Test title", "Test description.",
                ErrorUri.Locator("http://test.com/instance/1013"), "message-id", ("key", "value"))
            .WithErrorDetail("#/property", "Detail", "errors.property", ("min", 3))
            .WithMetadata("retryAfter", 30);

        // Act
        var result = error.ToBuilder().Build();

        // Assert
        result.ShouldBe(error);
        result.GetHashCode().ShouldBe(error.GetHashCode());
    }

    #endregion
}
