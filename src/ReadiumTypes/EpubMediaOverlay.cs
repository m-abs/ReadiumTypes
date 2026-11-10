using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes;

/// <summary>
/// EPUB profile media overlay metadata, the standard camelCase <c>mediaOverlay</c>
/// metadata key of the EPUB metadata extension — not the non-standard kebab-case
/// styling key, see <see cref="MediaOverlayStyling"/>. Defined in
/// <see href="https://readium.org/webpub-manifest/profiles/epub#mediaoverlay">the EPUB profile</see>
/// and its
/// <see href="https://github.com/readium/webpub-manifest/blob/master/schema/extensions/epub/metadata.schema.json">JSON Schema</see>.
/// </summary>
public record EpubMediaOverlay : IExtensionDataHolder
{
    /// <summary>Author-defined CSS class applied to the currently-playing content document element.</summary>
    [JsonPropertyName("activeClass")]
    public string? ActiveClass { get; set; }

    /// <summary>Author-defined CSS class applied to the document element of the content document while playback is active.</summary>
    [JsonPropertyName("playbackActiveClass")]
    public string? PlaybackActiveClass { get; set; }

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }
}