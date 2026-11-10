using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes;

/// <summary>
/// List of the positions of a publication, served at a manifest link with
/// <see cref="MediaTypes.PositionListJson"/> and <see cref="Rel.PositionList"/>.
/// </summary>
public record PositionList : IExtensionDataHolder
{
    /// <summary>Total number of positions in the publication.</summary>
    [JsonPropertyName("total")]
    public int Total { get; set; }

    /// <summary>Locators of the publication's positions, in reading order.</summary>
    [JsonPropertyName("positions")]
    public List<Locator> Positions { get; set; } = [];

    /// <summary>Wire properties not described by the specification.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }

    /// <summary>Determines whether <see cref="Positions"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Positions"/> contains items.</returns>
    public bool ShouldSerializePositions()
    {
        return Positions.Count > 0;
    }
}