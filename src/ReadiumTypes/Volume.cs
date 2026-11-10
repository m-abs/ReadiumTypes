using System.Collections.Generic;
using System.Text.Json.Serialization;

using Mabs.ReadiumTypes.Utils;

namespace Mabs.ReadiumTypes;

/// <summary>A volume a publication belongs to or contains.</summary>
public record Volume : CollectionObject
{
    /// <summary>Chapters of the volume.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Chapter>))]
    [JsonPropertyName("chapter")]
    public List<Chapter> Chapter { get; set; } = [];

    /// <summary>Issues of the volume.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Issue>))]
    [JsonPropertyName("issue")]
    public List<Issue> Issue { get; set; } = [];

    /// <summary>Story arcs of the volume.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<StoryArc>))]
    [JsonPropertyName("storyArc")]
    public List<StoryArc> StoryArc { get; set; } = [];

    /// <summary>Determines whether <see cref="Chapter"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Chapter"/> contains items.</returns>
    public bool ShouldSerializeChapter()
    {
        return Chapter.Count > 0;
    }

    /// <summary>Determines whether <see cref="Issue"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Issue"/> contains items.</returns>
    public bool ShouldSerializeIssue()
    {
        return Issue.Count > 0;
    }

    /// <summary>Determines whether <see cref="StoryArc"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="StoryArc"/> contains items.</returns>
    public bool ShouldSerializeStoryArc()
    {
        return StoryArc.Count > 0;
    }
}