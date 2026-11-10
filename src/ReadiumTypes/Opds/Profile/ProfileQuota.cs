using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes.Opds.Profile;

/// <summary>Quota information in an OPDS profile document.</summary>
public record ProfileQuota : IExtensionDataHolder
{
    /// <summary>Number of remaining updates allowed.</summary>
    [JsonPropertyName("updates")]
    public int? Updates { get; set; }

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }
}