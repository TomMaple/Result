using Maple.Result.Extensions;
using Maple.Result.Tests.Unit.Helpers;
using Moq;
using System;
using System.Threading.Tasks;

namespace Maple.Result.Tests.Unit.Extensions;

public class IfSuccessAsyncExtensionsUnitTests
{
    #region read-only fields

    private readonly Mock<ITest> _testMock;

    #endregion

    private ITest TestObj => _testMock.Object;

    #region set up

    public IfSuccessAsyncExtensionsUnitTests()
    {
        _testMock = new Mock<ITest>();
    }

    #endregion

    #region IfSuccessAsync (Result, Func<Task>)

    [Fact]
    public async Task IfSuccessAsync_NoResultWithAction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut.IfSuccessAsync(() => TestObj.ActionAsync()));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithNoAction_ThrowsException()
    {
        // Arrange
        var sut = Result.Success();
        const Func<Task>? ActionAsync = null;

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
        var sut = GetErrorResult();
        const Func<Task>? ActionAsync = null;

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
        var sut = Result.Success();

        // Act
        _ = await sut.IfSuccessAsync(() => TestObj.ActionAsync());

        // Assert
        _testMock.Verify(x => x.ActionAsync(), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithAction_ReturnsSuccessfulResult()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = await sut.IfSuccessAsync(() => TestObj.ActionAsync());

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeTrue();
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithAction_DoesNotCallAction()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        _ = await sut.IfSuccessAsync(() => TestObj.ActionAsync());

        // Assert
        _testMock.Verify(x => x.ActionAsync(), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithAction_ReturnsErrorResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = await sut.IfSuccessAsync(() => TestObj.ActionAsync());

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
        var exception = await Record.ExceptionAsync(() => Sut.IfSuccessAsync(() => TestObj.ResultFuncAsync()));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        var sut = Result.Success();
        const Func<Task<Result>>? Function = null;

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
        var sut = GetErrorResult();
        const Func<Task<Result>>? Function = null;

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
        var sut = Result.Success();

        // Act
        _ = await sut.IfSuccessAsync(() => TestObj.ResultFuncAsync());

        // Assert
        _testMock.Verify(x => x.ResultFuncAsync(), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithSuccessfulResultFunction_ReturnsSuccessfulResult()
    {
        // Arrange
        var sut = Result.Success();
        _testMock
            .Setup(x => x.ResultFuncAsync())
            .ReturnsAsync(Result.Success());

        // Act
        var result = await sut.IfSuccessAsync(() => TestObj.ResultFuncAsync());

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeTrue();
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithErrorResultFunction_ReturnsErrorResultWithFunctionError()
    {
        // Arrange
        var sut = Result.Success();
        var errorResult = GetErrorResult();
        _testMock
            .Setup(x => x.ResultFuncAsync())
            .ReturnsAsync(errorResult);

        // Act
        var result = await sut.IfSuccessAsync(() => TestObj.ResultFuncAsync());

        // Assert
        result.ShouldNotBeNull();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(errorResult.Error);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        _ = await sut.IfSuccessAsync(() => TestObj.ResultFuncAsync());

        // Assert
        _testMock.Verify(x => x.ResultFuncAsync(), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithResultFunction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = await sut.IfSuccessAsync(() => TestObj.ResultFuncAsync());

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
        var exception = await Record.ExceptionAsync(() => Sut.IfSuccessAsync(() => TestObj.IntFuncAsync()));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithNoIntFunction_ThrowsException()
    {
        // Arrange
        var sut = Result.Success();
        const Func<Task<int>>? Function = null;

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
        var sut = GetErrorResult();
        const Func<Task<int>>? Function = null;

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
        var sut = Result.Success();

        // Act
        _ = await sut.IfSuccessAsync(() => TestObj.IntFuncAsync());

        // Assert
        _testMock.Verify(x => x.IntFuncAsync(), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithIntFunction_ReturnsSuccessfulResultWithInt()
    {
        // Arrange
        const int Value = 2463;
        var sut = Result.Success();
        _testMock
            .Setup(x => x.IntFuncAsync())
            .ReturnsAsync(Value);

        // Act
        var result = await sut.IfSuccessAsync(() => TestObj.IntFuncAsync());

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
        var sut = GetErrorResult();

        // Act
        _ = await sut.IfSuccessAsync(() => TestObj.IntFuncAsync());

        // Assert
        _testMock.Verify(x => x.IntFuncAsync(), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithIntFunction_ReturnsErrorResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = await sut.IfSuccessAsync(() => TestObj.IntFuncAsync());

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfSuccessAsync (Result, Func<Task<Result<T>>>)

    [Fact]
    public async Task IfSuccessAsync_NoResultWithIntResultFunction_ThrowsException()
    {
        // Arrange
        const Result? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut.IfSuccessAsync(() => TestObj.IntResultFuncAsync()));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithNoIntResultFunction_ThrowsException()
    {
        // Arrange
        var sut = Result.Success();
        const Func<Task<Result<int>>>? Function = null;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithNoIntResultFunction_ThrowsException()
    {
        // Arrange
        var sut = GetErrorResult();
        const Func<Task<Result<int>>>? Function = null;

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
        var sut = Result.Success();

        // Act
        _ = await sut.IfSuccessAsync(() => TestObj.IntResultFuncAsync());

        // Assert
        _testMock.Verify(x => x.IntResultFuncAsync(), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulResultWithSuccessfulResultIntFunction_ReturnsSuccessfulResultWithInt()
    {
        // Arrange
        const int Value = 2463;
        var sut = Result.Success();
        _testMock
            .Setup(x => x.IntResultFuncAsync())
            .ReturnsAsync(Value);

        // Act
        var result = await sut.IfSuccessAsync(() => TestObj.IntResultFuncAsync());

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
        var error = GetError1();
        var sut = Result.Success();
        _testMock
            .Setup(x => x.IntResultFuncAsync())
            .ReturnsAsync(error);

        // Act
        var result = await sut.IfSuccessAsync(() => TestObj.IntResultFuncAsync());

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
        var sut = GetErrorResult();

        // Act
        _ = await sut.IfSuccessAsync(() => TestObj.IntResultFuncAsync());

        // Assert
        _testMock.Verify(x => x.IntResultFuncAsync(), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorResultWithResultIntFunction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult();

        // Act
        var result = await sut.IfSuccessAsync(() => TestObj.IntResultFuncAsync());

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfSuccessAsync (Result<T>, Func<T, Task>)

    [Fact]
    public async Task IfSuccessAsync_NoIntResultWithIntAction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut.IfSuccessAsync(x => TestObj.IntActionAsync(x)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulIntResultWithNoIntAction_ThrowsException()
    {
        // Arrange
        const int Value = 35;
        Result<int> sut = Value;
        const Func<int, Task>? ActionAsync = null;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorIntResultWithNoIntAction_ThrowsException()
    {
        // Arrange
        var sut = GetErrorResult<int>();
        const Func<int, Task>? ActionAsync = null;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(ActionAsync));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulIntResultWithIntAction_CallsAction()
    {
        // Arrange
        const int Value = 35;
        Result<int> sut = Value;

        // Act
        _ = await sut.IfSuccessAsync(x => TestObj.IntActionAsync(x));

        // Assert
        _testMock.Verify(x => x.IntActionAsync(Value), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulIntResultWithIntAction_ReturnsResultWithOriginalIntValue()
    {
        // Arrange
        const int Value = 35;
        Result<int> sut = Value;

        // Act
        var result = await sut.IfSuccessAsync(x => TestObj.IntActionAsync(x));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(Value);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorIntResultWithIntAction_DoesNotCallAction()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        _ = await sut.IfSuccessAsync(x => TestObj.IntActionAsync(x));

        // Assert
        _testMock.Verify(x => x.IntActionAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorIntResultWithIntAction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = await sut.IfSuccessAsync(x => TestObj.IntActionAsync(x));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfSuccessAsync (Result<T>, Func<T, Task<Result>>)

    [Fact]
    public async Task IfSuccessAsync_NoStringResultWithResultFunction_ThrowsException()
    {
        // Arrange
        const Result<string>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut.IfSuccessAsync((x => TestObj.ResultFuncAsync(x))));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulStringResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        const string Value = "Start";
        Result<string> sut = Value;
        const Func<string, Task<Result>>? Function = null;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorStringResultWithNoResultFunction_ThrowsException()
    {
        // Arrange
        var sut = GetErrorResult<string>();
        const Func<string, Task<Result>>? Function = null;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulStringResultWithResultFunction_CallsFunction()
    {
        // Arrange
        const string Value = "Start";
        Result<string> sut = Value;

        // Act
        _ = await sut.IfSuccessAsync(x => TestObj.ResultFuncAsync(x));

        // Assert
        _testMock.Verify(x => x.ResultFuncAsync(Value), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulStringResultWithSuccessfulResultFunction_ReturnsResultWithNewResultValue()
    {
        // Arrange
        const string Value = "Start";

        _testMock
            .Setup(x => x.ResultFuncAsync(Value))
            .ReturnsAsync(Result.Success());

        Result<string> sut = Value;

        // Act
        var result = await sut.IfSuccessAsync(x => TestObj.ResultFuncAsync(x));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeTrue();
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulStringResultWithErrorResultFunction_ReturnsErrorResultWithFunctionError()
    {
        // Arrange
        var errorResult = GetErrorResult();
        const string Value = "Start";

        _testMock
            .Setup(x => x.ResultFuncAsync(Value))
            .ReturnsAsync(errorResult);

        Result<string> sut = Value;

        // Act
        var result = await sut.IfSuccessAsync(x => TestObj.ResultFuncAsync(x));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(errorResult.Error);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorStringResultWithResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var sut = GetErrorResult<string>();

        // Act
        _ = await sut.IfSuccessAsync(x => TestObj.ResultFuncAsync(x));

        // Assert
        _testMock.Verify(x => x.ResultFuncAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorStringResultWithResultFunction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult<string>();

        // Act
        var result = await sut.IfSuccessAsync(x => TestObj.ResultFuncAsync(x));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfSuccessAsync (Result<T>, Func<T, Task<TNext>>)

    [Fact]
    public async Task IfSuccessAsync_NoDoubleResultWithIntFunction_ThrowsException()
    {
        // Arrange
        const Result<double>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut.IfSuccessAsync((x => TestObj.IntFuncAsync(x))));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulDoubleResultWithNoIntFunction_ThrowsException()
    {
        // Arrange
        const double Value = 12.34;
        Result<double> sut = Value;
        const Func<double, Task<int>>? Function = null;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorDoubleResultWithNoIntFunction_ThrowsException()
    {
        // Arrange
        var sut = GetErrorResult<double>();
        const Func<double, Task<int>>? Function = null;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulDoubleResultWithIntFunction_CallsFunction()
    {
        // Arrange
        const double Value = 12.34;
        Result<double> sut = Value;

        // Act
        _ = await sut.IfSuccessAsync(x => TestObj.IntFuncAsync(x));

        // Assert
        _testMock.Verify(x => x.IntFuncAsync(Value), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulDoubleResultWithIntFunction_ReturnsResultWithIntValue()
    {
        // Arrange
        const int ExpectedValue = 4937;
        const double Value = 12.34;

        Result<double> sut = Value;
        _testMock
            .Setup(x => x.IntFuncAsync(Value))
            .ReturnsAsync(ExpectedValue);

        // Act
        var result = await sut.IfSuccessAsync(x => TestObj.IntFuncAsync(x));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorDoubleResultWithIntFunction_DoesNotCallFunction()
    {
        // Arrange
        var sut = GetErrorResult<double>();

        // Act
        _ = await sut.IfSuccessAsync(x => TestObj.IntFuncAsync(x));

        // Assert
        _testMock.Verify(x => x.IntFuncAsync(It.IsAny<double>()), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorDoubleResultWithIntFunction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult<double>();

        // Act
        var result = await sut.IfSuccessAsync(x => TestObj.IntFuncAsync(x));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<int>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(sut.Error);
    }

    #endregion

    #region IfSuccessAsync (Result<T>, Func<T, Task<Result<TNext>>>)

    [Fact]
    public async Task IfSuccessAsync_NoIntResultWithStringResultFunction_ThrowsException()
    {
        // Arrange
        const Result<int>? Sut = null;

        // Act
        var exception = await Record.ExceptionAsync(() => Sut.IfSuccessAsync(x => TestObj.StringResultFuncAsync(x)));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulIntResultWithNoStringResultFunction_ThrowsException()
    {
        // Arrange
        const int Value = 39;
        Result<int> sut = Value;
        const Func<int, Task<Result<string>>>? Function = null;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorIntResultWithNoStringResultFunction_ThrowsException()
    {
        // Arrange
        var sut = GetErrorResult<int>();
        const Func<int, Task<Result<string>>>? Function = null;

        // Act
        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(Function));

        // Assert
        exception.ShouldNotBeNull();
        exception.ShouldBeOfType<ArgumentNullException>();
        exception.Message.ShouldStartWith("Value cannot be null.");
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulIntResultWithStringResultFunction_CallsFunction()
    {
        // Arrange
        const int Value = 39;
        Result<int> sut = Value;

        // Act
        _ = await sut.IfSuccessAsync(x => TestObj.StringResultFuncAsync(x));

        // Assert
        _testMock.Verify(x => x.StringResultFuncAsync(Value), Times.Once);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulIntResultWithSuccessfulStringResultFunction_ReturnsStringResult()
    {
        // Arrange
        const string ExpectedValue = "New Value";
        const int Value = 49;

        _testMock
            .Setup(x => x.StringResultFuncAsync(Value))
            .ReturnsAsync(Result<string>.FromValue(ExpectedValue));

        Result<int> sut = Value;

        // Act
        var result = await sut.IfSuccessAsync(x => TestObj.StringResultFuncAsync(x));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(ExpectedValue);
    }

    [Fact]
    public async Task IfSuccessAsync_SuccessfulIntResultWithErrorStringResultFunction_ReturnsErrorResultWithFunctionError()
    {
        // Arrange
        const int Value = 49;
        var errorResult = GetErrorResult<string>();

        _testMock
            .Setup(x => x.StringResultFuncAsync(Value))
            .ReturnsAsync(errorResult);

        Result<int> sut = Value;

        // Act
        var result = await sut.IfSuccessAsync(x => TestObj.StringResultFuncAsync(x));

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeOfType<Result<string>>();
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(errorResult.Error);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorIntResultWithStringResultFunction_DoesNotCallFunction()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        _ = await sut.IfSuccessAsync(x => TestObj.StringResultFuncAsync(x));

        // Assert
        _testMock.Verify(x => x.StringResultFuncAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task IfSuccessAsync_ErrorIntResultWithStringResultFunction_ReturnsOriginalErrorResult()
    {
        // Arrange
        var sut = GetErrorResult<int>();

        // Act
        var result = await sut.IfSuccessAsync(x => TestObj.StringResultFuncAsync(x));

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
