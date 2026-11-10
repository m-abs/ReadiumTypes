namespace Mabs.ReadiumTypes;

/// <summary>
/// Conformance URIs for the webpub-manifest profiles, for use in
/// <see cref="PublicationMetadata.ConformsTo"/>.
/// </summary>
public static class Profiles
{
    /// <summary>JSON-LD context URI used in <see cref="Publication.Context"/>.</summary>
    public const string Context = "http://readium.org/webpub-manifest/context.jsonld";

    /// <summary>URI identifying the Audiobook Profile.</summary>
    public const string Audiobook = "https://readium.org/webpub-manifest/profiles/audiobook";

    /// <summary>URI identifying the Divina Profile.</summary>
    public const string Divina = "https://readium.org/webpub-manifest/profiles/divina";

    /// <summary>URI identifying the EPUB Profile.</summary>
    public const string Epub = "https://readium.org/webpub-manifest/profiles/epub";

    /// <summary>URI identifying the PDF Profile.</summary>
    public const string Pdf = "https://readium.org/webpub-manifest/profiles/pdf";
}