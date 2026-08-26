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
using System.Threading.Tasks;

namespace Maple.Result.Tests.Unit.Extensions;

public class IfSuccessTaskAsyncExtensionsUnitTests
{
    #region IfSuccessAsync (Result, Func<Task>)

    [Fact]
    public async Task IfSuccessAsync_NoResultWithAction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfSuccessAsync(() => new Task(() => { })));

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
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync!));

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
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync!));

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
    public async Task IfSuccessAsync_SuccessfulResultWithAction_ReturnsOriginalResult()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = await sut.IfSuccessAsync(() => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeTrue();
        result.ShouldBeSameAs(sut);
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
    public async Task IfSuccessAsync_ErrorResultWithAction_ReturnsOriginalResult()
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
        var exception = await Record.ExceptionAsync(() => Sut!.IfSuccessAsync(() => Task.FromResult(Result.Success())));

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
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function!));

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
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function!));

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
    public async Task IfSuccessAsync_SuccessfulResultWithSuccessfulResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        var funcResult = Result.Success();

        var sut = Result.Success();

        // Act
        var result = await sut.IfSuccessAsync(() => Task.FromResult(funcResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeTrue();
        result.ShouldBeSameAs(funcResult);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithErrorResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        var funcError = GetErrorResult();

        var sut = Result.Success();

        // Act
        var result = await sut.IfSuccessAsync(() => Task.FromResult(funcError));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(funcError.Error);
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
    public async Task IfSuccessAsync_ErrorResultWithResultFunction_ReturnsOriginalResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = await sut.IfSuccessAsync(() => Task.FromResult(Result.Success()));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region IfSuccessAsync (Result, Func<Task<T>>)

    [Fact]
    public async Task IfSuccessAsync_NoResultWithGenericFunction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfSuccessAsync(() => Task.FromResult(456)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithNoGenericFunction_ThrowsException()
    {
        // Arrange
        const Func<Task<int>>? Function = null;

        var sut = Result.Success();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithNoGenericFunction_ThrowsException()
    {
        // Arrange
        const Func<Task<int>>? Function = null;

        var sut = GetErrorResult();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithGenericFunction_CallsFunction()
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
    public async Task IfSuccessAsync_SuccessfulResultWithGenericFunction_ReturnsFunctionValueResult()
    {
        // Arrange
        const int FuncValue = 2463;

        var sut = Result.Success();

        // Act
        var result = await sut.IfSuccessAsync(() => Task.FromResult(FuncValue));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(FuncValue);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithGenericFunction_DoesNotCallFunction()
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
    public async Task IfSuccessAsync_ErrorResultWithGenericFunction_ReturnsOriginalResult()
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
        var exception =
            await Record.ExceptionAsync(() => Sut!.IfSuccessAsync(() => Task.FromResult(Result.FromValue(123))));

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
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function!));

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
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithGenericResultFunction_CallsFunction()
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
    public async Task IfSuccessAsync_SuccessfulResultWithSuccessfulGenericResultFunction_ReturnsFunctionValueResult()
    {
        // Arrange
        const int FuncValue = 2463;

        var sut = Result.Success();

        // Act
        var result = await sut.IfSuccessAsync(() => Task.FromResult(Result.FromValue(FuncValue)));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(FuncValue);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithErrorGenericResultFunction_ReturnsFunctionValueResultWithError()
    {
        // Arrange
        var funcError = GetError();

        var sut = Result.Success();

        // Act
        var result = await sut.IfSuccessAsync(() => Task.FromResult(Result<int>.FromError(funcError)));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(funcError);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithGenericResultFunction_DoesNotCallFunction()
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
    public async Task IfSuccessAsync_ErrorResultWithGenericResultFunction_ReturnsOriginalResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = await sut.IfSuccessAsync(() => Task.FromResult(Result.FromValue(678)));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region IfSuccessAsync (Result<T>, Func<T, Task>)

    [Fact]
    public async Task IfSuccessAsync_NoGenericResultWithGenericAction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfSuccessAsync(_ => Task.CompletedTask));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithNoGenericAction_ThrowsException()
    {
        // Arrange
        const int InitialValue = 35;
        const Func<int, Task>? ActionAsync = null;

        Result<int> sut = InitialValue;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithNoGenericAction_ThrowsException()
    {
        // Arrange
        const Func<int, Task>? ActionAsync = null;

        var sut = GetErrorResult<int>();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithGenericAction_CallsAction()
    {
        // Arrange
        const int InitialValue = 35;
        var functionMock = new Mock<Func<int, Task>>();

        Result<int> sut = InitialValue;

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(InitialValue), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithGenericAction_ReturnsOriginalValueResult()
    {
        // Arrange
        const int InitialValue = 35;

        Result<int> sut = InitialValue;

        // Act
        var result = await sut.IfSuccessAsync(_ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(InitialValue);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithGenericAction_DoesNotCallAction()
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
    public async Task IfSuccessAsync_ErrorGenericResultWithGenericAction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = await sut.IfSuccessAsync(_ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region IfSuccessAsync (Result<T>, Func<T, Task<Result>>)

    [Fact]
    public async Task IfSuccessAsync_NoGenericResultWithResultFunction_ThrowsException()
    {
        // Arrange
        const Result<string>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfSuccessAsync(_ => Task.FromResult(Result.Success())));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const string InitialValue = "Start";
        const Func<string, Task<Result>>? Function = null;

        Result<string> sut = InitialValue;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function!));

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
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithResultFunction_CallsFunction()
    {
        // Arrange
        const string InitialValue = "Start";
        var functionMock = new Mock<Func<string, Task<Result>>>();

        Result<string> sut = InitialValue;

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(InitialValue), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithSuccessResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        const string InitialValue = "Start";

        Result<string> sut = InitialValue;

        // Act
        var result = await sut.IfSuccessAsync(_ => Task.FromResult(Result.Success()));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeTrue();
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithErrorResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        const string InitialValue = "Start";
        var errorResult = GetErrorResult();

        Result<string> sut = InitialValue;

        // Act
        var result = await sut.IfSuccessAsync(_ => Task.FromResult(errorResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(errorResult.Error);
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
    public async Task IfSuccessAsync_ErrorGenericResultWithResultFunction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<string>();

        // Act
        var result = await sut.IfSuccessAsync(_ => Task.FromResult(Result.Success()));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region IfSuccessAsync (Result<T>, Func<T, Task<TNext>>)

    [Fact]
    public async Task IfSuccessAsync_NoGenericResultWithGenericFunction_ThrowsException()
    {
        // Arrange
        const Result<double>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfSuccessAsync(_ => Task.FromResult(12.34)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithNoGenericFunction_ThrowsException()
    {
        // Arrange
        const double InitialValue = 12.34;
        const Func<double, Task<int>>? Function = null;

        Result<double> sut = InitialValue;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithNoGenericFunction_ThrowsException()
    {
        // Arrange
        const Func<double, Task<int>>? Function = null;

        var sut = GetErrorResult<double>();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithGenericFunction_CallsFunction()
    {
        // Arrange
        const double InitialValue = 12.34;
        var functionMock = new Mock<Func<double, Task<int>>>();

        Result<double> sut = InitialValue;

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(InitialValue), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithGenericFunction_ReturnsFunctionValueResult()
    {
        // Arrange
        const int ExpectedValue = 13;
        const double InitialValue = 12.34;

        Result<double> sut = InitialValue;

        // Act
        var result = await sut.IfSuccessAsync(x => Task.FromResult((int)x + 1));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultWithGenericFunction_DoesNotCallFunction()
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
    public async Task IfSuccessAsync_ErrorGenericResultWithGenericFunction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<double>();

        // Act
        var result = await sut.IfSuccessAsync(_ => Task.FromResult(9827));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region IfSuccessAsync (Result<T>, Func<T, Task<Result<TNext>>>)

    [Fact]
    public async Task IfSuccessAsync_NoGenericResultWithGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(
            () => Sut!.IfSuccessAsync(x => Task.FromResult(Result.FromValue(x.ToString()))));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const int InitialValue = 39;
        const Func<int, Task<Result<string>>>? Function = null;

        Result<int> sut = InitialValue;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function!));

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
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithGenericResultFunction_CallsFunction()
    {
        // Arrange
        const int InitialValue = 39;
        var functionMock = new Mock<Func<int, Task<Result<string>>>>();

        Result<int> sut = InitialValue;

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(InitialValue), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithSuccessfulGenericResultFunction_ReturnsFunctionValueResult()
    {
        // Arrange
        const int InitialValue = 49;
        const string FuncValue = "50";

        Result<int> sut = InitialValue;

        // Act
        var result = await sut.IfSuccessAsync(x => Task.FromResult(Result.FromValue((x + 1).ToString())));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(FuncValue);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultWithErrorGenericResultFunction_ReturnsFunctionValueResultWithError()
    {
        // Arrange
        const int InitialValue = 49;
        var funcError = GetErrorResult<string>();

        Result<int> sut = InitialValue;

        // Act
        var result = await sut.IfSuccessAsync(_ => Task.FromResult(funcError));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(funcError.Error);
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
    public async Task IfSuccessAsync_ErrorGenericResultWithGenericResultFunction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = await sut.IfSuccessAsync(x => Task.FromResult(Result.FromValue(x.ToString())));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region IfSuccessAsync (Task<Result>, Func<Task>)

    [Fact]
    public async Task IfSuccessAsync_NoResultTaskWithAction_ThrowsException()
    {
        // Arrange
        const Task<Result>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfSuccessAsync(() => new Task(() => { })));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Task>? ActionAsync = null;

        var sut = Task.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultTaskWithNoAction_ThrowsException()
    {
        // Arrange
        const Func<Task>? ActionAsync = null;

        var sut = Task.FromResult(GetErrorResult());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithAction_CallsAction()
    {
        // Arrange
        var actionMock = new Mock<Func<Task>>();

        var sut = Task.FromResult(Result.Success());

        // Act
        await sut.IfSuccessAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithAction_ReturnsOriginalResult()
    {
        // Arrange
        var initialResult = Result.Success();
        var sut = Task.FromResult(initialResult);

        // Act
        var result = await sut.IfSuccessAsync(() => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeTrue();
        result.ShouldBeSameAs(initialResult);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultTaskWithAction_DoesNotCallAction()
    {
        // Arrange
        var actionMock = new Mock<Func<Task>>();

        var sut = Task.FromResult(GetErrorResult());

        // Act
        await sut.IfSuccessAsync(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultTaskWithAction_ReturnsOriginalResult()
    {
        // Arrange
        var errorResult = GetErrorResult();
        var sut = Task.FromResult(errorResult);

        // Act
        var result = await sut.IfSuccessAsync(() => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(errorResult.Error);
    }

    #endregion

    #region IfSuccessAsync (Task<Result>, Func<Task<Result>>)

    [Fact]
    public async Task IfSuccessAsync_NoResultTaskWithResultFunction_ThrowsException()
    {
        // Arrange
        const Task<Result>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfSuccessAsync(() => Task.FromResult(Result.Success())));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Task<Result>>? Function = null;

        var sut = Task.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultTaskWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Task<Result>>? Function = null;

        var sut = GetErrorResult();

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithResultFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Task<Result>>>();

        var sut = Task.FromResult(Result.Success());

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithSuccessfulResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        var funcResult = Result.Success();

        var sut = Task.FromResult(Result.Success());

        // Act
        var result = await sut.IfSuccessAsync(() => Task.FromResult(funcResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeTrue();
        result.ShouldBeSameAs(funcResult);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithErrorResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        var funcError = GetErrorResult();

        var sut = Task.FromResult(Result.Success());

        // Act
        var result = await sut.IfSuccessAsync(() => Task.FromResult(funcError));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(funcError.Error);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultTaskWithResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Task<Result>>>();

        var sut = Task.FromResult(GetErrorResult());

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultTaskWithResultFunction_ReturnsOriginalResult()
    {
        // Arrange
        var errorResult = GetErrorResult();
        var sut = Task.FromResult(errorResult);

        // Act
        var result = await sut.IfSuccessAsync(() => Task.FromResult(Result.Success()));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(errorResult.Error);
    }

    #endregion

    #region IfSuccessAsync (Task<Result>, Func<Task<T>>)

    [Fact]
    public async Task IfSuccessAsync_NoResultTaskWithGenericFunction_ThrowsException()
    {
        // Arrange
        const Task<Result>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfSuccessAsync(() => Task.FromResult(456)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithNoGenericFunction_ThrowsException()
    {
        // Arrange
        const Func<Task<int>>? Function = null;

        var sut = Task.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultTaskWithNoGenericFunction_ThrowsException()
    {
        // Arrange
        const Func<Task<int>>? Function = null;

        var sut = Task.FromResult(GetErrorResult());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithGenericFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Task<int>>>();

        var sut = Task.FromResult(Result.Success());

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithGenericFunction_ReturnsFunctionValueResult()
    {
        // Arrange
        const int FuncValue = 2463;

        var sut = Task.FromResult(Result.Success());

        // Act
        var result = await sut.IfSuccessAsync(() => Task.FromResult(FuncValue));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(FuncValue);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultTaskWithGenericFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Task<int>>>();

        var sut = Task.FromResult(GetErrorResult());

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultTaskWithGenericFunction_ReturnsOriginalResult()
    {
        // Arrange
        var errorResult = GetErrorResult();
        var sut = Task.FromResult(errorResult);

        // Act
        var result = await sut.IfSuccessAsync(() => Task.FromResult(234));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(errorResult.Error);
    }

    #endregion

    #region IfSuccessAsync (Task<Result>, Func<Task<Result<T>>>)

    [Fact]
    public async Task IfSuccessAsync_NoResultTaskWithGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Task<Result>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfSuccessAsync(() => Task.FromResult(Result.FromValue(123))));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Task<Result<int>>>? Function = null;

        var sut = Task.FromResult(Result.Success());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultTaskWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Task<Result<int>>>? Function = null;

        var sut = Task.FromResult(GetErrorResult());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithGenericResultFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Task<Result<int>>>>();

        var sut = Task.FromResult(Result.Success());

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithSuccessfulGenericResultFunction_ReturnsFunctionValueResult()
    {
        // Arrange
        const int FuncValue = 2463;

        var sut = Task.FromResult(Result.Success());

        // Act
        var result = await sut.IfSuccessAsync(() => Task.FromResult(Result.FromValue(FuncValue)));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(FuncValue);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultTaskWithErrorGenericResultFunction_ReturnsFunctionValueResultWithError()
    {
        // Arrange
        var funcError = GetError();

        var sut = Task.FromResult(Result.Success());

        // Act
        var result = await sut.IfSuccessAsync(() => Task.FromResult(Result<int>.FromError(funcError)));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(funcError);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultTaskWithGenericResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Task<Result<int>>>>();

        var sut = Task.FromResult(GetErrorResult());

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultTaskWithGenericResultFunction_ReturnsOriginalResult()
    {
        // Arrange
        var errorResult = GetErrorResult();
        var sut = Task.FromResult(errorResult);

        // Act
        var result = await sut.IfSuccessAsync(() => Task.FromResult(Result.FromValue(678)));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(errorResult.Error);
    }

    #endregion

    #region IfSuccessAsync (Task<Result<T>>, Func<T, Task>)

    [Fact]
    public async Task IfSuccessAsync_NoGenericResultTaskWithGenericAction_ThrowsException()
    {
        // Arrange
        const Task<Result<int>>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfSuccessAsync(_ => Task.CompletedTask));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultTaskWithNoGenericAction_ThrowsException()
    {
        // Arrange
        const int InitialValue = 35;
        const Func<int, Task>? ActionAsync = null;

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultTaskWithNoGenericAction_ThrowsException()
    {
        // Arrange
        const Func<int, Task>? ActionAsync = null;

        var sut = Task.FromResult(GetErrorResult<int>());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultTaskWithGenericAction_CallsAction()
    {
        // Arrange
        const int InitialValue = 35;
        var functionMock = new Mock<Func<int, Task>>();

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(InitialValue), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultTaskWithGenericAction_ReturnsOriginalValueResult()
    {
        // Arrange
        const int InitialValue = 35;

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        var result = await sut.IfSuccessAsync(_ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(InitialValue);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultTaskWithGenericAction_DoesNotCallAction()
    {
        // Arrange
        var functionMock = new Mock<Func<int, Task>>();

        var sut = Task.FromResult(GetErrorResult<int>());

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultTaskWithGenericAction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var errorResult = GetErrorResult<int>();
        var sut = Task.FromResult(errorResult);

        // Act
        var result = await sut.IfSuccessAsync(_ => Task.CompletedTask);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(errorResult.Error);
    }

    #endregion

    #region IfSuccessAsync (Task<Result<T>>, Func<T, Task<Result>>)

    [Fact]
    public async Task IfSuccessAsync_NoGenericResultTaskWithResultFunction_ThrowsException()
    {
        // Arrange
        const Task<Result<string>>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfSuccessAsync(_ => Task.FromResult(Result.Success())));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultTaskWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const string InitialValue = "Start";
        const Func<string, Task<Result>>? Function = null;

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultTaskWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<string, Task<Result>>? Function = null;

        var sut = Task.FromResult(GetErrorResult<string>());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultTaskWithResultFunction_CallsFunction()
    {
        // Arrange
        const string InitialValue = "Start";
        var functionMock = new Mock<Func<string, Task<Result>>>();

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(InitialValue), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultTaskWithSuccessResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        const string InitialValue = "Start";

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        var result = await sut.IfSuccessAsync(_ => Task.FromResult(Result.Success()));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeTrue();
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultTaskWithErrorResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        const string InitialValue = "Start";
        var errorResult = GetErrorResult();

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        var result = await sut.IfSuccessAsync(_ => Task.FromResult(errorResult));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(errorResult.Error);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultTaskWithResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<string, Task<Result>>>();

        var sut = Task.FromResult(GetErrorResult<string>());

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultTaskWithResultFunction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var errorResult = GetErrorResult<string>();
        var sut = Task.FromResult(errorResult);

        // Act
        var result = await sut.IfSuccessAsync(_ => Task.FromResult(Result.Success()));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(errorResult.Error);
    }

    #endregion

    #region IfSuccessAsync (Task<Result<T>>, Func<T, Task<TNext>>)

    [Fact]
    public async Task IfSuccessAsync_NoGenericResultTaskWithGenericFunction_ThrowsException()
    {
        // Arrange
        const Task<Result<double>>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut!.IfSuccessAsync(_ => Task.FromResult(12.34)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultTaskWithNoGenericFunction_ThrowsException()
    {
        // Arrange
        const double InitialValue = 12.34;
        const Func<double, Task<int>>? Function = null;

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultTaskWithNoGenericFunction_ThrowsException()
    {
        // Arrange
        const Func<double, Task<int>>? Function = null;

        var sut = Task.FromResult(GetErrorResult<double>());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultTaskWithGenericFunction_CallsFunction()
    {
        // Arrange
        const double InitialValue = 12.34;
        var functionMock = new Mock<Func<double, Task<int>>>();

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(InitialValue), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultTaskWithGenericFunction_ReturnsFunctionValueResult()
    {
        // Arrange
        const int ExpectedValue = 13;
        const double InitialValue = 12.34;

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        var result = await sut.IfSuccessAsync(x => Task.FromResult((int)x + 1));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultTaskWithGenericFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<double, Task<int>>>();

        var sut = Task.FromResult(GetErrorResult<double>());

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<double>()), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultTaskWithGenericFunction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var errorResult = GetErrorResult<double>();
        var sut = Task.FromResult(errorResult);

        // Act
        var result = await sut.IfSuccessAsync(_ => Task.FromResult(9827));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(errorResult.Error);
    }

    #endregion

    #region IfSuccessAsync (Task<Result<T>>, Func<T, Task<Result<TNext>>>)

    [Fact]
    public async Task IfSuccessAsync_NoGenericResultTaskWithGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Task<Result<int>>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(
            () => Sut!.IfSuccessAsync(x => Task.FromResult(Result.FromValue(x.ToString()))));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultTaskWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const int InitialValue = 39;
        const Func<int, Task<Result<string>>>? Function = null;

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultTaskWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Func<int, Task<Result<string>>>? Function = null;

        var sut = Task.FromResult(GetErrorResult<int>());

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultTaskWithGenericResultFunction_CallsFunction()
    {
        // Arrange
        const int InitialValue = 39;
        var functionMock = new Mock<Func<int, Task<Result<string>>>>();

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(InitialValue), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultTaskWithSuccessfulGenericResultFunction_ReturnsFunctionValueResult()
    {
        // Arrange
        const int InitialValue = 49;
        const string FuncValue = "50";

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        var result = await sut.IfSuccessAsync(x => Task.FromResult(Result.FromValue((x + 1).ToString())));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(FuncValue);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulGenericResultTaskWithErrorGenericResultFunction_ReturnsFunctionValueResultWithError()
    {
        // Arrange
        const int InitialValue = 49;
        var funcError = GetErrorResult<string>();

        var sut = Task.FromResult(Result.FromValue(InitialValue));

        // Act
        var result = await sut.IfSuccessAsync(_ => Task.FromResult(funcError));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(funcError.Error);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultTaskWithGenericResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<int, Task<Result<string>>>>();

        var sut = Task.FromResult(GetErrorResult<int>());

        // Act
        await sut.IfSuccessAsync(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorGenericResultTaskWithGenericResultFunction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var errorResult = GetErrorResult<int>();
        var sut = Task.FromResult(errorResult);

        // Act
        var result = await sut.IfSuccessAsync(x => Task.FromResult(Result.FromValue(x.ToString())));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(errorResult.Error);
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
