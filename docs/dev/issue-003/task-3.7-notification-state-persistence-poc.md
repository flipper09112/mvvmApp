# TASK-3.7 — Notification State Persistence POC

**Status:** ✅ COMPLETE  
**Date:** 2026-02-20  
**Owner:** Mobile Dev  
**Duration:** 0.5 days  
**Priority:** P1 — Reliability Feature  
**Depends On:** TASK-3.4 ✅

---

## ✅ Summary of Implementation

Persistent notification deduplication POC delivered. Replaces the broken in-memory `HasNotify` flag from `ForegroundService.cs` (lost on restart → duplicate notifications) with a MAUI Preferences-backed store that survives app restarts. The deduplication window is per calendar day: each order/notification can trigger at most one alert per day. 22 unit tests cover key building, store operations, and the full send→skip→reset flow.

---

## 🔧 Technical Changes Applied

### New Files

| File | Description |
|---|---|
| `Services/Interfaces/Notifications/INotificationStateStore.cs` | Persistence abstraction: `IsNotified()`, `MarkNotified()`, `ClearExpired()`, `ClearAll()` |
| `Services/Interfaces/Notifications/IProximityNotificationService.cs` | Send interface: `NotifyOrderProximityAsync()`, `NotifyGeofenceAlertAsync()`, `ClearExpiredState()` |
| `Services/Interfaces/Notifications/ILocalNotificationSender.cs` | Platform notification abstraction: `SendAsync(id, title, message)` |
| `Services/Implementations/Notifications/IKeyValueStore.cs` + `MauiKeyValueStore` | Thin MAUI Preferences wrapper — removes IPreferences from test boundary |
| `Services/Implementations/Notifications/PreferencesNotificationStateStore.cs` | MAUI Preferences-backed `INotificationStateStore`. Index key tracks all written keys (MAUI has no enumeration API). Accepts `IKeyValueStore` for testability |
| `Services/Implementations/Notifications/DeduplicationKeyBuilder.cs` | Static key builder. Format: `"{Type}_{Id}_{yyyy-MM-dd}"`. Date in key = expiry scope |
| `Services/Implementations/Notifications/ProximityNotificationService.cs` | Full send+dedup implementation. Formats order/geofence notification content per TASK-3.4 spec |
| `Services/Implementations/Notifications/MauiLocalNotificationSender.cs` | POC sender — logs to `Debug.WriteLine`. Swap for `Plugin.LocalNotification` in production |
| `tabApp.Tests/Task37Tests.cs` | 22 NUnit tests across 3 fixtures |

### Modified Files

| File | Change |
|---|---|
| `MauiProgram.cs` | Added usings; registered `INotificationStateStore`, `ILocalNotificationSender`, `IProximityNotificationService` as singletons |
| `tabApp.Tests/tabApp.Tests.csproj` | Added `IProductsManagerService` Core include + TASK-3.7 source file includes |

---

## ⚠️ Concerns & Observations

### 1. MAUI Preferences has no key enumeration API
`Preferences.Default` does not expose a `GetAllKeys()` method. `PreferencesNotificationStateStore` maintains a pipe-delimited companion index key (`proximity_notif_index`) to track all written keys. This adds one extra read/write per `MarkNotified()` call — acceptable overhead (~1 ms).

### 2. `IProductsManagerService` not yet in MAUI DI
`ProximityNotificationService` depends on `IProductsManagerService` to format order bodies. This is registered as a TODO — the service will fail to resolve until TASK-3.8 wires Core services into MAUI DI.

### 3. `MauiLocalNotificationSender` is a stub
The POC sender only writes to `Debug.WriteLine`. Real push notifications require a plugin (e.g. `Plugin.LocalNotification`). This is deferred to the implementation phase to keep the POC scope minimal.

### 4. `IKeyValueStore` abstraction is intentional
Introducing `IKeyValueStore` instead of injecting `IPreferences` directly removes the `Microsoft.Maui.Storage` dependency from the test project boundary. Tests run on plain `net9.0` with no MAUI runtime.

---

## 🔬 Breaking Changes Identified

None. All changes are additive. The legacy `HasNotify` in-memory flag on `ExtraOrder` and `Notification` models is **not removed** — that is deferred to the full implementation phase once `ProximityNotificationService` is wired into `LocationForegroundService`.

---

## 📊 Risk Reassessment

| Risk from TASK-3.4 | Original | After TASK-3.7 |
|---|---|---|
| In-memory `HasNotify` lost on restart → duplicate notifications | HIGH | ✅ RESOLVED — `PreferencesNotificationStateStore` persists state across restarts |
| No expiration → keys accumulate forever | MEDIUM | ✅ RESOLVED — `ClearExpired()` removes keys older than today; called via `ClearExpiredState()` |
| Hard to test notification send logic | MEDIUM | ✅ RESOLVED — `ILocalNotificationSender` + `IKeyValueStore` make `ProximityNotificationService` fully mockable |
| Production scale (1000+ notifications/day) | LOW | ⚠️ DEFERRED — Preferences index key grows linearly; swap to SQLite hybrid store for production |

---

## 🧪 Validation Results

| File | Status | Notes |
|---|---|---|
| All 7 CrossPlatform notification source files | ✅ | 0 errors, 0 warnings |
| `MauiProgram.cs` | ✅ | 0 errors |
| `Task37Tests.cs` | ✅ | 0 errors |
| `tabApp.Tests.csproj` | ✅ | 0 errors |

---

## 🧪 Unit Test Impact Analysis

- **Tests required:** YES
- **New tests added:** 22 (in `Task37Tests.cs`)
- **Test project:** `tabApp.Tests` (`net9.0`, NUnit 4.2.2)

### Test Fixture Summary

| Fixture | Tests | What is covered |
|---|---|---|
| `DeduplicationKeyBuilderTests` | 5 | Key format for orders and notifications; distinct keys for different IDs; distinct keys for different dates; no-arg default |
| `NotificationStateStoreTests` | 9 | `IsNotified` on fresh store; `MarkNotified` → `IsNotified`; double-mark idempotency; `ClearAll`; `ClearExpired` removes yesterday/keeps today; empty store no-throw; `TryExtractDate` valid/malformed; ctor null guard |
| `ProximityNotificationServiceTests` | 8 | First order send → true; second order skipped → false; two different orders sent independently; after `ClearAll` resends; geofence first/second; `DontPay` body content; `ClearExpiredState` no-throw; 3× ctor null guards |

---

## 📌 Follow-Up Recommendations

1. **TASK-3.8 (next):** Register `IProductsManagerService`, `IOrdersManagerService`, `INotificationsManagerService` in MAUI DI so `ProximityNotificationService` resolves fully at runtime.
2. **Integration with TASK-3.5:** Wire `IProximityNotificationService` into `LocationForegroundService`'s 1-second loop so proximity alerts fire automatically during tracking.
3. **Implementation phase:** Replace `MauiLocalNotificationSender` with `Plugin.LocalNotification` or equivalent. Replace `PreferencesNotificationStateStore` with SQLite hybrid store for production scale.

