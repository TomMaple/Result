using System.Collections.Generic;

namespace Maple.Result;

/// <summary>
///     The structure for a templated message.
/// </summary>
/// <param name="TemplateId">The identifier of the template.</param>
/// <param name="Params">
///     The optional collection of parameters (name and value) that might be required to
///     generate a message from the specific template.
/// </param>
public readonly record struct MessageTemplate(string TemplateId, IReadOnlyDictionary<string, object>? Params = null);