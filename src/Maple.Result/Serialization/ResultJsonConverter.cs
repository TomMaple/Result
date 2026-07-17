// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Maple.Result.Serialization;

/// <summary>
///     Creates a <see cref="ResultJsonConverter{T}" /> for any closed <see cref="Result{T}" /> type.
/// </summary>
internal sealed class ResultJsonConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsGenericType
               && typeToConvert.GetGenericTypeDefinition() == typeof(Result<>);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var valueType = typeToConvert.GetGenericArguments()[0];

        return (JsonConverter)Activator.CreateInstance(
            typeof(ResultJsonConverter<>).MakeGenericType(valueType))!;
    }
}

/// <summary>
///     Writes the <see cref="Result{T}.Value" /> member only when a value is present, so that
///     a present <see langword="null"/> value (a successful result of a nullable <typeparamref name="T" />) is
///     told apart from an absent one on the way back.
/// </summary>
/// <remarks>
///     <para>
///         The default reflection-based contract cannot express presence: a failed <c>Result&lt;int&gt;</c> is written
///         as <c>"Value": 0</c>, indistinguishable from a successful result carrying zero; a failed reference-type
///         result is written as <c>"Value": null</c>, indistinguishable from a successful <see langword="null" /> value.
///     </para>
///     <para>
///         This converter instead omits the member for a failed (or not-yet-populated) result and writes it—<c>null</c>
///         included—only for a successful one. So <c>{"Value":null,"Error":null}</c> denotes a successful null value,
///         while a failed result carries no <c>Value</c> member at all.
///     </para>
/// </remarks>
internal sealed class ResultJsonConverter<T> : JsonConverter<Result<T>>
{
    private const string ErrorPropertyName = "Error";
    private const string ValuePropertyName = "Value";

    public override Result<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
            throw new JsonException($"Expected an object to deserialize a {typeToConvert.Name}.");

        var valueName = ResolveName(ValuePropertyName, options);
        var errorName = ResolveName(ErrorPropertyName, options);
        var comparison = options.PropertyNameCaseInsensitive
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;

        T? value = default;
        var hasValue = false;
        Error? error = null;

        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException($"Expected a property name to deserialize a {typeToConvert.Name}.");

            var propertyName = reader.GetString();
            reader.Read();

            if (string.Equals(propertyName, valueName, comparison))
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    // A null token is a present null value when T can hold null (a nullable reference type or
                    // Nullable<U>). For a non-nullable value type there is no null to hold, so it denotes an
                    // absent value (how a failed result reports it)—leave the member unset.
                    if (Result<T>.CanHoldNull)
                        hasValue = true;

                    continue;
                }

                value = JsonSerializer.Deserialize<T>(ref reader, options);
                hasValue = true;
            }
            else if (string.Equals(propertyName, errorName, comparison))
            {
                if (reader.TokenType == JsonTokenType.Null)
                    continue;

                error = JsonSerializer.Deserialize<Error>(ref reader, options);
            }
            else
            {
                reader.Skip();
            }
        }

        // An error wins over a value: a payload carrying both can only come from a producer that could not omit
        // the value (a non-nullable value type serialized as its default), and the error is the meaningful part.
        if (error is not null)
            return new Result<T>(error);

        return hasValue
            ? new Result<T>(value!)
            : new Result<T>();
    }

    public override void Write(Utf8JsonWriter writer, Result<T> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        var valueName = ResolveName(ValuePropertyName, options);

        // Write the value only when one is present and no error is set. A present value may itself be null
        // (a successful result of a nullable T), written as null. An absent value (a failed or not-yet-populated
        // result) is omitted so it is not mistaken for a successful null value when read back; the error guard
        // additionally ensures a failed result never emits a value.
        if (value.HasValue && value.Error is null)
        {
            writer.WritePropertyName(valueName);
            JsonSerializer.Serialize(writer, value.Value, options);
        }

        var errorName = ResolveName(ErrorPropertyName, options);

        if (value.Error is not null)
        {
            writer.WritePropertyName(errorName);
            JsonSerializer.Serialize(writer, value.Error, options);
        }
        else
        {
            writer.WriteNull(errorName);
        }

        writer.WriteEndObject();
    }

    private static string ResolveName(string name, JsonSerializerOptions options)
    {
        return options.PropertyNamingPolicy?.ConvertName(name) ?? name;
    }
}
