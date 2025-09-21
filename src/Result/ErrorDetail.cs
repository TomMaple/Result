namespace Maple.Result;

/// <summary>
///     The structure for an individual error occurence that contains a problem detail.
/// </summary>
/// <remarks>
///     For more details about JSON Pointers, see: <seealso href="https://datatracker.ietf.org/doc/html/rfc6901"/>.
/// </remarks>
/// <param name="PropertyPointer">The JSON Pointer which identifies the invalid value in the input data.</param>
/// <param name="Detail">The human-readable explanation specific to this individual error occurence.</param>
/// <param name="DetailTemplated">The message template with the human-readable explanation.</param>
public record ErrorDetail(string? PropertyPointer, string Detail, TemplatedMessage? DetailTemplated = null);
