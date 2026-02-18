# 🧪 POC Templates - Validation Checklist

---

## POC #1: Background Location Tracking

### Objective
Validate that .NET MAUI can track GPS location continuously in background for delivery tracking.

### Critical Requirements
- ✅ Track location every 30-60 seconds
- ✅ Continue tracking when app in background
- ✅ Continue tracking when screen locked
- ✅ Battery drain < 10%/hour
- ✅ Location accuracy within 10 meters

### Implementation Template

```csharp
// File: Services/LocationTrackingService.cs
using Microsoft.Maui.Devices.Sensors;

public interface ILocationTrackingService
{
    Task StartTrackingAsync();
    Task StopTrackingAsync();
    bool IsTracking { get; }
    event EventHandler<Location> LocationUpdated;
}

public class LocationTrackingService : ILocationTrackingService
{
    private CancellationTokenSource _cts;
    private Timer _timer;
    
    public bool IsTracking { get; private set; }
    public event EventHandler<Location> LocationUpdated;

    public async Task StartTrackingAsync()
    {
        if (IsTracking) return;

        // Request permissions
        var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        if (status != PermissionStatus.Granted)
            throw new PermissionException("Location permission denied");

        _cts = new CancellationTokenSource();
        IsTracking = true;

        // Option 1: Timer-based (simple but limited)
        _timer = new Timer(async _ => await GetLocationAsync(), null, 0, 30000);

        // Option 2: Continuous tracking (better but more complex)
        // GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(30));
        // await Geolocation.StartListeningForegroundAsync(request);
    }

    private async Task GetLocationAsync()
    {
        try
        {
            var location = await Geolocation.GetLocationAsync(new GeolocationRequest
            {
                DesiredAccuracy = GeolocationAccuracy.Best,
                Timeout = TimeSpan.FromSeconds(10)
            });

            if (location != null)
            {
                LocationUpdated?.Invoke(this, location);
                
                // Log for testing
                Debug.WriteLine($"Location: {location.Latitude}, {location.Longitude}");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error getting location: {ex.Message}");
        }
    }

    public Task StopTrackingAsync()
    {
        _cts?.Cancel();
        _timer?.Dispose();
        IsTracking = false;
        return Task.CompletedTask;
    }
}
```

### Android-Specific Implementation (if needed)

```csharp
// File: Platforms/Android/Services/ForegroundLocationService.cs
#if ANDROID
using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;

[Service]
public class ForegroundLocationService : Service
{
    private const int NOTIFICATION_ID = 1001;
    private ILocationTrackingService _locationService;

    public override IBinder OnBind(Intent intent) => null;

    public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
    {
        // Create notification channel
        CreateNotificationChannel();

        // Start foreground service
        var notification = new NotificationCompat.Builder(this, "location_channel")
            .SetContentTitle("Tracking Deliveries")
            .SetContentText("GPS tracking active")
            .SetSmallIcon(Resource.Drawable.ic_location)
            .SetOngoing(true)
            .Build();

        StartForeground(NOTIFICATION_ID, notification);

        // Start location tracking
        _locationService = IPlatformApplication.Current.Services.GetService<ILocationTrackingService>();
        _locationService?.StartTrackingAsync();

        return StartCommandResult.Sticky;
    }

    private void CreateNotificationChannel()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var channel = new NotificationChannel("location_channel", 
                "Location Tracking", 
                NotificationImportance.Low);
            
            var notificationManager = GetSystemService(NotificationService) as NotificationManager;
            notificationManager?.CreateNotificationChannel(channel);
        }
    }
}
#endif
```

### Alternative: Shiny Plugin (Recommended if native doesn't work)

```csharp
// Install: dotnet add package Shiny.Locations
using Shiny.Locations;

public class ShinyLocationDelegate : IGpsDelegate
{
    public async Task OnReading(GpsReading reading)
    {
        // Handle location update
        Debug.WriteLine($"Location: {reading.Position.Latitude}, {reading.Position.Longitude}");
    }
}

// Register in MauiProgram.cs
builder.Services.AddShiny();
builder.Services.AddGps<ShinyLocationDelegate>();
```

