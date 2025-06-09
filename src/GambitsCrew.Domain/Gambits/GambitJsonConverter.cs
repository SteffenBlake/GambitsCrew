using System.Text.Json;
using System.Text.Json.Serialization;

namespace GambitsCrew.Domain.Gambits;

public class GambitJsonConverter(
    IFileProviderService fileProvider
) : JsonConverter<IGambit>
{
    public override IGambit? Read(
        ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options
    )
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var name = reader.GetString()?.Trim();
            if (string.IsNullOrEmpty(name))
            {
                throw new JsonException("Crew Member by name cannot be null or empty");
            }

            using var stream = fileProvider.GetGambit(name);

            try
            {
                return JsonSerializer.Deserialize<Gambit>(stream, options)!;
            }
            catch (JsonException ex)
            {
                throw new JsonException(
                    $"Error serializing Gambit: '{name}', see inner exception for details.", 
                    ex
                );
            }
        }

        return JsonSerializer.Deserialize<Gambit>(ref reader, options)!;
    }

    public override void Write(
        Utf8JsonWriter writer, IGambit value, JsonSerializerOptions options
    )
    {
        JsonSerializer.Serialize(writer, value, options);
    }
}
