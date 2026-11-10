# Readium & OPDS JSON types in C# (System.Text.Json)

## Context

New .NET library in `/home/mabs/project/mabs.dk/ReadiumTypes` implementing the C# data types for the Readium Web Publication Manifest, Readium locator, Readium guided navigation, OPDS 2.0, and OPDS drafts (authentication 2.0, progression, profile), with System.Text.Json serialization/deserialization. The normative input is the JSON Schema set under `./specs/**/schema/*.schema.json` (git submodules, already checked out). The repo skeleton exists: empty `ReadiumTypes.slnx`, `Directory.Build.props` (EnforceCodeStyleInBuild), `Directory.Packages.props` (CPM: StyleCop.Analyzers 1.3.177-beta, xunit 2.9.2, xunit.runner.visualstudio 2.8.2, Microsoft.NET.Test.Sdk 17.12.0), and a 20 KB `.editorconfig` already carrying the StyleCop severity block (lines ~411–450) — do not modify it. End state: `dotnet build` zero warnings, `dotnet test` green, docfx API site builds.

Scope confirmed with user: **all** JSON schemas in the submodules. OPDS 1.0–1.2 (RNC/Atom) are XML, not in scope.

Conventions copied from `/home/mabs/project/mabs.dk/AudioCppClient`: public `record` models with `{ get; set; }` + XML docs on every public member, `ShouldSerialize{Prop}()` methods, `internal static Json`-style options holder, StyleCop.Analyzers referenced per-project, docfx at repo root.

## Global modeling rules (apply to every type; no per-type decisions left)

