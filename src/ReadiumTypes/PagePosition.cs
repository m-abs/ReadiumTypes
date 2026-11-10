using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes;

/// <summary>Position of a page in a synthetic spread.</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum PagePosition
{
    /// <summary>Left page of a spread.</summary>
    [JsonStringEnumMemberName("left")]
    Left,

    /// <summary>Right page of a spread.</summary>
    [JsonStringEnumMemberName("right")]
    Right,

    /// <summary>Center (single) page of a spread.</summary>
    [JsonStringEnumMemberName("center")]
    Center,
}