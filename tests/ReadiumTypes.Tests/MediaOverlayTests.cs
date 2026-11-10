using System.Text.Json;

using Xunit;

namespace Mabs.ReadiumTypes.Tests;

/// <summary>Media overlay manifest and node parsing tests.</summary>
public class MediaOverlayTests
{
    private const string ManifestJson = """
        {
          "metadata": {
            "title": "Audio book",
            "media-overlay": [
              {
                "href": "overlay.json",
                "type": "application/vnd.readium.mo+json",
                "language": "en",
                "audio": "audio.mp3",
                "playback-speed": 1.0,
                "volume": 0.5,
                "nodes": {
                  "id": "root",
                  "class": "paragraph",
                  "children": [
                    {
                      "id": "p1",
                      "ref-id": "root",
                      "text": "#p1",
                      "text-type": "text/html",
                      "audio-src": "audio.mp3#t=0.0,3.5"
                    }
                  ]
                }
              }
            ]
          },
          "links": [],
          "readingOrder": [
            {
              "href": "p1.xhtml",
              "type": "application/xhtml+xml",
              "properties": {
                "media-overlay": { "playback-speed": 1.25, "text-to-speech-voice": "Karen" }
              }
            }
          ]
        }
        """;

    /// <summary>mediaOverlay metadata, nodes and link properties parse into typed properties.</summary>
    [Fact]
    public void MediaOverlayParsesExpectedFields()
    {
        Publication publication = JsonSerializer.Deserialize<Publication>(ManifestJson, ReadiumJson.Options)!;
        MediaOverlay overlay = Assert.Single(publication.Metadata.MediaOverlay);
        Assert.Equal("overlay.json", overlay.Href);
        Assert.Equal("en", overlay.Language);
        Assert.Equal(1.0, overlay.PlaybackSpeed);
        Assert.Equal(0.5, overlay.Volume);
        Assert.Equal("root", overlay.Nodes!.Id);
        MediaOverlayNode child = Assert.Single(overlay.Nodes.Children);
        Assert.Equal("p1", child.Id);
        Assert.Equal("root", child.RefId);
        Assert.Equal("audio.mp3#t=0.0,3.5", child.AudioSrc);

        MediaOverlayProperties properties = Assert.Single(publication.ReadingOrder).Properties!.MediaOverlay!;
        Assert.Equal(1.25, properties.PlaybackSpeed);
        Assert.Equal("Karen", properties.TextToSpeechVoice);
    }

    /// <summary>Media overlay JSON round-trips unchanged.</summary>
    [Fact]
    public void MediaOverlayRoundtripIsStable()
    {
        Publication publication = JsonSerializer.Deserialize<Publication>(ManifestJson, ReadiumJson.Options)!;
        string first = JsonSerializer.Serialize(publication, ReadiumJson.Options);
        string second = JsonSerializer.Serialize(JsonSerializer.Deserialize<Publication>(first, ReadiumJson.Options)!, ReadiumJson.Options);
        Assert.Equal(first, second);
    }
}