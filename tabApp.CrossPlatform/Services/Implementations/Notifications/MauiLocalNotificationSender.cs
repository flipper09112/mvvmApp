using tabApp.CrossPlatform.Services.Interfaces.Notifications;

namespace tabApp.CrossPlatform.Services.Implementations.Notifications;

/// <summary>
/// POC implementation of <see cref="ILocalNotificationSender"/> using MAUI Essentials.
///
/// MAUI does not ship a built-in LocalNotification API in .NET 10.
/// This stub logs to debug output so the POC pipeline is complete and testable.
/// Replace with Plugin.LocalNotification (or equivalent) in the production implementation.
///
/// Reference: TASK-3.4 §Notification Content Strategy
/// </summary>
public sealed class MauiLocalNotificationSender : ILocalNotificationSender
{
    public Task SendAsync(int id, string title, string message)
    {
        // POC: write to debug output; swap for real notification plugin in production
        System.Diagnostics.Debug.WriteLine(
            $"[LocalNotification] id={id} | {title} | {message}");

        return Task.CompletedTask;
    }
}

