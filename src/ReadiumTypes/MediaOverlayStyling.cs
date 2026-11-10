using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes;

/// <summary>
/// Media overlay styling of a publication, under the kebab-case <c>media-overlay</c>
/// metadata key. <b>Non-standard:</b> this key is not defined by the webpub-manifest
/// schemas or any profile; it is emitted by Thorium (
/// <see href="https://github.com/edrlab/thorium-reader/blob/03d89b20c894273f78f210d593c75e869dc2ec82/src/r2-xxx-js/r2-shared-js/models/metadata-media-overlay.ts">metadata-media-overlay.ts</see>)
/// and modeled here only to round-trip its manifests. The standard key is the
/// EPUB profile's camelCase <c>mediaOverlay</c>, see <see cref="EpubMediaOverlay"/>.
/// The overlay documents themselves are separate resources, referenced by
/// <see cref="Link.Alternate"/> links of type <see cref="MediaTypes.SyncNarrJson"/>.
/// </summary>
public record MediaOverlayStyling : IExtensionDataHolder
{
    /// <summary>CSS class applied to the currently active media overlay element.</summary>
    [JsonPropertyName("active-class")]
    public string? ActiveClass { get; set; }

    /// <summary>CSS class applied while media overlay playback is active.</summary>
    [JsonPropertyName("playback-active-class")]
    public string? PlaybackActiveClass { get; set; }

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }
}