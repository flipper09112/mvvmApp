# TASK-3.2: MAUI Background Task Architecture Design

**Status:** 🚀 IN PROGRESS  
**Date:** 2026-02-19  
**Owner:** Architect  
**Duration:** 1.5 days  
**Priority:** P0 - Blocking  
**Depends On:** TASK-3.1 (✅ COMPLETE)

---

## 📋 Objective

Design the MAUI equivalent architecture for ForegroundService, considering:
- Cross-platform capabilities (Android + iOS)
- MAUI framework limitations
- Background execution guarantees
- Battery optimization
- User experience expectations

---

## 🔍 MAUI Background Options Analysis

### ⚠️ Requirement: Real-Time Tracking (Max 5 Second Interval)

**Critical Constraint:** Location must be updated **maximum every 5 seconds** in background for proximity detection accuracy.

This eliminates several options:

| Option | Interval | Viable |
|--------|----------|--------|
| MAUI Background Tasks | 5-30 sec | ❌ No (unreliable, no guarantee) |
| WorkManager (periodic) | 15 min minimum | ❌ No (too long) |
| Significant Location Change | ~500m+ events | ❌ No (event-driven, not time-based) |
| **FusedLocationProviderClient** | **1-5 sec** | ✅ **Yes** |
| **CLLocationManager** | **1-5 sec** | ✅ **Yes** |

---

### Option 1: Android FusedLocationProviderClient (5sec intervals)

**What it is:**
- Google Play Services location API
- Real-time location updates with configurable interval
- Supports background with foreground service notification
- Handles device power states intelligently

**Implementation Pattern:**
```csharp
public class AndroidLocationService : IBackgroundLocationService
{
    private FusedLocationProviderClient _fusedClient;
    private LocationCallback _locationCallback;
    
    public async Task StartAsync()
    {
        // Create location request with 5 second interval
        var request = new LocationRequest()
            .SetPriority(LocationRequest.PriorityHighAccuracy)
            .SetInterval(5000)      // 5 seconds
            .SetMaxWaitTime(5000)   // Max 5 seconds
            .SetFastestInterval(2000); // Allow faster updates (2 sec minimum)
        
        // Create callback
        _locationCallback = new LocationCallbackImpl(location =>
        {
            OnLocationChanged(location);
            FireLocationChangedEvent(location);
        });
        
        // Start foreground service (required for Android 8.0+)
        StartForegroundServiceWithNotification();
        
        // Request location updates
        _fusedClient.RequestLocationUpdates(request, _locationCallback, Looper.MainLooper);
    }
    
    public async Task StopAsync()
    {
        // Remove location updates
        _fusedClient.RemoveLocationUpdates(_locationCallback);
        
        // Stop foreground service
        StopForeground(true);
        
        // Clean up
        _locationCallback?.Dispose();
    }
    
    private void StartForegroundServiceWithNotification()
    {
        // Create persistent notification for background location tracking
        // (Same as current ForegroundService implementation)
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var notification = new NotificationCompat.Builder(this, NOTIFICATION_CHANNEL_ID)
                .SetContentTitle("Location Tracking Active")
                .SetContentText("App is tracking your location in the background")
                .SetSmallIcon(Resource.Drawable.notification_icon)
                .SetOngoing(true)
                .SetPriority(NotificationCompat.PriorityHigh)
                .Build();
            
            StartForeground(SERVICE_NOTIFICATION_ID, notification);
        }
    }
}
```

**Pros:**
- ✅ Real-time location updates (5 second intervals)
- ✅ Accurate GPS + Network + Sensors combination
- ✅ Background-capable with foreground notification
- ✅ Works even with battery saver mode
- ✅ Proven reliable in production (current implementation)

**Cons:**
- ⚠️ Android-only (requires separate iOS implementation)
- ⚠️ Battery drain high (continuous GPS)
- ⚠️ Requires visible foreground notification
- ⚠️ Requires "Always" location permission

