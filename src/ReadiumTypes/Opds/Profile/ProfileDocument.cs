using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using Mabs.ReadiumTypes.Utils;

namespace Mabs.ReadiumTypes.Opds.Profile;

/// <summary>An OPDS 2.0 profile document describing a patron's account.</summary>
public record ProfileDocument : IExtensionDataHolder
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

    /// <summary>Description of the document.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>Links of the document.</summary>
    [JsonPropertyName("links")]
    public List<Link> Links { get; set; } = [];

    /// <summary>Relation the document advertises.</summary>
    [JsonPropertyName("rel")]
    public string? Rel { get; set; }

    /// <summary>Quota information of the account.</summary>
    [JsonPropertyName("quota")]
    public ProfileQuota? Quota { get; set; }

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
}