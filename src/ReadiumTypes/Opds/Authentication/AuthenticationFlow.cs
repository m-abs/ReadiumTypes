using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes.Opds.Authentication;

/// <summary>One authentication flow offered by an OPDS authentication document.</summary>
public record AuthenticationFlow : IExtensionDataHolder
{
    /// <summary>URI identifying the authentication type.</summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>Links implementing the flow.</summary>
    [JsonPropertyName("links")]
    public List<Link> Links { get; set; } = [];

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }

    /// <summary>Determines whether <see cref="Links"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Links"/> contains items.</returns>
    public bool ShouldSerializeLinks()
    {
        return Links.Count > 0;
    }
}