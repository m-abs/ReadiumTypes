using System.Collections.Generic;
using System.Text.Json.Serialization;

using Mabs.ReadiumTypes.Utils;

namespace Mabs.ReadiumTypes;

/// <summary>An issue a publication contains.</summary>
public record Issue : CollectionObject
{
    /// <summary>Articles of the issue.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Article>))]
    [JsonPropertyName("article")]
    public List<Article> Article { get; set; } = [];

    /// <summary>Chapters of the issue.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<Chapter>))]
    [JsonPropertyName("chapter")]
    public List<Chapter> Chapter { get; set; } = [];

    /// <summary>Determines whether <see cref="Article"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Article"/> contains items.</returns>
    public bool ShouldSerializeArticle()
    {
        return Article.Count > 0;
    }

    /// <summary>Determines whether <see cref="Chapter"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Chapter"/> contains items.</returns>
    public bool ShouldSerializeChapter()
    {
        return Chapter.Count > 0;
    }
}