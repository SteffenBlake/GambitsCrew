using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace GambitsCrew.Domain.Selectors;

public class SelectorJsonConverter(
    IFileProviderService fileProvider
) : JsonConverter<ISelector>
{
    private static T? Deserialize<T>(JsonObject raw, JsonSerializerOptions options)
    {
        return raw.Deserialize<T>(options);
    }

    private static readonly Dictionary<string, Func<JsonObject, JsonSerializerOptions, ISelector?>> _mappings 
        = new(StringComparer.OrdinalIgnoreCase)
    {
        { "a", Deserialize<AllianceSelector> },
        { "ax", Deserialize<AllianceOtherSelector> },
        { "and", Deserialize<AndSelector> },
        { "bt", Deserialize<BattleTargetSelector> },
        { "l", Deserialize<LeaderSelector> },
        { "me", Deserialize<MeSelector> },
        { "not", Deserialize<NotSelector> },
        { "or", Deserialize<OrSelector> },
        { "px", Deserialize<PartyOtherSelector> },
        { "p", Deserialize<PartySelector> },
        { "pet", Deserialize<PetSelector> },
        { "scan", Deserialize<ScanSelector> },
        { "t", Deserialize<TargetSelector> }
    };

    public override ISelector? Read(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
    )
    {
        if (reader.TokenType == JsonTokenType.String) {
            var name = reader.GetString()?.Trim();
            if (string.IsNullOrEmpty(name))
            {
                throw new JsonException();
            }

            using var stream = fileProvider.GetSelector(name);

            try
            {
                return JsonSerializer.Deserialize<ISelector>(stream, options)!;
            }
            catch (JsonException ex)
            {
                throw new JsonException(
                    $"Error serializing Selector: '{name}', see inner exception for details.", 
                    ex
                );
            }
        }

        var raw = JsonSerializer.Deserialize<JsonObject>(ref reader, options)!;

        if (raw.Count > 1)
        {
            throw new JsonException(
                $"Unexpected property count for Selector, expected 1, got {raw.Count}"
            );
        }

        var key = raw.Single().Key;

        if (_mappings.TryGetValue(key.ToLower(), out var mapping))
        {
            return mapping(raw, options);
        }

        throw new JsonException(
            $"Unrecognized Selector key '{key}', expected one of: '{string.Join('|', _mappings.Keys)}'"
        );
    }

    public override void Write(
        Utf8JsonWriter writer, ISelector value, JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
