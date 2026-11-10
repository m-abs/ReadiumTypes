using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes;

/// <summary>Information needed to decrypt or deobfuscate an encrypted link target.</summary>
public record EncryptedProperties : IExtensionDataHolder
{
    /// <summary>URI identifying the algorithm used to encrypt the resource.</summary>
    [JsonPropertyName("algorithm")]
    public string Algorithm { get; set; } = string.Empty;

    /// <summary>Compression method used on the resource.</summary>
    [JsonPropertyName("compression")]
    public string? Compression { get; set; }

    /// <summary>Original length of the resource in bytes before compression and/or encryption.</summary>
    [JsonPropertyName("originalLength")]
    public int? OriginalLength { get; set; }

    /// <summary>URI identifying the encryption profile used to encrypt the resource.</summary>
    [JsonPropertyName("profile")]
    public string? Profile { get; set; }

    /// <summary>URI identifying the encryption scheme used to encrypt the resource.</summary>
    [JsonPropertyName("scheme")]
    public string? Scheme { get; set; }

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }
}