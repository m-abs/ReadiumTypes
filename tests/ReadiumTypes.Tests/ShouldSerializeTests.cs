using System.Text.Json;

using Mabs.ReadiumTypes.Opds;

using Xunit;

namespace Mabs.ReadiumTypes.Tests;

/// <summary><c>ShouldSerialize{Prop}()</c> behavior through <see cref="ReadiumJson.Options"/>.</summary>
public class ShouldSerializeTests
{
    /// <summary>Empty optional publication collections are absent from output; required ones are written.</summary>
    [Fact]
    public void EmptyOptionalCollectionsAreOmitted()
    {
        string json = JsonSerializer.Serialize(new Publication(), ReadiumJson.Options);
        Assert.DoesNotContain("\"@context\"", json);
        Assert.DoesNotContain("\"resources\"", json);
        Assert.DoesNotContain("\"covers\"", json);
        Assert.Contains("\"links\":[]", json);
        Assert.Contains("\"readingOrder\":[]", json);
    }

    /// <summary>Non-empty optional collections are written.</summary>
    [Fact]
    public void NonEmptyOptionalCollectionsAreWritten()
    {
        Publication publication = new();
        publication.Resources.Add(new Link { Href = "r" });
        string json = JsonSerializer.Serialize(publication, ReadiumJson.Options);
        Assert.Contains("\"resources\":[", json);
    }

    /// <summary>Empty optional link collections are absent from output.</summary>
    [Fact]
    public void EmptyLinkCollectionsAreOmitted()
    {
        string json = JsonSerializer.Serialize(new Link { Href = "a" }, ReadiumJson.Options);
        Assert.DoesNotContain("\"rel\"", json);
        Assert.DoesNotContain("\"language\"", json);
        Assert.DoesNotContain("\"alternate\"", json);
        Assert.DoesNotContain("\"children\"", json);
    }

    /// <summary>Empty optional metadata collections are absent from output.</summary>
    [Fact]
    public void EmptyMetadataCollectionsAreOmitted()
    {
        string json = JsonSerializer.Serialize(new PublicationMetadata { Title = new LocalizedString("t") }, ReadiumJson.Options);
        Assert.DoesNotContain("\"author\"", json);
        Assert.DoesNotContain("\"subject\"", json);
        Assert.DoesNotContain("\"language\"", json);
        Assert.DoesNotContain("\"altIdentifier\"", json);
    }

    /// <summary>Empty feed items and groups are absent from output; required links are written.</summary>
    [Fact]
    public void FeedOmitsEmptyItemsAndGroups()
    {
        Feed feed = new() { Metadata = new FeedMetadata { Title = new LocalizedString("t") } };
        string json = JsonSerializer.Serialize(feed, ReadiumJson.Options);
        Assert.DoesNotContain("\"items\"", json);
        Assert.DoesNotContain("\"groups\"", json);
        Assert.Contains("\"links\":[]", json);
    }
}