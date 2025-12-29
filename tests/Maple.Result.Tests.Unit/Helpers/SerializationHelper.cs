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