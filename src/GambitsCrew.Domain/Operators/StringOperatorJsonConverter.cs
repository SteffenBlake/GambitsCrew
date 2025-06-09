using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace GambitsCrew.Domain.Operators;

public class StringOperatorJsonConverter(
    IFileProviderService fileProvider
) : JsonConverter<IStringOperator>
{
    public override IStringOperator? Read(
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
            return JsonSerializer.Deserialize<IStringOperator>(stream, options)!;
        }

        var raw = JsonSerializer.Deserialize<JsonObject>(ref reader, options)!;

        if (raw.ContainsKey("contains"))
        {
            return raw.Deserialize<StringContainsOperator>(options);
        }
        if (raw.ContainsKey("eq"))
        {
            return raw.Deserialize<StringEqualsOperator>(options);
        }
        if (raw.ContainsKey("ne"))
        {
            return raw.Deserialize<StringNotEqualsOperator>(options);
        }
        if (raw.ContainsKey("lower"))
        {
            return raw.Deserialize<StringToLowerOperator>(options);
        }

        throw new JsonException();
    }

    public override void Write(
        Utf8JsonWriter writer, IStringOperator value, JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
