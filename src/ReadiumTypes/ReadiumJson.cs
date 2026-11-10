using System.Text.Json;
using System.Text.Json.Serialization;

using Mabs.ReadiumTypes.Utils;

namespace Mabs.ReadiumTypes;

/// <summary>Shared <see cref="JsonSerializerOptions"/> for all types in this library.</summary>
public static class ReadiumJson
{
    /// <summary>
    /// Options enabling <c>ShouldSerializeX</c> support through
    /// <see cref="ShouldSerializeJsonTypeInfoResolver"/>. Property names come from
    /// <see cref="System.Text.Json.Serialization.JsonPropertyNameAttribute"/> on each property.
    /// </summary>
    public static JsonSerializerOptions Options { get; } = new()
    {
        TypeInfoResolver = new ShouldSerializeJsonTypeInfoResolver(),

        // Optional properties absent from the wire; nulls would fail schema validation (ADR 0005).
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,

        // Official fixtures use strings for numeric link fields (e.g. "width": "992").
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
    };
}