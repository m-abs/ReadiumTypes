# 0006. Retarget OPDS 2.0 models to the published OPDS 2.0 schema

Status: Accepted

## Context

The OPDS models in `Mabs.ReadiumTypes.Opds` (`Feed`, `FeedGroup`, `FeedMetadata`,
`OpdsPublication`) followed the early OPDS 2.0 forum-draft wire format:
`@context http://opds.io/feed/2.0`, top-level `items`/`totalItems`/`next`/`prev`/`first`/`last`,
and flat capitalized entry properties (`Series`, `Rels`, `Links`, `Duration`, `NumPages`,
`MediaOverlay`, `BelongsTo`, `Contains`). That shape appears nowhere in the ground-truth
submodule declared by `AGENTS.md` (`specs/opds-specs`); the `@context` URI is absent from it
entirely.

The submodule's root schemas are the published OPDS 2.0 (`opds-2.0.md` Appendix A):
`feed.schema.json` defines a feed as `metadata` + `links` plus the collections
`publications`, `navigation`, `facets`, and `groups` (pagination is expressed only through
`links` rels), `feed-metadata.schema.json` defines `identifier`/`@type`/`title`/`subtitle`/
`modified`/`description`/`itemsPerPage`/`currentPage`/`numberOfItems`, and
`publication.schema.json` defines an OPDS Publication as `metadata` (webpub
`metadata.schema.json`) + `links` + `images`.

## Decision

Retarget the models to the published schema, no compatibility shims:

- `Feed`: keeps `metadata`/`links`; gains `Publications`, `Navigation`, `Facets`; loses
  `Context`, `Items`, `TotalItems`, `Next`, `Prev`, `First`, `Last`.
- `FeedGroup`: `{metadata, links, publications?, navigation?}`; loses `id`/`title`.
- `FeedMetadata`: mirrors `feed-metadata.schema.json`; loses `language` (not in the schema).
- `OpdsPublication`: `{metadata, links, images}` reusing core `PublicationMetadata`; loses all
  flat draft properties.

`Facets` reuse `FeedGroup`: the facet group wire shape is identical minus
`publications`/`navigation`, which simply stay empty (and are omitted by
`ShouldSerialize*`).

## Consequences

- Breaking for any consumer of the draft shape. Acquisition metadata is unaffected: `price`,
  `availability`, `holds`, `copies`, and `indirectAcquisition` live on `LinkProperties`
  (`properties.schema.json`) and stay as-is.
- `OneOrArrayObjectConverter<Link>` loses its last consumer; the converter keeps its
  string/number-item rejection guard for future consumers, and the test that exercised it via
  `OpdsPublication.Image` is removed.
