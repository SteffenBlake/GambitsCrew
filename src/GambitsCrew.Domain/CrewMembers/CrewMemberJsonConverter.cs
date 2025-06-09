using System.Text.Json;
using System.Text.Json.Serialization;

namespace GambitsCrew.Domain.CrewMembers;

public class CrewMemberJsonConverter(
    IFileProviderService fileProvider
) : JsonConverter<ICrewMember>
{
    public override ICrewMember? Read(
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

            try
            {
                return JsonSerializer.Deserialize<CrewMember>(stream, options)!;
            }
            catch (JsonException ex)
            {
                throw new JsonException(
                    $"Error serializing Condition: '{name}', see inner exception for details.", 
                    ex
                );
            }
        }

        return JsonSerializer.Deserialize<CrewMember>(ref reader, options)!;
    }

    public override void Write(
        Utf8JsonWriter writer, ICrewMember value, JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