### Testing Checklist

- [ ] **Test 1:** Start tracking, minimize app → Verify location updates continue
- [ ] **Test 2:** Lock screen → Verify location updates continue
- [ ] **Test 3:** Kill app (swipe away) → Verify service restarts (if required)
- [ ] **Test 4:** Monitor battery for 1 hour → Verify drain < 10%
- [ ] **Test 5:** Test on Android 10, 11, 12, 13, 14
- [ ] **Test 6:** Test location accuracy (compare with Google Maps)
- [ ] **Test 7:** Test in moving vehicle
- [ ] **Test 8:** Test permission revocation handling

### Success Criteria

| Criteria | Target | Result | Pass/Fail |
|----------|--------|--------|-----------|
| Location updates in background | Every 30-60s | ___ | ⬜ |
| Battery drain per hour | < 10% | ___% | ⬜ |
| Location accuracy | < 10m | ___m | ⬜ |
| Works when screen locked | Yes | ___ | ⬜ |
| Survives app kill | Yes/Acceptable | ___ | ⬜ |

### Decision

- [ ] ✅ **PASS** → Continue with MAUI native implementation
- [ ] ⚠️ **PASS with Shiny** → Use Shiny.Locations plugin
- [ ] 🔴 **FAIL** → Escalate risk, consider Android-specific implementation

---

## POC #2: Bluetooth Communication

### Objective
Validate Plugin.BLE for device-to-device Bluetooth sync.

### Critical Requirements
- ✅ Discover nearby Bluetooth devices
- ✅ Connect to specific device by MAC address
- ✅ Send/receive data reliably
- ✅ Handle connection failures gracefully
- ✅ Reconnect automatically

### Implementation Template

```csharp
// Install: dotnet add package Plugin.BLE
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;

public interface IBluetoothSyncService
{
    Task<IEnumerable<IDevice>> ScanDevicesAsync();
    Task<bool> ConnectAsync(IDevice device);
    Task<bool> SendDataAsync(byte[] data);
    Task<byte[]> ReceiveDataAsync();
    Task DisconnectAsync();
}

public class BluetoothSyncService : IBluetoothSyncService
{
    private readonly IBluetoothLE _ble;
    private readonly IAdapter _adapter;
    private IDevice _connectedDevice;
    private ICharacteristic _txCharacteristic;
    private ICharacteristic _rxCharacteristic;

    // UUIDs from original implementation
    private static readonly Guid SERVICE_UUID = Guid.Parse("your-service-uuid");
    private static readonly Guid TX_UUID = Guid.Parse("your-tx-uuid");
    private static readonly Guid RX_UUID = Guid.Parse("your-rx-uuid");

    public BluetoothSyncService()
    {
        _ble = CrossBluetoothLE.Current;
        _adapter = CrossBluetoothLE.Current.Adapter;
    }

    public async Task<IEnumerable<IDevice>> ScanDevicesAsync()
    {
        var devices = new List<IDevice>();
        
        _adapter.DeviceDiscovered += (s, e) =>
        {
            devices.Add(e.Device);
            Debug.WriteLine($"Found device: {e.Device.Name}");
        };

        await _adapter.StartScanningForDevicesAsync();
        await Task.Delay(5000); // Scan for 5 seconds
        await _adapter.StopScanningForDevicesAsync();

        return devices;
    }

    public async Task<bool> ConnectAsync(IDevice device)
    {
        try
        {
            await _adapter.ConnectToDeviceAsync(device);
            _connectedDevice = device;

            // Get service and characteristics
            var service = await device.GetServiceAsync(SERVICE_UUID);
            _txCharacteristic = await service.GetCharacteristicAsync(TX_UUID);
            _rxCharacteristic = await service.GetCharacteristicAsync(RX_UUID);

            // Subscribe to notifications
            _rxCharacteristic.ValueUpdated += OnDataReceived;
            await _rxCharacteristic.StartUpdatesAsync();

            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Connection failed: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> SendDataAsync(byte[] data)
    {
        try
        {
            if (_txCharacteristic == null)
                throw new InvalidOperationException("Not connected");

            await _txCharacteristic.WriteAsync(data);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Send failed: {ex.Message}");
            return false;
        }
    }

    private void OnDataReceived(object sender, CharacteristicUpdatedEventArgs e)
    {
        var data = e.Characteristic.Value;
        Debug.WriteLine($"Received {data.Length} bytes");
        // Handle received data
    }

    public async Task DisconnectAsync()
    {
        if (_connectedDevice != null)
        {
            await _adapter.DisconnectDeviceAsync(_connectedDevice);
            _connectedDevice = null;
        }
    }
}
```

