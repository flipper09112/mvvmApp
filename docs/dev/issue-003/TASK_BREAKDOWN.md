# ISSUE-003: Development Task Breakdown

**Status:** 📋 READY FOR ASSIGNMENT  
**Date:** 2026-02-19  
**Type:** Task List for Developers

---

## 🎯 Quick Reference - All Tasks

### PHASE 1: Analysis & Documentation (Days 1-2)

| Task ID | Title | Duration | Owner | Status |
|---------|-------|----------|-------|--------|
| 3.1 | Comprehensive ForegroundService Documentation | 1.5d | Tech Lead | ✅ COMPLETE |
| 3.2 | MAUI Background Task Architecture Design | 1.5d | Architect | 📋 Ready |
| 3.3 | Distance Calculation & Proximity Logic Analysis | 1d | Senior Dev | 📋 Ready |
| 3.4 | Notification State Management & Persistence Design | 1d | Tech Lead | 📋 Ready |

### PHASE 2: Proof of Concepts (Days 3-4)

| Task ID | Title | Duration | Owner | Status |
|---------|-------|----------|-------|--------|
| 3.5 | Background Location Tracking POC | 1.5d | Mobile Dev | 📋 Ready |
| 3.6 | Distance Calculation & Proximity Detection POC | 1d | Backend Dev | 📋 Ready |
| 3.7 | Notification State Persistence POC | 0.5d | Mobile Dev | 📋 Ready |
| 3.8 | DI Migration POC (MvxResolve → MAUI) | 0.5d | Architect | 📋 Ready |

### PHASE 3: Integration & Approval (Day 5)

| Task ID | Title | Duration | Owner | Status |
|---------|-------|----------|-------|--------|
| 3.9 | Integration Architecture Review | 1d | Tech Lead | 📋 Ready |
| 3.10 | Implementation Roadmap & Checklist | 0.5d | Tech Lead | 📋 Ready |
| 3.11 | Final Documentation & Archive | 0.5d | Tech Lead | 📋 Ready |

---

## 📝 Detailed Task Descriptions

### TASK 3.1: Comprehensive ForegroundService Documentation

**Owner:** Tech Lead  
**Duration:** 1.5 days  
**Priority:** P0 - Blocking for other tasks

**Objective:**
Document every aspect of the current ForegroundService implementation to establish a complete baseline for migration planning.

**Deliverables:**
1. `FOREGROUND_SERVICE_CODEBASE.md` - Line-by-line code analysis
2. `FEATURE_INVENTORY.md` - Feature list with scenarios
3. `INTEGRATION_POINTS.md` - Service dependency mapping
4. `CONFIGURATION_ANALYSIS.md` - Hardcoded values & settings
5. `PERMISSION_REQUIREMENTS.md` - Android & iOS permissions needed

**Checklist:**
- [ ] Read and understand all 373 lines of ForegroundService.cs
- [ ] Document each method's purpose & behavior
- [ ] Create flowchart of execution path
- [ ] List all service entry points (OnCreate, OnStartCommand, etc.)
- [ ] Identify unused/commented code
- [ ] Trace all calls to Mvx.Resolve<>
- [ ] Document exact Android manifest requirements
- [ ] Identify hardcoded magic numbers (80m, 1000ms, notification IDs)
- [ ] Note any TODO or FIXME comments in code
- [ ] Validate all 5 major features documented

**Acceptance Criteria:**
- [x] A developer unfamiliar with code can understand it from docs
- [x] All code paths traced and documented
- [x] All dependencies listed and explained
- [x] Ready for architecture team review

**Deliverables Created:**
- ✅ `TASK-3.1-FOREGROUND_SERVICE_ANALYSIS.md` - Complete codebase documentation
  - Code structure overview
  - 11 methods analyzed with implementation details
  - 9 issues identified and prioritized (1 critical, 4 high, 2 medium, 2 code quality)
  - Feature inventory (6 features)
  - Execution flow diagram
  - Dependency mapping
  - Configuration & constants analysis
  - Android manifest requirements
  - Total: 450+ lines of documentation

**Status:** ✅ COMPLETE (2026-02-19)

**Notes:**
- Reference: ForegroundService.cs (373 lines) - fully analyzed
- Related tasks: TASK-3.2 (uses this analysis for architecture design)

---

### TASK 3.2: MAUI Background Task Architecture Design

**Owner:** Architect / Senior Dev  
**Duration:** 1.5 days  
**Priority:** P0 - Critical for direction

**Objective:**
Design the MAUI equivalent architecture for ForegroundService, considering platform differences and MAUI capabilities/limitations.

**Deliverables:**
1. `MAUI_BACKGROUND_OPTIONS.md` - Comparison of 4+ approaches
2. `IBACKGROUND_LOCATION_SERVICE.cs` - Interface definition
3. `ANDROID_WORKMANAGER_DESIGN.md` - Android implementation design
4. `IOS_CLLOCATIONMANAGER_DESIGN.md` - iOS implementation design
5. `CROSS_PLATFORM_ABSTRACTION_DESIGN.md` - Architecture overview

**Subtask Breakdown:**

**3.2.1 - Compare MAUI Background Options**
- Research MAUI.Controls.Application.OnBackgroundResume
- Research Android WorkManager and how it integrates with MAUI
- Research iOS CLLocationManager background modes
- Create comparison table: Features, Pros, Cons, Battery Impact, Maintenance Burden
- Recommendation: Select option(s) to pursue
- Document: Why this option and not others

