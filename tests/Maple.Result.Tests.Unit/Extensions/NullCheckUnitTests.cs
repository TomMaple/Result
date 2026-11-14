// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Maple.Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Maple.Result.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Maple.Result.Tests.Unit.Extensions;

/// <summary>
///     The null checks of all extension methods (issue #121), checked over every public extension method by reflection:
///     a <see langword="null" /> argument of an asynchronous method throws from the call, and a <see langword="null" /> returned by a passed
///     function where a result, an <see cref="Error" /> or a task is required throws an <see cref="InvalidOperationException" /> that names
///     the function.
/// </summary>
public class NullCheckUnitTests
{
    private static readonly Error SourceError = Error.Failure(ErrorUri.None(), "0f6b1c1e-3f7d-4f43-9d4e-58b8b1a3c2d1", "Source error");

    private static readonly Dictionary<string, MethodInfo> Methods = typeof(Result).Assembly.GetExportedTypes()
        .Where(t => t.Namespace?.StartsWith("Maple.Result.Extensions", StringComparison.Ordinal) == true)
        .SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Static))
        .Where(m => m.GetCustomAttribute<ObsoleteAttribute>() is null)
        .Select(Close)
        .ToDictionary(Describe);

    #region asynchronous methods check their arguments synchronously

    public static TheoryData<string, string> NullArgumentCases()
    {
        var data = new TheoryData<string, string>();

        foreach (var (key, method) in Methods.Where(x => IsAwaitable(x.Value.ReturnType)))
        {
            foreach (var parameter in method.GetParameters().Where(p => !p.ParameterType.IsValueType))
                data.Add(key, parameter.Name!);
        }

        return data;
    }

    [Theory]
    [MemberData(nameof(NullArgumentCases))]
    public void AsyncMethod_NullArgument_ThrowsArgumentNullExceptionFromTheCall(string method, string parameter)
    {
        // Arrange
        var methodInfo = Methods[method];
        var arguments = CreateArguments(methodInfo, nullParameter: parameter, nullReturningParameter: null, variant: null);

        // Act
        var exception = Record.Exception(() => methodInfo.Invoke(null, arguments));

        // Assert
        exception.ShouldBeOfType<TargetInvocationException>().InnerException.ShouldBeOfType<ArgumentNullException>().ParamName.ShouldBe(parameter);
    }

    #endregion

    #region a function returning null where a result, an Error or a task is required

    public static TheoryData<string, string, string> NullReturnCases()
    {
        var data = new TheoryData<string, string, string>();

        foreach (var (key, method) in Methods)
        {
            foreach (var parameter in method.GetParameters())
            {
                foreach (var variant in NullVariants(parameter.ParameterType))
                    data.Add(key, parameter.Name!, variant);
            }
        }

        return data;
    }

    [Theory]
    [MemberData(nameof(NullReturnCases))]
    public async Task Method_FunctionReturningNull_ThrowsInvalidOperationExceptionNamingTheFunction(string method, string parameter, string variant)
    {
        // Arrange
        var methodInfo = Methods[method];
        var arguments = CreateArguments(methodInfo, nullParameter: null, nullReturningParameter: parameter, variant);
        var expectedMessage = variant == NullTask ? $"The function ‘{parameter}’ returned a null task." : $"The function ‘{parameter}’ returned null.";

        // Act
        var exception = await Record.ExceptionAsync(() => InvokeAndAwait(methodInfo, arguments));

        // Assert
        exception.ShouldBeOfType<InvalidOperationException>().Message.ShouldBe(expectedMessage);
    }

    [Fact]
    public async Task IfSuccessAsync_FunctionReturningNullValue_ReturnsSuccessfulResultWithNullValue()
    {
        // Arrange
        var sut = Result<int>.FromValue(1);

        // Act
        var result = await sut.IfSuccessAsync(_ => Task.FromResult<string?>(null));

        // Assert
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBeNull();
    }

    [Fact]
    public void Match_FunctionReturningNullValue_ReturnsSuccessfulResultWithNullValue()
    {
        // Arrange
        var sut = Result.Success();

        // Act
        var result = sut.Match(() => (string?)null, _ => { });

        // Assert
        result.IsSuccess().ShouldBeTrue();
        result.Value.ShouldBeNull();
    }

    #endregion

    #region helper methods

    private const string NullValue = "null";
    private const string NullTask = "null task";
    private const string NullTaskResult = "task with a null result";

    /// <summary>The ways a function of the type can return <see langword="null" /> where the extension methods require a value.</summary>
    private static IEnumerable<string> NullVariants(Type type)
    {
        if (!IsDelegate(type) || DelegateReturnType(type) is not { } returnType || returnType == typeof(void))
            yield break;

        if (IsRequired(returnType))
            yield return NullValue;
        else if (returnType == typeof(Task))
            yield return NullTask;
        else if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            yield return NullTask;

            if (IsRequired(returnType.GetGenericArguments()[0]))
                yield return NullTaskResult;
        }
        else if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(ValueTask<>) && IsRequired(returnType.GetGenericArguments()[0]))
            yield return NullTaskResult;
    }

    /// <summary>The types that a function may not return as <see langword="null" />.</summary>
    private static bool IsRequired(Type type)
    {
        return type == typeof(Result) || type == typeof(Error) || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Result<>));
    }

    private static MethodInfo Close(MethodInfo method)
    {
        if (!method.IsGenericMethodDefinition)
            return method;

        var arguments = method.GetGenericArguments().Select(a => a.Name == "TResult" ? typeof(Result) : typeof(int)).ToArray();
        return method.MakeGenericMethod(arguments);
    }

    private static string Describe(MethodInfo method)
    {
        var parameters = string.Join(", ", method.GetParameters().Select(p => TypeName(p.ParameterType)));
        return $"{method.DeclaringType!.Name}.{method.Name}({parameters})";
    }

    private static string TypeName(Type type)
    {
        if (Nullable.GetUnderlyingType(type) is { } underlying)
            return TypeName(underlying) + "?";

        if (!type.IsGenericType)
            return type.Name;

        var name = type.Name[..type.Name.IndexOf('`')];
        return $"{name}<{string.Join(", ", type.GetGenericArguments().Select(TypeName))}>";
    }

    private static bool IsAwaitable(Type type)
    {
        return type == typeof(Task) || type == typeof(ValueTask)
               || (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Task<>) || type.GetGenericTypeDefinition() == typeof(ValueTask<>)));
    }

    private static bool IsDelegate(Type type)
    {
        return typeof(Delegate).IsAssignableFrom(type);
    }

    private static Type? DelegateReturnType(Type type)
    {
        return type.GetMethod("Invoke")?.ReturnType;
    }

    /// <summary>
    ///     Valid arguments for the method: a successful source, or a failed one when the tested function handles the error, and functions that
    ///     return valid values, except for the parameter that is <see langword="null" /> or the function that returns <see langword="null" />.
    /// </summary>
    private static object?[] CreateArguments(MethodInfo method, string? nullParameter, string? nullReturningParameter, string? variant)
    {
        var successfulSource = nullReturningParameter?.StartsWith("ifError", StringComparison.Ordinal) != true;
        var parameters = method.GetParameters();
        var arguments = new object?[parameters.Length];

        for (var i = 0; i < parameters.Length; i++)
        {
            var parameter = parameters[i];

            if (parameter.Name == nullParameter)
                arguments[i] = null;
            else if (parameter.Name == nullReturningParameter)
                arguments[i] = CreateDelegate(parameter.ParameterType, NullReturnValue(DelegateReturnType(parameter.ParameterType)!, variant!));
            else if (i == 0)
                arguments[i] = CreateValue(parameter.ParameterType, successfulSource);
            else
                arguments[i] = CreateValue(parameter.ParameterType, successful: true);
        }

        return arguments;
    }

    private static object? NullReturnValue(Type returnType, string variant)
    {
        return variant switch
        {
            NullValue or NullTask => null,
            NullTaskResult when returnType.GetGenericTypeDefinition() == typeof(Task<>) => FromResult(returnType.GetGenericArguments()[0], null),
            NullTaskResult => CreateValueTask(returnType, null),
            _ => throw new ArgumentOutOfRangeException(nameof(variant), variant, null)
        };
    }

    private static object? CreateValue(Type type, bool successful)
    {
        if (type == typeof(bool?))
            return null;

        if (type == typeof(int))
            return 1;

        if (type == typeof(Error))
            return SourceError;

        if (type == typeof(Result))
            return successful ? Result.Success() : Result.FromError(SourceError);

        if (type == typeof(Task))
            return Task.CompletedTask;

        if (type == typeof(ValueTask))
            return ValueTask.CompletedTask;

        if (IsDelegate(type))
        {
            var returnType = DelegateReturnType(type)!;
            return CreateDelegate(type, returnType == typeof(void) ? null : CreateValue(returnType, successful: true));
        }

        if (type.IsGenericType)
        {
            var definition = type.GetGenericTypeDefinition();
            var argument = type.GetGenericArguments()[0];

            if (definition == typeof(Result<>))
            {
                return successful
                    ? type.GetMethod(nameof(Result<>.FromValue))!.Invoke(null, [CreateValue(argument, successful: true)])
                    : type.GetMethod(nameof(Result<>.FromError))!.Invoke(null, [SourceError]);
            }

            if (definition == typeof(Task<>))
                return FromResult(argument, CreateValue(argument, successful));

            if (definition == typeof(ValueTask<>))
                return CreateValueTask(type, CreateValue(argument, successful));
        }

        throw new NotSupportedException($"No test value for {type}.");
    }

    private static object CreateValueTask(Type valueTaskType, object? value)
    {
        return valueTaskType.GetConstructor([valueTaskType.GetGenericArguments()[0]])!.Invoke([value]);
    }

    private static object FromResult(Type type, object? value)
    {
        return typeof(Task).GetMethod(nameof(Task.FromResult))!.MakeGenericMethod(type).Invoke(null, [value])!;
    }

    private static Delegate CreateDelegate(Type delegateType, object? returnValue)
    {
        var invoke = delegateType.GetMethod("Invoke")!;
        var parameters = invoke.GetParameters().Select(p => Expression.Parameter(p.ParameterType, p.Name)).ToArray();
        Expression body = invoke.ReturnType == typeof(void) ? Expression.Empty() : Expression.Constant(returnValue, invoke.ReturnType);
        return Expression.Lambda(delegateType, body, parameters).Compile();
    }

    private static async Task InvokeAndAwait(MethodInfo method, object?[] arguments)
    {
        object? returned;

        try
        {
            returned = method.Invoke(null, arguments);
        }
        catch (TargetInvocationException exception) when (exception.InnerException is not null)
        {
            throw exception.InnerException;
        }

        switch (returned)
        {
            case Task task:
                await task;
                break;
            case ValueTask valueTask:
                await valueTask;
                break;
            case not null when returned.GetType().IsGenericType && returned.GetType().GetGenericTypeDefinition() == typeof(ValueTask<>):
                await (Task)returned.GetType().GetMethod(nameof(ValueTask<>.AsTask))!.Invoke(returned, null)!;
                break;
        }
    }

    #endregion
}
