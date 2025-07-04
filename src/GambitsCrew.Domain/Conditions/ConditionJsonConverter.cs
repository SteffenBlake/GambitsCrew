using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace GambitsCrew.Domain.Conditions;

public class ConditionJsonConverter(
    IFileProviderService fileProvider
) : JsonConverter<ICondition>
{
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

        if (raw.ContainsKey("buffs"))
        {
            return raw.Deserialize<BuffsCondition>(options);
        }
        if (raw.ContainsKey("claimed"))
        {
            return raw.Deserialize<ClaimedCondition>(options);
        }
        if (raw.ContainsKey("distance"))
        {
            return raw.Deserialize<DistanceCondition>(options);
        }
        if (raw.ContainsKey("facingTowards"))
        {
            return raw.Deserialize<FacingTowardsCondition>(options);
        }
        if (raw.ContainsKey("hpp"))
        {
            return raw.Deserialize<HppCondition>(options);
        }
        if (raw.ContainsKey("mpp"))
        {
            return raw.Deserialize<MppCondition>(options);
        }
        if (raw.ContainsKey("name"))
        {
            return raw.Deserialize<NameCondition>(options);
        }
        if (raw.ContainsKey("status"))
        {
            return raw.Deserialize<StatusCondition>(options);
        }
        if (raw.ContainsKey("tp"))
        {
            return raw.Deserialize<TpCondition>(options);
        }

        throw new JsonException();
    }

    public override void Write(Utf8JsonWriter writer, ICondition value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
