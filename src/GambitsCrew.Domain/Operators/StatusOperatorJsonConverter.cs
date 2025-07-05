using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace GambitsCrew.Domain.Operators;

public class StatusOperatorJsonConverter(
    IFileProviderService fileProvider
) : JsonConverter<IStatusOperator>
{
    private static T? Deserialize<T>(JsonObject raw, JsonSerializerOptions options)
    {
        return raw.Deserialize<T>(options);
    }

    private static readonly Dictionary<string, Func<JsonObject, JsonSerializerOptions, IStatusOperator?>> _mappings
        = new(StringComparer.OrdinalIgnoreCase)
        {
            { "hasFlag", Deserialize<StatusHasFlagOperator> },
        };

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
                    $"Error serializing Status Operator: '{name}', see inner exception for details.", 
                    ex
                );
            }
        }

        var raw = JsonSerializer.Deserialize<JsonObject>(ref reader, options)!;
        if (raw.Count > 1)
        {
            throw new JsonException(
                $"Unexpected property count for Status Operator, expected 1, got {raw.Count}"
            );
        }

        var key = raw.Single().Key;

        if (_mappings.TryGetValue(key.ToLower(), out var mapping))
        {
            return mapping(raw, options);
        }

        throw new JsonException(
            $"Unrecognized Status Operator key '{key}', expected one of: '{string.Join('|', _mappings.Keys)}'"
        );
    }

    public override void Write(
        Utf8JsonWriter writer, IStatusOperator value, JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
