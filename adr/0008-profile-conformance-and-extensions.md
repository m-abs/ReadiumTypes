# 0008. Profile conformance, schema extensions, and media-type constants

Status: Accepted

## Context

The webpub-manifest profiles (audiobook, divina, epub, pdf) are declared through
`metadata.@type` and `metadata.conformsTo`, neither of which was modeled, so profile
conformance round-tripped only through `ExtensionData`. The `schema/extensions/`
schemas were only partially modeled: encryption link properties and EPUB link
properties landed, but the EPUB metadata extension (`mediaOverlay.activeClass` /
`playbackActiveClass`) and the EPUB sub-collections (`pageList`, `landmarks`, `loa`,
`loi`, `lot`, `lov`) did not. `toc` from the core publication schema was also
missing. Consumers had no constants for the profile URIs or the profile/OPDS media
types and hardcoded strings instead.

## Decision

- Model `PublicationMetadata.Type` (`@type`) and `ConformsTo` (string-or-array via
  `StringOrArrayConverter`), plus the audiobook `abridged` boolean.
- Model the EPUB metadata extension as `EpubMediaOverlay` on `PublicationMetadata`,
  and the non-standard kebab-case `media-overlay` styling key emitted by Thorium
  as a separate `MediaOverlayStyling` record (
  `metadata-media-overlay.ts`; the earlier document-list shape of that key had no
  verifiable source and was removed; the overlay documents are `alternate` links
  of type `application/vnd.syncnarr+json` parsed as `MediaOverlayNode` trees —
  Readium's discarded pre-syncnarr `textref`/`audioref` syntax is not modeled).
- Add `toc` and the six EPUB sub-collection arrays of `Link` directly to
  `Publication`, each with a `ShouldSerialize` hook.
- Add `Profiles` and `MediaTypes` constant classes with the profile URIs, the
  JSON-LD context URI, and the media types defined by the profile and OPDS 2.0
  specs (plus the common image/audio types used in their examples).
- Add `Types` constants for the schema.org `@type` URIs used by the
  webpub-manifest and OPDS 2.0 examples, and `Rel` constants for the
  webpub-manifest relationship registry and the OPDS acquisition relations.

## Consequences

Profile-conformant manifests round-trip without relying on `ExtensionData`.
`conformsTo` always serializes as an array, which the schema accepts. The EPUB
sub-collections are typed on `Publication` rather than a separate extension record:
they are top-level publication keys, and a wrapper would add a nesting level the
wire format does not have.
