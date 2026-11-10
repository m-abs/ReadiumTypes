using System.Collections.Generic;
using System.Text.Json.Serialization;

using Mabs.ReadiumTypes.Utils;

namespace Mabs.ReadiumTypes;

/// <summary>A chapter a publication contains.</summary>
public record Chapter : CollectionObject
{
    /// <summary>Series the chapter belongs to.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Series>))]
    [JsonPropertyName("series")]
    public List<Series> Series { get; set; } = [];

    /// <summary>Determines whether <see cref="Series"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Series"/> contains items.</returns>
    public bool ShouldSerializeSeries()
    {
        return Series.Count > 0;
    }
}