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

public class IfErrorAsyncExtensionsUnitTests
{
    #region IfErrorAsync (Result, Func<Error, Task>)

    [Fact]
    public async Task IfErrorAsync_NoResultWithAction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfErrorAsync(_ => Task.CompletedTask));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task>? AsyncAction = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(AsyncAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task>? AsyncAction = null;

        var sut = GetErrorResult();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(AsyncAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultWithAction_DoesNotCallAction()
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
    public async Task IfErrorAsync_SuccessfulResultWithAction_ReturnsOriginalResult()
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
    public async Task IfErrorAsync_ErrorResultWithAction_ReturnsOriginalResult()
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
        var exception = await Record.ExceptionAsync(() => Sut!.IfErrorAsync(_ => Task.FromResult(Result.Success())));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultWithNoResultFunction_ThrowsException()
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
    public async Task IfErrorAsync_SuccessfulResultWithResultFunction_DoesNotCallFunction()
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
    public async Task IfErrorAsync_SuccessfulResultWithResultFunction_ReturnsOriginalResult()
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
    public async Task IfErrorAsync_ErrorResultWithSuccessResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        var asyncFuncResult = Result.Success();

        var sut = GetErrorResult();

        // Act
        var result = await sut.IfErrorAsync(_ => Task.FromResult(asyncFuncResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(asyncFuncResult);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultWithErrorResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        var asyncFuncError = GetReplacementError();

        var sut = GetErrorResult();

        // Act
        var result = await sut.IfErrorAsync(_ => Task.FromResult(Result.FromError(asyncFuncError)));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(asyncFuncError);
    }

    #endregion

    #region IfErrorAsync (Result<T>, Func<Error, Task>)

    [Fact]
    public async Task IfErrorAsync_NoGenericResultWithAction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfErrorAsync(_ => Task.CompletedTask));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultWithNoAction_ThrowsException()
    {
        // Arrange
        const int InitialValue = 24;
        const Func<Error, Task>? AsyncAction = null;

        Result<int> sut = InitialValue;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(AsyncAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task>? AsyncAction = null;

        var sut = GetErrorResult<int>();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(AsyncAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultWithAction_DoesNotCallAction()
    {
        // Arrange
        const int InitialValue = 29;
        var actionMock = new Mock<Func<Error, Task>>();

        Result<int> sut = InitialValue;

        // Act
        await sut.IfErrorAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultWithAction_ReturnsOriginalValueResult()
    {
        // Arrange
        const int InitialValue = 38;

        Result<int> sut = InitialValue;

        // Act
        var result = await sut.IfErrorAsync(_ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(InitialValue);
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
    public async Task IfErrorAsync_ErrorGenericResultWithAction_ReturnsOriginalValueResultWithError()
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
    public async Task IfErrorAsync_NoGenericResultWithGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfErrorAsync(_ => Task.FromResult(Result.FromValue(1))));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const int InitialValue = 24;
        const Func<Error, Task<Result<int>>>? Function = null;

        Result<int> sut = InitialValue;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultWithNoGenericResultFunction_ThrowsException()
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
    public async Task IfErrorAsync_SuccessfulGenericResultWithGenericResultFunction_DoesNotCallFunction()
    {
        // Arrange
        const int InitialValue = 42;
        var functionMock = new Mock<Func<Error, Task<Result<int>>>>();

        Result<int> sut = InitialValue;

        // Act
        await sut.IfErrorAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultWithGenericResultFunction_ReturnsOriginalValueResult()
    {
        // Arrange
        const int InitialValue = 11;

        Result<int> sut = InitialValue;

        // Act
        var result = await sut.IfErrorAsync(_ => Task.FromResult(Result.FromValue(2)));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
        result.Value.ShouldBe(InitialValue);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultWithGenericResultFunction_CallsFunction()
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
    public async Task IfErrorAsync_ErrorGenericResultWithGenericResultFunction_ReturnsFunctionValueResult()
    {
        // Arrange
        var asyncFuncResult = Result.FromValue(58);

        var sut = GetErrorResult<int>();

        // Act
        var result = await sut.IfErrorAsync(_ => Task.FromResult(asyncFuncResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(asyncFuncResult);
        result.Value.ShouldBe(58);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultWithGenericResultFunction_ReturnsFunctionValueResultWithError()
    {
        // Arrange
        var asyncFuncError = GetReplacementError();

        var sut = GetErrorResult<int>();

        // Act
        var result = await sut.IfErrorAsync(_ => Task.FromResult(Result<int>.FromError(asyncFuncError)));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(asyncFuncError);
    }

    #endregion

    #region IfErrorAsync (Task<Result>, Func<Error, Task>)

    [Fact]
    public async Task IfErrorAsync_NoResultTaskWithAction_ThrowsException()
    {
        // Arrange
        const Task<Result>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfErrorAsync(_ => Task.CompletedTask));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultTaskWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task>? AsyncAction = null;

        var sut = Task.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(AsyncAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultTaskWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task>? AsyncAction = null;

        var sut = Task.FromResult(GetErrorResult());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(AsyncAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultTaskWithAction_DoesNotCallAction()
    {
        // Arrange
        var actionMock = new Mock<Func<Error, Task>>();

        var sut = Task.FromResult(Result.Success());

        // Act
        await sut.IfErrorAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultTaskWithAction_ReturnsOriginalResult()
    {
        // Arrange
        var successResult = Result.Success();
        var sut = Task.FromResult(successResult);

        // Act
        var result = await sut.IfErrorAsync(_ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(successResult);
        result.IsSuccess().ShouldBeTrue();
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultTaskWithAction_CallsAction()
    {
        // Arrange
        var actionMock = new Mock<Func<Error, Task>>();

        var errorResult = GetErrorResult();
        var sut = Task.FromResult(errorResult);

        // Act
        await sut.IfErrorAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultTaskWithAction_ReturnsOriginalResult()
    {
        // Arrange
        var errorResult = GetErrorResult();
        var sut = Task.FromResult(errorResult);

        // Act
        var result = await sut.IfErrorAsync(_ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(errorResult);
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(errorResult.Error);
    }

    #endregion

    #region IfErrorAsync (Task<Result>, Func<Error, Task<Result>>)

    [Fact]
    public async Task IfErrorAsync_NoResultTaskWithResultFunction_ThrowsException()
    {
        // Arrange
        const Task<Result>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfErrorAsync(_ => Task.FromResult(Result.Success())));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultTaskWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task<Result>>? Function = null;

        var sut = Task.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultTaskWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task<Result>>? Function = null;

        var sut = Task.FromResult(GetErrorResult());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultTaskWithResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Error, Task<Result>>>();

        var sut = Task.FromResult(Result.Success());

        // Act
        await sut.IfErrorAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulResultTaskWithResultFunction_ReturnsOriginalResult()
    {
        // Arrange
        var successResult = Result.Success();
        var sut = Task.FromResult(successResult);

        // Act
        var result = await sut.IfErrorAsync(_ => Task.FromResult(Result.Success()));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(successResult);
        result.IsSuccess().ShouldBeTrue();
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultTaskWithResultFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Error, Task<Result>>>();

        var errorResult = GetErrorResult();
        var sut = Task.FromResult(errorResult);

        // Act
        await sut.IfErrorAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultTaskWithSuccessResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        var asyncFuncResult = Result.Success();

        var errorResult = GetErrorResult();
        var sut = Task.FromResult(errorResult);

        // Act
        var result = await sut.IfErrorAsync(_ => Task.FromResult(asyncFuncResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(asyncFuncResult);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultTaskWithErrorResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        var asyncFuncError = GetReplacementError();

        var sut = Task.FromResult(GetErrorResult());

        // Act
        var result = await sut.IfErrorAsync(_ => Task.FromResult(Result.FromError(asyncFuncError)));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(asyncFuncError);
    }

    #endregion

    #region IfErrorAsync (Task<Result<T>>, Func<Error, Task>)

    [Fact]
    public async Task IfErrorAsync_NoGenericResultTaskWithAction_ThrowsException()
    {
        // Arrange
        const Task<Result<int>>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfErrorAsync(_ => Task.CompletedTask));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultTaskWithNoAction_ThrowsException()
    {
        // Arrange
        const int InitialValue = 24;
        const Func<Error, Task>? AsyncAction = null;

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(AsyncAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultTaskWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Error, Task>? AsyncAction = null;

        var sut = Task.FromResult(GetErrorResult<int>());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(AsyncAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultTaskWithAction_DoesNotCallAction()
    {
        // Arrange
        const int InitialValue = 29;
        var actionMock = new Mock<Func<Error, Task>>();

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        await sut.IfErrorAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task IfErrorAsync_SuccessfulGenericResultTaskWithAction_ReturnsOriginalValueResult()
    {
        // Arrange
        const int InitialValue = 38;

        var resultValue = Result.FromValue(InitialValue);
        var sut = Task.FromResult(resultValue);

        // Act
        var result = await sut.IfErrorAsync(_ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(resultValue);
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(InitialValue);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultTaskWithAction_CallsAction()
    {
        // Arrange
        var actionMock = new Mock<Func<Error, Task>>();

        var errorResult = GetErrorResult<int>();
        var sut = Task.FromResult(errorResult);

        // Act
        await sut.IfErrorAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorGenericResultTaskWithAction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var errorResult = GetErrorResult<int>();
        var sut = Task.FromResult(errorResult);

        // Act
        var result = await sut.IfErrorAsync(_ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(errorResult);
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(errorResult.Error);
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
