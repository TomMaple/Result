using System;
using System.Threading.Tasks;
using Maple.Result.Extensions;
using Moq;

namespace Maple.Result.Tests.Unit.Extensions;

public class IfErrorValueTaskExtensionsUnitTests
{
    [Fact]
    public async Task IfErrorAsync_ErrorResultWithValueTaskAction_CallsActionAndReturnsOriginalResult()
    {
        var actionMock = new Mock<Func<Error, ValueTask>>();
        var sut = GetErrorResult();

        var result = await sut.IfErrorAsync(actionMock.Object);

        actionMock.Verify(x => x.Invoke(sut.Error!), Times.Once);
        result.ShouldBeSameAs(sut);
    }

    [Fact]
    public async Task IfErrorAsync_ErrorResultWithValueTaskFunction_ReturnsFunctionResult()
    {
        var expected = Result.Success();
        var sut = GetErrorResult();

        var result = await sut.IfErrorAsync(_ => ValueTask.FromResult(expected));

        result.ShouldBeSameAs(expected);
    }

    [Fact]
    public async Task IfErrorAsync_ValueTaskResultWithTaskAction_DoesNotCallActionOnSuccess()
    {
        var actionMock = new Mock<Func<Error, Task>>();
        var sut = new ValueTask<Result>(Result.Success());

        var result = await sut.IfErrorAsync(actionMock.Object);

        actionMock.Verify(x => x.Invoke(It.IsAny<Error>()), Times.Never);
        result.IsSuccess().ShouldBeTrue();
    }

    [Fact]
    public async Task IfErrorAsync_ValueTaskResultWithValueTaskAction_CallsActionOnError()
    {
        var actionMock = new Mock<Func<Error, ValueTask>>();
        var errorResult = GetErrorResult();
        var sut = new ValueTask<Result>(errorResult);

        var result = await sut.IfErrorAsync(actionMock.Object);

        actionMock.Verify(x => x.Invoke(errorResult.Error!), Times.Once);
        result.ShouldBeSameAs(errorResult);
    }

    [Fact]
    public async Task IfErrorAsync_ValueTaskResultWithNullResult_ThrowsException()
    {
        ValueTask<Result> sut = ValueTask.FromResult<Result>(null!);

        var exception = await Record.ExceptionAsync(() => sut.IfErrorAsync(_ => Task.CompletedTask));

        exception.ShouldBeOfType<InvalidOperationException>();
        exception.Message.ShouldStartWith("The asynchronous operation returned null.");
    }

    [Fact]
    public async Task IfErrorAsync_ValueTaskGenericResultWithTaskFunction_ReturnsFunctionResultOnError()
    {
        var expected = Result.FromValue(42);
        ValueTask<Result<int>> sut = ValueTask.FromResult(GetErrorResult<int>());

        var result = await sut.IfErrorAsync(_ => Task.FromResult(expected));

        result.ShouldBeSameAs(expected);
    }

    [Fact]
    public async Task IfErrorAsync_ValueTaskGenericResultWithValueTaskFunction_ReturnsOriginalResultOnSuccess()
    {
        var initial = Result.FromValue(5);
        ValueTask<Result<int>> sut = ValueTask.FromResult(initial);

        var result = await sut.IfErrorAsync(_ => ValueTask.FromResult(Result.FromValue(10)));

        result.ShouldBeSameAs(initial);
    }

    private static Result GetErrorResult() => Error.Failure(ErrorUri.None(), "iferror-valuetask", "Error");

    private static Result<T> GetErrorResult<T>() => Error.Failure(ErrorUri.None(), "iferror-valuetask-generic", "Error");
}
