using System;
using System.Text.Json;

using Mabs.ReadiumTypes.Opds;

using Xunit;

namespace Mabs.ReadiumTypes.Tests;

/// <summary>OPDS 2.0 feed parsing and round-trip tests.</summary>
public class OpdsFeedTests
{
    private const string FeedJson = """
        {
          "metadata": { "title": "New arrivals", "numberOfItems": 1 },
          "links": [ { "href": "/", "rel": "self", "type": "application/opds+json" } ],
          "publications": [
            {
              "metadata": {
                "identifier": "urn:isbn:9780316769488",
                "title": "The Catcher in the Rye",
                "language": ["en"],
                "modified": "2024-01-15T00:00:00+00:00"
              },
              "links": [
                {
                  "href": "/acquire",
                  "rel": "http://opds-spec.org/acquisition/buy",
                  "type": "application/pdf",
                  "properties": {
                    "price": { "value": 9.99, "currency": "USD", "valueInteger": 999 },
                    "availability": { "state": "available", "latest": "2025-01-15T00:00:00+00:00" },
                    "holds": { "total": 5, "reserved": 2 },
                    "copies": { "total": 10, "available": 4 },
                    "indirectAcquisition": [ { "type": "application/epub+zip" } ]
                  }
                }
              ],
              "images": [ { "href": "/cover.jpg", "type": "image/jpeg" } ]
            }
          ],
          "navigation": [ { "href": "/subjects", "title": "Subjects" } ],
          "facets": [
            {
              "metadata": { "title": "Availability" },
              "links": [ { "href": "/available", "rel": "http://opds-spec.org/facet" } ]
            }
          ],
          "groups": [
            {
              "metadata": { "title": "Browse" },
              "links": [ { "href": "/browse", "rel": "http://opds-spec.org/catalog" } ],
              "navigation": [ { "href": "/fiction", "title": "Fiction" } ]
            }
          ]
        }
        """;

    /// <summary>Feed metadata, links, publications, navigation, facets, groups and acquisition objects parse into typed properties.</summary>
    [Fact]
    public void FeedParsesExpectedFields()
    {
        Feed feed = JsonSerializer.Deserialize<Feed>(FeedJson, ReadiumJson.Options)!;
        Assert.Equal("New arrivals", feed.Metadata.Title.Default);
        Assert.Equal(1, feed.Metadata.NumberOfItems);
        Assert.Equal("self", Assert.Single(feed.Links).Rel[0]);

        OpdsPublication publication = Assert.Single(feed.Publications);
        Assert.Equal("urn:isbn:9780316769488", publication.Metadata.Identifier);
        Assert.Equal("The Catcher in the Rye", publication.Metadata.Title.Default);
        Assert.Equal("en", Assert.Single(publication.Metadata.Language));
        Assert.Equal(new DateTimeOffset(2024, 1, 15, 0, 0, 0, TimeSpan.Zero), publication.Metadata.Modified);
        Assert.Equal("/cover.jpg", Assert.Single(publication.Images).Href);

        LinkProperties properties = Assert.Single(publication.Links).Properties!;
        Assert.Equal(9.99m, properties.Price!.Value);
        Assert.Equal("USD", properties.Price.Currency);
        Assert.Equal(999, properties.Price.ValueInteger);
        Assert.Equal(AvailabilityState.Available, properties.Availability!.State);
        Assert.Equal(5, properties.Holds!.Total);
        Assert.Equal(2, properties.Holds.Reserved);
        Assert.Equal(10, properties.Copies!.Total);
        Assert.Equal(4, properties.Copies.Available);
        Assert.Equal("application/epub+zip", Assert.Single(properties.IndirectAcquisition).Type);

        Assert.Equal("Subjects", Assert.Single(feed.Navigation).Title);
        Assert.Equal("Availability", Assert.Single(feed.Facets).Metadata.Title.Default);
        FeedGroup group = Assert.Single(feed.Groups);
        Assert.Equal("Browse", group.Metadata.Title.Default);
        Assert.Equal("Fiction", Assert.Single(group.Navigation).Title);
    }

    /// <summary>Feed JSON round-trips unchanged.</summary>
    [Fact]
    public void FeedRoundtripIsStable()
    {
        Feed feed = JsonSerializer.Deserialize<Feed>(FeedJson, ReadiumJson.Options)!;
        string first = JsonSerializer.Serialize(feed, ReadiumJson.Options);
        string second = JsonSerializer.Serialize(JsonSerializer.Deserialize<Feed>(first, ReadiumJson.Options)!, ReadiumJson.Options);
        Assert.Equal(first, second);
    }
}