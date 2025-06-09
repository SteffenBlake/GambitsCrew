using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace GambitsCrew.Domain.Commands;

public class CommandJsonConverter(
    IFileProviderService fileProvider
) : JsonConverter<ICommand>
{
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

        if (raw.ContainsKey("ja"))
        {
            return raw.Deserialize<AbilityCommand>(options);
        }
        if (raw.ContainsKey("all"))
        {
            return raw.Deserialize<AllCommand>(options);
        }
        if (raw.ContainsKey("any"))
        {
            return raw.Deserialize<AnyCommand>(options);
        }
        if (raw.ContainsKey("assist"))
        {
            return raw.Deserialize<AssistCommand>(options);
        }
        if (raw.ContainsKey("attack"))
        {
            return raw.Deserialize<AttackCommand>(options);
        }
        if (raw.ContainsKey("cast"))
        {
            return raw.Deserialize<CastCommand>(options);
        }
        if (raw.ContainsKey("execute"))
        {
            return raw.Deserialize<ExecuteCommand>(options);
        }
        if (raw.ContainsKey("item"))
        {
            return raw.Deserialize<ItemCommand>(options);
        }
        if (raw.ContainsKey("pet"))
        {
            return raw.Deserialize<PetCommand>(options);
        }
        if (raw.ContainsKey("ws"))
        {
            return raw.Deserialize<WeaponskillCommand>(options);
        }

        throw new JsonException();
    }

    public override void Write(
        Utf8JsonWriter writer, ICommand value, JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
