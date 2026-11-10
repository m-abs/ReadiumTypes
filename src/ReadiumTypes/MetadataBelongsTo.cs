using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using Mabs.ReadiumTypes.Utils;

namespace Mabs.ReadiumTypes;

/// <summary>The entities the metadata of a publication describes.</summary>
public record MetadataBelongsTo : IExtensionDataHolder
{
    /// <summary>Publication the metadata describes.</summary>
    [JsonPropertyName("publication")]
    public string? Publication { get; set; }

    /// <summary>Series the metadata describes.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Series>))]
    [JsonPropertyName("series")]
    public List<Series> Series { get; set; } = [];

    /// <summary>Collections the metadata describes.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Collection>))]
    [JsonPropertyName("collection")]
    public List<Collection> Collection { get; set; } = [];

    /// <summary>Volumes the metadata describes.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Volume>))]
    [JsonPropertyName("volume")]
    public List<Volume> Volume { get; set; } = [];

    /// <summary>Seasons the metadata describes.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Season>))]
    [JsonPropertyName("season")]
    public List<Season> Season { get; set; } = [];

    /// <summary>Story arcs the metadata describes.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<StoryArc>))]
    [JsonPropertyName("storyArc")]
    public List<StoryArc> StoryArc { get; set; } = [];

    /// <summary>Periodicals the metadata describes.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Periodical>))]
    [JsonPropertyName("periodical")]
    public List<Periodical> Periodical { get; set; } = [];

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }

    /// <summary>Determines whether <see cref="Series"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Series"/> contains items.</returns>
    public bool ShouldSerializeSeries()
    {
        return Series.Count > 0;
    }

    /// <summary>Determines whether <see cref="Collection"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Collection"/> contains items.</returns>
    public bool ShouldSerializeCollection()
    {
        return Collection.Count > 0;
    }

    /// <summary>Determines whether <see cref="Volume"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Volume"/> contains items.</returns>
    public bool ShouldSerializeVolume()
    {
        return Volume.Count > 0;
    }

    /// <summary>Determines whether <see cref="Season"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Season"/> contains items.</returns>
    public bool ShouldSerializeSeason()
    {
        return Season.Count > 0;
    }

    /// <summary>Determines whether <see cref="StoryArc"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="StoryArc"/> contains items.</returns>
    public bool ShouldSerializeStoryArc()
    {
        return StoryArc.Count > 0;
    }

    /// <summary>Determines whether <see cref="Periodical"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Periodical"/> contains items.</returns>
    public bool ShouldSerializePeriodical()
    {
        return Periodical.Count > 0;
    }
}