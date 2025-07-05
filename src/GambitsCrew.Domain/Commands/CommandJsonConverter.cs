using GambitsCrew.Domain.Selectors;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace GambitsCrew.Domain.Commands;

public class CommandJsonConverter(
    IFileProviderService fileProvider
) : JsonConverter<ICommand>
{
    private static T? Deserialize<T>(JsonObject raw, JsonSerializerOptions options)
    {
        return raw.Deserialize<T>(options);
    }

    private static readonly Dictionary<string, Func<JsonObject, JsonSerializerOptions, ICommand?>> _mappings
        = new(StringComparer.OrdinalIgnoreCase)
        {
            { "ja", Deserialize<AbilityCommand> },
            { "all", Deserialize<AllCommand> },
            { "any", Deserialize<AnyCommand> },
            { "assist", Deserialize<AssistCommand> },
            { "attack", Deserialize<AttackCommand> },
            { "cast", Deserialize<CastCommand> },
            { "execute", Deserialize<ExecuteCommand> },
            { "faceTowards", Deserialize<FaceTowardsCommand> },
            { "follow", Deserialize<FollowCommand> },
            { "item", Deserialize<ItemCommand> },
            { "pet", Deserialize<PetCommand> },
            { "target", Deserialize<TargetCommand> },
            { "ws", Deserialize<WeaponskillCommand> }
        };


    public override ICommand? Read(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
    )
    {
        if (reader.TokenType == JsonTokenType.String) {
            var name = reader.GetString()?.Trim();
            if (string.IsNullOrEmpty(name))
            {
                throw new JsonException();
            }

            using var stream = fileProvider.GetCommand(name);

            try
            {
                return JsonSerializer.Deserialize<ICommand>(stream, options)!;
            }
            catch (JsonException ex)
            {
                throw new JsonException(
                    $"Error serializing Command: '{name}', see inner exception for details.", 
                    ex
                );
            }
        }

        var raw = JsonSerializer.Deserialize<JsonObject>(ref reader, options)!;

        if (raw.Count > 1)
        {
            throw new JsonException(
                $"Unexpected property count for Command, expected 1, got {raw.Count}"
            );
        }

        var key = raw.Single().Key;

        if (_mappings.TryGetValue(key.ToLower(), out var mapping))
        {
            return mapping(raw, options);
        }

        throw new JsonException(
            $"Unrecognized Command key '{key}', expected one of: '{string.Join('|', _mappings.Keys)}'"
        );
    }

    public override void Write(
        Utf8JsonWriter writer, ICommand value, JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
