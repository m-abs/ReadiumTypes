using System.Text.Json.Serialization;

using Mabs.ReadiumTypes.Utils;

namespace Mabs.ReadiumTypes.GuidedNavigation;

/// <summary>
/// Guided navigation text: a bare string on the wire or an object with plain text, SSML, and a
/// language tag. <see cref="NavigationTextConverter"/> reads both forms and writes the object form.
/// </summary>
/// <remarks>
/// shortcut: the converter drops unknown keys instead of capturing extension data; upgrade if
/// consumers need them.
/// </remarks>
[JsonConverter(typeof(NavigationTextConverter))]
public record NavigationText
{
    /// <summary>Plain text of the item.</summary>
    [JsonPropertyName("plain")]
    public string? Plain { get; set; }

    /// <summary>SSML markup of the item.</summary>
    [JsonPropertyName("ssml")]
    public string? Ssml { get; set; }

    /// <summary>BCP 47 language tag of the text.</summary>
    [JsonPropertyName("language")]
    public string? Language { get; set; }
}