// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

#pragma warning disable HAA0601

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Maple.Result.Converters;

/// <summary>
///     A JSON converter that can serialize and deserialize objects as their primitive types.
/// </summary>
/// <remarks>Based on the implementation from: https://stackoverflow.com/a/65974452</remarks>
public class ObjectAsPrimitiveConverter : JsonConverter<object>
{
    /// <summary>
    ///     Reads the JSON value and converts it to the primitive type, if possible.
    /// </summary>
    /// <param name="reader">A <see cref="Utf8JsonReader" /> to read the JSON value from.</param>
    /// <param name="typeToConvert">A type to convert the JSON value to.</param>
    /// <param name="options">A <see cref="JsonSerializerOptions" /> to use for serialization.</param>
    /// <returns>
    ///     A primitive object or an array or a map (can be recursive) of primitive objects, if possible;
    ///     otherwise a <see cref="JsonElement" /> representing the original JSON value.
    /// </returns>
    /// <exception cref="JsonException"></exception>
    public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.Comment:
            case JsonTokenType.None:
            case JsonTokenType.Null:
                return null;

            case JsonTokenType.False:
                return false;

            case JsonTokenType.True:
                return true;

            case JsonTokenType.String:
                return reader.GetString();

            case JsonTokenType.Number:
            {
                if (reader.TryGetInt32(out var int32))
                    return int32;

                if (reader.TryGetInt64(out var int64))
                    return int64;

                if (reader.TryGetDecimal(out var dec))
                    return dec;

                if (reader.TryGetDouble(out var dbl))
                    return dbl;

                using var doc = JsonDocument.ParseValue(ref reader);
                return doc.RootElement.Clone();
            }

            case JsonTokenType.StartArray:
            {
                var list = new List<object>();
                while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                {
                    var item = Read(ref reader, typeof(object), options);
                    if (item is not null)
                        list.Add(item);
                }

                return list;
            }

            case JsonTokenType.StartObject:
            {
                var dict = new Dictionary<string, object?>();
                while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
                {
                    while (reader.TokenType == JsonTokenType.Comment)
                        reader.Read();

                    if (reader.TokenType == JsonTokenType.Null)
                        continue;

                    if (reader.TokenType != JsonTokenType.PropertyName)
                    {
                        using var doc = JsonDocument.ParseValue(ref reader);

                        throw new JsonException($"Cannot parse object “{doc.RootElement.ToString()}”!");
                    }

                    var key = reader.GetString() ?? string.Empty;
                    reader.Read();
                    var value = Read(ref reader, typeof(object), options);
                    dict[key] = value;
                }

                return dict;
            }

            default:
                throw new JsonException($"Unknown token {reader.TokenType}");
        }
    }

    /// <summary>
    ///     Writes the JSON representation of the specified value.
    /// </summary>
    /// <param name="writer">A <see cref="Utf8JsonWriter" /> to write the JSON value to.</param>
    /// <param name="value">The value to write.</param>
    /// <param name="options">A <see cref="JsonSerializerOptions" /> to use for serialization.</param>
    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        if (value.GetType() == typeof(object))
        {
            writer.WriteStartObject();
            writer.WriteEndObject();
        }
        else
        {
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
