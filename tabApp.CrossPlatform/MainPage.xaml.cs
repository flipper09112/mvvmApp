using tabApp.CrossPlatform.Services.Interfaces.Location;
using tabApp.CrossPlatform.Services.Location;

namespace tabApp.CrossPlatform;

public partial class MainPage : ContentPage
{
    private readonly IBackgroundLocationTracker? _tracker;
    private bool _isTracking;

    public MainPage()
    {
        InitializeComponent();
        _tracker = IPlatformApplication.Current?.Services.GetService<IBackgroundLocationTracker>();
        UpdateStatusFromStore();

        Dispatcher.StartTimer(TimeSpan.FromSeconds(5), () =>
        {
            UpdateStatusFromStore();
            return true;
        });
    }

    private async void OnRequestPermissionsClicked(object? sender, EventArgs e)
    {
        var whenInUse = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
        if (whenInUse != PermissionStatus.Granted)
        {
            await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        }

        var always = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();
        if (always != PermissionStatus.Granted)
        {
            await Permissions.RequestAsync<Permissions.LocationAlways>();
        }

        StatusLabel.Text = "Status: permissions requested";
    }

    private async void OnStartTrackingClicked(object? sender, EventArgs e)
    {
        if (_tracker == null)
        {
            StatusLabel.Text = "Status: tracker not registered";
            return;
        }

        var started = await _tracker.StartAsync();
        _isTracking = started;
        StatusLabel.Text = started ? "Status: tracking started" : "Status: tracking not started";
    }

    private async void OnStopTrackingClicked(object? sender, EventArgs e)
    {
        if (_tracker == null)
        {
            StatusLabel.Text = "Status: tracker not registered";
            return;
        }

        await _tracker.StopAsync();
        _isTracking = false;
        StatusLabel.Text = "Status: tracking stopped";
    }

    private void UpdateStatusFromStore()
    {
        var snapshot = BackgroundLocationStatus.GetSnapshot();
        var lastUpdateText = snapshot.LastUpdateUtc?.ToLocalTime().ToString("u") ?? "(none)";
        var locationText = snapshot.LocationText ?? "(none)";
        var errorText = string.IsNullOrWhiteSpace(snapshot.ErrorMessage) ? "(none)" : snapshot.ErrorMessage;

        LastUpdateLabel.Text = $"Last update: {lastUpdateText} | {locationText}";
        LastErrorLabel.Text = $"Last error: {errorText}";

        if (_isTracking && snapshot.LastUpdateUtc == null)
        {
            StatusLabel.Text = "Status: tracking started (waiting for update)";
        }
    }
}