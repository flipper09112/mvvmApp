using tabApp.Core.Models;
using tabApp.Core.Models.Notifications;

namespace tabApp.CrossPlatform.Services.Interfaces.Notifications;

/// <summary>
/// Sends proximity-triggered push notifications with per-day deduplication.
///
/// Replaces the inline <c>NotifyOrder()</c> and <c>NotifyNotification()</c> methods
/// from <c>ForegroundService.cs</c> (lines 95–120), adding:
/// <list type="bullet">
///   <item>Persistent deduplication — survives app restarts (fixes in-memory <c>HasNotify</c> flag bug)</item>
///   <item>Per-day dedup window — one notification per item per calendar day</item>
///   <item>Testable interface — implementations can be mocked</item>
/// </list>
/// </summary>
public interface IProximityNotificationService
{
    /// <summary>
    /// Send a proximity alert for an order if not already sent today.
    /// </summary>
    /// <returns><c>true</c> if sent; <c>false</c> if skipped (deduplication).</returns>
    Task<bool> NotifyOrderProximityAsync(Client client, ExtraOrder order);

    /// <summary>
    /// Send a proximity alert for a geofence notification if not already sent today.
    /// </summary>
    /// <returns><c>true</c> if sent; <c>false</c> if skipped (deduplication).</returns>
    Task<bool> NotifyGeofenceAlertAsync(Notification notification, Client client);

    /// <summary>
    /// Remove notification state records from previous days.
    /// Call once on app startup.
    /// </summary>
    void ClearExpiredState();
}