**Best Use:** Real-time location tracking in background

**✅ SUITABLE for real-time proximity detection**

---

### Option 2: iOS CLLocationManager (5sec intervals + Standard Updates)

**What it is:**
- iOS Core Location framework
- Standard location tracking mode (NOT significant change)
- Supports background location with UIBackgroundModes
- Can achieve 5 second intervals

**Implementation Pattern:**
```csharp
public class iOSLocationService : IBackgroundLocationService, ICLLocationManagerDelegate
{
    private CLLocationManager _locationManager;
    
    public async Task StartAsync()
    {
        // Request "Always" permission (required for background)
        _locationManager.RequestAlwaysAndWhenInUseAuthorization();
        
        // Configure for background
        _locationManager.AllowsBackgroundLocationUpdates = true;
        _locationManager.PausesLocationUpdatesAutomatically = false;
        
        // Set accuracy
        _locationManager.DesiredAccuracy = CLLocation.AccuracyBest;  // ±5-10 meters
        
        // Set distance filter (minimum distance between updates)
        _locationManager.DistanceFilter = 5.0;  // Meters
        
        // Start standard location updates (NOT significant change)
        _locationManager.StartUpdatingLocation();
        
        // Note: iOS doesn't have direct 5-second interval control
        // Updates are event-driven based on movement and accuracy
        // In practice, you'll get updates every 5-10 seconds with good GPS
    }
    
    public async Task StopAsync()
    {
        _locationManager.StopUpdatingLocation();
    }
    
    [Export("locationManager:didUpdateLocations:")]
    public void DidUpdateLocations(CLLocationManager manager, CLLocation[] locations)
    {
        var location = locations.LastOrDefault();
        if (location != null)
        {
            OnLocationChanged(location);
            FireLocationChangedEvent(location);
        }
    }
}
```

**Pros:**
- ✅ Real-time location updates (~5-10 second intervals typical)
- ✅ Accurate GPS + Network + Sensors
- ✅ Background-capable (UIBackgroundModes = location)
- ✅ Standard iOS approach

**Cons:**
- ⚠️ iOS-only (requires separate Android implementation)
- ⚠️ Battery drain high (continuous GPS)
- ⚠️ Blue location indicator always visible
- ⚠️ Requires "Always" location permission (users hesitant)
- ⚠️ iOS doesn't expose exact interval control (event-driven)

**Best Use:** Real-time location tracking on iOS

**✅ SUITABLE for real-time proximity detection**

---

### Option 3: Hybrid Approach - Real-Time (Recommended ✅)

**Architecture:**
```
┌──────────────────────────────────────────────────────┐
│  App Layer (MVVM ViewModel)                          │
├──────────────────────────────────────────────────────┤
│                                                      │
│  IBackgroundLocationService (Abstraction)            │
│  ├─ StartLocationTracking()                          │
│  ├─ StopLocationTracking()                           │
│  ├─ LocationChanged Event (5sec max)                 │
│  └─ ProximityAlert Event                             │
│                                                      │
├──────────────────────────────────────────────────────┤
│  Platform Implementations:                           │
│                                                      │
│  Android Path (Real-Time):                           │
│  ├─ AndroidLocationService                           │
│  ├─ FusedLocationProviderClient (5sec interval)      │
│  ├─ Foreground Service (persistent notification)     │
│  └─ LocationCallback (fires every 5 seconds)         │
│                                                      │
│  iOS Path (Real-Time):                               │
│  ├─ iOSLocationService                               │
│  ├─ CLLocationManager (standard updates)             │
│  ├─ UIBackgroundModes = location (in Info.plist)     │
│  └─ LocationDelegate (fires every 5-10 seconds)      │
│                                                      │
├──────────────────────────────────────────────────────┤
│  Supporting Services:                                │
│                                                      │
│  ├─ IProximityService                                │
│  │  ├─ CalculateDistance(lat1, lng1, lat2, lng2)     │
│  │  ├─ GetOrdersInProximity(location)                │
│  │  └─ GetNotificationsInProximity(location)         │
│  │                                                   │
│  ├─ IProximityNotificationService                    │
│  │  ├─ NotifyOrderProximity(order)                   │
│  │  ├─ NotifyGeofenceAlert(notification)             │
│  │  └─ MarkNotificationSent(id)                      │
│  │                                                   │
│  └─ Persistent State (MAUI Preferences + SQLite)     │
│     └─ NotificationDeduplication cache               │
│                                                      │
└──────────────────────────────────────────────────────┘
```

