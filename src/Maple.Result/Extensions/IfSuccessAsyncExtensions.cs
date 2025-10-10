using System;
using System.Threading.Tasks;

namespace Maple.Result.Extensions;

/// <summary>
///     The collection of extension methods for executing asynchronous actions or functions conditionally based on
///     the success state of a <see cref="Result" /> instance.
/// </summary>
/// <remarks>
///     These methods enable fluent chaining of operations that should only be performed if a preceding
///     <see cref="Result" /> indicates success. They help simplify error handling and reduce boilerplate code
///     when working with <see cref="Result" />-based workflows.
/// </remarks>
public static class IfSuccessAsyncExtensions
{
    #region Result extensions

    /// <summary>
    ///     Invokes the specified asynchronous action as an asynchronous operation,
    ///     if the <paramref name="result" /> represents a successful outcome.
    /// </summary>
    /// <remarks>
    ///     This method enables fluent handling of successful <see cref="Result" />s by executing
    ///     the provided asynchronous action only when the <paramref name="result" /> indicates success.
    ///     The action is not invoked if the <paramref name="result" /> is not successful.
    /// </remarks>
    /// <param name="result">The <see cref="Result" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessAction">
    ///     The asynchronous action to execute if the <paramref name="result" /> is successful.
    ///     Cannot be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{Result}" /> that represents the original <paramref name="result" />,
    ///     allowing for method chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessAction" />
    ///     parameters are <see langword="null" />.
    /// </exception>
    public static async Task<Result> IfSuccessAsync(this Result result, Func<Task> ifSuccessAction)
    {
        ArgumentNullException.ThrowIfNull(result, nameof(result));
        ArgumentNullException.ThrowIfNull(ifSuccessAction, nameof(ifSuccessAction));

        if (result.IsSuccess())
            await ifSuccessAction();

        return result;
    }

    /// <summary>
    ///     Invokes the specified asynchronous function as an asynchronous operation,
    ///     if the <paramref name="result" /> represents a successful outcome;
    ///     otherwise, returns an error <see cref="Result" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     This method enables chaining additional asynchronous operations that should only execute
    ///     if the previous <see cref="Result" /> was successful. If <paramref name="result" /> is not successful,
    ///     the provided function is not invoked.
    /// </remarks>
    /// <param name="result">The <see cref="Result" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunc">
    ///     An asynchronous function to execute if <paramref name="result" /> is successful. The function must return
    ///     a <see cref="Task{Result}" /> and must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{Result}" /> returned by <paramref name="ifSuccessFunc" />
    ///     if <paramref name="result" /> is successful;
    ///     otherwise, a <see cref="Task{Result}" /> that represents the original error <paramref name="result" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunc" />
    ///     parameters are <see langword="null" />.
    /// </exception>
    public static async Task<Result> IfSuccessAsync(this Result result, Func<Task<Result>> ifSuccessFunc)
    {
        ArgumentNullException.ThrowIfNull(result, nameof(result));
        ArgumentNullException.ThrowIfNull(ifSuccessFunc, nameof(ifSuccessFunc));

        if (!result.IsSuccess())
            return result.Error!;

        return await ifSuccessFunc();
    }

    /// <summary>
    ///     Invokes the specified asynchronous function as an asynchronous operation, and returns its output
    ///     as a <see cref="Result{T}" />, if the operation represented by the <paramref name="result" /> is successful;
    ///     otherwise, returns a failed <see cref="Result{T}" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     This method enables chaining additional asynchronous operations that should only execute
    ///     if the previous <see cref="Result" /> was successful.
    ///     If <paramref name="result" /> is not successful, the provided function is not invoked.
    /// </remarks>
    /// <typeparam name="T">The type of the <see cref="Result{T}" /> value to return if the operation is successful.</typeparam>
    /// <param name="result">The <see cref="Result" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunc">
    ///     An asynchronous function to execute if the <paramref name="result" /> is successful. The function’s return value
    ///     is used as the successful <see cref="Result{T}" />. Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the successful <see cref="Result{T}" />
    ///     containing the value returned by <paramref name="ifSuccessFunc" />
    ///     if <paramref name="result" /> is successful;
    ///     otherwise, a <see cref="Task{TResult}" /> that represents the failed <see cref="Result{T}" />
    ///     with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunc" />
    ///     parameters are <see langword="null" />.
    /// </exception>
    public static async Task<Result<T>> IfSuccessAsync<T>(this Result result, Func<Task<T>> ifSuccessFunc)
    {
        ArgumentNullException.ThrowIfNull(result, nameof(result));
        ArgumentNullException.ThrowIfNull(ifSuccessFunc, nameof(ifSuccessFunc));

        if (!result.IsSuccess())
            return result.Error!;

        return await ifSuccessFunc();
    }

