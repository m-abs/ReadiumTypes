# 0009. Position list service model

Status: Accepted

## Context

The readium-architecture locators specification defines a position list service
exposed through a manifest link identified by the media type
`application/vnd.readium.position-list+json` and the relation
`http://readium.org/position-list`. Its document is `{ total, positions }` with
`positions` an array of Locator Objects. Unlike the core manifest, it has no JSON
Schema — the shape is defined in prose only.

## Decision

Model the document as `PositionList` (`Total` plus `Positions`, a `List<Locator>`
reusing the existing locator model) and add the service's media type and relation
constants (`MediaTypes.PositionListJson`, `Rel.PositionList`), along with the
locator resolver service constants from the same specification
(`MediaTypes.LocatorResolverJson`, `Rel.LocatorResolver`).

## Consequences

Position lists round-trip without hand-rolled `JsonElement` plumbing. The model
lives in the core `Mabs.ReadiumTypes` namespace next to `Locator` rather than a
sub-namespace: it is a single two-property document, and a namespace for one type
would be ceremony.

The README example uses a legacy singular `fragment` location key; the locator
JSON Schema defines `fragments` (array), which the model follows. While adding
the fields the example needs, `LocatorLocations` also gained the schema's
`progression`, `position` and `totalProgression`, which were missing.
