using System.Text.Json;

using Xunit;

namespace Mabs.ReadiumTypes.Tests;

/// <summary>Media overlay styling, syncnarr link and syncnarr document parsing tests.</summary>
public class MediaOverlayTests
{
    private const string ManifestJson = """
        {
          "metadata": {
            "title": "Audio book",
            "media-overlay": { "active-class": "-epub-media-overlay-active" }
          },
          "links": [],
          "readingOrder": [
            {
              "href": "p1.xhtml",
              "type": "application/xhtml+xml",
              "duration": 860.5,
              "alternate": [
                { "type": "application/vnd.syncnarr+json", "duration": 860.5, "href": "media-overlays_0.json" }
              ]
            }
          ]
        }
        """;

    private const string SyncNarrJson = """
        {
          "role": ["section", "bodymatter"],
          "narration": [
            { "text": "p1.xhtml#c01h01", "audio": "audio.mp3#t=24.5,29.268" },
            { "role": "aside", "text": "p1.xhtml#c01w00001", "audio": "audio.mp3#t=29.268,29.441" }
          ]
        }
        """;

    /// <summary>Media overlay styling and the syncnarr alternate link parse into typed properties.</summary>
    [Fact]
    public void MediaOverlayParsesExpectedFields()
    {
        Publication publication = JsonSerializer.Deserialize<Publication>(ManifestJson, ReadiumJson.Options)!;
        Assert.Equal("-epub-media-overlay-active", publication.Metadata.MediaOverlay!.ActiveClass);

        Link chapter = Assert.Single(publication.ReadingOrder);
        Link alternate = Assert.Single(chapter.Alternate);
        Assert.Equal(MediaTypes.SyncNarrJson, alternate.Type);
        Assert.Equal("media-overlays_0.json", alternate.Href);
    }

    /// <summary>A vnd.syncnarr+json document parses into the node tree, string or array roles included.</summary>
    [Fact]
    public void SyncNarrDocumentParsesIntoNodeTree()
    {
        MediaOverlayNode root = JsonSerializer.Deserialize<MediaOverlayNode>(SyncNarrJson, ReadiumJson.Options)!;
        Assert.Equal(["section", "bodymatter"], root.Role);
        Assert.Equal(2, root.Narration.Count);
        Assert.Empty(root.Narration[0].Role);
        Assert.Equal("p1.xhtml#c01h01", root.Narration[0].Text);
        Assert.Equal("audio.mp3#t=29.268,29.441", root.Narration[1].Audio);
        Assert.Equal(["aside"], root.Narration[1].Role);
    }

    /// <summary>Manifest and syncnarr document JSON round-trip unchanged.</summary>
    [Fact]
    public void MediaOverlayRoundtripIsStable()
    {
        Publication publication = JsonSerializer.Deserialize<Publication>(ManifestJson, ReadiumJson.Options)!;
        string first = JsonSerializer.Serialize(publication, ReadiumJson.Options);
        string second = JsonSerializer.Serialize(JsonSerializer.Deserialize<Publication>(first, ReadiumJson.Options)!, ReadiumJson.Options);
        Assert.Equal(first, second);
        Assert.Contains("\"media-overlay\":{\"active-class\":\"-epub-media-overlay-active\"}", first);

        MediaOverlayNode root = JsonSerializer.Deserialize<MediaOverlayNode>(SyncNarrJson, ReadiumJson.Options)!;
        string narrFirst = JsonSerializer.Serialize(root, ReadiumJson.Options);
        Assert.Equal(narrFirst, JsonSerializer.Serialize(JsonSerializer.Deserialize<MediaOverlayNode>(narrFirst, ReadiumJson.Options)!, ReadiumJson.Options));
        Assert.Contains("\"role\":[\"aside\"]", narrFirst);
        Assert.DoesNotContain("\"video\"", narrFirst);
    }
}