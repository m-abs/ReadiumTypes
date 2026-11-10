using System.Collections.Generic;
using System.Text.Json;

namespace Mabs.ReadiumTypes;

/// <summary>
/// Implemented by every model that carries <c>[JsonExtensionData]</c>, enabling the
/// <see cref="ExtensionDataExtensions"/> helpers. The members match the property every
/// such model already declares.
/// </summary>
public interface IExtensionDataHolder
{
    /// <summary>Properties outside the schema, preserved across round-trips.</summary>
    IDictionary<string, JsonElement>? ExtensionData { get; set; }
}