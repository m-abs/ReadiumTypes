using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using Mabs.ReadiumTypes.Utils;

namespace Mabs.ReadiumTypes;

/// <summary>A link to a resource in a publication or collection.</summary>
public record Link : IExtensionDataHolder
{
    /// <summary>URI or URI template of the linked resource.</summary>
    [JsonPropertyName("href")]
    public string Href { get; set; } = string.Empty;

    /// <summary>MIME type of the linked resource.</summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>Title of the linked resource.</summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>Indicates that a URI template is used in <see cref="Href"/>.</summary>
    [JsonPropertyName("templated")]
    public bool? Templated { get; set; }

    /// <summary>Relations between the linked resource and its containing collection.</summary>
    [JsonConverter(typeof(StringOrArrayConverter))]
    [JsonPropertyName("rel")]
    public List<string> Rel { get; set; } = [];

    /// <summary>Properties associated with the linked resource.</summary>
    [JsonPropertyName("properties")]
    public LinkProperties? Properties { get; set; }

    /// <summary>Height of the linked resource in pixels.</summary>
    [JsonPropertyName("height")]
    public int? Height { get; set; }

    /// <summary>Width of the linked resource in pixels.</summary>
    [JsonPropertyName("width")]
    public int? Width { get; set; }

    /// <summary>Original size of the resource in bytes, prior to encryption or compression.</summary>
    [JsonPropertyName("size")]
    public int? Size { get; set; }

    /// <summary>Bitrate of the linked resource in kbps.</summary>
    [JsonPropertyName("bitrate")]
    public double? Bitrate { get; set; }

    /// <summary>Length of the linked resource in seconds.</summary>
    [JsonPropertyName("duration")]
    public double? Duration { get; set; }

    /// <summary>Expected languages of the linked resource as BCP 47 tags.</summary>
    [JsonConverter(typeof(StringOrArrayConverter))]
    [JsonPropertyName("language")]
    public List<string> Language { get; set; } = [];

    /// <summary>Alternate resources for the linked resource.</summary>
    [JsonPropertyName("alternate")]
    public List<Link> Alternate { get; set; } = [];

    /// <summary>Resources that are children of the linked resource in a given collection role.</summary>
    [JsonPropertyName("children")]
    public List<Link> Children { get; set; } = [];

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }

    /// <summary>Determines whether <see cref="Rel"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Rel"/> contains items.</returns>
    public bool ShouldSerializeRel()
    {
        return Rel.Count > 0;
    }

    /// <summary>Determines whether <see cref="Language"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Language"/> contains items.</returns>
    public bool ShouldSerializeLanguage()
    {
        return Language.Count > 0;
    }

    /// <summary>Determines whether <see cref="Alternate"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Alternate"/> contains items.</returns>
    public bool ShouldSerializeAlternate()
    {
        return Alternate.Count > 0;
    }

    /// <summary>Determines whether <see cref="Children"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Children"/> contains items.</returns>
    public bool ShouldSerializeChildren()
    {
        return Children.Count > 0;
    }
}