### Testing Checklist

- [ ] **Test 1:** Scan for devices → Verify finds test device
- [ ] **Test 2:** Connect to device → Verify connection established
- [ ] **Test 3:** Send 100 bytes → Verify received correctly
- [ ] **Test 4:** Send 10KB data → Verify all chunks received
- [ ] **Test 5:** Disconnect and reconnect → Verify auto-reconnect works
- [ ] **Test 6:** Move devices apart → Verify connection loss handling
- [ ] **Test 7:** Send while disconnecting → Verify error handling
- [ ] **Test 8:** Test with actual printer device

### Success Criteria

| Criteria | Target | Result | Pass/Fail |
|----------|--------|--------|-----------|
| Device discovery time | < 10s | ___s | ⬜ |
| Connection time | < 5s | ___s | ⬜ |
| Data transfer success rate | > 95% | ___% | ⬜ |
| Reconnection time | < 10s | ___s | ⬜ |
| Works with printer | Yes | ___ | ⬜ |

### Decision

- [ ] ✅ **PASS** → Continue with Plugin.BLE
- [ ] ⚠️ **PARTIAL** → Works but needs fallback for some scenarios
- [ ] 🔴 **FAIL** → Consider InTheHand.BluetoothLE or WebAPI alternative

---

## POC #3: Maps Integration

### Objective
Validate Microsoft.Maui.Controls.Maps for route and client visualization.

### Critical Requirements
- ✅ Display map with user location
- ✅ Add custom markers for clients
- ✅ Draw route/polyline between points
- ✅ Handle marker taps
- ✅ Performance with 50+ markers

### Implementation Template

```csharp
// Install: dotnet add package Microsoft.Maui.Controls.Maps
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

public partial class MapTestPage : ContentPage
{
    private Map _map;

    public MapTestPage()
    {
        InitializeComponent();
        InitializeMap();
    }

    private void InitializeMap()
    {
        _map = new Map
        {
            IsShowingUser = true,
            MapType = MapType.Street
        };

        // Center on Lisbon (example)
        _map.MoveToRegion(MapSpan.FromCenterAndRadius(
            new Location(38.7223, -9.1393), 
            Distance.FromKilometers(5)));

        Content = _map;
    }

    public void AddClientMarkers(List<ClientLocation> clients)
    {
        foreach (var client in clients)
        {
            var pin = new Pin
            {
                Label = client.Name,
                Address = client.Address,
                Type = PinType.Place,
                Location = new Location(client.Latitude, client.Longitude)
            };

            pin.MarkerClicked += OnMarkerClicked;
            _map.Pins.Add(pin);
        }
    }

    public void DrawRoute(List<Location> points)
    {
        var polyline = new Polyline
        {
            StrokeColor = Colors.Blue,
            StrokeWidth = 5
        };

        foreach (var point in points)
        {
            polyline.Geopath.Add(point);
        }

        _map.MapElements.Add(polyline);
    }

    private void OnMarkerClicked(object sender, PinClickedEventArgs e)
    {
        var pin = sender as Pin;
        DisplayAlert("Cliente", pin.Label, "OK");
        e.HideInfoWindow = false;
    }
}
```

