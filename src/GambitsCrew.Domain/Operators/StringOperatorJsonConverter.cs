using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace GambitsCrew.Domain.Operators;

public class StringOperatorJsonConverter(
    IFileProviderService fileProvider
) : JsonConverter<IStringOperator>
{
    private static T? Deserialize<T>(JsonObject raw, JsonSerializerOptions options)
    {
        return raw.Deserialize<T>(options);
    }

    private static readonly Dictionary<string, Func<JsonObject, JsonSerializerOptions, IStringOperator?>> _mappings
        = new(StringComparer.OrdinalIgnoreCase)
        {
            { "contains", Deserialize<StringContainsOperator> },
            { "eq", Deserialize<StringEqualsOperator> },
            { "ne", Deserialize<StringNotEqualsOperator> },
            { "lower", Deserialize<StringToLowerOperator> }
        };


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

            try
            {
                return JsonSerializer.Deserialize<IStringOperator>(stream, options)!;
            }
            catch (JsonException ex)
            {
                throw new JsonException(
                    $"Error serializing String Operator: '{name}', see inner exception for details.", 
                    ex
                );
            }
        }

        var raw = JsonSerializer.Deserialize<JsonObject>(ref reader, options)!;
        if (raw.Count > 1)
        {
            throw new JsonException(
                $"Unexpected property count for String Operator, expected 1, got {raw.Count}"
            );
        }

        var key = raw.Single().Key;

        if (_mappings.TryGetValue(key.ToLower(), out var mapping))
        {
            return mapping(raw, options);
        }

        throw new JsonException(
            $"Unrecognized String Operator key '{key}', expected one of: '{string.Join('|', _mappings.Keys)}'"
        );
    }

    public override void Write(
        Utf8JsonWriter writer, IStringOperator value, JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
