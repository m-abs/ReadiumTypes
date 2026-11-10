using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using Mabs.ReadiumTypes.Utils;

namespace Mabs.ReadiumTypes;

/// <summary>
/// A node of a synchronized-narration (syncnarr) media overlay document, as parsed
/// from EPUB 3 Media Overlays SMIL by
/// <see href="https://github.com/edrlab/thorium-reader/blob/03d89b20c894273f78f210d593c75e869dc2ec82/src/r2-xxx-js/r2-shared-js/models/media-overlay.ts">Thorium's media-overlay model</see>.
/// A leaf pairs a <see cref="Text"/> fragment with an <see cref="Audio"/> fragment;
/// a sequence carries only <see cref="Role"/> and nested <see cref="Narration"/>.
/// </summary>
public record MediaOverlayNode : IExtensionDataHolder
{
    /// <summary>Document reference of the text fragment, e.g. <c>chapter1.html#paragraph1</c>.</summary>
    [JsonPropertyName("text")]
    public string? Text { get; set; }

    /// <summary>Media-fragment reference of the audio snippet, e.g. <c>chapter1.mp3#t=0,123</c>.</summary>
    [JsonPropertyName("audio")]
    public string? Audio { get; set; }

    /// <summary>Media-fragment reference of a video snippet.</summary>
    [JsonPropertyName("video")]
    public string? Video { get; set; }

    /// <summary>EPUB-type roles of the node, similar to the EPUB 3 <c>epub:type</c> semantics.</summary>
    [JsonConverter(typeof(StringOrArrayConverter))]
    [JsonPropertyName("role")]
    public List<string> Role { get; set; } = [];

    /// <summary>Ordered child nodes, similar to the SMIL <c>seq</c> element.</summary>
    [JsonPropertyName("narration")]
    public List<MediaOverlayNode> Narration { get; set; } = [];

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }

    /// <summary>Determines whether <see cref="Role"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Role"/> contains items.</returns>
    public bool ShouldSerializeRole()
    {
        return Role.Count > 0;
    }

    /// <summary>Determines whether <see cref="Narration"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Narration"/> contains items.</returns>
    public bool ShouldSerializeNarration()
    {
        return Narration.Count > 0;
    }
}