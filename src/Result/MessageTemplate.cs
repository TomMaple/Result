using System.Collections.Generic;

namespace Maple.Result;

/// <summary>
///     The structure for a templated message.
/// </summary>
/// <remarks>
///     The purpose of this structure is to provide a way to generate client-side localized messages.
/// </remarks>
/// <param name="TemplateId">The identifier of the message template.</param>
/// <param name="Params">
///     The optional collection of parameters (names and values) that might be required to
///     generate a message from the specific template.
/// </param>
public readonly record struct MessageTemplate(string TemplateId, IReadOnlyDictionary<string, object>? Params = null);