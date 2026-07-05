using System;
using System.Threading.Tasks;
using Maple.Result.Extensions;
using Moq;

namespace Maple.Result.Tests.Unit.Extensions;

public class LinqValueTaskExtensionsUnitTests
{
    [Fact]
    public async Task SelectMany_ValueTaskResultWithSelectors_CallsBothFunctionsAndReturnsValue()
    {
        var collectionSelectorMock = new Mock<Func<int, Task<Result<string>>>>();
        collectionSelectorMock.Setup(x => x.Invoke(4)).ReturnsAsync(Result.FromValue("4"));

        var resultSelectorMock = new Mock<Func<int, string, double>>();
        resultSelectorMock.Setup(x => x.Invoke(4, "4")).Returns(4d);

        ValueTask<Result<int>> sut = ValueTask.FromResult(Result.FromValue(4));

        var result = await sut.SelectMany(collectionSelectorMock.Object, resultSelectorMock.Object);

        collectionSelectorMock.Verify(x => x.Invoke(4), Times.Once);
        resultSelectorMock.Verify(x => x.Invoke(4, "4"), Times.Once);
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBe(4d);
    }

    [Fact]
    public async Task SelectMany_ValueTaskErrorResult_DoesNotCallSelectors()
    {
        var collectionSelectorMock = new Mock<Func<int, Task<Result<string>>>>();
        var resultSelectorMock = new Mock<Func<int, string, double>>();
        var error = Error.Failure(ErrorUri.None(), "linq-valuetask", "Error");
        ValueTask<Result<int>> sut = ValueTask.FromResult(Result<int>.FromError(error));

        var result = await sut.SelectMany(collectionSelectorMock.Object, resultSelectorMock.Object);

        collectionSelectorMock.Verify(x => x.Invoke(It.IsAny<int>()), Times.Never);
        resultSelectorMock.Verify(x => x.Invoke(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
        result.IsSuccess().ShouldBeFalse();
        result.Error.ShouldBe(error);
    }

    [Fact]
    public async Task SelectMany_ValueTaskResultWithNoCollectionSelector_ThrowsException()
    {
        const Func<int, Task<Result<string>>>? collectionSelector = null;
        ValueTask<Result<int>> sut = ValueTask.FromResult(Result.FromValue(4));

        var exception = await Record.ExceptionAsync(() => sut.SelectMany(collectionSelector!, (x, _) => x));

        exception.ShouldBeOfType<ArgumentNullException>();
    }
}
