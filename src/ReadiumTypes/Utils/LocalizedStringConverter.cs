using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes.Utils;

/// <summary>
/// Reads every localized-string wire form into a <see cref="LocalizedString"/>: a bare string,
/// a webpub language map, an OPDS <c>{"value": ..., "language": ...}</c> object, and an OPDS
/// array of such objects. Writes a bare string when only the default language is present,
/// otherwise the webpub map form.
/// </summary>
internal sealed class LocalizedStringConverter : JsonConverter<LocalizedString?>
{
    private static LocalizedString ReadObject(ref Utf8JsonReader reader)
    {
        Dictionary<string, string> entries = [];
        while (reader.Read() && reader.TokenType != JsonTokenType.EndObject)
        {
            string key = reader.GetString()!;
            reader.Read();
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException($"Expected a string for \"{key}\", found {reader.TokenType}.");
            }

            entries[key] = reader.GetString()!;
        }

        LocalizedString value = new();
        if (entries.TryGetValue("value", out string? opdsValue))
        {
            entries.TryGetValue("language", out string? language);
            value[language ?? LocalizedString.UndeterminedLanguage] = opdsValue;
            return value;
        }

        foreach (KeyValuePair<string, string> entry in entries)
        {
            value[entry.Key] = entry.Value;
        }

        return value;
    }

    private static LocalizedString ReadArray(ref Utf8JsonReader reader)
    {
        LocalizedString value = new();
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException($"Expected an object inside the array, found {reader.TokenType}.");
            }

            LocalizedString entry = ReadObject(ref reader);
            foreach (KeyValuePair<string, string> pair in entry)
            {
                value[pair.Key] = pair.Value;
            }
        }

        return value;
    }

    /// <inheritdoc/>
    public override LocalizedString? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Null => null,
            JsonTokenType.String => new LocalizedString(reader.GetString()!),
            JsonTokenType.StartObject => ReadObject(ref reader),
            JsonTokenType.StartArray => ReadArray(ref reader),
            _ => throw new JsonException($"Expected a string, object, or array, found {reader.TokenType}."),
        };
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, LocalizedString? value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        if (value.Count == 1 && value.Any(pair => pair.Key == LocalizedString.UndeterminedLanguage))
        {
            writer.WriteStringValue(value.Default);
            return;
        }

        writer.WriteStartObject();
        foreach (KeyValuePair<string, string> entry in value)
        {
            writer.WriteString(entry.Key, entry.Value);
        }

        writer.WriteEndObject();
    }
}