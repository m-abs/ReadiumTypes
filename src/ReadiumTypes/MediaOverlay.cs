using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes;

/// <summary>A media overlay document of a publication.</summary>
public record MediaOverlay : IExtensionDataHolder
{
    /// <summary>URI of the media overlay document.</summary>
    [JsonPropertyName("href")]
    public string Href { get; set; } = string.Empty;

    /// <summary>MIME type of the media overlay document.</summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>BCP 47 language tag of the media overlay.</summary>
    [JsonPropertyName("language")]
    public string? Language { get; set; }

    /// <summary>MIME type of the audio resources referenced by the media overlay.</summary>
    [JsonPropertyName("audio")]
    public string? Audio { get; set; }

    /// <summary>Playback speed of the media overlay.</summary>
    [JsonPropertyName("playback-speed")]
    public double? PlaybackSpeed { get; set; }

    /// <summary>Text-to-speech voice of the media overlay.</summary>
    [JsonPropertyName("text-to-speech-voice")]
    public string? TextToSpeechVoice { get; set; }

    /// <summary>Text-to-speech rate of the media overlay.</summary>
    [JsonPropertyName("text-to-speech-rate")]
    public string? TextToSpeechRate { get; set; }

    /// <summary>Volume of the media overlay.</summary>
    [JsonPropertyName("volume")]
    public double? Volume { get; set; }

    /// <summary>Root node of the media overlay.</summary>
    [JsonPropertyName("nodes")]
    public MediaOverlayNode? Nodes { get; set; }

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }
}