using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes;

/// <summary>How a publication should be displayed in a synthetic spread.</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Layout
{
    /// <summary>Reflowable content.</summary>
    [JsonStringEnumMemberName("reflowable")]
    Reflowable,

    /// <summary>Fixed-layout content.</summary>
    [JsonStringEnumMemberName("fixed")]
    Fixed,

    /// <summary>Scrollable content.</summary>
    [JsonStringEnumMemberName("scrolled")]
    Scrolled,
}