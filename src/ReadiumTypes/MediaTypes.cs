namespace Mabs.ReadiumTypes;

/// <summary>
/// Media types defined or used by the Readium webpub-manifest profiles and OPDS 2.0,
/// for use in <see cref="Link.Type"/>.
/// </summary>
public static class MediaTypes
{
    /// <summary>Media type of a Readium Web Publication Manifest.</summary>
    public const string WebPubJson = "application/webpub+json";

    /// <summary>Media type of a packaged (directory-based) Readium Web Publication.</summary>
    public const string WebPubZip = "application/webpub+zip";

    /// <summary>Media type of an audiobook manifest.</summary>
    public const string AudiobookJson = "application/audiobook+json";

    /// <summary>Media type of an audiobook package.</summary>
    public const string AudiobookZip = "application/audiobook+zip";

    /// <summary>Media type of a Divina (visual narrative) manifest.</summary>
    public const string DivinaJson = "application/divina+json";

    /// <summary>Media type of a Divina (visual narrative) package.</summary>
    public const string DivinaZip = "application/divina+zip";

    /// <summary>Media type of an EPUB package.</summary>
    public const string EpubZip = "application/epub+zip";

    /// <summary>Media type of a PDF document.</summary>
    public const string Pdf = "application/pdf";

    /// <summary>Media type of an XHTML document, required by the EPUB profile reading order.</summary>
    public const string XhtmlXml = "application/xhtml+xml";

    /// <summary>Media type of an HTML document, recommended for OPDS navigation links.</summary>
    public const string TextHtml = "text/html";

    /// <summary>Media type of an OpenType font, used in encryption module examples.</summary>
    public const string OpenTypeFont = "application/vnd.ms-opentype";

    /// <summary>Media type of an OPDS 2.0 catalog feed.</summary>
    public const string OpdsJson = "application/opds+json";

    /// <summary>Media type of an OPDS 2.0 publication.</summary>
    public const string OpdsPublicationJson = "application/opds-publication+json";

    /// <summary>Media type of an OPDS Authentication 2.0 document.</summary>
    public const string OpdsAuthenticationJson = "application/opds-authentication+json";

    /// <summary>Media type of a synchronized-narration media overlay document.</summary>
    public const string SyncNarrJson = "application/vnd.syncnarr+json";

    /// <summary>Media type of a publication's position list service.</summary>
    public const string PositionListJson = "application/vnd.readium.position-list+json";

    /// <summary>Media type of a publication's locator resolver service.</summary>
    public const string LocatorResolverJson = "application/vnd.readium.locator-resolver+json";

    /// <summary>Media type of a JPEG image.</summary>
    public const string ImageJpeg = "image/jpeg";

    /// <summary>Media type of a PNG image.</summary>
    public const string ImagePng = "image/png";

    /// <summary>Media type of an AVIF image.</summary>
    public const string ImageAvif = "image/avif";

    /// <summary>Media type of an MP3 audio resource.</summary>
    public const string AudioMpeg = "audio/mpeg";

    /// <summary>Media type of an Ogg audio resource.</summary>
    public const string AudioOgg = "audio/ogg";
}