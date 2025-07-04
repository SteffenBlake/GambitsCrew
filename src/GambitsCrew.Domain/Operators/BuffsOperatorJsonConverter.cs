using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace GambitsCrew.Domain.Operators;

public class BuffsOperatorJsonConverter(
    IFileProviderService fileProvider
) : JsonConverter<IBuffsOperator>
{
    public override IBuffsOperator? Read(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
    )
    {
        if (reader.TokenType == JsonTokenType.String) {
            var name = reader.GetString()?.Trim();
            if (string.IsNullOrEmpty(name))
            {
                throw new JsonException();
            }

            using var stream = fileProvider.GetOperator(name);
            try
            {
                return JsonSerializer.Deserialize<IBuffsOperator>(stream, options)!;
            }
            catch (JsonException ex)
            {
                throw new JsonException(
                    $"Error serializing Operator: '{name}', see inner exception for details.", 
                    ex
                );
            }
        }

        var raw = JsonSerializer.Deserialize<JsonObject>(ref reader, options)!;

        if (raw.ContainsKey("contains"))
        {
            return raw.Deserialize<BuffsContainsOperator>(options);
        }

        throw new JsonException();
    }

    public override void Write(
        Utf8JsonWriter writer, IBuffsOperator value, JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
