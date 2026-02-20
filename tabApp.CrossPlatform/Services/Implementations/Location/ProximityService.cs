using tabApp.Core.Models;
using tabApp.Core.Models.Notifications;
using tabApp.Core.Services.Interfaces.Notifications;
using tabApp.Core.Services.Interfaces.Orders;
using tabApp.CrossPlatform.Services.Interfaces.Location;
using tabApp.CrossPlatform.Services.Location;

namespace tabApp.CrossPlatform.Services.Implementations.Location;

/// <summary>
/// Cross-platform proximity detection service.
///
/// Replaces the two inner GetDistance() overloads in ForegroundService.cs (lines 123–159)
/// and the proximity-check loops in CheckIfClosestOrder() (lines 170–230).
///
/// Key improvements over the legacy implementation:
///   - Dependencies injected via constructor (no Mvx.Resolve in hot path)
///   - Single HaversineCalculator.CalculateMetres() call (no code duplication)
///   - Coordinate parsing isolated to HaversineCalculator.TryParse*() (no scattered double.Parse())
///   - Returns typed result lists instead of producing side effects inline
///
/// Performance: O(n) — acceptable for current data volumes (~250 items total).
///   Future: replace with spatial index when item count > 1 000.
/// </summary>
public sealed class ProximityService : IProximityService
{
    private readonly IOrdersManagerService _ordersService;
    private readonly INotificationsManagerService _notificationsService;

    public ProximityService(
        IOrdersManagerService ordersService,
        INotificationsManagerService notificationsService)
    {
        _ordersService = ordersService ?? throw new ArgumentNullException(nameof(ordersService));
        _notificationsService = notificationsService ?? throw new ArgumentNullException(nameof(notificationsService));
    }

    /// <inheritdoc />
    public IReadOnlyList<(Client Client, ExtraOrder ExtraOrder)> GetOrdersInProximity(
        double latitude,
        double longitude,
        double radiusMetres = IProximityService.DefaultRadiusMetres)
    {
        var result = new List<(Client, ExtraOrder)>();

        var todayOrders = _ordersService.TodayOrders;
        if (todayOrders is null || todayOrders.Count == 0)
            return result;

        foreach (var (client, extraOrder) in todayOrders)
        {
            if (client is null) continue;
            var address = client.Address;
            if (address is null)
                continue;

            // Parse the legacy "Coordenadas" string — isolated here, never in the hot-path loop
            if (!HaversineCalculator.TryParseAddress(address.Coordenadas,
                                                     out var addrLat,
                                                     out var addrLon))
                continue;

            var distanceMetres = HaversineCalculator.CalculateMetres(
                latitude, longitude,
                addrLat, addrLon);

            if (distanceMetres <= radiusMetres)
                result.Add((client!, extraOrder));
        }

        return result;
    }

    /// <inheritdoc />
    public IReadOnlyList<Notification> GetNotificationsInProximity(
        double latitude,
        double longitude,
        double radiusMetres = IProximityService.DefaultRadiusMetres)
    {
        var result = new List<Notification>();

        var todayNotifications = _notificationsService.TodayNotifications;
        if (todayNotifications is null || todayNotifications.Count == 0)
            return result;

        foreach (var notification in todayNotifications)
        {
            // Guard: skip records with empty coordinates (matches legacy !not.Latitude.Equals(string.Empty) check)
            if (!HaversineCalculator.TryParseStrings(notification.Latitude,
                                                     notification.Longitude,
                                                     out var notLat,
                                                     out var notLon))
                continue;

            var distanceMetres = HaversineCalculator.CalculateMetres(
                latitude, longitude,
                notLat, notLon);

            if (distanceMetres <= radiusMetres)
                result.Add(notification);
        }

        return result;
    }
}



