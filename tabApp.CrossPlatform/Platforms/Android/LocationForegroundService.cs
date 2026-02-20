using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using AndroidX.Core.App;
using tabApp.CrossPlatform.Services.Location;

namespace tabApp.CrossPlatform;

/// <summary>
/// Android Foreground Service that delivers continuous location updates at ~1-second intervals.
///
/// This is the MAUI replacement for the legacy ForegroundService.cs. WorkManager alone cannot
/// meet the 1-second update requirement because the Android OS enforces a hard 15-minute minimum
/// for periodic WorkManager tasks. A Foreground Service with a persistent notification is the
/// only Android mechanism that supports near-real-time background GPS polling.
///
/// Requirements satisfied:
///   - UPDATE_INTERVAL = 1 000 ms (matches ForegroundService.cs line ~60)
///   - Continuous background GPS via MAUI Geolocation
///   - Android 8.0+ notification channel + ongoing notification (ForegroundServiceType.Location)
///   - Proper Start/Stop lifecycle (fixes missing OnDestroy() memory leak in legacy service)
///
/// Usage:
///   Start: context.StartForegroundService(new Intent(context, typeof(LocationForegroundService)));
///   Stop:  context.StopService(new Intent(context, typeof(LocationForegroundService)));
/// </summary>
[Service(
    Exported = false,
    ForegroundServiceType = Android.Content.PM.ForegroundService.TypeLocation)]
public class LocationForegroundService : Service
{
    // ── Constants ──────────────────────────────────────────────────────────────
    private const int NotificationId = 1001;
    private const string ChannelId   = "location_tracking_channel";
    private const int UpdateIntervalMs = 1_000; // 1 second — matches legacy ForegroundService.cs

    // ── State ──────────────────────────────────────────────────────────────────
    private CancellationTokenSource? _cts;

    // ── IBinder (not a bound service) ─────────────────────────────────────────
    public override IBinder? OnBind(Intent? intent) => null;

    // ── Lifecycle ─────────────────────────────────────────────────────────────

    public override void OnCreate()
    {
        base.OnCreate();
        CreateNotificationChannel();
    }

    [return: GeneratedEnum]
    public override StartCommandResult OnStartCommand(Intent? intent, [GeneratedEnum] StartCommandFlags flags, int startId)
    {
        StartForeground(NotificationId, BuildNotification());
        BeginTracking();
        return StartCommandResult.Sticky; // Restart if killed by OS
    }

    public override void OnDestroy()
    {
        // CRITICAL FIX: legacy ForegroundService.cs was missing OnDestroy → memory leak + battery drain.
        // This implementation always cancels cleanly.
        StopTracking();
        base.OnDestroy();
    }

    // ── Tracking loop ─────────────────────────────────────────────────────────

    private void BeginTracking()
    {
        StopTracking(); // idempotent — cancel any running loop first
        _cts = new CancellationTokenSource();
        _ = Task.Run(() => TrackingLoopAsync(_cts.Token), _cts.Token);
    }

    private void StopTracking()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
    }

    private async Task TrackingLoopAsync(CancellationToken cancellationToken)
    {
        var request = new GeolocationRequest(
            GeolocationAccuracy.Best,
            TimeSpan.FromMilliseconds(UpdateIntervalMs));

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var location = await Geolocation.Default.GetLocationAsync(request, cancellationToken);

                if (location != null)
                {
                    BackgroundLocationStatus.Update(
                        location.Latitude,
                        location.Longitude,
                        DateTime.UtcNow);
                }
                else
                {
                    BackgroundLocationStatus.SetError("ForegroundService: location null – GPS unavailable or denied");
                }
            }
            catch (System.OperationCanceledException)
            {
                // Normal shutdown — exit cleanly
                break;
            }
            catch (PermissionException)
            {
                BackgroundLocationStatus.SetError("ForegroundService: location permission denied");
                break; // Can't recover without user action
            }
            catch (Exception ex)
            {
                BackgroundLocationStatus.SetError($"ForegroundService: {ex.Message}");
            }

            // Wait for next cycle; cancel is checked at the top of the loop
            try
            {
                await Task.Delay(UpdateIntervalMs, cancellationToken);
            }
            catch (System.OperationCanceledException)
            {
                break;
            }
        }
    }

    // ── Notification helpers ───────────────────────────────────────────────────

    private void CreateNotificationChannel()
    {
        if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            return;

        var channel = new NotificationChannel(
            ChannelId,
            "Location Tracking",
            NotificationImportance.Low) // Low = no sound, minimal intrusion
        {
            Description = "Active while location tracking is running"
        };

        var manager = (NotificationManager?)GetSystemService(NotificationService);
        manager?.CreateNotificationChannel(channel);
    }

    private Notification BuildNotification()
    {
        return new NotificationCompat.Builder(this, ChannelId)
            .SetContentTitle("Location Tracking Active")
            .SetContentText("Monitoring your location for delivery proximity alerts")
            .SetSmallIcon(Android.Resource.Drawable.IcDialogMap)
            .SetOngoing(true)       // Sticky — cannot be dismissed by user
            .SetForegroundServiceBehavior(NotificationCompat.ForegroundServiceImmediate)
            .Build();
    }
}






