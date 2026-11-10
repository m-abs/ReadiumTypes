# 0004. Default language key `und` instead of empty string

Status: Accepted

## Context

`LocalizedString` stored the language-unspecified value under the empty-string key.
When other languages were present, the map form was written as
`{"": "Hello", "fr": "Bonjour"}`, whose `""` key fails the webpub
`language-map.schema.json` (keys must match the BCP 47 pattern,
`additionalProperties: false`). The invalid form is reachable from valid OPDS
array input such as `[{"value":"x"},{"value":"y","language":"fr"}]`.

## Decision

Use the BCP 47 subtag `und` (undetermined) as the key of the
language-unspecified value: in the `LocalizedString(string)` constructor, the
`Default` lookup, the OPDS object form without `language`, and the bare-string
write condition.

## Consequences

Default-plus-tagged values now serialize schema-valid as
`{"und": "Hello", "fr": "Bonjour"}`. A map whose only key is `und` writes back
as a bare string, matching the existing single-default behavior. The legacy
`""` key is not read — it was never schema-valid — so no migration is needed.
