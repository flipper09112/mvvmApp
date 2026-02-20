// ─────────────────────────────────────────────────────────────────────────────
// TASK-3.7 — Notification State Persistence POC: Unit Tests
//
// Coverage:
//   InMemoryKeyValueStore             — test-double for IKeyValueStore
//   DeduplicationKeyBuilderTests      — key format + date embedding
//   NotificationStateStoreTests       — IsNotified / MarkNotified / ClearExpired / ClearAll
//   ProximityNotificationServiceTests — dedup flow (send once, skip, clear → resend)
// ─────────────────────────────────────────────────────────────────────────────

using Moq;
using tabApp.Core.Models;
using tabApp.Core.Models.Notifications;
using tabApp.Core.Services.Interfaces.Products;
using tabApp.CrossPlatform.Services.Implementations.Notifications;
using tabApp.CrossPlatform.Services.Interfaces.Notifications;

namespace tabApp.Tests;

// ─────────────────────────────────────────────────────────────────────────────
// In-memory IKeyValueStore test double — no MAUI runtime required
// ─────────────────────────────────────────────────────────────────────────────

internal sealed class InMemoryKeyValueStore : IKeyValueStore
{
    private readonly Dictionary<string, object> _store = new();

    public bool GetBool(string key, bool defaultValue = false)
        => _store.TryGetValue(key, out var v) ? (bool)v : defaultValue;

    public void SetBool(string key, bool value) => _store[key] = value;

    public string GetString(string key, string defaultValue = "")
        => _store.TryGetValue(key, out var v) ? (string)v : defaultValue;

    public void SetString(string key, string value) => _store[key] = value;

    public void Remove(string key) => _store.Remove(key);
}

// ─────────────────────────────────────────────────────────────────────────────
// 5. DeduplicationKeyBuilder
// ─────────────────────────────────────────────────────────────────────────────

[TestFixture]
[Category("Unit")]
public class DeduplicationKeyBuilderTests
{
    [Test]
    [Description("Order key must embed type, id and the supplied date.")]
    public void ForOrder_ReturnsCorrectFormat()
    {
        var key = DeduplicationKeyBuilder.ForOrder(42, new DateTime(2026, 2, 20));
        Assert.That(key, Is.EqualTo("Order_42_2026-02-20"));
    }

    [Test]
    [Description("Notification key must embed type, id and the supplied date.")]
    public void ForNotification_ReturnsCorrectFormat()
    {
        var key = DeduplicationKeyBuilder.ForNotification(7, new DateTime(2026, 2, 20));
        Assert.That(key, Is.EqualTo("Notification_7_2026-02-20"));
    }

    [Test]
    [Description("Keys for different order IDs on the same day must be distinct.")]
    public void ForOrder_DifferentIds_ProduceDifferentKeys()
    {
        var d = new DateTime(2026, 2, 20);
        Assert.That(DeduplicationKeyBuilder.ForOrder(1, d),
                    Is.Not.EqualTo(DeduplicationKeyBuilder.ForOrder(2, d)));
    }

    [Test]
    [Description("Same order ID on different days must produce different keys.")]
    public void ForOrder_SameIdDifferentDate_ProducesDifferentKey()
    {
        var k1 = DeduplicationKeyBuilder.ForOrder(42, new DateTime(2026, 2, 20));
        var k2 = DeduplicationKeyBuilder.ForOrder(42, new DateTime(2026, 2, 21));
        Assert.That(k1, Is.Not.EqualTo(k2));
    }

