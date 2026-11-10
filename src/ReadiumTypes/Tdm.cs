using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes;

/// <summary>Text and data mining reservation policy of a publication.</summary>
public record Tdm : IExtensionDataHolder
{
    /// <summary>Policy expressed as plain text.</summary>
    [JsonPropertyName("reserved")]
    public TdmReservation Reserved { get; set; } = TdmReservation.All;

    /// <summary>URI of the machine-readable policy.</summary>
    [JsonPropertyName("policy")]
    public Uri? Policy { get; set; }

    /// <summary>Date at which the policy takes effect.</summary>
    [JsonPropertyName("effective")]
    public DateTimeOffset? Effective { get; set; }

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }
}