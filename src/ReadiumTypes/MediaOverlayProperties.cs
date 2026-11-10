using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes;

/// <summary>
/// W3C synchronized-narration media overlay properties of a <see cref="Link"/>.
/// </summary>
public record MediaOverlayProperties : IExtensionDataHolder
{
    /// <summary>Override for the playback speed of the media overlay.</summary>
    [JsonPropertyName("playback-speed")]
    public double? PlaybackSpeed { get; set; }

    /// <summary>Override for the text-to-speech voice of the media overlay.</summary>
    [JsonPropertyName("text-to-speech-voice")]
    public string? TextToSpeechVoice { get; set; }

    /// <summary>Override for the text-to-speech rate of the media overlay.</summary>
    [JsonPropertyName("text-to-speech-rate")]
    public string? TextToSpeechRate { get; set; }

    /// <summary>Override for the volume of the media overlay.</summary>
    [JsonPropertyName("volume")]
    public double? Volume { get; set; }

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }
}