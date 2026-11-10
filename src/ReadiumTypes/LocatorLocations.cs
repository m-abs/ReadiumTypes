using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes;

/// <summary>Locations of a <see cref="Locator"/> using internal fragment identifier schemes.</summary>
public record LocatorLocations : IExtensionDataHolder
{
    /// <summary>Ordered fragments (css:..., xywh:..., page:..., etc.).</summary>
    [JsonPropertyName("fragments")]
    public List<string> Fragments { get; set; } = [];

    /// <summary>Progression in the resource, between 0 and 1.</summary>
    [JsonPropertyName("progression")]
    public double? Progression { get; set; }

    /// <summary>Index of the position in the publication, starting at 1.</summary>
    [JsonPropertyName("position")]
    public int? Position { get; set; }

    /// <summary>Progression in the publication, between 0 and 1.</summary>
    [JsonPropertyName("totalProgression")]
    public double? TotalProgression { get; set; }

    /// <summary>CSS selector of the position.</summary>
    [JsonPropertyName("cssSelector")]
    public string? CssSelector { get; set; }

    /// <summary>EPUB CFI of the position.</summary>
    [JsonPropertyName("partialCfi")]
    public string? PartialCfi { get; set; }

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }

    /// <summary>Determines whether <see cref="Fragments"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Fragments"/> contains items.</returns>
    public bool ShouldSerializeFragments()
    {
        return Fragments.Count > 0;
    }
}