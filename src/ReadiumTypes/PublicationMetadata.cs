using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using Mabs.ReadiumTypes.Utils;

namespace Mabs.ReadiumTypes;

/// <summary>Metadata of a publication, per the web publication metadata schema.</summary>
public record PublicationMetadata : IExtensionDataHolder
{
    /// <summary>Identifier of the publication.</summary>
    [JsonPropertyName("identifier")]
    public string? Identifier { get; set; }

    /// <summary>Title of the publication.</summary>
    [JsonPropertyName("title")]
    public LocalizedString Title { get; set; } = new();

    /// <summary>String used to sort the publication by title.</summary>
    [JsonPropertyName("sortAs")]
    public LocalizedString? SortAs { get; set; }

    /// <summary>Alternate identifiers of the publication.</summary>
    [JsonConverter(typeof(AltIdentifierConverter))]
    [JsonPropertyName("altIdentifier")]
    public List<AltIdentifier> AltIdentifier { get; set; } = [];

    /// <summary>Publisher of the publication.</summary>
    [JsonPropertyName("publisher")]
    public LocalizedString? Publisher { get; set; }

    /// <summary>Date of publication.</summary>
    [JsonPropertyName("published")]
    public DateTimeOffset? Published { get; set; }

    /// <summary>Date of last modification.</summary>
    [JsonPropertyName("modified")]
    public DateTimeOffset? Modified { get; set; }

    /// <summary>BCP 47 language tags of the publication's content.</summary>
    [JsonConverter(typeof(StringOrArrayConverter))]
    [JsonPropertyName("language")]
    public List<string> Language { get; set; } = [];

    /// <summary>Subjects of the publication.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Subject>))]
    [JsonPropertyName("subject")]
    public List<Subject> Subject { get; set; } = [];

    /// <summary>Authors of the publication.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Contributor>))]
    [JsonPropertyName("author")]
    public List<Contributor> Author { get; set; } = [];

    /// <summary>Translators of the publication.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Contributor>))]
    [JsonPropertyName("translator")]
    public List<Contributor> Translator { get; set; } = [];

    /// <summary>Editors of the publication.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Contributor>))]
    [JsonPropertyName("editor")]
    public List<Contributor> Editor { get; set; } = [];

    /// <summary>Artists of the publication.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Contributor>))]
    [JsonPropertyName("artist")]
    public List<Contributor> Artist { get; set; } = [];

    /// <summary>Illustrators of the publication.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Contributor>))]
    [JsonPropertyName("illustrator")]
    public List<Contributor> Illustrator { get; set; } = [];

    /// <summary>Other contributors to the publication.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Contributor>))]
    [JsonPropertyName("contributor")]
    public List<Contributor> Contributor { get; set; } = [];

    /// <summary>Description of the publication.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>Duration of the publication in seconds.</summary>
    [JsonPropertyName("duration")]
    public double? Duration { get; set; }

    /// <summary>Number of pages of the publication.</summary>
    [JsonPropertyName("numberOfPages")]
    public int? NumberOfPages { get; set; }

    /// <summary>Entities the metadata describes.</summary>
    [JsonPropertyName("belongs_to")]
    public MetadataBelongsTo? BelongsTo { get; set; }

    /// <summary>Entities the metadata contains.</summary>
    [JsonPropertyName("contains")]
    public MetadataContains? Contains { get; set; }

    /// <summary>Reading progression of the publication.</summary>
    [JsonPropertyName("readingProgression")]
    public ReadingProgression? ReadingProgression { get; set; }

    /// <summary>How the publication should be displayed in a synthetic spread.</summary>
    [JsonPropertyName("layout")]
    public Layout? Layout { get; set; }

    /// <summary>Accessibility information for the publication.</summary>
    [JsonPropertyName("accessibility")]
    public Accessibility? Accessibility { get; set; }

    /// <summary>Media overlay documents of the publication.</summary>
    [JsonPropertyName("media-overlay")]
    public List<MediaOverlay> MediaOverlay { get; set; } = [];

    /// <summary>Text and data mining reservation policy of the publication.</summary>
    [JsonPropertyName("tdm")]
    public Tdm? Tdm { get; set; }

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }

    /// <summary>Determines whether <see cref="AltIdentifier"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="AltIdentifier"/> contains items.</returns>
    public bool ShouldSerializeAltIdentifier()
    {
        return AltIdentifier.Count > 0;
    }

    /// <summary>Determines whether <see cref="Language"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Language"/> contains items.</returns>
    public bool ShouldSerializeLanguage()
    {
        return Language.Count > 0;
    }

    /// <summary>Determines whether <see cref="Subject"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Subject"/> contains items.</returns>
    public bool ShouldSerializeSubject()
    {
        return Subject.Count > 0;
    }

    /// <summary>Determines whether <see cref="Author"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Author"/> contains items.</returns>
    public bool ShouldSerializeAuthor()
    {
        return Author.Count > 0;
    }

    /// <summary>Determines whether <see cref="Translator"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Translator"/> contains items.</returns>
    public bool ShouldSerializeTranslator()
    {
        return Translator.Count > 0;
    }

    /// <summary>Determines whether <see cref="Editor"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Editor"/> contains items.</returns>
    public bool ShouldSerializeEditor()
    {
        return Editor.Count > 0;
    }

    /// <summary>Determines whether <see cref="Artist"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Artist"/> contains items.</returns>
    public bool ShouldSerializeArtist()
    {
        return Artist.Count > 0;
    }

    /// <summary>Determines whether <see cref="Illustrator"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Illustrator"/> contains items.</returns>
    public bool ShouldSerializeIllustrator()
    {
        return Illustrator.Count > 0;
    }

    /// <summary>Determines whether <see cref="Contributor"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Contributor"/> contains items.</returns>
    public bool ShouldSerializeContributor()
    {
        return Contributor.Count > 0;
    }

    /// <summary>Determines whether <see cref="MediaOverlay"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="MediaOverlay"/> contains items.</returns>
    public bool ShouldSerializeMediaOverlay()
    {
        return MediaOverlay.Count > 0;
    }
}