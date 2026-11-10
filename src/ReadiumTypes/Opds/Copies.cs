using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes.Opds;

/// <summary>Information about the copies a library has acquired.</summary>
public record Copies : IExtensionDataHolder
{
    /// <summary>Total number of copies.</summary>
    [JsonPropertyName("total")]
    public int Total { get; set; }

    /// <summary>Number of copies currently available.</summary>
    [JsonPropertyName("available")]
    public int? Available { get; set; }

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }
}