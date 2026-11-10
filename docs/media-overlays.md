# Media Overlays: two keys, two types

A publication manifest can carry media overlay information under two similarly
spelled metadata keys. They are unrelated on the wire and modeled by two
unrelated types; do not merge them.

| | `media-overlay` (kebab) | `mediaOverlay` (camel) |
|---|---|---|
| C# | `PublicationMetadata.MediaOverlay` (`MediaOverlayStyling`) | `PublicationMetadata.EpubMediaOverlay` (`EpubMediaOverlay`) |
| Wire | `{ "active-class": …, "playback-active-class": … }` | `{ "activeClass": …, "playbackActiveClass": … }` |
| Status | **Non-standard** — no schema, no profile defines it | Standard, schema-defined |
| Defined by | Thorium only: [metadata-media-overlay.ts](https://github.com/edrlab/thorium-reader/blob/03d89b20c894273f78f210d593c75e869dc2ec82/src/r2-xxx-js/r2-shared-js/models/metadata-media-overlay.ts) | [EPUB profile §mediaOverlay](https://readium.org/webpub-manifest/profiles/epub#mediaoverlay), [JSON Schema](https://github.com/readium/webpub-manifest/blob/master/schema/extensions/epub/metadata.schema.json) |

Both keys can appear in the same manifest; `System.Text.Json` matches on the
exact `JsonPropertyName`, so they never collide. Prefer writing the standard
camelCase key; the kebab-case one is modeled only to round-trip Thorium's
manifests.

The overlay **documents** themselves are not metadata: they are separate
resources referenced by `alternate` links of type
`application/vnd.syncnarr+json` (`MediaTypes.SyncNarrJson`), parsed from EPUB 3
Media Overlays SMIL. A document is a `MediaOverlayNode` tree
(`text`/`audio`/`video`/`role`/`narration`), matching
[Thorium's media-overlay model](https://github.com/edrlab/thorium-reader/blob/03d89b20c894273f78f210d593c75e869dc2ec82/src/r2-xxx-js/r2-shared-js/models/media-overlay.ts).
Readium's earlier JSON syntax (`textref`/`audioref`/`children`,
[archived here](https://github.com/readium/readium.github.io/blob/774f85f3c5768f97973efb5cf2c9cd8ef94cefd3/_pages/technical/r2-media-overlays-syntax.md))
was discarded in favor of syncnarr and is not modeled.