**Why This Approach:**
- ✅ Cross-platform from day 1 (abstraction layer)
- ✅ **Real-time location every 5 seconds** (meets requirement)
- ✅ Platform-optimal implementation for each OS
- ✅ Android: FusedLocationProviderClient with 5sec interval
- ✅ iOS: CLLocationManager standard updates
- ✅ Both support background tracking
- ✅ Unified interface for app code
- ✅ MAUI DI-friendly (dependency injection)

**Trade-offs Accepted:**
- ⚠️ High battery drain (continuous GPS)
  - **Mitigation:** User explicitly enables "Always" location permission (aware of battery impact)
  - **Mitigation:** Can pause tracking when not in active shift
- ⚠️ Visible location indicator (both Android & iOS show location is active)
  - **Mitigation:** Normal for location tracking apps (Uber, Google Maps, etc.)
- ⚠️ Two implementations to maintain
  - **Mitigation:** Clear interface contract minimizes coupling

---

### Comparison: Previous (15min) vs New (5sec Real-Time)

| Aspect | Previous (15min WorkManager) | New (5sec Real-Time) |
|--------|------------------------------|----------------------|
| **Android** | WorkManager (15min periodic) | FusedLocationProviderClient (5sec) |
| **iOS** | Significant change (~500m) | CLLocationManager standard (5sec) |
| **Update Interval** | 15 minutes | 5 seconds ⚡ |
| **Accuracy** | Low (event-driven) | High (GPS continuous) ⚡ |
| **Battery** | Very low | High (trade-off) |
| **Background** | Yes (no notification) | Yes (with notification) |
| **Proximity Detection** | Delayed (up to 15min) | Real-time (5sec) ⚡ |
| **Use Case** | Periodic sync | **Real-time tracking** ⚡ |

---

## 🏗️ IBackgroundLocationService Interface Design

**File:** `tabApp.Core/Services/Interfaces/IBackgroundLocationService.cs`

