using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes;

/// <summary>A physical position in a publication resource.</summary>
public record Locator : IExtensionDataHolder
{
    /// <summary>URI uniquely identifying the resource.</summary>
    [JsonPropertyName("href")]
    public string Href { get; set; } = string.Empty;

    /// <summary>Media type of the resource.</summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>Title of the resource or position.</summary>
    [JsonPropertyName("title")]
    public LocalizedString? Title { get; set; }

    /// <summary>BCP 47 language tag of the resource.</summary>
    [JsonPropertyName("language")]
    public string? Language { get; set; }

    /// <summary>Location in the resource using a range of terms from internal fragment schemes.</summary>
    [JsonPropertyName("locations")]
    public LocatorLocations? Locations { get; set; }

    /// <summary>Text surrounding the position.</summary>
    [JsonPropertyName("text")]
    public LocatorText? Text { get; set; }

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }
}