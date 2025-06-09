using System.Text.Json;
using System.Text.Json.Serialization;

namespace GambitsCrew.Domain.Gambits;

public class GambitJsonConverter(
    IFileProviderService fileProvider
) : JsonConverter<Gambit>
{
    public override Gambit? Read(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
    )
    {
        if (reader.TokenType == JsonTokenType.String) {
            var name = reader.GetString()?.Trim();
            if (string.IsNullOrEmpty(name))
            {
                throw new JsonException("Crew Member by name cannot be null or empty");
            }

            using var stream = fileProvider.GetCrewMember(name);
            return JsonSerializer.Deserialize<Gambit>(stream, options)!;
        }

        return JsonSerializer.Deserialize<Gambit>(ref reader, options)!;
    }

    public override void Write(
        Utf8JsonWriter writer, Gambit value, JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
