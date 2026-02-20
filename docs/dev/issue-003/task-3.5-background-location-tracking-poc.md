# TASK-3.5 — Background Location Tracking POC

**Status:** ✅ COMPLETE (corrected 2026-02-20)  
**Date:** 2026-02-20  
**Owner:** Mobile Dev / Platform Specialist  
**Duration:** 1.5 days  
**Priority:** P1 — Validates feasibility  
**Depends On:** TASK-3.2 ✅

---

## ✅ Summary of Implementation

Working MAUI background location tracking POC delivered on both Android and iOS. Initial implementation used WorkManager (Android) and CLLocationManager significant-change (iOS). A gap was identified: **WorkManager cannot meet the 1-second update interval** required by the legacy `ForegroundService.cs`. The Android implementation was corrected to use a proper **Android Foreground Service** (`LocationForegroundService.cs`) delivering continuous 1-second GPS polling — matching the original behaviour exactly.

---

## 🔧 Technical Changes Applied

### New Files

| File | Description |
|---|---|
| `Services/Interfaces/Location/IBackgroundLocationTracker.cs` | Cross-platform interface: `UpdateInterval`, `StartAsync()`, `StopAsync()` |
| `Services/Location/BackgroundLocationStatus.cs` | Thread-safe in-memory status store (lat/lon/timestamp/error) |
| `Platforms/Android/LocationForegroundService.cs` | **Android Foreground Service** — `ForegroundServiceType.TypeLocation`, 1 000 ms async poll loop, `StartForeground()` with persistent notification, proper `OnDestroy()` cleanup |
| `Platforms/Android/BackgroundLocationTracker.cs` | Android `IBackgroundLocationTracker` — delegates to `LocationForegroundService` via `StartForegroundService()` / `StopService()` |
| `Platforms/Android/LocationUpdateWorker.cs` | WorkManager Worker (retained for reference / future low-frequency sync tasks) |
| `Platforms/iOS/BackgroundLocationTracker.cs` | iOS `IBackgroundLocationTracker` — CLLocationManager with `AllowsBackgroundLocationUpdates = true`, `DistanceFilter = 50m` |
| `MainPage.xaml` / `MainPage.xaml.cs` | POC UI — permission request, start/stop tracking, live status display |

### Modified Files

| File | Change |
|---|---|
| `Platforms/Android/AndroidManifest.xml` | Added `FOREGROUND_SERVICE`, `FOREGROUND_SERVICE_LOCATION` permissions + `<service>` declaration with `foregroundServiceType="location"` |
| `Platforms/Android/BackgroundLocationTracker.cs` | Updated to use `LocationForegroundService` — `UpdateInterval = TimeSpan.FromSeconds(1)` |
| `tabApp.CrossPlatform.csproj` | Added `Xamarin.AndroidX.Work.Runtime 2.10.0` (Android TFM only — for LocationUpdateWorker) |
| `MauiProgram.cs` | `IBackgroundLocationTracker` registered as singleton |
| `Platforms/iOS/Info.plist` | `NSLocationAlwaysAndWhenInUseUsageDescription` + `UIBackgroundModes → location` |

---

## ⚠️ Gap Identified & Corrected

### Problem: WorkManager cannot meet 1-second requirement

**Original POC approach (incorrect for this use case):**
```csharp
// BackgroundLocationTracker.cs — WRONG for 1-second requirement
public TimeSpan UpdateInterval => TimeSpan.FromMinutes(15); // ❌ OS hard minimum

new PeriodicWorkRequest.Builder(typeof(LocationUpdateWorker), TimeSpan.FromMinutes(15))
```

**Root cause:** Android OS enforces a **hard 15-minute minimum** for all `PeriodicWorkRequest` intervals, regardless of what value is specified. Any value smaller than 15 minutes is silently rounded up by the OS.

**Legacy requirement (ForegroundService.cs line ~60):**
```csharp
private const int UPDATE_INTERVAL = 1000; // 1 second
```

**Corrected approach:**
```csharp
// BackgroundLocationTracker.cs — CORRECT
public TimeSpan UpdateInterval => TimeSpan.FromSeconds(1); // ✅ 1-second continuous

context.StartForegroundService(new Intent(context, typeof(LocationForegroundService)));
```

