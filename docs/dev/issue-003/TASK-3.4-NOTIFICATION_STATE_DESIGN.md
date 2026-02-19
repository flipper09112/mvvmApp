# TASK-3.4: Notification State Management & Persistence Design

**Status:** 🚀 IN PROGRESS  
**Date:** 2026-02-19  
**Owner:** Tech Lead  
**Duration:** 1 day  
**Priority:** P1 - Reliability Feature  
**Depends On:** TASK-3.1 (✅ COMPLETE)

---

## 🎯 Objective

Design notification deduplication and state persistence strategy for MAUI migration, ensuring users receive alerts only once despite app restarts or background re-execution.

---

## ❌ Current State Management Issues (TASK-3.1 Finding)

### Problem 1: In-Memory HasNotify Flag

**Location:** Lines 114, 168 in ForegroundService.cs

```csharp
// Current Implementation - BROKEN
private void NotifyNotification(IClientsManagerService clientsManagerService, Notification not)
{
    if (_notificationHelper == null)
        _notificationHelper = new NotificationHelper(ApplicationContext);
    
    if (!not.HasNotify)  // ⚠️ IN-MEMORY FLAG!
    {
        not.HasNotify = true;  // Set in RAM only
        // ... send notification
    }
}

private void NotifyOrder((Client Client, ExtraOrder ExtraOrder) order)
{
    if(!order.ExtraOrder.HasNotify)  // ⚠️ IN-MEMORY FLAG!
    {
        order.ExtraOrder.HasNotify = true;  // Set in RAM only
        // ... send notification
    }
}
```

### Impact Analysis

**Scenario 1: App Restart**
```
Time 0:00 - Location update @ 100m from Order #123
           - Order #123 HasNotify = false
           - Check: true, send notification
           - Order #123 HasNotify = true (in RAM)
           ✅ User receives notification

Time 0:15 - App crashes/restarts
           - Order #123 loaded from DB
           - Order #123 HasNotify = false (lost!)
           ⚠️ Flag reset when reloading from database

Time 0:30 - Another location update @ 100m from Order #123
           - Order #123 HasNotify = false
           - Check: true, send notification AGAIN!
           ❌ DUPLICATE notification!
```

**Scenario 2: Service Re-execution**
```
Android WorkManager restarts the location task:
- Old state lost (in-memory)
- New task sees HasNotify = false
- Sends duplicate notification
- User frustrated with repeated alerts
```

---

## 🏗️ Storage Strategy Comparison

### Option 1: MAUI Preferences (Simple Key-Value Store)

**What it is:**
- Cross-platform key-value storage
- Backed by platform-specific implementations:
  - Android: SharedPreferences
  - iOS: NSUserDefaults
- Persistent across app restarts
- Limited to simple types (string, int, bool, double)

**Implementation Pattern:**
```csharp
public class PreferencesNotificationStateStore
{
    public bool IsAlreadyNotified(string key)
    {
        // Key format: "notified_order_123_2026-02-19"
        return Preferences.Default.Get(key, false);
    }
    
    public void MarkNotified(string key)
    {
        Preferences.Default.Set(key, true);
    }
}

// Usage
var key = $"notified_order_{order.Id}_{DateTime.Now:yyyy-MM-dd}";
if (!store.IsAlreadyNotified(key))
{
    SendNotification(order);
    store.MarkNotified(key);
}
```

**Pros:**
- ✅ Built-in MAUI API
- ✅ Cross-platform
- ✅ Simple to use
- ✅ Zero setup
- ✅ Good for POC

**Cons:**
- ❌ No expiration support (keys persist forever)
- ❌ No structured queries
- ❌ Scale issues (thousands of keys)
- ❌ Manual cleanup needed

**Best Use:** Simple deduplication for POC  
**Performance:** Good (< 50ms per read/write)

---

### Option 2: SQLite Database (Structured, Queryable)

