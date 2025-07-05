using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace GambitsCrew.Domain.Conditions;

public class ConditionJsonConverter(
    IFileProviderService fileProvider
) : JsonConverter<ICondition>
{
    private static T? Deserialize<T>(JsonObject raw, JsonSerializerOptions options)
    {
        return raw.Deserialize<T>(options);
    }

    private static readonly Dictionary<string, Func<JsonObject, JsonSerializerOptions, ICondition?>> _mappings
        = new(StringComparer.OrdinalIgnoreCase)
        {
            { "buffs", Deserialize<BuffsCondition> },
            { "claimed", Deserialize<ClaimedCondition> },
            { "distance", Deserialize<DistanceCondition> },
            { "facingTowards", Deserialize<FacingTowardsCondition> },
            { "hpp", Deserialize<HppCondition> },
            { "mpp", Deserialize<MppCondition> },
            { "name", Deserialize<NameCondition> },
            { "status", Deserialize<StatusCondition> },
            { "tp", Deserialize<TpCondition> }
        };

    public override ICondition? Read(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
    )
    {
        if (reader.TokenType == JsonTokenType.String) {
            var name = reader.GetString()?.Trim();
            if (string.IsNullOrEmpty(name))
            {
                throw new JsonException();
            }

            using var stream = fileProvider.GetCondition(name);

            try
            {
                return JsonSerializer.Deserialize<ICondition>(stream, options)!;
            }
            catch (JsonException ex)
            {
                throw new JsonException(
                    $"Error serializing Condition: '{name}', see inner exception for details.", 
                    ex
                );
            }
        }

        var raw = JsonSerializer.Deserialize<JsonObject>(ref reader, options)!;

        if (raw.Count > 1)
        {
            throw new JsonException(
                $"Unexpected property count for Condition, expected 1, got {raw.Count}"
            );
        }

        var key = raw.Single().Key;

        if (_mappings.TryGetValue(key.ToLower(), out var mapping))
        {
            return mapping(raw, options);
        }

        throw new JsonException(
            $"Unrecognized Condition key '{key}', expected one of: '{string.Join('|', _mappings.Keys)}'"
        );
    }

    public override void Write(Utf8JsonWriter writer, ICondition value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
