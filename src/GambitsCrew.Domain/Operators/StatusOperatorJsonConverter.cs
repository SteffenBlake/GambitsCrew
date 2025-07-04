using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace GambitsCrew.Domain.Operators;

public class StatusOperatorJsonConverter(
    IFileProviderService fileProvider
) : JsonConverter<IStatusOperator>
{
    public override IStatusOperator? Read(
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
                return JsonSerializer.Deserialize<IStatusOperator>(stream, options)!;
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

        if (raw.ContainsKey("hasFlag"))
        {
            return raw.Deserialize<StatusHasFlagOperator>(options);
        }

        throw new JsonException();
    }

    public override void Write(
        Utf8JsonWriter writer, IStatusOperator value, JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
