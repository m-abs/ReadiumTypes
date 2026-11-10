using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes.GuidedNavigation;

/// <summary>Description of a guided navigation document.</summary>
public record GuidedNavigationDescription : IExtensionDataHolder
{
    /// <summary>Text of the description.</summary>
    [JsonPropertyName("text")]
    public NavigationText? Text { get; set; }

    /// <summary>URI of an audio rendition of the description.</summary>
    [JsonPropertyName("audio")]
    public string? Audio { get; set; }

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }
}