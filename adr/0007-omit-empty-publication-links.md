# 0007. Omit empty `Publication.Links` from wire output

Status: Accepted

## Context

Every optional collection in the models is omitted when empty via a
`ShouldSerialize{Prop}()` method, but `Publication.Links` had none, so an empty publication
serialized as `"links":[]` — and `ShouldSerializeTests.EmptyOptionalCollectionsAreOmitted`
pinned that output. `links` is required by the webpub manifest schema, but `"links":[]` does
not satisfy consumers any better than an absent key: both are unusable as a link set, and the
repo convention is that empty collections are noise.

## Decision

Add `ShouldSerializeLinks()` to `Publication` so an empty `Links` list is omitted, matching
every other collection. `readingOrder` keeps its always-written form. `Feed.Links` and
`OpdsPublication.Links` (required, with schema constraints on their contents) are unchanged.

## Consequences

- Wire output of an empty `Publication` loses `"links":[]`; consumers that assumed the key is
  always present must handle its absence.
- Round-trip is unaffected: deserializing a publication without `links` yields an empty list
  again.