### Custom Pin Template (if needed)

```xml
<!-- XAML -->
<maps:Map x:Name="map">
    <maps:Map.ItemTemplate>
        <DataTemplate>
            <maps:Pin 
                Location="{Binding Location}"
                Label="{Binding Name}"
                Address="{Binding Address}">
                <maps:Pin.IconImageSource>
                    <FontImageSource 
                        Glyph="📍" 
                        FontFamily="Arial"
                        Color="Red"
                        Size="30"/>
                </maps:Pin.IconImageSource>
            </maps:Pin>
        </DataTemplate>
    </maps:Map.ItemTemplate>
</maps:Map>
```

### Testing Checklist

- [ ] **Test 1:** Display map → Verify renders correctly
- [ ] **Test 2:** Show user location → Verify blue dot appears
- [ ] **Test 3:** Add 1 marker → Verify displays with custom icon
- [ ] **Test 4:** Add 50 markers → Verify performance is acceptable
- [ ] **Test 5:** Draw polyline → Verify route displays
- [ ] **Test 6:** Tap marker → Verify event fires
- [ ] **Test 7:** Zoom in/out → Verify smooth performance
- [ ] **Test 8:** Switch map types → Verify Street/Satellite work

### Success Criteria

| Criteria | Target | Result | Pass/Fail |
|----------|--------|--------|-----------|
| Map load time | < 2s | ___s | ⬜ |
| 50 markers render time | < 500ms | ___ms | ⬜ |
| Pan/zoom smoothness | 60 FPS | ___ FPS | ⬜ |
| Custom pins working | Yes | ___ | ⬜ |
| Polyline drawing | Yes | ___ | ⬜ |

### Decision

- [ ] ✅ **PASS** → Use Microsoft.Maui.Controls.Maps
- [ ] ⚠️ **PARTIAL** → Works but consider Google Maps SDK for advanced features
- [ ] 🔴 **FAIL** → Must use platform-specific Google Maps implementation

---

## 📊 Overall POC Decision Matrix

After completing all 3 POCs, fill this matrix:

| POC | Status | Risk | Go/No-Go | Notes |
|-----|--------|------|----------|-------|
| Background Location | ⬜ Pass / ⬜ Fail | ⬜ Low / ⬜ High | ⬜ Go / ⬜ No-Go | ___ |
| Bluetooth | ⬜ Pass / ⬜ Fail | ⬜ Low / ⬜ High | ⬜ Go / ⬜ No-Go | ___ |
| Maps | ⬜ Pass / ⬜ Fail | ⬜ Low / ⬜ High | ⬜ Go / ⬜ No-Go | ___ |

### Final Decision

**Overall Migration Recommendation:**

- [ ] ✅ **GO** - All POCs passed, proceed with full migration
- [ ] ⚠️ **GO WITH CAUTION** - Some POCs have issues, but workarounds exist
- [ ] 🛑 **NO-GO** - Critical POCs failed, migration not viable with MAUI

**Signed off by:** _________________  
**Date:** _________________

---

## 🚀 Next Steps After POC

If **GO**:
1. Commit POC code to repository
2. Document findings and decisions
3. Update project plan with POC results
4. Proceed to Issue #6 (MAUI Base Setup)

If **GO WITH CAUTION**:
1. Document limitations and workarounds
2. Adjust timeline for additional complexity
3. Plan fallback implementations
4. Get stakeholder approval for adjusted plan

If **NO-GO**:
1. Document why migration is not viable
2. Explore alternative approaches
3. Consider maintaining Xamarin.Android
4. Evaluate other cross-platform frameworks

---

**POC Execution Time:** 3 days  
**Next Review:** After POC completion  
**Decision Deadline:** [Set date]

