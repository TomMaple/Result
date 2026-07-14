using System;
using System.Threading.Tasks;
using Maple.Result.Extensions;
using Moq;

namespace Maple.Result.Tests.Unit.Extensions;

public class LinqTaskAsyncExtensionsUnitTests
{
#pragma warning disable CS8848 // Operator cannot be used here due to precedence.

    #region SelectMany(Task<Result<T>>, Func<T, Task<Result<TMiddle>>>, Func<T, TMiddle, TNext>)

    [Fact]
    public async Task SelectMany_NoResultWithSelectorFunctions_ThrowsException()
    {
        // Arrange
        const Task<Result<int>>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.SelectMany(x => Task.FromResult(Result.FromValue(x.ToString())), (x, _) => (double)x));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task SelectMany_SuccessfulResultWithNoCollectionSelectorAndResultSelector_ThrowsException()
    {
        // Arrange
        const Func<int, Task<Result<string>>>? CollectionSelector = null;

        var sut = Task.FromResult(Result.FromValue(5));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.SelectMany(CollectionSelector!, (x, _) => (double)x));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task SelectMany_ErrorResultWithNoCollectionSelectorAndResultSelector_ThrowsException()
    {
        // Arrange
        const Func<int, Task<Result<string>>>? CollectionSelector = null;

        var sut = GetErrorResultTask<int>();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.SelectMany(CollectionSelector!, (x, _) => (double)x));

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

