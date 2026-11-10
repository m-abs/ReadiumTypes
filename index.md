# Mabs.ReadiumTypes

C# record types for the [Readium](https://readium.org) JSON formats, with
[System.Text.Json](https://learn.microsoft.com/dotnet/standard/serialization/system-text-json/overview)
serialization and deserialization:

- [Readium Web Publication Manifest](https://readium.org/webpub-manifest/) (publication metadata, links, collections)
- [Readium locators](https://readium.org/architecture/models/locators/) (CSS, DOM, media, part locators)
- [Readium guided navigation](https://readium.org/guided-navigation/) documents
- [OPDS 2.0](https://specs.opds.io/opds-2.0) (feeds, publications, groups, availability, prices)
- OPDS drafts: [Authentication](https://drafts.opds.io/authentication-for-opds-1.0.html), [Progression](https://drafts.opds.io/opds-progression-1.0.html), [Profile](https://drafts.opds.io/opds-user-profile-1.0.html)

The models mirror the published JSON Schemas of these specifications and are
wire mirrors: no validation, unknown properties preserved via
`[JsonExtensionData]`.

Start with [Getting Started](docs/getting-started.md), or browse the
[API reference](api/Mabs.ReadiumTypes.yml).