- **R1 shape**: every model is a `public record` with `{ get; set; }` auto-properties, file-scoped namespaces, explicit `using`s (ImplicitUsings off). XML `<summary>` on every public type/member (SA1600 is warning; build must be clean).
- **R2 naming**: wire name → PascalCase property. Add `[JsonPropertyName("wire")]` ONLY where wire ≠ property name (`@context`→`Context`, `@type`→`Type`, `belongsTo`→`BelongsTo`, `tdm`→`Tdm`, `audioref`→`AudioRef`, `imgref`→`ImgRef`, `textref`→`TextRef`, `videoref`→`VideoRef`, `sortAs`→`SortAs`, `conformsTo`→`ConformsTo`, `storyArc`→`StoryArc`, `mediaOverlay`→`MediaOverlay`, `numberOfPages`→`NumberOfPages`, `readingProgression`→`ReadingProgression`, `itemsPerPage`→`ItemsPerPage`, `currentPage`→`CurrentPage`, `numberOfItems`→`NumberOfItems`, `indirectAcquisition`→`IndirectAcquisition`, `originalLength`→`OriginalLength`, `totalProgression`→`TotalProgression`, `pageList`→`PageList`, `loa`/`loi`/`lot`/`lov` keep names, `ssml`→`Ssml`).
- **R3 optionality**: schema `required` → non-nullable CLR type; else nullable. `string`→`string?`, `integer`→`int?`, `number`→`double?`, `boolean`→`bool?`, `format: date-time`/`date`→`DateTimeOffset?` (STJ parses both `date` and `date-time` into `DateTimeOffset`).
- **R4 collections**: every schema array → `List<T>` property initialized `= []` (never null). Add `public bool ShouldSerializeX() => X.Count > 0;` for every OPTIONAL collection (empty arrays are omitted on write, matching Readium's other implementations). Required collections (`readingOrder`, feed `links`, feed `metadata`, …) get no ShouldSerialize method — they always serialize.
- **R5 extension data**: every object model gets `[JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData { get; set; }` (all schemas allow additional properties; `language-map` is the only `additionalProperties: false`, and it is handled by a converter, not a model). Nested subcollections therefore stay raw `JsonElement` — `shortcut: raw JsonElement for nested subcollections, upgrade to typed Subcollection dict if consumers need typed access.`
- **R6 string-or-array** (`"type": ["string","array"]` — `@context`, `rel`, link `language`, metadata `language`, `conformsTo`, a11y `conformsTo`, contributor `role`): `List<string>` + `[JsonConverter(typeof(StringOrArrayConverter))]`. Read accepts both forms; write always emits an array (valid per schema).
- **R7 one-or-array-or-string objects** (contributor, subject, series, collection, volume, season, storyArc, article, chapter, episode, issue, periodical): property type `List<T>` + `[JsonConverter(typeof(OneOrArrayObjectConverter<T>))]`. Read normalizes: bare string → `new T { Name = value }`; bare number (issue) → `new T { Position = value }`; single object → one-element list; array of mixed strings/numbers/objects → per-item. Write always emits an array. The converter creates `T` via `new()` and sets `Name`/`Position` by reflection when the property exists — no marker interface.
- **R8 language maps / localized strings** (webpub `language-map.schema.json`; OPDS feed-metadata `title`/`subtitle` string|array|object): single type `LocalizedString` (R10) + `LocalizedStringConverter`, shared by both specs.
- **R9 enums**: `public enum` + `[JsonConverter(typeof(JsonStringEnumConverter))]` on the enum declaration + `[JsonPropertyName("wire")]` on members whose wire form differs. Enums only for small closed sets: `Layout { Fixed, Reflowable, Scrolled }` (metadata.layout), `ReadingProgression { Rtl, Ltr }` (metadata.readingProgression), `PagePosition { Left, Right, Center }` (link properties.page), `LinkLayout { Reflowable, Fixed }` (link properties.layout — distinct name to avoid clash with `Layout`), `TdmReservation { All, None }`, `AvailabilityState { Available, Unavailable, Reserved, Ready }`. Everything else with an `enum` in schema (a11y exemption/accessMode/accessModeSufficient/feature/hazard, currency, guided-navigation role, epub link `contains` values) stays `string`/`List<string>` — these lists grow with the spec and unknown values must not throw.
- **R10 `LocalizedString`** (`Mabs.ReadiumTypes`): sealed class wrapping `Dictionary<string, string>` (BCP-47 tag → value; empty string `""` key = unspecified/default language). API: `LocalizedString()`, `LocalizedString(string value)` (stores under `""`), `this[string language]` get/set, `int Count`, `string? Default` (value for `""` else first value), `IEnumerable<KeyValuePair<string,string>>` implementation, `Equals`/`GetHashCode`/`ToString()` (returns `Default`). `public static implicit operator LocalizedString(string value)`. `LocalizedStringConverter : JsonConverter<LocalizedString?>` handles ALL wire forms: bare string; webpub map `{"en": "…", "fr": "…"}`; OPDS object `{"value": "…", "language": "en"}`; OPDS array `[{"value": "…", "language": "en"}, …]`. Write: single entry with key `""` → bare string; otherwise the webpub map form (valid for both specs). OPDS forms are detected by the presence of a `"value"` key.
- **R11 serializer entry point**: `public static class ReadiumJson` in `Mabs.ReadiumTypes` with `public static JsonSerializerOptions Options { get; }` = `{ DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, TypeInfoResolver = new ShouldSerializeJsonTypeInfoResolver() }`. Consumers must use `ReadiumJson.Options` for the ShouldSerialize behavior; plain `JsonSerializer` still round-trips (converters are attribute-attached).
- **R12 no validation**: no data-annotation or manual validation; models are wire mirrors. Schema-level constraints (`minItems`, `contains`, `uniqueItems`, `anyOf`-required alternates) are NOT enforced in code.

## Approach

### Step 1 — project scaffolding

- `src/ReadiumTypes/ReadiumTypes.csproj`: `Microsoft.NET.Sdk`, `<TargetFramework>net10.0</TargetFramework>`, `<RootNamespace>Mabs.ReadiumTypes</RootNamespace>`, `<AssemblyName>ReadiumTypes</AssemblyName>`, `<Version>0.1.0</Version>`, `<IsPackable>true</IsPackable>`, `<PackageId>Mabs.ReadiumTypes</PackageId>`, `<PackageLicenseExpression>MIT</PackageLicenseExpression>`, `<Authors>Morten Sjøgren</Authors>`, `<Description>Readium and OPDS JSON data types for System.Text.Json.</Description>`, `<GenerateDocumentationFile>true</GenerateDocumentationFile>`, `<Nullable>enable</Nullable>`, `<ImplicitUsings>false</ImplicitUsings>`, `<LangVersion>latest</LangVersion>`. ItemGroup: `<PackageReference Include="StyleCop.Analyzers" />` — NO Version attribute (CPM supplies 1.3.177-beta; the IncludeAssets/PrivateAssets metadata lives in Directory.Packages.props already). Rival pattern to avoid: AudioCppClient's csproj pins versions inline — this repo uses CPM.
- `tests/ReadiumTypes.Tests/ReadiumTypes.Tests.csproj`: same TFM/nullable/langversion, `<IsPackable>false</IsPackable>`, `PackageReference`s (no Version): `Microsoft.NET.Test.Sdk`, `xunit`, `xunit.runner.visualstudio`, `StyleCop.Analyzers`; `ProjectReference` to `../../src/ReadiumTypes/ReadiumTypes.csproj`. Plus fixture copy: `<Content Include="..\..\specs\webpub-manifest\examples\Flatland\manifest.json" Link="fixtures\flatland-manifest.json"><CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory></Content>`.
- Add both projects to `ReadiumTypes.slnx` (currently `<Solution></Solution>`): `<Project Path="src/ReadiumTypes/ReadiumTypes.csproj" />` and `<Project Path="tests/ReadiumTypes.Tests/ReadiumTypes.Tests.csproj" />`.
- Build now to prove CPM + analyzer wiring: `dotnet build` (zero warnings expected; nothing but csproj files yet).

### Step 2 — Utils: resolver + converters (`src/ReadiumTypes/Utils/`)

- Copy `/home/mabs/project/mabs.dk/AudioCppClient/src/AudioCpp/Utils/ShouldSerializeJsonTypeInfoResolver.cs` verbatim into `src/ReadiumTypes/Utils/ShouldSerializeJsonTypeInfoResolver.cs`; change ONLY the namespace to `Mabs.ReadiumTypes.Utils`. Keep `internal sealed`.
- `src/ReadiumTypes/Utils/StringOrArrayConverter.cs`: `internal sealed class StringOrArrayConverter : JsonConverter<List<string>>` per R6 (read string→one-element list, array→list; write array; throw `JsonException` on other token kinds).
- `src/ReadiumTypes/Utils/OneOrArrayObjectConverter.cs`: `internal sealed class OneOrArrayObjectConverter<T> : JsonConverter<List<T>> where T : class, new()` per R7.
- `src/ReadiumTypes/Utils/LocalizedStringConverter.cs`: `internal sealed class LocalizedStringConverter : JsonConverter<LocalizedString?>` per R10.
- `src/ReadiumTypes/Utils/AltIdentifierConverter.cs`: `internal sealed class AltIdentifierConverter : JsonConverter<List<AltIdentifier>>` — read array of (string → `new AltIdentifier { Value = s }` | object); write array of objects.
- `src/ReadiumTypes/Utils/NavigationTextConverter.cs`: `internal sealed class NavigationTextConverter : JsonConverter<NavigationText>` — read bare string → `new NavigationText { Plain = s }`; object → properties. Write object form.

### Step 3 — webpub core (`src/ReadiumTypes/`, namespace `Mabs.ReadiumTypes`)

Mirror `specs/webpub-manifest/schema/` file-by-file:

- `LocalizedString.cs` (R10).
- `Link.cs` ← link.schema.json: `Href` (required string), `Type`, `Title` string?, `Templated` bool?, `Rel` List<string> (R6), `Properties` LinkProperties?, `Height`/`Width`/`Size` int?, `Bitrate`/`Duration` double?, `Language` List<string> (R6), `Alternate`/`Children` List<Link> (optional, R4), R5.
- `LinkProperties.cs` ← union of webpub `properties` + `extensions/epub/properties.schema.json` + `extensions/encryption/properties.schema.json` + `opds-specs/schema/properties.schema.json` (the schema allOf-composes these into one object) + W3C synchronized-narration media overlay (not in any schema — see `MediaOverlayProperties.cs`): `Page` PagePosition?, `Contains` List<string>?, `Layout` LinkLayout?, `Encrypted` EncryptedProperties?, `MediaOverlay` MediaOverlayProperties? (`[JsonPropertyName("media-overlay")]`), `NumberOfItems` int?, `Price` Price?, `IndirectAcquisition` List<AcquisitionObject>?, `Holds` Holds?, `Copies` Copies?, `Availability` Availability?, R5.
- `EncryptedProperties.cs` ← encryption extension: `Algorithm` (required string), `Compression` string?, `OriginalLength` int?, `Profile` string?, `Scheme` string?, R5.
- `AltIdentifier.cs`: `Value` (required string), `Scheme` string?.
- `Contributor.cs` ← contributor.schema.json `$defs.object`: `Name` LocalizedString (required), `Identifier` string?, `AltIdentifier` List<AltIdentifier> (AltIdentifierConverter, optional), `SortAs` LocalizedString?, `Role` List<string> (R6), `Links` List<Link>, R5. Standalone record — NOT part of the CollectionObject base (it has `role`, no `position`).
- `Subject.cs` ← subject.schema.json: `Name` (required), `SortAs`, `Code` string?, `Scheme` string?, `Links`, R5.
- `CollectionObject.cs`: base record with `Name` LocalizedString?, `Identifier` string?, `AltIdentifier` List<AltIdentifier>?, `SortAs` LocalizedString?, `Position` double?, `Links` List<Link>, R5. Derived records (same namespace, each adds only its child properties, all child properties `List<T>` + `OneOrArrayObjectConverter<T>` + R4):
  - `Series` ← series.schema.json: `Chapter`, `Episode`, `Issue`, `Season`, `StoryArc`, `Volume`.
  - `Collection` ← collection.schema.json: none.
  - `Volume` ← volume.schema.json: `Chapter`, `Issue`, `StoryArc`.
  - `Season` ← season.schema.json: `Episode`.
  - `StoryArc` ← storyArc.schema.json: `Chapter`, `Episode`, `Issue`.
  - `Article` ← article.schema.json: `Author`, `Translator`, `Editor`, `Artist`, `Illustrator`, `Contributor` (all `List<Contributor>`), `Description` string?, `Position` (already on base) — read the schema's full `$defs.object` for the complete child list including `Chapter`.
  - `Chapter` ← chapter.schema.json: `Series` List<Series>.
  - `Episode` ← episode.schema.json: none.
  - `Issue` ← issue.schema.json: `Article`, `Chapter`.
  - `Periodical` ← periodical.schema.json: `Issue`, `Volume`.
- `Accessibility.cs` ← a11y.schema.json: `ConformsTo` List<string> (R6), `Exemption` string?, `AccessMode`/`AccessModeSufficient`/`Feature`/`Hazard` List<string>?, `Certification` AccessibilityCertification?, `Summary` string?, R5. `AccessibilityCertification.cs` ← the `certification` object (read schema: `certifier` string-or-array, `certifiedBy` string) per R3/R6.
- `MediaOverlay.cs` ← epub metadata extension: `ActiveClass` string?, `PlaybackActiveClass` string?, R5. (This is the metadata-level `mediaOverlay` object; the link-level media overlay is a different shape — next two entries.)
- `MediaOverlayProperties.cs` ← W3C synchronized narration (`https://w3c.github.io/sync-media-pub/synchronized-narration.html#properties`; wire form confirmed against Thorium's parser `https://github.com/edrlab/thorium-reader/blob/03d89b20c894273f78f210d593c75e869dc2ec82/src/r2-xxx-js/r2-shared-js/models/media-overlay.ts#L92-L109` — the value of `link.properties["media-overlay"]`): `Duration` double?, `Nodes` List<MediaOverlayNode>? (optional, R4), R5.
- `MediaOverlayNode.cs` ← same sources (Thorium `MediaOverlayNode`): `Text` string?, `Audio` string?, `Video` string?, `Role` List<string>? (R6 — spec says array, Thorium's converter tolerates single string), `Narration` List<MediaOverlayNode>? (nested child nodes, wire `narration`, optional R4), R5. All fields optional: a background-music node carries only `audio` + `role`.
- `Tdm.cs`: `Reservation` TdmReservation (required), `Policy` string?, R5.
- `MetadataBelongsTo.cs`: `Collection` List<Collection>?, `Journal`/`Magazine`/`Newspaper`/`Periodical` List<Periodical>?, `Season` List<Season>?, `Series` List<Series>?, `StoryArc` List<StoryArc>?, `Volume` List<Volume>? — all OneOrArrayObjectConverter, R4, R5.
- `MetadataContains.cs`: `Article`, `Chapter`, `Episode`, `Issue`, `Season`, `Series`, `StoryArc`, `Volume` — same pattern, R4, R5.
- `PublicationMetadata.cs` ← metadata.schema.json (+ epub extension): `Type` List<string>? (`@type`), `ConformsTo` List<string>? (R6), `Title` LocalizedString (required), `SortAs`/`Subtitle` LocalizedString?, `Identifier` string?, `AltIdentifier` List<AltIdentifier>?, `Accessibility` Accessibility?, `Modified`/`Published` DateTimeOffset?, `Language` List<string>? (R6), `Author`/`Translator`/`Editor`/`Artist`/`Illustrator`/`Letterer`/`Penciler`/`Colorist`/`Inker`/`Narrator`/`Contributor`/`Publisher`/`Imprint` all `List<Contributor>?` (OneOrArrayObjectConverter), `Subject` List<Subject>?, `Layout` Layout?, `ReadingProgression` ReadingProgression?, `Description` string?, `Duration` double?, `NumberOfPages` int?, `BelongsTo` MetadataBelongsTo?, `Contains` MetadataContains?, `Tdm` Tdm?, `MediaOverlay` MediaOverlay?, R5.
- `Publication.cs` ← publication.schema.json (+ epub subcollections extension): `Context` List<string>? (R6, `@context`), `Metadata` PublicationMetadata (required), `Links`/`Resources`/`Toc` List<Link> (optional), `ReadingOrder` List<Link> (required, no ShouldSerialize), `PageList`/`Landmarks`/`Loa`/`Loi`/`Lot`/`Lov` List<Link> (optional, from epub subcollections extension), R5.
- `Enums.cs`: `Layout`, `ReadingProgression`, `PagePosition`, `LinkLayout`, `TdmReservation` per R9 (each member gets `[JsonPropertyName]` where wire ≠ lowercased name — for these enums the camelCase wire forms (`reflowable`, `scrolled`, …) match `JsonStringEnumConverter` default case-insensitive read; still pin `[JsonPropertyName]` on members to guarantee write form).
- `Locator.cs` ← `specs/readium-architecture/schema/locator.schema.json`: `Href` (required), `Type` (required), `Title` string?, `Locations` LocatorLocations?, `Text` LocatorText?, R5. `LocatorLocations.cs`: `Fragments` List<string>?, `Progression` double?, `Position` int?, `TotalProgression` double?, R5. `LocatorText.cs`: `Before`/`Highlight`/`After` string?, R5.

### Step 4 — OPDS (`src/ReadiumTypes/Opds/`, namespace `Mabs.ReadiumTypes.Opds`)

- `Feed.cs` ← feed.schema.json: `Metadata` FeedMetadata (required), `Links` List<Link> (required), `Publications` List<OpdsPublication>?, `Navigation` List<Link>?, `Facets`/`Groups` List<FeedGroup>?, R5.
- `FeedMetadata.cs` ← feed-metadata.schema.json: `Identifier` string?, `Type` string? (`@type`), `Title` LocalizedString (required, LocalizedStringConverter handles string|array|object), `Subtitle` LocalizedString?, `Modified` DateTimeOffset?, `Description` string?, `ItemsPerPage`/`CurrentPage`/`NumberOfItems` int?, R5.
- `FeedGroup.cs` ← feed schema `facets.items` and `groups.items` (one type covers both): `Metadata` FeedMetadata?, `Links` List<Link>?, `Publications` List<OpdsPublication>? (present only in groups — confirm against `feed.schema.json` groups block when editing), R5.
- `OpdsPublication.cs` ← opds-specs publication.schema.json: `Metadata` PublicationMetadata (required), `Links` List<Link> (required), `Images` List<Link>?, R5.
- `AcquisitionObject.cs`: `Type` string (required), `Child` List<AcquisitionObject>? (optional, R4), R5.
- `Price.cs`: `Value` double (required), `Currency` string (required — ISO-4217 kept as string per R9).
- `Holds.cs`: `Total` int?, `Position` int?, R5. `Copies.cs`: `Total` int?, `Available` int?, R5.
- `Availability.cs`: `State` AvailabilityState (required), `Since`/`Until` DateTimeOffset? (schema allows `date` or `date-time`), R5. `Enums.cs`: `AvailabilityState` per R9.

### Step 5 — guided navigation + OPDS drafts

- `src/ReadiumTypes/GuidedNavigation/` (namespace `Mabs.ReadiumTypes.GuidedNavigation`):
  - `NavigationDocument.cs` ← document.schema.json: `Guided` List<GuidedNavigationItem> (required).
  - `GuidedNavigationItem.cs`: `Id` string?, `AudioRef`/`ImgRef`/`TextRef`/`VideoRef` string?, `Text` NavigationText?, `Roles` List<string>? (`role`, always-array per schema — no R6 converter needed, plain List<string>), `Children` List<GuidedNavigationItem>?, `Description` GuidedNavigationDescription?, R5.
  - `GuidedNavigationDescription.cs` ← description.schema.json: `AudioRef`/`ImgRef`/`TextRef`/`VideoRef` string?, `Text` NavigationText?, R5.
  - `NavigationText.cs` ← text.schema.json: `Plain` string?, `Ssml` string?, `Language` string?; record annotated `[JsonConverter(typeof(NavigationTextConverter))]` (string|object form).
- `src/ReadiumTypes/Opds/Authentication/` (namespace `Mabs.ReadiumTypes.Opds.Authentication`):
  - `AuthenticationDocument.cs` ← authentication.schema.json: `Title` string (required), `Id` string (required, wire `id`), `Description` string?, `Links` List<Link>?, `Authentication` List<AuthenticationFlow> (required).
  - `AuthenticationFlow.cs`: `Type` string (required), `Links` List<Link>?, `Labels` AuthenticationLabels?.
  - `AuthenticationLabels.cs`: `Login` string?, `Password` string?.
- `src/ReadiumTypes/Opds/Progression/` (namespace `Mabs.ReadiumTypes.Opds.Progression`):
  - `ProgressionDocument.cs` ← progression.schema.json: `Title` string?, `Modified` DateTimeOffset? (required), `Device` ProgressionDevice (required), `Progression` double (required), `References` List<string>?.
  - `ProgressionDevice.cs`: `Id` string (required), `Name` string (required).
- `src/ReadiumTypes/Opds/Profile/` (namespace `Mabs.ReadiumTypes.Opds.Profile`):
  - `ProfileDocument.cs` ← profile.schema.json (OPDS user profile): `Name` string?, `Email` string?, `Links` List<Link>?, `Loans` ProfileQuota?, `Holds` ProfileQuota? (no required fields), R5.
  - `ProfileQuota.cs`: `Total` int?, `Available` int?, R5.
- `src/ReadiumTypes/ReadiumJson.cs` (namespace `Mabs.ReadiumTypes`): per R11.

### Step 6 — tests (`tests/ReadiumTypes.Tests/`)

- `RoundtripTests.cs`: load `fixtures/flatland-manifest.json` (copied Flatland example), `JsonSerializer.Deserialize<Publication>(json, ReadiumJson.Options)` → assert non-null `Metadata.Title.Default == "Flatland"`, `ReadingOrder` non-empty, `Metadata.Author` normalized from the single-object form to one-element list. Re-serialize → re-deserialize → `JsonNode.Parse(a)!.DeepEquals(JsonNode.Parse(b)!)` (`.NET 9+ JsonNode.DeepEquals`). Repeat for `specs/webpub-manifest/examples/MobyDick/manifest.json` and `CarrotAndPepper/manifest.json` (add both as Content items in the test csproj; CarrotAndPepper exercises `layout`/page-list style EPUB fields).
- `ConverterTests.cs`:
  - `StringOrArrayConverter`: input `{"rel":"self"}` → 1-element list; input `{"rel":["a","b"]}` → 2-element; serialize → array form.
  - `OneOrArrayObjectConverter<Contributor>`: `"author": "H. G. Wells"` → 1 Contributor with Name.Default; `"author": ["A", {"name":"B"}]` → 2 entries.
  - `OneOrArrayObjectConverter<Issue>`: `"issue": 3` → 1 Issue with Position 3.
  - `LocalizedStringConverter`: `"title": "Flatland"`, `"title": {"en":"Flatland","fr":"Platland"}`, OPDS `"title": [{"value":"x","language":"en"}]` all parse; single-default writes bare string.
  - `NavigationTextConverter`: `"text": "hello"` → `Plain == "hello"`.
  - `AltIdentifierConverter`: mixed string/object array.
- `ShouldSerializeTests.cs`: `Publication` with empty `Resources`/`Links`/`Toc` → serialized JSON via `ReadiumJson.Options` contains no `"resources"`/`"links"`/`"toc"` keys but DOES contain `"readingOrder":[]`; non-empty `Links` serializes. Also: `null` properties omitted (WhenWritingNull).
- `ExtensionDataTests.cs`: manifest with unknown top-level key (e.g. `"custom": {...}`) → `ExtensionData` captures it; re-serialize → key present (proves round-trip through R5).
- `OpdsFeedTests.cs`: hand-written minimal feed JSON (metadata.title string, links with `rel:["self"]`, one publication with acquisition link + properties.price + properties.availability) → deserialize → assert typed values → serialize → DeepEquals round-trip.
- `MediaOverlayTests.cs`: hand-written link JSON `"properties": {"media-overlay": {"duration": 30.0, "nodes": [{"text": "p1", "audio": "p1.mp3", "role": ["background-music"], "narration": [{"text": "p1.1", "audio": "p1.1.mp3"}]}]}}` → deserialize `Link` → assert `Properties.MediaOverlay.Duration == 30.0`, one node with `Text == "p1"`, `Role` one-element, one nested `Narration` node → serialize → DeepEquals round-trip. Also `"role": "background-music"` (bare string) parses to one-element list.

### Step 7 — docfx

- `docfx.json` at repo root: copy `/home/mabs/project/mabs.dk/AudioCppClient/docfx.json`; change `globalMetadata` `_appName`/`_appTitle` to `Mabs.ReadiumTypes`; in `build.content.exclude` add `"specs/**"`, `"api/**"`, `"_site/**"`, `"adr/**"`, `"AGENTS.md"` (the `**/*.{md,yml}` glob would otherwise ingest every spec markdown, generated yml, and the contributor docs). Keep `disableGitFeatures`, templates `default`+`modern`, `pdf: true`.
- `index.md`: short landing page (what the library covers, link to `docs/getting-started.md`).
- `toc.yml`: `items: - name: Docs, href: docs/, - name: API, href: api/` (mirror AudioCppClient `toc.yml`).
- `docs/getting-started.md`: install, `ReadiumJson.Options`, deserialize Flatland snippet, ShouldSerialize$PropName note, `[JsonExtensionData]` note.
- `bin/docfx.sh`: copy AudioCppClient's, change port to `8091` (8090 belongs to AudioCppClient).

### Step 8 — `AGENTS.md` (repo root)

Write `AGENTS.md` for agents working in this repo. Sections and required facts (no other content):

- **Project**: C# records mirroring the JSON Schemas under `specs/**/schema/` (git submodules — ground truth; `git submodule update --init` if missing) for Readium webpub manifest, locator, guided navigation, OPDS 2.0, OPDS drafts; System.Text.Json only. Namespaces: `Mabs.ReadiumTypes` (core + locator), `.Opds` (+ `.Opds.Authentication`/`.Opds.Progression`/`.Opds.Profile`), `.GuidedNavigation`.
- **Commands**: `dotnet build` (must be zero warnings — StyleCop + IDE rules are enforced via `EnforceCodeStyleInBuild` and `.editorconfig`, the law), `dotnet test`, `docfx docfx.json` (metadata + build), `bin/docfx.sh` (build + serve on 8091). `dotnet format` to normalize.
- **Conventions**: models are `public record` with `{ get; set; }` + XML doc on every public member; CPM is on — `PackageReference` never carries a `Version` attribute; consumers serialize with `ReadiumJson.Options`; optional collections get `ShouldSerialize{Prop}()` returning `Count > 0`; every object model has `[JsonExtensionData]`; wire unions (string|array, one-or-array, language maps) are handled by the internal converters in `src/ReadiumTypes/Utils/` — add a converter there rather than ad-hoc `JsonElement` plumbing.
- **ADRs**: every decision that changes the public API shape, wire output form of a converter, namespace layout, or adds a dependency requires an ADR before the code lands. Path `adr/NNNN-kebab-case-title.md` (`NNNN` = 4-digit zero-padded sequential number, next free number); template inline in AGENTS.md: `# NNNN. Title` / `Status: Proposed|Accepted|Superseded by NNNN` / `## Context` / `## Decision` / `## Consequences`. ADRs are contributor records, not user docs: `docfx.json` excludes `adr/**` and `AGENTS.md`, so they never appear in `_site`.

## Critical files & anchors

- `/home/mabs/project/mabs.dk/AudioCppClient/src/AudioCpp/Utils/ShouldSerializeJsonTypeInfoResolver.cs` — verbatim source for Step 2; only the namespace line changes.
- `/home/mabs/project/mabs.dk/AudioCppClient/src/AudioCpp/Utils/Json.cs` — shows the options shape (`DefaultIgnoreCondition.WhenWritingNull` + resolver) that `ReadiumJson.Options` must copy.
- `specs/webpub-manifest/schema/metadata.schema.json` — largest model; `belongsTo`/`contains`/`tdm` blocks + epub `allOf` at the tail define the extension merge.
- `specs/webpub-manifest/schema/contributor.schema.json` — canonical "string | mixed-array | object" anyOf that `OneOrArrayObjectConverter<T>` implements.
- `specs/webpub-manifest/examples/Flatland/manifest.json` — round-trip fixture with single-object author, string `@context`, localized-free title: exercises R6/R7/R10 in one file.

## Verification

1. `dotnet build` from repo root → exit 0, **zero warnings** (StyleCop SA + IDE rules are warnings-as-noise via EnforceCodeStyleInBuild; any warning fails the bar).
2. `dotnet test` → all tests pass; the Flatland round-trip (deserialize → serialize → re-deserialize → `JsonNode.DeepEquals`) is the primary end-to-end proof.
3. Manual spot-check via `dotnet test --filter ShouldSerializeTests` — proves `ShouldSerialize$PropName` actually takes effect through the copied resolver (empty `resources` absent from output JSON).
4. `bash bin/docfx.sh` (build part; serve blocks — run `docfx` directly instead: `docfx docfx.json`) → `_site/api/index.html` exists and `api/Mabs.ReadiumTypes.Publication.yml` was generated (metadata step ran); `_site/AGENTS.html` and `_site/adr/` do NOT exist (proves the docfx excludes from Step 7/8 hold).

## Assumptions & contingencies

- PackageId `Mabs.ReadiumTypes`, Authors `Morten Sjøgren`, MIT — mirrors AudioCppClient packaging metadata; user can override.
- Single TFM `net10.0` (SDK 10.0.114 installed; AudioCppClient is net10.0). If the implementer later needs netstandard2.0 for Unity-style consumers, that's a separate ask.
- `specs/` submodules stay checked out; tests consume fixtures via csproj Content copy, not runtime path walking. If a submodule directory is absent at build time, the Content copy fails loudly — re-init submodules (`git submodule update --init`), do not delete the fixture reference.
- If a schema's elided detail differs from this plan's property list (article/issue child lists, a11y certification, feed groups items), the schema file is ground truth — mirror it; the modeling rules still apply unchanged.
- `JsonStringEnumConverter` + `[JsonPropertyName]` on enum members requires .NET 9+ (available on net10.0). If a member attribute is rejected, fall back to `new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)` registered in `ReadiumJson.Options` only for enums whose wire form is pure camelCase, and keep `[JsonPropertyName]` for the hyphenated one (`remote-resources` stays a string per R9, so no hyphenated enum member exists).
