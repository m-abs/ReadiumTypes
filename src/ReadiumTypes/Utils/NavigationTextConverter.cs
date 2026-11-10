using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using Mabs.ReadiumTypes.GuidedNavigation;

namespace Mabs.ReadiumTypes.Utils;

/// <summary>
/// Reads guided navigation text as either a bare string (stored as <c>Plain</c>) or an object,
/// and always writes the object form.
/// </summary>
internal sealed class NavigationTextConverter : JsonConverter<NavigationText>
{
    /// <inheritdoc/>
    public override NavigationText Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.String:
                return new NavigationText { Plain = reader.GetString() };
            case JsonTokenType.StartObject:
                NavigationText text = new();
                while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
                {
                    string name = reader.GetString()!;
                    reader.Read();
                    switch (name)
                    {
                        case "plain":
                            text.Plain = reader.GetString();
                            break;
                        case "ssml":
                            text.Ssml = reader.GetString();
                            break;
                        case "language":
                            text.Language = reader.GetString();
                            break;
                        default:
                            reader.Skip();
                            break;
                    }
                }

                return text;
            default:
                throw new JsonException($"Expected a string or object, found {reader.TokenType}.");
        }
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, NavigationText value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        if (value.Plain is not null)
        {
            writer.WriteString("plain", value.Plain);
        }

        if (value.Ssml is not null)
        {
            writer.WriteString("ssml", value.Ssml);
        }

        if (value.Language is not null)
        {
            writer.WriteString("language", value.Language);
        }

        writer.WriteEndObject();
    }
}