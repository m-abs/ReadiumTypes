using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using Mabs.ReadiumTypes.Utils;

namespace Mabs.ReadiumTypes;

/// <summary>A web publication: metadata plus the links to its resources.</summary>
public record Publication : IExtensionDataHolder
{
    /// <summary>JSON-LD context URIs of the publication.</summary>
    [JsonConverter(typeof(StringOrArrayConverter))]
    [JsonPropertyName("@context")]
    public List<string> Context { get; set; } = [];

    /// <summary>Metadata of the publication.</summary>
    [JsonPropertyName("metadata")]
    public PublicationMetadata Metadata { get; set; } = new();

    /// <summary>Links to external resources of the publication.</summary>
    [JsonPropertyName("links")]
    public List<Link> Links { get; set; } = [];

    /// <summary>Ordered links to the resources that make up the reading order.</summary>
    [JsonPropertyName("readingOrder")]
    public List<Link> ReadingOrder { get; set; } = [];

    /// <summary>Ordered links to auxiliary resources of the publication.</summary>
    [JsonPropertyName("resources")]
    public List<Link> Resources { get; set; } = [];

    /// <summary>Links to cover images of the publication.</summary>
    [JsonPropertyName("covers")]
    public List<Link> Covers { get; set; } = [];

    /// <summary>How the publication should be displayed in a synthetic spread.</summary>
    [JsonPropertyName("layout")]
    public Layout? Layout { get; set; }

    /// <summary>Reading progression of the publication.</summary>
    [JsonPropertyName("readingProgression")]
    public ReadingProgression? ReadingProgression { get; set; }

    /// <summary>Sub-publications of the publication, keyed by identifier.</summary>
    [JsonPropertyName("subcollections")]
    public Dictionary<string, Publication>? Subcollections { get; set; }

    /// <summary>Text and data mining reservation policy of the publication.</summary>
    [JsonPropertyName("tdm")]
    public Tdm? Tdm { get; set; }

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }

    /// <summary>Determines whether <see cref="Links"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Links"/> contains items.</returns>
    public bool ShouldSerializeLinks()
    {
        return Links.Count > 0;
    }

    /// <summary>Determines whether <see cref="Context"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Context"/> contains items.</returns>
    public bool ShouldSerializeContext()
    {
        return Context.Count > 0;
    }

    /// <summary>Determines whether <see cref="Resources"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Resources"/> contains items.</returns>
    public bool ShouldSerializeResources()
    {
        return Resources.Count > 0;
    }

    /// <summary>Determines whether <see cref="Covers"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Covers"/> contains items.</returns>
    public bool ShouldSerializeCovers()
    {
        return Covers.Count > 0;
    }
}