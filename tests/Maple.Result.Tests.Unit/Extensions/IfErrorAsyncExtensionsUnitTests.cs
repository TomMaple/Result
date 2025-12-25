using System;
using System.Threading.Tasks;
using Maple.Result.Extensions;
using Moq;

namespace Maple.Result.Tests.Unit.Extensions;

public class IfErrorAsyncExtensionsUnitTests
{
    #region IfErrorAsync (Result, Func<Error, Task>)

    [Fact]
    public async Task IfErrorAsync_NoResultWithAction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut.IfErrorAsync(_ => Task.CompletedTask));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessResultWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task>? ActionAsync = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(ActionAsync!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task>? ActionAsync = null;

        var sut = GetErrorResult();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(ActionAsync!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessResultWithAction_DoesNotCallAction()
    {
        // Arrange
        var actionMock = new Mock<Func<Error, Task>>();

        var sut = Result.Success();

        // Act
        await sut.IfErrorAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task IfErrorAsync_SuccessResultWithAction_ReturnsOriginalSuccessResult()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = await sut.IfErrorAsync(_ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
        result.IsSuccess().ShouldBeTrue();
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultWithAction_CallsAction()
    {
        // Arrange
        var actionMock = new Mock<Func<Error, Task>>();

        var sut = GetErrorResult();

        // Act
        await sut.IfErrorAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultWithAction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = await sut.IfErrorAsync(_ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfErrorAsync (Result, Func<Error, Task<Result>>)

    [Fact]
    public async Task IfErrorAsync_NoResultWithResultFunction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut.IfErrorAsync(_ => Task.FromResult(Result.Success())));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task<Result>>? Function = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task<Result>>? Function = null;

        var sut = GetErrorResult();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessResultWithResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Error, Task<Result>>>();

        var sut = Result.Success();

        // Act
        await sut.IfErrorAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task IfErrorAsync_SuccessResultWithResultFunction_ReturnsOriginalSuccessResult()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = await sut.IfErrorAsync(_ => Task.FromResult(Result.Success()));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
        result.IsSuccess().ShouldBeTrue();
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultWithResultFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Error, Task<Result>>>();

        var sut = GetErrorResult();

        // Act
        await sut.IfErrorAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultWithResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        var replacementResult = Result.Success();

        var sut = GetErrorResult();

        // Act
        var result = await sut.IfErrorAsync(_ => Task.FromResult(replacementResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(replacementResult);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultWithErrorResultFunction_ReturnsFunctionError()
    {
        // Arrange
        var replacementError = GetReplacementError();

        var sut = GetErrorResult();

        // Act
        var result = await sut.IfErrorAsync(_ => Task.FromResult(Result.FromError(replacementError)));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(replacementError);
    }

    #endregion

    #region IfErrorAsync (Result<T>, Func<Error, Task>)

    [Fact]
    public async Task IfErrorAsync_NoGenericResultWithAction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut.IfErrorAsync(_ => Task.CompletedTask));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessGenericResultWithNoAction_ThrowsException()
    {
        // Arrange
        const int Value = 24;
        const Func<Error, Task>? ActionAsync = null;

        Result<int> sut = Value;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(ActionAsync!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task>? ActionAsync = null;

        var sut = GetErrorResult<int>();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(ActionAsync!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessGenericResultWithAction_DoesNotCallAction()
    {
        // Arrange
        const int Value = 29;
        var actionMock = new Mock<Func<Error, Task>>();

        Result<int> sut = Value;

        // Act
        await sut.IfErrorAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task IfErrorAsync_SuccessGenericResultWithAction_ReturnsOriginalSuccessResult()
    {
        // Arrange
        const int Value = 38;

        Result<int> sut = Value;

        // Act
        var result = await sut.IfErrorAsync(_ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(Value);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultWithAction_CallsAction()
    {
        // Arrange
        var actionMock = new Mock<Func<Error, Task>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.IfErrorAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultWithAction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = await sut.IfErrorAsync(_ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfErrorAsync (Result<T>, Func<Error, Task<IResult>>)

    [Fact]
    public async Task IfErrorAsync_NoGenericResultWithResultFunction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut.IfErrorAsync(_ => Task.FromResult(Result.FromValue(1))));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const int Value = 24;
        const Func<Error, Task<Result<int>>>? Function = null;

        Result<int> sut = Value;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task<Result<int>>>? Function = null;

        var sut = GetErrorResult<int>();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultWithResultFunction_DoesNotCallFunction()
    {
        // Arrange
        const int Value = 42;
        var functionMock = new Mock<Func<Error, Task<Result<int>>>>();

        Result<int> sut = Value;

        // Act
        var result = await sut.IfErrorAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultWithResultFunction_ReturnsOriginalSuccessResult()
    {
        // Arrange
        const int Value = 11;

        Result<int> sut = Value;

        // Act
        var result = await sut.IfErrorAsync(_ => Task.FromResult(Result.FromValue(2)));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
        result.Value.ShouldBe(Value);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultWithResultFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Error, Task<Result<int>>>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.IfErrorAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultWithResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        var replacementResult = Result.FromValue(58);

        var sut = GetErrorResult<int>();

        // Act
        var result = await sut.IfErrorAsync(_ => Task.FromResult(replacementResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(replacementResult);
        result.Value.ShouldBe(58);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultWithResultFunction_ReturnsErrorFromFunction()
    {
        // Arrange
        var replacementError = GetReplacementError();

        var sut = GetErrorResult<int>();

        // Act
        var result = await sut.IfErrorAsync(_ => Task.FromResult(Result<int>.FromError(replacementError)));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(replacementError);
    }

    #endregion

    #region helper methods

    private static Result GetErrorResult()
    {
        return Error.Failure(ErrorUri.None(), "4b3612e7-d22a-45f2-9a5c-4a8c14d39141", "Error title 1");
    }

    private static Result<T> GetErrorResult<T>()
    {
        return Error.Failure(ErrorUri.None(), "ff7711a7-c224-4280-816c-1722864177cf", "Error title 2");
    }

    private static Error GetReplacementError()
    {
        return Error.Failure(ErrorUri.None(), "53e1e02c-3e21-47c9-9e1a-93ba22f03ba8", "Replacement error");
    }

    #endregion
}