**What it is:**
- Embedded SQL database
- Relational schema
- Full query support
- TTL/expiration support

**Implementation Pattern:**
```csharp
public class SqliteNotificationStateStore
{
    private const string ConnectionString = "Filename=notification_state.db3";
    
    public void CreateTable()
    {
        using (var conn = new SqliteConnection(ConnectionString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS NotificationState (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ItemId INTEGER NOT NULL,
                    ItemType TEXT NOT NULL,  -- 'Order' or 'Notification'
                    NotifiedAt DATETIME NOT NULL,
                    ExpiresAt DATETIME NOT NULL,
                    UNIQUE(ItemId, ItemType, ExpiresAt)
                );
                CREATE INDEX IF NOT EXISTS idx_expiry ON NotificationState(ExpiresAt);
            ";
            cmd.ExecuteNonQuery();
        }
    }
    
    public bool IsAlreadyNotified(int itemId, string itemType, DateTime date)
    {
        using (var conn = new SqliteConnection(ConnectionString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT COUNT(*) FROM NotificationState
                WHERE ItemId = @itemId 
                  AND ItemType = @itemType
                  AND DATE(NotifiedAt) = @date
                  AND ExpiresAt > @now
            ";
            cmd.Parameters.AddWithValue("@itemId", itemId);
            cmd.Parameters.AddWithValue("@itemType", itemType);
            cmd.Parameters.AddWithValue("@date", date.Date);
            cmd.Parameters.AddWithValue("@now", DateTime.UtcNow);
            
            return (int)cmd.ExecuteScalar() > 0;
        }
    }
    
    public void MarkNotified(int itemId, string itemType, int expirationDays = 1)
    {
        using (var conn = new SqliteConnection(ConnectionString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO NotificationState (ItemId, ItemType, NotifiedAt, ExpiresAt)
                VALUES (@itemId, @itemType, @now, @expiry)
            ";
            cmd.Parameters.AddWithValue("@itemId", itemId);
            cmd.Parameters.AddWithValue("@itemType", itemType);
            cmd.Parameters.AddWithValue("@now", DateTime.UtcNow);
            cmd.Parameters.AddWithValue("@expiry", DateTime.UtcNow.AddDays(expirationDays));
            
            cmd.ExecuteNonQuery();
        }
    }
    
    public void ClearExpiredNotifications()
    {
        using (var conn = new SqliteConnection(ConnectionString))
        {
            conn.Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = "DELETE FROM NotificationState WHERE ExpiresAt < @now";
            cmd.Parameters.AddWithValue("@now", DateTime.UtcNow);
            cmd.ExecuteNonQuery();
        }
    }
}
```

**Pros:**
- ✅ Expiration support (automatic cleanup)
- ✅ Structured queries
- ✅ Better performance at scale
- ✅ Query history
- ✅ No app/process dependencies

**Cons:**
- ❌ More complex setup
- ❌ Database migrations needed
- ❌ Overkill for simple deduplication
- ❌ Slight performance overhead vs Preferences

**Best Use:** Production with many notifications  
**Performance:** Good (< 100ms per query with index)

---

### Option 3: Hybrid (Recommended) ✅

**Strategy:**
- Use MAUI Preferences for in-memory cache (fast)
- Use SQLite for persistent history (reliable)
- Sync on app startup
- Clear old records daily

