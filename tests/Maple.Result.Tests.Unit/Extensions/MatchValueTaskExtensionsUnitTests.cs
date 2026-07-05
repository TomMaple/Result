using System;
using System.Threading.Tasks;
using Maple.Result.Extensions;
using Moq;

namespace Maple.Result.Tests.Unit.Extensions;

public class MatchValueTaskExtensionsUnitTests
{
    [Fact]
    public async Task MatchAsync_ResultWithValueTaskActions_UsesSuccessBranch()
    {
        var successMock = new Mock<Func<ValueTask>>();
        var errorMock = new Mock<Func<Error, ValueTask>>();

        var result = await Result.Success().MatchAsync(successMock.Object, errorMock.Object);

        successMock.Verify(x => x.Invoke(), Times.Once);
        errorMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
        result.IsSuccess().ShouldBeTrue();
    }

    [Fact]
    public async Task MatchAsync_ResultWithValueTaskResultFunctions_UsesErrorBranch()
    {
        var expected = Result.Success();
        var sut = GetErrorResult();

        var result = await sut.MatchAsync(() => ValueTask.FromResult(Result.Success()), _ => ValueTask.FromResult(expected));

        result.ShouldBeSameAs(expected);
    }

    [Fact]
    public async Task MatchAsync_GenericResultWithValueTaskFunctions_ReturnsSuccessValue()
    {
        Result<int> sut = 5;

        var result = await sut.MatchAsync(x => ValueTask.FromResult((x + 1).ToString()), _ => ValueTask.FromResult("error"));

        result.ShouldBe("6");
    }

    [Fact]
    public async Task MatchAsync_GenericResultWithValueTaskResultFunctions_ReturnsErrorFunctionResult()
    {
        var expected = Result.FromValue("error");
        var sut = GetErrorResult<int>();

        var result = await sut.MatchAsync(x => ValueTask.FromResult(Result.FromValue(x.ToString())), _ => ValueTask.FromResult(expected));

        result.ShouldBeSameAs(expected);
    }

    [Fact]
    public async Task MatchAsync_ValueTaskResultWithTaskCallbacks_UsesSuccessBranch()
    {
        var successMock = new Mock<Func<Task>>();
        var errorMock = new Mock<Func<Error, Task>>();
        var sut = new ValueTask<Result>(Result.Success());

        var result = await sut.MatchAsync(successMock.Object, errorMock.Object);

        successMock.Verify(x => x.Invoke(), Times.Once);
        errorMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
        result.IsSuccess().ShouldBeTrue();
    }

    [Fact]
    public async Task MatchAsync_ValueTaskResultWithNullResult_ThrowsException()
    {
        ValueTask<Result> sut = ValueTask.FromResult<Result>(null!);

        var exception = await Record.ExceptionAsync(() => sut.MatchAsync(() => Task.CompletedTask, _ => Task.CompletedTask));

        exception.ShouldBeOfType<InvalidOperationException>();
    }

    [Fact]
    public async Task MatchAsync_ValueTaskGenericResultWithTaskCallbacks_UsesErrorBranch()
    {
        var errorResult = GetErrorResult<int>();
        var errorMock = new Mock<Func<Error, Task<string>>>();
        errorMock.Setup(x => x.Invoke(errorResult.Error!)).ReturnsAsync("error");
        ValueTask<Result<int>> sut = ValueTask.FromResult(errorResult);

        var result = await sut.MatchAsync(x => Task.FromResult(x.ToString()), errorMock.Object);

        errorMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
        result.ShouldBe("error");
    }

    private static Result GetErrorResult() => Error.Failure(ErrorUri.None(), "match-valuetask", "Error");

    private static Result<T> GetErrorResult<T>() => Error.Failure(ErrorUri.None(), "match-valuetask-generic", "Error");
}
