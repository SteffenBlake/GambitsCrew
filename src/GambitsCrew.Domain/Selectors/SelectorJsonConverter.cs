using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace GambitsCrew.Domain.Selectors;

public class SelectorJsonConverter(
    IFileProviderService fileProvider
) : JsonConverter<ISelector>
{
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

        if (raw.ContainsKey("a"))
        {
            return raw.Deserialize<AllianceSelector>(options);
        }
        if (raw.ContainsKey("ax"))
        {
            return raw.Deserialize<AllianceOtherSelector>(options);
        }
        if (raw.ContainsKey("and"))
        {
            return raw.Deserialize<AndSelector>(options);
        }
        if (raw.ContainsKey("bt"))
        {
            return raw.Deserialize<BattleTargetSelector>(options);
        }
        if (raw.ContainsKey("l"))
        {
            return raw.Deserialize<LeaderSelector>(options);
        }
        if (raw.ContainsKey("me"))
        {
            return raw.Deserialize<MeSelector>(options);
        }
        if (raw.ContainsKey("not"))
        {
            return raw.Deserialize<NotSelector>(options);
        }
        if (raw.ContainsKey("or"))
        {
            return raw.Deserialize<OrSelector>(options);
        }
        if (raw.ContainsKey("px"))
        {
            return raw.Deserialize<PartyOtherSelector>(options);
        }
        if (raw.ContainsKey("p"))
        {
            return raw.Deserialize<PartySelector>(options);
        }
        if (raw.ContainsKey("pet"))
        {
            return raw.Deserialize<PetSelector>(options);
        }
        if (raw.ContainsKey("scan"))
        {
            return raw.Deserialize<ScanSelector>(options);
        }
        if (raw.ContainsKey("t"))
        {
            return raw.Deserialize<TargetSelector>(options);
        }

        throw new JsonException();
    }

    public override void Write(
        Utf8JsonWriter writer, ISelector value, JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
