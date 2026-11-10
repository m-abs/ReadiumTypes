using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using Mabs.ReadiumTypes.Utils;

namespace Mabs.ReadiumTypes.Opds;

/// <summary>An OPDS 2.0 publication entry in a feed.</summary>
public record OpdsPublication : IExtensionDataHolder
{
    /// <summary>Identifier of the publication.</summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>Title of the publication.</summary>
    [JsonPropertyName("title")]
    public LocalizedString Title { get; set; } = new();

    /// <summary>Date of publication.</summary>
    [JsonPropertyName("published")]
    public DateTimeOffset? Published { get; set; }

    /// <summary>Date of last modification.</summary>
    [JsonPropertyName("updated")]
    public DateTimeOffset? Updated { get; set; }

    /// <summary>Authors of the publication.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Contributor>))]
    [JsonPropertyName("authors")]
    public List<Contributor> Authors { get; set; } = [];

    /// <summary>Publishers of the publication.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Contributor>))]
    [JsonPropertyName("publishers")]
    public List<Contributor> Publishers { get; set; } = [];

    /// <summary>BCP 47 language tags of the publication's content.</summary>
    [JsonConverter(typeof(StringOrArrayConverter))]
    [JsonPropertyName("language")]
    public List<string> Language { get; set; } = [];

    /// <summary>Subjects of the publication.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Subject>))]
    [JsonPropertyName("subjects")]
    public List<Subject> Subjects { get; set; } = [];

    /// <summary>Description of the publication.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>Duration of the publication in seconds.</summary>
    [JsonPropertyName("Duration")]
    public double? Duration { get; set; }

    /// <summary>Number of pages of the publication.</summary>
    [JsonPropertyName("NumPages")]
    public int? NumPages { get; set; }

    /// <summary>Accessibility information for the publication.</summary>
    [JsonPropertyName("accessibility")]
    public Accessibility? Accessibility { get; set; }

    /// <summary>Media overlay documents of the publication.</summary>
    [JsonPropertyName("MediaOverlay")]
    public List<MediaOverlay> MediaOverlay { get; set; } = [];

    /// <summary>Entities the metadata describes.</summary>
    [JsonPropertyName("BelongsTo")]
    public MetadataBelongsTo? BelongsTo { get; set; }

    /// <summary>Entities the metadata contains.</summary>
    [JsonPropertyName("Contains")]
    public MetadataContains? Contains { get; set; }

    /// <summary>Series the publication belongs to.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Series>))]
    [JsonPropertyName("Series")]
    public List<Series> Series { get; set; } = [];

    /// <summary>Collections the publication belongs to.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Collection>))]
    [JsonPropertyName("Collection")]
    public List<Collection> Collection { get; set; } = [];

    /// <summary>Links to cover images of the publication.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Link>))]
    [JsonPropertyName("Image")]
    public List<Link> Image { get; set; } = [];

    /// <summary>Links to audio resources of the publication.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Link>))]
    [JsonPropertyName("Audio")]
    public List<Link> Audio { get; set; } = [];

    /// <summary>Links to video resources of the publication.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Link>))]
    [JsonPropertyName("Video")]
    public List<Link> Video { get; set; } = [];

    /// <summary>Links of the publication entry.</summary>
    [JsonPropertyName("Links")]
    public List<Link> Links { get; set; } = [];

    /// <summary>Link relations of the publication entry.</summary>
    [JsonConverter(typeof(StringOrArrayConverter))]
    [JsonPropertyName("Rels")]
    public List<string> Rels { get; set; } = [];

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }

    /// <summary>Determines whether <see cref="Authors"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Authors"/> contains items.</returns>
    public bool ShouldSerializeAuthors()
    {
        return Authors.Count > 0;
    }

    /// <summary>Determines whether <see cref="Publishers"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Publishers"/> contains items.</returns>
    public bool ShouldSerializePublishers()
    {
        return Publishers.Count > 0;
    }

    /// <summary>Determines whether <see cref="Language"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Language"/> contains items.</returns>
    public bool ShouldSerializeLanguage()
    {
        return Language.Count > 0;
    }

    /// <summary>Determines whether <see cref="Subjects"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Subjects"/> contains items.</returns>
    public bool ShouldSerializeSubjects()
    {
        return Subjects.Count > 0;
    }

    /// <summary>Determines whether <see cref="MediaOverlay"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="MediaOverlay"/> contains items.</returns>
    public bool ShouldSerializeMediaOverlay()
    {
        return MediaOverlay.Count > 0;
    }

    /// <summary>Determines whether <see cref="Series"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Series"/> contains items.</returns>
    public bool ShouldSerializeSeries()
    {
        return Series.Count > 0;
    }

    /// <summary>Determines whether <see cref="Collection"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Collection"/> contains items.</returns>
    public bool ShouldSerializeCollection()
    {
        return Collection.Count > 0;
    }

    /// <summary>Determines whether <see cref="Image"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Image"/> contains items.</returns>
    public bool ShouldSerializeImage()
    {
        return Image.Count > 0;
    }

    /// <summary>Determines whether <see cref="Audio"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Audio"/> contains items.</returns>
    public bool ShouldSerializeAudio()
    {
        return Audio.Count > 0;
    }

    /// <summary>Determines whether <see cref="Video"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Video"/> contains items.</returns>
    public bool ShouldSerializeVideo()
    {
        return Video.Count > 0;
    }

    /// <summary>Determines whether <see cref="Rels"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Rels"/> contains items.</returns>
    public bool ShouldSerializeRels()
    {
        return Rels.Count > 0;
    }
}