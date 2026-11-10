using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using Mabs.ReadiumTypes.Utils;

namespace Mabs.ReadiumTypes.Opds.Progression;

/// <summary>An OPDS 2.0 progression document listing reading positions per device.</summary>
public record ProgressionDocument : IExtensionDataHolder
{
    /// <summary>JSON-LD context URIs of the document.</summary>
    [JsonConverter(typeof(StringOrArrayConverter))]
    [JsonPropertyName("@context")]
    public List<string> Context { get; set; } = [];

    /// <summary>Identifier of the document.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>Title of the document.</summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>Links of the document.</summary>
    [JsonPropertyName("links")]
    public List<Link> Links { get; set; } = [];

    /// <summary>Reading positions, one per device.</summary>
    [JsonPropertyName("devices")]
    public List<ProgressionDevice> Devices { get; set; } = [];

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }

    /// <summary>Determines whether <see cref="Context"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Context"/> contains items.</returns>
    public bool ShouldSerializeContext()
    {
        return Context.Count > 0;
    }

    /// <summary>Determines whether <see cref="Links"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Links"/> contains items.</returns>
    public bool ShouldSerializeLinks()
    {
        return Links.Count > 0;
    }

    /// <summary>Determines whether <see cref="Devices"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Devices"/> contains items.</returns>
    public bool ShouldSerializeDevices()
    {
        return Devices.Count > 0;
    }
}