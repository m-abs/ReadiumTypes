using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes.Opds.Progression;

/// <summary>A reading position on one device in a progression document.</summary>
public record ProgressionDevice : IExtensionDataHolder
{
    /// <summary>Identifier of the device.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>Type of the device.</summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>Date of the device's last activity.</summary>
    [JsonPropertyName("lastActivity")]
    public string? LastActivity { get; set; }

    /// <summary>Reading position on the device.</summary>
    [JsonPropertyName("locator")]
    public Locator? Locator { get; set; }

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }
}