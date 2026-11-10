using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using Mabs.ReadiumTypes.Utils;

namespace Mabs.ReadiumTypes;

/// <summary>A person or organization that contributed to a publication.</summary>
public record Contributor : IExtensionDataHolder
{
    /// <summary>Name of the contributor.</summary>
    [JsonPropertyName("name")]
    public LocalizedString Name { get; set; } = new();

    /// <summary>URI identifier of the contributor.</summary>
    [JsonPropertyName("identifier")]
    public string? Identifier { get; set; }

    /// <summary>Alternate identifiers of the contributor.</summary>
    [JsonConverter(typeof(AltIdentifierConverter))]
    [JsonPropertyName("altIdentifier")]
    public List<AltIdentifier> AltIdentifier { get; set; } = [];

    /// <summary>String used to sort the contributor by name.</summary>
    [JsonPropertyName("sortAs")]
    public LocalizedString? SortAs { get; set; }

    /// <summary>Roles the contributor played.</summary>
    [JsonConverter(typeof(StringOrArrayConverter))]
    [JsonPropertyName("role")]
    public List<string> Role { get; set; } = [];

    /// <summary>Links to resources describing the contributor.</summary>
    [JsonPropertyName("links")]
    public List<Link> Links { get; set; } = [];

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }

    /// <summary>Determines whether <see cref="AltIdentifier"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="AltIdentifier"/> contains items.</returns>
    public bool ShouldSerializeAltIdentifier()
    {
        return AltIdentifier.Count > 0;
    }

    /// <summary>Determines whether <see cref="Role"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Role"/> contains items.</returns>
    public bool ShouldSerializeRole()
    {
        return Role.Count > 0;
    }

    /// <summary>Determines whether <see cref="Links"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Links"/> contains items.</returns>
    public bool ShouldSerializeLinks()
    {
        return Links.Count > 0;
    }
}