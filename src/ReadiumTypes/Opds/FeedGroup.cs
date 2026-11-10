using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes.Opds;

/// <summary>A group of related navigation links or sub-feeds in a feed.</summary>
public record FeedGroup : IExtensionDataHolder
{
    /// <summary>Identifier of the group.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>Title of the group.</summary>
    [JsonPropertyName("title")]
    public LocalizedString Title { get; set; } = new();

    /// <summary>Links of the group.</summary>
    [JsonPropertyName("links")]
    public List<Link> Links { get; set; } = [];

    /// <summary>Navigation links of the group.</summary>
    [JsonPropertyName("navigation")]
    public List<Link> Navigation { get; set; } = [];

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }

    /// <summary>Determines whether <see cref="Links"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Links"/> contains items.</returns>
    public bool ShouldSerializeLinks()
    {
        return Links.Count > 0;
    }

    /// <summary>Determines whether <see cref="Navigation"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Navigation"/> contains items.</returns>
    public bool ShouldSerializeNavigation()
    {
        return Navigation.Count > 0;
    }
}