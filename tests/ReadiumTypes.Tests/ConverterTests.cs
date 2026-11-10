using System.Text.Json;

using Mabs.ReadiumTypes.GuidedNavigation;

using Xunit;

namespace Mabs.ReadiumTypes.Tests;

/// <summary>Wire-form converter read/write tests.</summary>
public class ConverterTests
{
    /// <summary>A bare string deserializes into a one-element list.</summary>
    [Fact]
    public void StringOrArrayReadsBareString()
    {
        Link link = JsonSerializer.Deserialize<Link>("{\"href\":\"a\",\"rel\":\"preview\"}", ReadiumJson.Options)!;
        Assert.Equal("preview", Assert.Single(link.Rel));
    }

    /// <summary>Serialization always emits the array form.</summary>
    [Fact]
    public void StringOrArrayWritesArrayForm()
    {
        Link link = new() { Href = "a" };
        link.Rel.Add("preview");
        string json = JsonSerializer.Serialize(link, ReadiumJson.Options);
        Assert.Contains("\"rel\":[\"preview\"]", json);
    }

    /// <summary>Bare string, language map, OPDS object and OPDS array forms all deserialize.</summary>
    [Fact]
    public void LocalizedStringReadsAllWireForms()
    {
        PublicationMetadata bare = JsonSerializer.Deserialize<PublicationMetadata>("{\"title\":\"x\"}", ReadiumJson.Options)!;
        Assert.Equal("x", bare.Title.Default);

        PublicationMetadata map = JsonSerializer.Deserialize<PublicationMetadata>("{\"title\":{\"en\":\"x\",\"fr\":\"y\"}}", ReadiumJson.Options)!;
        Assert.Equal("x", map.Title["en"]);
        Assert.Equal("y", map.Title["fr"]);

        PublicationMetadata opds = JsonSerializer.Deserialize<PublicationMetadata>("{\"title\":{\"value\":\"x\",\"language\":\"en\"}}", ReadiumJson.Options)!;
        Assert.Equal("x", opds.Title["en"]);

        PublicationMetadata array = JsonSerializer.Deserialize<PublicationMetadata>("{\"title\":[{\"value\":\"x\",\"language\":\"en\"},{\"value\":\"y\",\"language\":\"fr\"}]}", ReadiumJson.Options)!;
        Assert.Equal("x", array.Title["en"]);
        Assert.Equal("y", array.Title["fr"]);
    }

    /// <summary>A single default-language value writes as a bare string.</summary>
    [Fact]
    public void LocalizedStringWritesBareStringForDefaultOnly()
    {
        PublicationMetadata metadata = new() { Title = new LocalizedString("x") };
        string json = JsonSerializer.Serialize(metadata, ReadiumJson.Options);
        Assert.Contains("\"title\":\"x\"", json);
    }

    /// <summary>Multiple languages write as a BCP-47 map.</summary>
    [Fact]
    public void LocalizedStringWritesMapForMultipleLanguages()
    {
        PublicationMetadata metadata = new();
        metadata.Title["en"] = "x";
        metadata.Title["fr"] = "y";
        string json = JsonSerializer.Serialize(metadata, ReadiumJson.Options);
        Assert.Contains("\"title\":{\"en\":\"x\",\"fr\":\"y\"}", json);
    }

    /// <summary>A language-unspecified value alongside tagged languages writes the <c>und</c> key, never an empty key.</summary>
    [Fact]
    public void LocalizedStringWritesUndForDefaultAlongsideTagged()
    {
        PublicationMetadata metadata = new();
        LocalizedString title = new("Hello");
        title["fr"] = "Bonjour";
        metadata.Title = title;
        string json = JsonSerializer.Serialize(metadata, ReadiumJson.Options);
        Assert.Contains("\"title\":{\"und\":\"Hello\",\"fr\":\"Bonjour\"}", json);
        Assert.DoesNotContain("\"\":", json);
    }

    /// <summary>Single object and mixed string/object arrays normalize to lists.</summary>
    [Fact]
    public void OneOrArrayObjectReadsSingleObjectAndMixedArray()
    {
        PublicationMetadata single = JsonSerializer.Deserialize<PublicationMetadata>("{\"title\":\"t\",\"author\":{\"name\":\"A\"}}", ReadiumJson.Options)!;
        Assert.Equal("A", Assert.Single(single.Author).Name.Default);

        PublicationMetadata mixed = JsonSerializer.Deserialize<PublicationMetadata>("{\"title\":\"t\",\"author\":[\"B\",{\"name\":\"C\"}]}", ReadiumJson.Options)!;
        Assert.Equal(2, mixed.Author.Count);
        Assert.Equal("B", mixed.Author[0].Name.Default);
        Assert.Equal("C", mixed.Author[1].Name.Default);
    }

    /// <summary>A bare number becomes an element with only <c>Position</c> set.</summary>
    [Fact]
    public void OneOrArrayObjectReadsBareNumberAsPosition()
    {
        PublicationMetadata metadata = JsonSerializer.Deserialize<PublicationMetadata>("{\"title\":\"t\",\"contains\":{\"series\":3}}", ReadiumJson.Options)!;
        Assert.Equal(3d, Assert.Single(metadata.Contains!.Series).Position!.Value);
    }

    /// <summary>altIdentifier accepts bare strings and objects in the same array.</summary>
    [Fact]
    public void AltIdentifierReadsBareStringsAndObjects()
    {
        PublicationMetadata metadata = JsonSerializer.Deserialize<PublicationMetadata>("{\"title\":\"t\",\"altIdentifier\":[\"urn:isbn:1\",{\"value\":\"urn:isbn:2\",\"scheme\":\"https://schema.org/isbn\"}]}", ReadiumJson.Options)!;
        Assert.Equal(2, metadata.AltIdentifier.Count);
        Assert.Equal("urn:isbn:1", metadata.AltIdentifier[0].Value);
        Assert.Equal("urn:isbn:2", metadata.AltIdentifier[1].Value);
        Assert.Equal("https://schema.org/isbn", metadata.AltIdentifier[1].Scheme);
    }

    /// <summary>Navigation text accepts bare string and object forms.</summary>
    [Fact]
    public void NavigationTextReadsStringAndObjectForms()
    {
        NavigationDocument document = JsonSerializer.Deserialize<NavigationDocument>("{\"guided-navigation\":[{\"description\":\"hello\"},{\"description\":{\"plain\":\"p\",\"ssml\":\"s\",\"language\":\"en\"}}]}", ReadiumJson.Options)!;
        Assert.Equal("hello", document.Guided[0].Description!.Plain);
        Assert.Equal("p", document.Guided[1].Description!.Plain);
        Assert.Equal("s", document.Guided[1].Description!.Ssml);
        Assert.Equal("en", document.Guided[1].Description!.Language);
    }

    /// <summary>Enums read and write their lowercase wire values.</summary>
    [Fact]
    public void EnumsReadAndWriteLowercaseWireValues()
    {
        Publication publication = JsonSerializer.Deserialize<Publication>("{\"metadata\":{\"title\":\"t\"},\"layout\":\"fixed\",\"readingProgression\":\"rtl\"}", ReadiumJson.Options)!;
        Assert.Equal(Layout.Fixed, publication.Layout);
        Assert.Equal(ReadingProgression.Rtl, publication.ReadingProgression);
        string json = JsonSerializer.Serialize(publication, ReadiumJson.Options);
        Assert.Contains("\"layout\":\"fixed\"", json);
        Assert.Contains("\"readingProgression\":\"rtl\"", json);
    }
}