using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using Mabs.ReadiumTypes.Utils;

namespace Mabs.ReadiumTypes.GuidedNavigation;

/// <summary>A single item in a guided navigation document.</summary>
public record GuidedNavigationItem : IExtensionDataHolder
{
    /// <summary>Identifier of the item.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>Class of the item (e.g. chapter, section, page).</summary>
    [JsonPropertyName("class")]
    public string? Class { get; set; }

    /// <summary>Title of the item.</summary>
    [JsonPropertyName("title")]
    public LocalizedString? Title { get; set; }

    /// <summary>Description of the item.</summary>
    [JsonPropertyName("description")]
    public NavigationText? Description { get; set; }

    /// <summary>Relation of the item to the document.</summary>
    [JsonPropertyName("rel")]
    public string? Rel { get; set; }

    /// <summary>URI of the item.</summary>
    [JsonPropertyName("href")]
    public string? Href { get; set; }

    /// <summary>MIME type of the item.</summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>Roles of the item.</summary>
    [JsonConverter(typeof(StringOrArrayConverter))]
    [JsonPropertyName("role")]
    public List<string> Roles { get; set; } = [];

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }

    /// <summary>Determines whether <see cref="Roles"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Roles"/> contains items.</returns>
    public bool ShouldSerializeRoles()
    {
        return Roles.Count > 0;
    }
}