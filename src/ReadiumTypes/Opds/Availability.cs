using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes.Opds;

/// <summary>Availability of a publication or acquisition link.</summary>
public record Availability : IExtensionDataHolder
{
    /// <summary>Availability state.</summary>
    [JsonPropertyName("state")]
    public AvailabilityState State { get; set; }

    /// <summary>Date at which the current availability ends.</summary>
    [JsonPropertyName("latest")]
    public DateTimeOffset? Latest { get; set; }

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }
}