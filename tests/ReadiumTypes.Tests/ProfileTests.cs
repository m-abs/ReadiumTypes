using System;
using System.IO;
using System.Text.Json;

using Xunit;

namespace Mabs.ReadiumTypes.Tests;

/// <summary>Profile conformance, schema extension, and media-type constant tests.</summary>
public class ProfileTests
{
    /// <summary>Audiobook profile metadata, conformsTo and abridged parse into typed properties.</summary>
    [Fact]
    public void AudiobookProfileRoundTrips()
    {
        const string Json = """
            {
              "metadata": {
                "@type": "http://schema.org/Audiobook",
                "conformsTo": "https://readium.org/webpub-manifest/profiles/audiobook",
                "title": "Moby-Dick",
                "narrator": ["Joe Speaker", "Lucy Narrator"],
                "duration": 4320,
                "abridged": false
              },
              "links": [
                {"rel": "self", "href": "manifest.json", "type": "application/audiobook+json"}
              ],
              "readingOrder": [
                {"href": "part1.mp3", "type": "audio/mpeg", "bitrate": 128, "duration": 1980}
              ]
            }
            """;

        Publication publication = JsonSerializer.Deserialize<Publication>(Json, ReadiumJson.Options)!;

        Assert.Equal("http://schema.org/Audiobook", publication.Metadata.Type);
        Assert.Equal([Profiles.Audiobook], publication.Metadata.ConformsTo);
        Assert.False(publication.Metadata.Abridged);
        Assert.Equal(MediaTypes.AudiobookJson, publication.Links[0].Type);
    }

    /// <summary>conformsTo is omitted when empty and serialized as an array when set.</summary>
    [Fact]
    public void ConformsToSerializesAsArrayAndOmitsWhenEmpty()
    {
        Publication publication = new()
        {
            Metadata = new PublicationMetadata { Title = "Test" },
            ReadingOrder = [new Link { Href = "p.html", Type = MediaTypes.XhtmlXml }],
        };

        string json = JsonSerializer.Serialize(publication, ReadiumJson.Options);
        Assert.DoesNotContain("conformsTo", json);

        publication.Metadata.ConformsTo.Add(Profiles.Epub);
        json = JsonSerializer.Serialize(publication, ReadiumJson.Options);
        Assert.Contains("\"conformsTo\":[\"https://readium.org/webpub-manifest/profiles/epub\"]", json);
    }

