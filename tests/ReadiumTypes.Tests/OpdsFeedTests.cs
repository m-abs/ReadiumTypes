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
          "@context": "http://opds.io/feed/2.0",
          "metadata": { "title": "New arrivals", "numberOfItems": 1 },
          "links": [ { "href": "/", "rel": "self", "type": "application/opds+json" } ],
          "items": [
            {
              "id": "urn:isbn:9780316769488",
              "title": { "value": "The Catcher in the Rye", "language": "en" },
              "language": ["en"],
              "published": "2024-01-15T00:00:00+00:00",
              "authors": [ { "name": "J.D. Salinger" } ],
              "subjects": [ { "name": "Fiction" } ],
              "Series": { "name": "Penguin Classics", "position": 3 },
              "Rels": ["item"],
              "Links": [
                {
                  "href": "/acquire",
                  "rel": "http://opds.io/rel/acquire",
                  "type": "application/pdf",
                  "properties": {
                    "price": { "value": 9.99, "currency": "USD", "valueInteger": 999 },
                    "availability": { "state": "available", "latest": "2025-01-15T00:00:00+00:00" },
                    "holds": { "total": 5, "reserved": 2 },
                    "copies": { "total": 10, "available": 4 },
                    "indirectAcquisition": [ { "type": "application/epub+zip" } ]
                  }
                }
              ]
            }
          ],
          "groups": [ { "title": "Browse", "links": [ { "href": "/subjects", "rel": "http://opds.io/subject" } ] } ],
          "totalItems": 1,
          "next": { "href": "/?page=2" }
        }
        """;

    /// <summary>Feed metadata, links, items, acquisition objects and groups parse into typed properties.</summary>
    [Fact]
    public void FeedParsesExpectedFields()
    {
        Feed feed = JsonSerializer.Deserialize<Feed>(FeedJson, ReadiumJson.Options)!;
        Assert.Equal("New arrivals", feed.Metadata.Title.Default);
        Assert.Equal(1, feed.Metadata.NumberOfItems);
        Assert.Equal("http://opds.io/feed/2.0", Assert.Single(feed.Context));
        Assert.Equal("New arrivals", feed.Metadata.Title.Default);

        OpdsPublication item = Assert.Single(feed.Items);
        Assert.Equal("urn:isbn:9780316769488", item.Id);
        Assert.Equal("The Catcher in the Rye", item.Title.Default);
        Assert.Equal("en", Assert.Single(item.Language));
        Assert.Equal("J.D. Salinger", Assert.Single(item.Authors).Name.Default);
        Assert.Equal(3, Assert.Single(item.Series).Position);
        Assert.Equal("item", Assert.Single(item.Rels));
        Assert.Equal(new DateTimeOffset(2024, 1, 15, 0, 0, 0, TimeSpan.Zero), item.Published);

        LinkProperties properties = Assert.Single(item.Links).Properties!;
        Assert.Equal(9.99m, properties.Price!.Value);
        Assert.Equal("USD", properties.Price.Currency);
        Assert.Equal(999, properties.Price.ValueInteger);
        Assert.Equal(AvailabilityState.Available, properties.Availability!.State);
        Assert.Equal(5, properties.Holds!.Total);
        Assert.Equal(2, properties.Holds.Reserved);
        Assert.Equal(10, properties.Copies!.Total);
        Assert.Equal(4, properties.Copies.Available);
        Assert.Equal("application/epub+zip", Assert.Single(properties.IndirectAcquisition).Type);

        Assert.Equal("Browse", Assert.Single(feed.Groups).Title.Default);
        Assert.Equal(1, feed.TotalItems);
        Assert.Equal("/?page=2", feed.Next!.Href);
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