# Migrating from Maple.Result 2.x to 3.0

This guide lists the changes between the last 2.x release (`2.8.12`) and `3.0`, explains why they were made, and shows how to update your code.

Version 3.0 makes `Result`, `Result<T>` and `Error` **immutable** and **always valid**: an invalid instance can no longer be created, and an instance cannot change after it was created. It also adds the `LimitExceeded` error category, error metadata and an `ErrorBuilder`.

## Contents
* [Quick checklist](#quick-checklist)
* [Breaking changes](#breaking-changes)
  * [New `LimitExceeded` error category](#new-limitexceeded-error-category)
  * [Public constructors removed](#public-constructors-removed)
  * [`AddDetail()` removed](#adddetail-removed)
  * [`with` expressions no longer change errors](#with-expressions-no-longer-change-errors)
  * [No JSON deserialization without converters](#no-json-deserialization-without-converters)
  * [Changed JSON output](#changed-json-output)
  * [Stricter validation](#stricter-validation)
  * [Nullable named values](#nullable-named-values)
  * [`TemplatedMessage` constructors](#templatedmessage-constructors)
  * [Other behaviour changes](#other-behaviour-changes)
  * [`continueOnCapturedContext` is `bool?`](#continueoncapturedcontext-is-bool)
  * [`IResult` renamed to `IOperationResult`](#iresult-renamed-to-ioperationresult)
  * [`IfSuccess` and `Match` with an `Error` or a task](#ifsuccess-and-match-with-an-error-or-a-task)
  * [Extension packages](#extension-packages)
* [New features](#new-features)
* [Search-and-replace reference](#search-and-replace-reference)

## Quick checklist
1. Handle `ErrorCategory.LimitExceeded` in every exhaustive `switch` over `ErrorCategory`.
2. Replace `error.AddDetail(...)` with `error = error.WithErrorDetail(...)` or an `ErrorBuilder`.
3. Replace `error with { … }` with `error.ToBuilder()…Build()`.
4. Replace `new Result()`, `new Result<T>()`, `new Error()` and `new ErrorUri(...)` with the factory methods.
5. If you **read** `Result`, `Result<T>` or `Error` from JSON (e.g., Azure Durable Functions, message queues, caches, HTTP clients), see [No JSON deserialization without converters](#no-json-deserialization-without-converters).
6. If you compare or snapshot the **written** JSON, see [Changed JSON output](#changed-json-output).
7. Make sure error titles are never `null` or blank, and pass `null` instead of `ErrorUri.None()` as an instance *URI*.
8. Fix the calls of `IfSuccess`, `Match` and the LINQ queries that no longer compile or that now return a failed result (see [`IfSuccess` and `Match` with an `Error` or a task](#ifsuccess-and-match-with-an-error-or-a-task)).
9. Upgrade the `Maple.Result.Extensions.*` packages together with `Maple.Result` (see [Extension packages](#extension-packages)).

## Breaking changes

### New `LimitExceeded` error category
`ErrorCategory.LimitExceeded` (numeric value `10`) and the `Error.LimitExceeded(...)` factory methods have been added. Use the category when the request is valid and permitted, but a usage limit, quota or rate has been exhausted (HTTP `429`). `Unavailable` (HTTP `503`) now means only that a service is unavailable.

The numeric values of the existing members have not changed, but exhaustive `switch` statements and expressions must handle the new member:

```csharp
var statusCode = error.Category switch
{
    ErrorCategory.Validation => 400,
    // ...
    ErrorCategory.Unavailable => 503,
    ErrorCategory.LimitExceeded => 429, // new
    _ => 500
};
```

### Public constructors removed
The types can only be created through their factory methods, which always produce a valid instance.

| 2.x | 3.0 |
| --- | --- |
| `new Result()` | `Result.Success()` |
| `new Result { Error = error }` | `Result.FromError(error)` or the implicit conversion from `Error` |
| `new Result<T>()` | — (a result is always either successful or failed) |
| `new Result<T> { Value = value }` | `Result<T>.FromValue(value)`, `Result.FromValue(value)` or the implicit conversion from `T` |
| `new Result<T> { Error = error }` | `Result<T>.FromError(error)` or the implicit conversion from `Error` |
| `new Error()` / `new Error { … }` | `Error.Validation(...)`, `Error.NotFound(...)`, … or `ErrorBuilder` |
| `new ErrorUri("…")` | `ErrorUri.Tag("…")`, `ErrorUri.Locator("…")`, `ErrorUri.Urn("…")`, `ErrorUri.Relative("/…")` or `ErrorUri.None()` |

`ErrorDetail` and `TemplatedMessage` keep their public constructors.

`ErrorUri.Urn` (*RFC 8141*) and `ErrorUri.Relative` (a relative reference with the full path, e.g., `/account/12345/msgs/abc`) are new in 3.0, for values that 2.x could create only with the constructor, such as `urn:` types and relative instances. Other values, e.g., a relative path without a leading `/`, cannot be created.

> [!NOTE]
> In 2.x, `new Result<T>()` created a result with neither a value nor an error, and `IsSuccess()` then threw an `InvalidOperationException`. That state no longer exists: a `Result<T>` is successful exactly when its `Error` is `null`.

### `AddDetail()` removed
In 2.x, `AddDetail()` **changed the error it was called on** and returned the same instance. That was easy to misuse: adding a detail to an error kept in a `static readonly` field changed it for every caller, and the hash code of the error changed after it was used as a dictionary key.

In 3.0, `Error` is immutable. Use one of:
* `WithErrorDetail(...)`, which takes the same arguments in the same order and **returns a copy** with the detail appended (the parameters `messageId` and `namedValues` are renamed to `detailTemplateId` and `detailNamedValues`, so update calls that use named arguments),
* an `ErrorBuilder` with `AddErrorDetail(...)`, to add several details without copying the error each time.

```csharp
// 2.x
var error = Error.Validation(typeUri, "Invalid data to register a new user.");
error.AddDetail("#/email", "The email address is required.");
error.AddDetail("#/username", "The username is too short.", "errors.username.length", ("min", 3));

// 3.0, a few details: WithErrorDetail returns a copy, so assign the result
var error = Error.Validation(typeUri, "Invalid data to register a new user.")
    .WithErrorDetail("#/email", "The email address is required.")
    .WithErrorDetail("#/username", "The username is too short.", "errors.username.length", ("min", 3));

// 3.0, details collected in a loop: use a builder
var builder = ErrorBuilder.Validation(typeUri, "Invalid data to register a new user.");
foreach (var failure in validationFailures)
    builder.AddErrorDetail(failure.PropertyPointer, failure.Message);

var error = builder.Build();
```

> [!CAUTION]
> `error.WithErrorDetail(...);` on its own line compiles but does nothing useful: the returned copy is discarded and `error` is unchanged. Assign the result: `error = error.WithErrorDetail(...);`.

The 2.x `internal AddDetail(ErrorDetail)` overload is replaced by the public `WithErrorDetail(ErrorDetail)` and `ErrorBuilder.AddErrorDetail(ErrorDetail)`.

### `with` expressions no longer change errors
All properties of `Error`, `ErrorDetail` and `TemplatedMessage` are now get-only, so `with { Property = … }` does not compile. Use `ToBuilder()` for `Error`:

```csharp
// 2.x
var copy = error with { Title = "Localized title", Detail = null };

// 3.0
var copy = error.ToBuilder()
    .WithTitle("Localized title")
    .WithDetail(null)
    .Build();
```

For `ErrorDetail` and `TemplatedMessage`, create a new instance with the constructor.

### No JSON deserialization without converters
In 2.x, `Result`, `Result<T>`, `Error` and `ErrorUri` could be read from JSON by *System.Text.Json* and *Newtonsoft.Json*, thanks to public parameterless constructors, `init` setters and a built-in converter. That support allowed invalid instances (for example, an empty `Result<T>` read from `{}`), so it was removed:
* the built-in `ResultJsonConverter` and the `[JsonConverter]` attribute on `Result<T>` are gone,
* the types have no public constructors or setters.

Writing the types as JSON still works with the default settings. **Reading them is not supported**: for `Result`, `Result<T>` and `Error`, *System.Text.Json* throws a `NotSupportedException` and *Newtonsoft.Json* throws a `JsonSerializationException`.

> [!WARNING]
> `ErrorUri` is a struct, so both serializers can create it without a constructor: an `ErrorUri` that is a member of your own type is silently read as `about:blank`. Do not deserialize it without a dedicated converter; store the *URI* as a `string` in your DTOs instead.

Dedicated converter packages for *System.Text.Json* and *Newtonsoft.Json* are planned. Until they are available, if you need to read these types (e.g., as Azure Durable Functions activity results, Service Bus messages or cached values), map them to your own DTOs at the boundary:

```csharp
public sealed record ErrorDto(ErrorCategory Category, string TypeUri, string Title, string? Detail, string? InstanceUri);

public static ErrorDto ToDto(Error error) =>
    new(error.Category, error.TypeUri, error.Title, error.Detail, error.InstanceUri);

public static Error FromDto(ErrorDto dto) =>
    new ErrorBuilder(dto.Category, ToErrorUri(dto.TypeUri), dto.Title)
        .WithDetail(dto.Detail)
        .WithInstanceUri(dto.InstanceUri is null ? null : ToErrorUri(dto.InstanceUri))
        .Build();

private static ErrorUri ToErrorUri(string value) =>
    value == "about:blank" ? ErrorUri.None()
    : value.StartsWith("tag:", StringComparison.Ordinal) ? ErrorUri.Tag(value)
    : value.StartsWith("urn:", StringComparison.OrdinalIgnoreCase) ? ErrorUri.Urn(value)
    : value.StartsWith('/') ? ErrorUri.Relative(value)
    : ErrorUri.Locator(value);
```

> [!NOTE]
> `Maple.Json.ObjectAsPrimitiveConverter`, which the 2.x documentation suggested for reading metadata values, is no longer relevant to `Maple.Result`.

### Changed JSON output
The JSON written by the default serializers changed in two ways:

| | 2.x | 3.0 |
| --- | --- | --- |
| Failed `Result<int>` | `{"Error":{…}}` | `{"Value":0,"Error":{…}}` |
| Failed `Result<string>` | `{"Error":{…}}` | `{"Value":null,"Error":{…}}` |
| `Error` | no `Metadata` member | always has `"Metadata":{…}` (`{}` when empty) |

A failed `Result<T>` now always writes `Value` with the default value of `T`. Clients must use `Error` (not `Value`) to tell a failure from a success. Update any snapshot tests or contract tests that compare the JSON.

### Stricter validation
These values were accepted in 2.x and now throw:

| Value | Exception |
| --- | --- |
| A `null` error title (factory methods, `ErrorBuilder`) | `ArgumentNullException` |
| An empty or white-space-only error title | `ArgumentException` |
| An undefined `ErrorCategory` value, e.g., `(ErrorCategory)42` (`ErrorBuilder`) | `ArgumentOutOfRangeException` |
| An empty or white-space-only `detailTemplateId` / `TemplatedMessage` template id | `ArgumentException` |
| A repeated named-value name, e.g., `("min", 3), ("min", 4)` (was an exception from LINQ's `ToDictionary`, now with a clear message) | `ArgumentException` |
| A `null` named-value name | `ArgumentNullException` |
| Named values without a template id, e.g., `WithErrorDetail("#/name", "Too short.", null, ("min", 3))` (2.x **silently dropped** them) | `ArgumentException` |
| A `null` `TemplatedMessage.TemplateId` | `ArgumentNullException` |
| A `null` `ErrorDetail.Detail` | `ArgumentNullException` |
| `about:blank` as an instance *URI*, e.g., `instanceUri: ErrorUri.None()` (factory methods, `ErrorBuilder.WithInstanceUri`); pass `null` instead | `ArgumentException` |
| An `ErrorUri.Tag` or `ErrorUri.Locator` value with white space (including a trailing new line), invalid percent-encoding (e.g., `%zz`), or, for locators, characters that need escaping (e.g., a space) or literal non-ASCII characters; 2.x accepted them and stored the value as passed | `UriFormatException` |

In return, every `Error` you receive has a non-blank title, a defined category, a non-`null` type *URI* and no meaningless `about:blank` instance *URI*.

`about:blank` has a meaning only as a type (*RFC 9457*, section 4.2.1). A default `ErrorUri` is `about:blank` too, so `instanceUri: default` throws as well.

### Nullable named values
The named values of templated messages are now `object?` instead of `object`:
* the `detailNamedValues` parameters of the `Error` factory methods,
* the `detailNamedValues` parameter of `WithErrorDetail` (formerly `namedValues` of `AddDetail`),
* `TemplatedMessage.Params` (`IReadOnlyDictionary<string, object?>?`).

Existing calls still compile. With nullable reference types enabled, passing a `Dictionary<string, object>` produces warning CS8620; declare it as `Dictionary<string, object?>` instead. Code that reads `Params[...]` may get nullable warnings (CS8602) and should handle `null` values.

`TemplatedMessage` now copies `Params` into its own read-only dictionary with an ordinal (case-sensitive) comparer, so changing the source dictionary later no longer changes the message.

### `TemplatedMessage` constructors
`TemplatedMessage` is no longer a positional record. It has two constructors:
* `TemplatedMessage(string templateId, IEnumerable<KeyValuePair<string, object?>>? parameters = null)`: passing a dictionary still compiles, and lists of key-value pairs now work too;
* `TemplatedMessage(string templateId, params ReadOnlySpan<(string Key, object? Value)> parameters)`: new; the parameters can be given inline, as an array or as a collection expression.

```csharp
// 2.x
var message = new TemplatedMessage("errors.length", new Dictionary<string, object> { ["min"] = 3 });

// 3.0 (both work)
var message = new TemplatedMessage("errors.length", new Dictionary<string, object?> { ["min"] = 3 });
var message = new TemplatedMessage("errors.length", ("min", 3));
```

Changes that can break existing code:
* **The parameter names** changed from `TemplateId` / `Params` to `templateId` / `parameters`. Update calls that use named arguments.
* **`Deconstruct` is gone.** `var (id, parameters) = message;` and positional patterns such as `message is ("errors.length", var p)` no longer compile; use a property pattern instead: `message is { TemplateId: "errors.length", Params: var p }`.
* **Repeated or `null` parameter names throw** (see [Stricter validation](#stricter-validation)).

The named values of the factory methods (`detailNamedValues`), `WithErrorDetail`, `ErrorBuilder.AddErrorDetail` and `ErrorBuilder.WithDetailTemplated` are now `params ReadOnlySpan<(string Key, object? Value)>` instead of `params (string Key, object? Value)[]`, so a call no longer allocates an array:
* inline values, arrays and collection expressions still compile; recompile the calling code (the signatures changed);
* an explicit `null` is now an empty list (e.g., `WithDetailTemplated("id", null)` no longer throws);
* a call with named values cannot appear inside an expression tree (e.g., a Moq `Setup(x => …)` or an EF query); build the error outside the expression.

### Other behaviour changes
* **`Result.Success()`** returns the same, shared instance on every call. It is immutable, so this is safe; only code that relied on reference inequality of two successful results is affected.
* **`ToString()`** of `Error` and `TemplatedMessage` now prints the contents of `ErrorDetails`, `Metadata` and `Params` (instead of their type names), and `ErrorUri.ToString()` returns just the *URI* (instead of `ErrorUri { Value = … }`). Update tests or log parsers that depend on the old text.
* **`GetHashCode()`** of an `Error` never changes, because the error cannot change. An `Error` can be used safely as a dictionary key or in a `HashSet<T>`.
* **`ErrorDetails`** is a read-only collection; casting it to `IList<ErrorDetail>` and modifying it throws a `NotSupportedException`.
* **`ErrorCategory.Timeout`** is now documented as `408` (Request Timeout) when the client did not send the complete request in time, or `504` (Gateway Timeout) when an upstream server or a dependency did not respond in time; 2.x documented only `408`. The `ErrorCategory` members now have explicit values, equal to the 2.x ones, so stored and serialized values do not change.

### `continueOnCapturedContext` is `bool?`
The `continueOnCapturedContext` parameter of the asynchronous extension methods (`IfSuccessAsync`, `IfErrorAsync`, `MatchAsync`, `ToResultAsync`, for `Task` and `ValueTask`) changed from `bool continueOnCapturedContext = false` to `bool? continueOnCapturedContext = null`. `null` uses the new global setting [`ResultConfiguration.ContinueOnCapturedContext`](Reference/ResultConfiguration/ResultConfiguration_ContinueOnCapturedContext.md), which is `false` by default, so **the behaviour does not change** unless you change the setting.

Calls that pass `true` or `false` still compile; recompile code that calls these methods (the signature changed, so binaries built against 2.x do not bind to 3.0).

The LINQ query operators (`Select`, `SelectMany` on `Task` and `ValueTask` of a result) used a hard-coded `ConfigureAwait(false)`; they now use the global setting too.

If your application has a synchronization context (WPF, Windows Forms, .NET MAUI, Blazor Server) and the delegates you pass access UI elements, set the global setting once at startup instead of passing `continueOnCapturedContext: true` everywhere:

```csharp
ResultConfiguration.ContinueOnCapturedContext = true;
```

### `IResult` renamed to `IOperationResult`
The common interface of `Result` and `Result<T>` was renamed from `IResult` to [`IOperationResult`](Reference/IOperationResult/IOperationResult.md). Its members (`Error`, `IsSuccess()`) did not change.

The old name clashed with `Microsoft.AspNetCore.Http.IResult`. ASP.NET Core web projects import `Microsoft.AspNetCore.Http` implicitly, and Minimal API endpoints commonly return `IResult`, so adding `using Maple.Result;` caused CS0104 (ambiguous reference) in endpoint signatures. If you added an alias to work around it (e.g., `using IResult = Microsoft.AspNetCore.Http.IResult;`), you can remove it.

```csharp
// 2.x
void Log(Maple.Result.IResult result) { … }

// 3.0
void Log(IOperationResult result) { … }
```

### `IfSuccess` and `Match` with an `Error` or a task
Two kinds of calls compiled in 2.x but did not do what they looked like:

* **A function that returns an `Error`** bound to the generic overload and returned a **successful** `Result<Error>`. In 3.0, `IfSuccess`, `IfSuccessAsync`, `Match` and `MatchAsync` (the overloads with an error action) have overloads for such functions that return a **failed** `Result` with that error. The same applies to a LINQ query that selects an `Error` (`from user in GetUser(id) select Error…`). Code that read `.Value` of the `Result<Error>` no longer compiles; if you need it, specify the type arguments.
* **A function that returns a `Task`, `Task<T>`, `ValueTask` or `ValueTask<T>`** passed to `IfSuccess`, or to the `Match` overloads with an `Action<Error>`, was wrapped in a `Result<Task>` and **never awaited**. In 3.0 such a call is a compile error (CS0619); use the asynchronous method, or specify the type argument to keep the task on purpose.

```csharp
// 2.x: a successful Result<Error>; 3.0: a failed Result with the error
var result = userResult.IfSuccess(user => Error.Conflict(typeUri, "The user already exists."));
var result = userResult.Match(user => Error.Conflict(typeUri, "The user already exists."), error => Log(error));
var result = from user in userResult select Error.Conflict(typeUri, "The user already exists.");

// 2.x: the task was never awaited; 3.0: a compile error
var result = Result.Success().IfSuccess(() => SaveAsync());

// 3.0: await it…
var result = await Result.Success().IfSuccessAsync(() => SaveAsync());

// …or keep the task on purpose
Result<Task> result = Result.Success().IfSuccess<Task>(() => SaveAsync());
```

### Extension packages
The current (2.x-based) version of `Maple.Result.Extensions.HttpClient` calls an internal method of `Maple.Result` 2.x that no longer exists, so using it with `Maple.Result` 3.0 fails at runtime. Upgrade all `Maple.Result.Extensions.*` packages to versions built for 3.0 together with `Maple.Result`, and do not mix 2.x and 3.0 packages.

## New features
* **`ErrorBuilder`** — a mutable builder of an immutable `Error`. Create it with `new ErrorBuilder(category, typeUri, title)`, a static method per category (e.g., `ErrorBuilder.Validation(...)`) or `error.ToBuilder()`; set the members with `With…` / `AddErrorDetail` methods; call `Build()`. Every argument is validated when it is passed, so `Build()` never throws. See [ErrorBuilder](Reference/ErrorBuilder/ErrorBuilder.md).
* **`Error.Metadata`** — machine-readable, response-level values (RFC 9457 *extension members*), with `WithMetadata()`, `WithoutMetadata()` (one key or all) and `TryGetMetadata<T>()`. `TryGetMetadata<T>()` is strict: it returns `true` only when the stored value is a `T` (a stored `int` is not read as `long`), and never for a `null` value. See [Metadata](Reference/Error/Error_Metadata.md).
* **`WithMetadata` with tuples** — `error.WithMetadata(("limit", 100), ("remaining", 0))` on `Error` and `ErrorBuilder` adds several entries in one call.
* **`Error.WithErrorDetail()`** and **`Error.ToBuilder()`** — see above.
* **`ErrorCategory.LimitExceeded`** — see above.
* **`ErrorUri.Urn()`** and **`ErrorUri.Relative()`** — `urn:` types and instances (*RFC 8141*), and relative references with the full path (e.g., the *RFC 9457* example `/account/12345/msgs/abc`). See [ErrorUri](Reference/ErrorUri/ErrorUri.md).
* **Mixed LINQ queries** — synchronous and asynchronous `from` clauses can be mixed in one query (e.g., `from user in GetUserAsync(id) from address in GetAddress(user) select …`); the query is then asynchronous and has to be awaited. See [SelectMany](Reference/Extensions/LinqTaskAsyncExtensions_SelectMany.md).
* **`ResultConfiguration.ContinueOnCapturedContext`** — a global default for the `continueOnCapturedContext` argument of the asynchronous extension methods and for the LINQ query operators; see above.
* **Native AOT and trimming** — the core package no longer contains JSON code and produces no trimming or AOT warnings.

## Search-and-replace reference
| Find (2.x) | Replace with (3.0) |
| --- | --- |
| `error.AddDetail(…);` | `error = error.WithErrorDetail(…);` |
| `Error.X(…).AddDetail(…).AddDetail(…)` | `Error.X(…).WithErrorDetail(…).WithErrorDetail(…)` or `ErrorBuilder.X(…).AddErrorDetail(…).AddErrorDetail(…).Build()` |
| `error with { Title = t }` | `error.ToBuilder().WithTitle(t).Build()` |
| `error with { Detail = d }` | `error.ToBuilder().WithDetail(d).Build()` |
| `error with { Category = c }` | `error.ToBuilder().WithCategory(c).Build()` |
| `new Result()` | `Result.Success()` |
| `new Result<T> { Value = v }` | `Result<T>.FromValue(v)` |
| `new Result<T> { Error = e }` | `Result<T>.FromError(e)` |
| `new ErrorUri(s)` | `ErrorUri.Tag(s)` / `ErrorUri.Locator(s)` / `ErrorUri.Urn(s)` / `ErrorUri.Relative(s)` / `ErrorUri.None()` |
| `instanceUri: ErrorUri.None()` / `WithInstanceUri(ErrorUri.None())` | `instanceUri: null` / `WithInstanceUri(null)` |
| `result.IfSuccess(() => SaveAsync())` (a function returning a task) | `await result.IfSuccessAsync(() => SaveAsync())` |
| `result.IfSuccess(x => Error…)` relied on as a `Result<Error>` | `result.IfSuccess<T, Error>(x => Error…)` |
| `result.Match(x => Error…, e => …)` relied on as a `Result<Error>` | `result.Match<T, Error>(x => Error…, e => …)` |
| `Dictionary<string, object>` passed as named values | `Dictionary<string, object?>` |
| `IResult` (`Maple.Result`) | `IOperationResult` |
| `WithErrorDetail(…, messageId: …, namedValues: …)` (named arguments) | `WithErrorDetail(…, detailTemplateId: …, detailNamedValues: …)` |
| `var (id, parameters) = templatedMessage;` | `templatedMessage is { TemplateId: var id, Params: var parameters }` |
| `JsonSerializer.Deserialize<Result<T>>(…)` / `Deserialize<Error>(…)` | your own DTO, or the planned converter packages |

## See Also
* [README: Upgrading to 3.0](../README.md#upgrading-to-30)
* [Reference documentation](Reference/namespace.md)
* [Issue #102: Simplify Result and Error](https://github.com/TomMaple/Maple.Result/issues/102)
* [Issue #105: SynchronizationContext of the asynchronous extension methods](https://github.com/TomMaple/Maple.Result/issues/105)
