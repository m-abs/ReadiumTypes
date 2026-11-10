using System.Text.Json;

using Xunit;

namespace Mabs.ReadiumTypes.Tests;

/// <summary><c>[JsonExtensionData]</c> round-trip tests.</summary>
public class ExtensionDataTests
{
    /// <summary>Unknown top-level publication properties are preserved verbatim.</summary>
    [Fact]
    public void UnknownPublicationPropertiesSurviveRoundtrip()
    {
        string json = "{\"metadata\":{\"title\":\"t\"},\"x-vendor\":{\"a\":1},\"x-flag\":true}";
        Publication publication = JsonSerializer.Deserialize<Publication>(json, ReadiumJson.Options)!;
        Assert.NotNull(publication.ExtensionData);
        Assert.Contains("x-vendor", publication.ExtensionData!.Keys);
        string written = JsonSerializer.Serialize(publication, ReadiumJson.Options);
        Assert.Contains("\"x-vendor\":{\"a\":1}", written);
        Assert.Contains("\"x-flag\":true", written);
    }

    /// <summary>Unknown link properties are preserved verbatim.</summary>
    [Fact]
    public void UnknownLinkPropertiesSurviveRoundtrip()
    {
        Link link = JsonSerializer.Deserialize<Link>("{\"href\":\"a\",\"x-size\":42}", ReadiumJson.Options)!;
        Assert.Contains("x-size", link.ExtensionData!.Keys);
        string written = JsonSerializer.Serialize(link, ReadiumJson.Options);
        Assert.Contains("\"x-size\":42", written);
    }

    /// <summary>Unknown metadata properties are preserved verbatim.</summary>
    [Fact]
    public void UnknownMetadataPropertiesSurviveRoundtrip()
    {
        PublicationMetadata metadata = JsonSerializer.Deserialize<PublicationMetadata>("{\"title\":\"t\",\"x-thing\":[1,2]}", ReadiumJson.Options)!;
        Assert.Contains("x-thing", metadata.ExtensionData!.Keys);
        string written = JsonSerializer.Serialize(metadata, ReadiumJson.Options);
        Assert.Contains("\"x-thing\":[1,2]", written);
    }

    /// <summary><c>Add</c> writes the <c>x-</c> kebab-case key and <c>Get</c> reads it back.</summary>
    [Fact]
    public void AddWritesKebabPrefixedKey()
    {
        Publication publication = new();
        publication.Add("VarName", 42);
        string written = JsonSerializer.Serialize(publication, ReadiumJson.Options);
        Assert.Contains("\"x-var-name\":42", written);
        Assert.Equal(42, publication.Get<int>("VarName"));
        Assert.Equal(42, publication.Get<int>("x-var-name"));
    }

    /// <summary><c>AddRaw</c> writes the key verbatim and <c>Get</c> reads it back.</summary>
    [Fact]
    public void AddRawUsesKeyVerbatim()
    {
        Publication publication = new();
        publication.AddRaw("vendor:key", "v");
        string written = JsonSerializer.Serialize(publication, ReadiumJson.Options);
        Assert.Contains("\"vendor:key\":\"v\"", written);
        Assert.Equal("v", publication.Get<string>("vendor:key"));
    }

    /// <summary><c>Get</c> finds both key formats, casts primitives and defaults when absent.</summary>
    [Fact]
    public void GetReadsBothFormatsAndCasts()
    {
        Publication publication = JsonSerializer.Deserialize<Publication>("{\"metadata\":{\"title\":\"t\",\"x-acme-flag\":true,\"other\":3}}", ReadiumJson.Options)!;
        Assert.True(publication.Metadata.Get<bool>("AcmeFlag"));
        Assert.True(publication.Metadata.Get<bool>("x-acme-flag"));
        Assert.Equal(3, publication.Metadata.Get<int>("other"));
        Assert.Equal(3d, publication.Metadata.Get<double>("other"));
        Assert.Null(publication.Metadata.Get<string>("missing"));
        Assert.Equal(0, publication.Metadata.Get<int>("missing"));
    }
}