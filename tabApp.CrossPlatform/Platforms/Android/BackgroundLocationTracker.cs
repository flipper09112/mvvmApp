using Android.Content;
using tabApp.CrossPlatform.Services.Interfaces.Location;
using tabApp.CrossPlatform.Services.Location;

namespace tabApp.CrossPlatform;

/// <summary>
/// Android implementation of IBackgroundLocationTracker.
///
/// Uses <see cref="LocationForegroundService"/> to deliver continuous 1-second GPS updates —
/// matching the original ForegroundService.cs behaviour (UPDATE_INTERVAL = 1 000 ms).
///
/// WorkManager (15-min periodic) is intentionally NOT used here because the OS enforces a
/// hard 15-minute minimum that cannot meet the 1-second proximity-alert requirement.
/// WorkManager may still be used for low-frequency sync tasks in other parts of the app.
/// </summary>
public class BackgroundLocationTracker : IBackgroundLocationTracker
{
    /// <summary>
    /// Continuous tracking at 1-second intervals — matches ForegroundService.cs UPDATE_INTERVAL.
    /// </summary>
    public TimeSpan UpdateInterval => TimeSpan.FromSeconds(1);

    public Task<bool> StartAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var context = Android.App.Application.Context;
            var intent  = new Intent(context, typeof(LocationForegroundService));
            context.StartForegroundService(intent);
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            BackgroundLocationStatus.SetError($"StartAsync failed: {ex.Message}");
            return Task.FromResult(false);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var context = Android.App.Application.Context;
            var intent  = new Intent(context, typeof(LocationForegroundService));
            context.StopService(intent);
        }
        catch (Exception ex)
        {
            BackgroundLocationStatus.SetError($"StopAsync failed: {ex.Message}");
        }

        return Task.CompletedTask;
    }
}
