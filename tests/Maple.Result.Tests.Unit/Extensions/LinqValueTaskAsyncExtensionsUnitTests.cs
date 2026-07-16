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
using Moq;

namespace Maple.Result.Tests.Unit.Extensions;

public class LinqValueTaskAsyncExtensionsUnitTests
{
    #region SelectMany(ValueTask<Result<T>>, Func<T, ValueTask<Result<TMiddle>>>, Func<T, TMiddle, TNext>)

    [Fact]
    public async Task SelectMany_SuccessfulResultWithNoCollectionSelectorAndResultSelector_ThrowsException()
    {
        // Arrange
        const Func<int, ValueTask<Result<string>>>? CollectionSelector = null;

        var sut = ValueTask.FromResult(Result.FromValue(5));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.SelectMany(CollectionSelector!, (x, _) => (double)x).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task SelectMany_NoResultValueWithSelectorFunctions_ThrowsException()
    {
        // Arrange
        var sut = ValueTask.FromResult<Result<int>>(null!);

        // Act
        var exception = await Record.ExceptionAsync(() => sut.SelectMany(x => ValueTask.FromResult(Result.FromValue(x.ToString())), (x, _) => (double)x).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<InvalidOperationException>();
        exception.Message.ShouldStartWith("The asynchronous operation represented by ‘result’ returned null.");
    }

    [Fact]
    public async Task SelectMany_ErrorResultWithNoCollectionSelectorAndResultSelector_ThrowsException()
    {
        // Arrange
        const Func<int, ValueTask<Result<string>>>? CollectionSelector = null;

        var sut = GetErrorResultValueTask<int>();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.SelectMany(CollectionSelector!, (x, _) => (double)x).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task SelectMany_SuccessfulResultWithNoResultSelectorFunction_ThrowsException()
    {
        // Arrange
        const Func<int, string, double>? ResultSelectorFunction = null;

        var sut = ValueTask.FromResult(Result.FromValue(5));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.SelectMany(x => ValueTask.FromResult(Result.FromValue(x.ToString())), ResultSelectorFunction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task SelectMany_ErrorResultWithNoResultSelectorFunction_ThrowsException()
    {
        // Arrange
        const Func<int, string, double>? ResultSelectorFunction = null;

        var sut = GetErrorResultValueTask<int>();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.SelectMany(x => ValueTask.FromResult(Result.FromValue(x.ToString())), ResultSelectorFunction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task SelectMany_SuccessfulResultWithSelectorFunctions_CallsBothFunctions()
    {
        // Arrange
        const int InitialValue = 4839;
        const string ExpectedMiddleValue = "4839";
        const double ExpectedFinalValue = 4839;

        var collectionSelectorMock = new Mock<Func<int, ValueTask<Result<string>>>>();
        collectionSelectorMock
            .Setup(x => x.Invoke(InitialValue))
            .ReturnsAsync(Result.FromValue(ExpectedMiddleValue));

        var resultSelectorMock = new Mock<Func<int, string, double>>();
        resultSelectorMock
            .Setup(x => x.Invoke(InitialValue, ExpectedMiddleValue))
            .Returns(ExpectedFinalValue);

        var sut = ValueTask.FromResult(Result.FromValue(InitialValue));

        // Act
        await sut.SelectMany(collectionSelectorMock.Object, resultSelectorMock.Object);

        // Assert
        collectionSelectorMock.Verify(x => x.Invoke(InitialValue), Times.Once);
        resultSelectorMock.Verify(x => x.Invoke(InitialValue, ExpectedMiddleValue), Times.Once);
    }

    [Fact]
    public async Task SelectMany_SuccessfulResultWithSelectorFunctions_ReturnsValue()
    {
        // Arrange
        const int InitialValue = 4839;
        const double ExpectedFinalValue = 4839;

        var sut = ValueTask.FromResult(Result.FromValue(InitialValue));

        // Act
        var result = await sut.SelectMany(x => ValueTask.FromResult(Result.FromValue(x.ToString())), (x, _) => (double)x);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<double>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedFinalValue);
    }

    [Fact]
    public async Task SelectMany_ErrorResultWithSelectorFunctions_DoesNotCallFunctions()
    {
        // Arrange
        var collectionSelectorMock = new Mock<Func<int, ValueTask<Result<string>>>>();
        var resultSelectorMock = new Mock<Func<int, string, double>>();

        var sut = GetErrorResultValueTask<int>();

        // Act
        await sut.SelectMany(collectionSelectorMock.Object, resultSelectorMock.Object);

        // Assert
        collectionSelectorMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
        resultSelectorMock.Verify(x => x.Invoke(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task SelectMany_ErrorResultWithSelectorFunctions_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var errorResult = GetErrorResult<int>();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        var result = await sut.SelectMany(x => ValueTask.FromResult(Result.FromValue(x.ToString())), (x, _) => (double)x);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<double>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(errorResult.Error);
    }

    #endregion

    #region LINQ query syntax

    // ReSharper disable RedundantAssignment

    [Fact]
    public async Task LinqQueryAsyncSyntax_SuccessfulResult_CallsFunctions()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromValue(90));

        var function1Mock = new Mock<Func<int, ValueTask<Result<int>>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .ReturnsAsync(Result.FromValue(100));

        var function2Mock = new Mock<Func<int, ValueTask<Result<string>>>>();
        function2Mock
            .Setup(x => x.Invoke(100))
            .ReturnsAsync(Result.FromValue("100"));

        // Act
        await (
            from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result);

        // Assert
        function1Mock.Verify(x => x.Invoke(90), Times.Once);
        function2Mock.Verify(x => x.Invoke(100), Times.Once);
    }

    [Fact]
    public async Task LinqQueryAsyncSyntax_SuccessfulResult_ReturnsValueResult()
    {
        // Arrange
        const string ExpectedValue = "100";

        var sut = ValueTask.FromResult(Result.FromValue(90));

        static ValueTask<Result<int>> Function1(int value) => ValueTask.FromResult(Result.FromValue(value + 10));
        static ValueTask<Result<string>> Function2(int value) => ValueTask.FromResult(Result.FromValue(value.ToString()));

        // Act
        var result = await (
            from initialValue in sut
            from function1Result in Function1(initialValue)
            from function2Result in Function2(function1Result)
            select function2Result);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task LinqQueryAsyncSyntax_FirstErrorFunction_CallsFirstFunction()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromValue(90));

        var function1Mock = new Mock<Func<int, ValueTask<Result<int>>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .ReturnsAsync(GetErrorResult<int>());

        var function2Mock = new Mock<Func<int, ValueTask<Result<string>>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .ReturnsAsync("100");

        // Act
        await (
            from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result);

        // Assert
        function1Mock.Verify(x => x.Invoke(90), Times.Once);
    }

    [Fact]
    public async Task LinqQueryAsyncSyntax_FirstErrorFunction_DoesNotCallSecondFunction()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromValue(90));

        var function1Mock = new Mock<Func<int, ValueTask<Result<int>>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .ReturnsAsync(GetErrorResult<int>());

        var function2Mock = new Mock<Func<int, ValueTask<Result<string>>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .ReturnsAsync("100");

        // Act
        await (
            from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result);

        // Assert
        function2Mock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task LinqQueryAsyncSyntax_FirstErrorFunction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var errorResult = GetErrorResult<int>();
        var sut = ValueTask.FromResult(errorResult);

        var function1Mock = new Mock<Func<int, ValueTask<Result<int>>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .ReturnsAsync(GetErrorResult<int>());

        var function2Mock = new Mock<Func<int, ValueTask<Result<string>>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .ReturnsAsync("100");

        // Act
        var result = await (
            from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(errorResult.Error);
    }

    [Fact]
    public async Task LinqQueryAsyncSyntax_SecondErrorFunction_CallsFirstFunction()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromValue(90));

        var function1Mock = new Mock<Func<int, ValueTask<Result<int>>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .ReturnsAsync(100);

        var function2Mock = new Mock<Func<int, ValueTask<Result<string>>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .ReturnsAsync(GetErrorResult<string>());

        // Act
        await (
            from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result);

        // Assert
        function1Mock.Verify(x => x.Invoke(90), Times.Once);
    }

    [Fact]
    public async Task LinqQueryAsyncSyntax_SecondErrorFunction_CallsSecondFunction()
    {
        // Arrange
        var sut = ValueTask.FromResult(Result.FromValue(90));

        var function1Mock = new Mock<Func<int, ValueTask<Result<int>>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .ReturnsAsync(100);

        var function2Mock = new Mock<Func<int, ValueTask<Result<string>>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .ReturnsAsync(GetErrorResult<string>());

        // Act
        await (
            from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result);

        // Assert
        function2Mock.Verify(x => x.Invoke(100), Times.Once);
    }

    [Fact]
    public async Task LinqQueryAsyncSyntax_SecondErrorFunction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var errorResult = GetErrorResult<int>();
        var sut = ValueTask.FromResult(errorResult);

        var function1Mock = new Mock<Func<int, ValueTask<Result<int>>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .ReturnsAsync(100);

        var function2Mock = new Mock<Func<int, ValueTask<Result<string>>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .ReturnsAsync(GetErrorResult<string>());

        // Act
        var result = await (
            from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(errorResult.Error);
    }

    // ReSharper restore RedundantAssignment

    #endregion

    #region helper methods

    private static Result<T> GetErrorResult<T>()
    {
        return Error.Failure(ErrorUri.None(), "6b648e52-bde5-4e23-a233-c5dd4c4d2e73", "Error title 4");
    }

    private static ValueTask<Result<T>> GetErrorResultValueTask<T>()
    {
        Result<T> result = Error.Failure(ErrorUri.None(), "7e5de15a-8851-4d75-8308-ec18351d7b70", "Error title 4");
        return ValueTask.FromResult(result);
    }

    #endregion
}