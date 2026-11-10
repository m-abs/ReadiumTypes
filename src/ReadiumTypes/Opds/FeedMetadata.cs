using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using Mabs.ReadiumTypes.Utils;

namespace Mabs.ReadiumTypes.Opds;

/// <summary>Metadata of an OPDS 2.0 feed.</summary>
public record FeedMetadata : IExtensionDataHolder
{
    /// <summary>Title of the feed.</summary>
    [JsonPropertyName("title")]
    public LocalizedString Title { get; set; } = new();

    /// <summary>Number of items in the feed.</summary>
    [JsonPropertyName("numberOfItems")]
    public int? NumberOfItems { get; set; }

    /// <summary>BCP 47 language tags of the feed's content.</summary>
    [JsonConverter(typeof(StringOrArrayConverter))]
    [JsonPropertyName("language")]
    public List<string> Language { get; set; } = [];

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }

    /// <summary>Determines whether <see cref="Language"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Language"/> contains items.</returns>
    public bool ShouldSerializeLanguage()
    {
        return Language.Count > 0;
    }
}