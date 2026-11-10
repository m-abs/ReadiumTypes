using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using Mabs.ReadiumTypes.Utils;

namespace Mabs.ReadiumTypes;

/// <summary>
/// Base record for the webpub collection objects (series, collection, volume, season, story arc,
/// article, chapter, episode, issue, periodical) sharing name, identifiers, sort string,
/// position, and links.
/// </summary>
public record CollectionObject : IExtensionDataHolder
{
    /// <summary>Name of the collection object.</summary>
    [JsonPropertyName("name")]
    public LocalizedString? Name { get; set; }

    /// <summary>URI identifier of the collection object.</summary>
    [JsonPropertyName("identifier")]
    public string? Identifier { get; set; }

    /// <summary>Alternate identifiers of the collection object.</summary>
    [JsonConverter(typeof(AltIdentifierConverter))]
    [JsonPropertyName("altIdentifier")]
    public List<AltIdentifier> AltIdentifier { get; set; } = [];

    /// <summary>String used to sort the collection object by name.</summary>
    [JsonPropertyName("sortAs")]
    public LocalizedString? SortAs { get; set; }

    /// <summary>Position of the publication within the collection object.</summary>
    [JsonPropertyName("position")]
    public double? Position { get; set; }

    /// <summary>Links to resources describing the collection object.</summary>
    [JsonPropertyName("links")]
    public List<Link> Links { get; set; } = [];

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }

    /// <summary>Determines whether <see cref="AltIdentifier"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="AltIdentifier"/> contains items.</returns>
    public bool ShouldSerializeAltIdentifier()
    {
        return AltIdentifier.Count > 0;
    }

    /// <summary>Determines whether <see cref="Links"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Links"/> contains items.</returns>
    public bool ShouldSerializeLinks()
    {
        return Links.Count > 0;
    }
}