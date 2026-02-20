using tabApp.Core.Models;
using tabApp.Core.Models.Notifications;
using tabApp.Core.Services.Interfaces.Products;
using tabApp.CrossPlatform.Services.Interfaces.Notifications;

namespace tabApp.CrossPlatform.Services.Implementations.Notifications;

/// <summary>
/// Cross-platform proximity notification service with persistent per-day deduplication.
///
/// Replaces the inline notification logic in ForegroundService.cs (lines 95–120):
/// <code>
///   // Legacy — broken: HasNotify lost on restart
///   if (!order.ExtraOrder.HasNotify) { order.ExtraOrder.HasNotify = true; notify(); }
/// </code>
///
/// This implementation:
/// <list type="bullet">
///   <item>Persists notification state via <see cref="INotificationStateStore"/> (MAUI Preferences POC)</item>
///   <item>Deduplicates per calendar day using <see cref="DeduplicationKeyBuilder"/></item>
///   <item>Formats notification content per TASK-3.4 §Notification Content Strategy</item>
///   <item>Delegates actual send to <see cref="ILocalNotificationSender"/> (mockable)</item>
/// </list>
/// </summary>
public sealed class ProximityNotificationService : IProximityNotificationService
{
    private readonly INotificationStateStore _stateStore;
    private readonly ILocalNotificationSender _sender;
    private readonly IProductsManagerService _productsService;

    // Notification IDs must be unique per category to avoid stacking
    private const int OrderNotificationBaseId        = 2000;
    private const int GeofenceNotificationBaseId     = 3000;

    public ProximityNotificationService(
        INotificationStateStore stateStore,
        ILocalNotificationSender sender,
        IProductsManagerService productsService)
    {
        _stateStore      = stateStore      ?? throw new ArgumentNullException(nameof(stateStore));
        _sender          = sender          ?? throw new ArgumentNullException(nameof(sender));
        _productsService = productsService ?? throw new ArgumentNullException(nameof(productsService));
    }

    // ── IProximityNotificationService ─────────────────────────────────────────

    /// <inheritdoc />
    public async Task<bool> NotifyOrderProximityAsync(Client client, ExtraOrder order)
    {
        var key = DeduplicationKeyBuilder.ForOrder(order.Id);

        if (_stateStore.IsNotified(key))
            return false; // Already notified today — skip

        var title   = FormatOrderTitle(client, order);
        var message = FormatOrderBody(order);

        await _sender.SendAsync(OrderNotificationBaseId + order.Id, title, message);

        _stateStore.MarkNotified(key);
        return true;
    }

    /// <inheritdoc />
    public async Task<bool> NotifyGeofenceAlertAsync(Notification notification, Client client)
    {
        var key = DeduplicationKeyBuilder.ForNotification(notification.NotificationId);

        if (_stateStore.IsNotified(key))
            return false; // Already notified today — skip

        var title   = client.Name;
        var message = FormatGeofenceBody(notification, client);

        await _sender.SendAsync(GeofenceNotificationBaseId + notification.NotificationId, title, message);

        _stateStore.MarkNotified(key);
        return true;
    }

    /// <inheritdoc />
    public void ClearExpiredState() => _stateStore.ClearExpired();

    // ── Content formatting (TASK-3.4 §Notification Content Strategy) ──────────

    private string FormatOrderTitle(Client client, ExtraOrder order)
    {
        var orderType = order.IsTotal ? "Total" : "Extra";
        return $"{client.Name} ({orderType})";
    }

    private string FormatOrderBody(ExtraOrder order)
    {
        if (order.AllItems is { Count: 0 })
            return string.Empty;

        var lines = new List<string>(order.AllItems.Count);
        foreach (var item in order.AllItems)
        {
            if (item.ProductId == 0) continue; // guard: skip unresolvable items
            var product = _productsService.GetProductById(item.ProductId);

            var quantity = product.Unity
                ? item.Ammount.ToString("N0")
                : item.Ammount.ToString("N2");

            lines.Add($"{product.Name} - {quantity}");
        }

        return string.Join("\n", lines);
    }

    private static string FormatGeofenceBody(Notification notification, Client client)
    {
        var body = notification.Info;  // string — may be empty

        if (notification.NotificationType == NotificationTypeEnum.DontPay
            && client.ExtraValueToPay > 0)
        {
            body += $"\n\nValue(Nos extras) : {client.ExtraValueToPay:C}";
        }

        return body;
    }
}







