using Android.Content;
using AndroidX.Work;
using Microsoft.Maui.Devices.Sensors;
using tabApp.CrossPlatform.Services.Location;
using MauiLocation = Microsoft.Maui.Devices.Sensors.Location;

namespace tabApp.CrossPlatform;

/// <summary>
/// WorkManager Worker that runs periodically in the background to capture the device location.
/// Minimum scheduling interval on Android is 15 minutes (OS-enforced).
/// </summary>
public class LocationUpdateWorker : Worker
{
    private const string TagName = "location_updates";

    public LocationUpdateWorker(Context context, WorkerParameters workerParams)
        : base(context, workerParams) { }

    public override Result DoWork()
    {
        try
        {
            // Synchronously wait for location – acceptable inside a Worker's DoWork
            var location = GetCurrentLocationSync();

            if (location != null)
            {
                BackgroundLocationStatus.Update(
                    location.Latitude,
                    location.Longitude,
                    DateTime.UtcNow);
            }
            else
            {
                BackgroundLocationStatus.SetError("Worker: location null – GPS unavailable or denied");
            }

            return Result.InvokeSuccess();
        }
        catch (Exception ex)
        {
            BackgroundLocationStatus.SetError($"Worker exception: {ex.Message}");
            return Result.InvokeRetry();
        }
    }

    private MauiLocation? GetCurrentLocationSync()
    {
        var task = Task.Run(async () =>
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            return await Geolocation.Default.GetLocationAsync(request);
        });

        task.Wait(TimeSpan.FromSeconds(15));
        return task.IsCompletedSuccessfully ? task.Result : null;
    }

    /// <summary>
    /// Enqueues a unique periodic work request (15-minute interval).
    /// Safe to call multiple times – uses ExistingPeriodicWorkPolicy.Keep.
    /// </summary>
    public static void Enqueue()
    {
        var constraints = new Constraints.Builder()
            .SetRequiredNetworkType(NetworkType.NotRequired)
            .Build();

        var request = new PeriodicWorkRequest.Builder(
                typeof(LocationUpdateWorker),
                TimeSpan.FromMinutes(15))
            .SetConstraints(constraints)
            .AddTag(TagName)
            .Build();

        WorkManager.GetInstance(Android.App.Application.Context)
            .EnqueueUniquePeriodicWork(
                TagName,
                ExistingPeriodicWorkPolicy.Keep,
                request);
    }

    /// <summary>
    /// Cancels the scheduled periodic work.
    /// </summary>
    public static void Cancel()
    {
        WorkManager.GetInstance(Android.App.Application.Context)
            .CancelUniqueWork(TagName);
    }
}