    [Test]
    [Description("No-date overload defaults to DateTime.Today without throwing.")]
    public void ForOrder_NoDate_DoesNotThrow()
    {
        Assert.DoesNotThrow(() => DeduplicationKeyBuilder.ForOrder(1));
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// 6. PreferencesNotificationStateStore
// ─────────────────────────────────────────────────────────────────────────────

[TestFixture]
[Category("Unit")]
public class NotificationStateStoreTests
{
    private InMemoryKeyValueStore _kvStore = null!;
    private PreferencesNotificationStateStore _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _kvStore = new InMemoryKeyValueStore();
        _sut     = new PreferencesNotificationStateStore(_kvStore);
    }

    [Test]
    [Description("Fresh store — key absent — IsNotified returns false.")]
    public void IsNotified_FreshStore_ReturnsFalse()
    {
        var key = DeduplicationKeyBuilder.ForOrder(1, new DateTime(2026, 2, 20));
        Assert.That(_sut.IsNotified(key), Is.False);
    }

    [Test]
    [Description("After MarkNotified, IsNotified returns true for the same key.")]
    public void MarkNotified_ThenIsNotified_ReturnsTrue()
    {
        var key = DeduplicationKeyBuilder.ForOrder(1, new DateTime(2026, 2, 20));
        _sut.MarkNotified(key);
        Assert.That(_sut.IsNotified(key), Is.True);
    }

    [Test]
    [Description("Marking the same key twice must not throw or corrupt state.")]
    public void MarkNotified_Twice_StaysTrue()
    {
        var key = DeduplicationKeyBuilder.ForOrder(1, new DateTime(2026, 2, 20));
        _sut.MarkNotified(key);
        _sut.MarkNotified(key);
        Assert.That(_sut.IsNotified(key), Is.True);
    }

    [Test]
    [Description("ClearAll removes all keys — IsNotified returns false afterwards.")]
    public void ClearAll_RemovesAllKeys()
    {
        var k1 = DeduplicationKeyBuilder.ForOrder(1, new DateTime(2026, 2, 20));
        var k2 = DeduplicationKeyBuilder.ForNotification(7, new DateTime(2026, 2, 20));
        _sut.MarkNotified(k1);
        _sut.MarkNotified(k2);

        _sut.ClearAll();

        Assert.That(_sut.IsNotified(k1), Is.False);
        Assert.That(_sut.IsNotified(k2), Is.False);
    }

    [Test]
    [Description("ClearExpired removes past-date keys and retains today's key.")]
    public void ClearExpired_RemovesYesterdayKeepsToday()
    {
        var oldKey     = DeduplicationKeyBuilder.ForOrder(1, DateTime.Today.AddDays(-1));
        var currentKey = DeduplicationKeyBuilder.ForOrder(2, DateTime.Today);

        _sut.MarkNotified(oldKey);
        _sut.MarkNotified(currentKey);

        _sut.ClearExpired();

        Assert.That(_sut.IsNotified(oldKey),     Is.False, "Yesterday's key should be removed");
        Assert.That(_sut.IsNotified(currentKey), Is.True,  "Today's key should be retained");
    }

    [Test]
    [Description("ClearExpired on an empty store must not throw.")]
    public void ClearExpired_EmptyStore_DoesNotThrow()
    {
        Assert.DoesNotThrow(() => _sut.ClearExpired());
    }

    [Test]
    [Description("TryExtractDate parses the date from a well-formed storage key.")]
    public void TryExtractDate_ValidKey_ParsesCorrectly()
    {
        var storageKey = PreferencesNotificationStateStore.KeyPrefix + "Order_42_2026-02-20";
        var ok = PreferencesNotificationStateStore.TryExtractDate(storageKey, out var date);

        Assert.That(ok,   Is.True);
        Assert.That(date, Is.EqualTo(new DateTime(2026, 2, 20)));
    }

    [Test]
    [Description("TryExtractDate returns false for a key with no parseable date segment.")]
    public void TryExtractDate_MalformedKey_ReturnsFalse()
    {
        var ok = PreferencesNotificationStateStore.TryExtractDate("no_date_here", out _);
        Assert.That(ok, Is.False);
    }

    [Test]
    [Description("Constructor must throw ArgumentNullException for null store.")]
    public void Constructor_NullStore_Throws()
    {
        Assert.Throws<ArgumentNullException>(() =>
            _ = new PreferencesNotificationStateStore(null!));
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// 7. ProximityNotificationService — deduplication flow
// ─────────────────────────────────────────────────────────────────────────────

[TestFixture]
[Category("Unit")]
public class ProximityNotificationServiceTests
{
    private PreferencesNotificationStateStore _store = null!;
    private Mock<ILocalNotificationSender>    _senderMock   = null!;
    private Mock<IProductsManagerService>     _productsMock = null!;
    private ProximityNotificationService      _sut = null!;

    [SetUp]
    public void SetUp()
    {
        _store        = new PreferencesNotificationStateStore(new InMemoryKeyValueStore());
        _senderMock   = new Mock<ILocalNotificationSender>();
        _productsMock = new Mock<IProductsManagerService>();

        _senderMock
            .Setup(s => s.SendAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        _productsMock
            .Setup(s => s.GetProductById(It.IsAny<int>()))
            .Returns(new Product { Name = "Milk", Unity = true, Id = 1 });

        _sut = new ProximityNotificationService(_store, _senderMock.Object, _productsMock.Object);
    }

    // ── Helpers ────────────────────────────────────────────────────────────────

    private static (Client Client, ExtraOrder Order) MakeOrder(int orderId = 1)
    {
        var client = new Client
        {
            Id = 1, Name = "João Silva",
            ExtraOrdersList = [], DailyOrders = [], DetailsList = []
        };
        var order = new ExtraOrder
        {
            Id       = orderId,
            OrderDay = DateTime.Today,
            IsTotal  = true,
            AllItems = [new DailyOrderDetails { ProductId = 1, Ammount = 2 }]
        };
        return (client, order);
    }

    private static (Notification Notification, Client Client) MakeGeofence(int id = 1)
    {
        var notif = new Notification
        {
            NotificationId   = id,
            ClientId         = 1,
            Info             = "Check account",
            NotificationType = NotificationTypeEnum.None,
            AlertDay         = DateTime.Today
        };
        var client = new Client
        {
            Id = 1, Name = "Maria Santos",
            ExtraOrdersList = [], DailyOrders = [], DetailsList = []
        };
        return (notif, client);
    }

    // ── Order proximity ────────────────────────────────────────────────────────

    [Test]
    [Description("First call for an order must send and return true.")]
    public async Task NotifyOrder_FirstTime_SendsAndReturnsTrue()
    {
        var (client, order) = MakeOrder();
        var result = await _sut.NotifyOrderProximityAsync(client, order);

        Assert.That(result, Is.True);
        _senderMock.Verify(s => s.SendAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    [Description("Second call for same order same day must be skipped (dedup).")]
    public async Task NotifyOrder_SecondTime_SkipsAndReturnsFalse()
    {
        var (client, order) = MakeOrder();
        await _sut.NotifyOrderProximityAsync(client, order);
        var result = await _sut.NotifyOrderProximityAsync(client, order);

        Assert.That(result, Is.False);
        _senderMock.Verify(s => s.SendAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    [Description("Two different orders on the same day must each be notified independently.")]
    public async Task NotifyOrder_TwoDifferentOrders_BothSent()
    {
        var (c1, o1) = MakeOrder(orderId: 1);
        var (c2, o2) = MakeOrder(orderId: 2);
        c2.Id = 2;

        var r1 = await _sut.NotifyOrderProximityAsync(c1, o1);
        var r2 = await _sut.NotifyOrderProximityAsync(c2, o2);

        Assert.That(r1, Is.True);
        Assert.That(r2, Is.True);
        _senderMock.Verify(s => s.SendAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
    }

    [Test]
    [Description("After ClearAll state is wiped — same order must be sendable again.")]
    public async Task NotifyOrder_AfterClearAll_SendsAgain()
    {
        var (client, order) = MakeOrder();
        await _sut.NotifyOrderProximityAsync(client, order);
        _store.ClearAll();
        var result = await _sut.NotifyOrderProximityAsync(client, order);

        Assert.That(result, Is.True);
        _senderMock.Verify(s => s.SendAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(2));
    }

    // ── Geofence notification ─────────────────────────────────────────────────

    [Test]
    [Description("First geofence alert must send and return true.")]
    public async Task NotifyGeofence_FirstTime_SendsAndReturnsTrue()
    {
        var (notif, client) = MakeGeofence();
        var result = await _sut.NotifyGeofenceAlertAsync(notif, client);

        Assert.That(result, Is.True);
        _senderMock.Verify(s => s.SendAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    [Description("Second geofence alert for same notification same day must be skipped.")]
    public async Task NotifyGeofence_SecondTime_SkipsAndReturnsFalse()
    {
        var (notif, client) = MakeGeofence();
        await _sut.NotifyGeofenceAlertAsync(notif, client);
        var result = await _sut.NotifyGeofenceAlertAsync(notif, client);

        Assert.That(result, Is.False);
        _senderMock.Verify(s => s.SendAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
    }

    [Test]
    [Description("DontPay notification body must include the ExtraValueToPay amount.")]
    public async Task NotifyGeofence_DontPayType_BodyContainsExtraValue()
    {
        var (notif, client) = MakeGeofence();
        notif.NotificationType = NotificationTypeEnum.DontPay;
        client.ExtraValueToPay = 50.0;

        string? capturedMessage = null;
        _senderMock
            .Setup(s => s.SendAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
            .Callback<int, string, string>((_, _, msg) => capturedMessage = msg)
            .Returns(Task.CompletedTask);

        await _sut.NotifyGeofenceAlertAsync(notif, client);

        Assert.That(capturedMessage, Does.Contain("50"));
    }

    [Test]
    [Description("ClearExpiredState delegates to the store without throwing.")]
    public void ClearExpiredState_DoesNotThrow()
    {
        Assert.DoesNotThrow(() => _sut.ClearExpiredState());
    }

    // ── Constructor guards ────────────────────────────────────────────────────

    [Test]
    public void Constructor_NullStateStore_Throws() =>
        Assert.Throws<ArgumentNullException>(() =>
            _ = new ProximityNotificationService(null!, _senderMock.Object, _productsMock.Object));

    [Test]
    public void Constructor_NullSender_Throws() =>
        Assert.Throws<ArgumentNullException>(() =>
            _ = new ProximityNotificationService(_store, null!, _productsMock.Object));

    [Test]
    public void Constructor_NullProductsService_Throws() =>
        Assert.Throws<ArgumentNullException>(() =>
            _ = new ProximityNotificationService(_store, _senderMock.Object, null!));
}


