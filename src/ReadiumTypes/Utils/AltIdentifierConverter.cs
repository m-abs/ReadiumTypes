using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes.Utils;

/// <summary>
/// Reads the <c>altIdentifier</c> array whose items are bare identifier strings or objects,
/// normalizing strings to <see cref="AltIdentifier"/> values. Always writes the object form.
/// </summary>
internal sealed class AltIdentifierConverter : JsonConverter<List<AltIdentifier>>
{
    /// <inheritdoc/>
    public override List<AltIdentifier> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
        {
            throw new JsonException($"Expected an array, found {reader.TokenType}.");
        }

        List<AltIdentifier> items = [];
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                items.Add(new AltIdentifier { Value = reader.GetString()! });
            }
            else if (reader.TokenType == JsonTokenType.StartObject)
            {
                items.Add(JsonSerializer.Deserialize<AltIdentifier>(ref reader, options)!);
            }
            else
            {
                throw new JsonException($"Expected a string or object, found {reader.TokenType}.");
            }
        }

        return items;
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, List<AltIdentifier> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (AltIdentifier item in value)
        {
            JsonSerializer.Serialize(writer, item, options);
        }

        writer.WriteEndArray();
    }
}