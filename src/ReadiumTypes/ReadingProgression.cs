using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes;

/// <summary>Reading progression of a publication.</summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ReadingProgression
{
    /// <summary>Left-to-right progression.</summary>
    [JsonStringEnumMemberName("ltr")]
    Ltr,

    /// <summary>Right-to-left progression.</summary>
    [JsonStringEnumMemberName("rtl")]
    Rtl,

    /// <summary>Vertical right-to-left progression.</summary>
    [JsonStringEnumMemberName("vertical-rl")]
    VerticalRl,

    /// <summary>Vertical left-to-right progression.</summary>
    [JsonStringEnumMemberName("vertical-lr")]
    VerticalLr,

    /// <summary>Webtoon-style vertical progression.</summary>
    [JsonStringEnumMemberName("tts")]
    Tts,

    /// <summary>Reading progression is not specified.</summary>
    [JsonStringEnumMemberName("auto")]
    Auto,
}