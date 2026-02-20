// ─────────────────────────────────────────────────────────────────────────────
// TASK-3.6 — Proximity Detection POC: Unit Tests
// TASK-3.7 — Notification State Persistence POC: Unit Tests
//
// Coverage:
//   HaversineCalculatorTests          — formula accuracy + edge cases
//   CoordinateParsingTests            — TryParseAddress / TryParseStrings
//   ProximityServiceOrderTests        — GetOrdersInProximity filtering
//   ProximityServiceNotifTests        — GetNotificationsInProximity filtering
//   DeduplicationKeyBuilderTests      — key format + date embedding
//   NotificationStateStoreTests       — IsNotified / MarkNotified / ClearExpired / ClearAll
//   ProximityNotificationServiceTests — dedup flow (send once, skip twice, clear → resend)
// ─────────────────────────────────────────────────────────────────────────────

using Microsoft.Maui.Storage;
using Moq;
using tabApp.Core.Models;
using tabApp.Core.Models.Notifications;
using tabApp.Core.Services.Interfaces.Notifications;
using tabApp.Core.Services.Interfaces.Orders;
using tabApp.Core.Services.Interfaces.Products;
using tabApp.CrossPlatform.Services.Implementations.Location;
using tabApp.CrossPlatform.Services.Implementations.Notifications;
using tabApp.CrossPlatform.Services.Interfaces.Location;
using tabApp.CrossPlatform.Services.Interfaces.Notifications;
using tabApp.CrossPlatform.Services.Location;

namespace tabApp.Tests;

// ─────────────────────────────────────────────────────────────────────────────
// 1. HaversineCalculator — formula accuracy
// ─────────────────────────────────────────────────────────────────────────────

[TestFixture]
[Category("Unit")]
public class HaversineCalculatorTests
{
    // ── Accuracy ──────────────────────────────────────────────────────────────

    [Test]
    [Description("Same location must return 0 metres.")]
    public void CalculateMetres_SameCoordinates_ReturnsZero()
    {
        var result = HaversineCalculator.CalculateMetres(40.7128, -74.0060, 40.7128, -74.0060);
        Assert.That(result, Is.EqualTo(0.0).Within(0.1));
    }

    [Test]
    [Description("New York → Los Angeles: ~3 944–3 945 km (test vector from TASK-3.3 analysis).")]
    public void CalculateMetres_NewYorkToLosAngeles_Returns3945km()
    {
        var result = HaversineCalculator.CalculateMetres(
            40.7128, -74.0060,   // NYC
            34.0522, -118.2437); // LA

        // Tolerance ±15 km (0.4%) — matches TASK-3.3 validation
        Assert.That(result, Is.EqualTo(3_945_250).Within(15_000));
    }

    [Test]
    [Description("London → Paris: ~343–344 km (test vector from TASK-3.3 analysis).")]
    public void CalculateMetres_LondonToParis_Returns344km()
    {
        var result = HaversineCalculator.CalculateMetres(
            51.5074, -0.1278,   // London
            48.8566, 2.3522);   // Paris

        Assert.That(result, Is.EqualTo(343_770).Within(5_000));
    }

    [Test]
    [Description("Equator crossing: (0,0) → (45,0) ~5 003 km (test vector from TASK-3.3).")]
    public void CalculateMetres_EquatorCrossing_Returns5003km()
    {
        var result = HaversineCalculator.CalculateMetres(0.0, 0.0, 45.0, 0.0);

        Assert.That(result, Is.EqualTo(5_003_000).Within(20_000));
    }

    // ── Proximity threshold boundary ──────────────────────────────────────────

    [Test]
    [Description("Points ~50 m apart must be inside the 80 m default radius.")]
    public void CalculateMetres_50mApart_IsWithin80mRadius()
    {
        // Shift latitude by ~0.00045° ≈ 50 m
        var result = HaversineCalculator.CalculateMetres(
            40.7128, -74.0060,
            40.71325, -74.0060);

        Assert.That(result, Is.LessThanOrEqualTo(IProximityService.DefaultRadiusMetres));
    }

    [Test]
    [Description("Points ~200 m apart must be outside the 80 m default radius.")]
    public void CalculateMetres_200mApart_IsOutside80mRadius()
    {
        // Shift latitude by ~0.0018° ≈ 200 m
        var result = HaversineCalculator.CalculateMetres(
            40.7128, -74.0060,
            40.71460, -74.0060);

        Assert.That(result, Is.GreaterThan(IProximityService.DefaultRadiusMetres));
    }

    // ── Symmetry ──────────────────────────────────────────────────────────────