    /// <summary>
    ///     Invokes the specified asynchronous function as an asynchronous operation,
    ///     and returns a <see cref="Result{T}" /> with its output if the operation represented
    ///     by the <paramref name="result" /> is successful; otherwise, returns a failed <see cref="Result{T}" />
    ///     with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     This method enables chaining additional asynchronous operations that should only execute
    ///     if the previous <see cref="Result" /> was successful.
    ///     If <paramref name="result" /> is not successful, the provided function is not invoked.
    /// </remarks>
    /// <typeparam name="T">The type of the <see cref="Result{T}" /> value to return if the operation is successful.</typeparam>
    /// <param name="result">The <see cref="Result" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunc">
    ///     An asynchronous function to execute if the <paramref name="result" /> is successful. The function’s return value
    ///     is used as the successful <see cref="Result{T}" />. Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents a successful <see cref="Result{T}" />
    ///     returned by <paramref name="ifSuccessFunc" /> if <paramref name="result" /> is successful;
    ///     otherwise, a <see cref="Task{TResult}" /> that represents the failed <see cref="Result{T}" />
    ///     with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunc" />
    ///     parameters are <see langword="null" />.
    /// </exception>
    public static async Task<Result<T>> IfSuccessAsync<T>(this Result result, Func<Task<Result<T>>> ifSuccessFunc)
    {
        ArgumentNullException.ThrowIfNull(result, nameof(result));
        ArgumentNullException.ThrowIfNull(ifSuccessFunc, nameof(ifSuccessFunc));

        if (!result.IsSuccess())
            return result.Error!;

        return await ifSuccessFunc();
    }

    #endregion

    #region Result<T> extensions

    /// <summary>
    ///     Invokes the specified asynchronous action if the <paramref name="result" /> represents a successful outcome.
    /// </summary>
    /// <remarks>
    ///     This method enables fluent handling of successful <see cref="Result{T}" />s by executing
    ///     the provided asynchronous action only when the <paramref name="result" /> indicates success.
    ///     The action is not invoked if the <paramref name="result" /> is not successful.
    /// </remarks>
    /// <param name="result">The <see cref="Result{T}" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessAction">
    ///     The asynchronous action to execute if the <paramref name="result" /> is successful.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the original <paramref name="result" />,
    ///     allowing for method chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessAction" />
    ///     parameters are <see langword="null" />.
    /// </exception>
    public static async Task<Result<T>> IfSuccessAsync<T>(this Result<T> result, Func<T, Task> ifSuccessAction)
    {
        ArgumentNullException.ThrowIfNull(result, nameof(result));
        ArgumentNullException.ThrowIfNull(ifSuccessAction, nameof(ifSuccessAction));

        if (result.IsSuccess())
            await ifSuccessAction(result.Value!);

        return result;
    }

    /// <summary>
    ///     Invokes the specified asynchronous function if the <paramref name="result" /> represents a successful outcome;
    ///     otherwise, returns an error <see cref="Result" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     This method enables chaining additional asynchronous operations that should only execute
    ///     if the previous <see cref="Result" /> was successful. If <paramref name="result" /> is not successful,
    ///     the provided asynchronous function is not invoked.
    /// </remarks>
    /// <param name="result">The <see cref="Result{T}" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunc">
    ///     The asynchronous function to execute if <paramref name="result" /> is successful. The value of the successful
    ///     <see cref="Result{T}" /> is passed as a parameter. The function must return
    ///     a <see cref="Task{Result}" /> and must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{Result}" /> that is returned by <paramref name="ifSuccessFunc" /> if <paramref name="result" />
    ///     is successful; otherwise, a failed <see cref="Result" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunc" />
    ///     parameters are <see langword="null" />.
    /// </exception>
    public static async Task<Result> IfSuccessAsync<T>(this Result<T> result, Func<T, Task<Result>> ifSuccessFunc)
    {
        ArgumentNullException.ThrowIfNull(result, nameof(result));
        ArgumentNullException.ThrowIfNull(ifSuccessFunc, nameof(ifSuccessFunc));

        if (!result.IsSuccess())
            return result.Error!;

        return await ifSuccessFunc(result.Value!);
    }