**3.2.2 - Design IBackgroundLocationService Interface**
```csharp
public interface IBackgroundLocationService
{
    // Core methods
    Task StartAsync();
    Task StopAsync();
    Task<Location> GetLastLocationAsync();
    
    // Events
    event EventHandler<LocationChangedEventArgs> LocationChanged;
    event EventHandler<ProximityAlertEventArgs> ProximityAlertTriggered;
    
    // Configuration
    TimeSpan UpdateInterval { get; set; }
    double ProximityThresholdMeters { get; set; }
}
```

**3.2.3 - Design Android Implementation**
- Use WorkManager for periodic background location updates
- Design PeriodLocationWorker class
- Explain: Can we use continuous tracking or only periodic?
- Discuss: Integration with ForegroundServiceType.Location
- Design: Fallback strategy if WorkManager insufficient

**3.2.4 - Design iOS Implementation**
- Use CLLocationManager with significant location changes
- Design: Background mode configuration (Info.plist)
- Design: Location accuracy/frequency tradeoff
- Discuss: Battery impact and optimization strategies
- Design: User notification for continuous location tracking

**3.2.5 - Design Cross-Platform Abstraction**
- Create architecture diagram showing both implementations
- Design: How MAUI app references IBackgroundLocationService
- Design: DI configuration for platform-specific implementations
- Design: Error handling (location unavailable, permission denied)
- Design: Lifecycle management

**Acceptance Criteria:**
- [ ] All 4 background options researched & documented
- [ ] Interface definition is complete & realistic
- [ ] Android & iOS strategies are achievable
- [ ] Abstraction layer properly decouples platforms
- [ ] Architecture review team agrees approach is sound

**Notes:**
- Depends on: TASK-3.1 (to understand current requirements)
- Useful references:
  - Microsoft MAUI docs on background tasks
  - Android WorkManager documentation
  - iOS CLLocationManager documentation

---

### TASK 3.3: Distance Calculation & Proximity Logic Analysis

**Owner:** Senior Dev / Backend Dev  
**Duration:** 1 day  
**Priority:** P1 - Important for correctness

**Objective:**
Validate the Haversine distance calculation, identify performance issues, and design improved proximity detection logic.

**Deliverables:**
1. `HAVERSINE_VALIDATION.md` - Algorithm validation report with test results
2. `PROXIMITY_PERFORMANCE_ANALYSIS.md` - Performance impact analysis
3. `DATA_MODEL_ISSUES.md` - Data model problems & proposed fixes
4. `PROXIMITY_SERVICE_DESIGN.md` - Improved service design
5. `PROXIMITY_UNIT_TESTS.md` - Unit test strategy

**Subtask Breakdown:**

**3.3.1 - Validate Haversine Implementation**
- Implement Haversine calculator as standalone function
- Test with known test vectors:
  - Same location (distance = 0m)
  - Known city pairs (NY-LA, London-Paris, etc.)
  - Equator crossing
  - Pole proximity
