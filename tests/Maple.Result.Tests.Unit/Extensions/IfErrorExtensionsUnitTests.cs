using Maple.Result.Extensions;
using Moq;
using System;

namespace Maple.Result.Tests.Unit.Extensions;

public class IfErrorExtensionsUnitTests
{
    #region IfError (Result, Action<Error>)

    [Fact]
    public void IfError_NoResultWithAction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut.IfError(_ => { }));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfError_SuccessResultWithNoAction_ThrowsException()
    {
        // Arrange
        const Action<Error>? Action = null;

        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.IfError(Action));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfError_ErrorResultWithNoAction_ThrowsException()
    {
        // Arrange
        const Action<Error>? Action = null;

        var sut = GetErrorResult();

        // Act
        var exception = Record.Exception(() => sut.IfError(Action));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfError_SuccessResultWithAction_DoesNotCallAction()
    {
        // Arrange
        var actionMock = new Mock<Action<Error>>();

        var sut = Result.Success();

        // Act
        _ = sut.IfError(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public void IfError_SuccessResultWithAction_ReturnsOriginalSuccessResult()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = sut.IfError(_ => { });

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
        result.IsSuccess().ShouldBeTrue();
    }

    [Fact]
    public void IfError_ErrorResultWithAction_CallsAction()
    {
        // Arrange
        var actionMock = new Mock<Action<Error>>();

        var sut = GetErrorResult();

        // Act
        _ = sut.IfError(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public void IfError_ErrorResultWithAction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = sut.IfError(_ => { });

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfError (Result, Func<Error, Result>)

    [Fact]
    public void IfError_NoResultWithResultFunction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut.IfError(_ => Result.Success()));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfError_SuccessResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, Result>? Function = null;

        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.IfError(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfError_ErrorResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, Result>? Function = null;

        var sut = GetErrorResult();

        // Act
        var exception = Record.Exception(() => sut.IfError(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfError_SuccessResultWithResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Error, Result>>();

        var sut = Result.Success();

        // Act
        var result = sut.IfError(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
        result.ShouldBeSameAs(sut);
    }

    [Fact]
    public void IfError_SuccessResultWithResultFunction_ReturnsOriginalSuccessResult()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = sut.IfError(_ => Result.Success());

        // Assert
        result.ShouldBeSameAs(sut);
    }

    [Fact]
    public void IfError_ErrorResultWithResultFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Error, Result>>();

        var sut = GetErrorResult();

        // Act
        _ = sut.IfError(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public void IfError_ErrorResultWithResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        var replacementResult = Result.Success();

        var sut = GetErrorResult();

        // Act
        var result = sut.IfError(_ => replacementResult);

        // Assert
        result.ShouldBeSameAs(replacementResult);
    }

    [Fact]
    public void IfError_ErrorResultWithErrorResultFunction_ReturnsErrorFromFunction()
    {
        // Arrange
        var replacementError = GetReplacementError();

        var sut = GetErrorResult();

        // Act
        var result = sut.IfError(_ => replacementError);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(replacementError);
    }

    #endregion

    #region IfError (Result<T>, Action<Error>)

    [Fact]
    public void IfError_NoGenericResultWithAction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut.IfError(_ => { }));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfError_SuccessGenericResultWithNoAction_ThrowsException()
    {
        // Arrange
        const int Value = 24;
        const Action<Error>? Action = null;

        Result<int> sut = Value;

        // Act
        var exception = Record.Exception(() => sut.IfError(Action));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfError_ErrorGenericResultWithNoAction_ThrowsException()
    {
        // Arrange
        const Action<Error>? Action = null;

        var sut = GetErrorResult<int>();

        // Act
        var exception = Record.Exception(() => sut.IfError(Action));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfError_SuccessGenericResultWithAction_DoesNotCallAction()
    {
        // Arrange
        const int Value = 29;
        var actionMock = new Mock<Action<Error>>();

        Result<int> sut = Value;

        // Act
        _ = sut.IfError(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public void IfError_SuccessGenericResultWithAction_ReturnsOriginalSuccessResult()
    {
        // Arrange
        const int Value = 38;

        Result<int> sut = Value;

        // Act
        var result = sut.IfError(_ => { });

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(Value);
    }

    [Fact]
    public void IfError_ErrorGenericResultWithAction_CallsAction()
    {
        // Arrange
        var actionMock = new Mock<Action<Error>>();

        var sut = GetErrorResult<int>();

        // Act
        _ = sut.IfError(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public void IfError_ErrorGenericResultWithAction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = sut.IfError(_ => { });

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfError (Result<T>, Func<Error, Result<T>>)

    [Fact]
    public void IfError_NoGenericResultWithResultFunction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut.IfError(_ => Result<int>.FromValue(1)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfError_SuccessfulGenericResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const int Value = 24;
        const Func<Error, Result<int>>? Function = null;

        Result<int> sut = Value;

        // Act
        var exception = Record.Exception(() => sut.IfError(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfError_ErrorGenericResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, Result<int>>? Function = null;

        var sut = GetErrorResult<int>();

        // Act
        var exception = Record.Exception(() => sut.IfError(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfError_SuccessfulGenericResultWithResultFunction_DoesNotCallFunction()
    {
        // Arrange
        const int Value = 42;
        var functionMock = new Mock<Func<Error, Result<int>>>();

        Result<int> sut = Value;

        // Act
        var result = sut.IfError(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
        result.ShouldBeSameAs(sut);
        result.Value.ShouldBe(Value);
    }

    [Fact]
    public void IfError_SuccessfulGenericResultWithResultFunction_ReturnsOriginalSuccessResult()
    {
        // Arrange
        const int Value = 11;

        Result<int> sut = Value;

        // Act
        var result = sut.IfError(_ => Result<int>.FromValue(2));

        // Assert
        result.ShouldBeSameAs(sut);
        result.Value.ShouldBe(Value);
    }

    [Fact]
    public void IfError_ErrorGenericResultWithResultFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Error, Result<int>>>();

        var sut = GetErrorResult<int>();

        // Act
        _ = sut.IfError(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public void IfError_ErrorGenericResultWithResultFunction_ReturnsFunctionResult()
    {
        // Arrange
        var replacementResult = Result<int>.FromValue(58);

        var sut = GetErrorResult<int>();

        // Act
        var result = sut.IfError(_ => replacementResult);

        // Assert
        result.ShouldBeSameAs(replacementResult);
        result.Value.ShouldBe(58);
    }

    [Fact]
    public void IfError_ErrorGenericResultWithResultFunction_ReturnsErrorFromFunction()
    {
        // Arrange
        var replacementError = GetReplacementError();

        var sut = GetErrorResult<int>();

        // Act
        var result = sut.IfError(_ => replacementError);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(replacementError);
    }

    #endregion

    #region helper methods

    private static Result GetErrorResult()
    {
        return Error.Failure(ErrorUri.None(), "d10e75ee-2687-49c2-9641-fd8b86e07718", "Error title 1");
    }

    private static Result<T> GetErrorResult<T>()
    {
        return Error.Failure(ErrorUri.None(), "f2082bda-b481-4f4b-974d-1faccec44be4", "Error title 2");
    }

    private static Error GetReplacementError()
    {
        return Error.Failure(ErrorUri.None(), "a73d2604-e540-4dd3-8db8-3e6a70f89d72", "Replacement error");
    }

    #endregion
}