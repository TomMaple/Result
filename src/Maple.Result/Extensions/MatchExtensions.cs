using System;

namespace Maple.Result.Extensions;

public static class MatchExtensions
{
    #region Result

    public static Result Match(this Result result, Action ifSuccessAction, Action<Error> ifErrorAction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);

        if (result.IsSuccess())
            ifSuccessAction();
        else
            ifErrorAction(result.Error!);

        return result;
    }

    public static Result Match<TNext>(this Result result, Func<Result> ifSuccessFunction, Func<Error, Result> ifErrorFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return result.IsSuccess()
            ? ifSuccessFunction()
            : ifErrorFunction(result.Error!);
    }

    public static Result<T> Match<T>(this Result result, Func<T> ifSuccessFunction, Func<Error, T> ifErrorFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return result.IsSuccess()
            ? ifSuccessFunction()
            : ifErrorFunction(result.Error!);
    }

    public static Result<T> Match<T>(this Result result, Func<Result<T>> ifSuccessFunction, Func<Error, Result<T>> ifErrorFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return result.IsSuccess()
            ? ifSuccessFunction()
            : ifErrorFunction(result.Error!);
    }

    #endregion

    #region Result<T>

    public static Result<T> Match<T>(this Result<T> result, Action<T> ifSuccessAction, Action<Error> ifErrorAction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessAction);
        ArgumentNullException.ThrowIfNull(ifErrorAction);
     
        if (result.IsSuccess())
            ifSuccessAction(result.Value!);
        else
            ifErrorAction(result.Error!);

        return result;
    }

    public static Result<T> Match<T>(this Result<T> result, Func<T, T> ifSuccessFunction, Func<Error, T> ifErrorFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return result.IsSuccess()
            ? ifSuccessFunction(result.Value!)
            : ifErrorFunction(result.Error!);
    }

    public static Result<TNext> Match<T, TNext>(this Result<T> result, Func<T, TNext> ifSuccessFunction, Func<Error, TNext> ifErrorFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return result.IsSuccess()
            ? ifSuccessFunction(result.Value!)
            : ifErrorFunction(result.Error!);
    }

    public static Result<TNext> Match<T, TNext>(this Result<T> result, Func<T, Result<TNext>> ifSuccessFunction, Func<Error, Result<TNext>> ifErrorFunction)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(ifSuccessFunction);
        ArgumentNullException.ThrowIfNull(ifErrorFunction);

        return result.IsSuccess()
            ? ifSuccessFunction(result.Value!)
            : ifErrorFunction(result.Error!);
    }

    #endregion
}