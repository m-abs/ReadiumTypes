using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Mabs.ReadiumTypes;

/// <summary>
/// Get/set helpers for the <c>[JsonExtensionData]</c> dictionary of an
/// <see cref="IExtensionDataHolder"/>.
/// </summary>
public static class ExtensionDataExtensions
{
    private static readonly Regex UpperCaseBoundary = new("(?<=[a-z0-9])[A-Z]", RegexOptions.Compiled, TimeSpan.FromSeconds(1));

    /// <summary>
    /// Stores <paramref name="value"/> under an extension key derived from
    /// <paramref name="name"/>: PascalCase becomes kebab-case with an <c>x-</c> prefix
    /// (<c>VarName</c> → <c>x-var-name</c>). A name that already contains a hyphen is
    /// used as is.
    /// </summary>
    /// <param name="holder">The model to store on.</param>
    /// <param name="name">Property-style name, e.g. <c>VarName</c>.</param>
    /// <param name="value">Value to store, serialized like every other property.</param>
    /// <returns><paramref name="holder"/> for chaining.</returns>
    public static IExtensionDataHolder Add(this IExtensionDataHolder holder, string name, object? value)
    {
        holder.ExtensionData ??= new Dictionary<string, JsonElement>();
        holder.ExtensionData[ToVarKey(name)] = JsonSerializer.SerializeToElement(value, ReadiumJson.Options);
        return holder;
    }

    /// <summary>Stores <paramref name="value"/> under <paramref name="key"/> verbatim.</summary>
    /// <param name="holder">The model to store on.</param>
    /// <param name="key">Exact extension key, e.g. <c>vendor:key</c>.</param>
    /// <param name="value">Value to store, serialized like every other property.</param>
    /// <returns><paramref name="holder"/> for chaining.</returns>
    public static IExtensionDataHolder AddRaw(this IExtensionDataHolder holder, string key, object? value)
    {
        holder.ExtensionData ??= new Dictionary<string, JsonElement>();
        holder.ExtensionData[key] = JsonSerializer.SerializeToElement(value, ReadiumJson.Options);
        return holder;
    }

    /// <summary>
    /// Reads an extension value. <paramref name="name"/> is tried both as the
    /// <c>x-</c> kebab-case key produced by <see cref="Add"/> and verbatim, in that
    /// order. Primitive casts (e.g. stored <c>1</c> as <see cref="double"/> or
    /// <c>"42"</c> as <see cref="int"/>) are applied; complex values are deserialized
    /// with <see cref="ReadiumJson.Options"/>.
    /// </summary>
    /// <typeparam name="T">CLR type to read the value as.</typeparam>
    /// <param name="holder">The model to read from.</param>
    /// <param name="name">Property-style name (<c>VarName</c>) or exact extension key.</param>
    /// <returns>The value, or <c>default</c> when the key is absent or null.</returns>
    public static T? Get<T>(this IExtensionDataHolder holder, string name)
    {
        if (holder.ExtensionData is not { } data)
        {
            return default;
        }

        string varKey = ToVarKey(name);
        if (!data.TryGetValue(varKey, out JsonElement element) && !data.TryGetValue(name, out element))
        {
            return default;
        }

        if (element.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
        {
            return default;
        }

        if (typeof(T) == typeof(JsonElement))
        {
            return (T)(object)element;
        }

        try
        {
            return element.Deserialize<T>(ReadiumJson.Options);
        }
        catch (JsonException)
        {
            return Cast<T>(element);
        }
    }

    private static string ToVarKey(string name)
    {
        // shortcut: names already containing a hyphen are treated as exact keys,
        // upgrade when vendor keys with underscores need the x- treatment too.
        if (name.Contains('-', StringComparison.Ordinal))
        {
            return name;
        }

        return "x-" + UpperCaseBoundary.Replace(name, m => "-" + m.Value).ToLowerInvariant();
    }

    private static T? Cast<T>(JsonElement element)
        => element.ValueKind switch
        {
            JsonValueKind.String => (T?)Convert.ChangeType(element.GetString(), UnderlyingType<T>(), CultureInfo.InvariantCulture),
            JsonValueKind.True or JsonValueKind.False or JsonValueKind.Number => (T?)Convert.ChangeType(element.GetRawText(), UnderlyingType<T>(), CultureInfo.InvariantCulture),
            _ => throw new InvalidCastException($"Cannot cast {element.ValueKind} JSON to {typeof(T)}."),
        };

    private static Type UnderlyingType<T>() => Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
}