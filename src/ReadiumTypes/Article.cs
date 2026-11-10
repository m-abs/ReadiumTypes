using System.Collections.Generic;
using System.Text.Json.Serialization;

using Mabs.ReadiumTypes.Utils;

namespace Mabs.ReadiumTypes;

/// <summary>An article a publication contains.</summary>
public record Article : CollectionObject
{
    /// <summary>Authors of the article.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Contributor>))]
    [JsonPropertyName("author")]
    public List<Contributor> Author { get; set; } = [];

    /// <summary>Translators of the article.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Contributor>))]
    [JsonPropertyName("translator")]
    public List<Contributor> Translator { get; set; } = [];

    /// <summary>Editors of the article.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Contributor>))]
    [JsonPropertyName("editor")]
    public List<Contributor> Editor { get; set; } = [];

    /// <summary>Artists of the article.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Contributor>))]
    [JsonPropertyName("artist")]
    public List<Contributor> Artist { get; set; } = [];

    /// <summary>Illustrators of the article.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Contributor>))]
    [JsonPropertyName("illustrator")]
    public List<Contributor> Illustrator { get; set; } = [];

    /// <summary>Other contributors to the article.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Contributor>))]
    [JsonPropertyName("contributor")]
    public List<Contributor> Contributor { get; set; } = [];

    /// <summary>Description of the article.</summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>Number of pages of the article.</summary>
    [JsonPropertyName("numberOfPages")]
    public int? NumberOfPages { get; set; }

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
}