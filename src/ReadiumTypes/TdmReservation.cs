using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes;

/// <summary>Text and data mining reservation expressed as plain text.</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TdmReservation
{
    /// <summary>Text and data mining is not reserved.</summary>
    [JsonStringEnumMemberName("no")]
    No,

    /// <summary>All text and data mining uses are reserved.</summary>
    [JsonStringEnumMemberName("all")]
    All,
}