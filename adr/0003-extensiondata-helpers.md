# 0003. ExtensionData helpers via IExtensionDataHolder

Status: Accepted

## Context

Every object model carries `[JsonExtensionData] public IDictionary<string, JsonElement>? ExtensionData`, but reading and writing vendor properties is awkward: callers must null-check the dictionary, build `JsonElement`s by hand, and know the exact wire key. Consumers want `Add` (property-style name → `x-` kebab key), `AddRaw` (verbatim key), and `Get<T>` (both key formats, type casts).

## Decision

Add `public interface IExtensionDataHolder { IDictionary<string, JsonElement>? ExtensionData { get; set; } }` — matching the property every such model already declares — and implement it on all 39 models that declare `[JsonExtensionData]` (the ten `CollectionObject` subclasses inherit it). `Add`/`AddRaw`/`Get<T>` are static extension methods on the interface, not default interface members, because default interface members are not callable on the implementing class instance.

## Consequences

Public API grows: one interface, one static class, and `: IExtensionDataHolder` on every model's declaration (source-compatible; wire format unchanged). `Add("VarName", …)` writes `x-var-name`; names containing a hyphen are treated as exact keys. `Get<T>` tries the `x-` kebab key first, then the raw name, deserializes with `ReadiumJson.Options`, and falls back to `Convert.ChangeType` for primitive casts. Models without extension data (`NavigationText`) do not implement the interface.
