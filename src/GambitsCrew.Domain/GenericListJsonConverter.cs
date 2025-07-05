using System.Text.Json;
using System.Text.Json.Serialization;

namespace GambitsCrew.Domain;

public class GenericListJsonConverter<T> : JsonConverter<List<T>>
{
    public override List<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartObject)
        {
            return [JsonSerializer.Deserialize<T>(ref reader, options)];
        }

        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException();

        var list = new List<T>();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
                break;

            list.Add(JsonSerializer.Deserialize<T>(ref reader, options)!);
        }

        return list;
    }

    public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
