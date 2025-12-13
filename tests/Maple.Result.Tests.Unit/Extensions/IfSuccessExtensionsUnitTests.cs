using System;
using Maple.Result.Extensions;
using Maple.Result.Tests.Unit.Helpers;
using Moq;

namespace Maple.Result.Tests.Unit.Extensions;

public class IfSuccessExtensionsUnitTests
{
    #region read-only fields

    private readonly Mock<ITest> _testMock;

    #endregion

    private ITest TestObj => _testMock.Object;

    #region set up

    public IfSuccessExtensionsUnitTests()
    {
        _testMock = new Mock<ITest>();
    }

    #endregion

    #region IfSuccess (Result, Action)

    [Fact]
    public void IfSuccess_NoResultWithAction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut.IfSuccess(() => TestObj.Action()));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithNoAction_ThrowsException()
    {
        // Arrange
        var sut = Result.Success();
        const Action? Action = null;

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Action));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_ErrorResultWithNoAction_ThrowsException()
    {
        // Arrange
        var sut = GetErrorResult();
        const Action? Action = null;

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Action));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithAction_CallsAction()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        _ = sut.IfSuccess(() => TestObj.Action());

        // Assert
        _testMock.Verify(x => x.Action(), Times.Once);
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithAction_ReturnsSuccessfulResult()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = sut.IfSuccess(() => TestObj.Action());

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeTrue();
    }

    [Fact]
    public void IfSuccess_ErrorResultWithAction_DoesNotCallAction()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        _ = sut.IfSuccess(() => TestObj.Action());

        // Assert
        _testMock.Verify(x => x.Action(), Times.Never);
    }

    [Fact]
    public void IfSuccess_ErrorResultWithAction_ReturnsErrorResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = sut.IfSuccess(() => TestObj.Action());

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfSuccess (Result, Func<Result>)

    [Fact]
    public void IfSuccess_NoResultWithResultFunction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut.IfSuccess(() => TestObj.ResultFunc()));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        var sut = Result.Success();
        const Func<Result>? Function = null;

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_ErrorResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        var sut = GetErrorResult();
        const Func<Result>? Function = null;

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithResultFunction_CallsFunction()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        _ = sut.IfSuccess(() => TestObj.ResultFunc());

        // Assert
        _testMock.Verify(x => x.ResultFunc(), Times.Once);
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithSuccessfulResultFunction_ReturnsSuccessfulResult()
    {
        // Arrange
        var sut = Result.Success();
        _testMock
            .Setup(x => x.ResultFunc())
            .Returns(Result.Success());

        // Act
        var result = sut.IfSuccess(() => TestObj.ResultFunc());

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeTrue();
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithErrorResultFunction_ReturnsErrorResultWithFunctionError()
    {
        // Arrange
        var sut = Result.Success();
        var errorResult = GetErrorResult();
        _testMock
            .Setup(x => x.ResultFunc())
            .Returns(errorResult);

        // Act
        var result = sut.IfSuccess(() => TestObj.ResultFunc());

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(errorResult.Error);
    }

    [Fact]
    public void IfSuccess_ErrorResultWithResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        _ = sut.IfSuccess(() => TestObj.ResultFunc());

        // Assert
        _testMock.Verify(x => x.ResultFunc(), Times.Never);
    }

    [Fact]
    public void IfSuccess_ErrorResultWithResultFunction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = sut.IfSuccess(() => TestObj.ResultFunc());

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfSuccess (Result, Func<T>)

    [Fact]
    public void IfSuccess_NoResultWithIntFunction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut.IfSuccess(() => TestObj.IntFunc()));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithNoIntFunction_ThrowsException()
    {
        // Arrange
        var sut = Result.Success();
        const Func<int>? Function = null;

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_ErrorResultWithNoIntFunction_ThrowsException()
    {
        // Arrange
        var sut = GetErrorResult();
        const Func<int>? Function = null;

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithIntFunction_CallsFunction()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        _ = sut.IfSuccess(() => TestObj.IntFunc());

        // Assert
        _testMock.Verify(x => x.IntFunc(), Times.Once);
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithIntFunction_ReturnsSuccessfulResultWithInt()
    {
        // Arrange
        const int Value = 2463;
        var sut = Result.Success();
        _testMock
            .Setup(x => x.IntFunc())
            .Returns(Value);

        // Act
        var result = sut.IfSuccess(() => TestObj.IntFunc());

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(Value);
    }

    [Fact]
    public void IfSuccess_ErrorResultWithIntFunction_DoesNotCallFunction()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        _ = sut.IfSuccess(() => TestObj.IntFunc());

        // Assert
        _testMock.Verify(x => x.IntFunc(), Times.Never);
    }

    [Fact]
    public void IfSuccess_ErrorResultWithIntFunction_ReturnsErrorResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = sut.IfSuccess(() => TestObj.IntFunc());

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfSuccess (Result, Func<Result<T>>)

    [Fact]
    public void IfSuccess_NoResultWithIntResultFunction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut.IfSuccess(() => TestObj.IntResultFunc()));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithNoIntResultFunction_ThrowsException()
    {
        // Arrange
        var sut = Result.Success();
        const Func<Result<int>>? Function = null;

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_ErrorResultWithNoIntResultFunction_ThrowsException()
    {
        // Arrange
        var sut = GetErrorResult();
        const Func<Result<int>>? Function = null;

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithResultIntFunction_CallsFunction()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        _ = sut.IfSuccess(() => TestObj.IntResultFunc());

        // Assert
        _testMock.Verify(x => x.IntResultFunc(), Times.Once);
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithSuccessfulResultIntFunction_ReturnsSuccessfulResultWithInt()
    {
        // Arrange
        const int Value = 2463;
        var sut = Result.Success();
        _testMock
            .Setup(x => x.IntResultFunc())
            .Returns(Value);

        // Act
        var result = sut.IfSuccess(() => TestObj.IntResultFunc());

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(Value);
    }

    [Fact]
    public void IfSuccess_SuccessfulResultWithErrorResultIntFunction_ReturnsErrorResultWithFunctionError()
    {
        // Arrange
        var error = GetError1();
        var sut = Result.Success();
        _testMock
            .Setup(x => x.IntResultFunc())
            .Returns(error);

        // Act
        var result = sut.IfSuccess(() => TestObj.IntResultFunc());

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(error);
    }

    [Fact]
    public void IfSuccess_ErrorResultWithResultIntFunction_DoesNotCallFunction()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        _ = sut.IfSuccess(() => TestObj.IntResultFunc());

        // Assert
        _testMock.Verify(x => x.IntResultFunc(), Times.Never);
    }

    [Fact]
    public void IfSuccess_ErrorResultWithResultIntFunction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = sut.IfSuccess(() => TestObj.IntResultFunc());

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfSuccess (Result<T>, Action<T>)

    [Fact]
    public void IfSuccess_NoIntResultWithIntAction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut.IfSuccess(x => TestObj.IntAction(x)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulIntResultWithNoIntAction_ThrowsException()
    {
        // Arrange
        const int Value = 35;
        Result<int> sut = Value;
        const Action<int>? Action = null;

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Action));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_ErrorIntResultWithNoIntAction_ThrowsException()
    {
        // Arrange
        var sut = GetErrorResult<int>();
        const Action<int>? Action = null;

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Action));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulIntResultWithIntAction_CallsAction()
    {
        // Arrange
        const int Value = 35;
        Result<int> sut = Value;

        // Act
        _ = sut.IfSuccess(x => TestObj.IntAction(x));

        // Assert
        _testMock.Verify(x => x.IntAction(Value), Times.Once);
    }

    [Fact]
    public void IfSuccess_SuccessfulIntResultWithIntAction_ReturnsResultWithOriginalIntValue()
    {
        // Arrange
        const int Value = 35;
        Result<int> sut = Value;

        // Act
        var result = sut.IfSuccess(x => TestObj.IntAction(x));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(Value);
    }

    [Fact]
    public void IfSuccess_ErrorIntResultWithIntAction_DoesNotCallAction()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        _ = sut.IfSuccess(x => TestObj.IntAction(x));

        // Assert
        _testMock.Verify(x => x.IntAction(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void IfSuccess_ErrorIntResultWithIntAction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = sut.IfSuccess(x => TestObj.IntAction(x));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfSuccess (Result<T>, Func<T, Result>)

    [Fact]
    public void IfSuccess_NoStringResultWithResultFunction_ThrowsException()
    {
        // Arrange
        const Result<string>? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut.IfSuccess((x => TestObj.ResultFunc(x))));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulStringResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const string Value = "Start";
        Result<string> sut = Value;
        const Func<string, Result>? Function = null;

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_ErrorStringResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        var sut = GetErrorResult<string>();
        const Func<string, Result>? Function = null;

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulStringResultWithResultFunction_CallsFunction()
    {
        // Arrange
        const string Value = "Start";
        Result<string> sut = Value;

        // Act
        _ = sut.IfSuccess(x => TestObj.ResultFunc(x));

        // Assert
        _testMock.Verify(x => x.ResultFunc(Value), Times.Once);
    }

    [Fact]
    public void IfSuccess_SuccessfulStringResultWithSuccessfulResultFunction_ReturnsResultWithNewResultValue()
    {
        // Arrange
        const string Value = "Start";

        _testMock
            .Setup(x => x.ResultFunc(Value))
            .Returns(Result.Success());

        Result<string> sut = Value;

        // Act
        var result = sut.IfSuccess(x => TestObj.ResultFunc(x));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeTrue();
    }

    [Fact]
    public void IfSuccess_SuccessfulStringResultWithErrorResultFunction_ReturnsErrorResultWithFunctionError()
    {
        // Arrange
        var errorResult = GetErrorResult();
        const string Value = "Start";

        _testMock
            .Setup(x => x.ResultFunc(Value))
            .Returns(errorResult);

        Result<string> sut = Value;

        // Act
        var result = sut.IfSuccess(x => TestObj.ResultFunc(x));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(errorResult.Error);
    }

    [Fact]
    public void IfSuccess_ErrorStringResultWithResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var sut = GetErrorResult<string>();

        // Act
        _ = sut.IfSuccess(x => TestObj.ResultFunc(x));

        // Assert
        _testMock.Verify(x => x.ResultFunc(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void IfSuccess_ErrorStringResultWithResultFunction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult<string>();

        // Act
        var result = sut.IfSuccess(x => TestObj.ResultFunc(x));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfSuccess (Result<T>, Func<T, TNext>)

    [Fact]
    public void IfSuccess_NoDoubleResultWithIntFunction_ThrowsException()
    {
        // Arrange
        const Result<double>? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut.IfSuccess((x => TestObj.IntFunc(x))));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulDoubleResultWithNoIntFunction_ThrowsException()
    {
        // Arrange
        const double Value = 12.34;
        Result<double> sut = Value;
        const Func<double, int>? Function = null;

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_ErrorDoubleResultWithNoIntFunction_ThrowsException()
    {
        // Arrange
        var sut = GetErrorResult<double>();
        const Func<double, int>? Function = null;

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulDoubleResultWithIntFunction_CallsFunction()
    {
        // Arrange
        const double Value = 12.34;
        Result<double> sut = Value;

        // Act
        _ = sut.IfSuccess(x => TestObj.IntFunc(x));

        // Assert
        _testMock.Verify(x => x.IntFunc(Value), Times.Once);
    }

    [Fact]
    public void IfSuccess_SuccessfulDoubleResultWithIntFunction_ReturnsResultWithIntValue()
    {
        // Arrange
        const int ExpectedValue = 4937;
        const double Value = 12.34;

        Result<double> sut = Value;
        _testMock
            .Setup(x => x.IntFunc(Value))
            .Returns(ExpectedValue);

        // Act
        var result = sut.IfSuccess(x => TestObj.IntFunc(x));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public void IfSuccess_ErrorDoubleResultWithIntFunction_DoesNotCallFunction()
    {
        // Arrange
        var sut = GetErrorResult<double>();

        // Act
        _ = sut.IfSuccess(x => TestObj.IntFunc(x));

        // Assert
        _testMock.Verify(x => x.IntFunc(It.IsAny<double>()), Times.Never);
    }

    [Fact]
    public void IfSuccess_ErrorDoubleResultWithIntFunction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult<double>();

        // Act
        var result = sut.IfSuccess(x => TestObj.IntFunc(x));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfSuccess (Result<T>, Func<T, Result<TNext>>)

    [Fact]
    public void IfSuccess_NoIntResultWithStringResultFunction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = Record.Exception(() => Sut.IfSuccess(x => TestObj.StringResultFunc(x)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulIntResultWithNoStringResultFunction_ThrowsException()
    {
        // Arrange
        const int Value = 39;
        Result<int> sut = Value;
        const Func<int, Result<string>>? Function = null;

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_ErrorIntResultWithNoStringResultFunction_ThrowsException()
    {
        // Arrange
        var sut = GetErrorResult<int>();
        const Func<int, Result<string>>? Function = null;

        // Act
        var exception = Record.Exception(() => sut.IfSuccess(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public void IfSuccess_SuccessfulIntResultWithStringResultFunction_CallsFunction()
    {
        // Arrange
        const int Value = 39;
        Result<int> sut = Value;

        // Act
        _ = sut.IfSuccess(x => TestObj.StringResultFunc(x));

        // Assert
        _testMock.Verify(x => x.StringResultFunc(Value), Times.Once);
    }

    [Fact]
    public void IfSuccess_SuccessfulIntResultWithSuccessfulStringResultFunction_ReturnsStringResult()
    {
        // Arrange
        const string ExpectedValue = "New Value";
        const int Value = 49;

        _testMock
            .Setup(x => x.StringResultFunc(Value))
            .Returns(Result<string>.FromValue(ExpectedValue));

        Result<int> sut = Value;

        // Act
        var result = sut.IfSuccess(x => TestObj.StringResultFunc(x));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public void IfSuccess_SuccessfulIntResultWithErrorStringResultFunction_ReturnsErrorResultWithFunctionError()
    {
        // Arrange
        const int Value = 49;
        var errorResult = GetErrorResult<string>();

        _testMock
            .Setup(x => x.StringResultFunc(Value))
            .Returns(errorResult);

        Result<int> sut = Value;

        // Act
        var result = sut.IfSuccess(x => TestObj.StringResultFunc(x));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(errorResult.Error);
    }

    [Fact]
    public void IfSuccess_ErrorIntResultWithStringResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        _ = sut.IfSuccess(x => TestObj.StringResultFunc(x));

        // Assert
        _testMock.Verify(x => x.StringResultFunc(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public void IfSuccess_ErrorIntResultWithStringResultFunction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = sut.IfSuccess(x => TestObj.StringResultFunc(x));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region helper methods

    private static Error GetError1()
    {
        return Error.Failure(ErrorUri.None(), "e06d75de-115c-46ec-a5f6-f6373007ec61", "Error title 1");
    }

    private static Error GetError2()
    {
        return Error.Failure(ErrorUri.None(), "8f5969ac-f23b-46e1-84f4-530b209c07df", "Error title 2");
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
