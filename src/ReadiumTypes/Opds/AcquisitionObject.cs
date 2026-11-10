using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

using Mabs.ReadiumTypes.Utils;

namespace Mabs.ReadiumTypes.Opds;

/// <summary>A media type expected to be acquired after additional steps.</summary>
public record AcquisitionObject : IExtensionDataHolder
{
    /// <summary>MIME type of the indirect acquisition.</summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>Further media types acquired after this step.</summary>
    [JsonConverter(typeof(OneOrArrayObjectConverter<AcquisitionObject>))]
    [JsonPropertyName("child")]
    public List<AcquisitionObject> Child { get; set; } = [];

    /// <summary>Price of the indirect acquisition.</summary>
    [JsonPropertyName("price")]
    public Price? Price { get; set; }

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }

    /// <summary>Determines whether <see cref="Child"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Child"/> contains items.</returns>
    public bool ShouldSerializeChild()
    {
        return Child.Count > 0;
    }
}