using System.Collections.Generic;
using System.Text.Json.Serialization;

using Mabs.ReadiumTypes.Utils;

namespace Mabs.ReadiumTypes;

/// <summary>A story arc a publication belongs to or contains.</summary>
public record StoryArc : CollectionObject
{
    /// <summary>Chapters of the story arc.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Chapter>))]
    [JsonPropertyName("chapter")]
    public List<Chapter> Chapter { get; set; } = [];

    /// <summary>Episodes of the story arc.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Episode>))]
    [JsonPropertyName("episode")]
    public List<Episode> Episode { get; set; } = [];

    /// <summary>Issues of the story arc.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Issue>))]
    [JsonPropertyName("issue")]
    public List<Issue> Issue { get; set; } = [];

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