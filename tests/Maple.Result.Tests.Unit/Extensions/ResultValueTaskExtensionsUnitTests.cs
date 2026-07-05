using System;
using System.Threading.Tasks;
using Maple.Result.Extensions;

namespace Maple.Result.Tests.Unit.Extensions;

public class ResultValueTaskExtensionsUnitTests
{
    [Fact]
    public async Task ToResultAsync_SuccessfulValueTaskResult_ReturnsSuccess()
    {
        ValueTask<Result<int>> sut = ValueTask.FromResult(Result.FromValue(1));

        var result = await sut.ToResultAsync();

        result.IsSuccess().ShouldBeTrue();
    }

    [Fact]
    public async Task ToResultAsync_ErrorValueTaskResult_ReturnsOriginalError()
    {
        var error = Error.Failure(ErrorUri.None(), "result-valuetask", "Error");
        ValueTask<Result<int>> sut = ValueTask.FromResult(Result<int>.FromError(error));

        var result = await sut.ToResultAsync();

        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(error);
    }

    [Fact]
    public async Task ToResultAsync_NullValueTaskResult_ThrowsException()
    {
        ValueTask<Result<int>> sut = ValueTask.FromResult<Result<int>>(null!);

        var exception = await Record.ExceptionAsync(() => sut.ToResultAsync());

        exception.ShouldBeOfType<InvalidOperationException>();
    }
}