**Implementation Pattern:**
```csharp
public class HybridNotificationStateStore
{
    private readonly SqliteNotificationStateStore _database;
    private readonly Dictionary<string, bool> _cache;
    
    public async Task InitializeAsync()
    {
        // Load today's notifications from DB into cache
        _cache.Clear();
        var todayNotifications = _database.GetNotificationsForDate(DateTime.Today);
        foreach (var notif in todayNotifications)
        {
            var key = $"{notif.ItemType}_{notif.ItemId}";
            _cache[key] = true;
        }
        
        // Clear old records
        _database.ClearExpiredNotifications();
    }
    
    public bool IsAlreadyNotified(int itemId, string itemType)
    {
        var key = $"{itemType}_{itemId}";
        
        // Fast path: check cache first
        if (_cache.TryGetValue(key, out var cached))
            return cached;
        
        // Slow path: check database
        var result = _database.IsAlreadyNotified(itemId, itemType, DateTime.Today);
        
        // Cache for future lookups
        if (result)
            _cache[key] = true;
        
        return result;
    }
    
    public void MarkNotified(int itemId, string itemType)
    {
        // Update cache
        var key = $"{itemType}_{itemId}";
        _cache[key] = true;
        
        // Update database
        _database.MarkNotified(itemId, itemType);
    }
}
```

**Why Hybrid:**
- ✅ Cache provides sub-millisecond lookup
- ✅ Database provides persistence
- ✅ Sync on startup ensures consistency
- ✅ Best of both worlds
- ✅ Production-ready

---

## 🏗️ IProximityNotificationService Interface Design

**File:** `tabApp.Core/Services/Interfaces/IProximityNotificationService.cs`

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using tabApp.Core.Models;
using tabApp.Core.Models.Notifications;

namespace tabApp.Core.Services.Interfaces
{
    /// <summary>
    /// Service for sending proximity-based notifications with deduplication.
    /// 
    /// Responsibilities:
    /// - Send notifications when proximity triggered
    /// - Prevent duplicate notifications
    /// - Manage notification state persistence
    /// - Track notification history
    /// 
    /// Deduplication Strategy:
    /// - Per-day: Each item notified maximum once per day
    /// - Notification state persisted to survive app restart
    /// - Hybrid caching: Fast in-memory cache + SQLite backup
    /// </summary>
    public interface IProximityNotificationService
    {
        // ========== Core Methods ==========
        
        /// <summary>
        /// Send notification when order proximity triggered.
        /// Includes deduplication check.
        /// 
        /// Returns: true if notification sent, false if skipped (duplicate)
        /// </summary>
        Task<bool> NotifyOrderProximityAsync(Order order, LocationData location);
        
        /// <summary>
        /// Send notification when geofence proximity triggered.
        /// Includes deduplication check.
        /// 
        /// Returns: true if notification sent, false if skipped (duplicate)
        /// </summary>
        Task<bool> NotifyGeofenceAlertAsync(Notification notification, LocationData location);
        
        // ========== State Management ==========
        
        /// <summary>
        /// Mark notification as sent (for deduplication).
        /// Persists to storage.
        /// </summary>
        Task MarkNotificationSentAsync(int itemId, NotificationItemType itemType);
        
        /// <summary>
        /// Check if notification already sent today.
        /// </summary>
        Task<bool> IsAlreadyNotifiedAsync(int itemId, NotificationItemType itemType);
        
        /// <summary>
        /// Get notification history for date range.
        /// For debugging/analytics.
        /// </summary>
        Task<IEnumerable<NotificationRecord>> GetHistoryAsync(DateTime from, DateTime to);
        
        // ========== Maintenance ==========
        
        /// <summary>
        /// Clear notifications older than retention period.
        /// Call daily or on startup.
        /// </summary>
        Task ClearExpiredNotificationsAsync(int retentionDays = 1);
        
        /// <summary>
        /// Manually clear all notification state.
        /// Use for testing/debugging only.
        /// </summary>
        Task ClearAllAsync();
        
        // ========== Configuration ==========
        
        /// <summary>
        /// Proximity threshold for triggering alerts.
        /// Default: 80 meters
        /// </summary>
        double ProximityThresholdMeters { get; set; }
        
        /// <summary>
        /// Notification deduplication period.
        /// Default: 1 day (per-day deduplication)
        /// </summary>
        TimeSpan DeduplicationPeriod { get; set; }
    }
    
    // ========== Supporting Types ==========
    
    public enum NotificationItemType
    {
        Order,
        Notification
    }
    
