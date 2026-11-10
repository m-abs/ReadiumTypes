using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes;

/// <summary>
/// Properties of a <see cref="Link"/>, merging the webpub, EPUB, encryption, and OPDS link
/// property schemas plus the W3C synchronized-narration media overlay.
/// </summary>
public record LinkProperties : IExtensionDataHolder
{
    /// <summary>How the linked resource should be displayed in a synthetic spread.</summary>
    [JsonPropertyName("page")]
    public PagePosition? Page { get; set; }

    /// <summary>Content contained in the linked resource that a media type cannot express.</summary>
    [JsonPropertyName("contains")]
    public List<string> Contains { get; set; } = [];

    /// <summary>Hint about the nature of the layout of the linked resource.</summary>
    [JsonPropertyName("layout")]
    public LinkLayout? Layout { get; set; }

    /// <summary>Encryption information for an encrypted or obfuscated resource.</summary>
    [JsonPropertyName("encrypted")]
    public EncryptedProperties? Encrypted { get; set; }

    /// <summary>Hint about the expected number of items returned.</summary>
    [JsonPropertyName("numberOfItems")]
    public int? NumberOfItems { get; set; }

    /// <summary>Price of the publication tied to its acquisition link.</summary>
    [JsonPropertyName("price")]
    public Opds.Price? Price { get; set; }

    /// <summary>Expected media types acquired after additional steps.</summary>
    [JsonPropertyName("indirectAcquisition")]
    public List<Opds.AcquisitionObject> IndirectAcquisition { get; set; } = [];

    /// <summary>Hold-list information for an unavailable book.</summary>
    [JsonPropertyName("holds")]
    public Opds.Holds? Holds { get; set; }

    /// <summary>Information about the copies a library has acquired.</summary>
    [JsonPropertyName("copies")]
    public Opds.Copies? Copies { get; set; }

    /// <summary>Availability of the resource.</summary>
    [JsonPropertyName("availability")]
    public Opds.Availability? Availability { get; set; }

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }

    /// <summary>Determines whether <see cref="Contains"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Contains"/> contains items.</returns>
    public bool ShouldSerializeContains()
    {
        return Contains.Count > 0;
    }

    /// <summary>Determines whether <see cref="IndirectAcquisition"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="IndirectAcquisition"/> contains items.</returns>
    public bool ShouldSerializeIndirectAcquisition()
    {
        return IndirectAcquisition.Count > 0;
    }
}