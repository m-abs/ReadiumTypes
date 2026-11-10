using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes;

/// <summary>An accessibility certification of a publication.</summary>
public record AccessibilityCertification : IExtensionDataHolder
{
    /// <summary>Organization that certified the publication.</summary>
    [JsonPropertyName("certifiedBy")]
    public string? CertifiedBy { get; set; }

    /// <summary>URI of the credential granted.</summary>
    [JsonPropertyName("credential")]
    public string? Credential { get; set; }

    /// <summary>URL of the report backing the certification.</summary>
    [JsonPropertyName("report")]
    public string? Report { get; set; }

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }
}