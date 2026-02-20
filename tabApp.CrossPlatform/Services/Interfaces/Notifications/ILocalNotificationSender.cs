namespace tabApp.CrossPlatform.Services.Interfaces.Notifications;

/// <summary>
/// Sends a local (on-device) push notification.
///
/// Abstracts the platform notification API so <see cref="IProximityNotificationService"/>
/// can be unit-tested without a MAUI host.
///
/// POC implementation: <c>MauiLocalNotificationSender</c>
/// Production: replace with Plugin.LocalNotification or similar.
/// </summary>
public interface ILocalNotificationSender
{
    /// <summary>
    /// Post a notification to the device notification tray.
    /// </summary>
    /// <param name="id">Unique notification ID (prevents stacking same alert).</param>
    /// <param name="title">Notification title.</param>
    /// <param name="message">Notification body text.</param>
    Task SendAsync(int id, string title, string message);
}


