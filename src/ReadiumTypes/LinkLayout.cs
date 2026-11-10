using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes;

/// <summary>Hint about the nature of the layout of a linked resource (EPUB profile).</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LinkLayout
{
    /// <summary>Reflowable layout.</summary>
    [JsonStringEnumMemberName("reflowable")]
    Reflowable,

    /// <summary>Fixed layout.</summary>
    [JsonStringEnumMemberName("fixed")]
    Fixed,
}