- Compare results with online tools (https://www.movable-type.co.uk/scripts/latlong.html)
- Verify accuracy within ±0.5%
- Output: Test vectors document + pass/fail results

**3.3.2 - Performance Impact Analysis**
- Current performance: ~500 calculations/second worst case
  - 200 orders × 2 checks/sec = 400 checks
  - 50 notifications × 2 checks/sec = 100 checks
- Measure: Time per Haversine calculation (should be < 1ms)
- Identify bottlenecks:
  - String parsing? (coordinates are strings)
  - Math operations?
  - Loop iterations?
- Propose optimizations:
  - Spatial indexing (O(n) → O(log n))
  - Caching parsed coordinates
  - Lazy evaluation
- Output: Performance analysis with measurements & recommendations

**3.3.3 - Data Model Issues**
- Current Issue #1: String-based coordinates
  - Order.Client.Address.Coordenadas = single string (format?)
  - Notification.Latitude/Longitude = separate strings
  - Should be decimal or double properties
- Current Issue #2: Empty string validation
  - Code: `if (!not.Latitude.Equals(string.Empty))`
  - Better: Use nullable types, check HasValue
- Current Issue #3: Inconsistent coordinate storage
  - Standardize: Use Location class with Latitude/Longitude properties
- Propose: Update data models (Address, Notification, etc.)
- Output: Data model issues document with proposed updates

**3.3.4 - Design ProximityService**
```csharp
public interface IProximityService
{
    // Calculate distance between two coordinates
    double CalculateDistance(double lat1, double lng1, double lat2, double lng2);
    
    // Find items within radius
    IEnumerable<Order> GetOrdersInProximity(Location location, double radiusMeters);
    IEnumerable<Notification> GetNotificationsInProximity(Location location, double radiusMeters);
}
```
- Design: Static Haversine calculation (reusable)
- Design: Data access layer (dependency injection)
- Design: Caching strategy for addresses
- Design: Unit testability

**3.3.5 - Unit Test Strategy**
- Test cases:
  - ✅ Calculate distance between known coordinates
  - ✅ Proximity trigger at threshold (80m)
  - ✅ Proximity not trigger above threshold
  - ✅ Multiple items in proximity
  - ✅ No items in proximity
  - ✅ Performance with 200+ items
  - ✅ Edge cases (null coordinates, invalid values)
- Output: Unit test spec document

**Acceptance Criteria:**
- [ ] Haversine algorithm validated
- [ ] Performance benchmarks completed
- [ ] Data model issues clearly documented
- [ ] ProximityService design approved
- [ ] Unit test strategy ready for implementation

**Notes:**
- Depends on: TASK-3.1
- Related to: TASK-3.6 (POC implementation)

---

### TASK 3.4: Notification State Management & Persistence Design

**Owner:** Tech Lead  
**Duration:** 1 day  
**Priority:** P1 - Important for reliability

**Objective:**
Design how notification deduplication and state persistence will work in MAUI to prevent duplicate notifications.

**Deliverables:**
1. `CURRENT_STATE_MANAGEMENT_ISSUES.md` - Current problems documentation
2. `PERSISTENT_STATE_DESIGN.md` - Storage strategy & justification
3. `IPROXIMITY_NOTIFICATION_SERVICE.cs` - Service interface definition
4. `NOTIFICATION_CONTENT_STRATEGY.md` - Content formatting rules
5. `DEDUPLICATION_ALGORITHM.md` - Algorithm specification

**Subtask Breakdown:**

**3.4.1 - Analyze Current State Management**
- Current: ExtraOrder.HasNotify & Notification.HasNotify flags
- Problem: Flags are in-memory, lost on app restart/crash
- Risk: User receives duplicate notifications after app restart
- Example scenario:
  1. Location update triggers notification for Order #123
  2. ExtraOrder.HasNotify = true (in memory)
  3. App crashes or restarts
  4. HasNotify flag lost
  5. Next location update: notification sent again for Order #123 (duplicate!)
- Output: Issues document with scenarios

**3.4.2 - Design Persistent Storage**
- Option A: MAUI Preferences
  ```csharp
  // Simple key-value store
  Preferences.Default.Set($"notified_order_{orderId}", true);
  bool alreadyNotified = Preferences.Default.Get($"notified_order_{orderId}", false);
  ```
  - Pros: Simple, built-in, persists across restarts
  - Cons: Only suitable for small data, no expiration
  - Use: For deduplication with fixed retention period

- Option B: Local SQLite Database
  ```csharp
  // Structured, queryable, supports expiration
  CREATE TABLE NotificationHistory (
      Id INTEGER PRIMARY KEY,
      ItemId INTEGER,
      ItemType TEXT,
      NotifiedAt DATETIME,
      ExpiresAt DATETIME
  );
  ```
  - Pros: Structured, queryable, can expire old records
  - Cons: More complex, requires migrations
  - Use: For detailed audit trail & complex queries

- Recommendation: Hybrid approach
  - Use Preferences for quick in-memory cache
  - Use SQLite for persistent history
  - Sync on app startup

- Output: Design document with pros/cons & justification

**3.4.3 - Design IProximityNotificationService**
```csharp
public interface IProximityNotificationService
{
    // Send notification for order proximity
    Task NotifyOrderProximity(Order order);
    
    // Send notification for geofence alert
    Task NotifyGeofenceAlert(Notification notification);
    
    // Mark notification as sent (for deduplication)
    Task MarkNotificationSent(int itemId, NotificationItemType itemType);
    
    // Check if already notified
    Task<bool> IsAlreadyNotified(int itemId, NotificationItemType itemType);
    
    // Get notification history (for debugging)
    Task<IEnumerable<NotificationRecord>> GetNotificationHistory(DateTime from, DateTime to);
    
    // Clear old notifications
    Task ClearExpiredNotifications();
}
```
- Include event/callback mechanism?
- Error handling strategy?
- Logging strategy?
- Output: Interface definition in C#

**3.4.4 - Design Notification Content**
- Current format for orders:
  - Title: "Client Name (Total/Extra)"
  - Body: Item listing with quantities
- Current format for notifications:
  - Title: "Client Name"
  - Body: notification.Info + optional extra value
- Design: Template-based content generation?
  - Pro: Easier to customize, localization-ready
  - Con: More complex code
- Design: Localization for Portuguese
  - "Total" vs "Extra" translation
  - Number formatting (1.500,00 vs 1,500.00)
- Output: Content strategy document

**3.4.5 - Design Deduplication Algorithm**
```
Algorithm: CheckProximity(order)
  IF order already notified TODAY
    SKIP notification
  ELSE
    SEND notification
    MarkAsNotified(order.Id, today)
```

Questions to answer:
- Time window: Per day, per session, per location update?
- Same location rule: If user stays in same location, suppress repeated notifications?
- User interaction: If user taps notification, mark as read?
- Output: Algorithm specification

**Acceptance Criteria:**
- [ ] Current state management issues documented
- [ ] Persistent storage strategy selected & justified
- [ ] IProximityNotificationService interface defined
- [ ] Notification content strategy documented
- [ ] Deduplication algorithm specified
- [ ] Team agrees on approach

**Notes:**
- Depends on: TASK-3.1
- Related to: TASK-3.7 (POC implementation)

---

### TASK 3.5: Background Location Tracking POC

**Owner:** Mobile Dev / Platform Specialist  
**Duration:** 1.5 days  
**Priority:** P1 - Validates feasibility

**Objective:**
Create proof-of-concept demonstrating MAUI location tracking in background on both Android and iOS.

**Deliverables:**
1. Working MAUI test app with background location tracking
2. `POC_FINDINGS_REPORT.md` - Analysis of what works, what doesn't, learnings

**Key Milestones:**
- [ ] Day 1: Android location tracking working
- [ ] Day 2: iOS location tracking working + findings report

**Android Implementation:**
```csharp
// Use WorkManager for periodic updates
public class LocationUpdateWorker : Worker
{
    public override Result DoWork()
    {
        // Get location
        var location = await Geolocation.GetLocationAsync();
        
        // Process location
        // (Send to background service, etc.)
        
        return Result.InvokeRetry();
    }
}

// In MauiProgram.cs
.AddLocationService(services =>
{
    var workRequest = new PeriodicWorkRequest
        .Builder(typeof(LocationUpdateWorker), TimeSpan.FromMinutes(15))
        .Build();
    
    WorkManager.Instance.EnqueueUniquePeriodicWork(
        "location_updates",
        ExistingPeriodicWorkPolicy.Keep,
        workRequest
    );
});
```

**iOS Implementation:**
```csharp
// Use CLLocationManager with significant location changes
var locationManager = new CLLocationManager();
locationManager.RequestAlwaysAndWhenInUseAuthorization();
locationManager.AllowsBackgroundLocationUpdates = true;
locationManager.PausesLocationUpdatesAutomatically = false;

// In Info.plist
<key>UIBackgroundModes</key>
<array>
    <string>location</string>
</array>
```

**Testing Approach:**
- Simulated locations using Android Studio emulator
- Real device testing on Android and iOS
- Document battery impact on real devices
- Monitor log output for location updates

**Acceptance Criteria:**
- [ ] Android: Location updates every 15 minutes while app backgrounded
- [ ] iOS: Location updates via significant change while app backgrounded
- [ ] Both: Proper permission handling & requesting
- [ ] Both: Error handling (location unavailable, permissions denied)
- [ ] POC findings report completed
- [ ] No blocking technical issues discovered

**Notes:**
- Depends on: TASK-3.2 (architecture design)
- Must test on real devices (emulator not sufficient)
- Document battery impact for decision-making

---

### TASK 3.6: Distance Calculation & Proximity Detection POC

**Owner:** Backend Dev / QA  
**Duration:** 1 day  
**Priority:** P1 - Core logic validation

**Objective:**
Create working proof-of-concept for proximity detection with unit tests and performance benchmarks.

**Deliverables:**
1. `HaversineCalculator.cs` - Extracted distance calculation (static)
2. `ProximityService.cs` - Proximity detection service
3. `ProximityServiceTests.cs` - Unit tests (with code)
4. `PROXIMITY_PERFORMANCE_BENCHMARK.md` - Performance results

**Implementation Outline:**

```csharp
// HaversineCalculator.cs
public static class HaversineCalculator
{
    public static double CalculateDistance(double lat1, double lng1, double lat2, double lng2)
    {
        const double earthRadiusMeters = 6371000;
        
        var dLat = (lat2 - lat1) * Math.PI / 180.0;
        var dLng = (lng2 - lng1) * Math.PI / 180.0;
        
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1 * Math.PI / 180.0) * Math.Cos(lat2 * Math.PI / 180.0) *
                Math.Sin(dLng / 2) * Math.Sin(dLng / 2);
        
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        
        return earthRadiusMeters * c;
    }
}

// ProximityService.cs
public interface IProximityService
{
    IEnumerable<Order> GetOrdersInProximity(Location location, double radiusMeters);
    IEnumerable<Notification> GetNotificationsInProximity(Location location, double radiusMeters);
}

public class ProximityService : IProximityService
{
    private readonly IOrdersManagerService _ordersService;
    private readonly INotificationsManagerService _notificationsService;
    
    public ProximityService(IOrdersManagerService ordersService, INotificationsManagerService notificationsService)
    {
        _ordersService = ordersService;
        _notificationsService = notificationsService;
    }
    
    public IEnumerable<Order> GetOrdersInProximity(Location location, double radiusMeters)
    {
        return _ordersService.TodayOrders.Where(order =>
        {
            if (order.Client?.Address?.Latitude == null) return false;
            
            var distance = HaversineCalculator.CalculateDistance(
                location.Latitude,
                location.Longitude,
                order.Client.Address.Latitude.Value,
                order.Client.Address.Longitude.Value
            );
            
            return distance <= radiusMeters;
        });
    }
    
    public IEnumerable<Notification> GetNotificationsInProximity(Location location, double radiusMeters)
    {
        // Similar implementation
    }
}
```

**Unit Tests:**
```csharp
[TestFixture]
public class ProximityServiceTests
{
    private ProximityService _service;
    
    [Test]
    public void CalculateDistance_SameLocation_ReturnsZero()
    {
        var distance = HaversineCalculator.CalculateDistance(40.7128, -74.0060, 40.7128, -74.0060);
        Assert.AreEqual(0, distance, 0.1);
    }
    
    [Test]
    public void GetOrdersInProximity_OrderWithin80m_ReturnsOrder()
    {
        // Arrange: Setup mock orders, location
        // Act: Call GetOrdersInProximity with 80m radius
        // Assert: Returns order
    }
    
    [Test]
    public void GetOrdersInProximity_OrderOutside80m_ReturnsEmpty()
    {
        // Similar structure
    }
    
    // More tests...
}
```

**Performance Benchmarking:**
- Measure: Time to process all orders (200) + notifications (50)
- Expected: < 500ms for single location update
- Document: Results & analysis

**Acceptance Criteria:**
- [ ] HaversineCalculator is correct (validated against known distances)
- [ ] ProximityService properly filters by distance
- [ ] All unit tests pass
- [ ] Performance meets expectations (< 500ms)
- [ ] Code is ready for integration

**Notes:**
- Depends on: TASK-3.3 (analysis) & TASK-3.1 (understand data models)
- Reuse: HaversineCalculator from current code (extract & clean up)

---

### TASK 3.7: Notification State Persistence POC

**Owner:** Mobile Dev  
**Duration:** 0.5 days  
**Priority:** P1 - Reliability feature

**Objective:**
Create working notification deduplication and state persistence system.

**Deliverables:**
1. `NotificationStateStore.cs` - Persistence layer
2. `DeduplicationService.cs` - Deduplication logic
3. `NotificationSender.cs` - Integration with MAUI LocalNotificationService
4. Unit tests for persistence & deduplication

**Implementation Outline:**

```csharp
// NotificationStateStore.cs - Simple Preferences-based (for POC)
public interface INotificationStateStore
{
    Task<bool> IsAlreadyNotified(string key);
    Task MarkNotified(string key);
    Task ClearExpired(DateTime before);
}

public class NotificationStateStore : INotificationStateStore
{
    public async Task<bool> IsAlreadyNotified(string key)
    {
        return await MainThread.InvokeOnMainThreadAsync(() =>
            Preferences.Default.Get(key, false)
        );
    }
    
    public async Task MarkNotified(string key)
    {
        await MainThread.InvokeOnMainThreadAsync(() =>
            Preferences.Default.Set(key, true)
        );
    }
    
    public async Task ClearExpired(DateTime before)
    {
        // For Preferences-based POC, we'll use a timestamp approach
        // Production: Use SQLite for proper expiration
    }
}

// DeduplicationService.cs
public class DeduplicationService
{
    private readonly INotificationStateStore _stateStore;
    
    public async Task<bool> ShouldNotify(int itemId, NotificationItemType type)
    {
        string key = $"notified_{type}_{itemId}_{DateTime.UtcNow:yyyyMMdd}";
        return !await _stateStore.IsAlreadyNotified(key);
    }
    
    public async Task MarkNotified(int itemId, NotificationItemType type)
    {
        string key = $"notified_{type}_{itemId}_{DateTime.UtcNow:yyyyMMdd}";
        await _stateStore.MarkNotified(key);
    }
}

// NotificationSender.cs
public class NotificationSender
{
    private readonly ILocalNotificationService _notificationService;
    private readonly DeduplicationService _deduplicationService;
    
    public async Task SendOrderProximityNotification(Order order)
    {
        if (!await _deduplicationService.ShouldNotify(order.Id, NotificationItemType.Order))
            return;
        
        var request = new NotificationRequest
        {
            Title = $"{order.Client.Name} ({(order.IsTotal ? "Total" : "Extra")})",
            Description = FormatOrderDetails(order),
            // ... other properties
        };
        
        await _notificationService.SendAsync(request);
        await _deduplicationService.MarkNotified(order.Id, NotificationItemType.Order);
    }
}
```

**Unit Tests:**
```csharp
[TestFixture]
public class DeduplicationServiceTests
{
    [Test]
    public async Task ShouldNotify_FirstTime_ReturnsTrue()
    {
        // Arrange
        var dedup = new DeduplicationService(stateStore);
        
        // Act
        var should = await dedup.ShouldNotify(123, NotificationItemType.Order);
        
        // Assert
        Assert.True(should);
    }
    
    [Test]
    public async Task ShouldNotify_SecondTime_ReturnsFalse()
    {
        // Arrange
        await dedup.MarkNotified(123, NotificationItemType.Order);
        
        // Act
        var should = await dedup.ShouldNotify(123, NotificationItemType.Order);
        
        // Assert
        Assert.False(should);
    }
    
    [Test]
    public async Task MarkNotified_PersistsAcrossRestart()
    {
        // This requires app restart - test manually or via integration test
    }
}
```

**Acceptance Criteria:**
- [ ] Notifications sent successfully via MAUI LocalNotificationService
- [ ] Duplicate notifications suppressed on first execution
- [ ] State persists across app restart (test manually)
- [ ] Unit tests pass
- [ ] Code is production-ready (with notes for SQLite upgrade)

**Notes:**
- Depends on: TASK-3.4 (design)
- Future: Upgrade from Preferences to SQLite for better expiration handling

---

### TASK 3.8: DI Migration POC (MvxResolve → MAUI IServiceProvider)

**Owner:** Architect / Senior Dev  
**Duration:** 0.5 days  
**Priority:** P1 - Critical for MVVM replacement

**Objective:**
Demonstrate that current service dependencies can be properly handled via MAUI DI without MvxResolve.

**Deliverables:**
1. `DIConfiguration.cs` - MAUI DI setup
2. Refactored ProximityService without Mvx.Resolve
3. Integration tests verifying DI resolution

**Implementation Outline:**

```csharp
// DIConfiguration.cs or in MauiProgram.cs
public static class DependencyInjectionExtensions
{
    public static MauiAppBuilder ConfigureLocationServices(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<IOrdersManagerService, OrdersManagerService>();
        builder.Services.AddSingleton<INotificationsManagerService, NotificationsManagerService>();
        builder.Services.AddSingleton<IClientsManagerService, ClientsManagerService>();
        builder.Services.AddSingleton<IProductsManagerService, ProductsManagerService>();
        
        builder.Services.AddSingleton<IProximityService, ProximityService>();
        builder.Services.AddSingleton<IProximityNotificationService, ProximityNotificationService>();
        builder.Services.AddSingleton<IBackgroundLocationService, BackgroundLocationService>();
        
        return builder;
    }
}

// In MauiProgram.cs
return MauiApp.CreateBuilder()
    .UseMauiApp<App>()
    .ConfigureLocationServices()
    .Build();

// ProximityService without Mvx.Resolve
public class ProximityService : IProximityService
{
    private readonly IOrdersManagerService _ordersService;
    private readonly INotificationsManagerService _notificationsService;
    
    public ProximityService(
        IOrdersManagerService ordersService,
        INotificationsManagerService notificationsService)
    {
        _ordersService = ordersService;
        _notificationsService = notificationsService;
    }
    
    // ... rest of implementation
}
```

**Integration Tests:**
```csharp
[TestFixture]
public class DIDependencyResolutionTests
{
    private MauiApp _app;
    
    [SetUp]
    public void Setup()
    {
        var appBuilder = MauiApp.CreateBuilder()
            .UseMauiApp<App>()
            .ConfigureLocationServices();
        
        _app = appBuilder.Build();
    }
    
    [Test]
    public void ResolveDependency_IProximityService_Succeeds()
    {
        var service = _app.Services.GetRequiredService<IProximityService>();
        Assert.NotNull(service);
    }
    
    [Test]
    public void ResolveDependency_AllServices_Succeed()
    {
        // Verify all services can be resolved
        var proximity = _app.Services.GetRequiredService<IProximityService>();
        var notification = _app.Services.GetRequiredService<IProximityNotificationService>();
        var location = _app.Services.GetRequiredService<IBackgroundLocationService>();
        
        Assert.NotNull(proximity);
        Assert.NotNull(notification);
        Assert.NotNull(location);
    }
    
    [Test]
    public void ResolveDependency_ProximityService_HasProperInjection()
    {
        var service = _app.Services.GetRequiredService<IProximityService>();
        
        // Verify it has the correct implementations
        Assert.IsInstanceOf<ProximityService>(service);
        // More specific assertions...
    }
}
```

**Migration Checklist:**
- [ ] All services registered in DI container
- [ ] No Mvx.Resolve calls remain in code
- [ ] Interfaces properly injected via constructor
- [ ] Dependency graph has no cycles
- [ ] All services can be resolved
- [ ] Tests pass

**Acceptance Criteria:**
- [ ] MAUI DI configuration complete
- [ ] No MvxResolve calls in codebase
- [ ] All services properly injectable
- [ ] Integration tests pass
- [ ] Documentation clear for future development

**Notes:**
- Depends on: TASK-3.1, TASK-3.2 (understand what needs injecting)
- Important: This removes one of biggest ISSUE-002 blocking items

---

### TASK 3.9: Integration Architecture Review

**Owner:** Tech Lead / Architect  
**Duration:** 1 day  
**Priority:** P0 - Gate for implementation

**Objective:**
Conduct comprehensive architecture review with team stakeholders to gain consensus on migration approach before implementation begins.

**Deliverables:**
1. Architecture review presentation
2. Review meeting notes
3. Approved architecture & any approved changes
4. `RISK_MITIGATION_PLAN.md` - Risk handling strategy

**Preparation (0.5 days):**
- [ ] Consolidate all analysis documents
- [ ] Create visual architecture diagrams
- [ ] Prepare 15-20 minute presentation
- [ ] List key decisions & their rationale
- [ ] Identify potential concerns & have answers ready
- [ ] Create executive summary (1-2 pages)

**Presentation Outline:**
1. **Current State Problem** (5 min)
   - ForegroundService limitations
   - MAUI incompatibility
   - Why we can't just migrate as-is

2. **MAUI Background Options** (5 min)
   - WorkManager for Android
   - CLLocationManager for iOS
   - Tradeoffs & why selected

3. **Proposed Architecture** (5 min)
   - IBackgroundLocationService abstraction
   - Component diagram
   - Integration points

4. **POC Results** (3 min)
   - What worked
   - What didn't
   - Learnings

5. **Risk & Mitigation** (2 min)
   - Top risks
   - How we'll mitigate

6. **Timeline & Effort** (1 min)
   - Phase breakdown
   - Effort estimates

7. **Q&A** (5 min)

**Review Meeting Attendees:**
- Tech Lead
- Architects
- Mobile Dev team leads
- Backend team lead
- Project Manager

**Post-Review Actions:**
- [ ] Incorporate feedback into architecture
- [ ] Document decisions made in meeting
- [ ] Identify any action items
- [ ] Get formal sign-off from stakeholders
- [ ] Create risk mitigation plan

**Risk Mitigation Plan Template:**
```
TOP 5 RISKS:
1. Risk: Background location less reliable than continuous service
   Mitigation: Design with user expectations in mind; accept periodic updates
   Owner: Architecture Team
   Monitor: Battery life metrics, location accuracy in testing

2. Risk: iOS battery drain from continuous location tracking
   Mitigation: Use significant change tracking, not continuous
   Owner: iOS Dev
   Monitor: Battery testing on real devices

3. Risk: WorkManager not sufficient for business needs
   Mitigation: Fallback to direct Service integration on Android
   Owner: Android Dev
   Monitor: Feasibility testing in POC phase

4. Risk: Permission request flow differs significantly
   Mitigation: Comprehensive permission testing; clear user communication
   Owner: UX/Mobile Dev
   Monitor: Permission denial rates in beta

5. Risk: Integration complexity with existing services
   Mitigation: Incremental migration; maintain backward compatibility
   Owner: Tech Lead
   Monitor: Code review, testing coverage
```

**Acceptance Criteria:**
- [ ] Architecture review completed successfully
- [ ] Team consensus achieved on approach
- [ ] All concerns addressed
- [ ] Formal sign-off obtained
- [ ] Risk mitigation plan documented
- [ ] Ready to begin implementation

**Notes:**
- This is final gate before ISSUE-004+ implementation starts
- Ensure all TASKS 3.1-3.8 completed before this

---

### TASK 3.10: Implementation Roadmap & Checklist

**Owner:** Tech Lead / Project Manager  
**Duration:** 0.5 days  
**Priority:** P1 - Planning for next phase

**Objective:**
Create detailed implementation roadmap breaking analysis into actual development work.

**Deliverables:**
1. `IMPLEMENTATION_CHECKLIST.md` - All tasks for developers
2. `TEST_STRATEGY.md` - QA testing approach
3. `PERFORMANCE_TEST_PLAN.md` - Performance validation
4. `ROLLOUT_PLAN.md` - Staged production rollout
5. `IMPLEMENTATION_GUIDELINES.md` - Coding standards for team

**IMPLEMENTATION_CHECKLIST Contents:**
- [ ] Core components to build (BackgroundLocationService, ProximityService, etc.)
- [ ] For each component:
  - [ ] Interface definition
  - [ ] Android implementation
  - [ ] iOS implementation
  - [ ] Unit tests
  - [ ] Integration tests
- [ ] Data model updates (string coordinates → decimal)
- [ ] DI configuration updates
- [ ] UI/ViewModel updates (if needed)
- [ ] Documentation updates

**TEST_STRATEGY Contents:**
- Unit test coverage targets: 80%+ for core logic
- Integration test scenarios:
  - Location acquisition & callback
  - Proximity detection with test data
  - Notification sending & deduplication
  - Permission request flows
  - Error scenarios
- Device test matrix:
  - Android: 7.0, 8.0, 10, 12, 13 (major versions)
  - iOS: 13, 14, 15, 16 (major versions)
- Real GPS testing: Use GPX files in emulator, real locations on devices

**PERFORMANCE_TEST_PLAN Contents:**
- Location update interval: Target 1-5 minute intervals
- Proximity detection latency: < 100ms from location update to check
- Notification delivery: < 500ms from trigger to display
- Battery impact: < 5% per hour of active tracking
- Memory usage: < 50MB background service
- Validation: Automated + manual testing

**ROLLOUT_PLAN Contents:**
- Phase 1 (Alpha): Internal team testing (1 week)
- Phase 2 (Beta): 100 beta users (1 week)
- Phase 3 (Staged): 50%, 75%, 100% rollout (2 weeks)
- Rollback plan: Revert to previous version if critical issues
- Monitoring: Error rates, battery impact, permission issues

**IMPLEMENTATION_GUIDELINES Contents:**
- Code style & naming conventions
- Exception handling requirements
- Logging strategy for debugging
- Documentation requirements (comments, doc strings)
- Code review checklist
- Testing requirements before merge

**Acceptance Criteria:**
- [ ] Implementation checklist is complete & prioritized
- [ ] Test strategy covers all scenarios
- [ ] Performance targets are realistic & documented
- [ ] Rollout plan is feasible
- [ ] Guidelines are clear & actionable
- [ ] Team can begin development immediately

**Notes:**
- This drives all future issues (ISSUE-004, etc.)
- Should be detailed enough that developers don't need clarification

---

### TASK 3.11: Final Documentation & Archive

**Owner:** Tech Lead  
**Duration:** 0.5 days  
**Priority:** P0 - Closes ISSUE-003

**Objective:**
Finalize all documentation, organize artifacts, and officially close ISSUE-003 analysis phase.

**Deliverables:**
1. `ISSUE-003-SUMMARY.md` - Executive summary
2. `QUICK_REFERENCE.md` - One-page cheat sheet
3. Organized `docs/tech/issue-003/` folder
4. Updated `docs/dev/issue-003/` folder
5. Updated `PHASE_0_ASSESSMENT.md`
6. `DEVELOPER_ONBOARDING.md` - For future developers

**ISSUE-003-SUMMARY.md Contents:**
- Problem statement (1 paragraph)
- Solution approach (2-3 paragraphs)
- Key design decisions & rationale (5-6 key items)
- Risk summary (top 3 risks & mitigation)
- Next steps & timeline
- Links to detailed docs

**QUICK_REFERENCE.md Contents:**
- Architecture diagram
- Service interface quick reference (one-liner per method)
- Key decisions table
- FAQ:
  - Q: Why not use MAUI Background Tasks exclusively?
  - A: ...
  - Q: Why WorkManager on Android?
  - A: ...
  - Q: What about iOS?
  - A: ...

**Folder Organization:**
```
docs/
├── tech/issue-003/          # Technical documentation
│   ├── README.md            # Overview
│   ├── MATRIZ_DETALHES.md   # Detailed analysis
│   ├── ACTION_PLAN.md       # All tasks
│   ├── QUICK_REFERENCE.md   # Cheat sheet
│   ├── ISSUE-003-SUMMARY.md # Executive summary
│   ├── FOREGROUND_SERVICE_CODEBASE.md
│   ├── FEATURE_INVENTORY.md
│   ├── INTEGRATION_POINTS.md
│   ├── CONFIGURATION_ANALYSIS.md
│   ├── PERMISSION_REQUIREMENTS.md
│   ├── MAUI_BACKGROUND_OPTIONS.md
│   ├── ANDROID_WORKMANAGER_DESIGN.md
│   ├── IOS_CLLOCATIONMANAGER_DESIGN.md
│   ├── CROSS_PLATFORM_ABSTRACTION_DESIGN.md
│   ├── HAVERSINE_VALIDATION.md
│   ├── PROXIMITY_PERFORMANCE_ANALYSIS.md
│   ├── DATA_MODEL_ISSUES.md
│   ├── PROXIMITY_SERVICE_DESIGN.md
│   ├── CURRENT_STATE_MANAGEMENT_ISSUES.md
│   ├── PERSISTENT_STATE_DESIGN.md
│   ├── NOTIFICATION_CONTENT_STRATEGY.md
│   ├── DEDUPLICATION_ALGORITHM.md
│   ├── POC_FINDINGS_REPORT.md
│   ├── PROXIMITY_PERFORMANCE_BENCHMARK.md
│   ├── RISK_MITIGATION_PLAN.md
│   ├── ARCHITECTURE_SUMMARY.md
│   ├── IMPLEMENTATION_CHECKLIST.md
│   ├── TEST_STRATEGY.md
│   ├── PERFORMANCE_TEST_PLAN.md
│   ├── ROLLOUT_PLAN.md
│   └── IMPLEMENTATION_GUIDELINES.md
│
├── dev/issue-003/           # Development artifacts
│   ├── README.md            # Dev summary
│   ├── TASK_BREAKDOWN.md    # This file
│   ├── IBACKGROUND_LOCATION_SERVICE.cs
│   ├── IPROXIMITY_NOTIFICATION_SERVICE.cs
│   ├── HaversineCalculator.cs
│   ├── ProximityService.cs
│   ├── NotificationStateStore.cs
│   ├── DeduplicationService.cs
│   ├── DIConfiguration.cs
│   └── [POC code files]
```

**PHASE_0_ASSESSMENT.md Update:**
- Mark ISSUE-003 as ✅ COMPLETE
- Update status:
  - Completed: 2026-02-24 (5 days)
  - Duration vs estimate: "5 days (vs 5 estimated)" ✅ ON SCHEDULE
  - Status: All objectives met
- Note ISSUE-004 as next priority

**DEVELOPER_ONBOARDING.md Contents:**
- "If you need to understand ISSUE-003..."
- Key documents to read (in order)
- Key design decisions (5-6)
- Common questions (FAQ)
- Where to find code examples
- Who to ask for help

**Archive Tasks:**
- [ ] All documents in `docs/` with proper links
- [ ] Code examples with syntax highlighting
- [ ] Diagrams visible (not just links)
- [ ] No broken links or references
- [ ] All TODO/FIXME items addressed or documented
- [ ] Ready for hand-off to development team

**Acceptance Criteria:**
- [ ] ISSUE-003 officially closed
- [ ] All documentation organized & accessible
- [ ] No orphaned or loose documents
- [ ] Developer can understand entire scope in 1-2 hours
- [ ] Team ready to begin ISSUE-004 & implementation

**Notes:**
- This closes the analysis phase
- Next: Implementation phase with ISSUE-004+ and individual development tasks
- Archive should be "reference material" for future development

---

## 📅 Recommended Schedule

### Week 1: Analysis Phase (Days 1-2, Mon-Tue)
- Monday:
  - 9:00-12:00 AM: TASK-3.1 (Documentation) & TASK-3.2 (Architecture Design) kickoff
  - 2:00-5:00 PM: TASK-3.3 (Proximity Analysis) & TASK-3.4 (State Management) kickoff
- Tuesday:
  - All-day: Continue Tasks 3.1-3.4, review & finalize documentation

### Week 1: POC Phase (Days 3-4, Wed-Thu)
- Wednesday:
  - 9:00-12:00 AM: TASK-3.5 (Location Tracking POC) & TASK-3.6 (Proximity POC) start
  - 2:00-5:00 PM: TASK-3.7 (Notification POC) & TASK-3.8 (DI POC) start
- Thursday:
  - All-day: Continue POCs, fix issues, document findings

### Week 1: Review Phase (Day 5, Fri)
- Friday:
  - 9:00-11:00 AM: TASK-3.9 (Architecture Review meeting)
  - 11:00 AM-12:30 PM: TASK-3.10 (Implementation Planning)
  - 1:00-3:00 PM: TASK-3.11 (Documentation & Archive)
  - 3:00-5:00 PM: Buffer time for cleanup, issue resolution

---

## 🎯 Metrics & Success Indicators

### Analysis Phase Success:
- ✅ All 4 analysis documents complete & approved
- ✅ Team consensus on architectural approach
- ✅ No ambiguity about current ForegroundService behavior
- ✅ Clear migration path defined

### POC Phase Success:
- ✅ All 4 POCs functional & documented
- ✅ No blocking technical obstacles discovered
- ✅ Feasibility validated for both Android & iOS
- ✅ Learnings documented for implementation team

### Review & Planning Success:
- ✅ Architecture formally approved
- ✅ Team consensus on approach
- ✅ Implementation roadmap clear
- ✅ Ready to begin ISSUE-004+ development

---

## 📌 Key Contacts & Owners

| Task | Primary Owner | Secondary |
|------|---------------|-----------|
| TASK-3.1 | Tech Lead | Senior Dev |
| TASK-3.2 | Architect | Tech Lead |
| TASK-3.3 | Senior Dev | QA Lead |
| TASK-3.4 | Tech Lead | DB Specialist |
| TASK-3.5 | Mobile Dev | Platform Specialist |
| TASK-3.6 | Backend Dev | QA |
| TASK-3.7 | Mobile Dev | - |
| TASK-3.8 | Architect | Senior Dev |
| TASK-3.9 | Tech Lead | Architect |
| TASK-3.10 | Tech Lead | PM |
| TASK-3.11 | Tech Lead | - |

---

**Last Updated:** 2026-02-19  
**Status:** Ready for team assignment  
**Questions?** Contact Tech Lead

