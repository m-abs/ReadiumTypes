using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using Mabs.ReadiumTypes.Utils;

namespace Mabs.ReadiumTypes.Opds;

/// <summary>An OPDS 2.0 feed.</summary>
public record Feed : IExtensionDataHolder
{
    /// <summary>JSON-LD context URIs of the feed.</summary>
    [JsonConverter(typeof(StringOrArrayConverter))]
    [JsonPropertyName("@context")]
    public List<string> Context { get; set; } = [];

    /// <summary>Metadata of the feed.</summary>
    [JsonPropertyName("metadata")]
    public FeedMetadata Metadata { get; set; } = new();

    /// <summary>Links of the feed.</summary>
    [JsonPropertyName("links")]
    public List<Link> Links { get; set; } = [];

    /// <summary>Publication entries of the feed.</summary>
    [JsonPropertyName("items")]
    public List<OpdsPublication> Items { get; set; } = [];

    /// <summary>Groups of the feed.</summary>
    [JsonPropertyName("groups")]
    public List<FeedGroup> Groups { get; set; } = [];

    /// <summary>Total number of items matching the feed query.</summary>
    [JsonPropertyName("totalItems")]
    public int? TotalItems { get; set; }

    /// <summary>Link to the next page of the feed.</summary>
    [JsonPropertyName("next")]
    public Link? Next { get; set; }

    /// <summary>Link to the previous page of the feed.</summary>
    [JsonPropertyName("prev")]
    public Link? Prev { get; set; }

    /// <summary>Link to the first page of the feed.</summary>
    [JsonPropertyName("first")]
    public Link? First { get; set; }

    /// <summary>Link to the last page of the feed.</summary>
    [JsonPropertyName("last")]
    public Link? Last { get; set; }

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }

    /// <summary>Determines whether <see cref="Context"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Context"/> contains items.</returns>
    public bool ShouldSerializeContext()
    {
        return Context.Count > 0;
    }

    /// <summary>Determines whether <see cref="Items"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Items"/> contains items.</returns>
    public bool ShouldSerializeItems()
    {
        return Items.Count > 0;
    }

    /// <summary>Determines whether <see cref="Groups"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Groups"/> contains items.</returns>
    public bool ShouldSerializeGroups()
    {
        return Groups.Count > 0;
    }
}