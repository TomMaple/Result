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
///     Writes an absent <see cref="Result{T}.Value" /> as <see langword="null" />, or omits it entirely when
///     <typeparamref name="T" /> is a non-nullable value type that has no null to write.
/// </summary>
/// <remarks>
///     <para>
///         The default reflection-based contract cannot express "no value" for a non-nullable value type: a failed
///         <c>Result&lt;int&gt;</c> is written as <c>"Value": 0</c>, which is indistinguishable from a successful
///         result carrying zero and which the <see cref="Result{T}.Error" /> initializer rejects on the way back in.
///     </para>
///     <para>
///         Omitting the member is applied only where a null cannot be written, so the payload of every
///         <typeparamref name="T" /> that already round-tripped is unchanged.
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

            // A null token means the member is absent, which is how a failed result reports its value
            // (and a successful one its error). Leave the member unset rather than materializing it.
            if (string.Equals(propertyName, valueName, comparison))
            {
                if (reader.TokenType == JsonTokenType.Null)
                    continue;

                value = JsonSerializer.Deserialize<T>(ref reader, options);
                hasValue = value is not null;
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

        if (value.HasValue)
        {
            writer.WritePropertyName(valueName);
            JsonSerializer.Serialize(writer, value.Value, options);
        }
        else if (Result<T>.CanWriteNullValue)
        {
            writer.WriteNull(valueName);
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
