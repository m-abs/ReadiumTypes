using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes;

/// <summary>Hint about the nature of the layout of a linked resource.</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LinkLayout
{
    /// <summary>Default layout.</summary>
    [JsonStringEnumMemberName("default")]
    Default,

    /// <summary>Pre-paginated layout.</summary>
    [JsonStringEnumMemberName("pre-paginated")]
    PrePaginated,

    /// <summary>Non-scrollable layout.</summary>
    [JsonStringEnumMemberName("non-scrollable")]
    NonScrollable,

    /// <summary>Scrollable layout.</summary>
    [JsonStringEnumMemberName("scrollable")]
    Scrollable,
}