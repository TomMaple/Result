using System;
using System.Threading.Tasks;

namespace Maple.Result.Extensions;

/// <summary>
///     The collection of LINQ extension methods for a <see cref="Result" /> instance that support asynchronous operations.
/// </summary>
/// <remarks>
///     These methods enable fluent chaining of asynchronous operations and using LINQ query syntax
///     with asynchronous workflows.
///     They help simplify error handling and reduce boilerplate code when working with
///     <see cref="Result{T}" />-based asynchronous workflows.
/// </remarks>
public static class LinqAsyncExtensions
{
    /// <summary>
    ///     Returns a new instance of the <see cref="Task{TResult}" /> that represents the result of applying
    ///     the provided collection selector and a result selector functions if the current <see cref="Result{T}" />
    ///     is successful, or the new <see cref="Task{TResult}" /> that represents a new <see cref="Result{TNext}" /> instance
    ///     with the same error otherwise.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of the <see cref="Result{T}" /> value used to determine whether to execute
    ///     the passed <paramref name="collectionSelector" /> and <paramref name="resultSelector" /> functions.
    ///     If successful, this is also the type of the parameter passed to that function.
    /// </typeparam>
    /// <typeparam name="TMiddle">
    ///     The type of the <see cref="Result{TMiddle}" /> output type of the passed <paramref name="collectionSelector" />
    ///     function and the type of one of the parameters passed to the <paramref name="resultSelector" /> function.
    /// </typeparam>
    /// <typeparam name="TNext">
    ///     The output type of the <see cref="Result{TNext}" /> value to return.
    /// </typeparam>
    /// <param name="result">
    ///     The <see cref="Task{TResult}" /> that represents the initial result to transform if it is
    ///     successful.
    /// </param>
    /// <param name="collectionSelector">
    ///     A function that takes the value of the initial result and
    ///     returns a new result representing an intermediate value.
    /// </param>
    /// <param name="resultSelector">
    ///     A function that combines the value from the initial result and the intermediate value to produce the final value.
    /// </param>
    /// <exception cref="ArgumentNullException">
    ///     If any of the <paramref name="result" /> or <paramref name="collectionSelector" /> or
    ///     <paramref name="resultSelector" /> parameters are <see langword="null" />.
    /// </exception>
    /// <returns>
    ///     A <see cref="Task{TResult}" /> that represents the result of applying the provided collection selector and
    ///     a result selector functions if the current <paramref name="result" /> is successful;
    ///     otherwise, a <see cref="Task{TResult}" /> that represents a failed result.
    /// </returns>
    public static async Task<Result<TNext>> SelectMany<T, TMiddle, TNext>(
        this Task<Result<T>> result,
        Func<T, Task<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, TNext> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(result);
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        var resultValue = await result;

        return await resultValue.IfSuccessAsync(CollectionSelectorAsync);

        async Task<Result<TNext>> CollectionSelectorAsync(T x)
        {
            var collectionResult = await collectionSelector(x);
            return collectionResult.IfSuccess(y => resultSelector(x, y));
        }
    }

    /// <summary>
    ///     Returns a new instance of the <see cref="Task{TResult}" /> that represents the result of applying
    ///     the provided collection selector and a result selector functions if the current <see cref="Result{T}" />
    ///     is successful, or the new <see cref="Task{TResult}" /> that represents a new <see cref="Result{TNext}" /> instance
    ///     with the same error otherwise.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TMiddle">The type of the intermediate result value.</typeparam>
    /// <typeparam name="TNext">The output type of the <see cref="Result{TNext}" /> value to return.</typeparam>
    /// <param name="result">The <see cref="ValueTask{TResult}" /> that represents the initial result to transform if it is successful.</param>
    /// <param name="collectionSelector">A function that takes the value of the initial result and returns a new result representing an intermediate value.</param>
    /// <param name="resultSelector">A function that combines the value from the initial result and the intermediate value to produce the final value.</param>
    /// <exception cref="ArgumentNullException">If any of the <paramref name="collectionSelector" /> or <paramref name="resultSelector" /> parameters are <see langword="null" />.</exception>
    /// <returns>A <see cref="Task{TResult}" /> that represents the transformed result.</returns>
    public static async Task<Result<TNext>> SelectMany<T, TMiddle, TNext>(
        this ValueTask<Result<T>> result,
        Func<T, Task<Result<TMiddle>>> collectionSelector,
        Func<T, TMiddle, TNext> resultSelector)
    {
        ArgumentNullException.ThrowIfNull(collectionSelector);
        ArgumentNullException.ThrowIfNull(resultSelector);

        var resultValue = await result;

        return await resultValue.IfSuccessAsync(CollectionSelectorAsync);

        async Task<Result<TNext>> CollectionSelectorAsync(T x)
        {
            var collectionResult = await collectionSelector(x);
            return collectionResult.IfSuccess(y => resultSelector(x, y));
        }
    }

}
