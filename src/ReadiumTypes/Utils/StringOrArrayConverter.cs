using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes.Utils;

/// <summary>
/// Reads wire values declared as <c>string | string[]</c> into a list and always writes the
/// array form, which both alternatives of the union accept.
/// </summary>
internal sealed class StringOrArrayConverter : JsonConverter<List<string>>
{
    /// <inheritdoc/>
    public override List<string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.String:
                return [reader.GetString()!];
            case JsonTokenType.StartArray:
                List<string> items = [];
                while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                {
                    if (reader.TokenType != JsonTokenType.String)
                    {
                        throw new JsonException($"Expected a string inside the array, found {reader.TokenType}.");
                    }

                    items.Add(reader.GetString()!);
                }

                return items;
            default:
                throw new JsonException($"Expected a string or an array of strings, found {reader.TokenType}.");
        }
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, List<string> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (string item in value)
        {
            writer.WriteStringValue(item);
        }

        writer.WriteEndArray();
    }
}