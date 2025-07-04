using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace GambitsCrew.Domain.Operators;

public class NumberOperatorJsonConverter(
    IFileProviderService fileProvider
) : JsonConverter<INumberOperator>
{
    public override INumberOperator? Read(
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
                return JsonSerializer.Deserialize<INumberOperator>(stream, options)!;
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

        if (raw.ContainsKey("eq"))
        {
            return raw.Deserialize<NumberEqualsOperator>(options);
        }
        if (raw.ContainsKey("gt"))
        {
            return raw.Deserialize<NumberGreaterThanOperator>(options);
        }
        if (raw.ContainsKey("gte"))
        {
            return raw.Deserialize<NumberGreaterThanOrEqualsOperator>(options);
        }
        if (raw.ContainsKey("lt"))
        {
            return raw.Deserialize<NumberLessThanOperator>(options);
        }
        if (raw.ContainsKey("lte"))
        {
            return raw.Deserialize<NumberLessThanOrEqualsOperator>(options);
        }
        if (raw.ContainsKey("ne"))
        {
            return raw.Deserialize<NumberNotEqualsOperator>(options);
        }

        throw new JsonException();
    }

    public override void Write(
        Utf8JsonWriter writer, INumberOperator value, JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}