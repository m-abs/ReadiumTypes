# 0001. Enum wire names via JsonStringEnumMemberName

Status: Accepted

## Context

The implementation plan (R9) specified `[JsonConverter(typeof(JsonStringEnumConverter))]`
on the enum declarations plus `[JsonPropertyName("wire")]` on members whose wire form
differs (lowercase, hyphenated: `reflowable`, `pre-paginated`, `vertical-rl`, …).

`System.Text.Json`'s `JsonStringEnumConverter` ignores `JsonPropertyName` on enum
members: it reads and writes the CLR member name, so `Reflowable` would be emitted
instead of `reflowable` and the schemas' enum values would not round-trip. This was
caught by failing tests.

## Decision

Annotate enum members with `[JsonStringEnumMemberName("wire")]` (available since
.NET 9; the library targets `net10.0`) and keep `[JsonConverter(typeof(JsonStringEnumConverter))]`
on the enum declaration. Affected enums: `Layout`, `LinkLayout`, `PagePosition`,
`ReadingProgression`, `TdmReservation`, `Opds.AvailabilityState`.

## Consequences

Wire output/input matches the schemas exactly, with no custom enum converters.
The library cannot target frameworks older than .NET 9 without replacing the
attribute. `dotnet_diagnostic` tooling and consumers reading with plain
`JsonSerializer` (default enum handling is numeric) still need
`JsonStringEnumConverter`, which the attribute on the declaration provides.