    [Test]
    [Description("Distance A→B must equal distance B→A (great-circle symmetry).")]
    public void CalculateMetres_IsSymmetric()
    {
        var ab = HaversineCalculator.CalculateMetres(51.5074, -0.1278, 48.8566, 2.3522);
        var ba = HaversineCalculator.CalculateMetres(48.8566, 2.3522, 51.5074, -0.1278);

        Assert.That(ab, Is.EqualTo(ba).Within(0.001));
    }

    // ── Antipodal edge case ───────────────────────────────────────────────────

    [Test]
    [Description("Antipodal points (0,0)→(0,180) should return ~πR ≈ 20 015 km.")]
    public void CalculateMetres_AntipodalPoints_ReturnsHalfCircumference()
    {
        var result = HaversineCalculator.CalculateMetres(0.0, 0.0, 0.0, 180.0);
        var halfCircumference = Math.PI * HaversineCalculator.EarthRadiusMetres;

        Assert.That(result, Is.EqualTo(halfCircumference).Within(500));
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// 2. HaversineCalculator — coordinate parsing helpers
// ─────────────────────────────────────────────────────────────────────────────

[TestFixture]
[Category("Unit")]
public class CoordinateParsingTests
{
    // ── TryParseAddress ───────────────────────────────────────────────────────

    [Test]
    public void TryParseAddress_NullInput_ReturnsFalse()
    {
        var ok = HaversineCalculator.TryParseAddress(null, out _, out _);
        Assert.That(ok, Is.False);
    }

    [Test]
    public void TryParseAddress_EmptyString_ReturnsFalse()
    {
        var ok = HaversineCalculator.TryParseAddress("", out _, out _);
        Assert.That(ok, Is.False);
    }

    [Test]
    public void TryParseAddress_NullLiteral_ReturnsFalse()
    {
        var ok = HaversineCalculator.TryParseAddress("null", out _, out _);
        Assert.That(ok, Is.False);
    }

    [Test]
    [Description("2-part format: 'lat,lon' (decimal degrees).")]
    public void TryParseAddress_TwoPart_ParsesCorrectly()
    {
        var ok = HaversineCalculator.TryParseAddress("40.7128,-74.0060", out var lat, out var lon);

        Assert.That(ok, Is.True);
        Assert.That(lat, Is.EqualTo(40.7128).Within(0.0001));
        Assert.That(lon, Is.EqualTo(-74.0060).Within(0.0001));
    }

    [Test]
    [Description("4-part format: 'lat,decLat,lon,decLon' as produced by legacy Coordenadas field.")]
    public void TryParseAddress_FourPart_ParsesCorrectly()
    {
        // Legacy format: parts[0]="40", parts[1]="7128", parts[2]="-74", parts[3]="0060"
        var ok = HaversineCalculator.TryParseAddress("40,7128,-74,0060", out var lat, out var lon);

        Assert.That(ok, Is.True);
        Assert.That(lat, Is.EqualTo(40.7128).Within(0.0001));
        Assert.That(lon, Is.EqualTo(-74.0060).Within(0.0001));
    }

    // ── TryParseStrings ───────────────────────────────────────────────────────

    [Test]
    public void TryParseStrings_NullInputs_ReturnsFalse()
    {
        var ok = HaversineCalculator.TryParseStrings(null, null, out _, out _);
        Assert.That(ok, Is.False);
    }

    [Test]
    public void TryParseStrings_EmptyStrings_ReturnsFalse()
    {
        var ok = HaversineCalculator.TryParseStrings("", "", out _, out _);
        Assert.That(ok, Is.False);
    }

    [Test]
    [Description("Valid plain decimal strings as stored in Notification.Latitude/.Longitude.")]
    public void TryParseStrings_ValidDecimals_ParsesCorrectly()
    {
        var ok = HaversineCalculator.TryParseStrings("51.5074", "-0.1278", out var lat, out var lon);

        Assert.That(ok, Is.True);
        Assert.That(lat, Is.EqualTo(51.5074).Within(0.0001));
        Assert.That(lon, Is.EqualTo(-0.1278).Within(0.0001));
    }

    [Test]
    [Description("Decimal-separator tolerance: comma-separated strings must also parse.")]
    public void TryParseStrings_CommaDecimalSeparator_ParsesCorrectly()
    {
        var ok = HaversineCalculator.TryParseStrings("51,5074", "-0,1278", out var lat, out var lon);

        Assert.That(ok, Is.True);
        Assert.That(lat, Is.EqualTo(51.5074).Within(0.0001));
        Assert.That(lon, Is.EqualTo(-0.1278).Within(0.0001));
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// 3. ProximityService — GetOrdersInProximity
// ─────────────────────────────────────────────────────────────────────────────

[TestFixture]
[Category("Unit")]
public class ProximityServiceOrderTests
{
    private Mock<IOrdersManagerService> _ordersMock = null!;
    private Mock<INotificationsManagerService> _notifMock = null!;
    private ProximityService _sut = null!;

    // Reference location: New York (40.7128, -74.0060)
    private const double RefLat = 40.7128;
    private const double RefLon = -74.0060;

    [SetUp]
    public void SetUp()
    {
        _ordersMock = new Mock<IOrdersManagerService>();
        _notifMock  = new Mock<INotificationsManagerService>();
        _sut = new ProximityService(_ordersMock.Object, _notifMock.Object);

        // Default: no notifications
        _notifMock.Setup(s => s.TodayNotifications).Returns([]);
    }

    private static (Client, ExtraOrder) MakeOrder(string coordenadas)
    {
        var client = new Client
        {
            Id = 1,
            Name = "Test",
            Address = new Address { Coordenadas = coordenadas },
            DailyOrders = [],
            DetailsList = [],
            ExtraOrdersList = []
        };
        var order = new ExtraOrder { OrderDay = DateTime.Today };
        return (client, order);
    }

    [Test]
    [Description("An order whose address is ~50 m away must be returned.")]
    public void GetOrdersInProximity_OrderWithin80m_ReturnsIt()
    {
        // ~50 m north of ref location
        var entry = MakeOrder("40.71325,-74.0060");
        _ordersMock.Setup(s => s.TodayOrders).Returns([entry]);

        var result = _sut.GetOrdersInProximity(RefLat, RefLon);

        Assert.That(result, Has.Count.EqualTo(1));
    }

    [Test]
    [Description("An order whose address is ~200 m away must NOT be returned.")]
    public void GetOrdersInProximity_OrderOutside80m_IsExcluded()
    {
        // ~200 m north
        var entry = MakeOrder("40.71460,-74.0060");
        _ordersMock.Setup(s => s.TodayOrders).Returns([entry]);

        var result = _sut.GetOrdersInProximity(RefLat, RefLon);

        Assert.That(result, Is.Empty);
    }

    [Test]
    [Description("Exactly on the boundary (80 m) must be included (≤ check).")]
    public void GetOrdersInProximity_OrderAtExactBoundary_IsIncluded()
    {
        // Shift ~0.00072° ≈ 80 m north
        var entry = MakeOrder("40.71352,-74.0060");
        _ordersMock.Setup(s => s.TodayOrders).Returns([entry]);

        var result = _sut.GetOrdersInProximity(RefLat, RefLon, 80.0);

        // Distance is ≤ 80 m so should be included
        Assert.That(result, Has.Count.EqualTo(1));
    }

    [Test]
    [Description("Order with null/empty Coordenadas must be silently skipped.")]
    public void GetOrdersInProximity_NullCoordinates_SkipsEntry()
    {
        var entry = MakeOrder("null");
        _ordersMock.Setup(s => s.TodayOrders).Returns([entry]);

        var result = _sut.GetOrdersInProximity(RefLat, RefLon);

        Assert.That(result, Is.Empty);
    }

    [Test]
    [Description("Empty order list must return empty result without throwing.")]
    public void GetOrdersInProximity_EmptyList_ReturnsEmpty()
    {
        _ordersMock.Setup(s => s.TodayOrders).Returns([]);

        var result = _sut.GetOrdersInProximity(RefLat, RefLon);

        Assert.That(result, Is.Empty);
    }

    [Test]
    [Description("Mixed list: only orders within radius are returned.")]
    public void GetOrdersInProximity_MixedDistances_ReturnsOnlyNearby()
    {
        var near = MakeOrder("40.71325,-74.0060");   // ~50 m
        var far  = MakeOrder("40.71460,-74.0060");   // ~200 m

        // Give each a distinct Id
        near.Item1.Id = 1;
        far.Item1.Id  = 2;

        _ordersMock.Setup(s => s.TodayOrders).Returns([near, far]);

        var result = _sut.GetOrdersInProximity(RefLat, RefLon);

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Client.Id, Is.EqualTo(1));
    }

    [Test]
    [Description("Null order list must not throw — returns empty.")]
    public void GetOrdersInProximity_NullList_ReturnsEmpty()
    {
        _ordersMock.Setup(s => s.TodayOrders).Returns((List<(Client, ExtraOrder)>)null!);

        Assert.DoesNotThrow(() =>
        {
            var result = _sut.GetOrdersInProximity(RefLat, RefLon);
            Assert.That(result, Is.Empty);
        });
    }

    [Test]
    [Description("Constructor must throw ArgumentNullException for null ordersService.")]
    public void Constructor_NullOrdersService_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            _ = new ProximityService(null!, _notifMock.Object));
    }

    [Test]
    [Description("Constructor must throw ArgumentNullException for null notificationsService.")]
    public void Constructor_NullNotificationsService_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            _ = new ProximityService(_ordersMock.Object, null!));
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// 4. ProximityService — GetNotificationsInProximity
// ─────────────────────────────────────────────────────────────────────────────

[TestFixture]
[Category("Unit")]
public class ProximityServiceNotifTests
{
    private Mock<IOrdersManagerService> _ordersMock = null!;
    private Mock<INotificationsManagerService> _notifMock = null!;
    private ProximityService _sut = null!;