    /// <summary>Official fixtures' @type and rel values match the Types and Rel constants.</summary>
    [Fact]
    public void ConstantsMatchOfficialFixtures()
    {
        string carrot = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "fixtures", "carrotandpepper-manifest.json"));
        Publication carrotPublication = JsonSerializer.Deserialize<Publication>(carrot, ReadiumJson.Options)!;
        Assert.Equal(Types.ComicStory, carrotPublication.Metadata.Type);

        string moby = File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "fixtures", "mobydick-manifest.json"));
        Publication mobyPublication = JsonSerializer.Deserialize<Publication>(moby, ReadiumJson.Options)!;
        Assert.Equal(Types.Book, mobyPublication.Metadata.Type);
        Assert.Contains(Rel.Self, mobyPublication.Links[0].Rel);
        Assert.Contains(Rel.Cover, mobyPublication.Resources[0].Rel);
    }

    /// <summary>The position-list service document from the specification round-trips.</summary>
    [Fact]
    public void PositionListRoundTrips()
    {
        const string Json = """
            {
              "total": 2,
              "positions": [
                {
                  "href": "http://example.com/resource",
                  "type": "application/xhtml+xml",
                  "locations": {
                    "fragments": ["partialcfi(/4[body01])"],
                    "position": 1,
                    "progression": 0.0
                  }
                },
                {
                  "href": "http://example.com/resource",
                  "type": "application/xhtml+xml",
                  "locations": {
                    "fragments": ["partialcfi(/18[body02])"],
                    "position": 2,
                    "progression": 0.57289
                  }
                }
              ]
            }
            """;

        PositionList list = JsonSerializer.Deserialize<PositionList>(Json, ReadiumJson.Options)!;

        Assert.Equal(2, list.Total);
        Assert.Equal(2, list.Positions.Count);
        Assert.Equal(["partialcfi(/18[body02])"], list.Positions[1].Locations?.Fragments);
        Assert.Equal(2, list.Positions[1].Locations?.Position);
        Assert.Equal(0.57289, list.Positions[1].Locations?.Progression);

        string json = JsonSerializer.Serialize(list, ReadiumJson.Options);
        Assert.Contains("\"total\":2", json);
        Assert.Contains("\"positions\":[", json);
    }

    /// <summary>The kebab-case media-overlay styling object and camelCase EPUB mediaOverlay object coexist.</summary>
    [Fact]
    public void BothMediaOverlayKeysCoexist()
    {
        const string Json = """
            {
              "metadata": {
                "title": "Book",
                "media-overlay": { "active-class": "active", "playback-active-class": "playing" },
                "mediaOverlay": {"activeClass": "active"}
              },
              "readingOrder": [{"href": "c1.xhtml", "type": "application/xhtml+xml"}]
            }
            """;

        Publication publication = JsonSerializer.Deserialize<Publication>(Json, ReadiumJson.Options)!;

        Assert.Equal("active", publication.Metadata.MediaOverlay!.ActiveClass);
        Assert.Equal("playing", publication.Metadata.MediaOverlay!.PlaybackActiveClass);
        Assert.Equal("active", publication.Metadata.EpubMediaOverlay?.ActiveClass);

        string json = JsonSerializer.Serialize(publication, ReadiumJson.Options);
        Assert.Contains("\"media-overlay\":{\"active-class\":\"active\",\"playback-active-class\":\"playing\"}", json);
        Assert.Contains("\"mediaOverlay\":{\"activeClass\":\"active\"}", json);
    }

    /// <summary>EPUB metadata extension, toc and sub-collections round-trip.</summary>
    [Fact]
    public void EpubExtensionsRoundTrip()
    {
        const string Json = """
            {
              "metadata": {
                "@type": "http://schema.org/EBook",
                "conformsTo": ["https://readium.org/webpub-manifest/profiles/epub"],
                "title": "Book",
                "mediaOverlay": {
                  "activeClass": "active",
                  "playbackActiveClass": "playback-active"
                }
              },
              "readingOrder": [
                {"href": "chapter1.xhtml", "type": "application/xhtml+xml",
                 "properties": {"contains": ["svg", "remote-resources"], "layout": "fixed"}}
              ],
              "toc": [{"href": "chapter1.xhtml", "title": "Chapter 1"}],
              "pageList": [{"href": "p1.xhtml", "title": "1"}],
              "landmarks": [{"href": "toc.xhtml", "rel": "toc"}],
              "loi": [{"href": "img1.svg", "title": "Figure 1"}]
            }
            """;

        Publication publication = JsonSerializer.Deserialize<Publication>(Json, ReadiumJson.Options)!;

        Assert.Equal("active", publication.Metadata.EpubMediaOverlay?.ActiveClass);
        Assert.Equal("playback-active", publication.Metadata.EpubMediaOverlay?.PlaybackActiveClass);
        Assert.Single(publication.Toc);
        Assert.Single(publication.PageList);
        Assert.Single(publication.Landmarks);
        Assert.Single(publication.Loi);
        Assert.Empty(publication.Loa);
        Assert.Equal(MediaTypes.XhtmlXml, publication.ReadingOrder[0].Type);
        Assert.Equal(["svg", "remote-resources"], publication.ReadingOrder[0].Properties!.Contains);
        Assert.Equal(LinkLayout.Fixed, publication.ReadingOrder[0].Properties!.Layout);

        string json = JsonSerializer.Serialize(publication, ReadiumJson.Options);
        Assert.Contains("\"mediaOverlay\":{\"activeClass\":\"active\",\"playbackActiveClass\":\"playback-active\"}", json);
        Assert.Contains("\"toc\"", json);
        Assert.Contains("\"pageList\"", json);
        Assert.DoesNotContain("\"loa\"", json);
    }
}