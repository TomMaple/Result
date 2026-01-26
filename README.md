![NuGet Version](https://img.shields.io/nuget/v/Maple.Result?label=Maple.Result%20%E2%80%94%20nuget&link=https%3A%2F%2Fwww.nuget.org%2Fpackages%2FMaple.Result%2F)
![NuGet Downloads](https://img.shields.io/nuget/dt/Maple.Result)
![GitHub Actions Workflow Status](https://img.shields.io/github/actions/workflow/status/TomMaple/Result/build-and-publish.yml)
![GitHub last commit](https://img.shields.io/github/last-commit/TomMaple/Result)



# Result
An abstraction of the operation result which can be either success (with or without a value) or a failure (with an error). It can be mapped to the HTTP reponse if needed.

# Give it a star ⭐!
Do you like it? Show your support by giving this project a star!

# Why
The *Result* pattern is a great way to handle operations that can either succeed or fail. Instead of using exceptions for control flow, it allows to return a standardized error. It helps to make the code more readable, maintainable, and testable by clearly separating success and error handling logic.

There are already many existing implementations of the *Result* pattern in C#.

However, I decided to create a new implementation to address a few missing functionalities in other libraries like:
* support for localization,
* support for more precise error descriptions,
* following standards ([RFC 9457](https://datatracker.ietf.org/doc/html/rfc9457)).

# Getting Started
## Adding the *NuGet* Package
To get started, add the `Maple.Result` package to your project from the *NuGet* package manager or with the command:
```bash
dotnet add package Maple.Result
```

The `Maple.Result` package contains all the types and the core functionality of the *Result* pattern.

# Using a Result
## Creating a Result
```csharp
public async Task<Result<User>> GetUserAsync(int userId)
{
    try
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.id == userId);

        if (user is null)
        {
            return Error.NotFound(
                ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:account:notfound"),
                "Invalid user ID.",
                "The user account with the provided ID has not been found.",
                ErrorUri.Locator("https://api.exampleapp.com/errors/8783927589734857/details"),
                "errors.user.account.notFound",
                ("accountId", "12345"), ("action", "account:update"));
            });
        }

        return Result.Success(user);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "An unexpected error occurred while retrieving user with ID {UserId}", userId);

        return Error.InternalServerError(
            ErrorUri.Tag("tag:exampleapp.com,2026:errors:user:account:geterror"),
            "An unexpected error occurred.",
            "An unexpected error occurred while getting the user account details.",
            ErrorUri.Locator("https://api.exampleapp.com/errors/2393284590348/details"),
            "errors.internal.dberror",
            ("accountId", userId.ToString()), ("action", "account:retrieve"), ("_exception", ex.Message));
    }
}
```

## Controlling the application flow based on Result
```csharp
var userResult = await _userService.GetUserAsync(userData);

var userAddedResult = await userResult.IfSuccessAsync(async (user) =>
{
    return await _companyService.AddUserAsync(user);
});

var userTokenResult = await userAddedResult.MatchAsync(
    async (user) => await _loginService.GetUserTokenAsync(user),
    async (error) => await _auditService.LogErrorAsync(error));
```

## Support for multiple error details / validation errors
```csharp
if (string.IsNullOrWhiteSpace(userInput.email))
{
    error.AddDetail(
        "#/email",
        "The email is required.",
        "errors.user.input.email.required");
}

if (userInput.email.Length < 8)
{
    error.AddDetail(
        "#/email",
        "The email must be at least 8 characters long.",
        "errors.user.input.email.minLength",
        ("minLength", 8));
}
```

## Useful extensions
### IfSuccess / IfSuccessAsync




# Learn More
## Documentation
* [Maple.Result](namespace.md)
* [Maple.Result.Extensions](Extensions/namespace.md)

## See also
* [Problem Details for HTTP APIs - RFC 7807 is dead, long live RFC 9457](https://blog.frankel.ch/problem-details-http-apis/)
* [tag URI scheme](https://en.wikipedia.org/wiki/Tag_URI_scheme)
* [RFC 1738: Uniform Resource Locators (URL)](https://datatracker.ietf.org/doc/html/rfc1738)
* [RFC 3986: Uniform Resource Identifier (URI): Generic Syntax](https://datatracker.ietf.org/doc/html/rfc3986)
* [RFC 4151: The 'tag' URI Scheme](https://datatracker.ietf.org/doc/html/rfc4151)
* [RFC 6901: JavaScript Object Notation (JSON) Pointer](https://datatracker.ietf.org/doc/html/rfc6901)
* [RFC 9457: Problem Details for HTTP APIs](https://datatracker.ietf.org/doc/html/rfc9457)

# Error Response
Example
```json
{
    "type": "https://example.com/probs/out-of-credit", 
    "status": 400,
    "title": "You do not have enough credit.",
    "detail": "Your current balance is 30, but that costs 50.",
    "instance": "/accounts/12345/msgs/abc",
    "errors": [
        {
            "pointer": "#/age",
            "detail": "must be a positive integer",
            "detailTemplate": {
                "messageId":"user.details.age.mustBePositive"
            }
        },
        {
            "pointer": "#/profile/colour",
            "detail": "must be ‘green’, ‘red’ or ‘blue’",
            "detailTemplate": {
                "messageId": "user.profile.colour",
                "params": {
                    "validValueIds": [
                        "user.profile.colour.green",
                        "user.profile.colour.red",
                        "user.profile.colour.blue"
                    ]
                }
            }
        }
    ],
    "detailTemplate": {
        "messageId": "user.account.balance.tooLow",
        "params": {
            "errorCode": "UAB17",
            "accounts": [
                {
                    "title": "Main (***9456)",
                    "url": "/accounts/12345"
                },
                {
                    "title": "Main (***3357)",
                    "url": "/accounts/67890"
                }
            ],
            "currentBalance": 30,
            "requiredBalance": 50
        }
    }
}
```

# `Error` Properties
| Property                       | Type |
|--------------------------------|------|
| [Category](#category) | The category of the error. |
| [TypeUri](#type-uri)  | A *URI* that identifies the type of the problem. |
| [Title](#title)       | A short, human-readable summary of the problem type. |
| [Detail](#detail)     | A human-readable explanation specific to this occurrence of the problem. |
| [DetailTemplated](#detail-templated) | A structure to generate a localized detail message. |
| [InstanceUri](#instance-uri) | A *URI* that identifies the specific occurrence of the problem. |
| [ErrorDetails](#error-details) | A collection of additional error details. |

## `Category`
| Property | Type            | Required | Example |
|----------|-----------------|-------------|---------|
| Category | `ErrorCategory` | A required category of the error (one of predefined). | `ErrorCategory.Validation` |
| TypeUri  | `string`        | A mandatory *URI* that identifies the type of the problem. The value of this attribute can be: `about:blank`It is a *URL* that, when dereferenced, provides human-readable documentation about the problem. | `https://app.example.com/errors/out-of-credit` |
| Title    | `string`        | A short, human-readable summary of the problem type. It does not vary for different occurrences of the problem and is designed to be used in conjunction with the type field.

# Contribution
Please contact author: tom.maple(at)outlook.com