```csharp
// LocationForegroundService.cs — key loop
private const int UpdateIntervalMs = 1_000; // matches legacy UPDATE_INTERVAL

while (!cancellationToken.IsCancellationRequested)
{
    var location = await Geolocation.Default.GetLocationAsync(request, cancellationToken);
    BackgroundLocationStatus.Update(location.Latitude, location.Longitude, DateTime.UtcNow);
    await Task.Delay(UpdateIntervalMs, cancellationToken);
}
```

---

## 🔬 Breaking Changes Identified

None. All changes are additive within the POC. `LocationUpdateWorker.cs` is retained (not deleted) for potential future use in low-frequency sync tasks.

---

## ⚠️ Concerns & Observations

### 1. Android Foreground Service requires persistent notification
Android 8.0+ mandates that any Foreground Service displays an ongoing notification the user cannot dismiss. `LocationForegroundService` creates a `NotificationImportance.Low` channel (no sound, no popup) to minimise intrusion. This is identical to the behaviour of the legacy `ForegroundService.cs`.

### 2. iOS cannot guarantee 1-second cadence in background
iOS CLLocationManager with `StartUpdatingLocation()` can deliver ~1-second updates **while the app is foregrounded**, but when backgrounded the OS throttles GPS callbacks. The `DistanceFilter = 50m` setting ensures updates fire on meaningful movement rather than on a fixed timer. This is an OS restriction that cannot be overridden — `UpdateInterval = TimeSpan.FromMinutes(5)` (approximate) on iOS is correct.

### 3. Android 14+ requires `FOREGROUND_SERVICE_LOCATION` permission
Added to `AndroidManifest.xml`. Without this permission, `StartForegroundService()` throws a `SecurityException` on API 34+.

### 4. `OnDestroy()` memory-leak fix
The legacy `ForegroundService.cs` was missing `OnDestroy()` — location updates were never unregistered. `LocationForegroundService.OnDestroy()` calls `StopTracking()` → `_cts.Cancel()` → loop exits cleanly. This is a direct bug fix from TASK-3.1 finding #1.

---

## 📊 Risk Reassessment

| Risk | Original | After TASK-3.5 |
|---|---|---|
| No MAUI Foreground Service API | CRITICAL | ✅ RESOLVED — `LocationForegroundService.cs` is a native Android Service, started via `StartForegroundService()` |
| 1-second interval not achievable via WorkManager | HIGH | ✅ RESOLVED — Foreground Service loop meets 1-second requirement |
| Memory leak from missing OnDestroy | HIGH | ✅ RESOLVED — `OnDestroy()` cancels CancellationToken, cleans up |
| iOS background restrictions | CRITICAL | ⚠️ ACCEPTED — iOS cannot guarantee 1-second cadence when backgrounded (OS restriction) |

---

## 🧪 Validation Results

| Target | Status | Notes |
|---|---|---|
| `LocationForegroundService.cs` builds | ✅ | 0 errors, platform warnings only (expected) |
| `BackgroundLocationTracker.cs` (Android) builds | ✅ | 0 errors |
| `BackgroundLocationTracker.cs` (iOS) builds | ✅ | 0 errors |
| `AndroidManifest.xml` valid | ✅ | Permissions + service declaration correct |
| `UpdateInterval` = 1 second (Android) | ✅ | `TimeSpan.FromSeconds(1)` |

---

## 🧪 Unit Test Impact Analysis

- **Tests required:** NO — Foreground Service lifecycle is platform-specific and requires integration/device testing
- **New tests added:** 0
- **Manual verification:** Start/stop via POC UI `MainPage.xaml`, observe `BackgroundLocationStatus` snapshot updating at ~1-second intervals on a physical Android device

---

## 📌 Follow-Up Recommendations

1. **TASK-3.7 (next):** Integrate `LocationForegroundService` with `IProximityNotificationService` so proximity alerts fire inside the 1-second loop.
2. **TASK-3.8 (next):** Pass `IProximityService` and `INotificationsManagerService` into `LocationForegroundService` via `IServiceProvider` (cannot use constructor DI directly in Android Services — use `MauiApplication.Current.Services`).
3. **Implementation phase:** Consider exposing a `LocationChanged` event from `IBackgroundLocationTracker` so ViewModels can subscribe without polling `BackgroundLocationStatus`.

