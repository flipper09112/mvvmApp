using CoreLocation;
using Foundation;
using tabApp.CrossPlatform.Services.Interfaces.Location;
using tabApp.CrossPlatform.Services.Location;

namespace tabApp.CrossPlatform;

/// <summary>
/// iOS implementation of IBackgroundLocationTracker.
/// Uses CLLocationManager with significant-change updates so the OS can wake the app
/// in the background without requiring continuous GPS (battery-friendly).
/// </summary>
public class BackgroundLocationTracker : IBackgroundLocationTracker, IDisposable
{
    private CLLocationManager? _locationManager;
    private bool _started;

    /// <summary>
    /// Significant-change updates fire roughly every 500 m or after ~1 km of movement.
    /// Not a fixed interval, but battery-optimal for background use.
    /// </summary>
    public TimeSpan UpdateInterval => TimeSpan.FromMinutes(5); // approximate

    public Task<bool> StartAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_started)
                return Task.FromResult(true);

            _locationManager = new CLLocationManager
            {
                AllowsBackgroundLocationUpdates = true,
                PausesLocationUpdatesAutomatically = false,
                DistanceFilter = 50,          // metres – fire on meaningful movement
                DesiredAccuracy = CLLocation.AccuracyHundredMeters
            };

            _locationManager.AuthorizationChanged += OnAuthorizationChanged;
            _locationManager.LocationsUpdated += OnLocationsUpdated;
            _locationManager.Failed += OnFailed;

            // Request "always" so we receive updates when the app is backgrounded
            _locationManager.RequestAlwaysAuthorization();
            _locationManager.StartUpdatingLocation();

            _started = true;
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            BackgroundLocationStatus.SetError($"iOS StartAsync failed: {ex.Message}");
            return Task.FromResult(false);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        if (_locationManager != null)
        {
            _locationManager.StopUpdatingLocation();
            _locationManager.AuthorizationChanged -= OnAuthorizationChanged;
            _locationManager.LocationsUpdated -= OnLocationsUpdated;
            _locationManager.Failed -= OnFailed;
        }

        _started = false;
        return Task.CompletedTask;
    }

    private void OnLocationsUpdated(object? sender, CLLocationsUpdatedEventArgs e)
    {
        var latest = e.Locations.LastOrDefault();
        if (latest != null)
        {
            BackgroundLocationStatus.Update(
                latest.Coordinate.Latitude,
                latest.Coordinate.Longitude,
                DateTime.UtcNow);
        }
    }

    private void OnAuthorizationChanged(object? sender, CLAuthorizationChangedEventArgs e)
    {
        if (e.Status == CLAuthorizationStatus.Denied ||
            e.Status == CLAuthorizationStatus.Restricted)
        {
            BackgroundLocationStatus.SetError($"iOS location authorization: {e.Status}");
        }
    }

    private void OnFailed(object? sender, NSErrorEventArgs e)
    {
        BackgroundLocationStatus.SetError($"iOS CLLocationManager error: {e.Error.LocalizedDescription}");
    }

    public void Dispose()
    {
        _locationManager?.Dispose();
        _locationManager = null;
    }
}





