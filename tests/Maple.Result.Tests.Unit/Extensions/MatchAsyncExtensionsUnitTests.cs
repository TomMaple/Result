using Maple.Result.Extensions;
using Moq;
using System;
using System.Threading.Tasks;

namespace Maple.Result.Tests.Unit.Extensions;

public class MatchAsyncExtensionsUnitTests
{
    #region MatchAsync (Result, Action, Action<Error>)

    [Fact]
    public async Task MatchAsync_NoResultWithActions_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception =
            await Record.ExceptionAsync(() => Sut!.MatchAsync(() => Task.CompletedTask, _ => Task.CompletedTask));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithNoSuccessActionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Task>? SuccessAction = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(SuccessAction!, _ => Task.CompletedTask));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessActionAndNoErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task>? ErrorAction = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(() => Task.CompletedTask, ErrorAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessAndErrorActions_CallsSuccessAction()
    {
        // Arrange
        var successActionMock = new Mock<Func<Task>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(successActionMock.Object, _ => Task.CompletedTask);

        // Assert
        successActionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessAndErrorActions_DoesNotCallErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, Task>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(() => Task.CompletedTask, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessAndErrorActions_ReturnsOriginalResult()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = await sut.MatchAsync(() => Task.CompletedTask, _ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBe(sut);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithSuccessAndErrorActions_DoesNotCallSuccessAction()
    {
        // Arrange
        var successActionMock = new Mock<Func<Task>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(successActionMock.Object, _ => Task.CompletedTask);

        // Assert
        successActionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithSuccessAndErrorActions_CallsErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, Task>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(() => Task.CompletedTask, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithSuccessAndErrorActions_ReturnsOriginalResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = await sut.MatchAsync(() => Task.CompletedTask, _ => Task.CompletedTask);

        // Assert
        result.ShouldBe(sut);
    }

    #endregion

    #region MatchAsync (Result, Func<Result>, Action<Error>)

    [Fact]
    public async Task MatchAsync_NoResultWithSuccessResultFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(
            () => Sut!.MatchAsync(() => Task.FromResult(Result.Success()), _ => Task.CompletedTask));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithNoSuccessResultFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Task<Result>>? SuccessFunction = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(SuccessFunction!, _ => Task.CompletedTask));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessResultFunctionAndNoErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task>? ErrorAction = null;

        var sut = Result.Success();

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(() => Task.FromResult(Result.Success()), ErrorAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessResultFunctionAndErrorAction_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<Task<Result>>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => Task.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessResultFunctionAndErrorAction_DoesNotCallErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, Task>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(() => Task.FromResult(Result.Success()), errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessResultFunctionAndErrorAction_ReturnsSuccessFunctionResult()
    {
        // Arrange
        var successFunctionResult = Result.Success();

        var sut = Result.Success();

        // Act
        var result = await sut.MatchAsync(() => Task.FromResult(successFunctionResult), _ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(successFunctionResult);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessErrorResultFunctionAndErrorAction_ReturnsSuccessFunctionResult()
    {
        // Arrange
        var successFunctionError = GetErrorResult();

        var sut = Result.Success();

        // Act
        var result = await sut.MatchAsync(() => Task.FromResult(successFunctionError), _ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(successFunctionError);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithSuccessResultFunctionAndErrorAction_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<Task<Result>>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => Task.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithSuccessResultFunctionAndErrorAction_CallsErrorFunction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, Task>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(() => Task.FromResult(Result.Success()), errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithSuccessResultFunctionAndErrorAction_ReturnsOriginalResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = await sut.MatchAsync(() => Task.FromResult(Result.Success()), _ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
    }

    #endregion

    #region MatchAsync (Result, Func<Result>, Func<Error, Result>)

    [Fact]
    public async Task MatchAsync_NoResultWithSuccessAndErrorResultFunctions_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(
            () => Sut!.MatchAsync(() => Task.FromResult(Result.Success), _ => Task.FromResult(Result.Success())));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithNoSuccessResultFunctionAndErrorResultFunction_ThrowsException2()
    {
        // Arrange
        const Func<Task<Result>>? SuccessFunction = null;

        var sut = Result.Success();

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(SuccessFunction!, _ => Task.FromResult(Result.Success())));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessResultFunctionAndNoErrorResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task<Result>>? ErrorFunction = null;

        var sut = Result.Success();

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(() => Task.FromResult(Result.Success()), ErrorFunction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessAndErrorResultFunctions_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<Task<Result>>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => Task.FromResult(Result.Success()));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessAndErrorResultFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, Task<Result>>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(() => Task.FromResult(Result.Success()), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessAndErrorResultFunctions_ReturnsSuccessFunctionResult()
    {
        // Arrange
        var successFunctionResult = Result.Success();

        var sut = Result.Success();

        // Act
        var result = await sut.MatchAsync(
            () => Task.FromResult(successFunctionResult),
            _ => Task.FromResult(Result.Success()));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(successFunctionResult);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithSuccessAndErrorResultFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<Task<Result>>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => Task.FromResult(Result.Success()));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithSuccessAndErrorResultFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, Task<Result>>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(() => Task.FromResult(Result.Success()), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithSuccessAndErrorResultFunctions_ReturnsErrorFunctionResult()
    {
        // Arrange
        var errorFunctionResult = Result.Success();

        var sut = GetErrorResult();

        // Act
        var result = await sut.MatchAsync(
            () => Task.FromResult(Result.Success()),
            _ => Task.FromResult(errorFunctionResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(errorFunctionResult);
    }

    #endregion

    #region MatchAsync (Result, Func<T>, Action<Error>)

    [Fact]
    public async Task MatchAsync_NoResultWithGenericSuccessFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception =
            await Record.ExceptionAsync(() => Sut!.MatchAsync(() => Task.FromResult(1), _ => Task.CompletedTask));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithNoGenericSuccessFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Task<int>>? SuccessFunction = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(SuccessFunction!, _ => Task.CompletedTask));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericSuccessFunctionNoErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task>? ErrorAction = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(() => Task.FromResult(1), ErrorAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericSuccessFunctionAndErrorAction_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<Task<int>>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => Task.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericSuccessFunctionAndErrorAction_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, Task>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(() => Task.FromResult(345), errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericSuccessFunctionAndErrorAction_ReturnsSuccessFunctionValue()
    {
        // Arrange
        const int SuccessFunctionValue = 42;

        var sut = Result.Success();

        // Act
        var result = await sut.MatchAsync(() => Task.FromResult(SuccessFunctionValue), _ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(SuccessFunctionValue);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithGenericSuccessFunctionAndErrorAction_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<Task<int>>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => Task.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithGenericSuccessFunctionAndErrorAction_CallsErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, Task>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(() => Task.FromResult(432), errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithGenericSuccessFunctionAndErrorAction_ReturnsOriginalResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = await sut.MatchAsync(() => Task.FromResult(123), _ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region MatchAsync (Result, Func<T>, Func<Error, T>)

    [Fact]
    public async Task MatchAsync_NoResultWithGenericSuccessAndErrorFunctions_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception =
            await Record.ExceptionAsync(() => Sut!.MatchAsync(() => Task.FromResult(1), _ => Task.FromResult(2)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithNoGenericSuccessFunctionAndGenericErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<Task<int>>? Function = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(Function!, _ => Task.FromResult(2)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericSuccessFunctionAndNoGenericErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task<int>>? ErrorFunction = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(() => Task.FromResult(1), ErrorFunction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericSuccessAndErrorFunctions_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<Task<int>>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => Task.FromResult(2));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericSuccessAndErrorFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, Task<int>>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(() => Task.FromResult(1), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericSuccessAndErrorFunctions_ReturnsSuccessFunctionValue()
    {
        // Arrange
        const int ExpectedValue = 58;

        var sut = Result.Success();

        // Act
        var result = await sut.MatchAsync(() => Task.FromResult(ExpectedValue), _ => Task.FromResult(2));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithGenericSuccessAndErrorFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<Task<int>>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => Task.FromResult(2));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithGenericSuccessAndErrorFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, Task<int>>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(() => Task.FromResult(1), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithGenericSuccessAndErrorFunctions_ReturnsErrorFunctionValue()
    {
        // Arrange
        const int ExpectedValue = 73;

        var sut = GetErrorResult();

        // Act
        var result = await sut.MatchAsync(() => Task.FromResult(1), _ => Task.FromResult(ExpectedValue));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    #endregion

    #region MatchAsync (Result, Func<Result<T>>, Action<Error>)

    [Fact]
    public async Task MatchAsync_NoResultWithGenericResultSuccessFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(
            () => Sut!.MatchAsync(() => Task.FromResult(Result.FromValue(1)), _ => Task.CompletedTask));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithNoGenericResultResultFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Task<Result<int>>>? SuccessFunction = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(SuccessFunction!, _ => Task.CompletedTask));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericResultSuccessFunctionAndNoErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task>? ErrorAction = null;

        var sut = Result.Success();

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(() => Task.FromResult(Result.FromValue(1)), ErrorAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericResultSuccessFunctionAndErrorAction_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<Task<Result<int>>>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => Task.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericResultSuccessFunctionAndErrorAction_DoesNotCallErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, Task>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(() => Task.FromResult(Result.FromValue(1)), errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericResultSuccessFunctionAndErrorAction_ReturnsSuccessFunctionResultValue()
    {
        // Arrange
        var expectedResult = Result.FromValue(12);

        var sut = Result.Success();

        // Act
        var result = await sut.MatchAsync(() => Task.FromResult(expectedResult), _ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.ShouldBeSameAs(expectedResult);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithGenericResultSuccessFunctionAndErrorAction_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<Task<Result<int>>>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => Task.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithGenericResultSuccessFunctionAndErrorAction_CallsErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, Task>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(() => Task.FromResult(Result.FromValue(1)), errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithGenericResultSuccessFunctionAndErrorAction_ReturnsOriginalResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = await sut.MatchAsync(() => Task.FromResult(Result.FromValue(1)), _ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region MatchAsync (Result, Func<Result<T>>, Func<Error, Result<T>>)

    [Fact]
    public async Task MatchAsync_NoResultWithGenericResultSuccessAndErrorFunctions_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(
            () => Sut!.MatchAsync(() => Task.FromResult(Result.FromValue(1)), _ => Task.FromResult(Result.FromValue(2))));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithNoGenericResultSuccessFunctionAndGenericResultErrorFunction_ThrowsException3()
    {
        // Arrange
        const Func<Task<Result<int>>>? Function = null;

        var sut = Result.Success();

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(Function!, _ => Task.FromResult(Result.FromValue(2))));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericResultSuccessFunctionAndNoGenericResultErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task<Result<int>>>? ErrorFunction = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(
            () => sut.MatchAsync(() => Task.FromResult(Result.FromValue(1)), ErrorFunction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericResultSuccessAndErrorFunctions_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<Task<Result<int>>>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => Task.FromResult(Result.FromValue(2)));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericResultSuccessAndErrorFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, Task<Result<int>>>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(() => Task.FromResult(Result.FromValue(1)), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericResultSuccessAndErrorFunctions_ReturnsSuccessFunctionResult()
    {
        // Arrange
        var expectedResult = Result.FromValue(9);

        var sut = Result.Success();

        // Act
        var result = await sut.MatchAsync(
            () => Task.FromResult(expectedResult),
            _ => Task.FromResult(Result.FromValue(2)));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.ShouldBeSameAs(expectedResult);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithGenericResultSuccessAndErrorFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<Task<Result<int>>>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => Task.FromResult(Result.FromValue(2)));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithGenericResultSuccessAndErrorFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, Task<Result<int>>>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(() => Task.FromResult(Result.FromValue(1)), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithGenericResultSuccessAndErrorFunctions_ReturnsErrorFunctionValueResult()
    {
        // Arrange
        var expectedResult = Result.FromValue(15);

        var sut = GetErrorResult();

        // Act
        var result = await sut.MatchAsync(
            () => Task.FromResult(Result.FromValue(1)),
            _ => Task.FromResult(expectedResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(expectedResult);
    }

    #endregion

    #region MatchAsync (Result<T>, Action<T>, Action<Error>)

    [Fact]
    public async Task MatchAsync_NoGenericResultWithSuccessAndErrorActions_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception =
            await Record.ExceptionAsync(() => Sut!.MatchAsync(_ => Task.CompletedTask, _ => Task.CompletedTask));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithNoSuccessActionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<int, Task>? SuccessAction = null;

        Result<int> sut = 1;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(SuccessAction!, _ => Task.CompletedTask));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithSuccessActionAndNoErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task>? ErrorAction = null;

        Result<int> sut = 1;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(_ => Task.CompletedTask, ErrorAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithSuccessAndErrorActions_CallsSuccessAction()
    {
        // Arrange
        var successActionMock = new Mock<Func<int, Task>>();

        const int Value = 25;
        Result<int> sut = Value;

        // Act
        await sut.MatchAsync(successActionMock.Object, _ => Task.CompletedTask);

        // Assert
        successActionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithSuccessAndErrorActions_DoesNotCallErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, Task>>();

        const int Value = 25;
        Result<int> sut = Value;

        // Act
        await sut.MatchAsync(_ => Task.CompletedTask, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithSuccessAndErrorActions_ReturnsOriginalValueResult()
    {
        // Arrange
        const int Value = 25;
        Result<int> sut = Value;

        // Act
        var result = await sut.MatchAsync(_ => Task.CompletedTask, _ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.ShouldBeSameAs(sut);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithSuccessAndErrorActions_DoesNotCallSuccessAction()
    {
        // Arrange
        var successActionMock = new Mock<Func<int, Task>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.MatchAsync(successActionMock.Object, _ => Task.CompletedTask);

        // Assert
        successActionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithSuccessAndErrorActions_CallsErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, Task>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.MatchAsync(_ => Task.CompletedTask, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithSuccessAndErrorActions_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = await sut.MatchAsync(_ => Task.CompletedTask, _ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.ShouldBeSameAs(sut);
    }

    #endregion

    #region MatchAsync (Result<T, TNext>, Func<T, TNext>, Action<Error>)

    [Fact]
    public async Task MatchAsync_NoGenericResultWithGenericSuccessFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.MatchAsync(Task.FromResult, _ => Task.CompletedTask));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithNoGenericSuccessFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<int, Task<int>>? Transform = null;

        Result<int> sut = 1;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(Transform!, _ => Task.CompletedTask));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericSuccessFunctionAndNoErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task>? ErrorAction = null;

        Result<int> sut = 1;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(Task.FromResult, ErrorAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericSuccessFunctionAndErrorAction_CallsSuccessFunction()
    {
        // Arrange
        const int Value = 15;
        var successFunctionMock = new Mock<Func<int, Task<int>>>();

        Result<int> sut = Value;

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => Task.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericSuccessFunctionAndErrorAction_DoesNotCallErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, Task>>();

        Result<int> sut = 15;

        // Act
        await sut.MatchAsync(Task.FromResult, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericSuccessFunctionAndErrorAction_ReturnsSuccessFunctionValue()
    {
        // Arrange
        const int Value = 15;
        const int ExpectedValue = 30;

        Result<int> sut = Value;

        // Act
        var result = await sut.MatchAsync(x => Task.FromResult(2 * x), _ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithGenericSuccessFunctionAndErrorAction_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<int, Task<int>>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => Task.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithGenericSuccessFunctionAndErrorAction_CallsErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, Task>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.MatchAsync(Task.FromResult, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithGenericSuccessFunctionAndErrorAction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = await sut.MatchAsync(Task.FromResult, _ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region MatchAsync (Result<T>, Func<T, TNext>, Func<Error, TNext>)

    [Fact]
    public async Task MatchAsync_NoGenericResultWithGenericSuccessAndErrorFunctions_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(
            () => Sut!.MatchAsync(x => Task.FromResult(x.ToString()), _ => Task.FromResult("error")));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithNoGenericSuccessFunctionAndGenericErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<int, Task<string>>? SuccessFunction = null;

        Result<int> sut = 1;

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(SuccessFunction!, _ => Task.FromResult("error")));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericSuccessFunctionAndNoGenericErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task<string>>? ErrorFunction = null;

        Result<int> sut = 1;

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(x => Task.FromResult(x.ToString()), ErrorFunction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericSuccessAndErrorFunctions_CallsSuccessFunction()
    {
        // Arrange
        const int Value = 5;
        var successFunctionMock = new Mock<Func<int, Task<string>>>();
        successFunctionMock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .ReturnsAsync("success value");

        Result<int> sut = Value;

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => Task.FromResult("error"));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericSuccessAndErrorFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, Task<string>>>();

        Result<int> sut = 5;

        // Act
        await sut.MatchAsync(x => Task.FromResult(x.ToString()), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericSuccessAndErrorFunctions_ReturnsSuccessFunctionValue()
    {
        // Arrange
        const int Value = 5;
        const string ExpectedValue = "6";

        Result<int> sut = Value;

        // Act
        var result = await sut.MatchAsync(x => Task.FromResult((x + 1).ToString()), _ => Task.FromResult("error"));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithProjectionFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<int, Task<string>>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => Task.FromResult("error"));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithProjectionFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, Task<string>>>();
        errorFunctionMock
            .Setup(x => x.Invoke(It.IsAny<Error>()))
            .ReturnsAsync("error value");

        var sut = GetErrorResult<int>();

        // Act
        await sut.MatchAsync(x => Task.FromResult(x.ToString()), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithProjectionFunctions_ReturnsErrorFunctionValue()
    {
        // Arrange
        const string ExpectedValue = "Error function value";

        var sut = GetErrorResult<int>();

        // Act
        var result = await sut.MatchAsync(x => Task.FromResult(x.ToString()), _ => Task.FromResult(ExpectedValue));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    #endregion

    #region MatchAsync (Result<T>, Func<T, Result<TNext>>, Action<Error>)

    [Fact]
    public async Task MatchAsync_NoGenericResultWithGenericResultSuccessFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(
            () => Sut!.MatchAsync(x => Task.FromResult(Result.FromValue(x.ToString())), _ => Task.CompletedTask));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithNoGenericResultSuccessFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<int, Task<Result<string>>>? SuccessFunction = null;

        Result<int> sut = 1;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(SuccessFunction!, _ => Task.CompletedTask));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericResultSuccessFunctionAndNoErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task>? ErrorAction = null;

        Result<int> sut = 1;

        // Act
        var exception = await Record.ExceptionAsync(
            () => sut.MatchAsync(x => Task.FromResult(Result.FromValue(x.ToString())), ErrorAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericResultSuccessFunctionAndErrorAction_CallsSuccessFunction()
    {
        // Arrange
        const int Value = 88;
        var successFunctionMock = new Mock<Func<int, Task<Result<string>>>>();

        Result<int> sut = 88;

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => Task.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericResultSuccessFunctionAndErrorAction_DoesNotCallErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, Task>>();

        Result<int> sut = 88;

        // Act
        await sut.MatchAsync(x => Task.FromResult(Result.FromValue(x.ToString())), errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericResultSuccessFunctionAndErrorAction_ReturnsSuccessFunctionValueResult()
    {
        // Arrange
        const string ExpectedValue = "90";
        const int Value = 88;
        var expectedResult = Result.FromValue(ExpectedValue);

        Result<int> sut = Value;

        // Act
        var result = await sut.MatchAsync(
            x => Task.FromResult(Result.FromValue((x + 2).ToString())),
            _ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.ShouldBe(expectedResult);
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithGenericResultSuccessFunctionAndErrorAction_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<int, Task<Result<string>>>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => Task.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithGenericResultSuccessFunctionAndErrorAction_CallsErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, Task>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.MatchAsync(x => Task.FromResult(Result.FromValue(x.ToString())), errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithGenericResultSuccessFunctionAndErrorAction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result =
            await sut.MatchAsync(x => Task.FromResult(Result.FromValue(x.ToString())), _ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region MatchAsync (Result<T>, Func<T, Result<TNext>>, Func<Error, Result<TNext>>)

    [Fact]
    public async Task MatchAsync_NoGenericResultWithGenericResultSuccessAndErrorFunctions_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(
            () => Sut!.MatchAsync(
                x => Task.FromResult(Result.FromValue(x.ToString())),
                _ => Task.FromResult(Result.FromValue("error value"))));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithNoGenericResultSuccessFunctionAndGenericResultErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<int, Task<Result<string>>>? SuccessFunction = null;

        Result<int> sut = 1;

        // Act
        var exception = await Record.ExceptionAsync(
            () => sut.MatchAsync(SuccessFunction!, _ => Task.FromResult(Result.FromValue("error value"))));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericResultSuccessFunctionAndNoGenericResultErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task<Result<string>>>? ErrorFunction = null;

        Result<int> sut = 1;

        // Act
        var exception = await Record.ExceptionAsync(
            () => sut.MatchAsync(x => Task.FromResult(Result.FromValue(x.ToString())), ErrorFunction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericResultSuccessAndErrorFunctions_CallsSuccessFunction()
    {
        // Arrange
        const int Value = 90;
        var successFunctionMock = new Mock<Func<int, Task<Result<string>>>>();

        Result<int> sut = Value;

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => Task.FromResult(Result.FromValue("error value")));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericResultSuccessAndErrorFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, Task<Result<string>>>>();

        Result<int> sut = 90;

        // Act
        await sut.MatchAsync(x => Task.FromResult(Result.FromValue(x.ToString())), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericResultSuccessAndErrorFunctions_ReturnsSuccessFunctionValueResult()
    {
        // Arrange
        const string ExpectedValue = "-90-";
        const int Value = 90;

        Result<int> sut = Value;

        // Act
        var result = await sut.MatchAsync(
            x => Task.FromResult(Result.FromValue($"-{x}-")),
            _ => Task.FromResult(Result.FromValue("error value")));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithGenericResultSuccessAndErrorFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<int, Task<Result<string>>>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => Task.FromResult(Result.FromValue("error value")));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithGenericResultSuccessAndErrorFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, Task<Result<string>>>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.MatchAsync(x => Task.FromResult(Result.FromValue(x.ToString())), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithGenericResultSuccessAndErrorFunctions_ReturnsErrorFunctionValueResult()
    {
        // Arrange
        const string ExpectedValue = "error value";

        var sut = GetErrorResult<int>();

        // Act
        var result = await sut.MatchAsync(
            x => Task.FromResult(Result.FromValue(x.ToString())),
            _ => Task.FromResult(Result.FromValue(ExpectedValue)));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    #endregion

    #region helper methods

    private static Result GetErrorResult()
    {
        return Error.Failure(ErrorUri.None(), "50f1c1fb-560e-4bb3-84da-034072df7ca2", "Error title 6");
    }

    private static Result<T> GetErrorResult<T>()
    {
        return Error.Failure(ErrorUri.None(), "3d7a81ec-801d-45f4-b4b0-8d58e93b1d89", "Error title 7");
    }

    #endregion
}
