using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes.Opds;

/// <summary>Hold-list information for an unavailable book.</summary>
public record Holds : IExtensionDataHolder
{
    /// <summary>Total number of holds.</summary>
    [JsonPropertyName("total")]
    public int Total { get; set; }

    /// <summary>Number of patrons with a reserved hold.</summary>
    [JsonPropertyName("reserved")]
    public int? Reserved { get; set; }

    /// <summary>Number of patrons with a hold on one of several copies.</summary>
    [JsonPropertyName("withOneCopy")]
    public int? WithOneCopy { get; set; }

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }
}