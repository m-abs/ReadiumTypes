# AGENTS.md

## Project

C# records mirroring the JSON Schemas under `specs/**/schema/` (git submodules
— ground truth; run `git submodule update --init` if missing) for the Readium
Web Publication Manifest, Readium locators, Readium guided navigation, OPDS
2.0, and the OPDS drafts (Authentication 2.0, Progression, Profile).
System.Text.Json only. Namespaces: `Mabs.ReadiumTypes` (core + locator),
`Mabs.ReadiumTypes.Opds` (plus `.Opds.Authentication`, `.Opds.Progression`,
`.Opds.Profile`), `Mabs.ReadiumTypes.GuidedNavigation`.

## Commands

- `dotnet build` — must be **zero warnings**: StyleCop + IDE rules are enforced
  via `EnforceCodeStyleInBuild` and `.editorconfig`, the law.
- `dotnet test` — unit tests, including fixture round-trips from
  `specs/webpub-manifest/examples/`.
- `docfx docfx.json` — metadata + docs build into `_site/`.
- `bin/docfx.sh` — build + serve on port 8091.
- `dotnet format` — normalize style.

## Conventions

- Models are `public record` with `{ get; set; }` auto-properties and an XML
  doc comment on every public member.
- Central Package Management is on: `PackageReference` never carries a
  `Version` attribute (versions live in `Directory.Packages.props`).
- Consumers serialize with `ReadiumJson.Options`.
- Every optional collection property gets a
  `public bool ShouldSerialize{Prop}() => Prop.Count > 0;` method.
- Every object model has `[JsonExtensionData] public IDictionary<string,
  JsonElement>? ExtensionData { get; set; }`.
- Wire unions (string|array, one-or-array objects, language maps) are handled
  by the internal converters in `src/ReadiumTypes/Utils/` — add a converter
  there rather than ad-hoc `JsonElement` plumbing.

## ADRs

Every decision that changes the public API shape, the wire output form of a
converter, the namespace layout, or adds a dependency requires an ADR before
the code lands. Path: `adr/NNNN-kebab-case-title.md` where `NNNN` is the 4-digit
zero-padded sequential number (next free number). Template:

```markdown
# NNNN. Title

Status: Proposed | Accepted | Superseded by NNNN

## Context

## Decision

## Consequences
```

ADRs are contributor records, not user docs: `docfx.json` excludes `adr/**` and
`AGENTS.md`, so they never appear in `_site`.
