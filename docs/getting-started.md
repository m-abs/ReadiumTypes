# Getting Started

## Install

```shell
dotnet add package Mabs.ReadiumTypes
```

## Serialize and deserialize

All types round-trip through `System.Text.Json`, but to get the
`ShouldSerialize{PropName}` behavior you must use the shared options:

```csharp
using Mabs.ReadiumTypes;

var json = File.ReadAllText("manifest.json");
var publication = JsonSerializer.Deserialize<Publication>(json, ReadiumJson.Options);

// Optional collections that are empty are omitted from the output.
var roundTrip = JsonSerializer.Serialize(publication, ReadiumJson.Options);
```

`ReadiumJson.Options` sets `DefaultIgnoreCondition.WhenWritingNull` and a
`ShouldSerializeJsonTypeInfoResolver`, which picks up
`public bool ShouldSerializeResources() => Resources.Count > 0;`-style methods
on the models — empty optional collections (e.g. `resources`) never appear in
the output. Plain `JsonSerializer.Serialize` without these options still
round-trips correctly; the wire unions (`string | array`, one-or-array objects,
language maps) are handled by converters attached with `[JsonConverter]`
attributes.

## Wire unions

- `"rel": "preview"` or `["a", "b"]` deserializes into `List<string>`.
- `"author": {"name": "…"}`, a bare string, or a mixed array normalizes into
  `List<Contributor>`.
- `"title": "Title"`, `{"en": "Title", "fr": "Titre"}` and the OPDS
  `{"value": "…", "language": "…"}` forms all deserialize into
  `LocalizedString`.

## Unknown properties

Every model carries

```csharp
[JsonExtensionData]
public IDictionary<string, JsonElement>? ExtensionData { get; set; }
```

so properties outside the schema survive a deserialize/serialize round-trip.
