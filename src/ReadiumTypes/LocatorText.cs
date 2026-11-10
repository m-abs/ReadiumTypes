using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes;

/// <summary>Text surrounding the position of a <see cref="Locator"/>.</summary>
public record LocatorText : IExtensionDataHolder
{
    /// <summary>Text preceding the position.</summary>
    [JsonPropertyName("before")]
    public string? Before { get; set; }

    /// <summary>Text following the position.</summary>
    [JsonPropertyName("after")]
    public string? After { get; set; }

    /// <summary>Text at the position.</summary>
    [JsonPropertyName("content")]
    public LocalizedString? Content { get; set; }

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }
}