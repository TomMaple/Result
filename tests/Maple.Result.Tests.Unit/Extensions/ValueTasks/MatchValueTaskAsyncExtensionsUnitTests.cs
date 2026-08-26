// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Result.Extensions.ValueTasks;
using Moq;
using System;
using System.Threading.Tasks;

namespace Maple.Result.Tests.Unit.Extensions.ValueTasks;

public class MatchValueTaskAsyncExtensionsUnitTests
{
    #region MatchAsync (Result, Func<ValueTask>, Func<Error, ValueTask>)

    [Fact]
    public async Task MatchAsync_NoResultWithActions_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception =
            await Record.ExceptionAsync(() => Sut!.MatchAsync(() => ValueTask.CompletedTask, _ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithNoSuccessActionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<ValueTask>? SuccessAction = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(SuccessAction!, _ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessActionAndNoErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask>? ErrorAction = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(() => ValueTask.CompletedTask, ErrorAction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessAndErrorActions_CallsSuccessAction()
    {
        // Arrange
        var successActionMock = new Mock<Func<ValueTask>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(successActionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successActionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessAndErrorActions_DoesNotCallErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(() => ValueTask.CompletedTask, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessAndErrorActions_ReturnsOriginalResult()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = await sut.MatchAsync(() => ValueTask.CompletedTask, _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBe(sut);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithSuccessAndErrorActions_DoesNotCallSuccessAction()
    {
        // Arrange
        var successActionMock = new Mock<Func<ValueTask>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(successActionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successActionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithSuccessAndErrorActions_CallsErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(() => ValueTask.CompletedTask, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithSuccessAndErrorActions_ReturnsOriginalResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = await sut.MatchAsync(() => ValueTask.CompletedTask, _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldBe(sut);
    }

    #endregion

    #region MatchAsync (Result, Func<ValueTask<Result>>, Func<Error, ValueTask>)

    [Fact]
    public async Task MatchAsync_NoResultWithSuccessResultFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(
            () => Sut!.MatchAsync(() => ValueTask.FromResult(Result.Success()), _ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithNoSuccessResultFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<ValueTask<Result>>? SuccessFunction = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(SuccessFunction!, _ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessResultFunctionAndNoErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask>? ErrorAction = null;

        var sut = Result.Success();

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(() => ValueTask.FromResult(Result.Success()), ErrorAction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessResultFunctionAndErrorAction_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<Result>>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessResultFunctionAndErrorAction_DoesNotCallErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(Result.Success()), errorActionMock.Object);

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
        var result = await sut.MatchAsync(() => ValueTask.FromResult(successFunctionResult), _ => ValueTask.CompletedTask);

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
        var result = await sut.MatchAsync(() => ValueTask.FromResult(successFunctionError), _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(successFunctionError);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithSuccessResultFunctionAndErrorAction_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<Result>>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithSuccessResultFunctionAndErrorAction_CallsErrorFunction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(Result.Success()), errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithSuccessResultFunctionAndErrorAction_ReturnsOriginalResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = await sut.MatchAsync(() => ValueTask.FromResult(Result.Success()), _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
    }

    #endregion

    #region MatchAsync (Result, Func<ValueTask<Result>>, Func<Error, ValueTask<Result>>)

    [Fact]
    public async Task MatchAsync_NoResultWithSuccessAndErrorResultFunctions_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(
            () => Sut!.MatchAsync(() => ValueTask.FromResult(Result.Success()), _ => ValueTask.FromResult(Result.Success())).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithNoSuccessResultFunctionAndErrorResultFunction_ThrowsException2()
    {
        // Arrange
        const Func<ValueTask<Result>>? SuccessFunction = null;

        var sut = Result.Success();

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(SuccessFunction!, _ => ValueTask.FromResult(Result.Success())).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessResultFunctionAndNoErrorResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask<Result>>? ErrorFunction = null;

        var sut = Result.Success();

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(() => ValueTask.FromResult(Result.Success()), ErrorFunction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessAndErrorResultFunctions_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<Result>>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(Result.Success()));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithSuccessAndErrorResultFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<Result>>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(Result.Success()), errorFunctionMock.Object);

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
            () => ValueTask.FromResult(successFunctionResult),
            _ => ValueTask.FromResult(Result.Success()));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(successFunctionResult);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithSuccessAndErrorResultFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<Result>>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(Result.Success()));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithSuccessAndErrorResultFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<Result>>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(Result.Success()), errorFunctionMock.Object);

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
            () => ValueTask.FromResult(Result.Success()),
            _ => ValueTask.FromResult(errorFunctionResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(errorFunctionResult);
    }

    #endregion

    #region MatchAsync (Result, Func<ValueTask<T>>, Func<Error, ValueTask>)

    [Fact]
    public async Task MatchAsync_NoResultWithGenericSuccessFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception =
            await Record.ExceptionAsync(() => Sut!.MatchAsync(() => ValueTask.FromResult(1), _ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithNoGenericSuccessFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<ValueTask<int>>? SuccessFunction = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(SuccessFunction!, _ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericSuccessFunctionNoErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask>? ErrorAction = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(() => ValueTask.FromResult(1), ErrorAction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericSuccessFunctionAndErrorAction_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<int>>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericSuccessFunctionAndErrorAction_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(345), errorActionMock.Object);

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
        var result = await sut.MatchAsync(() => ValueTask.FromResult(SuccessFunctionValue), _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(SuccessFunctionValue);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithGenericSuccessFunctionAndErrorAction_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<int>>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithGenericSuccessFunctionAndErrorAction_CallsErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(432), errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithGenericSuccessFunctionAndErrorAction_ReturnsOriginalResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = await sut.MatchAsync(() => ValueTask.FromResult(123), _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region MatchAsync (Result, Func<ValueTask<T>>, Func<Error, ValueTask<T>>)

    [Fact]
    public async Task MatchAsync_NoResultWithGenericSuccessAndErrorFunctions_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception =
            await Record.ExceptionAsync(() => Sut!.MatchAsync(() => ValueTask.FromResult(1), _ => ValueTask.FromResult(2)).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithNoGenericSuccessFunctionAndGenericErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<ValueTask<int>>? Function = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(Function!, _ => ValueTask.FromResult(2)).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericSuccessFunctionAndNoGenericErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask<int>>? ErrorFunction = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(() => ValueTask.FromResult(1), ErrorFunction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericSuccessAndErrorFunctions_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<int>>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(2));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericSuccessAndErrorFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<int>>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(1), errorFunctionMock.Object);

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
        var result = await sut.MatchAsync(() => ValueTask.FromResult(ExpectedValue), _ => ValueTask.FromResult(2));

        // Assert
        result.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithGenericSuccessAndErrorFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<int>>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(2));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithGenericSuccessAndErrorFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<int>>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(1), errorFunctionMock.Object);

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
        var result = await sut.MatchAsync(() => ValueTask.FromResult(1), _ => ValueTask.FromResult(ExpectedValue));

        // Assert
        result.ShouldBe(ExpectedValue);
    }

    #endregion

    #region MatchAsync (Result, Func<ValueTask<Result<T>>>, Func<Error, ValueTask>)

    [Fact]
    public async Task MatchAsync_NoResultWithGenericResultSuccessFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(
            () => Sut!.MatchAsync(() => ValueTask.FromResult(Result.FromValue(1)), _ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithNoGenericResultResultFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<ValueTask<Result<int>>>? SuccessFunction = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(SuccessFunction!, _ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericResultSuccessFunctionAndNoErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask>? ErrorAction = null;

        var sut = Result.Success();

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(() => ValueTask.FromResult(Result.FromValue(1)), ErrorAction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericResultSuccessFunctionAndErrorAction_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<Result<int>>>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericResultSuccessFunctionAndErrorAction_DoesNotCallErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(Result.FromValue(1)), errorActionMock.Object);

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
        var result = await sut.MatchAsync(() => ValueTask.FromResult(expectedResult), _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.ShouldBeSameAs(expectedResult);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithGenericResultSuccessFunctionAndErrorAction_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<Result<int>>>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithGenericResultSuccessFunctionAndErrorAction_CallsErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(Result.FromValue(1)), errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithGenericResultSuccessFunctionAndErrorAction_ReturnsOriginalResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = await sut.MatchAsync(() => ValueTask.FromResult(Result.FromValue(1)), _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region MatchAsync (Result, Func<ValueTask<Result<T>>>, Func<Error, ValueTask<Result<T>>>)

    [Fact]
    public async Task MatchAsync_NoResultWithGenericResultSuccessAndErrorFunctions_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(
            () => Sut!.MatchAsync(() => ValueTask.FromResult(Result.FromValue(1)), _ => ValueTask.FromResult(Result.FromValue(2))).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithNoGenericResultSuccessFunctionAndGenericResultErrorFunction_ThrowsException3()
    {
        // Arrange
        const Func<ValueTask<Result<int>>>? Function = null;

        var sut = Result.Success();

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(Function!, _ => ValueTask.FromResult(Result.FromValue(2))).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericResultSuccessFunctionAndNoGenericResultErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask<Result<int>>>? ErrorFunction = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(
            () => sut.MatchAsync(() => ValueTask.FromResult(Result.FromValue(1)), ErrorFunction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericResultSuccessAndErrorFunctions_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<Result<int>>>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(Result.FromValue(2)));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultWithGenericResultSuccessAndErrorFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<Result<int>>>>();

        var sut = Result.Success();

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(Result.FromValue(1)), errorFunctionMock.Object);

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
            () => ValueTask.FromResult(expectedResult),
            _ => ValueTask.FromResult(Result.FromValue(2)));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.ShouldBeSameAs(expectedResult);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithGenericResultSuccessAndErrorFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<Result<int>>>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(Result.FromValue(2)));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultWithGenericResultSuccessAndErrorFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<Result<int>>>>();

        var sut = GetErrorResult();

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(Result.FromValue(1)), errorFunctionMock.Object);

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
            () => ValueTask.FromResult(Result.FromValue(1)),
            _ => ValueTask.FromResult(expectedResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(expectedResult);
    }

    #endregion

    #region MatchAsync (Result<T>, Func<T, ValueTask>, Func<Error, ValueTask>)

    [Fact]
    public async Task MatchAsync_NoGenericResultWithSuccessAndErrorActions_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception =
            await Record.ExceptionAsync(() => Sut!.MatchAsync(_ => ValueTask.CompletedTask, _ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithNoSuccessActionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<int, ValueTask>? SuccessAction = null;

        Result<int> sut = 1;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(SuccessAction!, _ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithSuccessActionAndNoErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask>? ErrorAction = null;

        Result<int> sut = 1;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(_ => ValueTask.CompletedTask, ErrorAction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithSuccessAndErrorActions_CallsSuccessAction()
    {
        // Arrange
        var successActionMock = new Mock<Func<int, ValueTask>>();

        const int Value = 25;
        Result<int> sut = Value;

        // Act
        await sut.MatchAsync(successActionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successActionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithSuccessAndErrorActions_DoesNotCallErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        const int Value = 25;
        Result<int> sut = Value;

        // Act
        await sut.MatchAsync(_ => ValueTask.CompletedTask, errorActionMock.Object);

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
        var result = await sut.MatchAsync(_ => ValueTask.CompletedTask, _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.ShouldBeSameAs(sut);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithSuccessAndErrorActions_DoesNotCallSuccessAction()
    {
        // Arrange
        var successActionMock = new Mock<Func<int, ValueTask>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.MatchAsync(successActionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successActionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithSuccessAndErrorActions_CallsErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.MatchAsync(_ => ValueTask.CompletedTask, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithSuccessAndErrorActions_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = await sut.MatchAsync(_ => ValueTask.CompletedTask, _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.ShouldBeSameAs(sut);
    }

    #endregion

    #region MatchAsync (Result<T>, Func<T, ValueTask<TNext>>, Func<Error, ValueTask>)

    [Fact]
    public async Task MatchAsync_NoGenericResultWithGenericSuccessFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.MatchAsync(ValueTask.FromResult, _ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithNoGenericSuccessFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<int, ValueTask<int>>? Transform = null;

        Result<int> sut = 1;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(Transform!, _ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericSuccessFunctionAndNoErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask>? ErrorAction = null;

        Result<int> sut = 1;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(ValueTask.FromResult, ErrorAction!).AsTask());

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
        var successFunctionMock = new Mock<Func<int, ValueTask<int>>>();

        Result<int> sut = Value;

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericSuccessFunctionAndErrorAction_DoesNotCallErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        Result<int> sut = 15;

        // Act
        await sut.MatchAsync(ValueTask.FromResult, errorActionMock.Object);

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
        var result = await sut.MatchAsync(x => ValueTask.FromResult(2 * x), _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithGenericSuccessFunctionAndErrorAction_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<int, ValueTask<int>>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithGenericSuccessFunctionAndErrorAction_CallsErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.MatchAsync(ValueTask.FromResult, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithGenericSuccessFunctionAndErrorAction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = await sut.MatchAsync(ValueTask.FromResult, _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region MatchAsync (Result<T>, Func<T, ValueTask<TNext>>, Func<Error, ValueTask<TNext>>)

    [Fact]
    public async Task MatchAsync_NoGenericResultWithGenericSuccessAndErrorFunctions_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(
            () => Sut!.MatchAsync(x => ValueTask.FromResult(x.ToString()), _ => ValueTask.FromResult("error")).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithNoGenericSuccessFunctionAndGenericErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<int, ValueTask<string>>? SuccessFunction = null;

        Result<int> sut = 1;

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(SuccessFunction!, _ => ValueTask.FromResult("error")).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericSuccessFunctionAndNoGenericErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask<string>>? ErrorFunction = null;

        Result<int> sut = 1;

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(x => ValueTask.FromResult(x.ToString()), ErrorFunction!).AsTask());

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
        var successFunctionMock = new Mock<Func<int, ValueTask<string>>>();
        successFunctionMock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .ReturnsAsync("success value");

        Result<int> sut = Value;

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult("error"));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericSuccessAndErrorFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<string>>>();

        Result<int> sut = 5;

        // Act
        await sut.MatchAsync(x => ValueTask.FromResult(x.ToString()), errorFunctionMock.Object);

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
        var result = await sut.MatchAsync(x => ValueTask.FromResult((x + 1).ToString()), _ => ValueTask.FromResult("error"));

        // Assert
        result.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithWithGenericSuccessAndErrorFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<int, ValueTask<string>>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult("error"));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithGenericSuccessAndErrorFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<string>>>();
        errorFunctionMock
            .Setup(x => x.Invoke(It.IsAny<Error>()))
            .ReturnsAsync("error value");

        var sut = GetErrorResult<int>();

        // Act
        await sut.MatchAsync(x => ValueTask.FromResult(x.ToString()), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithGenericSuccessAndErrorFunctions_ReturnsErrorFunctionValue()
    {
        // Arrange
        const string ExpectedValue = "Error function value";

        var sut = GetErrorResult<int>();

        // Act
        var result = await sut.MatchAsync(x => ValueTask.FromResult(x.ToString()), _ => ValueTask.FromResult(ExpectedValue));

        // Assert
        result.ShouldBe(ExpectedValue);
    }

    #endregion

    #region MatchAsync (Result<T>, Func<T, ValueTask<Result<TNext>>>, Func<Error, ValueTask>)

    [Fact]
    public async Task MatchAsync_NoGenericResultWithGenericResultSuccessFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(
            () => Sut!.MatchAsync(x => ValueTask.FromResult(Result.FromValue(x.ToString())), _ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithNoGenericResultSuccessFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<int, ValueTask<Result<string>>>? SuccessFunction = null;

        Result<int> sut = 1;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(SuccessFunction!, _ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericResultSuccessFunctionAndNoErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask>? ErrorAction = null;

        Result<int> sut = 1;

        // Act
        var exception = await Record.ExceptionAsync(
            () => sut.MatchAsync(x => ValueTask.FromResult(Result.FromValue(x.ToString())), ErrorAction!).AsTask());

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
        var successFunctionMock = new Mock<Func<int, ValueTask<Result<string>>>>();

        Result<int> sut = 88;

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericResultSuccessFunctionAndErrorAction_DoesNotCallErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        Result<int> sut = 88;

        // Act
        await sut.MatchAsync(x => ValueTask.FromResult(Result.FromValue(x.ToString())), errorActionMock.Object);

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
            x => ValueTask.FromResult(Result.FromValue((x + 2).ToString())),
            _ => ValueTask.CompletedTask);

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
        var successFunctionMock = new Mock<Func<int, ValueTask<Result<string>>>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithGenericResultSuccessFunctionAndErrorAction_CallsErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.MatchAsync(x => ValueTask.FromResult(Result.FromValue(x.ToString())), errorActionMock.Object);

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
            await sut.MatchAsync(x => ValueTask.FromResult(Result.FromValue(x.ToString())), _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region MatchAsync (Result<T>, Func<T, ValueTask<Result<TNext>>>, Func<Error, ValueTask<Result<TNext>>>)

    [Fact]
    public async Task MatchAsync_NoGenericResultWithGenericResultSuccessAndErrorFunctions_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(
            () => Sut!.MatchAsync(
                x => ValueTask.FromResult(Result.FromValue(x.ToString())),
                _ => ValueTask.FromResult(Result.FromValue("error value"))).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithNoGenericResultSuccessFunctionAndGenericResultErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<int, ValueTask<Result<string>>>? SuccessFunction = null;

        Result<int> sut = 1;

        // Act
        var exception = await Record.ExceptionAsync(
            () => sut.MatchAsync(SuccessFunction!, _ => ValueTask.FromResult(Result.FromValue("error value"))).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericResultSuccessFunctionAndNoGenericResultErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask<Result<string>>>? ErrorFunction = null;

        Result<int> sut = 1;

        // Act
        var exception = await Record.ExceptionAsync(
            () => sut.MatchAsync(x => ValueTask.FromResult(Result.FromValue(x.ToString())), ErrorFunction!).AsTask());

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
        var successFunctionMock = new Mock<Func<int, ValueTask<Result<string>>>>();

        Result<int> sut = Value;

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(Result.FromValue("error value")));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultWithGenericResultSuccessAndErrorFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<Result<string>>>>();

        Result<int> sut = 90;

        // Act
        await sut.MatchAsync(x => ValueTask.FromResult(Result.FromValue(x.ToString())), errorFunctionMock.Object);

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
            x => ValueTask.FromResult(Result.FromValue($"-{x}-")),
            _ => ValueTask.FromResult(Result.FromValue("error value")));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithGenericResultSuccessAndErrorFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<int, ValueTask<Result<string>>>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(Result.FromValue("error value")));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultWithGenericResultSuccessAndErrorFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<Result<string>>>>();

        var sut = GetErrorResult<int>();

        // Act
        await sut.MatchAsync(x => ValueTask.FromResult(Result.FromValue(x.ToString())), errorFunctionMock.Object);

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
            x => ValueTask.FromResult(Result.FromValue(x.ToString())),
            _ => ValueTask.FromResult(Result.FromValue(ExpectedValue)));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    #endregion

    #region MatchAsync (ValueTask<Result>, Func<ValueTask>, Func<Error, ValueTask>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithNoSuccessActionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<ValueTask>? SuccessAction = null;

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(SuccessAction!, _ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithSuccessActionAndNoErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask>? ErrorAction = null;

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(() => ValueTask.CompletedTask, ErrorAction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithSuccessAndErrorActions_CallsSuccessAction()
    {
        // Arrange
        var successActionMock = new Mock<Func<ValueTask>>();

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        await sut.MatchAsync(successActionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successActionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithSuccessAndErrorActions_DoesNotCallErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        await sut.MatchAsync(() => ValueTask.CompletedTask, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithSuccessAndErrorActions_ReturnsOriginalResult()
    {
        // Arrange
        var initialResult = Result.Success();
        var sut = ValueTask.FromResult(initialResult);

        // Act
        var result = await sut.MatchAsync(() => ValueTask.CompletedTask, _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBe(initialResult);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithSuccessAndErrorActions_DoesNotCallSuccessAction()
    {
        // Arrange
        var successActionMock = new Mock<Func<ValueTask>>();

        var sut = ValueTask.FromResult(GetErrorResult());

        // Act
        await sut.MatchAsync(successActionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successActionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithSuccessAndErrorActions_CallsErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var errorResult = GetErrorResult();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        await sut.MatchAsync(() => ValueTask.CompletedTask, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithSuccessAndErrorActions_ReturnsOriginalResult()
    {
        // Arrange
        var errorResult = GetErrorResult();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        var result = await sut.MatchAsync(() => ValueTask.CompletedTask, _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldBe(errorResult);
    }

    #endregion

    #region MatchAsync (ValueTask<Result>, Func<ValueTask<Result>>, Func<Error, ValueTask>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithNoSuccessResultFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<ValueTask<Result>>? SuccessFunction = null;

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(SuccessFunction!, _ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithSuccessResultFunctionAndNoErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask>? ErrorAction = null;

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(() => ValueTask.FromResult(Result.Success()), ErrorAction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithSuccessResultFunctionAndErrorAction_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<Result>>>();

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithSuccessResultFunctionAndErrorAction_DoesNotCallErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(Result.Success()), errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithSuccessResultFunctionAndErrorAction_ReturnsSuccessFunctionResult()
    {
        // Arrange
        var successFunctionResult = Result.Success();

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var result = await sut.MatchAsync(() => ValueTask.FromResult(successFunctionResult), _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(successFunctionResult);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithSuccessErrorResultFunctionAndErrorAction_ReturnsSuccessFunctionResult()
    {
        // Arrange
        var successFunctionError = GetErrorResult();

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var result = await sut.MatchAsync(() => ValueTask.FromResult(successFunctionError), _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(successFunctionError);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithSuccessResultFunctionAndErrorAction_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<Result>>>();

        var sut = ValueTask.FromResult(GetErrorResult());

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithSuccessResultFunctionAndErrorAction_CallsErrorFunction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var errorResult = GetErrorResult();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(Result.Success()), errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithSuccessResultFunctionAndErrorAction_ReturnsOriginalResult()
    {
        // Arrange
        var errorResult = GetErrorResult();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        var result = await sut.MatchAsync(() => ValueTask.FromResult(Result.Success()), _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(errorResult);
    }

    #endregion

    #region MatchAsync (ValueTask<Result>, Func<ValueTask<Result>>, Func<Error, ValueTask<Result>>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithNoSuccessResultFunctionAndErrorResultFunction_ThrowsException2()
    {
        // Arrange
        const Func<ValueTask<Result>>? SuccessFunction = null;

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(SuccessFunction!, _ => ValueTask.FromResult(Result.Success())).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithSuccessResultFunctionAndNoErrorResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask<Result>>? ErrorFunction = null;

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(() => ValueTask.FromResult(Result.Success()), ErrorFunction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithSuccessAndErrorResultFunctions_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<Result>>>();

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(Result.Success()));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithSuccessAndErrorResultFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<Result>>>();

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(Result.Success()), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithSuccessAndErrorResultFunctions_ReturnsSuccessFunctionResult()
    {
        // Arrange
        var successFunctionResult = Result.Success();

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var result = await sut.MatchAsync(
            () => ValueTask.FromResult(successFunctionResult),
            _ => ValueTask.FromResult(Result.Success()));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(successFunctionResult);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithSuccessAndErrorResultFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<Result>>>();

        var sut = ValueTask.FromResult(GetErrorResult());

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(Result.Success()));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithSuccessAndErrorResultFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<Result>>>();

        var errorResult = GetErrorResult();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(Result.Success()), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithSuccessAndErrorResultFunctions_ReturnsErrorFunctionResult()
    {
        // Arrange
        var errorFunctionResult = Result.Success();

        var sut = ValueTask.FromResult(GetErrorResult());

        // Act
        var result = await sut.MatchAsync(
            () => ValueTask.FromResult(Result.Success()),
            _ => ValueTask.FromResult(errorFunctionResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(errorFunctionResult);
    }

    #endregion

    #region MatchAsync (ValueTask<Result>, Func<ValueTask<T>>, Func<Error, ValueTask>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithNoGenericSuccessFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<ValueTask<int>>? SuccessFunction = null;

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(SuccessFunction!, _ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithGenericSuccessFunctionNoErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask>? ErrorAction = null;

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(() => ValueTask.FromResult(1), ErrorAction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithGenericSuccessFunctionAndErrorAction_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<int>>>();

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithGenericSuccessFunctionAndErrorAction_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(345), errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithGenericSuccessFunctionAndErrorAction_ReturnsSuccessFunctionValue()
    {
        // Arrange
        const int SuccessFunctionValue = 42;

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var result = await sut.MatchAsync(() => ValueTask.FromResult(SuccessFunctionValue), _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(SuccessFunctionValue);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithGenericSuccessFunctionAndErrorAction_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<int>>>();

        var sut = ValueTask.FromResult(GetErrorResult());

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithGenericSuccessFunctionAndErrorAction_CallsErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var errorResult = GetErrorResult();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(432), errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithGenericSuccessFunctionAndErrorAction_ReturnsOriginalResult()
    {
        // Arrange
        var errorResult = GetErrorResult();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        var result = await sut.MatchAsync(() => ValueTask.FromResult(123), _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(errorResult.Error);
    }

    #endregion

    #region MatchAsync (ValueTask<Result>, Func<ValueTask<T>>, Func<Error, ValueTask<T>>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithNoGenericSuccessFunctionAndGenericErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<ValueTask<int>>? Function = null;

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(Function!, _ => ValueTask.FromResult(2)).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithGenericSuccessFunctionAndNoGenericErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask<int>>? ErrorFunction = null;

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(() => ValueTask.FromResult(1), ErrorFunction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithGenericSuccessAndErrorFunctions_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<int>>>();

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(2));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithGenericSuccessAndErrorFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<int>>>();

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(1), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithGenericSuccessAndErrorFunctions_ReturnsSuccessFunctionValue()
    {
        // Arrange
        const int ExpectedValue = 58;

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var result = await sut.MatchAsync(() => ValueTask.FromResult(ExpectedValue), _ => ValueTask.FromResult(2));

        // Assert
        result.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithGenericSuccessAndErrorFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<int>>>();

        var sut = ValueTask.FromResult(GetErrorResult());

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(2));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithGenericSuccessAndErrorFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<int>>>();

        var errorResult = GetErrorResult();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(1), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithGenericSuccessAndErrorFunctions_ReturnsErrorFunctionValue()
    {
        // Arrange
        const int ExpectedValue = 73;

        var sut = ValueTask.FromResult(GetErrorResult());

        // Act
        var result = await sut.MatchAsync(() => ValueTask.FromResult(1), _ => ValueTask.FromResult(ExpectedValue));

        // Assert
        result.ShouldBe(ExpectedValue);
    }

    #endregion

    #region MatchAsync (ValueTask<Result>, Func<ValueTask<Result<T>>>, Func<Error, ValueTask>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithNoGenericResultResultFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<ValueTask<Result<int>>>? SuccessFunction = null;

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(SuccessFunction!, _ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithGenericResultSuccessFunctionAndNoErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask>? ErrorAction = null;

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(() => ValueTask.FromResult(Result.FromValue(1)), ErrorAction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithGenericResultSuccessFunctionAndErrorAction_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<Result<int>>>>();

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithGenericResultSuccessFunctionAndErrorAction_DoesNotCallErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(Result.FromValue(1)), errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithGenericResultSuccessFunctionAndErrorAction_ReturnsSuccessFunctionResultValue()
    {
        // Arrange
        var expectedResult = Result.FromValue(12);

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var result = await sut.MatchAsync(() => ValueTask.FromResult(expectedResult), _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.ShouldBeSameAs(expectedResult);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithGenericResultSuccessFunctionAndErrorAction_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<Result<int>>>>();

        var sut = ValueTask.FromResult(GetErrorResult());

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithGenericResultSuccessFunctionAndErrorAction_CallsErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var errorResult = GetErrorResult();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(Result.FromValue(1)), errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithGenericResultSuccessFunctionAndErrorAction_ReturnsOriginalResult()
    {
        // Arrange
        var errorResult = GetErrorResult();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        var result = await sut.MatchAsync(() => ValueTask.FromResult(Result.FromValue(1)), _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(errorResult.Error);
    }

    #endregion

    #region MatchAsync (ValueTask<Result>, Func<ValueTask<Result<T>>>, Func<Error, ValueTask<Result<T>>>)

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithNoGenericResultSuccessFunctionAndGenericResultErrorFunction_ThrowsException3()
    {
        // Arrange
        const Func<ValueTask<Result<int>>>? Function = null;

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(Function!, _ => ValueTask.FromResult(Result.FromValue(2))).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithGenericResultSuccessFunctionAndNoGenericResultErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask<Result<int>>>? ErrorFunction = null;

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(
            () => sut.MatchAsync(() => ValueTask.FromResult(Result.FromValue(1)), ErrorFunction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithGenericResultSuccessAndErrorFunctions_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<Result<int>>>>();

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(Result.FromValue(2)));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithGenericResultSuccessAndErrorFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<Result<int>>>>();

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(Result.FromValue(1)), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulResultValueTaskWithGenericResultSuccessAndErrorFunctions_ReturnsSuccessFunctionResult()
    {
        // Arrange
        var expectedResult = Result.FromValue(9);

        var sut = ValueTask.FromResult(Result.Success());

        // Act
        var result = await sut.MatchAsync(
            () => ValueTask.FromResult(expectedResult),
            _ => ValueTask.FromResult(Result.FromValue(2)));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.ShouldBeSameAs(expectedResult);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithGenericResultSuccessAndErrorFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<ValueTask<Result<int>>>>();

        var sut = ValueTask.FromResult(GetErrorResult());

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(Result.FromValue(2)));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithGenericResultSuccessAndErrorFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<Result<int>>>>();

        var errorResult = GetErrorResult();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        await sut.MatchAsync(() => ValueTask.FromResult(Result.FromValue(1)), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorResultValueTaskWithGenericResultSuccessAndErrorFunctions_ReturnsErrorFunctionValueResult()
    {
        // Arrange
        var expectedResult = Result.FromValue(15);

        var sut = ValueTask.FromResult(GetErrorResult());

        // Act
        var result = await sut.MatchAsync(
            () => ValueTask.FromResult(Result.FromValue(1)),
            _ => ValueTask.FromResult(expectedResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(expectedResult);
    }

    #endregion

    #region MatchAsync (ValueTask<Result<T>>, Func<T, ValueTask>, Func<Error, ValueTask>)

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithNoSuccessActionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<int, ValueTask>? SuccessAction = null;

        const int Value = 68;
        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(SuccessAction!, _ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithSuccessActionAndNoErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask>? ErrorAction = null;

        const int Value = 68;
        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(_ => ValueTask.CompletedTask, ErrorAction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithSuccessAndErrorActions_CallsSuccessAction()
    {
        // Arrange
        var successActionMock = new Mock<Func<int, ValueTask>>();

        const int Value = 25;
        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        await sut.MatchAsync(successActionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successActionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithSuccessAndErrorActions_DoesNotCallErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        const int Value = 25;
        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        await sut.MatchAsync(_ => ValueTask.CompletedTask, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithSuccessAndErrorActions_ReturnsOriginalValueResult()
    {
        // Arrange
        const int Value = 25;
        var initialResult = Result.FromValue(Value);
        var sut = ValueTask.FromResult(initialResult);

        // Act
        var result = await sut.MatchAsync(_ => ValueTask.CompletedTask, _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.ShouldBeSameAs(initialResult);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultValueTaskWithSuccessAndErrorActions_DoesNotCallSuccessAction()
    {
        // Arrange
        var successActionMock = new Mock<Func<int, ValueTask>>();

        var sut = ValueTask.FromResult(GetErrorResult<int>());

        // Act
        await sut.MatchAsync(successActionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successActionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultValueTaskWithSuccessAndErrorActions_CallsErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var errorResult = GetErrorResult<int>();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        await sut.MatchAsync(_ => ValueTask.CompletedTask, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultValueTaskWithSuccessAndErrorActions_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var errorResult = GetErrorResult<int>();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        var result = await sut.MatchAsync(_ => ValueTask.CompletedTask, _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.ShouldBeSameAs(errorResult);
    }

    #endregion

    #region MatchAsync (ValueTask<Result<T>>, Func<T, ValueTask<TNext>>, Func<Error, ValueTask>)

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithNoGenericSuccessFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<int, ValueTask<int>>? Transform = null;

        const int Value = 39;
        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(Transform!, _ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithGenericSuccessFunctionAndNoErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Error, ValueTask>? ErrorAction = null;

        const int Value = 39;
        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(ValueTask.FromResult, ErrorAction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithGenericSuccessFunctionAndErrorAction_CallsSuccessFunction()
    {
        // Arrange
        const int Value = 15;
        var successFunctionMock = new Mock<Func<int, ValueTask<int>>>();

        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithGenericSuccessFunctionAndErrorAction_DoesNotCallErrorAction()
    {
        // Arrange
        const int Value = 15;
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        await sut.MatchAsync(ValueTask.FromResult, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithGenericSuccessFunctionAndErrorAction_ReturnsSuccessFunctionValue()
    {
        // Arrange
        const int Value = 15;
        const int ExpectedValue = 30;

        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        var result = await sut.MatchAsync(x => ValueTask.FromResult(2 * x), _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultValueTaskWithGenericSuccessFunctionAndErrorAction_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<int, ValueTask<int>>>();

        var sut = ValueTask.FromResult(GetErrorResult<int>());

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultValueTaskWithGenericSuccessFunctionAndErrorAction_CallsErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var errorResult = GetErrorResult<int>();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        await sut.MatchAsync(ValueTask.FromResult, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultValueTaskWithGenericSuccessFunctionAndErrorAction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var errorResult = GetErrorResult<int>();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        var result = await sut.MatchAsync(ValueTask.FromResult, _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(errorResult.Error);
    }

    #endregion

    #region MatchAsync (ValueTask<Result<T>>, Func<T, ValueTask<TNext>>, Func<Error, ValueTask<TNext>>)

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithNoGenericSuccessFunctionAndGenericErrorFunction_ThrowsException()
    {
        // Arrange
        const int Value = 17;
        const Func<int, ValueTask<string>>? SuccessFunction = null;

        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(SuccessFunction!, _ => ValueTask.FromResult("error")).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithGenericSuccessFunctionAndNoGenericErrorFunction_ThrowsException()
    {
        // Arrange
        const int Value = 17;
        const Func<Error, ValueTask<string>>? ErrorFunction = null;

        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        var exception =
            await Record.ExceptionAsync(() => sut.MatchAsync(x => ValueTask.FromResult(x.ToString()), ErrorFunction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithGenericSuccessAndErrorFunctions_CallsSuccessFunction()
    {
        // Arrange
        const int Value = 17;
        var successFunctionMock = new Mock<Func<int, ValueTask<string>>>();
        successFunctionMock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .ReturnsAsync("success value");

        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult("error"));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithGenericSuccessAndErrorFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        const int Value = 17;
        var errorFunctionMock = new Mock<Func<Error, ValueTask<string>>>();

        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        await sut.MatchAsync(x => ValueTask.FromResult(x.ToString()), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithGenericSuccessAndErrorFunctions_ReturnsSuccessFunctionValue()
    {
        // Arrange
        const int Value = 17;
        const string ExpectedValue = "18";

        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        var result = await sut.MatchAsync(x => ValueTask.FromResult((x + 1).ToString()), _ => ValueTask.FromResult("error"));

        // Assert
        result.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultValueTaskWithWithGenericSuccessAndErrorFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<int, ValueTask<string>>>();

        var sut = ValueTask.FromResult(GetErrorResult<int>());

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult("error"));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultValueTaskWithGenericSuccessAndErrorFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<string>>>();
        errorFunctionMock
            .Setup(x => x.Invoke(It.IsAny<Error>()))
            .ReturnsAsync("error value");

        var errorResult = GetErrorResult<int>();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        await sut.MatchAsync(x => ValueTask.FromResult(x.ToString()), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultValueTaskWithGenericSuccessAndErrorFunctions_ReturnsErrorFunctionValue()
    {
        // Arrange
        const string ExpectedValue = "Error function value";

        var sut = ValueTask.FromResult(GetErrorResult<int>());

        // Act
        var result = await sut.MatchAsync(x => ValueTask.FromResult(x.ToString()), _ => ValueTask.FromResult(ExpectedValue));

        // Assert
        result.ShouldBe(ExpectedValue);
    }

    #endregion

    #region MatchAsync (ValueTask<Result<T>>, Func<T, ValueTask<Result<TNext>>>, Func<Error, ValueTask>)

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithNoGenericResultSuccessFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const int Value = 88;
        const Func<int, ValueTask<Result<string>>>? SuccessFunction = null;

        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(SuccessFunction!, _ => ValueTask.CompletedTask).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithGenericResultSuccessFunctionAndNoErrorAction_ThrowsException()
    {
        // Arrange
        const int Value = 88;
        const Func<Error, ValueTask>? ErrorAction = null;

        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        var exception = await Record.ExceptionAsync(
            () => sut.MatchAsync(x => ValueTask.FromResult(Result.FromValue(x.ToString())), ErrorAction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithGenericResultSuccessFunctionAndErrorAction_CallsSuccessFunction()
    {
        // Arrange
        const int Value = 88;
        var successFunctionMock = new Mock<Func<int, ValueTask<Result<string>>>>();

        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithGenericResultSuccessFunctionAndErrorAction_DoesNotCallErrorAction()
    {
        // Arrange
        const int Value = 88;
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        await sut.MatchAsync(x => ValueTask.FromResult(Result.FromValue(x.ToString())), errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithGenericResultSuccessFunctionAndErrorAction_ReturnsSuccessFunctionValueResult()
    {
        // Arrange
        const string ExpectedValue = "90";
        const int Value = 88;
        var expectedResult = Result.FromValue(ExpectedValue);

        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        var result = await sut.MatchAsync(
            x => ValueTask.FromResult(Result.FromValue((x + 2).ToString())),
            _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.ShouldBe(expectedResult);
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultValueTaskWithGenericResultSuccessFunctionAndErrorAction_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<int, ValueTask<Result<string>>>>();

        var sut = ValueTask.FromResult(GetErrorResult<int>());

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.CompletedTask);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultValueTaskWithGenericResultSuccessFunctionAndErrorAction_CallsErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Func<Error, ValueTask>>();

        var errorResult = GetErrorResult<int>();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        await sut.MatchAsync(x => ValueTask.FromResult(Result.FromValue(x.ToString())), errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultValueTaskWithGenericResultSuccessFunctionAndErrorAction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var errorResult = GetErrorResult<int>();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        var result =
            await sut.MatchAsync(x => ValueTask.FromResult(Result.FromValue(x.ToString())), _ => ValueTask.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(errorResult.Error);
    }

    #endregion

    #region MatchAsync (ValueTask<Result<T>>, Func<T, ValueTask<Result<TNext>>>, Func<Error, ValueTask<Result<TNext>>>)

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithNoGenericResultSuccessFunctionAndGenericResultErrorFunction_ThrowsException()
    {
        // Arrange
        const int Value = 90;
        const Func<int, ValueTask<Result<string>>>? SuccessFunction = null;

        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        var exception = await Record.ExceptionAsync(
            () => sut.MatchAsync(SuccessFunction!, _ => ValueTask.FromResult(Result.FromValue("error value"))).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithGenericResultSuccessFunctionAndNoGenericResultErrorFunction_ThrowsException()
    {
        // Arrange
        const int Value = 90;
        const Func<Error, ValueTask<Result<string>>>? ErrorFunction = null;

        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        var exception = await Record.ExceptionAsync(
            () => sut.MatchAsync(x => ValueTask.FromResult(Result.FromValue(x.ToString())), ErrorFunction!).AsTask());

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithGenericResultSuccessAndErrorFunctions_CallsSuccessFunction()
    {
        // Arrange
        const int Value = 90;
        var successFunctionMock = new Mock<Func<int, ValueTask<Result<string>>>>();

        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(Result.FromValue("error value")));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithGenericResultSuccessAndErrorFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        const int Value = 90;
        var errorFunctionMock = new Mock<Func<Error, ValueTask<Result<string>>>>();

        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        await sut.MatchAsync(x => ValueTask.FromResult(Result.FromValue(x.ToString())), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_SuccessfulGenericResultValueTaskWithGenericResultSuccessAndErrorFunctions_ReturnsSuccessFunctionValueResult()
    {
        // Arrange
        const string ExpectedValue = "-90-";
        const int Value = 90;

        var sut = ValueTask.FromResult(Result.FromValue(Value));

        // Act
        var result = await sut.MatchAsync(
            x => ValueTask.FromResult(Result.FromValue($"-{x}-")),
            _ => ValueTask.FromResult(Result.FromValue("error value")));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultValueTaskWithGenericResultSuccessAndErrorFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<int, ValueTask<Result<string>>>>();

        var sut = ValueTask.FromResult(GetErrorResult<int>());

        // Act
        await sut.MatchAsync(successFunctionMock.Object, _ => ValueTask.FromResult(Result.FromValue("error value")));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultValueTaskWithGenericResultSuccessAndErrorFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, ValueTask<Result<string>>>>();

        var errorResult = GetErrorResult<int>();
        var sut = ValueTask.FromResult(errorResult);

        // Act
        await sut.MatchAsync(x => ValueTask.FromResult(Result.FromValue(x.ToString())), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
    }

    [Fact]
    public async Task MatchAsync_ErrorGenericResultValueTaskWithGenericResultSuccessAndErrorFunctions_ReturnsErrorFunctionValueResult()
    {
        // Arrange
        const string ExpectedValue = "error value";

        var sut = ValueTask.FromResult(GetErrorResult<int>());

        // Act
        var result = await sut.MatchAsync(
            x => ValueTask.FromResult(Result.FromValue(x.ToString())),
            _ => ValueTask.FromResult(Result.FromValue(ExpectedValue)));

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
