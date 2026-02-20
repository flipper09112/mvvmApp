namespace tabApp.CrossPlatform.Services.Interfaces.Notifications;

/// <summary>
/// Persistent key-value store for notification deduplication state.
///
/// Abstracts the underlying storage mechanism (MAUI Preferences for POC,
/// SQLite for production) so <see cref="IProximityNotificationService"/>
/// never depends on a specific persistence technology.
///
/// Deduplication key format: "{itemType}_{itemId}_{yyyy-MM-dd}"
/// Example: "Order_42_2026-02-20"
/// </summary>
public interface INotificationStateStore
{
    /// <summary>
    /// Returns true if the item has already been notified on the current calendar day.
    /// </summary>
    bool IsNotified(string key);

    /// <summary>
    /// Marks the item as notified. State survives app restarts.
    /// </summary>
    void MarkNotified(string key);

    /// <summary>
    /// Removes all stored keys whose embedded date is older than today.
    /// Call once per day (e.g. on app startup or midnight rollover).
    /// </summary>
    void ClearExpired();

    /// <summary>
    /// Removes all stored notification state. Use for testing only.
    /// </summary>
    void ClearAll();
}

