namespace Mabs.ReadiumTypes;

/// <summary>
/// Link relations registered by the webpub-manifest relationships registry and
/// OPDS 2.0, for use in <see cref="Link.Rel"/>.
/// </summary>
public static class Rel
{
    /// <summary>Designates a substitute for the link's context.</summary>
    public const string Alternate = "alternate";

    /// <summary>Refers to a table of contents.</summary>
    public const string Contents = "contents";

    /// <summary>Refers to a publication's cover.</summary>
    public const string Cover = "cover";

    /// <summary>Refers to the first resource in a collection of resources.</summary>
    public const string First = "first";

    /// <summary>Refers to the last resource in a collection of resources.</summary>
    public const string Last = "last";

    /// <summary>Links to a manifest.</summary>
    public const string Manifest = "manifest";

    /// <summary>Refers to a URI or templated URI that will perform a search.</summary>
    public const string Search = "search";

    /// <summary>Conveys an identifier for the link's context.</summary>
    public const string Self = "self";

    /// <summary>Refers to the first resource that's part of the main body of the publication.</summary>
    public const string Start = "start";

    /// <summary>Indicates that a sub-set of the full publication is freely accessible.</summary>
    public const string Preview = "preview";

    /// <summary>Links to a publication's position list service.</summary>
    public const string PositionList = "http://readium.org/position-list";

    /// <summary>Links to a publication's locator resolver service.</summary>
    public const string LocatorResolver = "http://readium.org/locator-resolver";

    /// <summary>Fallback acquisition relation when no other relation fits the transaction.</summary>
    public const string Acquisition = "http://opds-spec.org/acquisition";

    /// <summary>Publication is freely accessible without any requirement, including authentication.</summary>
    public const string AcquisitionOpenAccess = "http://opds-spec.org/acquisition/open-access";

    /// <summary>Publication can be borrowed for a limited period of time.</summary>
    public const string AcquisitionBorrow = "http://opds-spec.org/acquisition/borrow";

    /// <summary>Publication can be purchased for a given price.</summary>
    public const string AcquisitionBuy = "http://opds-spec.org/acquisition/buy";

    /// <summary>A sub-set of the full publication is freely accessible at a given URI.</summary>
    public const string AcquisitionSample = "http://opds-spec.org/acquisition/sample";

    /// <summary>Publication can be subscribed to, usually as part of a purchase.</summary>
    public const string AcquisitionSubscribe = "http://opds-spec.org/acquisition/subscribe";
}