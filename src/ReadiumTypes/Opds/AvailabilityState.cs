using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes.Opds;

/// <summary>Availability state of a publication or acquisition link.</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum AvailabilityState
{
    /// <summary>The publication is available.</summary>
    [JsonStringEnumMemberName("available")]
    Available,

    /// <summary>The publication is not available.</summary>
    [JsonStringEnumMemberName("unavailable")]
    Unavailable,

    /// <summary>The publication is discoverable but not acquired.</summary>
    [JsonStringEnumMemberName("discoverable")]
    Discoverable,
}