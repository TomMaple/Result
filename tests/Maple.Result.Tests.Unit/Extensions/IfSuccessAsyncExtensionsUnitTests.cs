using Maple.Result.Extensions;
using Moq;
using System;
using System.Threading.Tasks;

namespace Maple.Result.Tests.Unit.Extensions;

public class IfSuccessAsyncExtensionsUnitTests
{
    #region IfSuccessAsync (Result, Func<Task>)

    [Fact]
    public async Task IfSuccessAsync_NoResultWithAction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut.IfSuccessAsync(() => new Task(() => { })));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Task>? ActionAsync = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Task>? ActionAsync = null;

        var sut = GetErrorResult();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithAction_CallsAction()
    {
        // Arrange
        var actionMock = new Mock<Func<Task>>();

        var sut = Result.Success();

        // Act
        await sut.IfSuccessAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithAction_ReturnsSuccessfulResult()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = await sut.IfSuccessAsync(() => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeTrue();
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithAction_DoesNotCallAction()
    {
        // Arrange
        var actionMock = new Mock<Func<Task>>();

        var sut = GetErrorResult();

        // Act
        await sut.IfSuccessAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithAction_ReturnsErrorResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = await sut.IfSuccessAsync(() => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfSuccessAsync (Result, Func<Task<Result>>)

    [Fact]
    public async Task IfSuccessAsync_NoResultWithResultFunction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut.IfSuccessAsync(() => Task.FromResult(Result.Success())));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Task<Result>>? Function = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Task<Result>>? Function = null;

        var sut = GetErrorResult();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithResultFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Task<Result>>>();

        var sut = Result.Success();

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithSuccessfulResultFunction_ReturnsSuccessfulResult()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = await sut.IfSuccessAsync(() => Task.FromResult(Result.Success()));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeTrue();
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithErrorResultFunction_ReturnsErrorResultWithFunctionError()
    {
        // Arrange
        var errorResult = GetErrorResult();

        var sut = Result.Success();

        // Act
        var result = await sut.IfSuccessAsync(() => Task.FromResult(errorResult));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(errorResult.Error);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Task<Result>>>();

        var sut = GetErrorResult();

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithResultFunction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = await sut.IfSuccessAsync(() => Task.FromResult(Result.Success()));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfSuccessAsync (Result, Func<Task<T>>)

    [Fact]
    public async Task IfSuccessAsync_NoResultWithIntFunction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut.IfSuccessAsync(() => Task.FromResult(456)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithNoIntFunction_ThrowsException()
    {
        // Arrange
        const Func<Task<int>>? Function = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithNoIntFunction_ThrowsException()
    {
        // Arrange
        const Func<Task<int>>? Function = null;

        var sut = GetErrorResult();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithIntFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Task<int>>>();

        var sut = Result.Success();

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithIntFunction_ReturnsSuccessfulResultWithInt()
    {
        // Arrange
        const int Value = 2463;

        var sut = Result.Success();

        // Act
        var result = await sut.IfSuccessAsync(() => Task.FromResult(Value));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(Value);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithIntFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Task<int>>>();

        var sut = GetErrorResult();

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithIntFunction_ReturnsErrorResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = await sut.IfSuccessAsync(() => Task.FromResult(234));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfSuccessAsync (Result, Func<Task<Result<T>>>)

    [Fact]
    public async Task IfSuccessAsync_NoResultWithGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut.IfSuccessAsync(() => Task.FromResult(Result.FromValue(123))));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Task<Result<int>>>? Function = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Task<Result<int>>>? Function = null;

        var sut = GetErrorResult();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithResultIntFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Task<Result<int>>>>();

        var sut = Result.Success();

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithSuccessfulResultIntFunction_ReturnsSuccessfulResultWithInt()
    {
        // Arrange
        const int Value = 2463;

        var sut = Result.Success();

        // Act
        var result = await sut.IfSuccessAsync(() => Task.FromResult(Result.FromValue(Value)));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(Value);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithErrorResultIntFunction_ReturnsErrorResultWithFunctionError()
    {
        // Arrange
        var error = GetError();

        var sut = Result.Success();

        // Act
        var result = await sut.IfSuccessAsync(() => Task.FromResult(Result<int>.FromError(error)));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(error);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithResultIntFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Task<Result<int>>>>();

        var sut = GetErrorResult();

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithResultIntFunction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = await sut.IfSuccessAsync(() => Task.FromResult(Result.FromValue(678)));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfSuccessAsync (Result<T>, Func<T, Task>)

    [Fact]
    public async Task IfSuccessAsync_NoGenericResultWithIntAction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut.IfSuccessAsync(_ => Task.CompletedTask));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithNoIntAction_ThrowsException()
    {
        // Arrange
        const int Value = 35;
        const Func<int, Task>? ActionAsync = null;
        
        Result<int> sut = Value;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithNoIntAction_ThrowsException()
    {
        // Arrange
        const Func<int, Task>? ActionAsync = null;

        var sut = GetErrorResult<int>();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithIntAction_CallsAction()
    {
        // Arrange
        const int Value = 35;
        var functionMock = new Mock<Func<int, Task>>();

        Result<int> sut = Value;

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithIntAction_ReturnsResultWithOriginalIntValue()
    {
        // Arrange
        const int Value = 35;

        Result<int> sut = Value;

        // Act
        var result = await sut.IfSuccessAsync(_ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(Value);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithIntAction_DoesNotCallAction()
    {
        // Arrange
        var functionMock = new Mock<Func<int, Task>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithIntAction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = await sut.IfSuccessAsync(_ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfSuccessAsync (Result<T>, Func<T, Task<Result>>)

    [Fact]
    public async Task IfSuccessAsync_NoGenericResultWithResultFunction_ThrowsException()
    {
        // Arrange
        const Result<string>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut.IfSuccessAsync(_ => Task.FromResult(Result.Success())));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const string Value = "Start";
        const Func<string, Task<Result>>? Function = null;

        Result<string> sut = Value;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<string, Task<Result>>? Function = null;

        var sut = GetErrorResult<string>();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithResultFunction_CallsFunction()
    {
        // Arrange
        const string Value = "Start";
        var functionMock = new Mock<Func<string, Task<Result>>>();

        Result<string> sut = Value;

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithSuccessfulResultFunction_ReturnsResultWithNewResultValue()
    {
        // Arrange
        const string Value = "Start";

        Result<string> sut = Value;

        // Act
        var result = await sut.IfSuccessAsync(_ => Task.FromResult(Result.Success()));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeTrue();
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithErrorResultFunction_ReturnsErrorResultWithFunctionError()
    {
        // Arrange
        const string Value = "Start";
        var errorResult = GetErrorResult();

        Result<string> sut = Value;

        // Act
        var result = await sut.IfSuccessAsync(_ => Task.FromResult(errorResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(errorResult.Error);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<string, Task<Result>>>();

        var sut = GetErrorResult<string>();

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithResultFunction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult<string>();

        // Act
        var result = await sut.IfSuccessAsync(x => Task.FromResult(Result.Success()));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfSuccessAsync (Result<T>, Func<T, Task<TNext>>)

    [Fact]
    public async Task IfSuccessAsync_NoGenericResultWithIntFunction_ThrowsException()
    {
        // Arrange
        const Result<double>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut.IfSuccessAsync(_ => Task.FromResult(12.34)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithNoIntFunction_ThrowsException()
    {
        // Arrange
        const double Value = 12.34;
        const Func<double, Task<int>>? Function = null;

        Result<double> sut = Value;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithNoIntFunction_ThrowsException()
    {
        // Arrange
        const Func<double, Task<int>>? Function = null;

        var sut = GetErrorResult<double>();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithIntFunction_CallsFunction()
    {
        // Arrange
        const double Value = 12.34;
        var functionMock = new Mock<Func<double, Task<int>>>();

        Result<double> sut = Value;

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithIntFunction_ReturnsResultWithIntValue()
    {
        // Arrange
        const int ExpectedValue = 4937;
        const double Value = 12.34;

        Result<double> sut = Value;

        // Act
        var result = await sut.IfSuccessAsync(_ => Task.FromResult(ExpectedValue));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithIntFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<double, Task<int>>>();

        var sut = GetErrorResult<double>();

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<double>()), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithIntFunction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult<double>();

        // Act
        var result = await sut.IfSuccessAsync(_ => Task.FromResult(9827));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfSuccessAsync (Result<T>, Func<T, Task<Result<TNext>>>)

    [Fact]
    public async Task IfSuccessAsync_NoGenericResultWithGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut.IfSuccessAsync(x => Task.FromResult(Result.FromValue(x.ToString()))));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const int Value = 39;
        const Func<int, Task<Result<string>>>? Function = null;

        Result<int> sut = Value;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Func<int, Task<Result<string>>>? Function = null;

        var sut = GetErrorResult<int>();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithGenericResultFunction_CallsFunction()
    {
        // Arrange
        const int Value = 39;
        var functionMock = new Mock<Func<int, Task<Result<string>>>>();

        Result<int> sut = Value;

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithSuccessfulGenericResultFunction_ReturnsGenericResult()
    {
        // Arrange
        const string ExpectedValue = "New Value";
        const int Value = 49;

        Result<int> sut = Value;

        // Act
        var result = await sut.IfSuccessAsync(_ => Task.FromResult(Result.FromValue(ExpectedValue)));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithErrorGenericResultFunction_ReturnsErrorResultWithFunctionError()
    {
        // Arrange
        const int Value = 49;
        var errorResult = GetErrorResult<string>();

        Result<int> sut = Value;

        // Act
        var result = await sut.IfSuccessAsync(_ => Task.FromResult(errorResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(errorResult.Error);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithGenericResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<int, Task<Result<string>>>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithGenericResultFunction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = await sut.IfSuccessAsync(x => Task.FromResult(Result.FromValue(x.ToString())));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region helper methods

    private static Error GetError()
    {
        return Error.Failure(ErrorUri.None(), "e06d75de-115c-46ec-a5f6-f6373007ec61", "Error title 1");
    }

    private static Result GetErrorResult()
    {
        return Error.Failure(ErrorUri.None(), "636d213a-2dc4-4f63-adaf-bbfb80d888ab", "Error title 3");
    }

    private static Result<T> GetErrorResult<T>()
    {
        return Error.Failure(ErrorUri.None(), "a3ef9d52-c18d-4707-8ef3-f7e25072224f", "Error title 4");
    }

    #endregion
}
