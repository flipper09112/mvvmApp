namespace tabApp.CrossPlatform.Services.Implementations.Notifications;

/// <summary>
/// Builds deterministic, per-day deduplication keys for proximity notifications.
///
/// Key format: "{ItemType}_{ItemId}_{yyyy-MM-dd}"
/// Examples:
///   Order        → "Order_42_2026-02-20"
///   Notification → "Notification_7_2026-02-20"
///
/// The date segment bounds the deduplication window to a single calendar day:
/// the same item can be re-notified the following day (new key).
/// </summary>
public static class DeduplicationKeyBuilder
{
    public static string ForOrder(int orderId, DateTime? date = null)
        => $"Order_{orderId}_{(date ?? DateTime.Today):yyyy-MM-dd}";

    public static string ForNotification(int notificationId, DateTime? date = null)
        => $"Notification_{notificationId}_{(date ?? DateTime.Today):yyyy-MM-dd}";
}

