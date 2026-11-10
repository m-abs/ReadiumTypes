using System.Collections.Generic;
using System.Text.Json.Serialization;

using Mabs.ReadiumTypes.Utils;

namespace Mabs.ReadiumTypes;

/// <summary>A periodical (journal, magazine, newspaper) a publication belongs to.</summary>
public record Periodical : CollectionObject
{
    /// <summary>Issues of the periodical.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Issue>))]
    [JsonPropertyName("issue")]
    public List<Issue> Issue { get; set; } = [];

    /// <summary>Volumes of the periodical.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Volume>))]
    [JsonPropertyName("volume")]
    public List<Volume> Volume { get; set; } = [];

    /// <summary>Determines whether <see cref="Issue"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Issue"/> contains items.</returns>
    public bool ShouldSerializeIssue()
    {
        return Issue.Count > 0;
    }

    /// <summary>Determines whether <see cref="Volume"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Volume"/> contains items.</returns>
    public bool ShouldSerializeVolume()
    {
        return Volume.Count > 0;
    }
}