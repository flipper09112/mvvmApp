namespace tabApp.CrossPlatform.Services.Location;

/// <summary>
/// Thread-safe in-memory store shared between the background worker/tracker and the UI.
/// </summary>
public static class BackgroundLocationStatus
{
    private static readonly object Lock = new();
    private static LocationSnapshot _snapshot = new();

    public static void Update(double latitude, double longitude, DateTime utcNow, string? errorMessage = null)
    {
        lock (Lock)
        {
            _snapshot = new LocationSnapshot
            {
                LastUpdateUtc = utcNow,
                LocationText = $"Lat: {latitude:F6}, Lon: {longitude:F6}",
                ErrorMessage = errorMessage
            };
        }
    }

    public static void SetError(string errorMessage)
    {
        lock (Lock)
        {
            _snapshot = new LocationSnapshot
            {
                LastUpdateUtc = _snapshot.LastUpdateUtc,
                LocationText = _snapshot.LocationText,
                ErrorMessage = errorMessage
            };
        }
    }

    public static LocationSnapshot GetSnapshot()
    {
        lock (Lock)
        {
            return _snapshot;
        }
    }
}

public sealed class LocationSnapshot
{
    public DateTime? LastUpdateUtc { get; init; }
    public string? LocationText { get; init; }
    public string? ErrorMessage { get; init; }
}



