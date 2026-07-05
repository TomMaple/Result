using System;
using System.Threading.Tasks;
using Maple.Result.Extensions;
using Moq;

namespace Maple.Result.Tests.Unit.Extensions;

public class IfSuccessValueTaskExtensionsUnitTests
{
    [Fact]
    public async Task IfSuccessAsync_ResultWithValueTaskAction_CallsAction()
    {
        var actionMock = new Mock<Func<ValueTask>>();
        var sut = Result.Success();

        var result = await sut.IfSuccessAsync(actionMock.Object);

        actionMock.Verify(x => x.Invoke(), Times.Once);
        result.ShouldBeSameAs(sut);
    }

    [Fact]
    public async Task IfSuccessAsync_ResultWithValueTaskResultFunction_ReturnsFunctionResult()
    {
        var expected = Result.Success();

        var result = await Result.Success().IfSuccessAsync(() => ValueTask.FromResult(expected));

        result.ShouldBeSameAs(expected);
    }

    [Fact]
    public async Task IfSuccessAsync_ResultWithValueTaskGenericFunction_ReturnsValueResult()
    {
        var result = await Result.Success().IfSuccessAsync(() => ValueTask.FromResult(7));

        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(7);
    }

    [Fact]
    public async Task IfSuccessAsync_GenericResultWithValueTaskAction_CallsActionWithValue()
    {
        var actionMock = new Mock<Func<int, ValueTask>>();
        Result<int> sut = 3;

        var result = await sut.IfSuccessAsync(actionMock.Object);

        actionMock.Verify(x => x.Invoke(3), Times.Once);
        result.ShouldBeSameAs(sut);
    }

    [Fact]
    public async Task IfSuccessAsync_GenericResultWithValueTaskGenericResultFunction_ReturnsFunctionResult()
    {
        Result<int> sut = 3;

        var result = await sut.IfSuccessAsync(x => ValueTask.FromResult(Result.FromValue($"{x}!")));

        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe("3!");
    }

    [Fact]
    public async Task IfSuccessAsync_ValueTaskResultWithTaskAction_CallsAction()
    {
        var actionMock = new Mock<Func<Task>>();
        var sut = new ValueTask<Result>(Result.Success());

        var result = await sut.IfSuccessAsync(actionMock.Object);

        actionMock.Verify(x => x.Invoke(), Times.Once);
        result.IsSuccess().ShouldBeTrue();
    }

    [Fact]
    public async Task IfSuccessAsync_ValueTaskResultWithValueTaskResultFunction_ReturnsFunctionResult()
    {
        var expected = Result.Success();
        var sut = new ValueTask<Result>(Result.Success());

        var result = await sut.IfSuccessAsync(() => ValueTask.FromResult(expected));

        result.ShouldBeSameAs(expected);
    }

    [Fact]
    public async Task IfSuccessAsync_ValueTaskResultWithNullResult_ThrowsException()
    {
        ValueTask<Result> sut = ValueTask.FromResult<Result>(null!);

        var exception = await Record.ExceptionAsync(() => sut.IfSuccessAsync(() => Task.CompletedTask));

        exception.ShouldBeOfType<InvalidOperationException>();
    }

    [Fact]
    public async Task IfSuccessAsync_ValueTaskGenericResultWithTaskAction_CallsAction()
    {
        var actionMock = new Mock<Func<int, Task>>();
        ValueTask<Result<int>> sut = ValueTask.FromResult(Result.FromValue(9));

        var result = await sut.IfSuccessAsync(actionMock.Object);

        actionMock.Verify(x => x.Invoke(9), Times.Once);
        result.Value.ShouldBe(9);
    }

    [Fact]
    public async Task IfSuccessAsync_ValueTaskGenericResultWithValueTaskGenericResultFunction_ReturnsFunctionResult()
    {
        ValueTask<Result<int>> sut = ValueTask.FromResult(Result.FromValue(9));

        var result = await sut.IfSuccessAsync(x => ValueTask.FromResult(Result.FromValue((x + 1).ToString())));

        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe("10");
    }
}
