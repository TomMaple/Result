// SPDX-License-Identifier: MIT
/*
 * This code is a part of a Maple.Result library project.
 * https://github.com/TomMaple/Result/
 * Copyright (c) Tom Maple
 *
 * This source code is licensed under the MIT license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System.Text.Json;

namespace Maple.Result.Tests.Unit.Helpers;

internal static class SerializationHelper
{
    internal static T DeserializeWithMicrosoft<T>(string json)
    {
        var result = JsonSerializer.Deserialize<T>(json);
        return result!;
    }

    internal static T DeserializeWithNewtonsoft<T>(string json)
    {
        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        return result!;
    }

    internal static string SerializeWithMicrosoft<T>(T error)
    {
        var result = JsonSerializer.Serialize(error);
        return result;
    }

    internal static string SerializeWithNewtonsoft<T>(T error)
    {
        var result = Newtonsoft.Json.JsonConvert.SerializeObject(error);
        return result;
    }
}