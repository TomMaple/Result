// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Result.Extensions;
using Moq;
using System;

namespace Maple.Result.Tests.Unit.Extensions;

public class LinqExtensionsUnitTests
{
    #region SelectMany(Result<T>, Func<T, Result<TMiddle>>, Func<T, TMiddle, TNext>)

    [Fact]
    public void SelectMany_NoResultWithSelectorFunctions_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.SelectMany(x => Result.FromValue(x.ToString()), (x, _) => (double)x));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void SelectMany_SuccessfulResultWithNoCollectionSelectorAndResultSelector_ThrowsException()
    {
        // Arrange
        const Func<int, Result<string>>? CollectionSelector = null;

        Result<int> sut = 5;

        // Act
        var exception = Record.Exception(() => sut.SelectMany(CollectionSelector!, (x, _) => (double)x));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void SelectMany_ErrorResultWithNoCollectionSelectorAndResultSelector_ThrowsException()
    {
        // Arrange
        const Func<int, Result<string>>? CollectionSelector = null;

        var sut = GetErrorResult<int>();

        // Act
        var exception = Record.Exception(() => sut.SelectMany(CollectionSelector!, (x, _) => (double)x));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void SelectMany_SuccessfulResultWithNoResultSelectorFunction_ThrowsException()
    {
        // Arrange
        const Func<int, string, double>? ResultSelectorFunction = null;

        Result<int> sut = 5;

        // Act
        var exception = Record.Exception(() => sut.SelectMany(x => Result.FromValue(x.ToString()), ResultSelectorFunction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void SelectMany_ErrorResultWithNoResultSelectorFunction_ThrowsException()
    {
        // Arrange
        const Func<int, string, double>? ResultSelectorFunction = null;

        var sut = GetErrorResult<int>();

        // Act
        var exception = Record.Exception(() => sut.SelectMany(x => Result.FromValue(x.ToString()), ResultSelectorFunction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void SelectMany_SuccessfulResultWithSelectorFunctions_CallsBothFunctions()
    {
        // Arrange
        const int InitialValue = 4839;
        const string ExpectedMiddleValue = "4839";
        const double ExpectedFinalValue = 4839;

        var collectionSelectorMock = new Mock<Func<int, Result<string>>>();
        collectionSelectorMock
            .Setup(x => x.Invoke(InitialValue))
            .Returns(Result.FromValue(ExpectedMiddleValue));

        var resultSelectorMock = new Mock<Func<int, string, double>>();
        resultSelectorMock
            .Setup(x => x.Invoke(InitialValue, ExpectedMiddleValue))
            .Returns(ExpectedFinalValue);

        Result<int> sut = InitialValue;

        // Act
        sut.SelectMany(collectionSelectorMock.Object, resultSelectorMock.Object);

        // Assert
        collectionSelectorMock.Verify(x => x.Invoke(InitialValue), Times.Once);
        resultSelectorMock.Verify(x => x.Invoke(InitialValue, ExpectedMiddleValue), Times.Once);
    }

    [Fact]
    public void SelectMany_SuccessfulResultWithSelectorFunctions_ReturnsValue()
    {
        // Arrange
        const int InitialValue = 4839;
        const double ExpectedFinalValue = 4839;

        Result<int> sut = InitialValue;

        // Act
        var result = sut.SelectMany(x => Result.FromValue(x.ToString()), (x, _) => (double)x);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<double>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedFinalValue);
    }

    [Fact]
    public void SelectMany_ErrorResultWithSelectorFunctions_DoesNotCallFunctions()
    {
        // Arrange
        var collectionSelectorMock = new Mock<Func<int, Result<string>>>();
        var resultSelectorMock = new Mock<Func<int, string, double>>();

        var sut = GetErrorResult<int>();

        // Act
        sut.SelectMany(collectionSelectorMock.Object, resultSelectorMock.Object);

        // Assert
        collectionSelectorMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
        resultSelectorMock.Verify(x => x.Invoke(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void SelectMany_ErrorResultWithSelectorFunctions_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = sut.SelectMany(x => Result.FromValue(x.ToString()), (x, _) => (double)x);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<double>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region Select(Result<T>, Func<T, TNext>)

    [Fact]
    public void Select_NoResultWithSelector_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.Select(x => x.ToString()));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Select_SuccessfulResultWithNoSelector_ThrowsException()
    {
        // Arrange
        const Func<int, string>? Selector = null;

        Result<int> sut = 5;

        // Act
        var exception = Record.Exception(() => sut.Select(Selector!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Select_ErrorResultWithNoSelector_ThrowsException()
    {
        // Arrange
        const Func<int, string>? Selector = null;

        var sut = GetErrorResult<int>();

        // Act
        var exception = Record.Exception(() => sut.Select(Selector!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Select_SuccessfulResultWithSelector_CallsSelector()
    {
        // Arrange
        const int InitialValue = 4839;
        const string ExpectedValue = "4839";

        var selectorMock = new Mock<Func<int, string>>();
        selectorMock
            .Setup(x => x.Invoke(InitialValue))
            .Returns(ExpectedValue);

        Result<int> sut = InitialValue;

        // Act
        sut.Select(selectorMock.Object);

        // Assert
        selectorMock.Verify(x => x.Invoke(InitialValue), Times.Once);
    }

    [Fact]
    public void Select_SuccessfulResultWithSelector_ReturnsValue()
    {
        // Arrange
        const int InitialValue = 4839;
        const string ExpectedValue = "4839";

        Result<int> sut = InitialValue;

        // Act
        var result = sut.Select(x => x.ToString());

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public void Select_ErrorResultWithSelector_DoesNotCallSelector()
    {
        // Arrange
        var selectorMock = new Mock<Func<int, string>>();

        var sut = GetErrorResult<int>();

        // Act
        sut.Select(selectorMock.Object);

        // Assert
        selectorMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void Select_ErrorResultWithSelector_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = sut.Select(x => x.ToString());

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region LINQ query syntax

    [Fact]
    public void LinqQuerySyntax_SingleClauseSuccessfulResult_ReturnsProjectedValueResult()
    {
        // Arrange
        const string ExpectedValue = "90";

        Result<int> sut = 90;

        // Act
        var result =
            from initialValue in sut
            select initialValue.ToString();

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public void LinqQuerySyntax_SingleClauseErrorResult_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result =
            from initialValue in sut
            select initialValue.ToString();

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    [Fact]
    public void LinqQuerySyntax_SuccessfulResult_CallsFunctions()
    {
        // Arrange
        Result<int> sut = 90;

        var function1Mock = new Mock<Func<int, Result<int>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .Returns(100);

        var function2Mock = new Mock<Func<int, Result<string>>>();
        function2Mock
            .Setup(x => x.Invoke(100))
            .Returns("100");

        // Act
        _ = from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result;

        // Assert
        function1Mock.Verify(x => x.Invoke(90), Times.Once);
        function2Mock.Verify(x => x.Invoke(100), Times.Once);
    }

    [Fact]
    public void LinqQuerySyntax_SuccessfulResult_ReturnsValueResult()
    {
        // Arrange
        const string ExpectedValue = "100";

        Result<int> sut = 90;

        var function1 = (int value) => Result.FromValue(value + 10);
        var function2 = (int value) => Result.FromValue(value.ToString());

        // Act
        var result = from initialValue in sut
            from function1Result in function1(initialValue)
            from function2Result in function2(function1Result)
            select function2Result;

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public void LinqQuerySyntax_FirstErrorFunction_CallsFirstFunction()
    {
        // Arrange
        Result<int> sut = 90;

        var function1Mock = new Mock<Func<int, Result<int>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .Returns(GetErrorResult<int>());

        var function2Mock = new Mock<Func<int, Result<string>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .Returns("100");

        // Act
        _ = from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result;

        // Assert
        function1Mock.Verify(x => x.Invoke(90), Times.Once);
    }

    [Fact]
    public void LinqQuerySyntax_FirstErrorFunction_DoesNotCallSecondFunction()
    {
        // Arrange
        Result<int> sut = 90;

        var function1Mock = new Mock<Func<int, Result<int>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .Returns(GetErrorResult<int>());

        var function2Mock = new Mock<Func<int, Result<string>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .Returns("100");

        // Act
        _ = from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result;

        // Assert
        function2Mock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void LinqQuerySyntax_FirstErrorFunction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        var function1Mock = new Mock<Func<int, Result<int>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .Returns(GetErrorResult<int>());

        var function2Mock = new Mock<Func<int, Result<string>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .Returns("100");

        // Act
        var result = from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result;

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    [Fact]
    public void LinqQuerySyntax_SecondErrorFunction_CallsFirstFunction()
    {
        // Arrange
        Result<int> sut = 90;

        var function1Mock = new Mock<Func<int, Result<int>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .Returns(100);

        var function2Mock = new Mock<Func<int, Result<string>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .Returns(GetErrorResult<string>());

        // Act
        _ = from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result;

        // Assert
        function1Mock.Verify(x => x.Invoke(90), Times.Once);
    }

    [Fact]
    public void LinqQuerySyntax_SecondErrorFunction_CallsSecondFunction()
    {
        // Arrange
        Result<int> sut = 90;

        var function1Mock = new Mock<Func<int, Result<int>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .Returns(100);

        var function2Mock = new Mock<Func<int, Result<string>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .Returns(GetErrorResult<string>());

        // Act
        _ = from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result;

        // Assert
        function2Mock.Verify(x => x.Invoke(100), Times.Once);
    }

    [Fact]
    public void LinqQuerySyntax_SecondErrorFunction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        var function1Mock = new Mock<Func<int, Result<int>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .Returns(100);

        var function2Mock = new Mock<Func<int, Result<string>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .Returns(GetErrorResult<string>());

        // Act
        var result = from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result;

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region helper methods

    private static Result<T> GetErrorResult<T>()
    {
        return Error.Failure(ErrorUri.None(), "6b648e52-bde5-4e23-a233-c5dd4c4d2e73", "Error title 4");
    }

    #endregion
}