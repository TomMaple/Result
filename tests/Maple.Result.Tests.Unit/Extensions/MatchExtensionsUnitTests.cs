using Maple.Result.Extensions;
using Moq;
using System;

namespace Maple.Result.Tests.Unit.Extensions;

public class MatchExtensionsUnitTests
{
    #region Match (Result, Action, Action<Error>)

    [Fact]
    public void Match_NoResultWithActions_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.Match(() => { }, _ => { }));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulResultWithNoSuccessActionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Action? SuccessAction = null;

        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.Match(SuccessAction!, _ => { }));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulResultWithSuccessActionAndNoErrorAction_ThrowsException()
    {
        // Arrange
        const Action<Error>? ErrorAction = null;

        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.Match(() => { }, ErrorAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulResultWithSuccessAndErrorActions_CallsSuccessAction()
    {
        // Arrange
        var successActionMock = new Mock<Action>();

        var sut = Result.Success();

        // Act
        sut.Match(successActionMock.Object, _ => { });

        // Assert
        successActionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public void Match_SuccessfulResultWithSuccessAndErrorActions_DoesNotCallErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Action<Error>>();

        var sut = Result.Success();

        // Act
        sut.Match(() => { }, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public void Match_SuccessfulResultWithSuccessAndErrorActions_ReturnsOriginalResult()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = sut.Match(() => { }, _ => { });

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBe(sut);
    }

    [Fact]
    public void Match_ErrorResultWithSuccessAndErrorActions_DoesNotCallSuccessAction()
    {
        // Arrange
        var successActionMock = new Mock<Action>();

        var sut = GetErrorResult();

        // Act
        sut.Match(successActionMock.Object, _ => { });

        // Assert
        successActionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public void Match_ErrorResultWithSuccessAndErrorActions_CallsErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Action<Error>>();

        var sut = GetErrorResult();

        // Act
        sut.Match(() => { }, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public void Match_ErrorResultWithSuccessAndErrorActions_ReturnsOriginalResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = sut.Match(() => { }, _ => { });

        // Assert
        result.ShouldBe(sut);
    }

    #endregion

    #region Match (Result, Func<Result>, Action<Error>)

    [Fact]
    public void Match_NoResultWithSuccessResultFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.Match(Result.Success, _ => { }));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulResultWithNoSuccessResultFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Result>? SuccessFunction = null;

        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.Match(SuccessFunction!, _ => { }));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulResultWithSuccessResultFunctionAndNoErrorAction_ThrowsException()
    {
        // Arrange
        const Action<Error>? ErrorAction = null;

        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.Match(Result.Success, ErrorAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulResultWithSuccessResultFunctionAndErrorAction_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<Result>>();

        var sut = Result.Success();

        // Act
        sut.Match(successFunctionMock.Object, _ => { });

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public void Match_SuccessfulResultWithSuccessResultFunctionAndErrorAction_DoesNotCallErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Action<Error>>();

        var sut = Result.Success();

        // Act
        sut.Match(Result.Success, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public void Match_SuccessfulResultWithSuccessResultFunctionAndErrorAction_ReturnsSuccessFunctionResult()
    {
        // Arrange
        var successFunctionResult = Result.Success();

        var sut = Result.Success();

        // Act
        var result = sut.Match(() => successFunctionResult, _ => { });

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(successFunctionResult);
    }

    [Fact]
    public void Match_SuccessfulResultWithSuccessErrorResultFunctionAndErrorAction_ReturnsSuccessFunctionResult()
    {
        // Arrange
        var successFunctionError = GetErrorResult();

        var sut = Result.Success();

        // Act
        var result = sut.Match(() => successFunctionError, _ => { });

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(successFunctionError);
    }

    [Fact]
    public void Match_ErrorResultWithSuccessResultFunctionAndErrorAction_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<Result>>();

        var sut = GetErrorResult();

        // Act
        sut.Match(successFunctionMock.Object, _ => { });

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public void Match_ErrorResultWithSuccessResultFunctionAndErrorAction_CallsErrorFunction()
    {
        // Arrange
        var errorActionMock = new Mock<Action<Error>>();

        var sut = GetErrorResult();

        // Act
        sut.Match(Result.Success, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public void Match_ErrorResultWithSuccessResultFunctionAndErrorAction_ReturnsOriginalResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = sut.Match(Result.Success, _ => { });

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(sut);
    }

    #endregion

    #region Match (Result, Func<Result>, Func<Error, Result>)

    [Fact]
    public void Match_NoResultWithSuccessAndErrorResultFunctions_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.Match(Result.Success, _ => Result.Success()));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulResultWithNoSuccessResultFunctionAndErrorResultFunction_ThrowsException2()
    {
        // Arrange
        const Func<Result>? SuccessFunction = null;

        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.Match(SuccessFunction!, _ => Result.Success()));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulResultWithSuccessResultFunctionAndNoErrorResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, Result>? ErrorFunction = null;

        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.Match(Result.Success, ErrorFunction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulResultWithSuccessAndErrorResultFunctions_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<Result>>();

        var sut = Result.Success();

        // Act
        sut.Match(successFunctionMock.Object, _ => Result.Success());

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public void Match_SuccessfulResultWithSuccessAndErrorResultFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, Result>>();

        var sut = Result.Success();

        // Act
        sut.Match(Result.Success, errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public void Match_SuccessfulResultWithSuccessAndErrorResultFunctions_ReturnsSuccessFunctionResult()
    {
        // Arrange
        var successFunctionResult = Result.Success();

        var sut = Result.Success();

        // Act
        var result = sut.Match(() => successFunctionResult, _ => Result.Success());

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(successFunctionResult);
    }

    [Fact]
    public void Match_ErrorResultWithSuccessAndErrorResultFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<Result>>();

        var sut = GetErrorResult();

        // Act
        sut.Match(successFunctionMock.Object, _ => Result.Success());

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public void Match_ErrorResultWithSuccessAndErrorResultFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, Result>>();

        var sut = GetErrorResult();

        // Act
        sut.Match(Result.Success, errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public void Match_ErrorResultWithSuccessAndErrorResultFunctions_ReturnsErrorFunctionResult()
    {
        // Arrange
        var errorFunctionResult = Result.Success();

        var sut = GetErrorResult();

        // Act
        var result = sut.Match(Result.Success, _ => errorFunctionResult);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(errorFunctionResult);
    }

    #endregion

    #region Match (Result, Func<T>, Action<Error>)

    [Fact]
    public void Match_NoResultWithGenericSuccessFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.Match(() => 1, _ => { }));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulResultWithNoGenericSuccessFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<int>? SuccessFunction = null;

        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.Match(SuccessFunction!, _ => { }));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulResultWithGenericSuccessFunctionNoErrorAction_ThrowsException()
    {
        // Arrange
        const Action<Error>? ErrorAction = null;

        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.Match(() => 1, ErrorAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulResultWithGenericSuccessFunctionAndErrorAction_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<int>>();

        var sut = Result.Success();

        // Act
        sut.Match(successFunctionMock.Object, _ => { });

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public void Match_SuccessfulResultWithGenericSuccessFunctionAndErrorAction_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorActionMock = new Mock<Action<Error>>();

        var sut = Result.Success();

        // Act
        sut.Match(() => 345, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public void Match_SuccessfulResultWithGenericSuccessFunctionAndErrorAction_ReturnsSuccessFunctionValue()
    {
        // Arrange
        const int SuccessFunctionValue = 42;

        var sut = Result.Success();

        // Act
        var result = sut.Match(() => SuccessFunctionValue, _ => { });

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(SuccessFunctionValue);
    }

    [Fact]
    public void Match_ErrorResultWithGenericSuccessFunctionAndErrorAction_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<int>>();

        var sut = GetErrorResult();

        // Act
        sut.Match(successFunctionMock.Object, _ => { });

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public void Match_ErrorResultWithGenericSuccessFunctionAndErrorAction_CallsErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Action<Error>>();

        var sut = GetErrorResult();

        // Act
        sut.Match(() => 432, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public void Match_ErrorResultWithGenericSuccessFunctionAndErrorAction_ReturnsOriginalResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = sut.Match(() => 123, _ => { });

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region Match (Result, Func<T>, Func<Error, T>)

    [Fact]
    public void Match_NoResultWithGenericSuccessAndErrorFunctions_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.Match(() => 1, _ => 2));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulResultWithNoGenericSuccessFunctionAndGenericErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<int>? Function = null;

        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.Match(Function!, _ => 2));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulResultWithGenericSuccessFunctionAndNoGenericErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, int>? ErrorFunction = null;

        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.Match(() => 1, ErrorFunction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulResultWithGenericSuccessAndErrorFunctions_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<int>>();

        var sut = Result.Success();

        // Act
        sut.Match(successFunctionMock.Object, _ => 2);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public void Match_SuccessfulResultWithGenericSuccessAndErrorFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, int>>();

        var sut = Result.Success();

        // Act
        sut.Match(() => 1, errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public void Match_SuccessfulResultWithGenericSuccessAndErrorFunctions_ReturnsSuccessFunctionValue()
    {
        // Arrange
        const int ExpectedValue = 58;

        var sut = Result.Success();

        // Act
        var result = sut.Match(() => ExpectedValue, _ => 2);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public void Match_ErrorResultWithGenericSuccessAndErrorFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<int>>();

        var sut = GetErrorResult();

        // Act
        sut.Match(successFunctionMock.Object, _ => 2);

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public void Match_ErrorResultWithGenericSuccessAndErrorFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, int>>();

        var sut = GetErrorResult();

        // Act
        sut.Match(() => 1, errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public void Match_ErrorResultWithGenericSuccessAndErrorFunctions_ReturnsErrorFunctionValue()
    {
        // Arrange
        const int ExpectedValue = 73;

        var sut = GetErrorResult();

        // Act
        var result = sut.Match(() => 1, _ => ExpectedValue);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    #endregion

    #region Match (Result, Func<Result<T>>, Action<Error>)

    [Fact]
    public void Match_NoResultWithGenericResultSuccessFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.Match(() => Result.FromValue(1), _ => { }));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulResultWithNoGenericResultResultFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<Result<int>>? SuccessFunction = null;

        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.Match(SuccessFunction!, _ => { }));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulResultWithGenericResultSuccessFunctionAndNoErrorAction_ThrowsException()
    {
        // Arrange
        const Action<Error>? ErrorAction = null;

        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.Match(() => Result.FromValue(1), ErrorAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulResultWithGenericResultSuccessFunctionAndErrorAction_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<Result<int>>>();

        var sut = Result.Success();

        // Act
        sut.Match(successFunctionMock.Object, _ => { });

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public void Match_SuccessfulResultWithGenericResultSuccessFunctionAndErrorAction_DoesNotCallErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Action<Error>>();

        var sut = Result.Success();

        // Act
        sut.Match(() => Result.FromValue(1), errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public void Match_SuccessfulResultWithGenericResultSuccessFunctionAndErrorAction_ReturnsSuccessFunctionResultValue()
    {
        // Arrange
        var expectedResult = Result.FromValue(12);

        var sut = Result.Success();

        // Act
        var result = sut.Match(() => expectedResult, _ => { });

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.ShouldBeSameAs(expectedResult);
    }

    [Fact]
    public void Match_ErrorResultWithGenericResultSuccessFunctionAndErrorAction_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<Result<int>>>();

        var sut = GetErrorResult();

        // Act
        sut.Match(successFunctionMock.Object, _ => { });

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public void Match_ErrorResultWithGenericResultSuccessFunctionAndErrorAction_CallsErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Action<Error>>();

        var sut = GetErrorResult();

        // Act
        sut.Match(() => Result.FromValue(1), errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public void Match_ErrorResultWithGenericResultSuccessFunctionAndErrorAction_ReturnsOriginalResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = sut.Match(() => Result.FromValue(1), _ => { });

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region Match (Result, Func<Result<T>>, Func<Error, Result<T>>)

    [Fact]
    public void Match_NoResultWithGenericResultSuccessAndErrorFunctions_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.Match(() => Result.FromValue(1), _ => Result.FromValue(2)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulResultWithNoGenericResultSuccessFunctionAndGenericResultErrorFunction_ThrowsException3()
    {
        // Arrange
        const Func<Result<int>>? Function = null;

        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.Match(Function!, _ => Result.FromValue(2)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulResultWithGenericResultSuccessFunctionAndNoGenericResultErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, Result<int>>? ErrorFunction = null;

        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.Match(() => Result.FromValue(1), ErrorFunction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulResultWithGenericResultSuccessAndErrorFunctions_CallsSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<Result<int>>>();

        var sut = Result.Success();

        // Act
        sut.Match(successFunctionMock.Object, _ => Result.FromValue(2));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public void Match_SuccessfulResultWithGenericResultSuccessAndErrorFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, Result<int>>>();

        var sut = Result.Success();

        // Act
        sut.Match(() => Result.FromValue(1), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public void Match_SuccessfulResultWithGenericResultSuccessAndErrorFunctions_ReturnsSuccessFunctionResult()
    {
        // Arrange
        var expectedResult = Result.FromValue(9);

        var sut = Result.Success();

        // Act
        var result = sut.Match(() => expectedResult, _ => Result.FromValue(2));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.ShouldBeSameAs(expectedResult);
    }

    [Fact]
    public void Match_ErrorResultWithGenericResultSuccessAndErrorFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<Result<int>>>();

        var sut = GetErrorResult();

        // Act
        sut.Match(successFunctionMock.Object, _ => Result.FromValue(2));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public void Match_ErrorResultWithGenericResultSuccessAndErrorFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, Result<int>>>();

        var sut = GetErrorResult();

        // Act
        sut.Match(() => Result.FromValue(1), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public void Match_ErrorResultWithGenericResultSuccessAndErrorFunctions_ReturnsErrorFunctionValueResult()
    {
        // Arrange
        var expectedResult = Result.FromValue(15);

        var sut = GetErrorResult();

        // Act
        var result = sut.Match(() => Result.FromValue(1), _ => expectedResult);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeSameAs(expectedResult);
    }

    #endregion

    #region Match (Result<T>, Action<T>, Action<Error>)

    [Fact]
    public void Match_NoGenericResultWithSuccessAndErrorActions_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.Match(_ => { }, _ => { }));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulGenericResultWithNoSuccessActionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Action<int>? SuccessAction = null;

        Result<int> sut = 1;

        // Act
        var exception = Record.Exception(() => sut.Match(SuccessAction!, _ => { }));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulGenericResultWithSuccessActionAndNoErrorAction_ThrowsException()
    {
        // Arrange
        const Action<Error>? ErrorAction = null;

        Result<int> sut = 1;

        // Act
        var exception = Record.Exception(() => sut.Match(_ => { }, ErrorAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulGenericResultWithSuccessAndErrorActions_CallsSuccessAction()
    {
        // Arrange
        var successActionMock = new Mock<Action<int>>();

        const int Value = 25;
        Result<int> sut = Value;

        // Act
        sut.Match(successActionMock.Object, _ => { });

        // Assert
        successActionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public void Match_SuccessfulGenericResultWithSuccessAndErrorActions_DoesNotCallErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Action<Error>>();

        const int Value = 25;
        Result<int> sut = Value;

        // Act
        sut.Match(_ => { }, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public void Match_SuccessfulGenericResultWithSuccessAndErrorActions_ReturnsOriginalValueResult()
    {
        // Arrange
        const int Value = 25;
        Result<int> sut = Value;

        // Act
        var result = sut.Match(_ => { }, _ => { });

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.ShouldBeSameAs(sut);
    }

    [Fact]
    public void Match_ErrorGenericResultWithSuccessAndErrorActions_DoesNotCallSuccessAction()
    {
        // Arrange
        var successActionMock = new Mock<Action<int>>();

        var sut = GetErrorResult<int>();

        // Act
        sut.Match(successActionMock.Object, _ => { });

        // Assert
        successActionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void Match_ErrorGenericResultWithSuccessAndErrorActions_CallsErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Action<Error>>();

        var sut = GetErrorResult<int>();

        // Act
        sut.Match(_ => { }, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public void Match_ErrorGenericResultWithSuccessAndErrorActions_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = sut.Match(_ => { }, _ => { });

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.ShouldBeSameAs(sut);
    }

    #endregion

    #region Match (Result<T, TNext>, Func<T, TNext>, Action<Error>)

    [Fact]
    public void Match_NoGenericResultWithGenericSuccessFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.Match(x => x, _ => { }));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulGenericResultWithNoGenericSuccessFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<int, int>? Transform = null;

        Result<int> sut = 1;

        // Act
        var exception = Record.Exception(() => sut.Match(Transform!, _ => { }));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulGenericResultWithGenericSuccessFunctionAndNoErrorAction_ThrowsException()
    {
        // Arrange
        const Action<Error>? ErrorAction = null;

        Result<int> sut = 1;

        // Act
        var exception = Record.Exception(() => sut.Match(x => x, ErrorAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulGenericResultWithGenericSuccessFunctionAndErrorAction_CallsSuccessFunction()
    {
        // Arrange
        const int Value = 15;
        var successFunctionMock = new Mock<Func<int, int>>();

        Result<int> sut = Value;

        // Act
        sut.Match(successFunctionMock.Object, _ => { });

        // Assert
        successFunctionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public void Match_SuccessfulGenericResultWithGenericSuccessFunctionAndErrorAction_DoesNotCallErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Action<Error>>();

        Result<int> sut = 15;

        // Act
        sut.Match(x => x, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public void Match_SuccessfulGenericResultWithGenericSuccessFunctionAndErrorAction_ReturnsSuccessFunctionValue()
    {
        // Arrange
        const int Value = 15;
        const int ExpectedValue = 30;

        Result<int> sut = Value;

        // Act
        var result = sut.Match(x => 2 * x, _ => { });

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public void Match_ErrorGenericResultWithGenericSuccessFunctionAndErrorAction_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<int, int>>();

        var sut = GetErrorResult<int>();

        // Act
        sut.Match(successFunctionMock.Object, _ => { });

        // Assert
        successFunctionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void Match_ErrorGenericResultWithGenericSuccessFunctionAndErrorAction_CallsErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Action<Error>>();

        var sut = GetErrorResult<int>();

        // Act
        sut.Match(x => x, errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public void Match_ErrorGenericResultWithGenericSuccessFunctionAndErrorAction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = sut.Match(x => x, _ => { });

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region Match (Result<T>, Func<T, TNext>, Func<Error, TNext>)

    [Fact]
    public void Match_NoGenericResultWithGenericSuccessAndErrorFunctions_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.Match(x => x.ToString(), _ => "error"));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulGenericResultWithNoGenericSuccessFunctionAndGenericErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<int, string>? SuccessFunction = null;

        Result<int> sut = 1;

        // Act
        var exception = Record.Exception(() => sut.Match(SuccessFunction!, _ => "error"));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulGenericResultWithGenericSuccessFunctionAndNoGenericErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, string>? ErrorFunction = null;

        Result<int> sut = 1;

        // Act
        var exception = Record.Exception(() => sut.Match(x => x.ToString(), ErrorFunction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulGenericResultWithGenericSuccessAndErrorFunctions_CallsSuccessFunction()
    {
        // Arrange
        const int Value = 5;
        var successFunctionMock = new Mock<Func<int, string>>();
        successFunctionMock
            .Setup(x => x.Invoke(It.IsAny<int>()))
            .Returns("success value");

        Result<int> sut = Value;

        // Act
        sut.Match(successFunctionMock.Object, _ => "error");

        // Assert
        successFunctionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public void Match_SuccessfulGenericResultWithGenericSuccessAndErrorFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, string>>();

        Result<int> sut = 5;

        // Act
        sut.Match(x => x.ToString(), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public void Match_SuccessfulGenericResultWithGenericSuccessAndErrorFunctions_ReturnsSuccessFunctionValue()
    {
        // Arrange
        const int Value = 5;
        const string ExpectedValue = "6";

        Result<int> sut = Value;

        // Act
        var result = sut.Match(x => (x + 1).ToString(), _ => "error");

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public void Match_ErrorGenericResultWithProjectionFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<int, string>>();

        var sut = GetErrorResult<int>();

        // Act
        sut.Match(successFunctionMock.Object, _ => "error");

        // Assert
        successFunctionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void Match_ErrorGenericResultWithProjectionFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, string>>();
        errorFunctionMock
            .Setup(x => x.Invoke(It.IsAny<Error>()))
            .Returns("error value");

        var sut = GetErrorResult<int>();

        // Act
        sut.Match(x => x.ToString(), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public void Match_ErrorGenericResultWithProjectionFunctions_ReturnsErrorFunctionValue()
    {
        // Arrange
        const string ExpectedValue = "Error function value";

        var sut = GetErrorResult<int>();

        // Act
        var result = sut.Match(x => x.ToString(), _ => ExpectedValue);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    #endregion

    #region Match (Result<T>, Func<T, Result<TNext>>, Action<Error>)

    [Fact]
    public void Match_NoGenericResultWithGenericResultSuccessFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.Match(x => Result.FromValue(x.ToString()), _ => { }));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulGenericResultWithNoGenericResultSuccessFunctionAndErrorAction_ThrowsException()
    {
        // Arrange
        const Func<int, Result<string>>? SuccessFunction = null;

        Result<int> sut = 1;

        // Act
        var exception = Record.Exception(() => sut.Match(SuccessFunction!, _ => { }));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulGenericResultWithGenericResultSuccessFunctionAndNoErrorAction_ThrowsException()
    {
        // Arrange
        const Action<Error>? ErrorAction = null;

        Result<int> sut = 1;

        // Act
        var exception = Record.Exception(() => sut.Match(x => Result.FromValue(x.ToString()), ErrorAction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulGenericResultWithGenericResultSuccessFunctionAndErrorAction_CallsSuccessFunction()
    {
        // Arrange
        const int Value = 88;
        var successFunctionMock = new Mock<Func<int, Result<string>>>();

        Result<int> sut = 88;

        // Act
        sut.Match(successFunctionMock.Object, _ => { });

        // Assert
        successFunctionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public void Match_SuccessfulGenericResultWithGenericResultSuccessFunctionAndErrorAction_DoesNotCallErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Action<Error>>();

        Result<int> sut = 88;

        // Act
        sut.Match(x => Result.FromValue(x.ToString()), errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public void Match_SuccessfulGenericResultWithGenericResultSuccessFunctionAndErrorAction_ReturnsSuccessFunctionValueResult()
    {
        // Arrange
        const string ExpectedValue = "90";
        const int Value = 88;
        var expectedResult = Result.FromValue(ExpectedValue);

        Result<int> sut = Value;

        // Act
        var result = sut.Match(x => Result.FromValue((x + 2).ToString()), _ => { });

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.ShouldBe(expectedResult);
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public void Match_ErrorGenericResultWithGenericResultSuccessFunctionAndErrorAction_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<int, Result<string>>>();

        var sut = GetErrorResult<int>();

        // Act
        sut.Match(successFunctionMock.Object, _ => { });

        // Assert
        successFunctionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void Match_ErrorGenericResultWithGenericResultSuccessFunctionAndErrorAction_CallsErrorAction()
    {
        // Arrange
        var errorActionMock = new Mock<Action<Error>>();

        var sut = GetErrorResult<int>();

        // Act
        sut.Match(x => Result.FromValue(x.ToString()), errorActionMock.Object);

        // Assert
        errorActionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public void Match_ErrorGenericResultWithGenericResultSuccessFunctionAndErrorAction_ReturnsOriginalValueResultWithError()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = sut.Match(x => Result.FromValue(x.ToString()), _ => { });

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region Match (Result<T>, Func<T, Result<TNext>>, Func<Error, Result<TNext>>)

    [Fact]
    public void Match_NoGenericResultWithGenericResultSuccessAndErrorFunctions_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = Record.Exception(
            () => Sut!.Match(x => Result.FromValue(x.ToString()), _ => Result.FromValue("error value")));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulGenericResultWithNoGenericResultSuccessFunctionAndGenericResultErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<int, Result<string>>? SuccessFunction = null;

        Result<int> sut = 1;

        // Act
        var exception = Record.Exception(() => sut.Match(SuccessFunction!, _ => Result.FromValue("error value")));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulGenericResultWithGenericResultSuccessFunctionAndNoGenericResultErrorFunction_ThrowsException()
    {
        // Arrange
        const Func<Error, Result<string>>? ErrorFunction = null;

        Result<int> sut = 1;

        // Act
        var exception = Record.Exception(() => sut.Match(x => Result.FromValue(x.ToString()), ErrorFunction!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void Match_SuccessfulGenericResultWithGenericResultSuccessAndErrorFunctions_CallsSuccessFunction()
    {
        // Arrange
        const int Value = 90;
        var successFunctionMock = new Mock<Func<int, Result<string>>>();

        Result<int> sut = Value;

        // Act
        sut.Match(successFunctionMock.Object, _ => Result.FromValue("error value"));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(Value), Times.Once);
    }

    [Fact]
    public void Match_SuccessfulGenericResultWithGenericResultSuccessAndErrorFunctions_DoesNotCallErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, Result<string>>>();

        Result<int> sut = 90;

        // Act
        sut.Match(x => Result.FromValue(x.ToString()), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
    }

    [Fact]
    public void Match_SuccessfulGenericResultWithGenericResultSuccessAndErrorFunctions_ReturnsSuccessFunctionValueResult()
    {
        // Arrange
        const string ExpectedValue = "-90-";
        const int Value = 90;

        Result<int> sut = Value;

        // Act
        var result = sut.Match(x => Result.FromValue($"-{x}-"), _ => Result.FromValue("error value"));

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public void Match_ErrorGenericResultWithGenericResultSuccessAndErrorFunctions_DoesNotCallSuccessFunction()
    {
        // Arrange
        var successFunctionMock = new Mock<Func<int, Result<string>>>();

        var sut = GetErrorResult<int>();

        // Act
        sut.Match(successFunctionMock.Object, _ => Result.FromValue("error value"));

        // Assert
        successFunctionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void Match_ErrorGenericResultWithGenericResultSuccessAndErrorFunctions_CallsErrorFunction()
    {
        // Arrange
        var errorFunctionMock = new Mock<Func<Error, Result<string>>>();

        var sut = GetErrorResult<int>();

        // Act
        sut.Match(x => Result.FromValue(x.ToString()), errorFunctionMock.Object);

        // Assert
        errorFunctionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
    }

    [Fact]
    public void Match_ErrorGenericResultWithGenericResultSuccessAndErrorFunctions_ReturnsErrorFunctionValueResult()
    {
        // Arrange
        const string ExpectedValue = "error value";

        var sut = GetErrorResult<int>();

        // Act
        var result = sut.Match(x => Result.FromValue(x.ToString()), _ => Result.FromValue(ExpectedValue));

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