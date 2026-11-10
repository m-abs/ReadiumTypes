using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes;

/// <summary>A node of a media overlay document.</summary>
public record MediaOverlayNode : IExtensionDataHolder
{
    /// <summary>Identifier of the node.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>Identifier of the node's parent.</summary>
    [JsonPropertyName("ref-id")]
    public string? RefId { get; set; }

    /// <summary>Class of the node.</summary>
    [JsonPropertyName("class")]
    public string? Class { get; set; }

    /// <summary>Text fragment the node points to.</summary>
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    /// <summary>Media type of the text fragment.</summary>
    [JsonPropertyName("text-type")]
    public string? TextType { get; set; }

    /// <summary>Media type of the audio resource.</summary>
    [JsonPropertyName("audio")]
    public string? Audio { get; set; }

    /// <summary>Media fragment identifying the audio snippet.</summary>
    [JsonPropertyName("audio-src")]
    public string? AudioSrc { get; set; }

    /// <summary>Start position of the audio snippet as a CSS selector.</summary>
    [JsonPropertyName("audio-text")]
    public string? AudioText { get; set; }

    /// <summary>Child nodes.</summary>
    [JsonPropertyName("children")]
    public List<MediaOverlayNode> Children { get; set; } = [];

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }

    /// <summary>Determines whether <see cref="Children"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Children"/> contains items.</returns>
    public bool ShouldSerializeChildren()
    {
        return Children.Count > 0;
    }
}