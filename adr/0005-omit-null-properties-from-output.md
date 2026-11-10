# 0005. Omit null properties from serialized output

Status: Accepted

## Context

`ReadiumJson.Options` never set `DefaultIgnoreCondition`, so every optional
property was written as `null`: `new Link { Href = "a" }` serialized as
`{"href":"a","type":null,"title":null,...}`. The getting-started docs and the
resolver's `IsIgnored` both assume `WhenWritingNull`, and the nulls make output
fail validation against the mirrored schemas (e.g. `link.type` is typed
`string`).

## Decision

Set `DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull` on
`ReadiumJson.Options`. `ShouldSerializeJsonTypeInfoResolver` already applies
the condition inside its per-property delegate, because STJ codegen skips its
own ignore-condition check once a `ShouldSerialize` delegate is set.

## Consequences

Serialized output no longer contains `"prop":null` entries. Required empty
collections (`links` in OPDS feeds, `readingOrder` in publications) are still
written, as they are empty lists, not nulls. Consumers that relied on explicit
null keys see different bytes.
