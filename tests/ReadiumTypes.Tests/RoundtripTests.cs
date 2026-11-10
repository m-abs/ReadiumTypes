using System;
using System.IO;
using System.Text.Json;

using Xunit;

namespace Mabs.ReadiumTypes.Tests;

/// <summary>Manifest fixture deserialize/serialize round-trip tests.</summary>
public class RoundtripTests
{
    private static string ReadFixture(string name)
    {
        return File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "fixtures", name));
    }

    private static Publication DeserializePublication(string json)
    {
        return JsonSerializer.Deserialize<Publication>(json, ReadiumJson.Options)!;
    }

    private static string SerializePublication(Publication publication)
    {
        return JsonSerializer.Serialize(publication, ReadiumJson.Options);
    }

    /// <summary>Each official fixture re-serializes to byte-identical JSON.</summary>
    /// <param name="fixture">Fixture file name under <c>fixtures/</c>.</param>
    [Theory]
    [InlineData("flatland-manifest.json")]
    [InlineData("mobydick-manifest.json")]
    [InlineData("carrotandpepper-manifest.json")]
    public void FixtureRoundtripIsStable(string fixture)
    {
        Publication publication = DeserializePublication(ReadFixture(fixture));
        string first = SerializePublication(publication);
        string second = SerializePublication(DeserializePublication(first));
        Assert.Equal(first, second);
    }

    /// <summary>Flatland manifest parses title, language and reading order.</summary>
    [Fact]
    public void FlatlandParsesExpectedFields()
    {
        Publication publication = DeserializePublication(ReadFixture("flatland-manifest.json"));
        Assert.Equal("Flatland", publication.Metadata.Title.Default);
        Assert.Contains("en", publication.Metadata.Language);
        Assert.NotEmpty(publication.ReadingOrder);
    }
}