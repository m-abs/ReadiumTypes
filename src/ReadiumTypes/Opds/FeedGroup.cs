using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes.Opds;

/// <summary>
/// A group of related publications or navigation links in a feed; also the wire shape of a
/// facet group, whose <see cref="Publications"/> and <see cref="Navigation"/> stay empty.
/// </summary>
public record FeedGroup : IExtensionDataHolder
{
    /// <summary>Metadata of the group.</summary>
    [JsonPropertyName("metadata")]
    public FeedMetadata Metadata { get; set; } = new();

    /// <summary>Links of the group.</summary>
    [JsonPropertyName("links")]
    public List<Link> Links { get; set; } = [];

    /// <summary>Publications of the group.</summary>
    [JsonPropertyName("publications")]
    public List<OpdsPublication> Publications { get; set; } = [];

    /// <summary>Navigation links of the group.</summary>
    [JsonPropertyName("navigation")]
    public List<Link> Navigation { get; set; } = [];

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }

    /// <summary>Determines whether <see cref="Publications"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Publications"/> contains items.</returns>
    public bool ShouldSerializePublications()
    {
        return Publications.Count > 0;
    }

    /// <summary>Determines whether <see cref="Navigation"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Navigation"/> contains items.</returns>
    public bool ShouldSerializeNavigation()
    {
        return Navigation.Count > 0;
    }
}