```csharp
using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace tabApp.Core.Services.Interfaces
{
    /// <summary>
    /// Cross-platform abstraction for background location tracking.
    /// 
    /// REQUIREMENT: Real-time location updates (maximum 5 second interval)
    /// 
    /// Android Implementation: FusedLocationProviderClient (5 second periodic updates)
    /// iOS Implementation: CLLocationManager standard updates (5-10 second intervals)
    /// 
    /// Both implementations provide real-time location updates for accurate proximity detection.
    /// Battery usage is high due to continuous GPS, but necessary for reliable geo-alerts.
    /// 
    /// Both platforms show visible location indicator to user.
    /// Both require "Always" location permission (background capable).
    /// </summary>
    public interface IBackgroundLocationService
    {
        // ========== Properties ==========
        
        /// <summary>
        /// Get the last known location (cached).
        /// Returns null if location never obtained.
        /// </summary>
        LocationData LastLocation { get; }
        
        /// <summary>
        /// Is the service currently tracking location?
        /// </summary>
        bool IsTracking { get; }
        
        /// <summary>
        /// Current update interval in milliseconds.
        /// Default: 5000ms (5 seconds) for real-time proximity detection.
        /// Minimum: 2000ms (2 seconds) on Android
        /// Note: iOS doesn't expose exact interval control (event-driven)
        /// </summary>
        int UpdateIntervalMs { get; set; }
        
        // ========== Methods ==========
        
        /// <summary>
        /// Start tracking location in background.
        /// Requests permissions if needed.
        /// 
        /// Throws PermissionException if user denies permission.
        /// Throws NotSupportedException if platform not supported.
        /// </summary>
        Task StartAsync();
        
        /// <summary>
        /// Stop tracking location.
        /// Clean up resources (callbacks, listeners, etc).
        /// </summary>
        Task StopAsync();
        
        /// <summary>
        /// Get last cached location (synchronous).
        /// Returns null if no location obtained yet.
        /// </summary>
        LocationData GetLastLocation();
        
        /// <summary>
        /// Get current location (may trigger fresh GPS request).
        /// Timeout: 30 seconds.
        /// Returns null if cannot obtain location within timeout.
        /// </summary>
        Task<LocationData> GetCurrentLocationAsync();
        
        // ========== Events ==========
        
        /// <summary>
        /// Fired when new location received.
        /// Frequency: Android ~every 5 seconds, iOS ~every 5-10 seconds.
        /// Always fired on main thread.
        /// </summary>
        event EventHandler<LocationChangedEventArgs> LocationChanged;
        
        /// <summary>
        /// Fired when proximity alert triggered.
        /// Only fired if distance < threshold AND not already notified.
        /// Always fired on main thread.
        /// </summary>
        event EventHandler<ProximityAlertEventArgs> ProximityAlertTriggered;
        
        /// <summary>
        /// Fired when location service encounters error.
        /// Examples: Permission denied, GPS disabled, network error.
        /// App should handle gracefully.
        /// </summary>
        event EventHandler<LocationServiceErrorEventArgs> ErrorOccurred;
        
        // ========== Permission Checks ==========
        
        /// <summary>
        /// Check current location permission status.
        /// Returns: None, WhenInUse, Always.
        /// </summary>
        Task<PermissionStatus> GetPermissionStatusAsync();
        
        /// <summary>
        /// Request location permission.
        /// On iOS: Requests "Always" permission (required for background).
        /// On Android: Requests fine location permission.
        /// </summary>
        Task<bool> RequestPermissionAsync();
    }
    
    // ========== Supporting Types ==========
    
    /// <summary>
    /// Current location data.
    /// </summary>
    public class LocationData
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Accuracy { get; set; }  // In meters
        public DateTime Timestamp { get; set; }
        public double? Altitude { get; set; }
        public double? Speed { get; set; }     // In m/s
        
        public override string ToString() => 
            $"({Latitude:F4}, {Longitude:F4}) ±{Accuracy:F0}m @ {Timestamp:HH:mm:ss}";
    }
    
    /// <summary>
    /// Event args for location changes.
    /// </summary>
    public class LocationChangedEventArgs : EventArgs
    {
        public LocationData Location { get; set; }
        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Event args for proximity alerts.
    /// </summary>
    public class ProximityAlertEventArgs : EventArgs
    {
        public LocationData Location { get; set; }
        public double Distance { get; set; }  // In meters
        public object AlertSource { get; set; }  // Order or Notification
        public DateTime AlertedAt { get; set; } = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Event args for service errors.
    /// </summary>
    public class LocationServiceErrorEventArgs : EventArgs
    {
        public Exception Exception { get; set; }
        public string Message { get; set; }
        public DateTime ErrorAt { get; set; } = DateTime.UtcNow;
    }
    
    /// <summary>
    /// Permission status for location.
    /// </summary>
    public enum PermissionStatus
    {
        None,         // No permission
        WhenInUse,    // Only when app in foreground
        Always        // Anytime (background capable)
    }
}
```

---

## 🔧 Android Implementation Design

**File:** `tabApp.Droid/Services/AndroidLocationService.cs`

