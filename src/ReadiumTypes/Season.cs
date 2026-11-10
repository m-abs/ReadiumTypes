using System.Collections.Generic;
using System.Text.Json.Serialization;

using Mabs.ReadiumTypes.Utils;

namespace Mabs.ReadiumTypes;

/// <summary>A season a publication belongs to or contains.</summary>
public record Season : CollectionObject
{
    /// <summary>Episodes of the season.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Episode>))]
    [JsonPropertyName("episode")]
    public List<Episode> Episode { get; set; } = [];

    /// <summary>Determines whether <see cref="Episode"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Episode"/> contains items.</returns>
    public bool ShouldSerializeEpisode()
    {
        return Episode.Count > 0;
    }
}