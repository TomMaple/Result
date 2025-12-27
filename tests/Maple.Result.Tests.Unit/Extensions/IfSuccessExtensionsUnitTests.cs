using Maple.Result.Extensions;
using Moq;
using System;

namespace Maple.Result.Tests.Unit.Extensions;

public class IfSuccessExtensionsUnitTests
{
    #region IfSuccess (Result, Action)

    [Fact]
    public void IfSuccess_NoResultWithAction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.IfSuccess(() => { }));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithNoAction_ThrowsException()
    {
        // Arrange
        const Action? Action = null;

        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Action!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_ErrorResultWithNoAction_ThrowsException()
    {
        // Arrange
        const Action? Action = null;

        var sut = GetErrorResult();

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Action!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithAction_CallsAction()
    {
        // Arrange
        var actionMock = new Mock<Action>();

        var sut = Result.Success();

        // Act
        sut.IfSuccess(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithAction_ReturnsOriginalSuccessfulResult()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = sut.IfSuccess(() => { });

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeTrue();
        result.ShouldBeSameAs(sut);
    }

    [Fact]
    public void IfSuccess_ErrorResultWithAction_DoesNotCallAction()
    {
        // Arrange
        var actionMock = new Mock<Action>();

        var sut = GetErrorResult();

        // Act
        sut.IfSuccess(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public void IfSuccess_ErrorResultWithAction_ReturnsErrorResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = sut.IfSuccess(() => { });

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region IfSuccess (Result, Func<Result>)

    [Fact]
    public void IfSuccess_NoResultWithResultFunction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.IfSuccess(Result.Success));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Result>? Function = null;

        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_ErrorResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Result>? Function = null;

        var sut = GetErrorResult();

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithResultFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Result>>();

        var sut = Result.Success();

        // Act
        sut.IfSuccess(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithSuccessfulResultFunction_ReturnsSuccessfulResult()
    {
        // Arrange
        var funcResult = Result.Success();

        var sut = Result.Success();

        // Act
        var result = sut.IfSuccess(() => funcResult);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeTrue();
        result.ShouldBeSameAs(funcResult);
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithErrorResultFunction_ReturnsErrorResultWithFunctionError()
    {
        // Arrange
        var funcError = GetErrorResult();

        var sut = Result.Success();

        // Act
        var result = sut.IfSuccess(() => funcError);

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(funcError.Error);
    }

    [Fact]
    public void IfSuccess_ErrorResultWithResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Result>>();

        var sut = GetErrorResult();

        // Act
        sut.IfSuccess(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public void IfSuccess_ErrorResultWithResultFunction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = sut.IfSuccess(Result.Success);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region IfSuccess (Result, Func<T>)

    [Fact]
    public void IfSuccess_NoResultWithIntFunction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.IfSuccess(() => 38));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithNoIntFunction_ThrowsException()
    {
        // Arrange
        const Func<int>? Function = null;

        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_ErrorResultWithNoIntFunction_ThrowsException()
    {
        // Arrange
        const Func<int>? Function = null;

        var sut = GetErrorResult();

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithIntFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<int>>();

        var sut = Result.Success();

        // Act
        sut.IfSuccess(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithIntFunction_ReturnsSuccessfulResultWithInt()
    {
        // Arrange
        const int FuncValue = 2463;

        var sut = Result.Success();

        // Act
        var result = sut.IfSuccess(() => FuncValue);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(FuncValue);
    }

    [Fact]
    public void IfSuccess_ErrorResultWithIntFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<int>>();

        var sut = GetErrorResult();

        // Act
        sut.IfSuccess(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public void IfSuccess_ErrorResultWithIntFunction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = sut.IfSuccess(() => 2987);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region IfSuccess (Result, Func<Result<T>>)

    [Fact]
    public void IfSuccess_NoResultWithGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.IfSuccess(() => Result.FromValue(897)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Result<int>>? Function = null;

        var sut = Result.Success();

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_ErrorResultWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Func<Result<int>>? Function = null;

        var sut = GetErrorResult();

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithResultIntFunction_CallsFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Result<int>>>();

        var sut = Result.Success();

        // Act
        sut.IfSuccess(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Once);
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithSuccessfulResultIntFunction_ReturnsSuccessfulResultWithInt()
    {
        // Arrange
        const int FuncValue = 2463;

        var sut = Result.Success();

        // Act
        var result = sut.IfSuccess(() => Result.FromValue(FuncValue));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(FuncValue);
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithErrorResultIntFunction_ReturnsErrorResultWithFunctionError()
    {
        // Arrange
        var funcError = GetError();

        var sut = Result.Success();

        // Act
        var result = sut.IfSuccess(() => Result<int>.FromError(funcError));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(funcError);
    }

    [Fact]
    public void IfSuccess_ErrorResultWithResultIntFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<Result<int>>>();

        var sut = GetErrorResult();

        // Act
        sut.IfSuccess(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(), Times.Never);
    }

    [Fact]
    public void IfSuccess_ErrorResultWithResultIntFunction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = sut.IfSuccess(() => Result.FromValue(321));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region IfSuccess (Result<T>, Action<T>)

    [Fact]
    public void IfSuccess_NoGenericResultWithIntAction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.IfSuccess(_ => { }));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulGenericResultWithNoIntAction_ThrowsException()
    {
        // Arrange
        const int InitialValue = 35;
        const Action<int>? Action = null;
        
        Result<int> sut = InitialValue;

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Action!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_ErrorGenericResultWithNoIntAction_ThrowsException()
    {
        // Arrange
        const Action<int>? Action = null;

        var sut = GetErrorResult<int>();

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Action!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulGenericResultWithIntAction_CallsAction()
    {
        // Arrange
        const int InitialValue = 35;
        var actionMock = new Mock<Action<int>>();

        Result<int> sut = InitialValue;

        // Act
        sut.IfSuccess(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(InitialValue), Times.Once);
    }

    [Fact]
    public void IfSuccess_SuccessfulGenericResultWithIntAction_ReturnsResultWithOriginalIntValue()
    {
        // Arrange
        const int InitialValue = 35;

        Result<int> sut = InitialValue;

        // Act
        var result = sut.IfSuccess(_ => { });

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(InitialValue);
    }

    [Fact]
    public void IfSuccess_ErrorGenericResultWithIntAction_DoesNotCallAction()
    {
        // Arrange
        var actionMock = new Mock<Action<int>>();

        var sut = GetErrorResult<int>();

        // Act
        sut.IfSuccess(actionMock.Object);

        // Assert
        actionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void IfSuccess_ErrorGenericResultWithIntAction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = sut.IfSuccess(_ => { });

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region IfSuccess (Result<T>, Func<T, Result>)

    [Fact]
    public void IfSuccess_NoGenericResultWithResultFunction_ThrowsException()
    {
        // Arrange
        const Result<string>? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.IfSuccess((_ => Result.Success())));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulGenericResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const string InitialValue = "Start";
        Result<string> sut = InitialValue;

        const Func<string, Result>? Function = null;

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_ErrorGenericResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const Func<string, Result>? Function = null;

        var sut = GetErrorResult<string>();

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulGenericResultWithResultFunction_CallsFunction()
    {
        // Arrange
        const string InitialValue = "Start";
        var functionMock = new Mock<Func<string, Result>>();

        Result<string> sut = InitialValue;

        // Act
        sut.IfSuccess(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(InitialValue), Times.Once);
    }

    [Fact]
    public void IfSuccess_SuccessfulGenericResultWithSuccessfulResultFunction_ReturnsResultWithNewResultValue()
    {
        // Arrange
        const string InitialValue = "Start";
        var expectedResult = Result.Success();

        Result<string> sut = InitialValue;

        // Act
        var result = sut.IfSuccess(_ => expectedResult);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeTrue();
        result.ShouldBeSameAs(expectedResult);
    }

    [Fact]
    public void IfSuccess_SuccessfulGenericResultWithErrorResultFunction_ReturnsErrorResultWithFunctionError()
    {
        // Arrange
        var funcError = GetErrorResult();
        const string Value = "Start";

        Result<string> sut = Value;

        // Act
        var result = sut.IfSuccess(_ => funcError);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(funcError.Error);
    }

    [Fact]
    public void IfSuccess_ErrorGenericResultWithResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<string, Result>>();

        var sut = GetErrorResult<string>();

        // Act
        sut.IfSuccess(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void IfSuccess_ErrorGenericResultWithResultFunction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult<string>();

        // Act
        var result = sut.IfSuccess(_ => Result.Success());

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region IfSuccess (Result<T>, Func<T, TNext>)

    [Fact]
    public void IfSuccess_NoGenericResultWithIntFunction_ThrowsException()
    {
        // Arrange
        const Result<double>? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.IfSuccess(_ => 428));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulGenericResultWithNoIntFunction_ThrowsException()
    {
        // Arrange
        const double InitialValue = 12.34;
        const Func<double, int>? Function = null;

        Result<double> sut = InitialValue;

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_ErrorGenericResultWithNoIntFunction_ThrowsException()
    {
        // Arrange
        const Func<double, int>? Function = null;

        var sut = GetErrorResult<double>();

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulGenericResultWithIntFunction_CallsFunction()
    {
        // Arrange
        const double InitialValue = 12.34;
        var functionMock = new Mock<Func<double, int>>();

        Result<double> sut = InitialValue;

        // Act
        sut.IfSuccess(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(InitialValue), Times.Once);
    }

    [Fact]
    public void IfSuccess_SuccessfulGenericResultWithIntFunction_ReturnsResultWithIntValue()
    {
        // Arrange
        const double InitialValue = 12.34;
        const int FuncValue = 4937;

        Result<double> sut = InitialValue;

        // Act
        var result = sut.IfSuccess(_ => FuncValue);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(FuncValue);
    }

    [Fact]
    public void IfSuccess_ErrorGenericResultWithIntFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<double, int>>();

        var sut = GetErrorResult<double>();

        // Act
        sut.IfSuccess(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<double>()), Times.Never);
    }

    [Fact]
    public void IfSuccess_ErrorGenericResultWithIntFunction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult<double>();

        // Act
        var result = sut.IfSuccess(_ => 4873);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
    }

    #endregion

    #region IfSuccess (Result<T>, Func<T, Result<TNext>>)

    [Fact]
    public void IfSuccess_NoGenericResultWithGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut!.IfSuccess(x => Result.FromValue("new text value")));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulGenericResultWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const int InitialValue = 39;
        const Func<int, Result<string>>? Function = null;

        Result<int> sut = InitialValue;

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_ErrorGenericResultWithNoGenericResultFunction_ThrowsException()
    {
        // Arrange
        const Func<int, Result<string>>? Function = null;
        
        var sut = GetErrorResult<int>();

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Function!));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulGenericResultWithGenericResultFunction_CallsFunction()
    {
        // Arrange
        const int InitialValue = 39;
        var functionMock = new Mock<Func<int,  Result<string>>>();

        Result<int> sut = InitialValue;

        // Act
        sut.IfSuccess(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(InitialValue), Times.Once);
    }

    [Fact]
    public void IfSuccess_SuccessfulGenericResultWithSuccessfulGenericResultFunction_ReturnsGenericResult()
    {
        // Arrange
        const int InitialValue = 49;
        const string FuncValue = "New Value";

        Result<int> sut = InitialValue;

        // Act
        var result = sut.IfSuccess(_ => Result.FromValue(FuncValue));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(FuncValue);
    }

    [Fact]
    public void IfSuccess_SuccessfulGenericResultWithErrorGenericResultFunction_ReturnsErrorResultWithFunctionError()
    {
        // Arrange
        const int InitialValue = 49;
        var funcErrorResult = GetErrorResult<string>();

        Result<int> sut = InitialValue;

        // Act
        var result = sut.IfSuccess(_ => funcErrorResult);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(funcErrorResult.Error);
    }

    [Fact]
    public void IfSuccess_ErrorGenericResultWithGenericResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var functionMock = new Mock<Func<int, Result<string>>>();

        var sut = GetErrorResult<int>();

        // Act
        sut.IfSuccess(functionMock.Object);

        // Assert
        functionMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void IfSuccess_ErrorGenericResultWithGenericResultFunction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = sut.IfSuccess(_ => Result.FromValue("new text value"));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBeSameAs(sut.Error);
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
