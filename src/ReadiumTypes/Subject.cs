using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes;

/// <summary>A subject of a publication.</summary>
public record Subject : IExtensionDataHolder
{
    /// <summary>Name of the subject.</summary>
    [JsonPropertyName("name")]
    public LocalizedString Name { get; set; } = new();

    /// <summary>String used to sort the subject by name.</summary>
    [JsonPropertyName("sortAs")]
    public LocalizedString? SortAs { get; set; }

    /// <summary>Code identifying the subject in its scheme.</summary>
    [JsonPropertyName("code")]
    public string? Code { get; set; }

    /// <summary>URI identifying the scheme the code comes from.</summary>
    [JsonPropertyName("scheme")]
    public string? Scheme { get; set; }

    /// <summary>Links to resources describing the subject.</summary>
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