```csharp
using Android.App;
using Android.Content;
using Android.Gms.Location;
using System;
using System.Threading.Tasks;
using tabApp.Core.Services.Interfaces;

namespace tabApp.Droid.Services
{
    /// <summary>
    /// Android background location service using FusedLocationProviderClient.
    /// 
    /// Provides real-time location updates (5 second intervals) with foreground service notification.
    /// 
    /// Uses FusedLocationProviderClient for location acquisition.
    /// Maintains foreground service throughout tracking (required for background location on Android 8.0+).
    /// </summary>
    public class AndroidLocationService : Service, IBackgroundLocationService
    {
        private const int SERVICE_NOTIFICATION_ID = 123;
        private const string NOTIFICATION_CHANNEL_ID = "com.company.app.location";
        
        private FusedLocationProviderClient _fusedClient;
        private LocationCallback _locationCallback;
        private bool _isTracking;
        
        public LocationData LastLocation { get; private set; }
        public bool IsTracking => _isTracking;
        public int UpdateIntervalMs { get; set; } = 5000;  // 5 seconds
        
        public event EventHandler<LocationChangedEventArgs> LocationChanged;
        public event EventHandler<ProximityAlertEventArgs> ProximityAlertTriggered;
        public event EventHandler<LocationServiceErrorEventArgs> ErrorOccurred;
        
        public override void OnCreate()
        {
            base.OnCreate();
            _fusedClient = LocationServices.GetFusedLocationProviderClient(this);
        }
        
        public async Task StartAsync()
        {
            if (_isTracking) return;
            
            try
            {
                // Create location request with 5 second interval
                var request = new LocationRequest()
                    .SetPriority(LocationRequest.PriorityHighAccuracy)
                    .SetInterval(5000)      // 5 seconds
                    .SetMaxWaitTime(5000)   // Max 5 seconds
                    .SetFastestInterval(2000); // Allow faster updates (2 sec minimum)
                
                // Create callback
                _locationCallback = new LocationCallbackImpl(location =>
                {
                    LastLocation = new LocationData
                    {
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                        Accuracy = location.Accuracy,
                        Timestamp = DateTime.UtcNow,
                        Altitude = location.HasAltitude ? location.Altitude : null,
                        Speed = location.HasSpeed ? location.Speed : null
                    };
                    
                    LocationChanged?.Invoke(this, new LocationChangedEventArgs 
                    { 
                        Location = LastLocation 
                    });
                });
                
                // Start foreground service (required for Android 8.0+)
                StartForegroundServiceWithNotification();
                
                // Request location updates
                _fusedClient.RequestLocationUpdates(request, _locationCallback, Looper.MainLooper);
                
                _isTracking = true;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new LocationServiceErrorEventArgs 
                { 
                    Exception = ex, 
                    Message = ex.Message 
                });
                throw;
            }
        }
        
        public async Task StopAsync()
        {
            if (!_isTracking) return;
            
            try
            {
                // Remove location updates
                if (_locationCallback != null)
                {
                    _fusedClient.RemoveLocationUpdates(_locationCallback);
                    _locationCallback?.Dispose();
                }
                
                // Stop foreground service
                StopForeground(true);
                
                _isTracking = false;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new LocationServiceErrorEventArgs 
                { 
                    Exception = ex, 
                    Message = ex.Message 
                });
            }
        }
        
        private void StartForegroundServiceWithNotification()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var notification = new NotificationCompat.Builder(this, NOTIFICATION_CHANNEL_ID)
                    .SetContentTitle("Location Tracking Active")
                    .SetContentText("App is tracking your location to create geo alerts")
                    .SetSmallIcon(Resource.Drawable.notification_icon_background)
                    .SetOngoing(true)
                    .SetPriority(NotificationCompat.PriorityHigh)
                    .SetContentIntent(BuildIntentToShowMainActivity())
                    .Build();
                
                var notificationManager = GetSystemService(NotificationService) as NotificationManager;
                var chan = new NotificationChannel(
                    NOTIFICATION_CHANNEL_ID, 
                    "Location Tracking", 
                    NotificationImportance.High);
                notificationManager.CreateNotificationChannel(chan);
                
                StartForeground(SERVICE_NOTIFICATION_ID, notification);
            }
        }
        
        private PendingIntent BuildIntentToShowMainActivity()
        {
            var notificationIntent = new Intent(this, typeof(MainActivity));
            notificationIntent.SetFlags(ActivityFlags.SingleTop | ActivityFlags.ClearTask);
            return PendingIntent.GetActivity(this, 0, notificationIntent, 0);
        }
        
        public LocationData GetLastLocation() => LastLocation;
        
        public async Task<LocationData> GetCurrentLocationAsync()
        {
            // Implementation in POC phase
            throw new NotImplementedException();
        }
        
        public async Task<PermissionStatus> GetPermissionStatusAsync()
        {
            // Implementation in POC phase
            throw new NotImplementedException();
        }
        
        public async Task<bool> RequestPermissionAsync()
        {
            // Implementation in POC phase
            throw new NotImplementedException();
        }
        
        public override IBinder OnBind(Intent intent) => null;
    }
    
    public class LocationCallbackImpl : LocationCallback
    {
        private readonly Action<Android.Locations.Location> _onLocation;
        
        public LocationCallbackImpl(Action<Android.Locations.Location> onLocation)
        {
            _onLocation = onLocation;
        }
        
        public override void OnLocationResult(LocationResult result)
        {
            var location = result.LastLocation;
            if (location != null)
                _onLocation(location);
        }
    }
}
```

