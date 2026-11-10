using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes.Opds;

/// <summary>An OPDS 2.0 feed.</summary>
public record Feed : IExtensionDataHolder
{
    /// <summary>Feed-level metadata such as title or number of items.</summary>
    [JsonPropertyName("metadata")]
    public FeedMetadata Metadata { get; set; } = new();

    /// <summary>Feed-level links such as search or pagination.</summary>
    [JsonPropertyName("links")]
    public List<Link> Links { get; set; } = [];

    /// <summary>A list of publications that can be acquired.</summary>
    [JsonPropertyName("publications")]
    public List<OpdsPublication> Publications { get; set; } = [];

    /// <summary>Navigation for the catalog using links.</summary>
    [JsonPropertyName("navigation")]
    public List<Link> Navigation { get; set; } = [];

    /// <summary>Facets to re-order or obtain a subset of the current list of publications.</summary>
    [JsonPropertyName("facets")]
    public List<FeedGroup> Facets { get; set; } = [];

    /// <summary>Groups providing a curated experience, grouping publications or navigation links.</summary>
    [JsonPropertyName("groups")]
    public List<FeedGroup> Groups { get; set; } = [];

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

    /// <summary>Determines whether <see cref="Facets"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Facets"/> contains items.</returns>
    public bool ShouldSerializeFacets()
    {
        return Facets.Count > 0;
    }

    /// <summary>Determines whether <see cref="Groups"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Groups"/> contains items.</returns>
    public bool ShouldSerializeGroups()
    {
        return Groups.Count > 0;
    }
}