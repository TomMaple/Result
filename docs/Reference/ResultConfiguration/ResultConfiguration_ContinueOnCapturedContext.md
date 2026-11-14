# ResultConfiguration.ContinueOnCapturedContext Property
## Definition
Namespace: [Maple.Result](../namespace.md)<br>
Assembly: Maple.Result.dll<br>
Source: <a href="https://github.com/TomMaple/Maple.Result/blob/main/src/Maple.Result/ResultConfiguration.cs#L46" target="_blank">ResultConfiguration.cs</a>

Gets or sets the default value used by the asynchronous extension methods (e.g., `IfSuccessAsync`, `MatchAsync` and the LINQ query operators) when their `continueOnCapturedContext` argument is not passed.

```csharp
public static bool ContinueOnCapturedContext { get; set; }
```

### Property Value
[Boolean](https://learn.microsoft.com/dotnet/api/system.boolean)

`true` to run the continuations, including the passed delegates, on the captured context (e.g., the UI thread); `false` to run them on any thread. The default is `false`.

## Remarks
The asynchronous extension methods that take a `Task` or `ValueTask` of a result await it first and then invoke the passed delegates. With `false`, they await it without capturing the current [SynchronizationContext](https://learn.microsoft.com/dotnet/api/system.threading.synchronizationcontext) (`ConfigureAwait(false)`), so the delegates run on a thread-pool thread.

| Application | Synchronization context | Recommended value |
| ----------- | ----------------------- | ----------------- |
| ASP.NET Core, console applications, Azure Functions, background services | none | `false` (the default); the value makes no difference |
| WPF, Windows Forms, .NET MAUI, Blazor Server | yes | `true` when the delegates access UI elements or other context-bound state |

An explicit `continueOnCapturedContext` argument always takes precedence over this setting. The LINQ query syntax cannot pass the argument, so the LINQ operators ([Select](../Extensions/LinqTaskAsyncExtensions_Select.md), [SelectMany](../Extensions/LinqTaskAsyncExtensions_SelectMany.md) and their [ValueTask](../Extensions/ValueTasks/LinqValueTaskAsyncExtensions.md) versions) always use this setting.

> [!NOTE]
> Set the property once at application startup. It applies to the whole process; changing it while asynchronous operations are running affects only the awaits that start afterwards.

> [!CAUTION]
> With `true`, a caller that blocks on the asynchronous operation on the UI thread (e.g., `.Result` or `.Wait()`) deadlocks, as it would with any `await` in its own code. Await the operation instead.

## Examples
```csharp
// WPF: App.xaml.cs
protected override void OnStartup(StartupEventArgs e)
{
    ResultConfiguration.ContinueOnCapturedContext = true;
    base.OnStartup(e);
}

// The delegate now runs on the UI thread.
await _service.LoadUserAsync()
    .IfSuccessAsync(user =>
    {
        NameLabel.Content = user.Name;
        return Task.CompletedTask;
    });
```

## See Also
* [ResultConfiguration](ResultConfiguration.md)
* [ConfigureAwait FAQ](https://devblogs.microsoft.com/dotnet/configureawait-faq/)
