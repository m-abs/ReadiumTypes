using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes.Utils;

/// <summary>
/// Reads the webpub "one object, string, number, or mixed array" union (contributor, subject,
/// series, ...) into a list: a bare string becomes an item whose <c>Name</c> is that string, a
/// bare number becomes an item whose <c>Position</c> is that number, and objects are read
/// normally. Always writes the array form.
/// </summary>
/// <typeparam name="T">The model type; must expose settable <c>Name</c> and/or <c>Position</c> properties.</typeparam>
internal sealed class OneOrArrayObjectConverter<T> : JsonConverter<List<T>>
    where T : class, new()
{
    private static readonly PropertyInfo? NameProperty = typeof(T).GetProperty("Name");
    private static readonly PropertyInfo? PositionProperty = typeof(T).GetProperty("Position");

    private static T FromName(string name)
    {
        T item = new();
        if (NameProperty?.PropertyType == typeof(LocalizedString))
        {
            NameProperty.SetValue(item, new LocalizedString(name));
        }
        else if (NameProperty?.PropertyType == typeof(string))
        {
            NameProperty.SetValue(item, name);
        }

        return item;
    }

    private static T FromPosition(double position)
    {
        T item = new();
        PositionProperty?.SetValue(item, position);
        return item;
    }

    private static T ReadItem(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.String => FromName(reader.GetString()!),
            JsonTokenType.Number => FromPosition(reader.GetDouble()),
            JsonTokenType.StartObject => JsonSerializer.Deserialize<T>(ref reader, options)!,
            _ => throw new JsonException($"Expected a string, number, or object, found {reader.TokenType}."),
        };
    }

    /// <inheritdoc/>
    public override List<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        switch (reader.TokenType)
        {
            case JsonTokenType.String:
            case JsonTokenType.Number:
                return [ReadItem(ref reader, options)];
            case JsonTokenType.StartArray:
                List<T> items = [];
                while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                {
                    items.Add(ReadItem(ref reader, options));
                }

                return items;
            case JsonTokenType.StartObject:
                return [JsonSerializer.Deserialize<T>(ref reader, options)!];
            default:
                throw new JsonException($"Expected an object, string, number, or array, found {reader.TokenType}.");
        }
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        foreach (T item in value)
        {
            JsonSerializer.Serialize(writer, item, options);
        }

        writer.WriteEndArray();
    }
}