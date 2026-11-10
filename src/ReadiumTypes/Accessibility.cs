using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mabs.ReadiumTypes;

/// <summary>Accessibility information for a publication.</summary>
public record Accessibility : IExtensionDataHolder
{
    /// <summary>Human-readable summary of the accessibility features.</summary>
    [JsonPropertyName("summary")]
    public LocalizedString? Summary { get; set; }

    /// <summary>Accessibility features of the publication.</summary>
    [JsonPropertyName("features")]
    public List<string> Features { get; set; } = [];

    /// <summary>Accessibility hazards of the publication.</summary>
    [JsonPropertyName("hazards")]
    public List<string> Hazards { get; set; } = [];

    /// <summary>Media types the publication relies on for accessibility.</summary>
    [JsonPropertyName("accessMode")]
    public List<string> AccessMode { get; set; } = [];

    /// <summary>
    /// Single media type sufficient to access the publication, or a list of media types that
    /// together are sufficient.
    /// </summary>
    /// <remarks>
    /// shortcut: the schema also allows a nested array of sufficient combinations; a flat list is
    /// used here. Upgrade if consumers need combinations.
    /// </remarks>
    [JsonPropertyName("accessModeSufficient")]
    public List<string> AccessModeSufficient { get; set; } = [];

    /// <summary>Accessibility certifications of the publication.</summary>
    [JsonPropertyName("certifications")]
    public List<AccessibilityCertification> Certifications { get; set; } = [];

    /// <summary>Wire properties not described by the schema.</summary>
    [JsonExtensionData]
    public IDictionary<string, JsonElement>? ExtensionData { get; set; }

    /// <summary>Determines whether <see cref="Features"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Features"/> contains items.</returns>
    public bool ShouldSerializeFeatures()
    {
        return Features.Count > 0;
    }

    /// <summary>Determines whether <see cref="Hazards"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Hazards"/> contains items.</returns>
    public bool ShouldSerializeHazards()
    {
        return Hazards.Count > 0;
    }

    /// <summary>Determines whether <see cref="AccessMode"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="AccessMode"/> contains items.</returns>
    public bool ShouldSerializeAccessMode()
    {
        return AccessMode.Count > 0;
    }

    /// <summary>Determines whether <see cref="AccessModeSufficient"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="AccessModeSufficient"/> contains items.</returns>
    public bool ShouldSerializeAccessModeSufficient()
    {
        return AccessModeSufficient.Count > 0;
    }

    /// <summary>Determines whether <see cref="Certifications"/> is written; an empty collection is omitted.</summary>
    /// <returns><see langword="true"/> when <see cref="Certifications"/> contains items.</returns>
    public bool ShouldSerializeCertifications()
    {
        return Certifications.Count > 0;
    }
}