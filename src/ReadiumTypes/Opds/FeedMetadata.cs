using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes.Opds;

/// <summary>Metadata of an OPDS 2.0 feed.</summary>
public record FeedMetadata : IExtensionDataHolder
{
    /// <summary>Identifier of the feed.</summary>
    [JsonPropertyName("identifier")]
    public string? Identifier { get; set; }

    /// <summary>RDF type of the feed.</summary>
    [JsonPropertyName("@type")]
    public string? Type { get; set; }

    /// <summary>Title of the feed.</summary>
    [JsonPropertyName("title")]
    public LocalizedString Title { get; set; } = new();

    /// <summary>Subtitle of the feed.</summary>
    [JsonPropertyName("subtitle")]
    public LocalizedString? Subtitle { get; set; }

    /// <summary>Date of last modification of the feed.</summary>
    [JsonPropertyName("modified")]
    public DateTimeOffset? Modified { get; set; }

    /// <summary>Description of the feed.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>Number of items per page.</summary>
    [JsonPropertyName("itemsPerPage")]
    public int? ItemsPerPage { get; set; }

    /// <summary>Current page of a paginated feed.</summary>
    [JsonPropertyName("currentPage")]
    public int? CurrentPage { get; set; }

    /// <summary>Number of items in the feed.</summary>
    [JsonPropertyName("numberOfItems")]
    public int? NumberOfItems { get; set; }

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }
}