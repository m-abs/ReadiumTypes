# 0002. AllowReadingFromString on ReadiumJson.Options

Status: Accepted

## Context

The official webpub-manifest fixture `CarrotAndPepper/manifest.json` writes numeric
link fields as strings (`"width": "992"`), although the link schema declares them
`integer`. Strict parsing rejects the fixture, so the round-trip tests could not
pass against the spec's own examples.

## Decision

Set `NumberHandling = JsonNumberHandling.AllowReadingFromString` on the shared
`ReadiumJson.Options`. Reading accepts string-encoded numbers for all numeric
properties; writing is unchanged (numbers are still emitted as JSON numbers).

## Consequences

`ReadiumJson.Options` tolerates string-encoded numerics anywhere, not just
`width`/`height` — more lenient than the schema. Consumers using bare
`JsonSerializer` options remain strict. An empty string deserializes to `null`
for nullable numerics (STJ behavior with this flag); non-numeric strings still
throw.
