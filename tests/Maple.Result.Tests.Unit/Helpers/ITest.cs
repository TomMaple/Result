using System.Threading.Tasks;

namespace Maple.Result.Tests.Unit.Helpers;

internal interface ITest
{
    #region synchronous

    void Action();
    void IntAction(int value);

    Result ResultFunc();

    int IntFunc();

    int IntFunc(int value);

    int IntFunc(double value);

    Result<int> IntResultFunc();

    Result<int> IntResultFunc(string value);

    Result<string> StringResultFunc(int value);

    Result ResultFunc(string value);

    #endregion

    #region asynchronous

    Task ActionAsync();
    Task IntActionAsync(int value);

    Task<Result> ResultFuncAsync();

    Task<int> IntFuncAsync();

    Task<int> IntFuncAsync(int value);

    Task<int> IntFuncAsync(double value);

    Task<Result<int>> IntResultFuncAsync();

    Task<Result<int>> IntResultFuncAsync(string value);

    Task<Result<string>> StringResultFuncAsync(int value);

    Task<Result> ResultFuncAsync(string value);

    #endregion
}
