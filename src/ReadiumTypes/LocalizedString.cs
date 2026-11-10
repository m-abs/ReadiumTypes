using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

using Mabs.ReadiumTypes.Utils;

namespace Mabs.ReadiumTypes;

/// <summary>
/// A value in one or more languages keyed by BCP 47 tag; the empty-string key holds the
/// language-unspecified (default) value. All wire forms are read and written through
/// <see cref="LocalizedStringConverter"/>.
/// </summary>
[JsonConverter(typeof(LocalizedStringConverter))]
public sealed class LocalizedString : IEnumerable<KeyValuePair<string, string>>
{
    private readonly Dictionary<string, string> _map = [];

    /// <summary>Creates an empty localized string.</summary>
    public LocalizedString()
    {
    }

    /// <summary>Creates a localized string with one language-unspecified value.</summary>
    /// <param name="value">The value.</param>
    public LocalizedString(string value)
    {
        _map[string.Empty] = value;
    }

    /// <summary>Converts a plain string to a localized string with a language-unspecified value.</summary>
    /// <param name="value">The value.</param>
    /// <returns>A localized string holding <paramref name="value"/>.</returns>
    public static implicit operator LocalizedString(string value)
    {
        return new LocalizedString(value);
    }

    /// <summary>Gets or sets the value for a language tag.</summary>
    /// <param name="language">The BCP 47 language tag; the empty string is the default language.</param>
    public string this[string language]
    {
        get => _map[language];
        set => _map[language] = value;
    }

    /// <summary>Gets the number of language entries.</summary>
    public int Count => _map.Count;

    /// <summary>Gets the default-language value, or the first available value, or <see langword="null"/> when empty.</summary>
    public string? Default
    {
        get
        {
            if (_map.TryGetValue(string.Empty, out string? value))
            {
                return value;
            }

            return _map.Count == 0 ? null : _map.Values.First();
        }
    }

    /// <summary>Determines whether this instance holds the same languages and values as <paramref name="obj"/>.</summary>
    /// <param name="obj">The object to compare with.</param>
    /// <returns><see langword="true"/> when the language maps are equal.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not LocalizedString other || other._map.Count != _map.Count)
        {
            return false;
        }

        foreach (KeyValuePair<string, string> pair in _map)
        {
            if (!other._map.TryGetValue(pair.Key, out string? value) || value != pair.Value)
            {
                return false;
            }
        }

        return true;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        HashCode hashCode = default;
        foreach (KeyValuePair<string, string> pair in _map)
        {
            hashCode.Add(pair.Key);
            hashCode.Add(pair.Value);
        }

        return hashCode.ToHashCode();
    }

    /// <inheritdoc/>
    public override string? ToString()
    {
        return Default;
    }

    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        return _map.GetEnumerator();
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}