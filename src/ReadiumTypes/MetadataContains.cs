using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using Mabs.ReadiumTypes.Utils;

namespace Mabs.ReadiumTypes;

/// <summary>The entities the metadata of a publication contains.</summary>
public record MetadataContains : IExtensionDataHolder
{
    /// <summary>Series contained in the publication.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Series>))]
    [JsonPropertyName("series")]
    public List<Series> Series { get; set; } = [];

    /// <summary>Collections contained in the publication.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Collection>))]
    [JsonPropertyName("collection")]
    public List<Collection> Collection { get; set; } = [];

    /// <summary>Volumes contained in the publication.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Volume>))]
    [JsonPropertyName("volume")]
    public List<Volume> Volume { get; set; } = [];

    /// <summary>Seasons contained in the publication.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Season>))]
    [JsonPropertyName("season")]
    public List<Season> Season { get; set; } = [];

    /// <summary>Story arcs contained in the publication.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<StoryArc>))]
    [JsonPropertyName("storyArc")]
    public List<StoryArc> StoryArc { get; set; } = [];

    /// <summary>Periodicals contained in the publication.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Periodical>))]
    [JsonPropertyName("periodical")]
    public List<Periodical> Periodical { get; set; } = [];

    /// <summary>Articles contained in the publication.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Article>))]
    [JsonPropertyName("article")]
    public List<Article> Article { get; set; } = [];

    /// <summary>Chapters contained in the publication.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Chapter>))]
    [JsonPropertyName("chapter")]
    public List<Chapter> Chapter { get; set; } = [];

    /// <summary>Episodes contained in the publication.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Episode>))]
    [JsonPropertyName("episode")]
    public List<Episode> Episode { get; set; } = [];

    /// <summary>Issues contained in the publication.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Issue>))]
    [JsonPropertyName("issue")]
    public List<Issue> Issue { get; set; } = [];

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

    /// <summary>Determines whether <see cref="Article"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Article"/> contains items.</returns>
    public bool ShouldSerializeArticle()
    {
        return Article.Count > 0;
    }

    /// <summary>Determines whether <see cref="Chapter"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Chapter"/> contains items.</returns>
    public bool ShouldSerializeChapter()
    {
        return Chapter.Count > 0;
    }

    /// <summary>Determines whether <see cref="Episode"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Episode"/> contains items.</returns>
    public bool ShouldSerializeEpisode()
    {
        return Episode.Count > 0;
    }

    /// <summary>Determines whether <see cref="Issue"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Issue"/> contains items.</returns>
    public bool ShouldSerializeIssue()
    {
        return Issue.Count > 0;
    }
}