    /// <summary>
    ///     Invokes the specified asynchronous function and returns its output as a <see cref="Result{TNext}" />
    ///     if the operation represented by the <paramref name="result" /> is successful;
    ///     otherwise, returns a failed <see cref="Result{TNext}" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     This method enables chaining additional asynchronous operations that should only execute
    ///     if the previous <see cref="Result{T}" /> was successful. If <paramref name="result" /> is not successful,
    ///     the provided asynchronous function is not invoked.
    /// </remarks>
    /// <typeparam name="T">
    ///     The type of the <see cref="Result{T}" /> value used to determine whether to execute the passed
    ///     function. If successful, this is also the type of the parameter passed to that function.
    /// </typeparam>
    /// <typeparam name="TNext">
    ///     The output type of the passed function and the type of the <see cref="Result{TNext}" />
    ///     value to return.
    /// </typeparam>
    /// <param name="result">The <see cref="Result{T}" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunc">
    ///     An asynchronous function to execute if the <paramref name="result" /> is successful.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter.
    ///     The function’s return value is used as the successful <see cref="Result{TNext}" />.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the successful <see cref="Result{TNext}" /> containing
    ///     the value returned by <paramref name="ifSuccessFunc" /> if <paramref name="result" /> is successful;
    ///     otherwise, a failed <see cref="Result{TNext}" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunc" />
    ///     parameters are <see langword="null" />.
    /// </exception>
    public static async Task<Result<TNext>> IfSuccessAsync<T, TNext>(this Result<T> result,
        Func<T, Task<TNext>> ifSuccessFunc)
    {
        ArgumentNullException.ThrowIfNull(result, nameof(result));
        ArgumentNullException.ThrowIfNull(ifSuccessFunc, nameof(ifSuccessFunc));

        if (!result.IsSuccess())
            return result.Error!;

        return await ifSuccessFunc(result.Value!);
    }

    /// <summary>
    ///     Invokes the specified asynchronous function and returns a <see cref="Result{TNext}" /> with its output
    ///     if the operation represented by the <paramref name="result" /> is successful;
    ///     otherwise, returns a failed <see cref="Result{TNext}" /> with the original <see cref="Error" />.
    /// </summary>
    /// <remarks>
    ///     This method enables chaining additional asynchronous operations that should only execute
    ///     if the previous <see cref="Result{T}" /> was successful. If <paramref name="result" /> is not successful,
    ///     the provided function is not invoked.
    /// </remarks>
    /// <typeparam name="T">
    ///     The type of the <see cref="Result{T}" /> value used to determine whether to execute the passed
    ///     function. If successful, this is also the type of the parameter passed to that function.
    /// </typeparam>
    /// <typeparam name="TNext">
    ///     The type of the <see cref="Result{TNext}" /> value to return and of the output
    ///     of the passed function.
    /// </typeparam>
    /// <param name="result">The <see cref="Result{T}" /> to evaluate for success. Must not be <see langword="null" />.</param>
    /// <param name="ifSuccessFunc">
    ///     An asynchronous function to execute if the <paramref name="result" /> is successful.
    ///     The value of the successful <see cref="Result{T}" /> is passed as a parameter.
    ///     The function’s must return <see cref="Task{Result}" /> that represents <see cref="Result{TNext}" />.
    ///     Must not be <see langword="null" />.
    /// </param>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the successful <see cref="Result{TNext}" /> returned by
    ///     <paramref name="ifSuccessFunc" /> if <paramref name="result" /> is successful;
    ///     otherwise, a failed <see cref="Result{TNext}" /> with the original <see cref="Error" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="ifSuccessFunc" />
    ///     parameters are <see langword="null" />.
    /// </exception>
    public static async Task<Result<TNext>> IfSuccessAsync<T, TNext>(this Result<T> result,
        Func<T, Task<Result<TNext>>> ifSuccessFunc)
    {
        ArgumentNullException.ThrowIfNull(result, nameof(result));
        ArgumentNullException.ThrowIfNull(ifSuccessFunc, nameof(ifSuccessFunc));

        if (!result.IsSuccess())
            return result.Error!;

        return await ifSuccessFunc(result.Value!);
    }

    #endregion
}