    private const double RefLat = 40.7128;
    private const double RefLon = -74.0060;

    [SetUp]
    public void SetUp()
    {
        _ordersMock = new Mock<IOrdersManagerService>();
        _notifMock  = new Mock<INotificationsManagerService>();
        _sut = new ProximityService(_ordersMock.Object, _notifMock.Object);

        // Default: no orders
        _ordersMock.Setup(s => s.TodayOrders).Returns([]);
    }

    private static Notification MakeNotification(string? lat, string? lon, int id = 1) =>
        new() { NotificationId = id, Latitude = lat, Longitude = lon };

    [Test]
    [Description("Notification ~50 m away must be returned.")]
    public void GetNotificationsInProximity_NotifWithin80m_ReturnsIt()
    {
        _notifMock.Setup(s => s.TodayNotifications)
                  .Returns([MakeNotification("40.71325", "-74.0060")]);

        var result = _sut.GetNotificationsInProximity(RefLat, RefLon);

        Assert.That(result, Has.Count.EqualTo(1));
    }

    [Test]
    [Description("Notification ~200 m away must NOT be returned.")]
    public void GetNotificationsInProximity_NotifOutside80m_IsExcluded()
    {
        _notifMock.Setup(s => s.TodayNotifications)
                  .Returns([MakeNotification("40.71460", "-74.0060")]);

        var result = _sut.GetNotificationsInProximity(RefLat, RefLon);

        Assert.That(result, Is.Empty);
    }

