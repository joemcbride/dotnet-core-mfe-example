using System.Text.Json;
using System.Text.Json.Serialization;

namespace App.Core;

public interface IJsonSerializer
{
    string Serialize<T>(T value);
    T Deserialize<T>(string value);
}

public class JsonSerializer : IJsonSerializer
{
    readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    public JsonSerializer()
        : this(false)
    {
    }

    public JsonSerializer(bool writeIndented)
    {
        _options.WriteIndented = writeIndented;
        _options.Converters.Add(new JsonDateTimeConverter());
    }

    public T Deserialize<T>(string value)
    {
        return System.Text.Json.JsonSerializer.Deserialize<T>(value, _options);
    }

    public string Serialize<T>(T value)
    {
        return System.Text.Json.JsonSerializer.Serialize(value, _options);
    }
}
