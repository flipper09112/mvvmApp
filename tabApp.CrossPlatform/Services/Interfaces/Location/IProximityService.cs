using tabApp.Core.Models;
using tabApp.Core.Models.Notifications;

namespace tabApp.CrossPlatform.Services.Interfaces.Location;

/// <summary>
/// Proximity detection service — identifies which orders and notifications
/// fall within a given radius of a device's current location.
///
/// Designed as a cross-platform abstraction over HaversineCalculator.
/// Platform implementations inject this via MAUI DI.
/// </summary>
public interface IProximityService
{
    /// <summary>Default proximity alert radius in metres (matches ForegroundService.cs magic number 80).</summary>
    const double DefaultRadiusMetres = 80.0;

    /// <summary>
    /// Return all (Client, ExtraOrder) tuples whose delivery address falls
    /// within <paramref name="radiusMetres"/> of the given location.
    /// </summary>
    IReadOnlyList<(Client Client, ExtraOrder ExtraOrder)> GetOrdersInProximity(
        double latitude,
        double longitude,
        double radiusMetres = DefaultRadiusMetres);

    /// <summary>
    /// Return all notifications whose coordinates fall within
    /// <paramref name="radiusMetres"/> of the given location.
    /// </summary>
    IReadOnlyList<Notification> GetNotificationsInProximity(
        double latitude,
        double longitude,
        double radiusMetres = DefaultRadiusMetres);
}