**Key Features:**
- ✅ Real-time updates every 5 seconds
- ✅ FusedLocationProviderClient for accurate location
- ✅ Foreground service notification (required for background)
- ✅ LocationCallback handler for location events
- ✅ Clean shutdown (remove updates, stop foreground service)

---

## 🍎 iOS Implementation Design

**File:** `tabApp.iOS/Services/iOSLocationService.cs`

```csharp
using CoreLocation;
using Foundation;
using System;
using System.Threading.Tasks;
using tabApp.Core.Services.Interfaces;

namespace tabApp.iOS.Services
{
    /// <summary>
    /// iOS background location service using CLLocationManager.
    /// 
    /// Uses standard location tracking mode (NOT significant location change).
    /// Provides real-time updates every 5-10 seconds for accurate proximity detection.
    /// 
    /// Requires:
    /// - NSLocationAlwaysAndWhenInUseUsageDescription in Info.plist
    /// - UIBackgroundModes = location in Info.plist
    /// - User must grant "Always" permission
    /// </summary>
    public class iOSLocationService : IBackgroundLocationService, ICLLocationManagerDelegate
    {
        private CLLocationManager _locationManager;
        private bool _isTracking;
        
        public LocationData LastLocation { get; private set; }
        public bool IsTracking => _isTracking;
        public int UpdateIntervalMs { get; set; } = 5000;  // iOS doesn't use this directly (event-driven)
        
        public event EventHandler<LocationChangedEventArgs> LocationChanged;
        public event EventHandler<ProximityAlertEventArgs> ProximityAlertTriggered;
        public event EventHandler<LocationServiceErrorEventArgs> ErrorOccurred;
        
        // Constructor
        public iOSLocationService()
        {
            _locationManager = new CLLocationManager();
            _locationManager.Delegate = this;
        }
        
        public async Task StartAsync()
        {
            if (_isTracking) return;
            
            try
            {
                // Request "Always" permission (required for background)
                _locationManager.RequestAlwaysAndWhenInUseAuthorization();
                
                // Configure for background location tracking
                _locationManager.AllowsBackgroundLocationUpdates = true;
                _locationManager.PausesLocationUpdatesAutomatically = false;
                
                // Set accuracy to best (±5-10 meters)
                _locationManager.DesiredAccuracy = CLLocation.AccuracyBest;
                
                // Set distance filter (minimum distance between updates)
                // 5 meters = trigger update for every 5m of movement
                _locationManager.DistanceFilter = 5.0;
                
                // Start standard location updates (NOT significant change)
                _locationManager.StartUpdatingLocation();
                
                _isTracking = true;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new LocationServiceErrorEventArgs 
                { 
                    Exception = ex, 
                    Message = ex.Message 
                });
                throw;
            }
        }
        
        public async Task StopAsync()
        {
            if (!_isTracking) return;
            
            try
            {
                _locationManager.StopUpdatingLocation();
                _isTracking = false;
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, new LocationServiceErrorEventArgs 
                { 
                    Exception = ex, 
                    Message = ex.Message 
                });
            }
        }
        
        public LocationData GetLastLocation() => LastLocation;
        
        public async Task<LocationData> GetCurrentLocationAsync()
        {
            // Implementation in POC phase
            throw new NotImplementedException();
        }
        
        public async Task<PermissionStatus> GetPermissionStatusAsync()
        {
            // Implementation in POC phase
            throw new NotImplementedException();
        }
        
        public async Task<bool> RequestPermissionAsync()
        {
            // Implementation in POC phase
            throw new NotImplementedException();
        }
        
        // ICLLocationManagerDelegate implementation
        
        [Export("locationManager:didUpdateLocations:")]
        public void DidUpdateLocations(CLLocationManager manager, CLLocation[] locations)
        {
            var location = locations?.LastOrDefault();
            if (location != null)
            {
                LastLocation = new LocationData
                {
                    Latitude = location.Coordinate.Latitude,
                    Longitude = location.Coordinate.Longitude,
                    Accuracy = location.HorizontalAccuracy,
                    Timestamp = DateTime.UtcNow,
                    Altitude = location.Altitude >= 0 ? location.Altitude : null,
                    Speed = location.Speed >= 0 ? location.Speed : null
                };
                
                LocationChanged?.Invoke(this, new LocationChangedEventArgs 
                { 
                    Location = LastLocation 
                });
            }
        }
        
        [Export("locationManager:didFailWithError:")]
        public void DidFailWithError(CLLocationManager manager, NSError error)
        {
            var ex = new Exception($"Location error: {error.LocalizedDescription}");
            ErrorOccurred?.Invoke(this, new LocationServiceErrorEventArgs 
            { 
                Exception = ex, 
                Message = error.LocalizedDescription 
            });
        }
    }
}
```