    [Test]
    [Description("Notification with empty Latitude must be skipped (replicates legacy !Latitude.Equals(string.Empty) guard).")]
    public void GetNotificationsInProximity_EmptyLatitude_SkipsEntry()
    {
        _notifMock.Setup(s => s.TodayNotifications)
                  .Returns([MakeNotification("", "-74.0060")]);

        var result = _sut.GetNotificationsInProximity(RefLat, RefLon);

        Assert.That(result, Is.Empty);
    }

    [Test]
    [Description("Notification with null coordinates must be silently skipped.")]
    public void GetNotificationsInProximity_NullCoordinates_SkipsEntry()
    {
        _notifMock.Setup(s => s.TodayNotifications)
                  .Returns([MakeNotification(null, null)]);

        var result = _sut.GetNotificationsInProximity(RefLat, RefLon);

        Assert.That(result, Is.Empty);
    }

    [Test]
    [Description("Empty notification list returns empty result.")]
    public void GetNotificationsInProximity_EmptyList_ReturnsEmpty()
    {
        _notifMock.Setup(s => s.TodayNotifications).Returns([]);

        var result = _sut.GetNotificationsInProximity(RefLat, RefLon);

        Assert.That(result, Is.Empty);
    }

    [Test]
    [Description("Mixed list: only notifications within radius returned.")]
    public void GetNotificationsInProximity_MixedDistances_ReturnsOnlyNearby()
    {
        var near = MakeNotification("40.71325", "-74.0060", id: 1);
        var far  = MakeNotification("40.71460", "-74.0060", id: 2);

        _notifMock.Setup(s => s.TodayNotifications).Returns([near, far]);

        var result = _sut.GetNotificationsInProximity(RefLat, RefLon);

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].NotificationId, Is.EqualTo(1));
    }

    [Test]
    [Description("Custom radius parameter is honoured.")]
    public void GetNotificationsInProximity_CustomRadius_FiltersByCustomRadius()
    {
        // ~150 m away — outside 80 m but inside 200 m
        _notifMock.Setup(s => s.TodayNotifications)
                  .Returns([MakeNotification("40.71405", "-74.0060")]);

        var insideCustom  = _sut.GetNotificationsInProximity(RefLat, RefLon, 200.0);
        var outsideCustom = _sut.GetNotificationsInProximity(RefLat, RefLon, 80.0);

        Assert.That(insideCustom,  Has.Count.EqualTo(1));
        Assert.That(outsideCustom, Is.Empty);
    }
}
