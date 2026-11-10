using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes.Opds;

/// <summary>An OPDS 2.0 publication in a feed.</summary>
public record OpdsPublication : IExtensionDataHolder
{
    /// <summary>Metadata of the publication.</summary>
    [JsonPropertyName("metadata")]
    public PublicationMetadata Metadata { get; set; } = new();

    /// <summary>Links of the publication, including at least one acquisition link.</summary>
    [JsonPropertyName("links")]
    public List<Link> Links { get; set; } = [];

    /// <summary>Images meant to be displayed when browsing publications.</summary>
    [JsonPropertyName("images")]
    public List<Link> Images { get; set; } = [];

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }

    /// <summary>Determines whether <see cref="Images"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Images"/> contains items.</returns>
    public bool ShouldSerializeImages()
    {
        return Images.Count > 0;
    }
}