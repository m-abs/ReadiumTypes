# Mabs.ReadiumTypes

C# record types for the [Readium](https://readium.org) JSON formats, with
[System.Text.Json](https://learn.microsoft.com/dotnet/standard/serialization/system-text-json/overview)
serialization and deserialization:

- [Readium Web Publication Manifest](https://readium.org/webpub-manifest/) (publication metadata, links, collections)
- [Readium locators](https://readium.org/architecture/models/locators/) (CSS, DOM, media, part locators)
- [Readium guided navigation](https://readium.org/guided-navigation/) documents
- [OPDS 2.0](https://specs.opds.io/opds-2.0) (feeds, publications, groups, availability, prices)
- OPDS drafts: [Authentication](https://drafts.opds.io/authentication-for-opds-1.0.html), [Progression](https://drafts.opds.io/opds-progression-1.0.html), [Profile](https://drafts.opds.io/opds-user-profile-1.0.html)

The models mirror the published JSON Schemas of these specifications (kept as
git submodules under `specs/`) and are wire mirrors: no validation, unknown
properties preserved via `[JsonExtensionData]`.

## Usage

```csharp
using Mabs.ReadiumTypes;
using Mabs.ReadiumTypes.Opds;

// Deserialize an OPDS 2.0 feed
Feed feed = JsonSerializer.Deserialize<Feed>(json, ReadiumJson.Options)!;

// Serialize a web publication manifest
string manifest = JsonSerializer.Serialize(publication, ReadiumJson.Options);
```

Always round-trip through `ReadiumJson.Options` — it carries the type-modifier
resolver that honors the `ShouldSerialize{Prop}()` omission hooks and the null
omission condition.

## Building

Requires .NET 10. Clone with submodules (fixture round-trip tests read
`specs/webpub-manifest/examples/`):

```sh
git clone --recursive <repo-url>
# or: git submodule update --init
dotnet build   # must be zero warnings; StyleCop + IDE rules are enforced
dotnet test
docfx docfx.json   # API docs into _site/; bin/docfx.sh serves on :8091
```

## Conventions

- Models are `public record` with `{ get; set; }` auto-properties and XML doc
  comments on every public member.
- Optional collections are omitted when empty via `ShouldSerialize{Prop}()`.
- Wire unions (string|array, one-or-array objects, language maps) are handled
  by internal converters in `src/ReadiumTypes/Utils/`.
- Namespaces: `Mabs.ReadiumTypes` (core + locator), `Mabs.ReadiumTypes.Opds`
  (plus `.Opds.Authentication`, `.Opds.Progression`, `.Opds.Profile`),
  `Mabs.ReadiumTypes.GuidedNavigation`.
- API-shape and wire-format decisions are recorded in [`adr/`](adr/).

## Credits

This library was developed with a local coding agent driving the design
decisions, ADRs, and implementation; the schema ground truth and review remain
human.

## License

[MIT](LICENSE)
