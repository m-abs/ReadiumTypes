using System.Collections.Generic;
using System.Text.Json.Serialization;

using Mabs.ReadiumTypes.Utils;

namespace Mabs.ReadiumTypes;

/// <summary>A series a publication belongs to or contains.</summary>
public record Series : CollectionObject
{
    /// <summary>Chapters of the series.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Chapter>))]
    [JsonPropertyName("chapter")]
    public List<Chapter> Chapter { get; set; } = [];

    /// <summary>Episodes of the series.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Episode>))]
    [JsonPropertyName("episode")]
    public List<Episode> Episode { get; set; } = [];

    /// <summary>Issues of the series.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Issue>))]
    [JsonPropertyName("issue")]
    public List<Issue> Issue { get; set; } = [];

    /// <summary>Seasons of the series.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Season>))]
    [JsonPropertyName("season")]
    public List<Season> Season { get; set; } = [];

    /// <summary>Story arcs of the series.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<StoryArc>))]
    [JsonPropertyName("storyArc")]
    public List<StoryArc> StoryArc { get; set; } = [];

    /// <summary>Volumes of the series.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Volume>))]
    [JsonPropertyName("volume")]
    public List<Volume> Volume { get; set; } = [];

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

    /// <summary>Determines whether <see cref="Volume"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Volume"/> contains items.</returns>
    public bool ShouldSerializeVolume()
    {
        return Volume.Count > 0;
    }
}