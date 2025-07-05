using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace GambitsCrew.Domain.Operators;

public class NumberOperatorJsonConverter(
    IFileProviderService fileProvider
) : JsonConverter<INumberOperator>
{
    private static T? Deserialize<T>(JsonObject raw, JsonSerializerOptions options)
    {
        return raw.Deserialize<T>(options);
    }

    private static readonly Dictionary<string, Func<JsonObject, JsonSerializerOptions, INumberOperator?>> _mappings
        = new(StringComparer.OrdinalIgnoreCase)
        {
            { "eq", Deserialize<NumberEqualsOperator> },
            { "gt", Deserialize<NumberGreaterThanOperator> },
            { "gte", Deserialize<NumberGreaterThanOrEqualsOperator> },
            { "lt", Deserialize<NumberLessThanOperator> },
            { "lte", Deserialize<NumberLessThanOrEqualsOperator> },
            { "ne", Deserialize<NumberNotEqualsOperator> }
        };

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
                    $"Error serializing Number Operator: '{name}', see inner exception for details.", 
                    ex
                );
            }
        }

        var raw = JsonSerializer.Deserialize<JsonObject>(ref reader, options)!;
        if (raw.Count > 1)
        {
            throw new JsonException(
                $"Unexpected property count for Number Operator, expected 1, got {raw.Count}"
            );
        }

        var key = raw.Single().Key;

        if (_mappings.TryGetValue(key.ToLower(), out var mapping))
        {
            return mapping(raw, options);
        }

        throw new JsonException(
            $"Unrecognized Number Operator key '{key}', expected one of: '{string.Join('|', _mappings.Keys)}'"
        );
    }

    public override void Write(
        Utf8JsonWriter writer, INumberOperator value, JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}