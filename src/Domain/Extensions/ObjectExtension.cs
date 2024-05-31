using System.Text.Json;
using System.Text.Json.Serialization;

namespace Domain.Extensions;

public static class ObjectExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    public static string AsJson(this object? source)
    {
        if (source is null)
        {
            return string.Empty;
        }

        return JsonSerializer.Serialize(source, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
        });
    }
}