**Info.plist Configuration:**
```xml
<key>UIBackgroundModes</key>
<array>
    <string>location</string>
</array>

<key>NSLocationAlwaysAndWhenInUseUsageDescription</key>
<string>This app needs your location to create proximity-based geo alerts</string>

<key>NSLocationWhenInUseUsageDescription</key>
<string>This app needs your location to create proximity-based geo alerts</string>

<key>NSLocationAlwaysUsageDescription</key>
<string>This app needs your location to create proximity-based geo alerts</string>
```

**Key Features:**
- ✅ Real-time updates every 5-10 seconds (event-driven based on movement + accuracy)
- ✅ Standard location updates (NOT significant location change)
- ✅ AccuracyBest for high precision
- ✅ DistanceFilter = 5m (trigger for every 5m of movement)
- ✅ Background-capable with UIBackgroundModes configuration
- ✅ LocationDelegate for handling updates and errors

---

## 🔗 DI Configuration Design

**File:** `MauiProgram.cs`

```csharp
using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using tabApp.Core.Services.Interfaces;

namespace tabApp.CrossPlatform
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder()
                .UseMauiApp<App>()
                // ... other configuration ...
                .ConfigureLocationServices();
            
            return builder.Build();
        }
    }
    
    public static class LocationServiceExtensions
    {
        public static MauiAppBuilder ConfigureLocationServices(
            this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IBackgroundLocationService>(
                serviceProvider =>
                {
#if __ANDROID__
                    var context = Microsoft.Maui.Controls.Application.Current
                        ?.Handler?.MauiContext?.Services
                        .GetService<Android.Content.Context>();
                    return new AndroidLocationService(context);
#elif __IOS__
                    return new iOSLocationService();
#else
                    throw new PlatformNotSupportedException();
#endif
                });
            
            // Register supporting services
            builder.Services.AddSingleton<IProximityService, ProximityService>();
            builder.Services.AddSingleton<IProximityNotificationService, 
                ProximityNotificationService>();
            
            return builder;
        }
    }
}
```