    public class NotificationRecord
    {
        public int ItemId { get; set; }
        public NotificationItemType ItemType { get; set; }
        public DateTime NotifiedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
```

---

## 📋 Deduplication Algorithm Design

### Algorithm: Per-Day Notification Deduplication

```
Function: ShouldSendNotification(itemId, itemType)
  INPUT: itemId (int), itemType (Order|Notification)
  OUTPUT: boolean (true = send, false = skip)
  
  STEPS:
  1. Get current date (start of day)
  2. Build deduplication key: "{itemType}_{itemId}_{date}"
  3. Check persistent store:
     IF key exists AND not expired:
       RETURN false  -- Already notified today, skip
     ELSE:
       RETURN true   -- First notification today, send
  
  4. After sending notification:
     - Store key with expiration = end of day
     - Update in-memory cache
     - Persist to database
     - Log notification sent

Function: ClearExpiredNotifications()
  INPUT: none
  OUTPUT: number of records deleted
  
  STEPS:
  1. Query database: WHERE ExpiresAt < NOW
  2. Delete matching records
  3. Log cleanup results
  4. Clear cache entries
```

### Example Execution

```
Scenario: User moves near Order #123 three times in one day

TIME 08:00
- Location: 100m from Order #123
- Check dedup key: "Order_123_2026-02-19"
- Result: NOT found (first time)
- Action: SEND notification ✅
- Store: Set key with expiry = 2026-02-20 00:00

TIME 10:00
- Location: 100m from Order #123 (moved closer again)
- Check dedup key: "Order_123_2026-02-19"
- Result: FOUND and not expired
- Action: SKIP notification (duplicate) ⏭️
- Reason: Already notified once today

TIME 23:59
- Location: 100m from Order #123 (still nearby)
- Check dedup key: "Order_123_2026-02-19"
- Result: FOUND and not expired (key expires at 00:00)
- Action: SKIP notification ⏭️

TIME 00:00 (next day)
- Automatic cleanup job runs
- Delete: "Order_123_2026-02-19" (expired)
- Result: Key removed from store

TIME 08:00 (next day)
- Location: 100m from Order #123
- Check dedup key: "Order_123_2026-02-20"
- Result: NOT found (first time on new day)
- Action: SEND notification ✅
- Reason: New deduplication window
```

---

## 📊 Notification Content Strategy

### Order Proximity Alert

**Title:** `"{ClientName} ({OrderType})"`
- Example: "John Silva (Total)"
- Example: "Maria Santos (Extra)"

**Body:** Formatted order details
```
Product 1 - Quantity
Product 2 - Quantity
Product 3 - Quantity
...
```

**Example:**
```
Title: "John Silva (Total)"
Body:  "Milk - 2
        Bread - 1
        Cheese - 500g"
```

### Geofence Alert

**Title:** `"{ClientName}"`
- Example: "John Silva"

**Body:** Notification info + optional extra value
```
[Notification Info]
[Value for DontPay type]
```

**Example (Normal):**
```
Title: "John Silva"
Body:  "Check account balance"
```

**Example (DontPay type):**
```
Title: "John Silva"
Body:  "Check account balance
        
        Value(Nos extras) : €50.00"
```

### Content Formatting Implementation

```csharp
public class NotificationContentFormatter
{
    public static string FormatOrderTitle(Order order)
    {
        var orderType = order.ExtraOrder.IsTotal ? "Total" : "Extra";
        return $"{order.Client.Name} ({orderType})";
    }
    
    public static string FormatOrderBody(ExtraOrder order, IProductsManagerService productsService)
    {
        var lines = new List<string>();
        foreach (var item in order.AllItems)
        {
            var product = productsService.GetProductById(item.ProductId);
            var quantity = product.Unity 
                ? item.Ammount.ToString("N0") 
                : item.Ammount.ToString("N2");
            lines.Add($"{product.Name} - {quantity}");
        }
        return string.Join("\n", lines);
    }
    
