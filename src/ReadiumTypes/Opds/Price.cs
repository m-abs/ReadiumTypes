using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes.Opds;

/// <summary>Price of a publication tied to an acquisition link.</summary>
public record Price : IExtensionDataHolder
{
    /// <summary>Price value.</summary>
    [JsonPropertyName("value")]
    public decimal Value { get; set; }

    /// <summary>ISO 4217 currency code.</summary>
    [JsonPropertyName("currency")]
    public string Currency { get; set; } = string.Empty;

    /// <summary>Price value in the smallest currency unit (e.g. cents).</summary>
    [JsonPropertyName("valueInteger")]
    public int? ValueInteger { get; set; }

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }
}