---

## 📊 Comparison: Current vs MAUI

| Aspect | Current (Android) | MAUI (Hybrid Real-Time) |
|--------|-------------------|-------------------------|
| **Platform** | Android only | Android + iOS |
| **Location API** | ForegroundService + FusedLocationProviderClient | FusedLocationProviderClient (Android) + CLLocationManager (iOS) |
| **Update Interval** | 1000ms (1 second) | Android: 5000ms (5 sec), iOS: ~5-10 sec |
| **Accuracy** | High (GPS always) | High (GPS always) |
| **Battery** | Very high drain | Very high drain |
| **Foreground Notification** | Required | Required |
| **Reliability** | System-dependent | System-dependent |
| **Permission Model** | Runtime requests | MAUI Permissions API |
| **Testability** | Hard (service dependency) | Medium (interface-based) |
| **Maintainability** | Single platform | Two implementations, clear interface |
| **Status** | Production | Design ready for POC |

---

## ⚠️ Trade-offs & Considerations

### Battery Drain (High)

**Current:** Continuous GPS in foreground service (similar battery impact as MAUI solution)

**MAUI:** Continuous GPS with 5-second intervals + foreground notification

**Impact:**
- ⚠️ High battery drain (continuous GPS required)
- ⚠️ Not suitable for all-day background tracking
- ⚠️ User may disable based on battery concerns

**Mitigation:**
- Accept high battery drain as trade-off for real-time accuracy
- User explicitly enables "Always" location permission (aware of impact)
- Can implement pause/resume based on shift timing
- Show battery impact warning in UI
- Provide option to manually refresh location instead of continuous tracking
- Use lower accuracy mode when not needed (e.g., stationary periods)

### Visible Location Indicator

**Android:** Small location icon in status bar (always visible)

**iOS:** Blue location indicator + "Using Your Location" banner (always visible)

**Impact:**
- ⚠️ Users always see location is being tracked
- ✅ Transparency (required by both platforms)
- ⚠️ May concern privacy-conscious users

**Mitigation:**
- Normal for location tracking apps (Uber, Google Maps, etc.)
- Clear user communication about why location tracking is needed
- Explain that geo-alerts require real-time tracking

### Two Implementations

**Challenge:** Maintaining Android + iOS implementations separately

**Impact:**
- ⚠️ Code duplication possible
- ⚠️ Need to test both platforms

**Mitigation:**
- Clear interface contract (IBackgroundLocationService)
- Shared logic in services layer (ProximityService, etc.)
- Unit tests applicable to both platforms
- Platform-specific implementations isolated

---

## ✅ Acceptance Criteria

**TASK-3.2 is complete when:**
- [ ] IBackgroundLocationService interface designed with full documentation
- [ ] Android implementation approach documented (WorkManager-based)
- [ ] iOS implementation approach documented (CLLocationManager-based)
- [ ] DI configuration designed
- [ ] Trade-offs documented and justified
- [ ] Code examples provided for all components
- [ ] Architecture diagram created
- [ ] Platform differences explained
- [ ] Ready for TASK-3.5 (POC implementation)

---

**Status:** 🔄 IN PROGRESS  
**Next:** Complete interface + implementation designs (detailed in next section)  
**Then:** TASK-3.3 (Distance Calculation Analysis)