    public static string FormatNotificationTitle(Notification notification, Client client)
    {
        return client.Name;
    }
    
    public static string FormatNotificationBody(
        Notification notification, 
        Client client)
    {
        var body = notification.Info;
        
        // Add extra value for DontPay type
        if (notification.NotificationType == NotificationTypeEnum.DontPay)
        {
            body += $"\n\nValue(Nos extras) : {client.ExtraValueToPay:C}";
        }
        
        return body;
    }
}
```

---

## 🧪 Unit Test Strategy

### Test Suite 1: Deduplication Logic

```csharp
[TestFixture]
public class NotificationDeduplicationTests
{
    private IProximityNotificationService _service;
    private Order _testOrder;
    
    [SetUp]
    public void Setup()
    {
        _service = new ProximityNotificationService();
        _testOrder = CreateTestOrder();
    }
    
    [Test]
    public async Task SendNotification_FirstTime_Sends()
    {
        // Arrange
        var order = CreateTestOrder();
        var location = new LocationData { Latitude = 40.7128, Longitude = -74.0060 };
        
        // Act
        var result = await _service.NotifyOrderProximityAsync(order, location);
        
        // Assert
        Assert.IsTrue(result);  // Notification sent
    }
    
    [Test]
    public async Task SendNotification_SecondTime_Skips()
    {
        // Arrange
        var order = CreateTestOrder();
        var location = new LocationData { Latitude = 40.7128, Longitude = -74.0060 };
        
        // Act
        await _service.NotifyOrderProximityAsync(order, location);  // First
        var result = await _service.NotifyOrderProximityAsync(order, location);  // Second
        
        // Assert
        Assert.IsFalse(result);  // Notification skipped
    }
    
    [Test]
    public async Task SendNotification_NextDay_Sends()
    {
        // Arrange
        var order = CreateTestOrder();
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);
        
        // Act
        // Simulate sending today
        await _service.NotifyOrderProximityAsync(order, new LocationData());
        
        // Clear expired (simulate next day)
        await _service.ClearExpiredNotificationsAsync(retentionDays: 0);
        
        // Try again (with mocked date)
        var result = await _service.NotifyOrderProximityAsync(order, new LocationData());
        
        // Assert
        Assert.IsTrue(result);  // New day, should send
    }
}
```

### Test Suite 2: State Persistence

```csharp
[TestFixture]
public class NotificationStatePersistenceTests
{
    [Test]
    public async Task NotificationState_PersistsAcrossRestart()
    {
        // Arrange
        var store = new HybridNotificationStateStore();
        var order = CreateTestOrder();
        
        // Act - Part 1: Send notification
        await store.MarkNotifiedAsync(order.Id, NotificationItemType.Order);
        var beforeRestart = await store.IsAlreadyNotifiedAsync(order.Id, NotificationItemType.Order);
        
        // Simulate restart (create new store instance)
        store = new HybridNotificationStateStore();
        await store.InitializeAsync();
        var afterRestart = await store.IsAlreadyNotifiedAsync(order.Id, NotificationItemType.Order);
        
        // Assert
        Assert.IsTrue(beforeRestart);
        Assert.IsTrue(afterRestart);  // ✅ Persisted!
    }
}
```

---

## ✅ Acceptance Criteria

**TASK-3.4 is complete when:**
- [x] Current state management issues documented
- [x] Storage strategy comparison (3 options analyzed)
- [x] Hybrid approach recommended & justified
- [x] IProximityNotificationService interface designed
- [x] Deduplication algorithm specified
- [x] Notification content strategy defined
- [x] Unit test strategy provided
- [x] Example implementations included
- [x] Ready for TASK-3.5 (POC implementation)

---

**Status:** 🚀 IN PROGRESS  
**Next:** Finalize implementation examples  
**Then:** TASK-3.5 (POC & Validation - PHASE 2)

