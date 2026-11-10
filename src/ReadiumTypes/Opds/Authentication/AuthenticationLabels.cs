using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes.Opds.Authentication;

/// <summary>Labels a client should display for authentication credentials.</summary>
public record AuthenticationLabels : IExtensionDataHolder
{
    /// <summary>Label for the username field.</summary>
    [JsonPropertyName("username")]
    public string? Username { get; set; }

    /// <summary>Label for the password field.</summary>
    [JsonPropertyName("password")]
    public string? Password { get; set; }

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }
}