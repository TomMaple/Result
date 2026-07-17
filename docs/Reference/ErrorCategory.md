# ErrorCategory Enum
## Definition
Namespace: [Maple.Result](namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Result/blob/main/src/Maple.Result/ErrorCategory.cs" target="_blank">ErrorCategory.cs</a>

Defines the category of an error.

```csharp
public enum ErrorCategory
```

Inheritance [Object](https://learn.microsoft.com/dotnet/api/system.object) → [ValueType](https://learn.microsoft.com/dotnet/api/system.valuetype) → [Enum](https://learn.microsoft.com/dotnet/api/system.enum) → ErrorCategory

## Fields
| Name            | Value | Description                          |
| --------------- | ----- | ------------------------------------ |
| Validation      | 0     | A validation error. The operation failed due to something wrong with the input data.<br>It relates to the `400` HTTP status code (Bad Request). |
| Unauthenticated | 1     | The action is not authenticated. The authentication is either missing or invalid.<br>It relates to the `401` HTTP status code (Unauthorized). |
| Unauthorized    | 2     | The action is not authorized. The authentication might be valid, but the identity does not have the required permissions.<br>It relates to the `403` HTTP status code (Forbidden). |
| NotFound        | 3     | The resource was not found. The requested resource does not exist.<br>It relates to the `404` HTTP status code (Not Found). |
| Timeout         | 4     | A timeout error. The operation timed out before it could complete.<br>It relates to the `408` HTTP status code (Request Timeout). |
| Conflict        | 5     | A conflict error. The operation failed due to the data or state conflict (such as a unique constraint violation).<br>It relates to the `409` HTTP status code (Conflict). |
| Failure         | 6     | An expected error that is not critical to the operation. Do not use for authentication, authorization, validation, data conflict or unexpected errors.<br>It relates to the `422` HTTP status code (Unprocessable Content). |
| Critical        | 7     | An unexpected, critical error.<br>It relates to the `500` HTTP status code (Internal Server Error). |
| NotImplemented  | 8     | The not implemented error. The operation, feature or case is not implemented.<br>It relates to the `501` HTTP status code (Not Implemented). |
| Unavailable     | 9     | The resource is not available. Try again later.<br>It relates to the `503` HTTP status code (Service Unavailable). |

## See Also
* [HTTP Status Codes](https://developer.mozilla.org/en-US/docs/Web/HTTP/Reference/Status)
* [HttpStatusCode Enum](https://learn.microsoft.com/en-us/dotnet/api/system.net.httpstatuscode)