        var sut = Task.FromResult(Result.FromValue(5));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.SelectMany(x => Task.FromResult(Result.FromValue(x.ToString())), ResultSelectorFunction!));

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

        var sut = GetErrorResultTask<int>();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.SelectMany(x => Task.FromResult(Result.FromValue(x.ToString())), ResultSelectorFunction!));

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

        var collectionSelectorMock = new Mock<Func<int, Task<Result<string>>>>();
        collectionSelectorMock
            .Setup(x => x.Invoke(InitialValue))
            .ReturnsAsync(Result.FromValue(ExpectedMiddleValue));

        var resultSelectorMock = new Mock<Func<int, string, double>>();
        resultSelectorMock
            .Setup(x => x.Invoke(InitialValue, ExpectedMiddleValue))
            .Returns(ExpectedFinalValue);

        var sut = Task.FromResult(Result.FromValue(InitialValue));

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

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        var result = await sut.SelectMany(x => Task.FromResult(Result.FromValue(x.ToString())), (x, _) => (double)x);

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
        var collectionSelectorMock = new Mock<Func<int, Task<Result<string>>>>();
        var resultSelectorMock = new Mock<Func<int, string, double>>();

        var sut = GetErrorResultTask<int>();

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
        var sut = Task.FromResult(errorResult);

        // Act
        var result = await sut.SelectMany(x => Task.FromResult(Result.FromValue(x.ToString())), (x, _) => (double)x);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<double>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(errorResult.Error);
    }

    #endregion

    #region LINQ query syntax

    [Fact]
    public async Task LinqQueryAsyncSyntax_SuccessfulResult_CallsFunctions()
    {
        // Arrange
        var sut = Task.FromResult(Result.FromValue(90));

        var function1Mock = new Mock<Func<int, Task<Result<int>>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .ReturnsAsync(Result.FromValue(100));

        var function2Mock = new Mock<Func<int, Task<Result<string>>>>();
        function2Mock
            .Setup(x => x.Invoke(100))
            .ReturnsAsync(Result.FromValue("100"));

        // Act
        await
            from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result;

        // Assert
        function1Mock.Verify(x => x.Invoke(90), Times.Once);
        function2Mock.Verify(x => x.Invoke(100), Times.Once);
    }

    [Fact]
    public async Task LinqQueryAsyncSyntax_SuccessfulResult_ReturnsValueResult()
    {
        // Arrange
        const string ExpectedValue = "100";

        var sut = Task.FromResult(Result.FromValue(90));

        static Task<Result<int>> Function1(int value) => Task.FromResult(Result.FromValue(value + 10));
        static Task<Result<string>> Function2(int value) => Task.FromResult(Result.FromValue(value.ToString()));

        // Act
        var result = await
            from initialValue in sut
            from function1Result in Function1(initialValue)
            from function2Result in Function2(function1Result)
            select function2Result;

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task LinqQueryAsyncSyntax_FirstErrorFunction_CallsFirstFunction()
    {
        // Arrange
        var sut = Task.FromResult(Result.FromValue(90));

        var function1Mock = new Mock<Func<int, Task<Result<int>>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .ReturnsAsync(GetErrorResult<int>());

        var function2Mock = new Mock<Func<int, Task<Result<string>>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .ReturnsAsync("100");

        // Act
        await
            from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result;

        // Assert
        function1Mock.Verify(x => x.Invoke(90), Times.Once);
    }

    [Fact]
    public async Task LinqQueryAsyncSyntax_FirstErrorFunction_DoesNotCallSecondFunction()
    {
        // Arrange
        var sut = Task.FromResult(Result.FromValue(90));

        var function1Mock = new Mock<Func<int, Task<Result<int>>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .ReturnsAsync(GetErrorResult<int>());

        var function2Mock = new Mock<Func<int, Task<Result<string>>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .ReturnsAsync("100");

        // Act
        await
            from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result;

        // Assert
        function2Mock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task LinqQueryAsyncSyntax_FirstErrorFunction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var errorResult = GetErrorResult<int>();
        var sut = Task.FromResult(errorResult);

        var function1Mock = new Mock<Func<int, Task<Result<int>>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .ReturnsAsync(GetErrorResult<int>());

        var function2Mock = new Mock<Func<int, Task<Result<string>>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .ReturnsAsync("100");

        // Act
        var result = await
            from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result;

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
        var sut = Task.FromResult(Result.FromValue(90));

        var function1Mock = new Mock<Func<int, Task<Result<int>>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .ReturnsAsync(100);

        var function2Mock = new Mock<Func<int, Task<Result<string>>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .ReturnsAsync(GetErrorResult<string>());

        // Act
        await
            from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result;

        // Assert
        function1Mock.Verify(x => x.Invoke(90), Times.Once);
    }

    [Fact]
    public async Task LinqQueryAsyncSyntax_SecondErrorFunction_CallsSecondFunction()
    {
        // Arrange
        var sut = Task.FromResult(Result.FromValue(90));

        var function1Mock = new Mock<Func<int, Task<Result<int>>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .ReturnsAsync(100);

        var function2Mock = new Mock<Func<int, Task<Result<string>>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .ReturnsAsync(GetErrorResult<string>());

        // Act
        await
            from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result;

        // Assert
        function2Mock.Verify(x => x.Invoke(100), Times.Once);
    }

    [Fact]
    public async Task LinqQueryAsyncSyntax_SecondErrorFunction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var errorResult = GetErrorResult<int>();
        var sut = Task.FromResult(errorResult);

        var function1Mock = new Mock<Func<int, Task<Result<int>>>>();
        function1Mock
            .Setup(x => x.Invoke(90))
            .ReturnsAsync(100);

        var function2Mock = new Mock<Func<int, Task<Result<string>>>>();
        function2Mock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .ReturnsAsync(GetErrorResult<string>());

        // Act
        var result = await
            from initialValue in sut
            from function1Result in function1Mock.Object(initialValue)
            from function2Result in function2Mock.Object(function1Result)
            select function2Result;

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(errorResult.Error);
    }

    #endregion

#pragma warning restore CS8848 // Operator cannot be used here due to precedence.

    #region helper methods

    private static Result<T> GetErrorResult<T>()
    {
        return Error.Failure(ErrorUri.None(), "6b648e52-bde5-4e23-a233-c5dd4c4d2e73", "Error title 4");
    }

    private static Task<Result<T>> GetErrorResultTask<T>()
    {
        Result<T> result = Error.Failure(ErrorUri.None(), "7e5de15a-8851-4d75-8308-ec18351d7b70", "Error title 4");
        return Task.FromResult(result);
    }

    #endregion
}